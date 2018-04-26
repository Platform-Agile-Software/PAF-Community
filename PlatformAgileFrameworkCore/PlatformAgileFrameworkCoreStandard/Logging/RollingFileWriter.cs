//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Reflection;
//using System.Threading;
//using PlatformAgileFramework.Annotations;
//using PlatformAgileFramework.FileAndIO;
//using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
//using PlatformAgileFramework.FrameworkServices;
//using PlatformAgileFramework.MultiProcessing.Threading.Locks;
//using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
//using PlatformAgileFramework.Platform;
//using PlatformAgileFramework.StringParsing;
//using PlatformAgileFramework.TypeHandling.Delegates;

//namespace PlatformAgileFramework.Logging
//{
//	/// <summary>
//	/// Standard rotating text file manager. It solves the problem of ever-increasing
//	/// log file size. It manages a finite set of logfiles of finite size that maintain
//	/// a "time-window" of logged information. New files are created and old files are
//	/// deleted as time goes on.
//	/// </summary>
//	/// <history>
//	/// <contribution>
//	/// <author> KRM </author>
//	/// <date> 26jul2017 </date>
//	/// <description>
//	/// New. Bulit this new one with an atomic container, since the old had
//	/// concurrency hazards.
//	/// </description>
//	/// </contribution>
//	/// </history>
//	public class RollingFileWriter
//	{
//		#region Fields and AutoProperties

//		/// <summary>
//		/// This is a container for our stream and stream writer that
//		/// we use as a lock object to provide atomic handling of these
//		/// objects.
//		/// </summary>
//		private StreamAndWriterAtomicContainer m_StreamAndWriterContainer;
//		/// <summary>
//		/// Shuts us off if our machinery has defects and generates an exception.
//		/// We log the exception, but turn ourselves off so the other loggers,
//		/// such as the emergency logger can still report our problem.
//		/// </summary>
//		private volatile bool m_IsDisabled;

//		/// <summary>
//		/// Have we been disposed?.
//		/// </summary>
//		private volatile bool m_IsDisposed = true;

//		/// <summary>
//		/// This is the default internal format string for turning dates into text
//		/// at the end of files. Plug in a <see cref="DateTimeFormatter"/> if you want
//		/// something different like UTC.
//		/// </summary>
//		protected internal static string m_DefaultDateTimeFormatString
//			= "_{0:yyyy-MM-dd}_{0:HH-mm-ss}";

//		/// <summary>
//		/// Logfile extension.
//		/// </summary>
//		protected internal string m_LogfileExtensionWithDot = ".log";

//		/// <summary>
//		/// Searches for our numbered set of files.
//		/// </summary>
//		protected internal string m_FileSearchPattern;

//		/// <summary>
//		/// This is the base file name which has date/time information appended to the end
//		/// to identify individual files in the set of retained files. This is just the name,
//		/// without preceding path information.
//		/// </summary>
//		protected internal string LogFileBaseName { get; internal set; }

//		/// <summary>
//		/// This is the directory in which log files are placed. This may NOT be a symbolic
//		/// directory.
//		/// </summary>
//		protected internal string LogDirectory { get; set; }

//		/// <summary>
//		/// This is the maximum size of each logfile. The default is 10000, or 10 KB.
//		/// </summary>
//		protected internal int MaxLogFileSizeKb { get; set; }

//		/// <summary>
//		/// Pluggable formatter for date/time.
//		/// </summary>
//		protected internal Func<DateTime, string> DateTimeFormatter { get; set; }

//		/// <summary>
//		/// Defines the maximum number of files kept in the set of rolling logs.
//		/// </summary>
//		protected internal int MaxLogFiles { get; set; }

//		/// <summary>
//		/// Determines whether we are "Versioning" the sets of rolling log
//		/// files. Sometimes we don't want to overwrite log files from previous runs.
//		/// In that case, we append 1 or 2 or 3, etc., so logfile names are
//		/// something like ..._0.log, ..._1.log, etc. This class makes no provison
//		/// for cleaning old versions out, except, if it is constructed with
//		/// versioning off, we write in the base file set again and destroy
//		/// the versioned file sets that are existing.
//		/// </summary>
//		protected internal bool IsVersioning { get; set; }

//		/// <summary>
//		/// m_NumWrites is reset to 0 when it reaches this value. This
//		/// is so we don't have to check file size on every single write.
//		/// </summary>
//		protected internal int SizeCheckFrequency { get; internal set; }

