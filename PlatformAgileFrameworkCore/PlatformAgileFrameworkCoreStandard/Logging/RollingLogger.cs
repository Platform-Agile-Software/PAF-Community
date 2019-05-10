//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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

#region Using Directives
using PlatformAgileFramework.FileAndIO;
using System;
using System.Collections.Generic;
using System.Threading;
#endregion // Using Directives


namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// Standard rotating log file manager. It solves the problem of ever-increasing
	/// log file size. It manages a finite set of log files of finite size that maintain
	/// a "time-window" of logged information. New files are created and old files are
	/// deleted as time goes on. This logger collapses to using a single ever-increasing
	/// file with appropriate settings of constructor arguments. This class derives it's
	/// file,rotation functionality from it's superclass, <see cref="RollingFileWriter"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 02jan2019 </date>
	/// <description>
	/// Adjusted just a bit more for Golea, by changing file size granularity
	/// to bytes instead of kilobytes.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21nov2018 </date>
	/// <description>
	/// Refactored for extension to "log transmitter". Fixed more of the mistakes
	/// I made in converting this for .Net standard. Took file zipping out
	/// and relegated it to the file dispatch plugin. Refactored for Golea to
	/// allow sub-classing from <see cref="PAFLoggingService"/> so service can
	/// be better changed in nature by adjustment of parameters.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> Bogas </author>
	/// <date> 21dec2017 </date>
	/// <description>
	/// Had to back-port some functionality, since we still need this for Golea.
	/// We can't use things like <c>System.IO.Directory</c>.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26jul2017 </date>
	/// <description>
	/// New. Built this new one for .Net standard. Internal access on some members
	/// for testability.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe by locking on OUR set of files in a directory. No concurrency
	/// within this class.
	/// </threadsafety>
	public class RollingLogger : PAFLoggingService
	{
		#region Fields and AutoProperties
		#endregion // Fields and AutoProperties

		#region Constructors
		/// <summary>
		/// Constructor. Most parameters are passed to <see cref="RollingFileWriter"/>'s
		/// main constructor.
		/// </summary>
		/// <param name="logFileDirectory">
		/// Log file path (not including file name). This may be a symbolic directory.
		/// Not <see langword ="null"/>.
		/// </param>
		/// <param name="logFileBaseNameWithExtension">
		/// Log file base name. This file must NOT have date/time information already
		/// added or a version number. It MUST have an extension.
		/// Not <see langword ="null"/>.
		/// </param>
		/// <param name="maxFileSizeInBytes">
		/// The maximum size of each log file. Default is 10000000 (10 MB).
		/// </param>
		/// <param name="maxFiles">
		/// The maximum number of log files retained in the directory. Default is 10.
		/// </param>
		/// <param name="logFormatterDelegate">
		/// Pluggable log output formatter. Default = <see cref="PAFLoggingUtils.DefaultFormatterDelegate"/>
		/// </param>
		/// <param name="sizeCheckFrequency">
		/// This is the number of writes that occur before file size checking and
		/// potential file rotation. <see cref="RollingFileWriter"/> constructor.
		/// </param>
		/// <param name="filenameStamperAndParser">
		/// Date/version manipulation plugin. <see cref="RollingFileWriter"/> constructor.
		/// </param>
		/// <param name="fileDispatcher">
		/// Plugin to "dispatch" files when rolling. <see cref="RollingFileWriter"/> constructor.
		/// </param>
		/// <param name="loggingLevel">
		/// The default logging level for this logger.<see cref="PAFLoggingService"/> constructor.
		/// </param>
		/// <param name="enableTimeStamp">
		/// Enables a time stamp in the header. <see cref="PAFLoggingService"/> constructor.
		/// </param>
		/// <param name="header">
		/// String placed at the top of every log entry. <see cref="PAFLoggingService"/> constructor.
		/// </param>
		/// <param name="isVersioning">
		/// This parameter tells whether we are writing "versioned" files.
		/// <see cref="RollingFileWriter"/> constructor.
		/// </param>
		/// <param name="versionToken">
		/// String preceding the version number. <see cref="RollingFileWriter"/> constructor.
		/// </param>
		public RollingLogger(string logFileDirectory, string logFileBaseNameWithExtension,
			long maxFileSizeInBytes = 10000000, int maxFiles = 10,
			LogFormatterDelegate logFormatterDelegate = null,
			int sizeCheckFrequency = 100,
			IPAFFilenameStamperAndParser filenameStamperAndParser = null,
			Action<IList<string>> fileDispatcher = null,
			PAFLoggingLevel loggingLevel = PAFLoggingLevel.Error,
			bool enableTimeStamp = true, string header = null,
			bool isVersioning = false,
			string versionToken = null
			)
		:base(logFileDirectory, logFileBaseNameWithExtension,
			maxFileSizeInBytes, maxFiles,
			sizeCheckFrequency,
			filenameStamperAndParser,
			fileDispatcher,
			loggingLevel,
			enableTimeStamp, header,
			logFormatterDelegate,
			isVersioning,
			versionToken)
		{
		}

		#endregion // Constructors

		#region Methods
		/// <remarks>
		/// Just a gate to ensure <see cref="PAFLoggingService.OutputFileName"/> is set first time through.
		/// </remarks>
		public override void LogEntry(object message, PAFLoggingLevel logLevel = PAFLoggingLevel.Error,
			Exception exception = null)
		{
			if (OutputFileName == null)
				OutputFileName = CreateNewFile();

			base.LogEntry(message, logLevel, exception);
		}

		/// <summary>
		/// See Base class. This override adds the file rotation mechanism.
		/// </summary>
		/// <param name="logEntry">See base method.</param>
		/// <param name="logFile">See base method.</param>
		protected override void WriteData(string logEntry, string logFile = null)
		{
			if (m_IsDisabled)
				return;
			if (m_IsPaused)
				return;

			Exception loggingException = null;
			try
			{
				int? bytesAboutToBeWritten = null;
				if ((SizeCheckFrequency == 1) && (logEntry != null))
					bytesAboutToBeWritten = logEntry.Length;

				// Lock directory for file manipulation operations.
				lock (m_FileLockObject)
				{
					RotateFilesIfNecessary(m_NumWrites, FileLengthInBytes,
						bytesAboutToBeWritten);
				}

				base.WriteData(logEntry, logFile);

				Interlocked.Increment(ref m_NumWrites);
			}
			catch (Exception exc)
			{
				loggingException = exc;
			}

			if (loggingException != null)
			{
				PAFLoggingUtils.ReportDisabledLogger(loggingException, out m_IsDisabled);
			}
		}
		#endregion // Methods
	}
}


