//@#$&+////The MIT X11 License////Copyright (c) 2018 Icucom Corporation////Permission is hereby granted, free of charge, to any person obtaining a copy//of this software and associated documentation files (the "Software"), to deal//in the Software without restriction, including without limitation the rights//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell//copies of the Software, and to permit persons to whom the Software is//furnished to do so, subject to the following conditions:////The above copyright notice and this permission notice shall be included in//all copies or substantial portions of the Software.////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN//THE SOFTWARE.//@#$&-using System.Linq;using System.Text;using NUnit.Framework;using PlatformAgileFramework.Collections;using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;using PlatformAgileFramework.FrameworkServices;using PlatformAgileFramework.FrameworkServices.Tests;// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Logging.Tests{
	/// <summary>	/// These tests test the behavior of the rolling logger under non-concurrent	/// conditions.	/// </summary>    [TestFixture]	public class RollingLoggerTests : BasicServiceManagerTestFixtureBase	{
		/// <summary>		/// Start dispatching files at this point.		/// </summary>        public static int s_MaxNumLogFiles = 10;
		/// <summary>		/// Open new file at this point.		/// </summary>        public static int s_MaxFileSizeInBytes = 1000;
		/// <summary>		/// Default is to check every time we write.		/// </summary>        public static int s_SizeCheckFrequency = 1;

		/// <summary>		/// Text to write. <c>1</c> at the beginning with <c>*</c>s following.		/// </summary>        public static readonly string s_OneKBOfText;


		/// <summary>		/// Name of the logger in the Service Manager.		/// </summary>        public static readonly string s_LoggerName;

		/// <summary>		/// Logger file directory.		/// </summary>        public static string s_LoggerDirectory;
		/// <summary>		/// For independently manipulating storage.		/// </summary>        public IPAFStorageService m_StorageService;
		/// <summary>		/// Need one copy of the service manager internals.		/// </summary>        internal IPAFServiceManagerInternal<IPAFService> m_ServiceManagerInternal;

		/// <summary>		/// Builds our 1 kb of text.		/// </summary>        static RollingLoggerTests()		{			var sb = new StringBuilder("1", 1000);			sb.Append('*', 999);			s_OneKBOfText = sb.ToString();			s_LoggerName = "RollingLogger";		}

		/// <summary>
		/// This one staples in the internal SM, staples in the storage service		/// and sets the logger directory.
		/// </summary>
		public override void TestFixtureSetUp()		{			base.TestFixtureSetUp();			m_ServiceManagerInternal				= (IPAFServiceManagerInternal<IPAFService>)PAFServices.Manager;			m_StorageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();			s_LoggerDirectory =				"Documents:" + DS + "RollingLoggerDirectory";		}		/// <summary>		/// This ensures that our test directories exist, then clears them.		/// </summary>		[SetUp]		public override void SetUp()		{			base.SetUp();

			// Make sure the directory is there.
			m_StorageService.PAFEnsureDirectoryExists(s_LoggerDirectory);

			// Empty us out first.
			m_StorageService.PAFEmptyDirectoryOfFiles(s_LoggerDirectory);		}
		/// <summary>		/// Writes enough data to cause the logger to create one new file		/// while logging, in addition to the initial file.		/// Result will be two dated files in directory. This tests		/// limit on file size.		/// </summary>        [Test]		public void CreateNewFile()		{
			// Set up to check file size every time we write.
			var rollingLogger = new RollingLogger(s_LoggerDirectory, "XXX.log", s_MaxFileSizeInBytes,
				s_MaxNumLogFiles, PAFLoggingUtils.MessageOnlyFormatterDelegate, s_SizeCheckFrequency);

			// Always make the service fresh - core services are static.
			var loggingServiceDescription
				= new PAFServiceDescription<IPAFLoggingService>
				(rollingLogger, s_LoggerName);			m_ServiceManagerInternal.AddOrReplaceServiceInternal(loggingServiceDescription);			var logger = PAFServices.Manager.GetTypedService<IPAFLoggingService>(s_LoggerName, true, null);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			var numFiles = m_StorageService.PAFGetFileNames(s_LoggerDirectory).SafeCount();			Assert.IsTrue(numFiles == 2, "numFiles == 2");		}

		/// <summary>		/// Writes enough data to cause the logger to create 12 log files		/// while logging. This will trigger a deletion of two files.		/// Result will be ten dated files in directory. This tests		/// for files being deleted properly.		/// </summary>        [Test]		public void RollLogFiles()		{
			// Start deleting at 10.
			var maxNumLogFiles = 10;

			// Set up to check file size every time we write.
			var rollingLogger = (IPAFLoggingService)new RollingLogger(s_LoggerDirectory, "XXX.log", s_MaxFileSizeInBytes,
				maxNumLogFiles, PAFLoggingUtils.MessageOnlyFormatterDelegate, s_SizeCheckFrequency);

			// Always make the service fresh - core services are static.
			var loggingServiceDescription				= new PAFServiceDescription<IPAFLoggingService>(rollingLogger, s_LoggerName);			m_ServiceManagerInternal.AddOrReplaceServiceInternal(loggingServiceDescription);			var logger = PAFServices.Manager.GetTypedService<IPAFLoggingService>(s_LoggerName, true, null);

			// Write what should be 12 files.
			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);			logger.LogEntry(s_OneKBOfText);

			// Should have had 2 files deleted.
			var numFiles = m_StorageService.PAFGetFileNames(s_LoggerDirectory).Count();			Assert.IsTrue(numFiles == 10, "numFiles == 10");
		}

		/// <summary>		/// Writes enough data to cause the logger to overflow a file		/// and declare itself "disabled".		/// </summary>        [Test]		public void OverflowLogFile()		{
			// This really doesn't matter in this test.
			var maxNumLogFiles = 1;

			// Set up to check file size every time we write. Also need
			// access to the implementation for the test.
			var rollingLogger = new RollingLogger(s_LoggerDirectory, "XXX.log", s_MaxFileSizeInBytes,
				maxNumLogFiles, PAFLoggingUtils.MessageOnlyFormatterDelegate, s_SizeCheckFrequency); 
			var loggingService = (IPAFLoggingService)rollingLogger;

			// Always make the service fresh - core services are static.
			var loggingServiceDescription				= new PAFServiceDescription<IPAFLoggingService>(loggingService, s_LoggerName);			m_ServiceManagerInternal.AddOrReplaceServiceInternal(loggingServiceDescription);			var logger = PAFServices.Manager.GetTypedService<IPAFLoggingService>(s_LoggerName, true, null);

			// Writing 2K should generate an exception (which is logged)
			// and disable the rolling logger.
			logger.LogEntry(s_OneKBOfText + s_OneKBOfText);			Assert.IsTrue(rollingLogger.m_IsDisabled, "Overflow not detected");
		}
	}
}