
//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Platform;

namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// Default implementation of <see cref="IPAFLoggingServiceInternal"/>. This
	/// implementation uses <see cref="Debug.WriteLine (string)"/> if no writers
	/// are installed.
	/// </summary>
	/// <history>
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
	public class PAFLoggingService: PAFService, IPAFLoggingServiceInternal,
		IPAFLoggerParameters, IPAFLogfileReader
	{
		#region Fields and Autoproperties
		/// <summary>
		/// For testability.
		/// </summary>
		internal static IList<Action<string>> s_PreloadedWriters = new List<Action<string>>();
		/// <summary>
		/// Allows a custum formatter to be plugged in.
		/// </summary>
		protected internal LogFormatterDelegate FormatterDelegate {get; set; }
		/// <summary>
		/// This is the current logging level.
		/// </summary>
		public PAFLoggingLevel LoggingLevel { get; protected internal set; }
		/// <summary>
		/// Optional SINGLE logfile that will be added to the writer collection if
		/// this string is not vacuous.
		/// </summary>
		public string LogFile { get; protected internal set; }
		/// <summary>
		/// This is a header that is placed above every log output. It goes before the
		/// timestamp, if there is one.
		/// </summary>
		public string Header { get; protected internal set; }
		/// <summary>
		/// Enable date stamp under header if there is one.
		/// </summary>
		public bool? EnableTimeStamp { get; protected internal set; }
		/// <summary>
		/// This needs to be static, since we are contending for one file. The lock should
		/// really be a system-wide mutex, but the emergency logger is normally only instantiated
		/// in the Main AppDomain or process, anyway, so we don't worry about it.
		/// </summary>
		private static readonly object s_FileLockObject = new object();
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		private readonly IList<Action<string>> m_Writers;
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
		public static IEnumerable<Action<string>> GetPreLoadWriters()
		{
			return s_PreloadedWriters;
		}
		/// <summary>
		/// Deletes a writer if found.
		/// </summary>
		/// <param name="writer">Writer to remove.</param>
		[SecurityCritical]
		public static void RemovePreLoadWriter(Action<string> writer)
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
		public PAFLoggingService(): this(PAFLoggingLevel.Error)
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
		/// Stringful header to be put at top of every log entry. <see langword="null"/> for
		/// no entry. This is the default. 
		/// </param>
		/// <param name="logFile">
		/// Optional argument allows a single log file to be specified and we synchronize
		/// access to it. Other log files can be specified by creating a set of loggers
		/// and calling them in turn or by supplying an external logging delegate in the
		/// logger property below. Please synchronize access to it or we will have no
		/// sympathy for you when you call in for support. See our example.
		/// </param>
		/// <param name="logFormatter">
		/// Optional argument allows a delegate to be plugged in to do the writing.
		/// </param>
		public PAFLoggingService
			(PAFLoggingLevel loggingLevel = PAFLoggingLevel.Error,
			bool enableTimeStamp = true, string header = null,
			string logFile = null, LogFormatterDelegate logFormatter = null)
		{
			LoggingLevel = loggingLevel;
			EnableTimeStamp = enableTimeStamp;
			Header = header;
			m_Writers = new Collection<Action<string>> ();
			LogFile = logFile;
			FormatterDelegate = logFormatter;

			if (s_PreloadedWriters != null)
			{
				foreach(var writer in s_PreloadedWriters)
					m_Writers.Add(writer);
			}
		}
		#endregion // Constructors
		#region Novel Methods
		//////////////////////////////////////////////////////////////////////////////////////////
		// stuff here is not part of the PUBLIC interface, since we want to keep it NARROW.
		//////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds a writer.
		/// </summary>
		/// <param name="writer">Writer to add</param>
		/// <remarks>
		/// For privileged callers.
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
		/// Mostly for automated testing support. Reads the logfile as specified
		/// in the <paramref name="logFile"/>.
		/// </summary>
		/// <param name="logFile">
		/// Specify <see cref="String.Empty"/> to read the log file specified by the
		/// current <see cref="LogFile"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if we got nothin'.
		/// </returns>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual string ReadLogFileUtility(string logFile)
		{
			return ReadLogFile(logFile);
		}
		/// <summary>
		/// Sets the delegate.
		/// </summary>
		/// <remarks>
		/// For privileged callers.
		/// </remarks>
		[SecurityCritical]
		public virtual void SetFormattingDelegatee(LogFormatterDelegate logFormattingDelegate)
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
		public virtual void SetLogFile(string logFile)
		{
			LogFile = logFile;
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
		#endregion // Novel Methods
		#region Implementation of IPAFLoggingServiceInternal
		/// <remarks>
		/// See <see cref="IPAFLoggingServiceInternal"/>
		/// </remarks>
		ICollection<Action<string>> IPAFLoggingServiceInternal.LogWriters
		{ get { return m_Writers; } }
		/// <summary>
		/// See <see cref="IPAFLoggingServiceInternal"/>.
		/// </summary>
		string IPAFLoggingServiceInternal.ReadLogFile(string logFile)
		{
			return ReadLogFile(logFile);
		}
		/// <summary>
		/// Our reader that reads the log file. Mostly for automated testing support.
		/// </summary>
		/// <param name="logFile">
		/// Specify <see cref="String.Empty"/> to read the log file specified by the
		/// current <see cref="LogFile"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if we got nothin'.
		/// </returns>
		internal virtual string ReadLogFile(string logFile)
		{
			if (logFile == string.Empty) logFile = LogFile;
			if (string.IsNullOrEmpty(logFile)) return null;
			lock (s_FileLockObject)
			{
				var storageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
				using (var stream = storageService.PAFOpenFile(logFile, PAFFileAccessMode.READONLY))
				{
					return stream.PAFReadString();
				}
			}
		}

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
		void IPAFLoggingServiceInternal.SetLogFile(string logFile)
		{
			LogFile = logFile;
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
		/// See <see cref="IPAFLoggingService"/>
		/// </remarks>
		public virtual void LogEntry(object message,
			PAFLoggingLevel logLevel = PAFLoggingLevel.Error,
			Exception exception = null)
		{
			var header = Header;
			var enableTimeStamp = EnableTimeStamp != false;
			var loggingLevel = LoggingLevel;
			var logFile = LogFile;

			// Find out if user has customized the logger in the call.
			var specialMessage = message as IPAFLoggerMessage;
			if (specialMessage != null)
			{
				message = specialMessage.LogMessage;
				var parameters = specialMessage.LoggerParameters;
				if (parameters != null)
				{
					header = parameters.Header;
					enableTimeStamp = parameters.EnableTimeStamp != false;
					loggingLevel = parameters.LoggingLevel;
					if (parameters.LogFile != null) logFile = parameters.LogFile;
				}
			}

			if ((logLevel != PAFLoggingLevel.Default) && (logLevel < loggingLevel)) return;

			string lineOut;
			if (FormatterDelegate != null)
			{
				lineOut = FormatterDelegate(message, logLevel, exception, header, enableTimeStamp);
			}
			else
			{
				lineOut = DefaultFormatterDelegate(message, logLevel, exception, header, enableTimeStamp);
			}

			// Simple case doesn't break existing code.
			if ((m_Writers.Count == 0) && (string.IsNullOrEmpty(logFile)))
			{
				Debug.WriteLine (lineOut);
				return;
			}
			foreach (var writer in m_Writers)
			{
				writer (lineOut);
			}
			WriteToLogFile(lineOut, logFile);
		}
		#endregion // Implementation of IPAFLoggingService
		#region Implementation of IPAFLogfileReader
		/// <summary>
		/// See <see cref="IPAFLogfileReader"/>
		/// </summary>
		public string ReadInstanceLogFile()
		{
			return ReadLogFile(LogFile);
		}
		#endregion // Implementation of IPAFLogfileReader
		/// <summary>
		/// Tiny little helper that wraps the conditional debug write
		/// statememt because it's not there when not running in debug mode.
		/// </summary>
		/// <param name="stringOut"></param>
		public static void DebugWriteLine(string stringOut)
		{
			Debug.WriteLine(stringOut);
		}

		/// <summary>
		/// Our writer that writes to a file if one is specified.
		/// </summary>
		/// <param name="logEntry">Entry to write.</param>
		/// <param name="logFile">File to write to.</param>
		protected internal virtual void WriteToLogFile(string logEntry, string logFile)
		{
			if (string.IsNullOrEmpty(logFile)) return;
			lock (s_FileLockObject)
			{
				var storageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
				using (var stream = storageService.PAFOpenFile(logFile, PAFFileAccessMode.APPEND))
				{
					stream.PAFWriteString(logEntry);
				}
			}
		}

		/// <summary>
		/// Our default formatter that is used if none plugged.
		/// </summary>
		/// <param name="message"><see cref="LogFormatterDelegate"/>.</param>
		/// <param name="logLevel"><see cref="LogFormatterDelegate"/></param>
		/// <param name="exception"><see cref="LogFormatterDelegate"/></param>
		/// <param name="header"><see cref="LogFormatterDelegate"/></param>
		/// <param name="enableTimeStamp"><see cref="LogFormatterDelegate"/></param>
		/// <returns></returns>
		protected internal virtual string DefaultFormatterDelegate(object message, PAFLoggingLevel logLevel,
			Exception exception, string header, bool enableTimeStamp)
		{
			var lineOut = "";
			if (header != null) lineOut = header + PlatformUtils.LTRMN;
			var logTag = "[" + logLevel;
			if (enableTimeStamp) logTag += " - " + DateTime.Now.ToString(CultureInfo.InvariantCulture);
			logTag += "]" + PlatformUtils.LTRMN;
			lineOut += logTag;
			var output = message.ToString();
			if (string.IsNullOrEmpty(output))
			{
				output = "";
			}
			lineOut += output + PlatformUtils.LTRMN;
			if (exception != null) lineOut += ("Exception: " + exception.Message + PlatformUtils.LTRMN);
			return lineOut;
		}
	}
}
