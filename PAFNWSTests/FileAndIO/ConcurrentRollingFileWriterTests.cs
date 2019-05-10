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
	/// These tests test the behavior of the rolling file writer under concurrent
	/// conditions. Writes and dispatches are made on timers at random times. We have
	/// use case in which files need to be dispatched when the file writing is still
	/// in progress. The typical situation is for the file writing to be stopped
	/// before dispatching.
	/// </summary>
	[TestFixture]
	public class ConcurrentRollingFileWriterTests : BasicServiceManagerTestFixtureBase
	{
		/// <summary>
		/// Millisecond mask for MAXIMUM random Write Delay - 1 second.
		/// </summary>
		public static int s_WriteDelay = 1024;
		/// <summary>
		/// Millisecond mask for MAXIMUM random Dispatch Delay - 1/4 second.
		/// We want the dispatcher to be running frequently.
		/// </summary>
		public static int s_DispatchDelay = 255;
		/// <summary>
		/// Number of threads to write simultaneously on.
		/// </summary>
		public static int s_NumThreads = 2;
		/// <summary>
		/// Start dispatching files at this point, from the rolling file writer INTERNALLY.
		/// </summary>
		public static int s_MaxNumFiles = 1000;
		/// <summary>
		/// Open new file at this point.
		/// </summary>
		public static int s_MaxFileSizeInBytes = 1000;
		/// <summary>
		/// Default is to check every eighth time we write.
		/// </summary>
		public static int s_SizeCheckFrequency = 8;

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

		public override void TestFixtureSetUp()
		{
			base.TestFixtureSetUp();
			m_StorageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			s_FileDirectory =
				"Documents:" + DS + "RollingFileWriterDirectory";
			s_DispatchDirectory =
				"Documents:" + DS + "RollingFileWriterDispatchDirectory";
			m_FileMoveDispatcher
				= new AuditingTestFileDispatcher(s_DispatchDirectory);
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
		/// This tests makes concurrent calls to write and also to dispatch.
		/// </summary>
		/// <remarks>
		/// This method demonstrates the way to separate functionality into two
		/// non-inheriting interfaces. The dispatch functionality is generally
		/// a privileged operation and would not be exposed, say, from a
		/// service manager.
		/// </remarks>
		[Test]
		public void CallWriterOnMultipleThreads()
		{
			// Instantiate the single instance of the writer that we hammer
			// with writes.
			var rollingFileWriter =
				new RollingFileWriter(
					s_FileDirectory, "XXX.log",
					s_MaxFileSizeInBytes,
					s_MaxNumFiles,
					s_SizeCheckFrequency, null, m_FileMoveDispatcher.DispatchFiles);

			var fileWriter = (IPAFFileWriter)rollingFileWriter;
			var fileDispatcher = (IPAFFileDispatcher)rollingFileWriter;

			// Build the asynchronous writers.
			var controlObjects = BuildFileWriterControlObjects(fileWriter);

			// Add the dispatchers.
			controlObjects.AddItems(BuildFileDispatcherControlObjects(fileDispatcher));

			var controller = new AsyncTaskControllerObject(controlObjects);

			var task = Task.Factory.StartNew(controller.ControlProcess, controller);

			Task.Delay(s_WriteDelay * 4).Wait();

			Assert.IsTrue(controller.ProcessHasStarted, "Controller didn't start");

			// Run long enough to get threads from writers and dispatcher to collide.
			Task.Delay(s_WriteDelay * 9).Wait();

			// Signal all tasks to stop.
			controller.ProcessShouldTerminate = true;

			task.Wait();

			Assert.IsTrue(controller.ProcessHasTerminated, "Controller didn't stop");

			// Clean out with a final dispatch. This should transfer all files, then the
			// dispatcher writes one entry into the "current" file in this directory, since
			// that's what we have it do.
			StaticFileWriterTestMethods.DispatcherDispatchFiles(fileDispatcher);

			// We just do a check to see tht there is just one file.
			var numFiles = m_StorageService.PAFGetFileNames(s_FileDirectory).SafeCount();
			Assert.IsTrue(numFiles == 1, "Directory has more than one file");
		}

		/// <summary>
		/// This method builds a list of control objects, handled just by the
		/// non-Generic <see cref="IAsyncControlObject"/> so we can build variegated
		/// control objects. These carry a <see cref="IPAFFileWriter"/> payload.
		/// </summary>
		/// <returns>
		/// The objects.
		/// </returns>
		/// <remarks>
		/// It's not a practical scenario to have more than ONE dispatcher running, but we want to
		/// make absolutely sure that the locks are working properly, so we hammer the writer
		/// with multiple dispatchers, too.
		/// </remarks>
		public IList<IAsyncControlObject> BuildFileDispatcherControlObjects(IPAFFileDispatcher fileDispatcher)
		{
			var controlObjects = new List<IAsyncControlObject>();
			for (var controlObjectIndex = 0; controlObjectIndex < s_NumThreads; controlObjectIndex++)
			{
				// Create an incomplete work control object, which will be loaded with the
				// work object.
				var writerControlObject = new AsyncGenericWorkControlObject<IPAFFileDispatcher>();

				// Each delegator is loaded with the same mean delay, but
				// will have a different seed, based on TOD.
				var delegator
					= new FileDispatcherCallDelegator(StaticFileWriterTestMethods.DispatchFiles,
						fileDispatcher, writerControlObject, s_DispatchDelay);

				var workObject
					= new AsyncGenericWorkPayloadObject<IPAFFileDispatcher>(delegator.ThreadMethodArgument,
						delegator.DelegateDelayCaller, delegator.ContravariantThreadMethod, writerControlObject);

				writerControlObject.WorkPayloadObject = workObject;

				controlObjects.Add(writerControlObject);
			}

			return controlObjects;
		}
		/// <summary>
		/// This method builds a list of control objects, handled just by the
		/// non-Generic <see cref="IAsyncControlObject"/> so we can build variegated
		/// control objects.These carry a <see cref="IPAFFileWriter"/> payload.
		/// </summary>
		/// <returns>
		/// The objects.
		/// </returns>
		public IList<IAsyncControlObject> BuildFileWriterControlObjects(IPAFFileWriter fileWriter)
		{
			var controlObjects = new List<IAsyncControlObject>();
			for (var controlObjectIndex = 0; controlObjectIndex < s_NumThreads; controlObjectIndex++)
			{
				// Create an incomplete work control object, which will be loaded with the
				// work object.
				var writerControlObject = new AsyncGenericWorkControlObject<IPAFFileWriter>();

				// Each delegator is loaded with the same mean delay, but
				// will have a different seed, based on TOD.
				var delegator
					= new FileWriterCallDelegator(StaticFileWriterTestMethods.WriteATime,
						fileWriter, writerControlObject, s_WriteDelay);

				var workObject
					= new AsyncGenericWorkPayloadObject<IPAFFileWriter>(delegator.ThreadMethodArgument,
						delegator.DelegateDelayCaller, delegator.ContravariantThreadMethod, writerControlObject);

				writerControlObject.WorkPayloadObject = workObject;

				controlObjects.Add(writerControlObject);
			}

			return controlObjects;
		}
	}
}