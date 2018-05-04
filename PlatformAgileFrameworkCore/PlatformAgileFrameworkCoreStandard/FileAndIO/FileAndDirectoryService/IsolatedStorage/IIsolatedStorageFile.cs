
using System;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// This is an interface that models methods on "System.IO.IsolatedStorage.IsolatedStorageFile".
	/// The interface is designed to allow retrofitting existing Silverlight storage access
	/// code to control other storage implementations. See individual implementation classes
	/// for details of the behavior of methods. They cannot be described in the interface,
	/// since implementations may differ widely.
	/// </summary>
	// ReSharper disable once PartialTypeWithSinglePart
	// for ECMACLR
	public partial interface IIsolatedStorageFile : IDisposable
	{

		/// <remarks/>
		long UsedSize { get; }

		/// <remarks/>
		long Quota { get; }

		/// <remarks/>
		long AvailableFreeSpace { get; }

		/// <remarks/>
		void Remove();

		/// <remarks/>
		bool IncreaseQuotaTo(long newQuotaSize);

		/// <remarks/>
		void DeleteFile(string file);

		/// <remarks/>
		bool FileExists(string path);

		/// <remarks/>
		bool DirectoryExists(string path);

		/// <remarks/>
		void CreateDirectory(string dir);

		/// <remarks/>
		void DeleteDirectory(string dir);
		
		/// <remarks/>
		string[] GetFileNames();

		/// <remarks/>
		string[] GetFileNames(string searchPattern);

		/// <remarks/>
		string[] GetDirectoryNames();

		/// <remarks/>
		string[] GetDirectoryNames(string searchPattern);

		/// <remarks/>
		IIsolatedStorageFileStream OpenFile(string path, PAFFileAccessMode mode);

		/// <remarks/>
		IIsolatedStorageFileStream CreateFile(string path);

		/// <remarks/>
		DateTimeOffset GetCreationTime(string path);

		/// <remarks/>
		DateTimeOffset GetLastAccessTime(string path);

		/// <remarks/>
		DateTimeOffset GetLastWriteTime(string path);

		/// <remarks/>
		void CopyFile(string sourceFileName, string destinationFileName);

		/// <remarks/>
		void CopyFile(string sourceFileName, string destinationFileName, bool overwrite);

		/// <remarks/>
		void MoveFile(string sourceFileName, string destinationFileName);

		/// <remarks/>
		void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName);
	}
}