//		/// <summary>
//		/// Determines whether we are "Versioning" the sets of rolling log
//		/// files. Sometimes we don't want to overwrite log files from previous runs.
//		/// In that case, we append 1 or 2 or 3, etc., so logfile names are
//		/// something like ...@0.log, ...@1.log, etc. This class makes no provison
//		/// for cleaning old versions out, except, if it is constructed with
//		/// versioning off, we write in the base file set again and destroy
//		/// the versioned file sets that are existing. Versioned files
//		/// are meant primarily for testing. Note that an INDEPENDENT
//		/// rolling logger can be created for testing.
//		/// This will be <see langword="null"/> if not versioning.
//		/// </summary>
//		protected internal int? CurrentVersionNumber { get; internal set; }

//		/// <summary>
//		/// This is the atomic tally of the current number of writes
//		/// that have been made.  GT 0 and  LT SizeCheckFrequency.
//		/// </summary>
//		private volatile int m_NumWrites = 0;

//		/// <summary>
//		/// File service stapled in in the construction path.
//		/// </summary>
//		protected internal IPAFStorageService m_StorageService;
//		#endregion // Fields and AutoProperties

//		#region Constructors
//		/// <summary>
//		/// Constructor
//		/// </summary>
//		/// <param name="logDirectory">
//		/// Log file path (not including file name). This may be a symbolic directory.
//		/// Not <see langword ="null"/>.
//		/// </param>
//		/// <param name="logFileBaseName">
//		/// Log file base name.
//		/// Not <see langword ="null"/>.
//		/// </param>
//		/// <param name="maxLogFileSizeKb">
//		/// The maximum size of each log file. Default is 10000 (10 MB).
//		/// </param>
//		/// <param name="maxLogFiles">
//		/// The maximum number of log files retained in the directory. Default is 10000.
//		/// </param>
//		/// <param name="sizeCheckFrequency">
//		/// This is the number of writes that occur before file size checking and
//		/// potential file rotation. Sets <see cref="SizeCheckFrequency"/>. 
//		/// </param>
//		/// <param name="dateTimeFormatter">
//		/// Formatter plugin. Default = <see langword="null"/> causes default internal
//		/// format to be used for the date/time part of the filename.
//		/// </param>
//		/// <param name="isVersioning">
//		/// This parameter tells whether we should overwrite an existing log file when we
//		/// are initialized to start writing over the base log file.
//		/// </param>
//		public RollingFileWriter(string logDirectory, string logFileBaseName,
//			int maxLogFileSizeKb = 10000, int maxLogFiles = 10,
//			int sizeCheckFrequency = 100,
//			[CanBeNull] Func<DateTime, string> dateTimeFormatter = null,
//			bool isVersioning = false)
//		{
//			if (!Directory.Exists(logDirectory))
//				throw new DirectoryNotFoundException(logDirectory);
//			LogDirectory = logDirectory;
//			LogFileBaseName = logFileBaseName;
//			MaxLogFileSizeKb = maxLogFileSizeKb;
//			MaxLogFiles = maxLogFiles;
//			DateTimeFormatter = dateTimeFormatter;
//			IsVersioning = isVersioning;
//			SizeCheckFrequency = sizeCheckFrequency;
//			m_FileSearchPattern = LogFileBaseName + "*" + m_LogfileExtensionWithDot;
//			m_StorageService
//				= PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFStorageService>();

//			// Initialize an empty container for use as a lock object.
//			m_StreamAndWriterContainer = new StreamAndWriterAtomicContainer();

//			// This will initialize us to write to the correct file.
//			OpenOrCreateLog(isVersioning);
//		}

//		#endregion // Constructors

//		#region Methods
//		/// <summary>
//		/// This method creates a filename based on the <paramref name ="dateTime"/> passed
//		/// in. It uses the <see cref="DateTimeFormatter"/> if one is plugged.
//		/// </summary>
//		/// <param name="logFileBaseName">
//		/// The base file name we want to put date/time stuff on the end of. No
//		/// extension, please.
//		/// </param>
//		/// <param name="extensionWithDot">
//		/// The file extension which already has a dot at the beginning.
//		/// </param>
//		/// <param name="dateTime">Date/Time for the name.</param>
//		/// <param name="version">
//		/// Non-null if we are versioning. See <see cref="CurrentVersionNumber"/>.
//		/// </param>
//		/// <returns>The completed file name.</returns>
//		[NotNull]
//		protected virtual string CreateFileName(string logFileBaseName,
//			string extensionWithDot, DateTime dateTime, int? version = null)
//		{
//			var fileName = logFileBaseName + PlatformUtils.GetDirectorySeparatorChar() + logFileBaseName;
//			string dateTimeString;

