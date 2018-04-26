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
		protected internal readonly IPAFStorageService s_AsIstorage;
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
			s_AsIstorage = this;
			if (PlatformUtils.ApplicationRoot != null)
			{
				// This mst be preset for startup.
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
		protected virtual string ApplicationRootDirectoryPIV
		{
			get { return m_ApplicationRootDirectory; }
		}

		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFAvailableFreeSpace
		{
			get{return PAFAvailableFreeSpacePIV;}
		}
		/// <summary>
		/// backing for the interface. Returns max value.
		/// </summary>
		protected virtual long PAFAvailableFreeSpacePIV
		{
			get{return long.MaxValue;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFAvailableSize
		{
			get { return PAFAvailableSizePIV; }
		}

		/// <summary>
		/// backing for the interface. Returns max value.
		/// </summary>
		protected virtual long PAFAvailableSizePIV
		{
			get {return long.MaxValue;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFQuota
		{
			get{return PAFQuotaPIV;}
		}
		/// <summary>
		/// backing for the interface. Returns max value.
		/// </summary>
		protected virtual long PAFQuotaPIV
		{
			get{return long.MaxValue;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		long IPAFStorageService.PAFUsedSize
		{
			get { return PAFUsedSizePIV; }
		}
		/// <summary>
		/// backing for the interface. Returns 0.
		/// </summary>
		protected virtual long PAFUsedSizePIV
		{
			get{return 0;}
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		string IPAFStorageService.StorageTag
		{
			get { return StorageTagPIV; }
		}
		/// <summary>
		/// backing for the interface.
		/// NO-OP returns "TAG" for base. 
		/// </summary>
		protected virtual string StorageTagPIV
		{
			get { return "TAG"; }
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		string IPAFStorageService.GetConvertedFileSpec(string fileSpec)
		{
			return GetConvertedFileOrDirectorySpecPIV(fileSpec);
		}
		/// <summary>
		/// backing for the interface.
		/// Gets path with symbolic directories converted.
		/// </summary>
		protected virtual string GetConvertedFileOrDirectorySpecPIV(string fileSpec)
		{
			var normalizedPath =  FileUtils.NormalizeFilePathWithDriveOrDirectoryInternal(fileSpec);
			// If not rooted and root dir is forced, use it.
			if (!FileUtils.IsPathSpecRooted(normalizedPath) && !string.IsNullOrEmpty(ApplicationRootDirectoryPIV))
			{
				normalizedPath = ApplicationRootDirectoryPIV + PlatformUtils.GetDirectorySeparatorChar() + normalizedPath;
			}
			return normalizedPath;
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		string IPAFStorageService.GetMappedDirectorySymbol(string symbolicDirectorySymbol)
		{
			return GetMappedDirectorySymbolPIV(symbolicDirectorySymbol);
		}
		/// <summary>
		/// backing for the interface.
		/// Just accesses the static.
		/// </summary>
		protected virtual string GetMappedDirectorySymbolPIV(string symbolicDirectorySymbol)
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
			PAFCopyFileSymbolicPIV(sourceFileName, destinationFileName,overwrite);
		}

		/// <summary>
		/// backing for the interface.
		/// symbolic shim.
		/// </summary>
		protected virtual void PAFCopyFileSymbolicPIV(string sourceFileName,
			string destinationFileName, bool overwrite)
		{
			var convertedSourceFileName = GetConvertedFileOrDirectorySpecPIV(sourceFileName);
			var convertedDestinationFileName = GetConvertedFileOrDirectorySpecPIV(destinationFileName);
			PAFCopyFilePIV(convertedSourceFileName, convertedDestinationFileName, overwrite);
		}

		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFCopyFilePIV(string sourceFileName,
			string destinationFileName, bool overwrite);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFCreateDirectory(string dir)
		{
			PAFCreateDirectorySymbolicPIV(dir);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual object PAFCreateDirectorySymbolicPIV(string dir)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPIV(dir);
			return PAFCreateDirectoryPIV(convertedDir);
		}
		/// <summary>
		/// Backing support for the interface. Since <c>DirectoryInfo</c>
		/// cannot be part of a platform-independent interface, an object
		/// is returned here for any interface extensions to use.
		/// </summary>
		protected abstract object PAFCreateDirectoryPIV(string dir);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFCreateDirectory(string dir, object clientObject)
		{
			PAFCreateDirectorySymbolicPIV(dir, clientObject);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual object PAFCreateDirectorySymbolicPIV(string dir, object clientObject)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPIV(dir);
			return PAFCreateDirectoryPIV(convertedDir, convertedDir);
		}
		/// <summary>
		/// Backing support for the interface.
		/// client object not used in this base
		/// implementation.
		/// </summary>
		protected abstract object PAFCreateDirectoryPIV(string dir, object clientObject);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IPAFStorageStream IPAFStorageService.PAFCreateFile(string path)
		{
			return PAFCreateFileSmbolicPIV(path);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual IPAFStorageStream PAFCreateFileSmbolicPIV(string path)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPIV(path);
			return PAFCreateFilePIV(convertedPath);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract IPAFStorageStream PAFCreateFilePIV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteDirectory(string dir)
		{
			PAFDeleteDirectorySymbolicPIV(dir);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFDeleteDirectorySymbolicPIV(string dir)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPIV(dir);
			PAFDeleteDirectoryPIV(convertedDir);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFDeleteDirectoryPIV(string dir);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteDirectory(string dir, object clientObject)
		{
			PAFDeleteDirectorySymbolicPIV(dir, clientObject);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFDeleteDirectorySymbolicPIV(string dir, object clientObject)
		{
			var convertedDir = GetConvertedFileOrDirectorySpecPIV(dir);
			PAFDeleteDirectoryPIV(convertedDir);
		}
		/// <summary>
		/// backing for the interface. Client object not used in this implementation.
		/// </summary>
		protected virtual void PAFDeleteDirectoryPIV(string dir, object clientObject)
		{
			PAFDeleteDirectorySymbolicPIV(dir);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteFile(string file)
		{
			PAFSymbolicFileSymbolicPIV(file);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFSymbolicFileSymbolicPIV(string file)
		{
			var convertedFile = GetConvertedFileOrDirectorySpecPIV(file);
			PAFDeleteFilePIV(convertedFile);
		}

		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFDeleteFilePIV(string file);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFDeleteFile(string file, object clientObject)
		{
			PAFDeleteFileSymbolicPIV(file, clientObject);
		}
		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFDeleteFileSymbolicPIV(string file, object clientObject)
		{
			var convertedFile = GetConvertedFileOrDirectorySpecPIV(file);
			PAFDeleteFilePIV(convertedFile, clientObject);
		}
		/// <summary>
		/// backing for the interface. Client object not used in this implementation.
		/// </summary>
		protected virtual void PAFDeleteFilePIV(string file, object clientObject)
		{
			PAFDeleteFilePIV(file);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFDirectoryExists(string dir)
		{
			return PAFDirectoryExistsSymbolicPIV(dir);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		/// <remarks>
		/// Gate null values in here.
		/// </remarks>
		protected virtual bool PAFDirectoryExistsSymbolicPIV(string dir)
		{
			if (string.IsNullOrEmpty(dir))
				return false;
			var convertedDir = GetConvertedFileOrDirectorySpecPIV(dir);
			return PAFDirectoryExistsPIV(convertedDir);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract bool PAFDirectoryExistsPIV(string dir);
		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFFileExists(string path)
		{
			return PAFFileExistsSymbolicPIV(path);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		/// <remarks>
		/// Gate null values in here.
		/// </remarks>
		protected virtual bool PAFFileExistsSymbolicPIV(string path)
		{
			if (string.IsNullOrEmpty(path))
				return false;
			var convertedPath = GetConvertedFileOrDirectorySpecPIV(path);
			return PAFFileExistsPIV(convertedPath);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract bool PAFFileExistsPIV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		DateTime IPAFStorageService.PAFGetCreationTime(string path)
		{
			return PAFGetCreationTimeSymbolicPIV(path);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		/// <remarks>
		/// Gate null values in here.
		/// </remarks>
		protected virtual DateTime PAFGetCreationTimeSymbolicPIV(string path)
		{
			if (string.IsNullOrEmpty(path))
				return DateTime.MinValue;
			var convertedPath = GetConvertedFileOrDirectorySpecPIV(path);
			return PAFGetCreationTimePIV(convertedPath);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract DateTime PAFGetCreationTimePIV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetDirectoryNames()
		{
			return PAFGetDirectoryNamesPIV();
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetDirectoryNamesPIV()
		{
			return PAFGetDirectoryNamesPIV(null);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetDirectoryNames(string dirSpec)
		{
			return PAFGetDirectoryNamesSymbolicPIV(dirSpec);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetDirectoryNamesSymbolicPIV(string dirSpec)
		{
			var convertedDirSpec = GetConvertedFileOrDirectorySpecPIV(dirSpec);
			return PAFGetDirectoryNamesPIV(convertedDirSpec);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract IEnumerable<string> PAFGetDirectoryNamesPIV(string dirSpec);
 /**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetFileNames()
		{
			return PAFGetFileNamesPIV();
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetFileNamesPIV()
		{
			return PAFGetFileNamesPIV(null);
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IEnumerable<string> IPAFStorageService.PAFGetFileNames(string dirSpec)
		{
			return PAFGetFileNamesSymbolicPIV(dirSpec);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual IEnumerable<string> PAFGetFileNamesSymbolicPIV(string dirSpec)
		{
			var convertedDirSpec = GetConvertedFileOrDirectorySpecPIV(dirSpec);
			return PAFGetFileNamesPIV(convertedDirSpec);
		}
		/// <summary>
		/// Backing support for the interface.
		/// </summary>
		protected abstract IEnumerable<string> PAFGetFileNamesPIV(string dirSpec);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>.
		/// </summary>
		DateTime IPAFStorageService.PAFGetLastAccessTime(string path)
		{
			return PAFGetLastAccessTimeSymbolicPIV(path);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual DateTime PAFGetLastAccessTimeSymbolicPIV(string path)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPIV(path);
			return PAFGetLastAccessTimePIV(convertedPath);
		}

		/// <summary>
		/// Backing support for the interface.
		/// </summary>
		protected abstract DateTime PAFGetLastAccessTimePIV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		DateTime IPAFStorageService.PAFGetLastWriteTime(string path)
		{
			return PAFGetLastWriteTimeSymbolicPIV(path);
		}

		/// <summary>
		/// Backing support for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual DateTime PAFGetLastWriteTimeSymbolicPIV(string path)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPIV(path);
			return PAFGetLastWriteTimePIV(convertedPath);
		}
		/// <summary>
		/// Backing support for the interface.
		/// </summary>
		protected abstract DateTime PAFGetLastWriteTimePIV(string path);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFIncreaseQuotaTo(long newQuotaSize)
		{
			return PAFIncreaseQuotaToPIV(newQuotaSize);
		}
		/// <summary>
		/// backing for the interface.
		/// NO-OP in ECMA. returns <see langword="true"/>.
		/// </summary>
		protected virtual bool PAFIncreaseQuotaToPIV(long newQuotaSize)
		{
			return true;
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		bool IPAFStorageService.PAFIncreaseQuotaTo(long newQuotaSize, object clientObject)
		{
			return PAFIncreaseQuotaToPIV(newQuotaSize, clientObject);
		}
		/// <summary>
		/// backing for the interface.
		/// NO-OP in ECMA. returns <see langword="true"/>.
		/// </summary>
		protected virtual bool PAFIncreaseQuotaToPIV(long newQuotaSize, object clientObject)
		{
			return true;
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFMoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			PAFMoveDirectorySymbolicPIV(sourceDirectoryName, destinationDirectoryName);
		}

		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFMoveDirectorySymbolicPIV(string sourceDirectoryName, string destinationDirectoryName)
		{
			var convertedSourceDirectoryName = GetConvertedFileOrDirectorySpecPIV(sourceDirectoryName);
			var convertedDestinationDirectoryName = GetConvertedFileOrDirectorySpecPIV(destinationDirectoryName);
			PAFMoveDirectoryPIV(convertedSourceDirectoryName, convertedDestinationDirectoryName);

		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFMoveDirectoryPIV(string sourceDirectoryName, string destinationDirectoryName);
		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFMoveFile(string sourceFileName, string destinationFileName)
		{
			PAFMoveFileSymbolicPIV(sourceFileName, destinationFileName);
		}
		/// <summary>
		/// backing for the interface.
		/// Symbolic shim.
		/// </summary>
		protected virtual void PAFMoveFileSymbolicPIV(string sourceFileName, string destinationFileName)
		{
			var convertedSourceFileName = GetConvertedFileOrDirectorySpecPIV(sourceFileName);
			var convertedDestinationFileName = GetConvertedFileOrDirectorySpecPIV(destinationFileName);
			PAFMoveFilePIV(convertedSourceFileName, convertedDestinationFileName);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected abstract void PAFMoveFilePIV(string sourceFileName, string destinationFileName);
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		IPAFStorageStream IPAFStorageService.PAFOpenFile(string path)
		{
			return s_AsIstorage.PAFOpenFile(path, null);
		}

		/// <remarks>
		/// <see cref="IPAFStorageService"/>.
		/// </remarks>
		IPAFStorageStream IPAFStorageService.PAFOpenFile(string path, PAFFileAccessMode mode)
		{
			return PAFOpenFileSymbolicPIV(path, mode);
		}

		/// <remarks>
		/// backing for interface.
		/// Symbolic shim.
		/// </remarks>
		protected virtual IPAFStorageStream PAFOpenFileSymbolicPIV(string path, PAFFileAccessMode mode)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPIV(path);
			return PAFOpenFilePIV(convertedPath, mode);
		}
		/// <remarks>
		/// backing for interface.
		/// </remarks>
		protected abstract IPAFStorageStream PAFOpenFilePIV(string path, PAFFileAccessMode mode);
/**/
		/// <remarks>
		/// <see cref="IPAFStorageService"/>. <paramref name="clientObject"/> is
		/// not used in this implementation.
		/// </remarks>
		IPAFStorageStream IPAFStorageService.PAFOpenFile(string path, PAFFileAccessMode mode, object clientObject)
		{
			return PAFOpenFileSymbolicPIV(path, mode, clientObject);
		}

		/// <remarks>
		/// backing for interface.
		/// Symbolic shim.
		/// </remarks>
		protected virtual IPAFStorageStream PAFOpenFileSymbolicPIV(string path, PAFFileAccessMode mode, object clientObject)
		{
			var convertedPath = GetConvertedFileOrDirectorySpecPIV(path);
			return PAFOpenFilePIV(convertedPath, mode);
		}

		/// <remarks>
		/// backing for interface.
		/// </remarks>
		protected abstract IPAFStorageStream PAFOpenFilePIV(string path, PAFFileAccessMode mode, object clientObject);
		/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFRemove()
		{
			PAFRemovePIV();
		}
		/// <summary>
		/// backing for interface. Doesn't do a thing by default.
		/// </summary>
		protected virtual void PAFRemovePIV()
		{			
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.PAFRemove(object clientObject)
		{
			PAFRemovePIV();
		}

		/// <summary>
		/// backing for interface. Doesn't do a thing by default.
		/// </summary>
		protected virtual void PAFRemovePIV(object clientObject)
		{
		}
/**/
		/// <summary>
		/// <see cref="IPAFStorageService"/>
		/// </summary>
		void IPAFStorageService.SetApplicationRoot(string rootDirSpec)
		{
			SetApplicationRootPIV(rootDirSpec);
		}
		/// <summary>
		/// backing for interface.
		/// </summary>
		protected virtual void SetApplicationRootPIV(string rootDirSpec)
		{
			m_ApplicationRootDirectory = rootDirSpec;
		}

	}

}
