using System;
using System.Collections.Generic;
using PlatformAgileFramework.FileAndIO.SymbolicDirectories;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Platform;


namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// This implementation typically employs standard stuff from <c>File</c>
	/// and <c>Directory</c>, which are exposed fully on ECMA and now also on
	/// Xamarin.Android and Xamarin.iOS. That is why we have some defaults in
	/// here assuming a wide-open file/directory system.
	/// </summary>
	/// <remarks>
	/// As usual, we employ explicit interface implementation with virtual
	/// backing methods for extenders.
	/// </remarks>
	/// <threadsafety>
	/// Depends on the implementation.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 22apr2015 </date>
	/// <desription>
	/// New. This is the reworked version that caters to the fact that
	/// Xamarin now exposes the file and directory stuff instead of using
	/// the Silverlight model. Seems kind of dangerous...............
	/// </desription>
	/// </contribution>
	/// </history>
	public abstract class PAFStorageServiceAbstract : PAFService, IPAFStorageService
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Saves some code.
		/// </summary>
		// ReSharper disable once IdentifierTypo
		protected internal readonly IPAFStorageService m_AsIstorage;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_ApplicationRootDirectory;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This default constructor just sets some needed fields.
		/// </summary>
		protected PAFStorageServiceAbstract()
		{
			m_AsIstorage = this;
			if (PlatformUtils.ApplicationRoot != null)
			{
				// This must be preset for startup.
				m_ApplicationRootDirectory = PlatformUtils.ApplicationRoot;
			}
		}
		#endregion // Constructors