//			if (DateTimeFormatter != null)
//				dateTimeString = DateTimeFormatter(dateTime);
//			else
//				dateTimeString = string.Format(m_DefaultDateTimeFormatString, dateTime);

//			fileName += dateTimeString;

//			if (version.HasValue)
//				fileName += "@" + version.Value;

//			fileName += extensionWithDot;

//			return fileName;
//		}

//		/// <summary>
//		/// This method can be called at any time to put the writer and the set of files
//		/// in a proper state. It must be called at class initialization and must be
//		/// called periodically to see if the current file is getting too large.
//		/// </summary>
//		protected internal virtual void CheckAndRoll()
//		{
//			if (m_NumWrites % SizeCheckFrequency == 0)
//			{
//				lock (_rotateLogFileLock)
//				{
//					try
//					{
//						if ((MaxLogFileSizeKb > 0) && (GetFileSizeInKb() >= MaxLogFileSizeKb))
//						{

//							FileStream newLogFile = null;
//							StreamWriter newLogFileWriter = null;

//							OpenOrCreateLog(LogFileBaseName, out newLogFile, out newLogFileWriter);

//							if ((newLogFile != null) && (newLogFileWriter != null))
//							{

//								if (_logFileWriterPrev != null)
//								{
//									_logFileWriterPrev.Dispose();
//								}

//								if (_logFilePrev != null)
//								{
//									_logFilePrev.Dispose();
//								}


//								Interlocked.Exchange(ref _logFile, newLogFile);
//								Interlocked.Exchange(ref _logFileWriter, newLogFileWriter);

//								CleanupLogs();
//							}
//						}
//					}
//					catch (Exception exc)
//					{
//						// If we have an exception here, we should log it and then disable this logger.
//						var logger = PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFLoggingService>();
//						logger.LogEntry("RollingLoggerError" + MethodBase.GetCurrentMethod().Name, PAFLoggingLevel.Error, exc);
//						m_IsDisabled = true;
//					}

//				}
//			}
//		}

//		protected internal virtual void CleanupLogs()
//		{

//			DirectoryInfo dir;
//			FileInfo[] files;
//			// Check if we need to clean up some of the log files.
//			if (MaxLogFiles >= 0)
//			{
//				// Zero retained means just leave the most current log file.
//				try
//				{
//					dir = new DirectoryInfo(LogDirectory);
//					files = dir.GetFiles(m_FileSearchPattern);

//					// Get the files sorted.
//					var timeSortedFiles = new Dictionary<DateTime, FileInfo>();

//					Array.Sort(files,
//						(FileInfo file1, FileInfo file2) => { return file1.CreationTime.CompareTo(file2.CreationTime); });
//					if (files.Length > _maxNumOfLogsRetained + 1)
//					{
//						int numFilesToDelete = files.Length - (_maxNumOfLogsRetained + 1);
//						for (int i = 0; i < numFilesToDelete; ++i)
//						{
//							files[i].Delete();
//						}
//					}
//				}
//				catch (Exception exc)
//				{
//					// If we have an exception here, we should log it and then disable this logger.
//					var logger = PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFLoggingService>();
//					logger.LogEntry("RollingLoggerError" + MethodBase.GetCurrentMethod().Name, PAFLoggingLevel.Error, exc);
//					m_IsDisabled = true;
//				}
//			}

//		}

//		protected internal virtual int GetFileSizeInKb()
//		{
//			long bytes;
//			lock (m_StreamAndWriterContainer)
//			{
//				bytes = _logFile.Length;
//			}

//			var kiloBytes = (int) (bytes / (1024));
//			return kiloBytes;
//		}


//		/// <summary>
//		/// Gets the last file written in the directory.
//		/// </summary>
//		/// <returns>
//		/// 
//		/// </returns>
//		protected internal virtual StreamAndWriterAtomicContainer GetLastFile()
//		{
//			lock (m_StreamAndWriterContainer)
//			{
//				// If we are not starting, return current file stream/writer.
//				if (m_StreamAndWriterContainer.LogStreamWriter != null)
//					return m_StreamAndWriterContainer;
//			}

//		}
//		protected internal virtual string GetLogFilePath(string logFileBaseName, string ext, bool overWriteExisting)
//		{
//			var dateTime = DateTime.Now;
//			var path = CreateFileName(logFileBaseName, ext, dateTime);
//			int instance = 2;
//			while (!overWriteExisting && File.Exists(path))
//			{
//				path = CreateFileName(logFileBaseName, ext, dateTime, instance);
//				instance++;
//			}

