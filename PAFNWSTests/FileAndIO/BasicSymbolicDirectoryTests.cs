using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.Collections.Dictionaries;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FileAndIO.SymbolicDirectories;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.FrameworkServices.Tests;
using PlatformAgileFramework.Platform;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FileAndIO.Tests
{
	/// <summary>
	/// These tests check the loading of the symbolic directory mapping dictionary
	/// and it's use.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31dec2018 </date>
	/// <description>
	/// Documented. Added calls to <see cref="DictionaryExtensionMethods.EnsureEntry{T,U}"/>.
	/// </description>
	/// </contribution>
	/// </history>
	[TestFixture]
	public class BasicSymbolicDirectoryTests : BasicServiceManagerTestFixtureBase
	{
		/// <summary>
		/// Test path.
		/// </summary>
		public static string s_DocumentInDocumentsFilePath = "Documents:document.txt";

		/// <summary>
		/// Recursive test path.
		/// </summary>
		public static string s_DirectoryInDocumentsDirectory;

		/// <summary>
		/// Recursive test path.
		/// </summary>
		public static string s_DirectoryInDirectoryInDocumentsDirectory;

		/// <summary>
		/// Another test path.
		/// </summary>
		public static string s_SpecialFilesTestDirectory;

		/// <summary>
		/// Some text for the tests.
		/// </summary>
		public const string TEXT_IN_DOCUMENT_FILE = "this is a document";

		/// <summary>
		/// This one calls base and additionally sets up our test directories and files.
		/// </summary>
		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			s_DirectoryInDocumentsDirectory = "Documents:" + DS + "FirstLevelDir";
			s_DirectoryInDirectoryInDocumentsDirectory = "Documents:" + DS + "FirstLevelDir" + DS + "SecondLevelDir";
			s_SpecialFilesTestDirectory = PlatformUtils.s_C_DriveMapping + DS + "specialfiles";
		}

		/// <summary>
		/// Original NUnit attribute name.
		/// </summary>
		public override void TestFixtureTearDown()
		{
			var fileService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			fileService.PAFEmptyAndDeleteDirectory("Documents:");
			fileService.PAFEnsureDirectoryExists("Documents:");
			Assert.IsTrue(fileService.PAFDirectoryExists("Documents:"));
		}
		/// <summary>
		/// Gets around the problem of NUnit being crippled without test fixture setups.
		/// </summary>
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
		}

		/// <summary>
		/// This one should be fired first (remember NUnit alphabetizes test order).
		/// This reads in entries from a file containing directory symbol mappings.
		/// This test shows a whole bunch of things and should be broken up to follow
		/// best practices. We show what symbolic directory mappings are and how they
		/// work. We assume that a PLATFORM-INDEPENDENT mapping file is available
		/// for load, which contains some mapped directories. We make sure that the
		/// mapping system works correctly. We show how to dynamically push in
		/// symbolically directory mappings.
		/// </summary>
		/// <remarks>
		/// This test expects a test symbolic directory mapping file to contain:
		/// <c>"SpecialFiles" -> "c:/specialfiles"</c>
		/// <c>SpecialFilesInDocuments" -> "Documents:/specialfiles" </c>
		/// </remarks>
		[Test]
		public void A_TestToManipulateDictionary()
		{
			// We want to push in the "Documents" mapping if it is not already there.
			var internalDict = SymbolicDirectoryMappingDictionary.s_DirectoryMappingDictionary;
			// We don't overwrite it if it's already been loaded.
			internalDict.EnsureEntry("Documents", PlatformUtils.s_C_DriveMapping + DS + "Documents");

			var documentsMapping
				= SymbolicDirectoryMappingDictionary.GetStaticMappingInternal("Documents");

			var recursiveSymbolResult
				= PAFFileUtils.NormalizeFilePath(PAFFileUtils.WalkDirectorySymbol("SpecialFilesInDocuments"));
			var correctResult = documentsMapping + DS + "specialfiles";

			Assert.IsTrue(recursiveSymbolResult.Equals(correctResult, StringComparison.Ordinal));

			var normalizedResult
				= PAFFileUtils.NormalizeFilePath(recursiveSymbolResult);

			var correctNormalizedResult = PlatformUtils.s_C_DriveMapping + DS
				// ReSharper disable once StringLiteralTypo
				+ "Documents" + DS + "specialfiles";

			Assert.IsTrue(normalizedResult.Equals(correctNormalizedResult, StringComparison.Ordinal));
		}

		/// <summary>
		/// This test verifies that we can delete directories recursively on a symbolic path.
		/// </summary>
		[Test]
		public void B_TestToRecursivelyDeleteDirectories()
		{
			// SOB ReSharper doesn't alphabetize tests in VS2017,
			// so we have to put this here.
			var fileService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			EnsureDocumentsDirExists(fileService);


			if (!fileService.PAFDirectoryExists(s_DirectoryInDocumentsDirectory))
				fileService.PAFCreateDirectory(s_DirectoryInDocumentsDirectory);
			Assert.IsTrue(fileService.PAFDirectoryExists(s_DirectoryInDocumentsDirectory));
			if (!fileService.PAFDirectoryExists(s_DirectoryInDirectoryInDocumentsDirectory))
				fileService.PAFCreateDirectory(s_DirectoryInDirectoryInDocumentsDirectory);
			Assert.IsTrue(fileService.PAFDirectoryExists(s_DirectoryInDirectoryInDocumentsDirectory));
			if (!fileService.PAFFileExists(s_DirectoryInDocumentsDirectory + DS + "Test.txt"))
			{
				// Create and close
				var file = fileService.PAFCreateFile(s_DirectoryInDocumentsDirectory + DS + "Test.txt");
				file.Dispose();
			}
			Assert.IsTrue(fileService.PAFFileExists(s_DirectoryInDocumentsDirectory + DS + "Test.txt"));
			if (!fileService.PAFFileExists(s_DirectoryInDirectoryInDocumentsDirectory + DS + "Test.txt"))
			{
				// Create and close
				var file = fileService.PAFCreateFile(s_DirectoryInDirectoryInDocumentsDirectory + DS + "Test.txt");
				file.Dispose();
			}
			Assert.IsTrue(fileService.PAFFileExists(s_DirectoryInDirectoryInDocumentsDirectory + DS + "Test.txt"));

			// Now blow everything away.
			fileService.PAFEmptyDirectoryOfDirectories("Documents:");

			var subdirectoriesInDocuments = fileService.PAFGetDirectoryNames("Documents:");

			Assert.IsTrue((subdirectoriesInDocuments == null) || (subdirectoriesInDocuments.ToArray().Length == 0));
		}

		/// <summary>
		/// This test uses our symbolic "C:" drive mapping and the "SpecialFiles"
		/// mapping.
		/// </summary>
		[Test]
		public void C_TestToTestSymbolicDirectoryAccess()
		{
			var fileService = PAFServices.Manager.GetTypedService<IPAFStorageService>();

			/* Delete if here, and recreate. */
			var specialDirExists = fileService.PAFDirectoryExists(s_SpecialFilesTestDirectory);

			if (specialDirExists) fileService.PAFDeleteDirectory(s_SpecialFilesTestDirectory);

			Assert.IsTrue(!fileService.PAFDirectoryExists(s_SpecialFilesTestDirectory));

			EnsureSpecialDirExists(fileService);

			fileService.PAFEmptyAndDeleteDirectory(s_SpecialFilesTestDirectory);
			// Always some timing bullshit with Windows......
			Task.Delay(100).Wait();
			Assert.IsTrue(!fileService.PAFDirectoryExists(s_SpecialFilesTestDirectory));

			/* Same thing with a symbolic directory. */
			var documentsMapping
				= SymbolicDirectoryMappingDictionary.GetStaticMappingInternal("Documents");
			var documentsDirExists = fileService.PAFDirectoryExists(documentsMapping);

			if (documentsDirExists) fileService.PAFEmptyAndDeleteDirectory(documentsMapping);

			// Always some timing bullshit with Windows......
			Task.Delay(100).Wait();
			Assert.IsTrue(!fileService.PAFDirectoryExists(documentsMapping));

			EnsureDocumentsDirExists(fileService);

			fileService.PAFEmptyAndDeleteDirectory(documentsMapping);
			Assert.IsTrue(!fileService.PAFDirectoryExists(documentsMapping));

			/* Again with a symbolic dir in a symbolic dir - tests path walking. */
			var specialFilesInDocumentsDirMapping
				= PAFFileUtils.NormalizeFilePath(PAFFileUtils.WalkDirectorySymbol("SpecialFilesInDocuments"));
			var specialFilesInDocumentsDirExists = fileService.PAFDirectoryExists(specialFilesInDocumentsDirMapping);

			if (specialFilesInDocumentsDirExists) fileService.PAFEmptyAndDeleteDirectory(specialFilesInDocumentsDirMapping);

			Assert.IsTrue(!fileService.PAFDirectoryExists(specialFilesInDocumentsDirMapping));

			EnsureSpecialFilesDocumentsDirExists(fileService);

			fileService.PAFEmptyAndDeleteDirectory(specialFilesInDocumentsDirMapping);
			Assert.IsTrue(!fileService.PAFDirectoryExists(specialFilesInDocumentsDirMapping));

		}

		/// <summary>
		/// This test verifies that we can create and delete <see cref="s_DocumentInDocumentsFilePath"/>
		/// and correctly write <see cref="TEXT_IN_DOCUMENT_FILE"/> into it.
		/// </summary>
		[Test]
		public void D_TestToWriteToSymbolicDirectoryFiles()
		{
			var fileService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			EnsureDocumentsDirExists(fileService);


			if (fileService.PAFFileExists(s_DocumentInDocumentsFilePath))
				fileService.PAFDeleteFile(s_DocumentInDocumentsFilePath);
			Assert.IsTrue(!fileService.PAFFileExists(s_DocumentInDocumentsFilePath));

			EnsureDocumentInDocuments(fileService);

			using (var stream = fileService.PAFOpenFile(s_DocumentInDocumentsFilePath))
			{
				stream.PAFWriteString(TEXT_IN_DOCUMENT_FILE);
			}

			string textRead;

			using (var stream = fileService.PAFOpenFile(s_DocumentInDocumentsFilePath))
			{
				textRead = stream.PAFReadString();
			}

			Assert.IsTrue(string.CompareOrdinal(textRead, TEXT_IN_DOCUMENT_FILE) == 0);

			//  Clean up.
			fileService.PAFDeleteFile(s_DocumentInDocumentsFilePath);
			Assert.IsTrue(!fileService.PAFFileExists(s_DocumentInDocumentsFilePath));
		}

		/// <summary>
		/// This test verifies that we can create directories recursively on a symbolic path.
		/// </summary>
		[Test]
		public void E_TestToRecursivelyCreateDirectories()
		{
			var fileService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			EnsureDocumentsDirExists(fileService);


			if (fileService.PAFDirectoryExists(s_DirectoryInDocumentsDirectory))
				fileService.PAFEmptyAndDeleteDirectory(s_DirectoryInDocumentsDirectory);
			Assert.IsTrue(!fileService.PAFDirectoryExists(s_DirectoryInDocumentsDirectory));

			// Creating one level down without recursion should work.
			fileService.PAFEnsureDirectoryExists(s_DirectoryInDocumentsDirectory, false);
			Assert.IsTrue(fileService.PAFDirectoryExists(s_DirectoryInDocumentsDirectory));

			// Creating two levels down without recursion should not work.
			fileService.PAFEmptyAndDeleteDirectory(s_DirectoryInDocumentsDirectory);
			Exception caughtException = null;
			try
			{
				fileService.PAFEnsureDirectoryExists(s_DirectoryInDirectoryInDocumentsDirectory, false);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}

			Assert.IsTrue(caughtException != null);

			// Creating two levels down with recursion should work.
			if (fileService.PAFDirectoryExists(s_DirectoryInDocumentsDirectory))
				fileService.PAFEmptyAndDeleteDirectory(s_DirectoryInDocumentsDirectory);
			fileService.PAFEnsureDirectoryExists(s_DirectoryInDirectoryInDocumentsDirectory);
			Assert.IsTrue(fileService.PAFDirectoryExists(s_DirectoryInDirectoryInDocumentsDirectory));

		}


		#region Helpers
		/// <summary>
		/// This is a helper that just ensures that out specialfiles test directory
		/// is there.
		/// </summary>
		/// <param name="fileService">instance of storage service.</param>
		/// <remarks>
		/// Exhibits just a tiny bit of test fixture design strategy - this
		/// is called trivially in early tests, then for a purpose in later tests.
		/// </remarks>
		public virtual void EnsureSpecialDirExists([NotNull] IPAFStorageService fileService)
		{
			var specialDirExists = fileService.PAFDirectoryExists(@"c:\specialfiles");

			if (specialDirExists) return;

			fileService.PAFCreateDirectory(@"c:\specialfiles");
			Assert.IsTrue(fileService.PAFDirectoryExists(@"c:\specialfiles"));
		}

		/// <summary>
		/// This is a helper that just ensures that our documents test directory
		/// is there.
		/// </summary>
		/// <param name="fileService">instance of storage service.</param>
		public virtual void EnsureDocumentsDirExists(IPAFStorageService fileService)
		{
			var documentsMapping
				= SymbolicDirectoryMappingDictionary.GetStaticMappingInternal("Documents");
			Assert.IsTrue(!string.IsNullOrEmpty(documentsMapping), "No documents mapping.");

			var documentsDirExists = fileService.PAFDirectoryExists(documentsMapping);

			if (documentsDirExists) return;

			fileService.PAFCreateDirectory(documentsMapping);
			Assert.IsTrue(fileService.PAFDirectoryExists(documentsMapping));
		}
		/// <summary>
		/// This is a helper that just ensures that our specialdir within the
		/// documents test directory is there.
		/// </summary>
		/// <param name="fileService">instance of storage service.</param>
		public virtual void EnsureSpecialFilesDocumentsDirExists(IPAFStorageService fileService)
		{
			var specialFilesInDocumentsDirMapping
				= PAFFileUtils.NormalizeFilePath(PAFFileUtils.WalkDirectorySymbol("SpecialFilesInDocuments"));
			var specialFilesInDocumentsDirExists = fileService.PAFDirectoryExists(specialFilesInDocumentsDirMapping);

			if (specialFilesInDocumentsDirExists) return;

			fileService.PAFCreateDirectory(specialFilesInDocumentsDirMapping);
			Assert.IsTrue(fileService.PAFDirectoryExists(specialFilesInDocumentsDirMapping));
		}
		/// <summary>
		/// This is a helper that just ensures that our document within the
		/// documents test directory is there.
		/// </summary>
		/// <param name="fileService">instance of storage service.</param>
		public virtual void EnsureDocumentInDocuments(IPAFStorageService fileService)
		{
			if (fileService.PAFFileExists(s_DocumentInDocumentsFilePath)) return;
			var stream = fileService.PAFCreateFile(s_DocumentInDocumentsFilePath);
			stream.Dispose();
			Assert.IsTrue(fileService.PAFFileExists(s_DocumentInDocumentsFilePath));
		}
		#endregion Helpers
	}
}

