//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2015 Icucom Corporation
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
using System.IO;
using System.Collections.Generic;


namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
    /// <summary>
    /// This implementation employs standard stuff from <see cref="File"/>
    /// and <see cref="Directory"/>, which are exposed fully on Android now.
    /// </summary>
    /// <threadsafety>
    /// This implementation employs <see cref="Environment.CurrentDirectory"/>
    /// for CWD. Same caveats as described in the interface apply. 
    /// </threadsafety>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 12apr2017 </date>
    /// <desription>
    /// New. This is the reconstituted Android/ECMA version.
    /// </desription>
    /// </contribution>
    /// </history>
    // TODO - KRM - change to internal when in the SM.
    public class PAFStorageServiceAndroid : PAFStorageServiceAbstract
    {
        /// <summary>
        /// backing for the interface.
        /// </summary>
        protected override void PAFCopyFilePIV(string sourceFileName,
            string destinationFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destinationFileName, overwrite);
        }
        /// <summary>
        /// Backing support for the interface. Since <see cref="DirectoryInfo"/>
        /// cannot be part of a platform-independent interface, it is returned as
        /// an object.
        /// </summary>
        protected override object PAFCreateDirectoryPIV(string dir)
        {
            return Directory.CreateDirectory(dir);
        }
        /// <summary>
        /// Backing support for the interface. Since <see cref="DirectoryInfo"/>
        /// cannot be part of a platform-independent interface, it is returned here for
        /// any interface extensions to use. client object not used in this base
        /// implementation.
        /// </summary>
        protected override object PAFCreateDirectoryPIV(string dir, object clientObject)
        {
            return PAFCreateDirectoryPIV(dir);
        }
        /// <summary>
        /// backing for the interface.
        /// </summary>
        protected override IPAFStorageStream PAFCreateFilePIV(string path)
        {
            return new PAFStorageStream(File.Create(path));
        }
        /// <summary>
        /// backing for the interface.
        /// </summary>
        protected override void PAFDeleteDirectoryPIV(string dir)
        {
            Directory.Delete(dir);
        }
        /// <summary>
        /// backing for the interface. Client object not used in this implementation.
        /// </summary>
        protected override void PAFDeleteDirectoryPIV(string dir, object clientObject)
        {
            PAFDeleteDirectoryPIV(dir);
        }
        /// <summary>
        /// backing for the interface.
        /// </summary>
        protected override void PAFDeleteFilePIV(string file)
        {
            File.Delete(file);
        }
        /// <summary>
        /// backing for the interface. Client object not used in this implementation.
        /// </summary>
        protected override void PAFDeleteFilePIV(string file, object clienObject)
        {
            PAFDeleteFilePIV(file);
        }
        /// <summary>
        /// backing for the interface.
        /// </summary>
        protected override bool PAFDirectoryExistsPIV(string dir)
        {
            if (dir == null)
                return false;
            return Directory.Exists(dir);
        }
        /// <summary>
        /// backing for the interface. In ECMA, path specs can be relative and
        /// the runtime figures out a rooted path.
        /// </summary>
        protected override bool PAFFileExistsPIV(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            return File.Exists(path);
        }
        /// <summary>
        /// backing for the interface. This implementation returns
        /// <see cref="DateTime.MinValue"/> if the file does not exist.
        /// </summary>
        protected override DateTime PAFGetCreationTimePIV(string path)
        {
            if (!PAFFileExistsPIV(path))
                return DateTime.MinValue;
            return File.GetCreationTime(path);
        }
        /// <summary>
        /// backing for the interface.
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> or <see cref="String.Empty"/> defers to CWD.
        /// </remarks>
        protected override IEnumerable<string> PAFGetDirectoryNamesPIV(string dirSpec)
        {
            if (string.IsNullOrEmpty(dirSpec))
                dirSpec = Environment.CurrentDirectory;
            return Directory.GetDirectories(dirSpec);
        }
        /// <summary>
        /// Backing support for the interface.
        /// </summary>
        /// <remarks>
        /// <see langword="null"/> or <see cref="String.Empty"/> defers to CWD.
        /// </remarks>
        protected override IEnumerable<string> PAFGetFileNamesPIV(string dirSpec)
        {
            if (string.IsNullOrEmpty(dirSpec))
                dirSpec = Environment.CurrentDirectory;
            return Directory.GetFiles(dirSpec);
        }
        /// <summary>
        /// Backing support for the interface. This implementation returns
        /// <see cref="DateTime.MinValue"/> if the file does not exist.
        /// </summary>
        protected override DateTime PAFGetLastAccessTimePIV(string path)
        {
            if (!PAFFileExistsPIV(path))
                return DateTime.MinValue;
            return File.GetLastAccessTime(path);
        }
        /// <summary>
        /// Backing support for the interface. This implementation returns
        /// <see cref="DateTime.MinValue"/> if the file does not exist.
        /// </summary>
        protected override DateTime PAFGetLastWriteTimePIV(string path)
        {
            if (!PAFFileExistsPIV(path))
                return DateTime.MinValue;
            return File.GetLastWriteTime(path);
        }
        /// <summary>
        /// backing for the interface.
        /// </summary>
        protected override void PAFMoveDirectoryPIV(string sourceDirectoryName, string destinationDirectoryName)
        {
            Directory.Move(sourceDirectoryName, destinationDirectoryName);
        }
        /// <summary>
        /// backing for the interface.
        /// </summary>
        protected override void PAFMoveFilePIV(string sourceFileName, string destinationFileName)
        {
            File.Move(sourceFileName, destinationFileName);
        }

        /// <remarks>
        /// <see cref="IPAFStorageService"/>.
        /// </remarks>
        protected override IPAFStorageStream PAFOpenFilePIV(string path, PAFFileAccessMode mode)
        {
            var accessProps = FileAccessProps.MapFileAccess(mode);
            return new PAFStorageStream(File.Open(path, accessProps.Mode, accessProps.Access));
        }

        /// <remarks>
        /// <see cref="IPAFStorageService"/>. <paramref name="clientObject"/> is
        /// not used in this implementation.
        /// </remarks>
        protected override IPAFStorageStream PAFOpenFilePIV(string path, PAFFileAccessMode mode, object clientObject)
        {
            return s_AsIstorage.PAFOpenFile(path, mode);
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
        public FileMode Mode { get; private set; }
        /// <summary>
        /// Standard .Net access type.
        /// </summary>
        public FileAccess Access { get; private set; }
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
        /// if bit fileds are inconsistent.
        /// </returns>
        public static FileAccessProps MapFileAccess(PAFFileAccessMode pafFileAccessMode)
        {
            if (pafFileAccessMode == null)
            {
                return new FileAccessProps(FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }

            if (!PAFFileAccessMode.ValidateFileAccessMode(pafFileAccessMode))
                throw new InvalidDataException("Bad PAFFileAccessMode" + " " + pafFileAccessMode);

            FileMode mode;

            var access = FileAccess.Read;
            if ((pafFileAccessMode & PAFFileAccessMode.READONLY) != 0)
            {
                access = FileAccess.Read;
                mode = FileMode.Open;
                return new FileAccessProps(mode, access);
            }
            // Now map to the mode.
            if ((pafFileAccessMode & PAFFileAccessMode.REPLACE) != 0)
                mode = FileMode.CreateNew;
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
