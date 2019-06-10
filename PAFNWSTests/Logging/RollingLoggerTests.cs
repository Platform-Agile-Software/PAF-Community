﻿//@#$&+
namespace PlatformAgileFramework.Logging.Tests
	/// <summary>
		/// <summary>
		/// <summary>
		/// <summary>

		/// <summary>


		/// <summary>

		/// <summary>
		/// <summary>
		/// <summary>

		/// <summary>

		/// <summary>
		/// This one staples in the internal SM, staples in the storage service
		/// </summary>
		public override void TestFixtureSetUp()

			// Make sure the directory is there.
			m_StorageService.PAFEnsureDirectoryExists(s_LoggerDirectory);

			// Empty us out first.
			m_StorageService.PAFEmptyDirectoryOfFiles(s_LoggerDirectory);
		/// <summary>
			// Set up to check file size every time we write.
			var rollingLogger = new RollingLogger(s_LoggerDirectory, "XXX.log", s_MaxFileSizeInBytes,
				s_MaxNumLogFiles, PAFLoggingUtils.MessageOnlyFormatterDelegate, s_SizeCheckFrequency);

			// Always make the service fresh - core services are static.
			var loggingServiceDescription
				= new PAFServiceDescription<IPAFLoggingService>
				(rollingLogger, s_LoggerName);

		/// <summary>
			// Start deleting at 10.
			var maxNumLogFiles = 10;

			// Set up to check file size every time we write.
			var rollingLogger = (IPAFLoggingService)new RollingLogger(s_LoggerDirectory, "XXX.log", s_MaxFileSizeInBytes,
				maxNumLogFiles, PAFLoggingUtils.MessageOnlyFormatterDelegate, s_SizeCheckFrequency);

			// Always make the service fresh - core services are static.
			var loggingServiceDescription

			// Write what should be 12 files.
			logger.LogEntry(s_OneKBOfText);

			// Should have had 2 files deleted.
			var numFiles = m_StorageService.PAFGetFileNames(s_LoggerDirectory).Count();
		}

		/// <summary>
			// This really doesn't matter in this test.
			var maxNumLogFiles = 1;

			// Set up to check file size every time we write. Also need
			// access to the implementation for the test.
			var rollingLogger = new RollingLogger(s_LoggerDirectory, "XXX.log", s_MaxFileSizeInBytes,
				maxNumLogFiles, PAFLoggingUtils.MessageOnlyFormatterDelegate, s_SizeCheckFrequency); 
			var loggingService = (IPAFLoggingService)rollingLogger;

			// Always make the service fresh - core services are static.
			var loggingServiceDescription

			// Writing 2K should generate an exception (which is logged)
			// and disable the rolling logger.
			logger.LogEntry(s_OneKBOfText + s_OneKBOfText);
		}
	}
}