//			return path;
//		}


//		/// <summary>
//		/// Filters the list of files found in a directory by "version number".
//		/// See <see cref="CurrentVersionNumber"/> for details about how the
//		/// versioned file names are formed.
//		/// </summary> 
//		/// <param name="files">
//		/// Set of files to be filtered. <see langword="null"/> returns
//		/// an empty list.
//		/// </param>
//		/// <param name="versionNumber">
//		/// The version number. Specify -1 to gather ALL versioned files.
//		/// Useful for gathering versioned files for deletion. <see langword="null"/>
//		/// will return unversioned files.
//		/// </param>
//		/// <returns>A list - never <see langword="null"/>.</returns>
//		[NotNull]
//		protected internal virtual IList<FileInfo>
//			GetVersionedFiles([CanBeNull] IEnumerable<FileInfo> files, int? versionNumber)
//		{
//			var retval = new List<FileInfo>();
//			if (files == null)
//				return retval;

//			foreach (var file in files)
//			{
//				var fileName = file.Name;

//				// See if we're even a log file.
//				if (!fileName.Contains(m_LogfileExtensionWithDot))
//					continue;
//				var dotIndex = fileName.LastIndexOf('.');

//				// Check for a valid file. User could have no time stamp or version.
//				if (dotIndex == 0)
//					continue;

//				var atSignIndex = fileName.LastIndexOf('@');
//				if (atSignIndex < 1)
//				{
//					// If we were looking for unversioned files, add it.
//					retval.Add(file);

//					// Non-versioned weird file name.
//					continue;
//				}

//				// We don't deal with the case where someone
//				// purposefully named files to crash us.
//				// This loop should get us the number otherwise.
//				var charIndex = dotIndex-1;
//				while (charIndex > atSignIndex)
//				{
//					if (!StringParsingUtils.IsANumber(fileName[charIndex]))
//						break;
//					charIndex--;
//				}

//				if (charIndex != atSignIndex)
//				{
//					// No proper version number - found no numeric digits.
//					// If we were looking for unversioned files, add it.
//					if (!versionNumber.HasValue)
//					{
//						retval.Add(file);
//					}
//					continue;
//				}

//				if (versionNumber == -1)
//				{
//					// We don't care about the version numebr, as long as it's versioned.
//					retval.Add(file);
//					continue;
//				}

//				var fileVersionNumberString = fileName.Substring(atSignIndex + 1, (dotIndex - 1) - atSignIndex);

//				var fileVersionNumber = int.Parse(fileVersionNumberString);

//				if(fileVersionNumber == versionNumber)
//					retval.Add(file);
//			}

//			return retval;
//		}

//		/// <summary>
//		/// This method opens the last logfile created in the directory (if any) and
//		/// uses it if it is not full. If it is full, then it checks to see if there
//		/// are already too many files in the directory and calls 
//		/// </summary>
//		protected internal void OpenOrCreateLog(bool overWriteExisting = false)
//		{
//			var stream = GetLastFile();
//			if ((stream != null) && (stream.Length < MaxLogFileSizeKb))
//			{
//				stream.Position = stream.Length - 1;
//				lock (m_LogStreamLock)
//				{
//					LogStream = stream;
//				}
//			}

//			string ext = ".log";
//			Stream logStream = null;
//			StreamWriter logStreamWriter = null;

//			int pos = LogFileBaseName.LastIndexOf('.');
//			if (pos > 0)
//			{
//				ext = LogFileBaseName.Substring(pos);
//				LogFileBaseName = LogFileBaseName.Substring(0, pos);
//			}

//			// Need to tack on terminator.
//			var logPath = FileUtils.EnsureDirTerm(LogDirectory);


//			try
//			{
//				string path = GetLogFilePath(LogFileBaseName, ext, overWriteExisting);
//				logStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
//				logStreamWriter = new StreamWriter(logStream);
//			}
//			catch (Exception exc)
//			{
//				// If we have an exception here, we should log it and then disable this logger.
//				var logger = PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFLoggingService>();
//				logger.LogEntry("RollingLoggerError" + MethodBase.GetCurrentMethod().Name, PAFLoggingLevel.Error, exc);
//				m_IsDisabled = true;

//				logStreamWriter?.Dispose();
//				logStream?.Dispose();
//			}
//		}

//		/// <summary>
//		/// Writes the message to the log.
//		/// </summary>
//		/// <param name="message">Message text</param>
//		public virtual void WriteLine(string message)
//		{
//			try
//			{
//				if (null != _logFileWriter)
//				{
//					if ((null == category) || (category.Length == 0))
//					{
//						category = DefaultCategory;
//					}

