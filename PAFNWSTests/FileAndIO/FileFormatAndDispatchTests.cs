using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
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
	/// These tests check the ability to format and dispatch a file, using
	/// <see cref="IPAFFormattingFileDispatcher"/>. A variety of other capabilities
	/// in <see cref="IPAFStorageService"/> are tested along the way.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17feb2019 </date>
	/// <description>
	/// New for <see cref="IPAFFormattingFileDispatcher"/>.
	/// </description>
	/// </contribution>
	/// </history>
	[TestFixture]
	public class FileFormatAndDispatchTests : BasicServiceManagerTestFixtureBase
	{
		/// <summary>
		/// Name of a file.
		/// </summary>
		public static string s_FileName1 = "FileName1";

		/// <summary>
		/// Name of a file.
		/// </summary>
		public static string s_FileName2 = "FileName2";

		/// <summary>
		/// The directory that we dispatch files into.
		/// </summary>
		public static string s_DispatchDirectory;

		/// <summary>
		/// The directory that we create files to be dispatched in.
		/// </summary>
		public static string s_SourceDirectory;

		/// <summary>
		/// Some text for the tests.
		/// </summary>
		public const string ORIGINAL_TEXT_IN_FILES = "thiiis is a document";

		/// <summary>
		/// This is the text after transformation.
		/// </summary>
		public const string FORMATTED_TEXT_IN_FILES = "thi.i.i.s i.s a document";

		/// <summary>
		/// This one calls base and additionally sets up our test directories.
		/// </summary>
		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			s_DispatchDirectory = "Documents:" + DS + "DispatchDirectory";
			s_SourceDirectory = "Documents:" + DS + "SourceDirectory";
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
		/// This method writes some files into a "source" directory, then runs
		/// the <see cref="PAFFormattingFileDispatcher"/> class to reformat them with
		/// a simple test formatter, then writes then out to another directory,
		/// according to the usage model of <see cref="IPAFFormattingFileDispatcher"/>.
		/// </summary>
		[Test]
		public void TestFormatAndDispatchFile()
		{
			var sep = PlatformUtils.GetDirectorySeparatorChar().ToString();
			// We want to push in the "Documents" mapping if it is not already there.
			// We use this specific directory for most tests, since sometimes it needs
			// to be pre-created off the C: drive on some machines.
			var internalDict = SymbolicDirectoryMappingDictionary.s_DirectoryMappingDictionary;
			// If this is pre-loaded (e.g. from a mapping file), we don't touch it. 
			internalDict.EnsureEntry("Documents", PlatformUtils.s_C_DriveMapping + DS + "Documents");

			var fileService = PAFServices.Manager.GetTypedService<IPAFStorageService>();

			// Clean out and re-establish our directories each time through.
			fileService.PAFEmptyAndDeleteDirectory(s_SourceDirectory);
			fileService.PAFEnsureDirectoryExists(s_SourceDirectory, false);
			fileService.PAFEmptyAndDeleteDirectory(s_DispatchDirectory);
			fileService.PAFEnsureDirectoryExists(s_DispatchDirectory, false);

			var fileNames = new Collection<string>
				{s_SourceDirectory + sep + s_FileName1, s_SourceDirectory + sep + s_FileName2};

			// Write a couple of files.
			using (var stream = fileService.PAFOpenFile(fileNames[0]))
			{
				stream.PAFWriteString(ORIGINAL_TEXT_IN_FILES);
			}
			using (var stream = fileService.PAFOpenFile(fileNames[1]))
			{
				stream.PAFWriteString(ORIGINAL_TEXT_IN_FILES);
			}

			// Always handle by the interface.
			IPAFFormattingFileDispatcher formatAndDispatch
				= new PAFFormattingFileDispatcher(s_DispatchDirectory,SimpleFormatter);

			// Perform the operation.
			formatAndDispatch.DispatchFiles(fileNames);

			string contents;
			// Read back just one file.
			using (var stream = fileService.PAFOpenFile(s_DispatchDirectory + sep + s_FileName1))
			{
				contents = stream.PAFReadString();
			}

			var isCorrectLyDispatchedString
				= contents.Equals(FORMATTED_TEXT_IN_FILES, StringComparison.Ordinal);

			Assert.IsTrue(isCorrectLyDispatchedString, "CorrectlyDispatchedString");
		}

		#region Helpers
		/// <summary>
		/// Simple test formatter that just puts a dot after every <c>i</c>.
		/// </summary>
		/// <param name="stringToFormat">String to process.</param>
		/// <returns>String with dots inserted.</returns>
		public static string SimpleFormatter(string stringToFormat)
		{
			return stringToFormat.Replace("i", "i.");
		}
		#endregion // Helpers
	}
}

