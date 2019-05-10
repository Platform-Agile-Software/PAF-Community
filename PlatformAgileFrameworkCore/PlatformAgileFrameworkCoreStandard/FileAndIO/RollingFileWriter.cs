//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.Properties;
#region Exception Shorthand
// ReSharper disable IdentifierTypo
using IPAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFFileAndDirectoryIOExceptionData;
using PAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFFileAndDirectoryIOExceptionData;
// ReSharper restore IdentifierTypo
#endregion // Exception Shorthand
#endregion // Using Directives


namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// <para>
	/// Standard rotating text file manager. It solves the problem of ever-increasing
	/// text file size. It manages a finite set of text files of finite size that maintain
	/// a "time-window" of written information. New files are created and old files are
	/// deleted as time goes on. This writer collapses to using a single ever-increasing
	/// file with appropriate settings of constructor arguments.
	/// </para>
	/// <para>
	/// A key assumption in this base class is that
	/// this writer has exclusive access to a directory where it writes. Access to our
	/// class of files SHOULD BE only by this writer. Our class of files is filtered by
	/// name, but it's best to have a directory solely devoted to our files.
	/// </para>
	/// <para>
	/// Dispatching writer is a specialized writer and should be stapled in
	/// at the app level, typically as a static, if the public method
	/// <see cref="DispatchFilesIfNeededPV"/> needs to be accessed. since it is intentionally
	/// not part of the public <see cref="IPAFFileWriter"/> interface.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 02jan2019 </date>
	/// <description>
	/// Adjusted just a bit more for Golea, by changing file size granularity
	/// to bytes instead of kilobytes. Also made most functionality pluggable.
	/// It was the only way to support multiple client requirements.
	/// </description>
	/// </contribution>
	/// <author> Brian T. </author>
	/// <date> 21nov2018 </date>
	/// <description>
	/// New. Refactored from the loggers, since we needed something that just
	/// did file write.
	/// </description>
	/// </history>
	/// <threadsafety>
	/// Thread-safe by locking on OUR files in directory. No concurrency within this class.
	/// </threadsafety>
	/// <remarks>
	/// This implementation uses the Golea filename format: <c>BASEFILENAME_yyyy-MM-dd_HH-mm-ss-zzz_(n).ext</c>,
	/// where n represents the version change the default plugin <see cref="PAFFilenameStamperAndParser"/> if something
	/// different is needed.
	/// </remarks>
	public class RollingFileWriter : PAFService, IPAFFileWriter, IPAFFileDispatcher
	{
		#region Fields and AutoProperties
		/// <summary>
		/// This is a plugin that was added to support dispatching files to a
		/// remote sink. Without it, oldest file is just deleted. The list is of
		/// all the files of the current version, oldest first.
		/// </summary>
		/// <remarks>
		/// Note that a lock is taken on the writer while this operation is
		/// in progress. Typical procedure is to move files to another directory
		/// if eventual dispatch is to a remote service. Plugin should do
		/// any deletions in the main file directory before returning to
		/// ensure thread safety.
		/// </remarks>
		protected internal Action<IList<string>> OptionalFileDispatcher { get; set; }
		/// <summary>
		/// Current version number is initialized to <see langword="null"/> on
		/// construction of an instance and set to <see cref="int.MinValue"/>
		/// if versioning is off.
		/// </summary>
		private int? m_CurrentVersionNumber = null;

		/// <summary>
		/// Determines whether we are "Versioning" the sets of rolling log
		/// files. Sometimes we don't want to overwrite log files from previous runs.
		/// In that case, we append 0 or 1 or 2, etc., so logfile names are
		/// something like ..._(0).log, ..._(1).log, etc. This class makes no provision
		/// for cleaning old versions out. This functionality is deemed
		/// application-specific. Versioned files
		/// are meant primarily for testing. Note that an INDEPENDENT
		/// rolling logger can also be created for testing.
		/// This will be <see cref="int.MinValue"/> if not versioning.
		/// This number is set automatically during program operation
		/// by scanning for existing versioned file sets if
		/// <see cref="IsVersioning"/> is true. Version number cannot
		/// be changed during operation of an application. Changing the
		/// version number during an application run is application-specific
		/// and we want to force clients to subclass to do this.
		/// </summary>
		public virtual int? CurrentVersionNumber
		{
			get
			{
				lock (m_VersionLockObject)
				{
					// If it's null, then we need to discover it from the files.
					if (m_CurrentVersionNumber == null)
					{
						var nullableVersionNumber = GetCurrentVersionNumberFromFiles();

						if (nullableVersionNumber == null)
							FilenameStamperAndParser.FileVersion = 0;
						else
							// Bump it by one if we found versioned files.
							FilenameStamperAndParser.FileVersion = nullableVersionNumber.Value + 1;

						m_CurrentVersionNumber = FilenameStamperAndParser.FileVersion;
					}
					// No versioning?
					else if (m_CurrentVersionNumber == int.MinValue)
						return null;

					return m_CurrentVersionNumber;
				}
			}

			protected internal set
			{
				lock (m_VersionLockObject)
				{
					m_CurrentVersionNumber = value;
				}
			}
		}


		/// <summary>
		/// This is the base file name which is somehow modified if versioning
		/// or date stamping is used.
		/// </summary>
		protected internal readonly string m_FileBaseName;

		/// <summary>
		/// This is the directory in which files are placed. This MAY be a symbolic
		/// directory.
		/// </summary>
		protected internal string FileDirectory { get; set; }

		/// <summary>
		/// This is the file extension, without dot.
		/// </summary>
		protected internal readonly string m_FileExtension;

		/// <summary>
		/// We are contending for a set of files. We also need to use the lock for
		/// all file creation/deletion operations.
		/// </summary>
		protected readonly object m_FileLockObject = new object();

		/// <summary>
		/// The filename processor.
		/// </summary>
		public IPAFFilenameStamperAndParser FilenameStamperAndParser { get; protected internal set; }
		/// <summary>
		/// Determines whether we are "Versioning" the sets of rolling log
		/// files. Sometimes we don't want to overwrite log files from previous runs.
		/// In that case, we append 1 or 2 or 3, etc., so logfile names are
		/// something like ..._(0).log, ..._(1).log, etc. This class makes no provision
		/// for cleaning old versions out. If versioning if off, we don't touch or
		/// generate any versioned files. If it's on, we start generating new versions
		/// higher than the last version number we find in the directory.
		/// </summary>
		protected internal bool IsVersioning { get; set; }

		/// <summary>
		/// This is the maximum size of each file, in bytes. The default is 10000000, or 1 MB.
		/// </summary>
		protected internal long MaxFileSizeInBytes { get; set; }

		/// <summary>
		/// Defines the maximum number of files kept in the set of rolling files.
		/// </summary>
		protected internal int MaxFiles { get; set; }

		/// <summary>
		/// m_NumWrites is reset to 0 when it reaches this value. This
		/// is so we don't have to check file size on every single write.
		/// A value of <see cref="int.MaxValue"/> ensures initial file
		/// will be used through the life of the application, thus collapsing
		/// the class into a single file writer. Set to <c>1</c> for Golea.
		/// </summary>
		protected internal int SizeCheckFrequency { get; internal set; }

		/// <summary>
		/// This is the atomic tally of the current number of writes
		/// that have been made. GE 0 and LT SizeCheckFrequency. Should
		/// be read only outside this base class.
		/// </summary>
		protected internal int m_NumWrites;

		/// <summary>
		/// Storage service constructed lazily.
		/// </summary>
		protected internal IPAFStorageService m_StorageService;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_OutputFileName;
		/// <summary>
		/// Added to allow sub-classing to rolling logger.
		/// </summary>
		protected virtual long FileLengthInBytes
		{
			get
			{
				lock (m_FileLengthInBytesLockObject)
				{
					return m_FileLengthInBytes;
				}
			}
			set
			{
				lock (m_FileLengthInBytesLockObject)
				{
					m_FileLengthInBytes = value;
				}
			}
		}
		/// <summary>
		/// Need the backing field for access in a constructor.
		/// </summary>
		protected internal long m_FileLengthInBytes;
		/// <summary>
		/// Lock on a data type longer than 32 bits.
		/// </summary>
		protected readonly object m_FileLengthInBytesLockObject = new object();
		/// <summary>
		/// Lock for the field, which is fiddled with.
		/// </summary>
		private readonly object m_VersionLockObject = new object();

		/// <summary>
		/// This is the string preceding the version. Default = <c>"_"</c>.
		/// </summary>
		protected internal readonly string m_VersionToken;
		#endregion // Fields and AutoProperties

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fileDirectory">
		/// File path (not including file name). This may be a symbolic directory.
		/// Not <see langword ="null"/>.
		/// </param>
		/// <param name="fileBaseNameWithExtension">
		/// File base name. This file must NOT have date/time information already
		/// added or a version number. It MUST have an extension. This is name
		/// that will be embellished with date/time/version information if needed.
		/// Not <see langword ="null"/>.
		/// </param>
		/// <param name="maxFileSizeInBytes">
		/// The maximum size of each file. Default is 10000000 (10 MB).
		/// </param>
		/// <param name="maxFiles">
		/// The maximum number of files retained in the directory. Default is 10.
		/// </param>
		/// <param name="sizeCheckFrequency">
		/// This is the number of writes that occur before file size checking and
		/// potential file rotation. Sets <see cref="SizeCheckFrequency"/>.
		/// </param>
		/// <param name="filenameStamperAndParser">
		/// Formatter plugin. Default = <see langword="null"/> causes <see cref="PAFFilenameStamperAndParser"/>
		/// to be instantiated and used.
		/// </param>
		/// <param name="fileDispatcher">
		/// Loads <see cref="OptionalFileDispatcher"/>.
		/// </param>
		/// <param name="isVersioning">
		/// If <see langword="false"/> versioned files are neither generated or accessed.
		/// </param>
		/// <param name="versionToken">
		/// This is the character that is placed after the date and before the version number
		/// in the filename if versioning is in use. Default = <c>_</c>. Not used in this
		/// implementation, since we use the Golea format, which is 
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentException">
		/// <paramref name="fileDirectory"/> <c>"DirectoryNotFound"</c>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="fileBaseNameWithExtension"/> <c>"No filename extension"</c>
		/// </exception>
		/// </exceptions>
		public RollingFileWriter([NotNull]string fileDirectory, [NotNull]string fileBaseNameWithExtension,
			long maxFileSizeInBytes = 10000000, int maxFiles = 10,
			int sizeCheckFrequency = 100,
			IPAFFilenameStamperAndParser filenameStamperAndParser = null,
			Action<IList<string>> fileDispatcher = null,
			bool isVersioning = false,
			string versionToken = null)
		{
			if (!StorageService.PAFDirectoryExists(fileDirectory))
				throw new ArgumentException("DirectoryNotFound: " + fileDirectory);
			FileDirectory = fileDirectory;

			if (string.IsNullOrEmpty(FileDirectory))
				FileDirectory = PlatformUtils.ApplicationRoot;

			var dotIndex = fileBaseNameWithExtension.IndexOf(".", StringComparison.Ordinal);
			if (dotIndex < 0)
				throw new ArgumentException("No filename Extension");
			m_FileBaseName = fileBaseNameWithExtension.Substring(0, dotIndex);
			m_FileExtension = fileBaseNameWithExtension.Substring(dotIndex + 1);
			MaxFileSizeInBytes = maxFileSizeInBytes;
			MaxFiles = maxFiles;
			FilenameStamperAndParser = filenameStamperAndParser ?? new PAFFilenameStamperAndParser(m_FileBaseName);
			SizeCheckFrequency = sizeCheckFrequency;
			OptionalFileDispatcher = fileDispatcher;
			IsVersioning = isVersioning;
			if (!IsVersioning)
				m_CurrentVersionNumber = int.MinValue;

			m_VersionToken = versionToken ?? "_";
		}
		/// <summary>
		/// Constructor sets properties to behave as an ordinary logger
		/// with just one output file. This method can have a <see langword="null"/>
		/// or <see cref="string.Empty"/> filename in subclasses.
		/// </summary>
		/// <param name="fileAndDirectoryNameWithExtension">
		/// File base name. It MAY have a directory specification on the front.
		/// If a directory specification exists, the directory MUST exist.
		/// The directory is NOT auto-created. Filename MUST have an extension.
		/// <see langword ="null"/> and <see cref="string.Empty"/> are
		/// OK, since subclasses don't necessarily write to a file passed
		/// in through a constructor.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentException">
		/// <paramref name="fileAndDirectoryNameWithExtension"/>
		/// <c>"No filename extension"</c> if incoming name is not vacuous and
		/// file has no extension.
		/// </exception>
		/// </exceptions>
		protected internal RollingFileWriter(string fileAndDirectoryNameWithExtension)
		{

			var dirAndFile = PAFFileUtils.SeparateDirectoryFromFile(fileAndDirectoryNameWithExtension);
			if(dirAndFile == null)
				dirAndFile = new string[2];

			FileDirectory = dirAndFile[0];
			if (string.IsNullOrEmpty(FileDirectory))
				FileDirectory = PlatformUtils.ApplicationRoot;
			var fileNameWithExtension = dirAndFile[1];

			if (!string.IsNullOrEmpty(fileNameWithExtension))
			{
				var dotIndex = fileNameWithExtension.IndexOf(".", StringComparison.Ordinal);
				if (dotIndex < 0)
					throw new ArgumentException("No filename Extension");
				m_FileBaseName = fileNameWithExtension.Substring(0, dotIndex);
				m_FileExtension = fileNameWithExtension.Substring(dotIndex + 1);
			}

			MaxFileSizeInBytes = int.MaxValue;
			MaxFiles = int.MaxValue;
			FilenameStamperAndParser = new PAFFilenameStamperAndParser(m_FileBaseName);
			SizeCheckFrequency = int.MaxValue;
			OptionalFileDispatcher = null;
		}

		#endregion // Constructors
		#region Implementation of IPAFFileWriter
		/// <remarks>
		/// <see cref="IPAFFileWriter"/>
		/// </remarks>
		public virtual void WriteDataEntry(string dataEntry)
		{
			WriteData(dataEntry);
		}
		/// <summary>
		/// Holds current filename. A filename can be attached after construction
		/// by setting it here.
		/// </summary>
		public virtual string OutputFileName
		{
			get
			{
				// Startup guard.
				if (string.IsNullOrEmpty(m_OutputFileName))
					m_OutputFileName = CreateNewFile();
				return m_OutputFileName;
			}
			protected internal set { m_OutputFileName = value; }
		}
		#endregion // Implementation of IPAFFileWriter
		#region Novel Properties
		/// <summary>
		/// Lazy access to storage service, since this service is
		/// not always available in the construction path.
		/// </summary>
		protected internal IPAFStorageService StorageService
		{
			get
			{
				if(m_StorageService == null)
				{
					m_StorageService
						= PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFStorageService>();
				}
				return m_StorageService;
			}
		}

		#endregion // Novel Properties

		#region Novel Methods
		/// <summary>
		/// This method creates a filename based on a <see cref="DateTime"/> and a
		/// version number. It uses the <see cref="FilenameStamperAndParser"/> For an
		/// ordinary rolling file manager like this, date/time is ordinarily
		/// <see cref="DateTime.Now"/>.
		/// </summary>
		/// <returns>
		/// The completed file name based on directory, basename, extension,
		/// and date/time.
		/// </returns>
		/// <remarks>
		/// Note that our default file name generation procedure always puts
		/// milliseconds into the filename. This is to protect against file overwrite
		/// when files are generated less than 1 second apart. 
		/// </remarks>
		[NotNull]
		protected virtual string CreateNewFile()
		{
			var fileName = m_FileBaseName;
			// We must cover the case where the filename does not include a directory.
			if (!string.IsNullOrEmpty(FileDirectory))
			{
				fileName = FileDirectory + PlatformUtils.GetDirectorySeparatorChar()
				                             + m_FileBaseName;
			}

			// Avoid filename collision on fast machines.
			Task.Delay(2).Wait();

			var versionNumber = CurrentVersionNumber;

			FilenameStamperAndParser.FileVersion = versionNumber;

			fileName = FilenameStamperAndParser.StampFilename(fileName);

			fileName += "." + m_FileExtension;

			using (StorageService.PAFOpenFile(fileName, PAFFileAccessMode.REPLACE))
			{
			}

			FileLengthInBytes = 0;
			// ReSharper disable once InconsistentlySynchronizedField
			//// 32-bit load is atomic, ReSharper.
			m_NumWrites = 0;

			return fileName;
		}
		/// <summary>
		/// This method scans files in <see cref="FileDirectory"/> to
		/// determine what versions of files are present.
		/// </summary>
		/// <returns>
		/// <see langword="null"/> if <see cref="IsVersioning"/>
		/// is <see langword="false"/> or no versioned files present.
		/// </returns>
		/// <remarks>
		/// Takes the lock on the directory during operation to get files.
		/// </remarks>
		protected virtual int? GetCurrentVersionNumberFromFiles()
		{
			int? versionNumber;
			if (!IsVersioning)
				versionNumber = null;
			else
			{
				IEnumerable<string> allFiles;
				lock (m_FileLockObject)
				{
					// Collect all files in the directory.
					allFiles = m_StorageService.PAFGetFileNames(FileDirectory);
				}

				// Order into lists.
				var ascendingVersionedFileLists = GetVersionedFiles(allFiles).SortEntriesByAscendingKey();

				// No files is easy....
				if (ascendingVersionedFileLists == null || !ascendingVersionedFileLists.Any())
					versionNumber = null;
				// Return null if no versioned files present.
				else if (ascendingVersionedFileLists[0].Key == -1)
					versionNumber = null;
				// If there are versioned files present, return the highest version #.
				else
					versionNumber = ascendingVersionedFileLists.Last().Key;
			}

			return versionNumber;
		}


		/// <summary>
		/// Provides a dictionary of versioned files. Key is the version number, which
		/// is -1 for un-versioned files. Versioned files are used often for testing,
		/// so they should be cleaned out after use in this case. Otherwise, the operation
		/// of this method is somewhat slow.
		/// </summary> 
		/// <param name="files">
		/// Set of files to be analyzed. <see langword="null"/> returns
		/// an empty dictionary.
		/// </param>
		/// <returns>A dictionary - never <see langword="null"/>.</returns>
		/// <remarks>
		/// Files that are not <see cref="IsValidFile"/> are ignored.
		/// </remarks>
		[NotNull]
		protected virtual IDictionary<int, IList<string>>
			GetVersionedFiles([CanBeNull] IEnumerable<string> files)
		{
			var versionedFileDictionary = new Dictionary<int, IList<string>>();
			if (files == null)
				return versionedFileDictionary;

			foreach (var fileName in files)
			{
				// Skip invalid files.
				if (!IsValidFile(fileName))
					continue;

				// Grab the version.
				////
				var fileVersionNumber = GetVersionNumber(fileName);

				// Deal with the edge case.
				////
				if (fileVersionNumber != null)
				{
					versionedFileDictionary.AddToListValue(fileVersionNumber.Value, fileName);
				}
			}

			return versionedFileDictionary;
		}
		/// <summary>
		/// Parses a filename to extract the version number. Filename must have a dot
		/// and extension and can contain a full path specification. The
		/// <see cref="FilenameStamperAndParser"/> does the job.
		/// </summary> 
		/// <param name="fileName">
		/// File name to be analyzed With or without directory spec..
		/// <see langword="null"/> returns <see langword="false"/>.
		/// </param>
		/// <returns>
		/// -1 if file is not versioned.
		/// <see langword="null"/> if file does not contain our <see cref="m_FileBaseName"/>,
		/// <see langword="null"/> if file does not contain a dot and extension,
		/// thus not conforming to our template.
		/// </returns>
		[SuppressMessage("ReSharper", "CommentTypo")]
		protected virtual int? GetVersionNumber([CanBeNull] string fileName)
		{
			if (!IsValidFile(fileName))
				return null;

			fileName = PAFFileUtils.KillExtension(fileName);

			// Plugged stamper/parser takes care of the whole thing.
			return FilenameStamperAndParser.ParseFilename(fileName).FileVersion;
		}

		/// <remarks>
		/// Explicit implementation of the interface to emphasize
		/// the <see cref="IPAFFileDispatcher"/> usage pattern. Hopefully
		/// this will help avoid programming errors. Calls
		/// <see cref="GetDisposableLockPV"/>.
		/// </remarks>
		IDisposable IPAFFileDispatcher.GetDisposableLock()
		{
			return GetDisposableLockPV();
		}
		/// <summary>
		/// Calling this method will suspend all file and directory operations
		/// performed by this <see cref="RollingFileWriter"/> until
		/// the returned <see cref="IDisposable"/> is disposed.
		/// </summary>
		/// <returns>
		/// The <see cref="IDisposable"/>, which is normally accessed
		/// in a using block to promote thread safety.
		/// </returns>
		/// <remarks>
		/// Protected virtual implementation of the interface.
		/// </remarks>
		protected virtual IDisposable GetDisposableLockPV()
		{
			return new LockBroker(this);
		}
		/// <remarks>
		/// Explicit implementation of the interface to emphasize
		/// the <see cref="IPAFFileDispatcher"/> usage pattern. Hopefully
		/// will help avoid programming errors. Calls
		/// <see cref="DispatchFilesIfNeededPV"/>.
		/// </remarks>
		void IPAFFileDispatcher.DispatchFilesIfNeeded()
		{
			DispatchFilesIfNeededPV();
		}

		/// <summary>
		/// This method "dispatches" files when there are too many in
		/// the rolling file directory. It is called automatically when
		/// writing to a file causes a new file to be created and
		/// the number of files in the file directory exceeds or equals
		/// the limit on number of files. When called automatically, it
		/// must be called PRIOR to the creation of a new file. If an
		/// optional dispatcher is plugged, it takes over the decision
		/// about whether file dispatch is needed. It can be called at
		/// the end of a given writing session to transmit ALL files to
		/// a server or something similar. Note that the writer is locked
		/// during this operation, so this should be a fast operation,
		/// such as moving files to another directory for final
		/// processing. This method is intentionally synchronous.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method requires file writing and all other operations on
		/// "our" files to be suspended on all other threads. For our internal
		/// operations, a <see cref="Monitor"/> lock is taken on a lock object.
		/// The same object is locked by an external client with the
		/// <see cref="IPAFFileDispatcher.GetDisposableLock"/> method.
		/// This lock must be taken by any external caller before calling
		/// this method.
		/// </para>
		/// <para>
		/// Protected virtual implementation of the interface.
		/// </para>
		/// </remarks>
		protected virtual void DispatchFilesIfNeededPV()
		{
			// Collect all files in the directory.
			var allFileNames = StorageService.PAFGetFileNames(FileDirectory).ToList();

			var ourFiles = GetValidFiles(allFileNames);

			// Are there any files of "our type" in the directory?
			if (ourFiles.SafeCount() <= 0) return;

			// Get list of files in ascending date/time order.
			var ascendingDateTimeFiles
				= GetFileDateTimes(ourFiles).SortValuesByAscendingKey();

			// Any files to dispatch?
			if (ascendingDateTimeFiles.SafeCount() == 0) return;

			// If we have a customized way to deal with too many files, use it.
			if (OptionalFileDispatcher != null)
			{
				OptionalFileDispatcher(ascendingDateTimeFiles.ToList());
			}

			else if (ascendingDateTimeFiles.Count >= MaxFiles)
			{
				// Delete least-recent file.
				StorageService.PAFDeleteFile(ascendingDateTimeFiles[0]);
			}
		}
		/// <summary>
		/// Just figures out which files in the directory are ours. If
		/// possible, make a separate directory for this writerer, so this is easy.
		/// Client requirements dictate we build it this way.
		/// </summary> 
		/// <param name="fileNames">
		/// Set of files to be analyzed. <see langword="null"/> returns
		/// an empty dictionary.
		/// </param>
		/// <returns>A list of valid files.</returns>
		/// <remarks>
		/// Files that are not <see cref="IsValidFile"/> are ignored.
		/// </remarks>
		[NotNull]
		protected virtual IList<string>
			GetValidFiles(IEnumerable<string> fileNames)
		{
			var validFileNames = new List<string>();
			if (fileNames == null)
				return validFileNames;

			foreach (var fileName in fileNames)
			{
				// Skip invalid files.
				if (!IsValidFile(fileName))
					continue;

				validFileNames.Add(fileName);
			}

			return validFileNames;
		}

		/// <summary>
		/// Parses a filename to extract the creation <see cref="DateTime"/>.
		/// </summary> 
		/// <param name="fileName">
		/// File name to be analyzed with or without directory spec.
		/// <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if file does not contain a date/time.
		/// <see langword="null"/> if file does not contain a dot and extension,
		/// thus not conforming to our template.
		/// <see langword="null"/> if filename cannot be parsed. Parse file name
		/// before passing if parsing diagnosis is needed. (Not our job).
		/// </returns>
		protected virtual DateTime? GetDateTime([CanBeNull] string fileName)
		{
			// Verify that file is proforma.
			if (!IsValidFile(fileName))
				return null;

			fileName = PAFFileUtils.KillExtension(fileName);

			// Do it with the plugin.
			return FilenameStamperAndParser.ParseFilename(fileName).FileDateTime;
		}
		/// <summary>
		/// This method calls <see cref="GetDateTime"/> on a list of files to extract the creation
		/// <see cref="DateTime"/> for each.
		/// </summary>
		/// <param name="fileNames">List of file names to process.</param>
		/// <returns>
		/// Dictionary of file name values, keyed by date.
		/// </returns>
		[NotNull]
		protected virtual IDictionary<DateTime, string> GetFileDateTimes([CanBeNull] IList<string> fileNames)
		{
			var datedFileDictionary = new Dictionary<DateTime, string>();
			if (fileNames == null)
				return datedFileDictionary;
			foreach (var fileName in fileNames)
			{
				// Skip invalid files.
				if (!IsValidFile(fileName))
					continue;

				// Grab the DateTime.
				////
				var fileDate = GetDateTime(fileName);

				// Deal with the edge case.
				////
				if (fileDate != null)
				{
					datedFileDictionary.Add(fileDate.Value, fileName);
				}
			}

			return datedFileDictionary;
		}


		/// <summary>
		/// Parses a filename to determine if it is valid for consideration
		/// as one of our files. It must have the base file name embedded
		/// and an extension after a dot with no other embedded dots.
		/// </summary> 
		/// <param name="fileName">
		/// File name to be analyzed. <see langword="null"/> returns
		/// <see langword="false"/>. This is file name only, without directory.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if file does not contain our <see cref="m_FileBaseName"/>
		/// at the beginning. <see langword="false"/> if file does not contain a dot and extension,
		/// thus not conforming to our template.
		/// </returns>
		protected virtual bool IsValidFile(string fileName)
		{
			if (fileName == null)
				return false;

			var baseFilenameStartIndex
				= fileName.IndexOf(m_FileBaseName, StringComparison.Ordinal);

			// Not one of us?
			if (baseFilenameStartIndex < 0)
				return false;

			// Check for a valid file. File could have no extension.
			var dotIndex = fileName.LastIndexOf('.');

			// We don't allow just a dot at the end.
			if (dotIndex == fileName.Length - 1)
				return false;

			// Doesn't fit our template?
			return dotIndex > 0;
		}

		/// <summary>
		/// Method to check if there are too many files. If the current file
		/// is within length limits, according to <see cref="MaxFileSizeInBytes"/>,
		/// we exit. If there is room for another file, we create a new one. If
		/// there are too many files, according to <see cref="MaxFiles"/>, we
		/// "dispatch" some. If <see cref="OptionalFileDispatcher"/> is not plugged,
		/// our default dispatch procedure is to delete the oldest file in the
		/// versioned set. Note that excess files are dispatched before a new file
		/// is created so the capacity of the store is not exceeded (Golea).
		/// </summary>
		/// <param name="numberOfWrites">
		/// Number of writes on the current file in the version set.
		/// </param>
		/// <param name="fileLengthInBytes">
		/// Length of the current file.
		/// </param>
		/// <param name="lengthInBytesOfDataAboutToBeWritten">
		/// This is an optional value added for Golea, so we can check that
		/// the overall size of any file segment is not above <see cref="MaxFileSizeInBytes"/>.
		/// This allows pre-checking of the data that will be written.
		/// </param>
		/// <remarks>
		/// This one is not virtual, since there is some danger with the lock being used.
		/// This writer is disabled if even a single attempt is made to overflow the storage
		/// area. Measure the length of the string to be written to ensure that it is not
		/// larger than <see cref="MaxFileSizeInBytes"/> to avoid this.
		/// </remarks>
		/// <threadsafety>
		/// The call to this method should always be done inside the lock on file/directory.
		/// </threadsafety>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFFIOED}">
		/// <see cref="PAFFileAndIOExceptionMessageTags.FILE_OVERFLOW"/>
		/// for writing too much data.
		/// </exception>
		/// </exceptions>
		protected void RotateFilesIfNecessary(int numberOfWrites,
			long fileLengthInBytes,
			long? lengthInBytesOfDataAboutToBeWritten = null)
		{
			// Get out if not time to check.
			// Number of writes is one less when we are about to write,
			// so we pre-check.
			if ((numberOfWrites + 1) < SizeCheckFrequency)
				return;

			// Take into account data about to be written?
			if (lengthInBytesOfDataAboutToBeWritten.HasValue)
				fileLengthInBytes += lengthInBytesOfDataAboutToBeWritten.Value;

			// We can't write more data than we are allowed.
			if ((lengthInBytesOfDataAboutToBeWritten.HasValue)
				 && (lengthInBytesOfDataAboutToBeWritten.Value > MaxFileSizeInBytes))
			{
				var exceptionData = new PAFFIOED(OutputFileName, null, PAFLoggingLevel.Error);
				throw new PAFStandardException<IPAFFIOED>(exceptionData,
					PAFFileAndIOExceptionMessageTags.FILE_OVERFLOW);
			}

			// Get out if current file still OK.
			if (fileLengthInBytes <= MaxFileSizeInBytes)
				return;

			// Possibly dispatch files.
			DispatchFilesIfNeededPV();

			// If we're here, we need a new file.
			OutputFileName = CreateNewFile();
		}

		/// <summary>
		/// Our writer that writes to a file if one is specified.
		/// </summary>
		/// <param name="dataEntry">Entry to write.</param>
		/// <param name="outputFile">
		/// File to write to. <see langword="null"/> writes to
		/// <see cref="OutputFileName"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFFIOED}">
		/// <see cref="PAFFileAndIOExceptionMessageTags.ERROR_WRITING_FILE"/>
		/// for any errors.
		/// </exception>
		/// </exceptions>
		protected virtual void WriteData(string dataEntry, string outputFile = null)
		{
			try
			{
				lock (m_FileLockObject)
				{
					int? bytesAboutToBeWritten = null;
					if ((SizeCheckFrequency == 1) && (dataEntry != null))
						bytesAboutToBeWritten = dataEntry.Length;

					RotateFilesIfNecessary(m_NumWrites, FileLengthInBytes,
						bytesAboutToBeWritten);

					// This has to be after file rolling.
					if (string.IsNullOrEmpty(outputFile))
						outputFile = OutputFileName;

					var storageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
					using (var stream = storageService.PAFOpenFile(outputFile, PAFFileAccessMode.APPEND))
					{
						stream.PAFWriteString(dataEntry);
						FileLengthInBytes = stream.PAFLength;
					}

					Interlocked.Increment(ref m_NumWrites);
				}
			}

			catch (Exception exc)
			{
				var exceptionData = new PAFFIOED(outputFile, exc, PAFLoggingLevel.Error);
				throw new PAFStandardException<IPAFFIOED>(exceptionData,
					PAFFileAndIOExceptionMessageTags.ERROR_WRITING_FILE);
			}
		}
		#endregion // Novel Methods
		/// <summary>
		/// Nested class just to expose a disposable to conveniently
		/// use in a <see langword="using"/> statement to temporarily
		/// hold up the file writer's actions.
		/// </summary>
		protected internal class LockBroker: IDisposable
		{
			/// <summary>
			/// Our access to the outer class.
			/// </summary>
			protected internal RollingFileWriter m_RollingFileWriter;
			/// <summary>
			/// Our constructor just staples in our reference to the outer class.
			/// </summary>
			/// <param name="rollingFileWriter">
			/// Our reference to the outer class.
			/// </param>
			/// <remarks>
			/// The constructor takes a monitor lock on the outer lock
			/// object.
			/// </remarks>
			protected internal LockBroker(RollingFileWriter rollingFileWriter)
			{
				m_RollingFileWriter = rollingFileWriter;
				Monitor.Enter(m_RollingFileWriter.m_FileLockObject);
			}
			/// <summary>
			/// Dispose must always be called to release the lock.
			/// </summary>
			public void Dispose()
			{
				Monitor.Exit(m_RollingFileWriter.m_FileLockObject);
				// null the reference so we can be GC'd.
				m_RollingFileWriter = null;
			}
		}
	}
}

