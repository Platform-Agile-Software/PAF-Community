//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010-2019 Icucom Corporation
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

using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Platform;

namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// Helpers for working with files. Added back, since there is more flexibility
	/// on the phones now.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17feb2019 </date>
	/// <description>
	/// Check for directory existence before doing operations.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04Jun2016 </date>
	/// <description>
	/// New.
	/// Put only the stuff in here that will work in core.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe.
	/// </threadsafety>
	public static class PAFStorageServiceExtensions
	{
		/// <summary>
		/// Gets the size of a file by actually opening it. We don't have
		/// the luxury of something like fileinfo on all platforms, since
		/// we aren't always writing to files.
		/// </summary>
		/// <param name="storageService">
		/// One of us.
		/// </param>
		/// <param name="filePath">
		/// filename, with directory spec. on the front.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <c>filePath</c> is <see langword="null"/> or blank.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFFileAndDirectoryIOExceptionData}">
		/// Message = <see cref="PAFFileAndIOExceptionMessageTags.FILE_NOT_FOUND"/>
		/// is thrown if the file does not exist.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFFileAndDirectoryIOExceptionData}">
		/// Message = <see cref="PAFFileAndIOExceptionMessageTags.ERROR_OPENING_FILE"/>
		/// is thrown if there is a problem opening the file.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// This method won't handle shares. (//)
		/// </remarks>
		public static long PAFFileSize(this IPAFStorageService storageService,
			string filePath)
		{
			if (string.IsNullOrEmpty( filePath))
				throw new ArgumentNullException(nameof(filePath));

			if(!storageService.PAFFileExists(filePath))
			{
				var data = new PAFFileAndDirectoryIOExceptionData(filePath);
				throw new PAFStandardException<IPAFFileAndDirectoryIOExceptionData>(
					data, PAFFileAndIOExceptionMessageTags.FILE_NOT_FOUND);
			}

			long size;
			IPAFStorageStream stream = null;
			try
			{
				stream = storageService.PAFOpenFile(filePath, PAFFileAccessMode.READONLY);

				size = stream.PAFLength;
			}
			catch (Exception ex)
			{
				var data = new PAFFileAndDirectoryIOExceptionData(filePath);
				throw new PAFStandardException<IPAFFileAndDirectoryIOExceptionData>(
					data, PAFFileAndIOExceptionMessageTags.ERROR_OPENING_FILE, ex);
			}
			finally
			{
				stream?.Dispose();
			}
			return size;
		}

		/// <summary>
		/// Ensures that a directory exists. Creates it if it doesn't.
		/// </summary>
		/// <param name="storageService">
		/// One of us.
		/// </param>
		/// <param name="directoryPath">
		/// The directory to ensure the existence of. Can have a mapped symbol
		/// on the front, can have a terminating separator or not. If it is
		/// <see cref="string.Empty"/> we are assumed to be referencing the CWD
		/// and we return <see langword="true"/> Cannot be <see langword="null"/>.
		/// </param>
		/// <param name="createRecursively">
		/// If <see langword="false"/>, only the last directory segment will be
		/// created. If <see langword="true"/>, the path will be built segment
		/// by segment.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">directoryPath</exception>
		/// <exception cref="PAFStandardException{IPAFFileAndDirectoryIOExceptionData}">
		/// Message = <see cref="PAFFileAndIOExceptionMessageTags.DIRECTORY_NOT_FOUND"/>
		/// is thrown if we are not recursing and the directory path up to the leaf
		/// directory to be created does not exist.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFFileAndDirectoryIOExceptionData}">
		/// Message = <see cref="PAFFileAndIOExceptionMessageTags.ERROR_CREATING_DIRECTORY"/>
		/// is thrown if directory creation fails. This exception wraps one thrown by the OS.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// This method won't handle shares. (//)
		/// </remarks>
		public static void PAFEnsureDirectoryExists(this IPAFStorageService storageService,
			string directoryPath, bool createRecursively = true)
		{
			if (directoryPath == null)
				throw new ArgumentNullException(nameof(directoryPath));
			if (directoryPath == string.Empty)
				return;

			var DS = PlatformUtils.GetDirectorySeparatorChar();

			directoryPath = PAFFileUtils.EnsureNoDirTerm(directoryPath);
			directoryPath = PAFFileUtils.NormalizeFilePathWithDriveOrDirectoryInternal(directoryPath);
			var segments = directoryPath.Split(new[] { DS }, StringSplitOptions.None);

			var pathToEvaluate = directoryPath;
			if (segments.Length > 1)
			{
				// If we have at least two segments (impossible to have just one), tack the first
				// on (might be empty on unix) the front of the second. 
				pathToEvaluate = segments[0] + DS + segments[1];
			}

			var segIndex = 1;
			while (true)
			{
				if (!storageService.PAFDirectoryExists(pathToEvaluate))
				{
					if ((segIndex == segments.Length - 1) || (createRecursively))
						try
						{
							storageService.PAFCreateDirectory(pathToEvaluate);
						}
						catch (Exception ex)
						{
							var data = new PAFFileAndDirectoryIOExceptionData(pathToEvaluate);
							throw new PAFStandardException<IPAFFileAndDirectoryIOExceptionData>(
								data, PAFFileAndIOExceptionMessageTags.ERROR_CREATING_DIRECTORY, ex);
						}
					else
					{
						var data = new PAFFileAndDirectoryIOExceptionData(pathToEvaluate);
						throw new PAFStandardException<IPAFFileAndDirectoryIOExceptionData>(
							data, PAFFileAndIOExceptionMessageTags.DIRECTORY_NOT_FOUND);
					}
				}
				if (segIndex == segments.Length - 1) return;
				segIndex++;
				pathToEvaluate += DS + segments[segIndex];
			}
		}

		/// <summary>
		/// Empties a directory by deleting files.
		/// </summary>
		/// <param name="storageService">
		/// One of us.
		/// </param>
		/// <param name="directoryPath">
		/// The directory to remove files from. Can have a mapped symbol
		/// on the front, can have a terminating separator or not.
		/// Cannot be <see langword="null"/> or <see cref="string.Empty"/>.
		/// </param>
		/// <param name="emptyRecursively">
		/// If <see langword="false"/>, only the top-level directory will be
		/// emptied.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">directoryPath</exception>
		/// We do not catch other exceptions that come up from the implementation
		/// for lack of permission, file in use, etc.
		/// </exceptions>
		/// <remarks>
		/// This method won't handle shares (//). Will do nothing if directory does
		/// not exist.
		/// </remarks>
		public static void PAFEmptyDirectoryOfFiles(this IPAFStorageService storageService,
			string directoryPath, bool emptyRecursively = false)
		{
			if (string.IsNullOrEmpty(directoryPath))
				throw new ArgumentNullException(nameof(directoryPath));

			if (!storageService.PAFDirectoryExists(directoryPath))
				return;

			var filesNames = storageService.PAFGetFileNames(directoryPath);
			if (filesNames != null)
			{
				foreach (var fileName in filesNames)
				{
					storageService.PAFDeleteFile(fileName);
				}
			}

			if (emptyRecursively)
			{
				var directories = storageService.PAFGetDirectoryNames(directoryPath);
				if (directories != null)
				{
					foreach (var directory in directories)
					{
						storageService.PAFEmptyDirectoryOfFiles(directory);
					}
				}
			}
		}
		/// <summary>
		/// Empties a directory by deleting all directories recursively down its tree.
		/// Files must be deleted down the tree first, so this is done.
		/// </summary>
		/// <param name="storageService">
		/// One of us.
		/// </param>
		/// <param name="directoryPath">
		/// The directory to clear. Can have a mapped symbol
		/// on the front, can have a terminating separator or not.
		/// Cannot be <see langword="null"/> or <see cref="string.Empty"/>. 
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">directoryPath</exception>
		/// We do not catch other exceptions that come up from the implementation
		/// for lack of permission, file in use, etc.
		/// </exceptions>
		/// <remarks>
		/// This method won't handle shares (//). Will do nothing if directory does
		/// not exist.
		/// </remarks>
		public static void PAFEmptyDirectoryOfDirectories(this IPAFStorageService storageService,
			string directoryPath)
		{
			if (string.IsNullOrEmpty(directoryPath))
				throw new ArgumentNullException(nameof(directoryPath));

			if (!storageService.PAFDirectoryExists(directoryPath))
				return;

			// First clean out all the files.
			storageService.PAFEmptyDirectoryOfFiles(directoryPath);

			var directoryNames = storageService.PAFGetDirectoryNames(directoryPath);

			if (directoryNames == null) return;
			foreach (var directoryName in directoryNames)
			{
				storageService.PAFEmptyDirectoryOfDirectories(directoryName);
				storageService.PAFDeleteDirectory(directoryName);
			}
		}
		/// <summary>
		/// Empties a directory by deleting all files recursively down its tree..
		/// </summary>
		/// <param name="storageService">
		/// One of us.
		/// </param>
		/// <param name="directoryPath">
		/// The directory to empty and delete. Can have a mapped symbol
		/// on the front, can have a terminating separator or not. Cannot be <see langword="null"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">directoryPath</exception>
		/// We do not catch other exceptions that come up from the implementation
		/// for lack of permission, file in use, etc.
		/// </exceptions>
		/// <remarks>
		/// This method won't handle shares (//). Will do nothing if directory does
		/// not exist.
		/// </remarks>
		public static void PAFEmptyAndDeleteDirectory(this IPAFStorageService storageService,
			string directoryPath)
		{
			if (directoryPath == null)
				throw new ArgumentNullException(nameof(directoryPath));
			if (directoryPath == string.Empty)
				return;

			if (!storageService.PAFDirectoryExists(directoryPath))
				return;

			// First delete everything below.
			storageService.PAFEmptyDirectoryOfDirectories(directoryPath);

			// Now ok to delete me.
			storageService.PAFDeleteDirectory(directoryPath);
		}
	}
}