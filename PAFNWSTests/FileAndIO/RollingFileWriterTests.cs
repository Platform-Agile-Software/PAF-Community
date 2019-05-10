//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2018 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System.Linq;
using System.Text;
using NUnit.Framework;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.FrameworkServices.Tests;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FileAndIO.Tests{
	/// <summary>
	/// These tests test the behavior of the rolling file writer under non-concurrent
	/// conditions.
	/// </summary>
	[TestFixture]
	public class RollingFileWriterTests : BasicServiceManagerTestFixtureBase
	{
		/// <summary>
		/// Start dispatching files at this point.
		/// </summary>
		public static int s_MaxNumFiles = 10;
		/// <summary>
		/// Open new file at this point.
		/// </summary>
		public static int s_MaxFileSizeInBytes = 1000;
		/// <summary>
		/// Default is to check every time we write.
		/// </summary>
		public static int s_SizeCheckFrequency = 1;

		/// <summary>
		/// Text to write. <c>1</c> at the beginning with <c>*</c>s following.
		/// </summary>
		public static readonly string s_OneKBOfText;

		/// <summary>
		/// Directory we create/write files in .
		/// </summary>
		public static string s_FileDirectory;
		/// <summary>
		/// "dispatch" file directory.
		/// </summary>
		public static string s_DispatchDirectory;
		/// <summary>
		/// For independently manipulating storage.
		/// </summary>
		public IPAFStorageService m_StorageService;

		/// <summary>
		/// Dispatcher for just moving files.
		/// </summary>
		public IPAFFormattingFileDispatcher m_FileMoveDispatcher;

		/// <summary>
		/// Builds our 1 kb of text.
		/// </summary>
		static RollingFileWriterTests()
		{
			var sb = new StringBuilder("1", 1000);
			sb.Append('*', 999);

			s_OneKBOfText = sb.ToString();

		}

		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			m_StorageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			s_FileDirectory =
				"Documents:" + DS + "RollingFileWriterDirectory";
			s_DispatchDirectory =
				"Documents:" + DS + "RollingFileWriterDispatchDirectory";
			m_FileMoveDispatcher
				= new PAFFormattingFileDispatcher(s_DispatchDirectory);
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			// Kill directory.
			m_StorageService.PAFEmptyAndDeleteDirectory(s_FileDirectory);
			// Re-create it.
			m_StorageService.PAFEnsureDirectoryExists(s_FileDirectory);
			// Empty us out first.
			m_StorageService.PAFEmptyDirectoryOfFiles(s_FileDirectory);
			// Kill directory.
			m_StorageService.PAFEmptyAndDeleteDirectory(s_DispatchDirectory);
			// Re-create it.
			m_StorageService.PAFEnsureDirectoryExists(s_DispatchDirectory);
			// Empty us out first.
			m_StorageService.PAFEmptyDirectoryOfFiles(s_DispatchDirectory);
		}
		/// <summary>
		/// Writes enough data to cause the logger to create one new file
		/// while writing, in addition to the initial file.
		/// Result will be two dated files in directory. This tests
		/// limit on file size.
		/// </summary>
		/// <remarks>
		/// Reworked a bit to test file loading more precisely for a client.
		/// </remarks>
		[Test]
		public void CreateNewFile()
		{
			// Set up to check file size every time we write.
			var rollingWriter = new RollingFileWriter(s_FileDirectory, "XXX.log", s_MaxFileSizeInBytes,
				s_MaxNumFiles, s_SizeCheckFrequency);

			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);

			var files = m_StorageService.PAFGetFileNames(s_FileDirectory);

			// ReSharper disable once PossibleMultipleEnumeration
			//// There's a reason, ReSharper.
			var numFiles = files.SafeCount();
			Assert.IsTrue(numFiles == 2, "numFiles == 2");

			// ReSharper disable once PossibleMultipleEnumeration
			//// There's a reason, ReSharper.
			var fileList = files.ToList();

			var file1Size = m_StorageService.PAFFileSize(fileList[0]);
			Assert.IsTrue(file1Size == 1000, "file1Size = 1000");

			var file2Size = m_StorageService.PAFFileSize(fileList[1]);
			Assert.IsTrue(file2Size == 1000, "file2Size = 1000");

		}

		/// <summary>
		/// Writes enough data to cause the writer to create 12 log files
		/// while writing. This will trigger a deletion of two files.
		/// Result will be ten dated files in directory. This tests
		/// for files being deleted properly.
		/// </summary>
		[Test]
		public void RollFiles()
		{
			// Start deleting at 10.
			var maxNumFiles = 10;

			// Set up to check file size every time we write.
			var rollingWriter = new RollingFileWriter(s_FileDirectory, "XXX.log", s_MaxFileSizeInBytes,
				maxNumFiles, s_SizeCheckFrequency);

			// Write what should be 12 files.
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);


			// Should have had 2 files deleted.
			var numFiles = m_StorageService.PAFGetFileNames(s_FileDirectory).Count();
			Assert.IsTrue(numFiles == 10, "numFiles == 10");
		}

		/// <summary>
		/// We construct and run the writer 3 times, which should give 6
		/// versioned files in our directory. We write twice with each instatnce.
		/// </summary>
		[Test]
		public void CreateVersionedFiles()
		{
			IPAFFilenameStamperAndParser fsap = new AuditingFilenameStamperAndParser("XXX");
			// Set up to check file size every time we write.
			var rollingWriter = new RollingFileWriter(s_FileDirectory, "XXX.log", s_MaxFileSizeInBytes,
				1000, s_SizeCheckFrequency, fsap, null, true);

			// Write what should be 2 files.
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);

			fsap = new AuditingFilenameStamperAndParser("XXX");
			// Set up to check file size every time we write.
			rollingWriter = new RollingFileWriter(s_FileDirectory, "XXX.log", s_MaxFileSizeInBytes,
				1000, s_SizeCheckFrequency, fsap, null, true);

			// Write what should be 2 files.
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);

			fsap = new AuditingFilenameStamperAndParser("XXX");
			// Set up to check file size every time we write.
			rollingWriter = new RollingFileWriter(s_FileDirectory, "XXX.log", s_MaxFileSizeInBytes,
				1000, s_SizeCheckFrequency, fsap, null, true);

			// Write what should be 2 files.
			rollingWriter.WriteDataEntry(s_OneKBOfText);
			rollingWriter.WriteDataEntry(s_OneKBOfText);


			// Should have 6 files.
			var numFiles = m_StorageService.PAFGetFileNames(s_FileDirectory).Count();
			Assert.IsTrue(numFiles == 6, "numFiles == 6");

			// What we really care about is that the last file has a version of 2.
			Assert.IsTrue(AuditingFilenameStamperAndParser.s_FileNames[5].Contains("(2)"));


		}

		/// <summary>
		/// Writes ONE file, then dispatches it. In
		/// this case our "dispatcher" moves the file to another directory.
		/// This is the STANDARD thing to do. Transmittal to a server or any
		/// other time consuming operation should be done asynchronously from
		/// this dispatch directory.
		/// </summary>
		/// <remarks>
		/// This method demonstrates the way to separate functionality into two
		/// non-inheriting interfaces. The dispatch functionality is generally
		/// a privileged operation and would not be exposed, say, from a
		/// service manager.
		/// </remarks>
		[Test]
		public void DispatchFile()
		{
			// Set up to check file size every time we write.
			var rollingFileWriter =
				new RollingFileWriter(
					s_FileDirectory, "XXX.log",
					s_MaxFileSizeInBytes,
					s_MaxNumFiles,
					s_SizeCheckFrequency, null, m_FileMoveDispatcher.DispatchFiles);

			var fileWriter = (IPAFFileWriter)rollingFileWriter;
			var fileDispatcher = (IPAFFileDispatcher)rollingFileWriter;

			fileWriter.WriteDataEntry("file entry");

			// We dispatch files outside the lock, since we are not in a
			// concurrent situation.
			fileDispatcher.DispatchFilesIfNeeded();

			// The file directory should be empty.
			var numFiles = m_StorageService.PAFGetFileNames(s_FileDirectory).SafeCount();
			Assert.IsTrue(numFiles == 0, "numFiles == 0");

			// The dispatch directory should have one file.
			numFiles = m_StorageService.PAFGetFileNames(s_DispatchDirectory).SafeCount();
			Assert.IsTrue(numFiles == 1, "numFiles == 1");
		}
	}
}