//					int level = 0;
//					char x = category[category.Length - 1];
//					if (Char.IsDigit(x))
//					{
//						level = x - '0';
//					}

//					if (level <= _maxLevel)
//					{
//						long numberOfWrites = Interlocked.Increment(ref _numberOfWrites);
//						RotateLogFileIfNecessary(numberOfWrites);

//						_logFileWriter.WriteLine("[{0,4:x}] {1,18} {2,8}: {3}",
//							Thread.CurrentThread.ManagedThreadId,
//							DateTime.Now.ToString("MM/dd HH:mm:ss:fff"),
//							category,
//							message);
//						_logFileWriter.Flush();
//					}
//				}
//			}
//			catch
//			{
//				// Failed to write to log file => ignore. The .NET DefaultTraceListener still outputs the message.
//				// An IO exception can happen for instance during process termination if there is a call to WriteLine
//				// after the log file has already been closed.
//			}
//		}

//		/// <summary>
//		/// Writes a message to the log
//		/// </summary>
//		/// <param name="message">Message text</param>
//		/// 
//		public void Write(string message)
//		{
//			try
//			{
//				if (null != _logFileWriter)
//				{
//					long numberOfWrites = Interlocked.Increment(ref m_numberOfWrites);
//					RotateLogFileIfNecessary(numberOfWrites);

//					_logFileWriter.Write(message);
//					_logFileWriter.Flush();
//				}
//			}
//			catch
//			{
//				// Failed to write to log file => ignore.
//			}
//		}

//		protected virtual void Dispose(bool disposing)
//		{
//			if (!m_IsDisposed)
//			{
//				if (disposing)
//				{
//					if (_logFileWriter != null)
//					{
//						_logFileWriter.Dispose();
//					}

//					if (_logFile != null)
//					{
//						_logFile.Dispose();
//					}

//					if (_logFileWriterPrev != null)
//					{
//						_logFileWriterPrev.Dispose();
//					}

//					if (_logFilePrev != null)
//					{
//						_logFilePrev.Dispose();
//					}
//				}

//				m_IsDisposed = true;
//			}
//			#endregion // Methods
//		}

//		/// <summary>
//		/// A little container for the stream and writer so
//		/// they can be synchronized atomically. This is designed as a
//		/// mutable type with props that are forced to be set
//		/// at the same time - just makes things
//		/// organized.
//		/// </summary>
//		public class StreamAndWriterAtomicContainer: IDisposable
//		{
//			#region Fields and Autoprops
//			/// <summary>
//			/// Prop for the stream.
//			/// </summary>
//			/// <remarks>
//			/// Internal set for testing.
//			/// </remarks>
//			public Stream LogStream { get; internal set; }

//			/// <summary>
//			/// Prop for the stream writer.
//			/// </summary>
//			/// <remarks>
//			/// Internal set for testing.
//			/// </remarks>
//			public StreamWriter LogStreamWriter { get; internal set; }
//			#endregion // Fields and Autoprops
//			#region Methods
//			/// <summary>
//			/// Method sets both stream and writer at the same time.
//			/// </summary>
//			/// <param name="logStream">Not <see langword="null"/>.</param>
//			/// <param name="logStreamWriter">Not <see langword="null"/>.</param>
//			/// <exceptions>
//			/// <exception cref="ArgumentNullException">"logStream"</exception>
//			/// <exception cref="ArgumentNullException">"logStreamWriter"</exception>
//			/// </exceptions>
//			public virtual void SetStreamAndWriter([NotNull] Stream logStream, [NotNull] StreamWriter logStreamWriter)
//			{
//				LogStream = logStream ?? throw new ArgumentNullException(nameof(logStream));
//				LogStreamWriter = logStreamWriter ?? throw new ArgumentNullException(nameof(logStreamWriter));
//			}
//			#endregion // Methods

//			#region Disposal Pattern
//			/// <remarks>
//			/// Standard disposal pattern.
//			/// </remarks>
//			protected virtual void Dispose(bool disposing)
//			{
//				if (disposing)
//				{
//					LogStream?.Dispose();
//					LogStreamWriter?.Dispose();
//				}
//			}

//			/// <remarks>
//			/// Standard disposal pattern.
//			/// </remarks>
//			public void Dispose()
//			{
//				Dispose(true);
//				GC.SuppressFinalize(this);
//			}
//			#endregion // Disposal Pattern
//		}
//	}
//}
