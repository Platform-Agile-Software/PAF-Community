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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Security;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
//using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.FrameworkServices;

#region Exception Shorthand

//using IPAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFFileAndDirectoryIOExceptionData;
//using PAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFFileAndDirectoryIOExceptionData;
#endregion // Exception Shorthand

#endregion // Using Directives
namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// <para>
	///	Interface for our file service.
	/// This interface manipulates a storage partition that is similar
	/// to a directory and is fully integrated with the PAF symbolic
	/// directory system that is fundamental to the PAF cross-platform
	/// abstraction of the storage function. The file service is heavily
	/// dependent on the PAF symbolic directory mapping system which
	/// maps symbolic directories (such as c: or d:) on a specific
	/// platform to a directory (e.g. /lib/usr) or to a location in
	/// isolated storageor "sandbox" storage. Core SL model does not
	/// include addin support, so it always works out of either
	/// the site storage area or the application storage area.
	/// Extended SL supports symbolic directories and most of
	/// the addin model.
	/// </para>
	/// <para>
	/// The extensions in the PAF addin model are also related to trusted
	/// addins being able to access a wider class of storage/data. This
	/// extended functionality is effected mostly through extra parameters
	/// and a number of additional methods germane only to the extended
	/// functionality. The methods from the addin framework have all
	/// been reformulated to correspond to the SL model. This was a
	/// decision that was taken with care. It was felt that legacy PAF
	/// addin apps could be recrafted easily by their developers, while
	/// SL app developers would be reluctant to use a different set of
	/// conventions. In order to minimize code, no translation layer
	/// has been provided - addin app developers can author one.
	/// </para>
	/// <para>
	/// For SL or ECMA/CLR, most of the methods here are simple wrappers
	/// for the equivalent "File" or "IsolatedStorageFile" methods that
	/// translate the filenames to user-defined places. We don't catch
	/// any exceptions from underlying system methods. However, we always
	/// throw an exception when a directory symbol mapping is not found in
	/// the dictionary.
	/// </para>
	/// <para>
	/// This interface prescribes the functionality for accessing both the 
	/// SL "isolated storage" model and the PAF "sandbox storage" model. It
	/// is used as a framework service, but has the equivalent access points
	/// as "IsolatedStorageFile" in the SL model. The PAF storage model is
	/// more general than pure file storage, but again, we're using the
	/// SL/ECMA/CLR nomenclature to recast things as a single interface,
	/// familiar to the largest user group.
	/// </para>
	/// <para>
	/// Since the interface is used in different environments, the rules for
	/// forming and interpretation of filepaths is different in these varied
	/// environments. A directory will be always created under the current working
	/// directory for sandboxed storage. In sandbox mode, any leading directory
	/// separators will be ignored. In the ECMA/CLR environment, the directory
	/// will be created under the current working directory. If symbolic directories
	/// are supported, if the path is prefixed with a symbolic directory, the
	/// directory symbol is resolved and the proper path is calculated. The same
	/// idea holds true for filepaths (including the actual filename). The best way
	/// to abstract the storage function is, again, to use symbolic directories.
	/// Map "C_DRIVE" to a filesystem directory in ECMA/CLR (or even a drive letter
	/// for Windows) and a subdirectory under the root app store for the SL or
	/// PAF sandboxed models.
	/// </para>
	/// <para>
	/// An important extension to the standard SL isolated storage methods is
	/// the inclusion of an <see cref="object"/> called a "clientObject"
	/// as a parameter. This is used within the addin framework to ensure that
	/// individual addins do not misuse system resources. In the addin framework,
	/// the security object exposes the addin's "access token". There
	/// is no compulsion for any implementation to use these parameters,
	/// but they are available for general use for security purposes. It widens
	/// the interface, but there is a good reason to have them in a common
	/// interface. We generally allow discovery of files/directories without
	/// security, but not modification. The addin functionality is available
	/// in SL extended.
	/// </para>
	/// <para>
	/// This interface is extended in the addin model and the ECMA/CLR model
	/// to include the concept of the "Current Working Directory" (CWD). These
	/// extensions allow a pointer to be moved around within the storage area
	/// to change the default location for many of the operations exposed by
	/// the methods within this interface. In the Core SL implementation, the
	/// CWD is either the application or site root.
	/// </para>
	/// </summary>
	/// <remarks>
	/// <para>
	/// Noted: members are prefixed with "PAF" to avoid confusion between
	/// calling native methods and calling our abstraction layer.
	/// </para>
	/// <para>
	/// We tend not to use optional parameters in some interfaces - those that
	/// we want to be implemented explicitly if the user desires. This is the case
	/// most of the time. It's more work for us, not you.
	/// </para>
	/// <para>
	/// Symbolic directories can be used within path and directory specifications
	/// if they are implemented for a specific environment. They normally are for
	/// ECMA/CLR and the addin framework. Otherwise directories must be relative.
	/// </para>
	/// <para>
	/// Symbolic directories are easily implemented on mobile devices that allow
	/// use of multile directories.
	/// </para>
	/// </remarks>
	/// <exceptions>
	/// <exception cref="PAFStandardException{T}">
	/// is thrown in every method that takes a filepath, when a symbolic
	/// directory symbol is prepended and the symbolic directory symbol
	/// cannot be found in the symbol dictionary. This means that the proper
	/// symbol has not been installed in the configuration file for the
	/// application or in code.
	///  Message = <see cref="PAFStoragePathFormatExceptionDataBase.DIRECTORY_SYMBOL_NOT_FOUND"/>.
	/// </exception>
	/// <exception cref="PAFStandardException{IPAFFFED}"> is thrown in every
	/// method that takes a filepath, when the filepath must be a filename
	/// only (no path characters).
	/// Message = <see cref="PAFStoragePathFormatExceptionDataBase.DIRECTORY_SPECIFICATION_NOT_ALLOWED"/>.
	/// </exception>
	/// </exceptions>
	/// <history>
	/// <contribution>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 12mar2015 </date>s
	/// <description>
	/// Rewrote to mostly update some documentation to indicate support for
	/// Xamarin.Forms and operation on mobile devices.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17jan2012 </date>
	/// <description>
	/// New.
	/// Decided to put the functionality from the addin framework storage access
	/// in here. The add-in requirements are only a modest extension of the isolated
	/// storage model and this allows one more interface to be consoldidated.
	/// </description>
	/// </contribution>
	/// <author> KRM </author>
	/// <date> 22jun2014 </date>
	/// <description>
	/// Exposed symbolic directory translation stuff, since XML classes need to be supplied
	/// with filenames.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Implementations should always be made thread-safe.
	/// There are several methods within the SL API, when called
	/// with other methods do not conform to a concurrent style of programming.
	/// These are indicated below and in <see cref="IPAFStorageStream"/>.
	/// Implementors must be aware of the problems in working with a finite
	/// store that may be accessed by concurrent callers and provide appropriate
	/// safeguards. The basic problems revolve around the issue of two callers
	/// thinking there is adequate space for what they want to write and then
	/// finding that both of them can't write all their data. Appropriate
	/// locks or some suitable control mechanism must be provided.
	/// </threadsafety>
	/// ReSharper disable once PartialTypeWithSinglePart
	/// search paths don't work right on Mono
	public partial interface IPAFStorageService: IPAFService
	{
		#region Properties

		/// <summary>
		/// This is the application root directory in cases where the root needs to be
		/// forced. This needs to be placed on the front end of unrooted filespecs if it
		/// is not <see langword="null"/> or <see cref="string.Empty"/>.
		/// </summary>
		string ApplicationRootDirectory { get; }
		/// <summary>
		/// Indicates the available free space in the overall store for this
		/// application or site. This is the SL equivalent and has the same name.
		/// For the addin app, it returns the total available space for the addin
		/// in it's entire directory tree. For ECMA/CLR this should USUALLY return
		/// <see cref="long.MaxValue"/>.
		/// </summary>
		/// <threadsafety>
		/// This method cannot be relied upon to produce correct indications
		/// of free space available except at the instant at which it is called.
		/// Calling this method, followed by a call to <see cref="IPAFStorageStream.PAFSetLength"/>
		/// can result in an exception being thrown. See <see cref="IPAFStorageStream.PAFTrySetStorageSize"/>.
		/// </threadsafety>
		long PAFAvailableFreeSpace { get; }
		/// <summary>
		/// Indicates the available free space within the CWD. For SL this will
		/// always return the same as <see cref="PAFAvailableFreeSpace"/>. For ECMA/CLR
		/// this should USUALLY return  <see cref="Int64.MaxValue"/>.
		/// </summary>
		/// <threadsafety>
		/// This method cannot be relied upon to produce correct indications
		/// of free space available except at the instant at which it is called.
		/// Calling this method, followed by a call to <see cref="IPAFStorageStream.PAFSetLength"/>
		/// can result in an exception being thrown. See <see cref="IPAFStorageStream.PAFTrySetStorageSize"/>.
		/// </threadsafety>
		long PAFAvailableSize { get; }
		/// <summary>
		/// This is the SL equivalent and has the same name. It returns the
		/// available free space within the CWD for the addins. For ECMA/CLR
		/// this should USUALLY return  <see cref="Int64.MaxValue"/>.
		/// </summary>
		/// <threadsafety>
		/// This method cannot be relied upon to produce correct indications
		/// of free space available except at the instant at which it is called.
		/// Calling this method, followed by a call to <see cref="IPAFStorageStream.PAFSetLength"/>
		/// can result in an exception being thrown. See <see cref="IPAFStorageStream.PAFTrySetStorageSize"/>.
		/// </threadsafety>
		long PAFQuota { get; }
		/// <summary>
		/// This is the SL equivalent and has the same name. It is redundant and
		/// was never present in the addin model previously. It works in both SL
		/// apps and addin apps. 
		/// </summary>
		/// <threadsafety>
		/// This method cannot be relied upon to produce correct indications
		/// of free space available except at the instant at which it is called.
		/// Calling this method, followed by a call to <see cref="IPAFStorageStream.PAFSetLength"/>
		/// can result in an exception being thrown. See <see cref="IPAFStorageStream.PAFTrySetStorageSize"/>.
		/// </threadsafety>
		long PAFUsedSize { get; }
		/// <summary>
		/// Different than the name of the service. This can be mapped to a directory or a
		/// DB or something else entirely.
		/// </summary>
		string StorageTag { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Translates a symbolic directory symbol to a real directory spec on the native machine
		/// or environment.
		/// </summary>
		/// <param name="symbolicDirectorySymbol">
		/// The stringful symbol.
		/// </param>
		/// <returns><see langword="null"/> if symbol not in dictionary.</returns>
		/// <remarks>
		/// This is a <see cref="SecurityCriticalAttribute"/> call. Non-explicit implementations
		/// must be marked with this attribute.
		/// </remarks>
		[SecurityCritical]
		string GetMappedDirectorySymbol(string symbolicDirectorySymbol);

		/// <summary>
		/// Translates a file specification symbol to a real file path on the native machine
		/// or environment.
		/// </summary>
		/// <param name="fileSpec">
		/// Name of the file, including possible symbolic spec and alternative
		/// directory separators that must be converted.
		/// </param>
		/// <returns>
		/// Converted and normalized file specification.
		/// </returns>
		/// <returns>Never <see langword="null"/>.</returns>
		/// <exceptions>
		/// <exception>
		/// <see cref="PAFStandardException{IPAFFilePathFormatExceptionData}"/>
		/// <see cref="PAFStoragePathFormatExceptionDataBase.BAD_FILE_PATH"/> if filepath
		/// cannot be resolved.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// This is a <see cref="SecurityCriticalAttribute"/> call. Non-explicit implementations
		/// must be marked with this attribute.
		/// </remarks>
		[SecurityCritical]
		string GetConvertedFileSpec(string fileSpec);

		/// <summary>
		/// Copies a file with a potential overwrite. For the addin
		/// model, this method is marked "security critical", since a call
		/// could be made to overwrite existing files and we don't want just
		/// anybody copying files, either.
		/// </summary>
		/// <param name="sourceFileName">
		/// Name of the file to copy. This can actually be a path, including
		/// the file name always.
		/// </param>
		/// <param name="destinationFileName">
		/// Name of the file to copy the source file into. This can actually be
		/// a path, including the file name always.
		/// </param>
		/// <param name="overwrite">
		/// <see langword="true"/> to overwrite an existing file.
		/// </param>
		/// <remarks>
		/// We don't provide a secured overload for this with the "clientObject"
		/// parameter. An extension method wrapping the open and write methods is
		/// used instead.
		/// </remarks>
		void PAFCopyFile(string sourceFileName, string destinationFileName, bool overwrite);
		/// <summary>
		/// Creates a directory or a "mount point" for storage.
		/// </summary>
		/// <param name="dir">
		/// This is a directory that will be created in different places, depending
		/// on the environment. See the description for this class.
		/// </param>
		/// <remarks>
		/// The old "CreateMountPoint" is now replaced with this. Same signature as SL.
		/// SL functionality in SL. Note that this method may be marked security critical
		/// in some environments (e.g. addin).
		/// </remarks>
		void PAFCreateDirectory(string dir);
		/// <summary>
		/// Secured version of <see cref="PAFCreateDirectory(String)"/>.
		/// </summary>
		/// <param name="dir">
		/// See <see cref="PAFCreateDirectory(String)"/>.
		/// </param>
		/// <param name="clientObject">
		/// An object that MAY contain security information.
		/// </param>
		void PAFCreateDirectory(string dir, object clientObject);
		/// <summary>
		/// Creates a storage stream with an attached persistance spec..
		/// In core, this is just a filename with a possible directory spec..
		/// Use this method instead of the open method to resolutely replace
		/// a file if it already exists. Client must have read/write/delete
		/// permissions on the data store.
		/// </summary>
		/// <param name="path">
		/// This is a stream that will be persisted in different places, depending
		/// on the environment. See the description for this class. In core, this is
		/// always a file or a virtual file.
		/// </param>
		/// <remarks>
		/// The old "GetStorageStream" is now replaced with this. Same signature as SL.
		/// SL functionality in SL. Note that this method may be marked security critical
		/// in some environments (e.g. addin). We don't provide a secured overload for
		/// this with the "clientObject" parameter. An extension method wrapping the
		/// open method is used instead.
		/// </remarks>
		IPAFStorageStream PAFCreateFile(string path);
		/// <summary>
		/// Removes the specified directory from within the current storage area.
		/// </summary>
		/// <param name="dir">
		/// Directory to be deleted. It may contain an optional trailing path separator.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IFIOED}">
		/// Message: <see cref="PAFFileAndDirectoryIOExceptionDataBase.ERROR_DELETING_DIRECTORY"/> is thrown
		/// if anything goes wrong with the operation. This exception may contain
		/// various inner exceptions which are platform-specific. If the directory
		/// does not exist, an exception is thrown. Check for existance first
		/// to avoid this.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// Note that this method may be marked security critical
		/// in some environments (e.g. addin). In order to keep the interface narrow, recursive
		/// deletes are done in extension methods.
		/// </remarks>
		void PAFDeleteDirectory(string dir);
		/// <summary>
		/// Removes the specified directory from within the current storage area.
		/// </summary>
		/// <param name="dir">
		/// See <see cref="PAFDeleteDirectory(String)"/>.
		/// </param>
		/// <param name="clientObject">
		/// An object that MAY contain security information.
		/// </param>
		void PAFDeleteDirectory(string dir, object clientObject);
		/// <summary>
		/// Removes the specified file from within the current storage area
		/// </summary>
		/// <param name="file">
		/// File to be deleted. This string can contain a directory spec, thus
		/// forming a full file path.
		/// </param>
		/// <remarks>
		/// Note that this method may be marked security critical in some environments
		/// (e.g. addin). 
		/// </remarks>
		void PAFDeleteFile(string file);
		/// <summary>
		/// Secured version of <see cref="PAFDeleteFile(String)"/>.
		/// </summary>
		/// <param name="file">
		/// See <see cref="PAFDeleteFile(String)"/>.
		/// </param>
		/// <param name="clientObject">
		/// An object that MAY contain security information.
		/// </param>
		void PAFDeleteFile(string file, object clientObject);
		/// <summary>
		/// Determines whether the specified path refers to an existing directory.
		/// </summary>
		/// <param name="dir">
		/// The directory to test. It may contain an optional trailing path separator.
		/// <see langword="null"/> or empty returns <see langword="false"/>.
		/// </param>
		/// <returns>
		/// true if <paramref name="dir"/> refers to an existing directory.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		bool PAFDirectoryExists(string dir);
		/// <summary>
		/// Determines whether the specified path refers to an existing file.
		/// </summary>
		/// <param name="file">
		/// File to be checked for. This string can contain a directory spec, thus
		/// forming a full file path. <see langword="null"/> or empty returns
		/// <see langword="false"/>.
		/// </param>
		/// <returns>
		/// true if <paramref name="file"/> refers to an existing file.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		bool PAFFileExists(string file);

		/// <summary>
		/// Returns the creation date and time of a specified file or directory.
		/// </summary>
		/// <param name="path">
		/// The path to the file or directory. Directory specifications must
		/// end with a path separator character. <see langword="null"/>  or empty
		/// returns <see cref="DateTime.MinValue"/>.
		/// </param>
		/// <returns>
		/// The creation date and time for the specified file or directory.
		/// This value is expressed in local time.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		DateTime PAFGetCreationTime(string path);
		/// <summary>
		/// Obtains the names of directoriess in current working directory of the
		/// storage area. For Core SL model, this is either the app store
		/// root or the site store root. For ECMA, this is the exe directory.
		/// For phones, this is platform-dependent. Directories are returned without
		/// terminating path separator.
		/// </summary>
		/// <returns>
		/// An array of relative paths of directories in the CWD of the storage area.
		/// A <see langword = "null"/> specifies that there are no files in the storage area.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		/// <threadsafety>
		/// Generally NOT thead-safe if CWD is in use in the infrastructure. For multithreaded
		/// applications, this call should be made in the bootstrapper only or from the (single) thread
		/// that sets the CWD.
		/// </threadsafety>
		IEnumerable<string> PAFGetDirectoryNames();
		/// <summary>
		/// Obtains the names of directories in the specified directory of the
		/// storage area. For Core SL model, this is either the app store
		/// root or the site store root. For ECMA, this is the exe directory.
		/// For phones, this is platform-dependent. Directories are returned without
		/// terminating path separator.
		/// </summary>
		/// <param name="dirSpec">
		/// Can be a rooted path or a relative path, with trailing dirsep or not.
		/// </param>
		/// <returns>
		/// An array of paths of directories in the specified storage area.
		/// A <see langword = "null"/> specifies that there are no directories in the
		/// storage area.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		IEnumerable<string> PAFGetDirectoryNames(string dirSpec);
		/// <summary>
		/// Obtains the names of files in the specified directory of the
		/// storage area. For Core SL model, this is either the app store
		/// root or the site store root. For ECMA, this is the exe directory.
		/// For phones, this is platform-dependent.
		/// </summary>
		/// <returns>
		/// An array of filenames in the CWD of the storage area.
		/// A <see langword = "null"/> specifies that there are no files in the storage area.
		/// </returns>
		/// <threadsafety>
		/// Generally NOT thead-safe if CWD is in use in the infrastructure. For multithreaded
		/// applications, this call should be made in the bootstrapper only or from the (single) thread
		/// that sets the CWD.
		/// </threadsafety>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		IEnumerable<string> PAFGetFileNames();

		/// <summary>
		/// Obtains the names of filess in the specified directory of the
		/// storage area. For Core SL model, this is either the app store
		/// root or the site store root. For ECMA, this is the exe directory.
		/// For phones, this is platform-dependent.
		/// </summary>
		/// <param name="dirSpec">
		/// Can be a rooted path or a relative path, with trailing dirsep or not.
		/// </param>
		/// <returns>
		/// An array of filenames in the specified storage area.
		/// A <see langword = "null"/> specifies that there are no files in the
		/// storage area.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		IEnumerable<string> PAFGetFileNames(string dirSpec);
		/// <summary>
		/// Returns the last access date and time of a specified file or directory.
		/// </summary> 
		/// <param name="path">
		/// The path to the file or directory. Directory specifications must
		/// end with a path separator character.
		/// </param>
		/// <returns>
		/// The last access date and time for the specified file or directory.
		/// This value is expressed in local time.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		DateTime PAFGetLastAccessTime(string path);

		/// <summary>
		/// Returns the last write date and time of a specified file or directory.
		/// </summary>
		/// <param name="path">
		/// The path to the file or directory. Directory specifications must
		/// end with a path separator character.
		/// </param>
		/// <returns>
		/// The last access date and time for the specified file or directory.
		/// This value is expressed in local time.
		/// </returns>
		DateTime PAFGetLastWriteTime(string path);
		/// <summary>
		/// Increases the quota of the store. Usually a NOP for ECMA/CLR. For
		/// SL or addin model, it does what is says.
		/// </summary>
		/// <param name="newQuotaSize">
		/// The new size of the storage, in byte.
		/// </param>
		/// <returns>
		/// <see langword="true"/> for success.
		/// </returns>
		/// <threadsafety>
		/// Concurrency problems exist and must be managed. A thread cannot expect
		/// to increase the quota large enough to write a certain dataset and
		/// not have another thread sneak in and write something before it
		/// (the original thread) does. This sceanario must be managed in any
		/// implementation.
		/// </threadsafety>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		/// <remarks>
		/// Note that this method may be marked security critical
		/// in some environments (e.g. addin). 
		/// </remarks>
		bool PAFIncreaseQuotaTo(long newQuotaSize);
		/// <summary>
		/// Secured version of <see cref="PAFIncreaseQuotaTo(Int64)"/>.
		/// </summary>
		/// <param name="newQuotaSize">
		/// See <see cref="PAFIncreaseQuotaTo(Int64)"/>.
		/// </param>
		/// <param name="clientObject">
		/// An object that MAY contain security information.
		/// </param>
		/// <returns>
		/// See <see cref="PAFIncreaseQuotaTo(Int64)"/>.
		/// </returns>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		bool PAFIncreaseQuotaTo(long newQuotaSize, object clientObject);
		/// <summary>
		/// Moves a specified directory and its contents to a new location. The
		/// old directory is deleted.
		/// </summary>
		/// <param name="sourceDirectoryName">
		/// The name of the directory to move.
		/// </param>
		/// <param name="destinationDirectoryName">
		/// The name of the directory to move to. This directory must not exist.
		/// </param>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		void PAFMoveDirectory(string sourceDirectoryName, string destinationDirectoryName);
		/// <summary>
		/// Moves a specified file to a new file. The old file is deleted.
		/// </summary>
		/// <param name="sourceFileName">
		/// The name of the file to move.
		/// </param>
		/// <param name="destinationFileName">
		/// The name of the file to move to. This file must not exist.
		/// </param>
		/// <remarks>
		/// Note that this method may be marked security critical
		/// in some environments (e.g. addin). We don't provide a secured overload for
		/// this with the "clientObject" parameter. An extension method wrapping the
		/// open, write and delete methods is used instead.
		/// </remarks>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		void PAFMoveFile(string sourceFileName, string destinationFileName);
		/// <summary>
		/// Main file acccess method for both low-trust and high-trust callers. Interface
		/// also contains several overloads with fewer parameters.
		/// </summary>
		/// <param name="path">
		/// This is a stream that will be persisted in different places, depending
		/// on the environment. See the description for this class. In core, this is
		/// always a file or a virtual file.
		/// </param>
		/// <param name="mode">
		/// See <see cref="PAFFileAccessMode"/>.
		/// </param>
		/// <param name="clientObject">
		/// An object that MAY contain security information. Some implementations mark
		/// other overloads of this method as security critical and require the
		/// <paramref name="clientObject"/> to be used.
		/// </param>
		/// <returns>
		/// A stream if successful. Underlying OS-specific exceptions are not
		/// caught.
		/// </returns>
		/// <remarks>
		/// This method is not in the standard SL isolated storage model. It
		/// contains an extra <paramref name="clientObject"/> argument that
		/// is used in the PAF addin model. It can perfectly well be used,
		/// however, in a straight SL application when the developer wants
		/// to secure certain areas of storage, etc.
		/// </remarks>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		IPAFStorageStream PAFOpenFile(string path, PAFFileAccessMode mode, object clientObject);
		/// <remarks>
		/// <see cref="PAFOpenFile(String, PAFFileAccessMode , Object)"/>.
		/// Uses "clientObject" = <see langword="null"/>.
		/// </remarks>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		IPAFStorageStream PAFOpenFile(string path, PAFFileAccessMode mode);
		/// <remarks>
		/// <see cref="PAFOpenFile(String, PAFFileAccessMode , Object)"/>.
		/// Uses "clientObject" = <see langword="null"/>.
		/// Default access mode depends on platform.
		/// </remarks>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		IPAFStorageStream PAFOpenFile(string path);
		/// <summary>
		/// Removes the storage scope storage (either app or site for a SL application).
		/// For the PAF addin model, it can remove any addin storage area. CWD goes up one
		/// level upon completion for the addin model.
		/// </summary>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		/// <remarks>
		/// Note that this method may be marked security critical in some environments
		/// (e.g. addin). 
		/// </remarks>
		void PAFRemove();
		/// <summary>
		/// Secured version of <see cref="PAFRemove()"/>.
		/// </summary>
		/// <param name="clientObject">
		/// An object that MAY contain security information.
		/// </param>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		void PAFRemove(object clientObject);
		/// <summary>
		/// Sets application root in cases where it needs to be forced.
		/// </summary>
		/// <param name="rootDirSpec">
		/// The stringful root spec with no trailing dirsep.
		/// </param>
		/// <remarks>
		/// Note that this method may be marked security critical in some environments
		/// (e.g. addin). 
		/// </remarks>
		/// <exceptions>
		/// None generated and none caught.
		/// </exceptions>
		void SetApplicationRoot(string rootDirSpec);

		#endregion // Methods

	}
} 