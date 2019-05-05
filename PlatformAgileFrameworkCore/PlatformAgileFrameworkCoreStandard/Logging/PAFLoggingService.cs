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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling.Disposal;
#endregion // Using Directives


namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// Default implementation of <see cref="IPAFLoggingServiceInternal"/>. This
	/// implementation uses <see cref="Debug.WriteLine (string)"/> if no writers
	/// are installed.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21dec2018 </date>
	/// <description>
	/// Put in better support for logger failure and emergency logger for
	/// back port to Golea. Added internal visibility to some for testing.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 03jan2015 </date>
	/// <description>
	/// Put the writers into core and made default writer. Fixed things (I hope) so it
	/// won't break legacy stuff.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22jan2011 </date>
	/// <description>
	/// Added history and documentation.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// The logging interface method is safe, the novel methods on this class are NOT.
	/// Adding a writer or setting a logfile should be done only before logger is
	/// released for general consumption. If you need thread-safety on these items,
	/// derive from the class and synchronize them.
	/// </threadsafety>
	// ReSharper disable once InconsistentNaming
	public class PAFLoggingService : RollingFileWriter, IPAFLoggingServiceInternal,
		IPAFLoggerParameters, IPAFLogfileReader
	{
		#region Fields and Autoproperties
		/// <summary>
		/// For testability.
		/// </summary>
		internal static IList<Action<string>> s_PreloadedWriters
			= new List<Action<string>>();
		/// <summary>
		/// Holds a single <see cref="TraceListener"/>. This will be
		/// <see langword="null"/> if we didn't create one. This is
		/// created from <see cref="PAFLoggingUtils.TraceLoggerFactory"/>
		/// at application startup time and should be destroyed from same
		/// in the <see	cref="IDisposable.Dispose"/> in this class..
		/// </summary>
		public TraceListener LogTraceListener { get; [SecurityCritical] set; }
		/// <summary>
		/// Allows a custom formatter to be plugged in.
		/// </summary>
		public LogFormatterDelegate FormatterDelegate { get; protected internal set; }
		/// <summary>
		/// This is the current logging level.
		/// </summary>
		public PAFLoggingLevel LoggingLevel { get; protected internal set; }
		/// <summary>
		/// This is a header that is placed above every log output. It goes before the
		/// timestamp, if there is one.
		/// </summary>
		public string Header { get; protected internal set; }
		/// <summary>
		/// Enable date/time stamp under header if there is one.
		/// </summary>
		public bool? EnableTimeStamp { get; protected internal set; }
		/// <summary>
		/// List of writers which may be hooked to simply write out text to
		/// something or somewhere. Each of these hooked writers should be
		/// protected against throwing exceptions and preferably "disableable".
		/// </summary>
		private readonly IList<Action<string>> m_Writers;
		/// <summary>
		/// Shuts us off if our machinery has defects and generates an exception.
		/// We log the exception, but turn ourselves off so the other loggers
		/// can still perform. Protected so the entire class hierarchy gets disabled.
		/// Never reset to <see langword="false"/>.
		/// </summary>
		protected internal bool m_IsDisabled;

		/// <summary>
		/// Client needs to disable/enable logger. When disabled, logger
		/// swallows all log entries.
		/// </summary>
		protected internal bool m_IsPaused;
		#endregion // Fields and Autoproperties
		#region Static Helpers for the PreLoad
		/// <summary>
		/// Adds a writer.
		/// </summary>
		/// <param name="writer">Writer to add.</param>
		[SecurityCritical]
		public static void AddPreLoadWriter(Action<string> writer)
		{
			s_PreloadedWriters.Add(writer);
		}
		/// <summary>
		/// Produces the writers for the client to fiddle with.
		/// </summary>
		/// <returns>
		/// Never <see langword = "null"/>.
		/// </returns>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		internal static IEnumerable<Action<string>> GetPreLoadWriters()
		{
			return s_PreloadedWriters;
		}
		/// <summary>
		/// Deletes a writer if found.
		/// </summary>
		/// <param name="writer">Writer to remove.</param>
		internal static void RemovePreLoadWriter(Action<string> writer)
		{
			s_PreloadedWriters.Remove(writer);
		}
		#endregion // Static Helpers for the PreLoad
		#region Constructors
		/// <summary>
		/// Has to be here, because activator can't figure out that there is
		/// a default constructor. ReSharper doesn't get it, either.
		/// </summary>
		// ReSharper disable once RedundantArgumentDefaultValue
		public PAFLoggingService() : this(PAFLoggingLevel.Error)
		{
		}

		/// <summary>
		/// Constructor allows setting of time stamp and header, etc.
		/// </summary>
		/// <param name="loggingLevel">
		/// This is the threshold level for the logger. All logging calls with a
		/// level more severe than this will be output. 
		/// </param>
		/// <param name="enableTimeStamp">
		/// <see langword="true"/> to enable time stamp. This is the default.
		/// </param>
		/// <param name="header">
		/// String-ful header to be put at top of every log entry. <see langword="null"/> for
		/// no entry. This is the default. 
		/// </param>
		/// <param name="logFile">
		/// Optional argument allows a single log file to be specified and we synchronize
		/// access to it. Other log files can be specified by creating a set of "writers".
		/// </param>
		/// <param name="logFormatter">
		/// Optional argument allows a delegate to be plugged in to do the writing.
		/// </param>
		/// <remarks>
		/// This logger has dependencies on the storage service, but, it is
		/// assumed that an emergency logger has been installed to break the
		/// deadlock between storage and logger dependencies. If this is not
		/// the case, this logger will not be able to log the problem. This
		/// logger is usually installed as the "main" logger in any
		/// <see cref="IPAFEmergencyServiceProvider{IPAFLogger}"/> that is
		/// installed in the system. <see cref="IPAFServiceExtended.ServiceIsInitialized"/>
		/// is set to <see langword="true"/> in the constructor. This behavior
		/// is driven by customer requirements. 
		/// </remarks>
		public PAFLoggingService
			(PAFLoggingLevel loggingLevel = PAFLoggingLevel.Error,
			bool enableTimeStamp = true, string header = null,
			string logFile = null, LogFormatterDelegate logFormatter = null)
			:base(logFile)
		{
			LoggingLevel = loggingLevel;
			EnableTimeStamp = enableTimeStamp;
			Header = header;
			m_Writers = new Collection<Action<string>>();
			m_OutputFileName = logFile;
			FormatterDelegate = logFormatter ?? PAFLoggingUtils.DefaultFormatterDelegate;

			if (s_PreloadedWriters == null) return;
			foreach (var writer in s_PreloadedWriters)
				m_Writers.Add(writer);
		}
		/// <summary>
		/// This constructor is used to produce a "rolling logger" that
		/// is based on the <see cref="RollingFileWriter"/>. It does not
		/// transfer any static writers, since this type of logger writes
		/// by creating it's own filenames on a rolling basis here. Static
		/// writers don't make sense most of the time. Add them with
		/// <see cref="AddWriter"/> explicitly if needed. 
		/// </summary>
		/// <param name="logFileDirectory">See public constructor.</param>
		/// <param name="logFileBaseNameWithExtension">See public constructor.</param>
		/// <param name="maxFileSizeInBytes">See public constructor.</param>
		/// <param name="maxFiles">See public constructor.</param>
		/// <param name="sizeCheckFrequency">See public constructor.</param>
		/// <param name="filenameStamperAndParser">See public constructor.</param>
		/// <param name="fileDispatcher">See public constructor.</param>
		/// <param name="loggingLevel">See public constructor.</param>
		/// <param name="enableTimeStamp">See public constructor.</param>
		/// <param name="header">See public constructor.</param>
		/// <param name="logFormatter">See public constructor.</param>
		/// <param name="isVersioning">See public constructor.</param>
		/// <param name="versionToken">See public constructor.</param>
		protected PAFLoggingService(
			[NotNull] string logFileDirectory, [NotNull] string logFileBaseNameWithExtension,
			long maxFileSizeInBytes = 10000000, int maxFiles = 10,
			int sizeCheckFrequency = 100,
			IPAFFilenameStamperAndParser filenameStamperAndParser = null,
			Action<IList<string>> fileDispatcher = null,
			PAFLoggingLevel loggingLevel = PAFLoggingLevel.Error,
			bool enableTimeStamp = true, string header = null,
			LogFormatterDelegate logFormatter = null,
			bool isVersioning = false,
			string versionToken = null)
			: base(logFileDirectory, logFileBaseNameWithExtension, maxFileSizeInBytes,
				maxFiles, sizeCheckFrequency, filenameStamperAndParser, fileDispatcher,
				isVersioning, versionToken)
		{
			m_Writers = new Collection<Action<string>>();
			LoggingLevel = loggingLevel;
			EnableTimeStamp = enableTimeStamp;
			Header = header;
			FormatterDelegate = logFormatter;
			if (FormatterDelegate == null)
				FormatterDelegate = PAFLoggingUtils.DefaultFormatterDelegate;
		}
		#endregion // Constructors
		#region Novel Methods
		//////////////////////////////////////////////////////////////////////////////////////////
		// stuff here is not part of the PUBLIC interface, since we want to keep it NARROW and
		// also not expose this stuff to arbitrary callers.
		//////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds a writer.
		/// </summary>
		/// <param name="writer">Writer to add.</param>
		/// <remarks>
		/// For privileged callers. Each of these hooked writers should be
		/// protected against throwing exceptions and preferably "disableable".
		/// Every writer must have access to it synchronized internally unless
		/// this particular instance of the logger is not called concurrently.
		/// </remarks>
		[SecurityCritical]
		public virtual void AddWriter(Action<string> writer)
		{
			m_Writers.Add(writer);
		}
		/// <summary>
		/// Produces the writers for the client to fiddle with.
		/// </summary>
		/// <returns>
		/// Never <see langword = "null"/>.
		/// </returns>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual IEnumerable<Action<string>> GetWriters()
		{
			return m_Writers;
		}
		/// <summary>
		/// Deletes a writer if found.
		/// </summary>
		/// <param name="writer">Writer to remove.</param>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual void RemoveWriter(Action<string> writer)
		{
			m_Writers.Remove(writer);
		}
		/// <summary>
		/// Our reader that reads the log file. Mostly for automated testing support.
		/// </summary>
		/// <param name="logFile">
		/// Specify <see cref="string.Empty"/> to read the log file specified by the
		/// current <see cref="RollingFileWriter.OutputFileName"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if we got nothing.
		/// </returns>
		internal virtual string ReadLogFile(string logFile)
		{
			if (logFile == string.Empty) logFile = OutputFileName;
			if (string.IsNullOrEmpty(logFile)) return null;
			lock (m_FileLockObject)
			{
				var storageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
				using (var stream = storageService.PAFOpenFile(logFile, PAFFileAccessMode.READONLY))
				{
					return stream.PAFReadString();
				}
			}
		}

		/// <summary>
		/// Sets the delegate.
		/// </summary>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual void SetFormattingDelegate(LogFormatterDelegate logFormattingDelegate)
		{
			FormatterDelegate = logFormattingDelegate;
		}

		/// <summary>
		/// This is the current logging level for the logger instance.
		/// </summary>
		/// <param name="loggingLevel">
		/// Sets the current <see cref="PAFLoggingLevel"/>. All logs
		/// with a value equal to this or more severe will be logged.
		/// </param>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual void SetLoggingLevel(PAFLoggingLevel loggingLevel)
		{
			LoggingLevel = loggingLevel;
		}
		/// <summary>
		/// This is the file to write to.
		/// </summary>
		/// <param name="logFile">
		/// Sets the filename for the SINGLE base log file that we write
		/// to. It defaults to <see langword="null"/> so it often needs
		/// to be set. External writers can do their own thing if they
		/// are hooked.
		/// </param>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual void SetOutputFile(string logFile)
		{
			OutputFileName = logFile;
		}
		/// <summary>
		/// Sets the header.
		/// </summary>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual void SetHeader(string header)
		{
			Header = header;
		}
		/// <summary>
		/// Enables timestamp.
		/// </summary>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual void SetTimeStampEnabled(bool enableTimeStamp)
		{
			EnableTimeStamp = enableTimeStamp;
		}

		/// <summary>
		/// See base. This override just adds guards for paused and disabled
		/// and also, for no file, which is OK in this class.
		/// </summary>
		/// <param name="dataEntry">See base.</param>
		/// <param name="outputFile">
		/// See base.
		/// </param>
		protected override void WriteData(string dataEntry,
			string outputFile = null)
		{
			if (m_IsDisabled)
				return;

			if (m_IsPaused)
				return;

			if (outputFile == null && OutputFileName == null)
				return;

			base.WriteData(dataEntry, outputFile);
		}

		#region Static Helper Methods
		/// <summary>
		/// Tiny little helper that wraps the conditional debug write
		/// statement because it's not there when not running in debug mode.
		/// </summary>
		/// <param name="stringOut"></param>
		public static void DebugWriteLine(string stringOut)
		{
			Debug.WriteLine(stringOut);
		}

		#endregion // Static Helper Methods
		#endregion // Novel Methods
		#region Implementation of IPAFLoggingServiceInternal
		#region Implementation of IPAFFileWriterInternal
		/// <remarks>
		/// See <see cref="IPAFFileWriterInternal"/>
		/// </remarks>
		ICollection<Action<string>> IPAFFileWriterInternal.Writers
		{ get { return m_Writers; } }
		/// <summary>
		/// See <see cref="IPAFFileWriterInternal"/>.
		/// </summary>
		string IPAFFileWriterInternal.ReadOutputFile(string outputFile)
		{
			return ReadLogFile(outputFile);
		}
		/// <summary>
		/// <see cref="IPAFFileWriterInternal"/>
		/// </summary>
		void IPAFFileWriterInternal.SetOutputFile(string outputFile)
		{
			OutputFileName = outputFile;
		}
		#endregion // Implementation of IPAFFileWriterInternal
		/// <summary>
		/// <see cref="IPAFLoggingServiceInternal"/>
		/// </summary>
		void IPAFLoggingServiceInternal.SetFormattingDelegate(LogFormatterDelegate logFormattingDelegate)
		{
			FormatterDelegate = logFormattingDelegate;
		}
		/// <summary>
		/// <see cref="IPAFLoggingServiceInternal"/>
		/// </summary>
		void IPAFLoggingServiceInternal.SetLoggingLevel(PAFLoggingLevel loggingLevel)
		{
			LoggingLevel = loggingLevel;
		}
		/// <summary>
		/// <see cref="IPAFLoggingServiceInternal"/>
		/// </summary>
		void IPAFLoggingServiceInternal.SetHeader(string header)
		{
			Header = header;
		}
		/// <summary>
		/// <see cref="IPAFLoggingServiceInternal"/>
		/// </summary>
		void IPAFLoggingServiceInternal.SetTimeStampEnabled(bool enableTimeStamp)
		{
			EnableTimeStamp = enableTimeStamp;
		}
		#endregion // Implementation of IPAFLoggingServiceInternal
		#region Implementation of IPAFLoggingService

		/// <remarks>
		/// See <see cref="IPAFLoggingService"/>. This interface method was
		/// originally introduced for a Xamarin app that needed to upload
		/// a partially completed log file from time to time. This functionality
		/// has now been largely absorbed by the rolling logger. This remains
		/// for backward compatibility, but has been removed from the interface.
		/// </remarks>
		public virtual bool IsPaused
		{
			get => m_IsPaused;
			set => m_IsPaused = value;
		}

		/// <remarks>
		/// See <see cref="IPAFLoggingService"/>. This implementation allows the
		/// "trap door" message to be used to customize the logging parameters
		/// on an individual call. See <see cref="IPAFLoggerMessage"/> is
		/// passed as "message".
		/// Uses <see cref="FormatterDelegate"/> if plugged. Uses a time stamp
		/// <see cref="EnableTimeStamp"/> = <see langword="false"/>.
		/// </remarks>
		public virtual void LogEntry(object message, PAFLoggingLevel logLevel = PAFLoggingLevel.Error,
			Exception exception = null)
		{
			if (m_IsDisabled)
				return;

			if (IsPaused)
				return;

			var header = Header;
			var enableTimeStamp = EnableTimeStamp != false;
			var loggingLevel = LoggingLevel;
			var logFile = OutputFileName;

			// Find out if user has customized the logger in the call.
			if (message is IPAFLoggerMessage specialMessage)
			{
				message = specialMessage.LogMessage;
				var parameters = specialMessage.LoggerParameters;
				if (parameters != null)
				{
					header = parameters.Header;
					enableTimeStamp = parameters.EnableTimeStamp != false;
					loggingLevel = parameters.LoggingLevel;
					if (parameters.OutputFileName != null)
						logFile = parameters.OutputFileName;
				}
			}

			if ((logLevel != PAFLoggingLevel.Default) && (logLevel < loggingLevel)) return;

			var lineOut = FormatterDelegate(message, logLevel, exception, header, enableTimeStamp);

			// We can do the debug write here, since not writing anything makes no sense.
			// Moved from the trace writer for code consolidation.
			if ((m_Writers.SafeCount() == 0) && (string.IsNullOrEmpty(logFile)))
			{
				DebugWriteLine(lineOut);
				return;
			}

			if ((m_Writers.SafeCount() != 0))
			{
				foreach (var writer in m_Writers)
				{
					writer(lineOut);
				}
			}

			WriteData(lineOut, logFile);
		}
		#endregion // Implementation of IPAFLoggingService
		#region Implementation of IPAFLogfileReader
		/// <summary>
		/// See <see cref="IPAFLogfileReader"/>.
		/// </summary>
		public virtual string ReadInstanceLogFile()
		{
			return ReadLogFile(OutputFileName);
		}
		#endregion // Implementation of IPAFLogfileReader
		#region Disposal-Related
		/// <summary>
		/// <see cref="IPAFDisposable"/>. This is a method that is supplied as a delegate
		/// to the disposer to call during disposal. We add our disposable stuff in this
		/// override.
		/// </summary>
		/// <param name="disposing">
		/// <see cref="IPAFDisposable"/>.
		/// </param>
		/// <param name="obj">
		/// <see cref="IPAFDisposable"/>.
		/// This is not used in this method.
		/// </param>
		/// <returns>
		/// <see cref="IPAFDisposable"/>.
		/// </returns>
		protected override Exception PAFFrameworkServiceDispose(bool disposing, object obj)
		{
			var eList = new List<Exception>();
			Exception traceDisposalException = null;
			if (LogTraceListener != null)
			{
				try
				{
					// If the factory is not here, then the listener was installed
					// improperly somehow and we want to record the exception.
					PAFLoggingUtils.TraceLoggerFactory.DestroyListener(LogTraceListener);
				}
				catch (Exception caughtException)
				{
					traceDisposalException = caughtException;
				}
			}
			
			eList.AddNoNulls(traceDisposalException);

			// Gather exceptions from base.
			var baseExceptions = base.PAFFrameworkServiceDispose(disposing, obj);
			eList.AddNoNulls(baseExceptions);

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count == 0) return null;

			var exceptions = new PAFAggregateExceptionData(eList);
			var ex = new PAFStandardException<PAFAggregateExceptionData>(exceptions);
			// Seal the list.
			exceptions.AddException(null);
			// We just put these in the registry.
			DisposalRegistry.RecordDisposalException(GetType(), ex);
			return ex;
		}
		#endregion // Disposal-Related
	}
}
