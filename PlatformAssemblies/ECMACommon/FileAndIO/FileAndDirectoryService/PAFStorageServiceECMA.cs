using System;
using System.IO;
using System.Collections.Generic;
using System.Security.AccessControl;
using PlatformAgileFramework.Annotations;
namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// This implementation employs standard stuff from <see cref="File"/>
	/// and <see cref="Directory"/>, which are exposed fully on ECMA.
	/// </summary>
	/// <threadsafety>
	/// This implementation employs <see cref="Environment.CurrentDirectory"/>
	/// for CWD. Same caveat as described in the interface apply. 
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 12apr2016 </date>
	/// <desription>
	/// New. This is the reconstituted ECMA version.
	/// </desription>
	/// </contribution>
	/// </history>
	// TODO - KRM - change to internal when in the SM.
	public class PAFStorageServiceECMA : PAFStorageServiceAbstract
	{
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected override void PAFCopyFilePV(string sourceFileName,
			string destinationFileName, bool overwrite)
		{
			File.Copy(sourceFileName, destinationFileName, overwrite);
		}
		/// <summary>
		/// Backing support for the interface. Since <see cref="DirectoryInfo"/>
		/// cannot be part of a platform-independent interface, it is returned as
		/// an object.
		/// </summary>
		protected override object PAFCreateDirectoryPV(string dir)
		{
			return Directory.CreateDirectory(dir);
		}
		/// <summary>
		/// Backing support for the interface. Since <see cref="DirectoryInfo"/>
		/// cannot be part of a platform-independent interface, it is returned here for
		/// any interface extensions to use. client object not used in this base
		/// implementation.
		/// </summary>
		protected override object PAFCreateDirectoryPV(string dir, object clientObject)
		{
			return PAFCreateDirectoryPV(dir);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected override IPAFStorageStream PAFCreateFilePV(string path)
		{
			return new PAFStorageStream(File.Create(path));
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected override void PAFDeleteDirectoryPV(string dir)
		{
			Directory.Delete(dir);
		}
		/// <summary>
		/// backing for the interface. Client object not used in this implementation.
		/// </summary>
		protected override void PAFDeleteDirectoryPV(string dir, object clientObject)
		{
			PAFDeleteDirectoryPV(dir);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected override void PAFDeleteFilePV(string file)
		{
			File.Delete(file);
		}
		/// <summary>
		/// backing for the interface. Client object not used in this implementation.
		/// </summary>
		protected override void PAFDeleteFilePV(string file, object clientObject)
		{
			PAFDeleteFilePV(file);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected override bool PAFDirectoryExistsPV(string dir)
		{
			if (dir == null)
				return false;
			return Directory.Exists(dir);
		}
		/// <summary>
		/// backing for the interface. In ECMA, path specs can be relative and
		/// the runtime figures out a rooted path.
		/// </summary>
		protected override bool PAFFileExistsPV(string path)
		{
			if (string.IsNullOrEmpty(path))
				return false;
			return File.Exists(path);
		}
		/// <summary>
		/// backing for the interface. This implementation returns
		/// <see cref="DateTime.MinValue"/> if the file does not exist.
		/// </summary>
		protected override DateTime PAFGetCreationTimePV(string path)
		{
			if(!PAFFileExistsPV(path))
				return DateTime.MinValue;
			return File.GetCreationTime(path);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		/// <remarks>
		/// <see langword="null"/> or <see cref="string.Empty"/> defers to CWD.
		/// </remarks>
		protected override IEnumerable<string> PAFGetDirectoryNamesPV(string dirSpec)
		{
			if(string.IsNullOrEmpty(dirSpec))
				dirSpec = Environment.CurrentDirectory;
			return Directory.GetDirectories(dirSpec);
		}
		/// <summary>
		/// Backing support for the interface.
		/// </summary>
		/// <remarks>
		/// <see langword="null"/> or <see cref="string.Empty"/> defers to CWD.
		/// </remarks>
		protected override IEnumerable<string> PAFGetFileNamesPV(string dirSpec)
		{
			if (string.IsNullOrEmpty(dirSpec))
				dirSpec = Environment.CurrentDirectory;
			return Directory.GetFiles(dirSpec);
		}
		/// <summary>
		/// Backing support for the interface. This implementation returns
		/// <see cref="DateTime.MinValue"/> if the file does not exist.
		/// </summary>
		protected override DateTime PAFGetLastAccessTimePV(string path)
		{
			if (!PAFFileExistsPV(path))
				return DateTime.MinValue;
			return File.GetLastAccessTime(path);
		}
		/// <summary>
		/// Backing support for the interface. This implementation returns
		/// <see cref="DateTime.MinValue"/> if the file does not exist.
		/// </summary>
		protected override DateTime PAFGetLastWriteTimePV(string path)
		{
			if (!PAFFileExistsPV(path))
				return DateTime.MinValue;
			return File.GetLastWriteTime(path);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected override void PAFMoveDirectoryPV(string sourceDirectoryName, string destinationDirectoryName)
		{
			Directory.Move(sourceDirectoryName, destinationDirectoryName);
		}
		/// <summary>
		/// backing for the interface.
		/// </summary>
		protected override void PAFMoveFilePV([NotNull] string sourceFileName, [NotNull] string destinationFileName)
		{
			File.Move(sourceFileName, destinationFileName);
		}

		/// <remarks>
		/// <see cref="IPAFStorageService"/>.
		/// </remarks>
		protected override IPAFStorageStream PAFOpenFilePV([NotNull] string path, PAFFileAccessMode mode)
		{
			// KRM patch made 05may2019 to make this work on all platforms.
			if ((mode != null) && ((mode & PAFFileAccessMode.REPLACE) != 0))
			{
				PAFDeleteFilePV(path);
			}
			var accessProps = FileAccessProps.MapFileAccess(mode);
			return new PAFStorageStream(File.Open(path, accessProps.Mode, accessProps.Access));
		}

		/// <remarks>
		/// <see cref="IPAFStorageService"/>. <paramref name="clientObject"/> is
		/// not used in this implementation.
		/// </remarks>
		protected override IPAFStorageStream PAFOpenFilePV(string path, PAFFileAccessMode mode, object clientObject)
		{
			return m_AsIstorage.PAFOpenFile(path, mode);
		}
	}
	/// <summary>
	/// Just a little struct to hold file access attributes.
	/// </summary>
	public struct FileAccessProps
	{
		#region Fields and Autoprops
		/// <summary>
		/// Standard .Net file mode.
		/// </summary>
		public FileMode Mode { get; internal set; }
		/// <summary>
		/// Standard .Net access type.
		/// </summary>
		public FileAccess Access { get; internal set; }
		#endregion // Fields and Autoprops
		#region Constructors
		/// <summary>
		/// Constructor just loads the props.
		/// </summary>
		/// <param name="mode">Loads <see cref="Mode"/></param>
		/// <param name="access">Loads <see cref="Access"/></param>
		public FileAccessProps(FileMode mode, FileAccess access) : this()
		{
			Mode = mode;
			Access = access;
		}
		#endregion // Constructors

		#region Methods
		/// <summary>
		/// Method maps <see cref="PAFFileAccessMode"/>
		/// </summary>
		/// <param name="pafFileAccessMode">
		/// <see langword="null"/> results in <see cref="FileAccess.ReadWrite"/>
		/// and <see cref="FileMode.OpenOrCreate"/>.
		/// </param>
		/// <returns><see langword = "false"/>
		/// if bit fields are inconsistent.
		/// </returns>
		public static FileAccessProps MapFileAccess(PAFFileAccessMode pafFileAccessMode)
		{
			if (pafFileAccessMode == null)
			{
				return new FileAccessProps(FileMode.OpenOrCreate, FileAccess.ReadWrite);
			}

			if(!PAFFileAccessMode.ValidateFileAccessMode(pafFileAccessMode))
				throw new InvalidDataException("Bad PAFFileAccessMode" + " " + pafFileAccessMode);

			FileMode mode;

			FileAccess access;
			if ((pafFileAccessMode & PAFFileAccessMode.READONLY) != 0)
			{
				mode = FileMode.Open;
				access = FileAccess.Read;
				return new FileAccessProps(mode, access);
			}
			// Now map to the mode.
			if ((pafFileAccessMode & PAFFileAccessMode.REPLACE) != 0)
			{
				mode = FileMode.CreateNew;
				access = FileAccess.ReadWrite;
			}
			else if ((pafFileAccessMode & PAFFileAccessMode.APPEND) != 0)
			{
				mode = FileMode.Append;
				access = FileAccess.Write;
			}
			else
			{
				mode = FileMode.OpenOrCreate;
				access = FileAccess.ReadWrite;
			}

			return new FileAccessProps(mode, access);
		}
		#endregion //Methods

	}

}