/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		string IPAFStorageService.ApplicationRootDirectory
		{
			get { return m_ApplicationRootDirectory; }
		}
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		protected virtual string ApplicationRootDirectoryPV
		{
			get { return m_ApplicationRootDirectory; }
		}

		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFAvailableFreeSpace
		{
			get{return PAFAvailableFreeSpacePV;}
		}
		/// <summary>
		/// backing for the interface. Returns max value.
		/// </summary>
		protected virtual long PAFAvailableFreeSpacePV
		{
			get{return long.MaxValue;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFAvailableSize
		{
			get { return PAFAvailableSizePV; }
		}

		/// <summary>
		/// backing for the interface. Returns max value.
		/// </summary>
		protected virtual long PAFAvailableSizePV
		{
			get {return long.MaxValue;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFQuota
		{
			get{return PAFQuotaPV;}
		}
		/// <summary>
		/// backing for the interface. Returns max value.
		/// </summary>
		protected virtual long PAFQuotaPV
		{
			get{return long.MaxValue;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFUsedSize
		{
			get { return PAFUsedSizePV; }
		}
		/// <summary>
		/// backing for the interface. Returns 0.
		/// </summary>
		protected virtual long PAFUsedSizePV
		{
			get{return 0;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		string IPAFStorageService.StorageTag
		{
			get { return StorageTagPV; }
		}
		/// <summary>
		/// backing for the interface.
		/// NO-OP returns "TAG" for base. 
		/// </summary>
		protected virtual string StorageTagPV
		{
			get { return "TAG"; }
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		string IPAFStorageService.GetConvertedFileSpec(string fileSpec)
		{
			return GetConvertedFileOrDirectorySpecPV(fileSpec);
		}
		/// <summary>
		/// backing for the interface.
		/// Gets path with symbolic directories converted.
		/// </summary>
		protected virtual string GetConvertedFileOrDirectorySpecPV(string fileSpec)
		{
			var normalizedPath =  PAFFileUtils.NormalizeFilePathWithDriveOrDirectoryInternal(fileSpec);
			// If not rooted and root dir is forced, use it.
			if (!PAFFileUtils.IsPathSpecRooted(normalizedPath) && !string.IsNullOrEmpty(ApplicationRootDirectoryPV))
			{
				normalizedPath = ApplicationRootDirectoryPV + PlatformUtils.GetDirectorySeparatorChar() + normalizedPath;
			}
			return normalizedPath;
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		string IPAFStorageService.GetMappedDirectorySymbol(string symbolicDirectorySymbol)
		{
			return GetMappedDirectorySymbolPV(symbolicDirectorySymbol);
		}
		/// <summary>
		/// backing for the interface.
		/// Just accesses the static.
		/// </summary>
		protected virtual string GetMappedDirectorySymbolPV(string symbolicDirectorySymbol)
		{
			return SymbolicDirectoryMappingDictionary.GetStaticMappingInternal(symbolicDirectorySymbol);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFCopyFile(string sourceFileName,
			string destinationFileName, bool overwrite)
		{
			PAFCopyFileSymbolicPV(sourceFileName, destinationFileName,overwrite);
		}

		/// <summary>
		/// backing for the interface.
		/// symbolic shim.
		/// </summary>
		protected virtual void PAFCopyFileSymbolicPV(string sourceFileName,
			string destinationFileName, bool overwrite)
		{
			var convertedSourceFileName = GetConvertedFileOrDirectorySpecPV(sourceFileName);
			var convertedDestinationFileName = GetConvertedFileOrDirectorySpecPV(destinationFileName);
			PAFCopyFilePV(convertedSourceFileName, convertedDestinationFileName, overwrite);
		}

		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFCopyFilePV(string sourceFileName,
			string destinationFileName, bool overwrite);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFCreateDirectory(string dir)
		{
			PAFCreateDirectorySymbolicPV(dir);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual object PAFCreateDirectorySymbolicPV(string dir)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPV(dir);
			return PAFCreateDirectoryPV(convertedDir);
		}
		/// <summary>
		/// Backing support for the interface. Since <c>DirectoryInfo</c>
		/// cannot be part of a platform-independent interface, an object
		/// is returned here for any interface extensions to use.
		/// </summary>
		protected abstract object PAFCreateDirectoryPV(string dir);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFCreateDirectory(string dir, object clientObject)
		{
			PAFCreateDirectorySymbolicPV(dir, clientObject);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual object PAFCreateDirectorySymbolicPV(string dir, object clientObject)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPV(dir);
			return PAFCreateDirectoryPV(convertedDir, convertedDir);
		}
		/// <summary>
		/// Backing support for the interface.
		/// client object not used in this base
		/// implementation.
		/// </summary>
		protected abstract object PAFCreateDirectoryPV(string dir, object clientObject);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IPAFStorageStream IPAFStorageService.PAFCreateFile(string path)
		{
			return PAFCreateFileSymbolicPV(path);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual IPAFStorageStream PAFCreateFileSymbolicPV(string path)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPV(path);
			return PAFCreateFilePV(convertedPath);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract IPAFStorageStream PAFCreateFilePV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteDirectory(string dir)
		{
			PAFDeleteDirectorySymbolicPV(dir);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFDeleteDirectorySymbolicPV(string dir)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPV(dir);
			PAFDeleteDirectoryPV(convertedDir);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFDeleteDirectoryPV(string dir);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteDirectory(string dir, object clientObject)
		{
			PAFDeleteDirectorySymbolicPV(dir, clientObject);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFDeleteDirectorySymbolicPV(string dir, object clientObject)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPV(dir);
			PAFDeleteDirectoryPV(convertedDir, clientObject);
		}
		/// <summary>
		/// backing for the interface. Client object not used in this implementation.
		/// </summary>
		protected virtual void PAFDeleteDirectoryPV(string dir, object clientObject)
		{
			PAFDeleteDirectorySymbolicPV(dir);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteFile(string file)
		{
			PAFSymbolicFileSymbolicPV(file);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFSymbolicFileSymbolicPV(string file)
		{
			var convertedFile = GetConvertedFileOrDirectorySpecPV(file);
			PAFDeleteFilePV(convertedFile);
		}

		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFDeleteFilePV(string file);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteFile(string file, object clientObject)
		{
			PAFDeleteFileSymbolicPV(file, clientObject);
		}
		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFDeleteFileSymbolicPV(string file, object clientObject)
		{
			var convertedFile = GetConvertedFileOrDirectorySpecPV(file);
			PAFDeleteFilePV(convertedFile, clientObject);
		}
		/// <summary>
		/// backing for the interface. Client object not used in this implementation.
		/// </summary>
		protected virtual void PAFDeleteFilePV(string file, object clientObject)
		{
			PAFDeleteFilePV(file);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFDirectoryExists(string dir)
		{
			return PAFDirectoryExistsSymbolicPV(dir);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		/// <remarks>
		/// Gate null values in here.
		/// </remarks>
		protected virtual bool PAFDirectoryExistsSymbolicPV(string dir)
		{
			if (string.IsNullOrEmpty(dir))
				return false;
			var convertedDir = GetConvertedFileOrDirectorySpecPV(dir);
			return PAFDirectoryExistsPV(convertedDir);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract bool PAFDirectoryExistsPV(string dir);
		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFFileExists(string path)
		{
			return PAFFileExistsSymbolicPV(path);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		/// <remarks>
		/// Gate null values in here.
		/// </remarks>
		protected virtual bool PAFFileExistsSymbolicPV(string path)
		{
			if (string.IsNullOrEmpty(path))
				return false;
			var convertedPath = GetConvertedFileOrDirectorySpecPV(path);
			return PAFFileExistsPV(convertedPath);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract bool PAFFileExistsPV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		DateTime IPAFStorageService.PAFGetCreationTime(string path)
		{
			return PAFGetCreationTimeSymbolicPV(path);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		/// <remarks>
		/// Gate null values in here.
		/// </remarks>
		protected virtual DateTime PAFGetCreationTimeSymbolicPV(string path)
		{
			if (string.IsNullOrEmpty(path))
				return DateTime.MinValue;
			var convertedPath = GetConvertedFileOrDirectorySpecPV(path);
			return PAFGetCreationTimePV(convertedPath);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract DateTime PAFGetCreationTimePV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetDirectoryNames()
		{
			return PAFGetDirectoryNamesPV();
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetDirectoryNamesPV()
		{
			return PAFGetDirectoryNamesPV(null);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetDirectoryNames(string dirSpec)
		{
			return PAFGetDirectoryNamesSymbolicPV(dirSpec);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetDirectoryNamesSymbolicPV(string dirSpec)
		{
			var convertedDirSpec = GetConvertedFileOrDirectorySpecPV(dirSpec);
			return PAFGetDirectoryNamesPV(convertedDirSpec);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract IEnumerable<string> PAFGetDirectoryNamesPV(string dirSpec);
 /**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetFileNames()
		{
			return PAFGetFileNamesPV();
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetFileNamesPV()
		{
			return PAFGetFileNamesPV(null);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetFileNames(string dirSpec)
		{
			return PAFGetFileNamesSymbolicPV(dirSpec);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetFileNamesSymbolicPV(string dirSpec)
		{
			var convertedDirSpec = GetConvertedFileOrDirectorySpecPV(dirSpec);
			return PAFGetFileNamesPV(convertedDirSpec);
		}
		/// <summary>
		/// Backing support for the interface.
		/// </summary>
		protected abstract IEnumerable<string> PAFGetFileNamesPV(string dirSpec);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>.
		/// </summary>
		DateTime IPAFStorageService.PAFGetLastAccessTime(string path)
		{
			return PAFGetLastAccessTimeSymbolicPV(path);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual DateTime PAFGetLastAccessTimeSymbolicPV(string path)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPV(path);
			return PAFGetLastAccessTimePV(convertedPath);
		}

		/// <summary>
		/// Backing support for the interface.
		/// </summary>
		protected abstract DateTime PAFGetLastAccessTimePV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		DateTime IPAFStorageService.PAFGetLastWriteTime(string path)
		{
			return PAFGetLastWriteTimeSymbolicPV(path);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual DateTime PAFGetLastWriteTimeSymbolicPV(string path)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPV(path);
			return PAFGetLastWriteTimePV(convertedPath);
		}
		/// <summary>
		/// Backing support for the interface.
		/// </summary>
		protected abstract DateTime PAFGetLastWriteTimePV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFIncreaseQuotaTo(long newQuotaSize)
		{
			return PAFIncreaseQuotaToPV(newQuotaSize);
		}
		/// <summary>
		/// backing for the interface.
		/// NO-OP in ECMA. returns <see langword="true"/>.
		/// </summary>
		protected virtual bool PAFIncreaseQuotaToPV(long newQuotaSize)
		{
			return true;
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFIncreaseQuotaTo(long newQuotaSize, object clientObject)
		{
			return PAFIncreaseQuotaToPV(newQuotaSize, clientObject);
		}
		/// <summary>
		/// backing for the interface.
		/// NO-OP in ECMA. returns <see langword="true"/>.
		/// </summary>
		protected virtual bool PAFIncreaseQuotaToPV(long newQuotaSize, object clientObject)
		{
			return true;
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFMoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			PAFMoveDirectorySymbolicPV(sourceDirectoryName, destinationDirectoryName);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFMoveDirectorySymbolicPV(string sourceDirectoryName, string destinationDirectoryName)
		{
			var convertedSourceDirectoryName = GetConvertedFileOrDirectorySpecPV(sourceDirectoryName);
			var convertedDestinationDirectoryName = GetConvertedFileOrDirectorySpecPV(destinationDirectoryName);
			PAFMoveDirectoryPV(convertedSourceDirectoryName, convertedDestinationDirectoryName);

		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFMoveDirectoryPV(string sourceDirectoryName, string destinationDirectoryName);
		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFMoveFile(string sourceFileName, string destinationFileName)
		{
			PAFMoveFileSymbolicPV(sourceFileName, destinationFileName);
		}
		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFMoveFileSymbolicPV(string sourceFileName, string destinationFileName)
		{
			var convertedSourceFileName = GetConvertedFileOrDirectorySpecPV(sourceFileName);
			var convertedDestinationFileName = GetConvertedFileOrDirectorySpecPV(destinationFileName);
			PAFMoveFilePV(convertedSourceFileName, convertedDestinationFileName);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFMoveFilePV(string sourceFileName, string destinationFileName);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IPAFStorageStream IPAFStorageService.PAFOpenFile(string path)
		{
			return m_AsIstorage.PAFOpenFile(path, null);
		}

		/// <remarks>
		/// <see cref="IPAFStorageService"/>.
		/// </remarks>
		IPAFStorageStream IPAFStorageService.PAFOpenFile(string path, PAFFileAccessMode mode)
		{
			return PAFOpenFileSymbolicPV(path, mode);
		}

		/// <remarks>
		/// backing for interface.
		/// Symbolic shim.
		/// </remarks>
		protected virtual IPAFStorageStream PAFOpenFileSymbolicPV(string path, PAFFileAccessMode mode)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPV(path);
			if (mode == PAFFileAccessMode.REPLACE)
			{
				if(PAFFileExistsPV(path))
					PAFDeleteFilePV(convertedPath);
			}

			return PAFOpenFilePV(convertedPath, mode);
		}
		/// <remarks>
		/// backing for interface.
		/// </remarks>
		protected abstract IPAFStorageStream PAFOpenFilePV(string path, PAFFileAccessMode mode);
/**/
		/// <remarks>
		/// <see cref="IPAFStorageService"/>. <paramref name="clientObject"/> is
		/// not used in this implementation.
		/// </remarks>
		IPAFStorageStream IPAFStorageService.PAFOpenFile(string path, PAFFileAccessMode mode, object clientObject)
		{
			return PAFOpenFileSymbolicPV(path, mode, clientObject);
		}

		/// <remarks>
		/// backing for interface.
		/// Symbolic shim.
		/// </remarks>
		protected virtual IPAFStorageStream PAFOpenFileSymbolicPV(string path, PAFFileAccessMode mode, object clientObject)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPV(path);
			return PAFOpenFilePV(convertedPath, mode, clientObject);
		}

		/// <remarks>
		/// backing for interface.
		/// </remarks>
		protected abstract IPAFStorageStream PAFOpenFilePV(string path, PAFFileAccessMode mode, object clientObject);
		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFRemove()
		{
			PAFRemovePV();
		}
		/// <summary>
		/// backing for interface. Doesn't do a thing by default.
		/// </summary>
		protected virtual void PAFRemovePV()
		{			
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFRemove(object clientObject)
		{
			PAFRemovePV(clientObject);
		}

		/// <summary>
		/// backing for interface. Doesn't do a thing by default.
		/// </summary>
		protected virtual void PAFRemovePV(object clientObject)
		{
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.SetApplicationRoot(string rootDirSpec)
		{
			SetApplicationRootPV(rootDirSpec);
		}
		/// <summary>
		/// backing for interface.
		/// </summary>
		protected virtual void SetApplicationRootPV(string rootDirSpec)
		{
			m_ApplicationRootDirectory = rootDirSpec;
		}

	}

}
