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

using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.FrameworkServices.Tests;
using PlatformAgileFramework.MultiProcessing.AsyncControl;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FileAndIO.Tests{
	/// <summary>
	/// These tests test our system for abstracting file access and manipulation. Unfortunately
	/// these mechanisms do not work the same across all ECMA implementations, so we have to
	/// have verification tests for even these basic things.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04may2019 </date>
	/// <description>
	/// New. Had to put in some tests to verify fixes to things that didn't work across platforms like
	/// we assumed.
	/// </description>
	/// </contribution>
	/// </history>
	[TestFixture]
	public class FileAccessTests : BasicServiceManagerTestFixtureBase
	{
		/// <summary>
		/// This is the file that is created fresh when the tests start with a clean
		/// empty directory.
		/// </summary>
		public static string s_FreshFile  = "FreshFile.txt";

		/// <summary>
		/// Directory we create/write files in.
		/// </summary>
		public static string s_FileDirectory;
		/// <summary>
		/// For independently manipulating storage.
		/// </summary>
		public IPAFStorageService m_StorageService;


		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			m_StorageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			s_FileDirectory =
				"Documents:" + DS + "FileTestDirectory";
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
		}
		/// <summary>
		/// This test ensures that our patch for "REPLACE" file access works
		/// and the "OPENORCREATE" works.
		/// </summary>
		[Test]
		public void CallWriterOnMultipleThreads()
		{
			// Open the file and write into it. The default should create it.
			using (var file = m_StorageService.PAFOpenFile(s_FileDirectory + DS + s_FreshFile))
			{
				file.PAFWriteString("1");
			}

			string fileContents;
			// Open the file and read from it. The default should open existing.
			using (var file = m_StorageService.PAFOpenFile(s_FileDirectory + DS + s_FreshFile))
			{
				fileContents = file.PAFReadString();
			}

			var contentsCorrect = fileContents == "1";
			Assert.IsTrue(contentsCorrect, "Contents == 1");
			// Open the file and write a 2 into it after rewind. The default
			// should open existing.
			using (var file = m_StorageService.PAFOpenFile(s_FileDirectory + DS + s_FreshFile))
			{
				file.PAFPosition = 0;
				file.PAFWriteString("2");
			}

			// Open the file and check for the 2. The default
			// should open existing.
			using (var file = m_StorageService.PAFOpenFile(s_FileDirectory + DS + s_FreshFile))
			{
				fileContents = file.PAFReadString();
			}

			contentsCorrect = fileContents[0] == '2';
			Assert.IsTrue(contentsCorrect, "Contents == 2");

			//  Replace the file and write a 3 into it.
			using (var file = m_StorageService.PAFOpenFile(s_FileDirectory + DS + s_FreshFile,
				PAFFileAccessMode.REPLACE))
			{
				file.PAFWriteString("3");
			}

			// Open the file and check for the 3. The default
			// should open existing.
			using (var file = m_StorageService.PAFOpenFile(s_FileDirectory + DS + s_FreshFile))
			{
				fileContents = file.PAFReadString();
			}

			contentsCorrect = fileContents == "3";
			Assert.IsTrue(contentsCorrect, "Contents == 3");
		}
	}
}