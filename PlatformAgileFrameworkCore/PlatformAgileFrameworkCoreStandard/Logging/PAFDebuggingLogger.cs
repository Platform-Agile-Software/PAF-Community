// copied from Golea
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
using PlatformAgileFramework.FrameworkServices;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// This is a little helper that allows one to set up a custom logging
	/// operation when one is working on debugging a specific class or a
	/// specific area of code. It uses the the message object on the log
	/// call to change the logger behavior.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16apr2016 </date>
	/// <description>
	/// Added instance props, since a non-singleton didn't make any sense
	/// without them.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 02jun2015 </date>
	/// <description>
	/// Copied from Golea.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// KRM 11apr2016 - this is kind of a goofy class. You can use it as a singleton or not.
	/// I can't figure out if this is a good thing....
	/// </remarks>
	// ReSharper disable once InconsistentNaming
	public class PAFDebuggingLogger : IPAFLoggerMessage, IPAFLoggerParameters
	{
		#region Fields and Autoproperties
		/// <summary>
		/// This is a log file that will be written to INSTEAD of the default
		/// log file (if there is one) in the main logger.
		/// </summary>
		public static string StaticLogFile { get; set; }
		/// <summary>
		/// This is the current logging level. It is static, since it is designed
		/// to be set only once as this class is designed to be used as a singleton
		/// for a developer's personal debugging assistance.
		/// </summary>
		public static PAFLoggingLevel StaticLoggingLevel { get; set; }
		/// <summary>
		/// This is a header that is placed above every log output. It goes before the
		/// timestamp, if there is one. It is static, since it is designed
		/// to be set only once as this class is designed to be used as a singleton
		/// for a developer's personal debugging assistance.
		/// </summary>
		public static string StaticHeader { get; set; }
		/// <summary>
		/// Allows a logger to stapled in statically. If not here, we grab the logger
		/// from the service manager dynamically. It is static, since it is designed
		/// to be set only once as this class is designed to be used as a singleton
		/// for a developer's personal debugging assistance.
		/// </summary>
		public static IPAFLoggingService StaticLoggingService { get; set; }
		/// <summary>
		/// Enable date stamp under header if there is one. It is static, since it is designed
		/// to be set only once as this class is designed to be used as a singleton
		/// for a developer's personal debugging assistance. It is static, since it is designed
		/// to be set only once as this class is designed to be used as a singleton
		/// for a developer's personal debugging assistance.
		/// </summary>
		public static bool StaticEnableTimeStamp { get; set; }
		/// <summary>
		/// backing for the prop.
		/// </summary>
		protected internal string m_LogFile;
		/// <summary>
		/// backing for the prop.
		/// </summary>
		protected internal PAFLoggingLevel m_LoggingLevel;
		/// <summary>
		/// backing for the prop.
		/// </summary>
		protected internal string m_Header;
		/// <summary>
		/// backing for the prop.
		/// </summary>
		protected internal bool? m_EnableTimeStamp;
		/// <summary>
		/// This is what we need to serialize access to.
		/// </summary>
		private object _logMessage;
		/// <summary>
		/// For thread safety, we need to serialize calls into this class.
		/// </summary>
		private static readonly object _messageLock = new object();
		/// <summary>
		/// For thread safety, we need to serialize calls into this class.
		/// </summary>
		private static readonly object _logEntryLock = new object();
		/// <summary>
		/// This a thread-safe wrapper for constructing the singleton.
		/// </summary>
		/// <remarks>
		/// Lazy class is thread-safe by default.
		/// </remarks>
		private static readonly Lazy<PAFDebuggingLogger> _singleton =
			   new Lazy<PAFDebuggingLogger>(ConstructLogger);

		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor sets up defaults for the debug level as
		/// <see cref="PAFLoggingLevel.Default"/> and enables a time stamp.
		/// These can be overriden by simply setting the statics on this
		/// class before accessing <see cref="MyLogger"/>.
		/// </summary>
		static PAFDebuggingLogger()
		{
			StaticLoggingLevel = PAFLoggingLevel.Default;
			StaticEnableTimeStamp = true;
		}
		/// <summary>
		/// Not quite a constructor - a factory for the lazy construction.
		/// </summary>
		private static PAFDebuggingLogger ConstructLogger()
		{
			return new PAFDebuggingLogger();
		}
		/// <summary>
		/// Constructor for the singleton.
		/// </summary>
		// ReSharper disable once EmptyConstructor
		internal PAFDebuggingLogger()
		{
		}

		/// <summary>
		/// Constructor with full params allows instances to be produced.
		/// </summary>
		/// <param name="header">
		/// Default = <see langword="null"/> fetches the static header.
		/// </param>
		/// <param name="logFile">
		/// Default = <see langword="null"/> fetches the static logfile.
		/// </param>
		/// <param name="loggingLevel">
		/// Default = <see cref="PAFLoggingLevel.Default"/> causes logger to always log.
		/// </param>
		/// <param name="enableTimeStamp">
		/// Default = <see langword="true"/> puts timestamp.
		/// </param>
		public PAFDebuggingLogger(string header = null, string logFile = null,
			PAFLoggingLevel loggingLevel = PAFLoggingLevel.Default, bool? enableTimeStamp = true)
		{
			if (string.IsNullOrEmpty(header)) m_Header = header;
			if (string.IsNullOrEmpty(logFile)) m_LogFile = logFile;
			m_LoggingLevel = loggingLevel;
			m_EnableTimeStamp = enableTimeStamp;
		}

		#endregion // Constructors
		/// <summary>
		/// Get the singleton instance of the logger. 
		/// </summary>
		/// <returns>The logger.</returns>
		public static PAFDebuggingLogger MyLogger
		{
			get { return _singleton.Value; }
		}
		/// <summary>
		/// This is our main logging method.
		/// </summary>
		/// <param name="message">
		/// The message, which can be <see langword="null"/>.
		/// </param>
		/// <param name="logLevel">
		/// default is <see	cref ="PAFLoggingLevel.Default"/>
		/// </param>
		/// <param name="exception"></param>
		public virtual void MyLogEntry(object message, PAFLoggingLevel logLevel = PAFLoggingLevel.Default,
			Exception exception = null)
		{
			// We must lock ourselves while we update the message string and present ourselves
			// as a <see cref="IPAFLoggerMessage"/> to the logger.
			lock (_logEntryLock)
			{
				LogMessage = message;
				var mainLogger = StaticLoggingService;
				if (mainLogger == null)
					mainLogger = PAFServices.Manager.GetTypedService<IPAFLoggingService>();
				mainLogger.LogEntry(this, logLevel, exception);
			}
		}
		#region Implementation of IPAFLoggerParameters

		/// <summary>
		/// See <see cref="IPAFLoggerParameters"/>.
		/// Returns the static (possibly null) if instance not set.
		/// </summary>
		public string LogFile
		{
			get
			{
				if(m_LogFile != null) return m_LogFile;
				return StaticLogFile;
			}
		}

		/// <summary>
		/// See <see cref="IPAFLoggerParameters"/>.
		/// Tries for the instance if not default, then the static if not "off",
		/// then loads default as a last alternative.
		/// </summary>
		public PAFLoggingLevel LoggingLevel
		{
			get
			{
				if (m_LoggingLevel != PAFLoggingLevel.Default) return m_LoggingLevel;
				if (StaticLoggingLevel != 0) return StaticLoggingLevel;
				return PAFLoggingLevel.Default;
			}
		}

		/// <summary>
		/// See <see cref="IPAFLoggerParameters"/>.
		/// Tries for the instance if not null, then the static if not null,
		/// then loads a blank as a last alternative.
		/// </summary>
		public string Header
		{
			get
			{
				if (m_Header != null) return m_Header;
				if (StaticHeader != null) return StaticHeader;
				return "";
			}
		}

		/// <summary>
		/// See <see cref="IPAFLoggerParameters"/>.
		/// Tries for the instance if not null, then returns the value of the static.
		/// </summary>
		public bool? EnableTimeStamp
		{
			get
			{
				if (m_EnableTimeStamp.HasValue) return m_EnableTimeStamp;
				if (StaticEnableTimeStamp) return true;
				return false;
			}
		}
		#endregion // Implementation of IPAFLoggerParameters

		#region Implementation of IPAFLoggerMessage
		/// <summary>
		/// See <see cref="IPAFLoggerMessage"/>.
		/// </summary>
		public IPAFLoggerParameters LoggerParameters
		{
			get { return this; }
		}
		/// <summary>
		/// See <see cref="IPAFLoggerMessage"/>.
		/// </summary>
		public object LogMessage
		{
			get
			{
				lock (_messageLock)
				{
					return _logMessage;
				}
			}
			protected internal set
			{
				lock (_messageLock)
				{
					_logMessage = value;
				}
			}
		}
		#endregion // IPAFLoggerMessage

		#region Overrides of Object
		/// <summary>
		/// Have to override <see cref="ToString()"/> to produce a message in the default case.
		/// </summary>
		/// <returns>Our message or "" if null.</returns>
		public override string ToString()
		{
			if (LogMessage == null) return "";
			return LogMessage.ToString();
		}
		#endregion
	}
}
