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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using PlatformAgileFramework.FileAndIO.SymbolicDirectories;
using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.StringParsing;

#region Exception Shorthand

// ReSharper disable IdentifierTypo
using IFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFFileAndDirectoryIOExceptionData;
using FIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFFileAndDirectoryIOExceptionData;
// ReSharper restore IdentifierTypo
#endregion // Exception Shorthand

namespace PlatformAgileFramework.FileAndIO
{

	#region class FileAndIOUtils
	/// <summary>
	/// A utility class for performing IO to/from files and streams. This
	/// class deals with streams rather than stream writers or more specialized
	/// classes/functionality. This is the "basic" service class.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 28feb2019 </date>
	/// <description>
	/// Added history. Moved date/time stamping capabilities to here.
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	public static partial class PAFFileUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// This is the default date/time format covering everything BUT milliseconds.
		/// We now use a <c>$</c> because of a conflict a client had. The character is
		/// irrelevant, but must be just ONE character for our parsing routines to work.
		/// </summary>
		public const string DEFAULT_FILE_DATE_TIME_FORMAT_STRING
			= "{0:yyyy-MM-dd}${0:HH-mm-ss}";

		/// <summary>
		/// This is what separates a symbolic directory key from the
		/// rest of a filespec. This symbol is chosen to overload the
		/// colon after the drive letter in Windows systems to allow
		/// ease of platform porting for Windows apps that use hardcoded
		/// directories.
		/// </summary>
		public const char DIR_SYM_SEP = ':';

		/// <summary>
		/// This is loaded by the constructor, typically called by a bootstrapper.
		/// </summary>
		internal static ISymbolicDirectoryMappingDictionary s_DirectoryMappings;
		#endregion //Class Fields And Autoproperties
		#region Preload
		/// <summary>
		/// Sets the mapping dictionary, which always needs to be done if local
		/// dictionaries are not used.
		/// </summary>
		/// <param name="mappings">The dictionary.</param>
		/// <remarks>
		/// For trusted callers.
		/// </remarks>
		[SecurityCritical]
		public static void SetMappings(ISymbolicDirectoryMappingDictionary mappings)
		{
			s_DirectoryMappings = mappings;
		}
		/// <summary>
		/// Sets the mapping dictionary, which always neds to be done if local
		/// dictionaries are not used.
		/// </summary>
		/// <param name="mappings">The dictionary.</param>
		internal static void SetMappingsInternal(ISymbolicDirectoryMappingDictionary mappings)
		{
			s_DirectoryMappings = mappings;
		}
		#endregion // Preload
		/// <summary>
		/// Parses a filename to extract the creation <see cref="DateTime"/>. Designed for
		/// a very specific file name format (see below), which has the date encoded into
		/// the filename.
		/// </summary> 
		/// <param name="fileName">
		/// File name to be analyzed with or without directory spec.
		///  Not <see langword="null"/>.
		/// </param>
		/// <param name="fileBaseName">See below. Not <see langword="null"/>.</param>
		/// <returns>
		/// <see langword="null"/> if file does not contain a date/time.
		/// <see langword="null"/> if file does not contain a dot and extension,
		/// thus not conforming to our template.
		/// <see langword="null"/> if filename cannot be parsed. Parse file name
		/// before passing if parsing diagnosis is needed. (Not our job).
		/// </returns>
		/// <remarks>
		/// Originally designed for the file format <c>BASEFILENAME_yyyy-MM-dd*HH-mm-ss_(n).ext</c>
		/// where n is the version number. Now also deals with <c>BASEFILENAME_yyyy-MM-dd*HH-mm-ss-zzz_(n).ext</c>,
		/// optionally. Milliseconds need not be present, however. In the above spec, the <c>*</c> can
		/// be any SINGLE character.
		/// </remarks>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"><paramref name="fileName"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="fileBaseName"/>.</exception>
		/// </exceptions>
		[SuppressMessage("ReSharper", "CommentTypo")]
		public static DateTime? DefaultGetDateTimeFromFilename(string fileName,
			string fileBaseName)
		{
			if (fileName == null) throw new ArgumentNullException(nameof(fileName));
			if (fileBaseName == null) throw new ArgumentNullException(nameof(fileBaseName));

			// grab basename.
			var baseFileNameIndex = fileName.LastIndexOf(fileBaseName, StringComparison.Ordinal);

			try
			{
				///////////////////////////////////////////////////////////
				// Had to replace date/time parse because it did not work right on all versions
				// of the ECMA framework.
				///////////////////////////////////////////////////////////
				// Move past directory separator and name.
				var yearStartIndex = baseFileNameIndex + fileBaseName.Length;
				var yearString = fileName.Substring(yearStartIndex, 4);
				if (!int.TryParse(yearString, out var year))
					return null;

				var monthStartIndex = yearStartIndex + 5;
				var monthString = fileName.Substring(monthStartIndex, 2);
				if (!int.TryParse(monthString, out var month))
					return null;

				var dayStartIndex = yearStartIndex + 8;
				var dayString = fileName.Substring(dayStartIndex, 2);
				if (!int.TryParse(dayString, out var day))
					return null;

				var hourStartIndex = yearStartIndex + 11;
				var hourString = fileName.Substring(hourStartIndex, 2);
				if (!int.TryParse(hourString, out var hour))
					return null;

				var minuteStartIndex = yearStartIndex + 14;
				var minuteString = fileName.Substring(minuteStartIndex, 2);
				if (!int.TryParse(minuteString, out var minute))
					return null;

				var secondStartIndex = yearStartIndex + 17;
				var secondString = fileName.Substring(secondStartIndex, 2);
				if (!int.TryParse(secondString, out var second))
					return null;

				var millisecond = 0;
				var millisecondHyphenIndex = yearStartIndex + 19;
				if (fileName[millisecondHyphenIndex] == '-')
				{
					// Now we must have a number or we are malformed and we return null.
					var millisecondStartIndex = millisecondHyphenIndex + 1;
					var numberLength = StringParsingUtils.GetNumberSpan(fileName.Substring(millisecondStartIndex));
					if (numberLength == 0)
						return null;

					var millisecondString = fileName.Substring(millisecondStartIndex, numberLength);
					if (!int.TryParse(millisecondString, out millisecond))
						return null;
				}

				var dateTime = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
				dateTime += TimeSpan.FromMilliseconds(millisecond);
				return dateTime;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// This method simply tacks on a path separator (.e.g "\" or "/") if the incoming
		/// string does not have one.
		/// </summary>
		/// <param name="directoryString">
		/// String to check. Can be <see langword="null"/>.
		/// </param>
		/// <returns>
		/// Original string with a separator appended to it if it did not already
		/// have one. The original string if it did or string was <see langword="null"/>.
		/// </returns>
		public static string EnsureDirTerm(string directoryString)
		{
			if (directoryString == null) return null;
			var dirSep = PlatformUtils.GetDirectorySeparatorChar();

			if (directoryString.LastIndexOf(dirSep) != directoryString.Length - 1)
				return directoryString + PlatformUtils.GetDirectorySeparatorChar();

			return directoryString;
		}
		/// <summary>
		/// This method simply removes a terminating path separator (.e.g "\" or "/")
		/// if the incoming string has one.
		/// </summary>
		/// <param name="directoryString">
		/// String to check. Can be <see langword="null"/>.
		/// </param>
		/// <returns>
		/// Original string with a the separator removed if it had one. The original
		/// string if it did not or string was <see langword="null"/>.
		/// </returns>
		public static string EnsureNoDirTerm(string directoryString)
		{
			if (string.IsNullOrEmpty(directoryString)) return null;
			var dirSep = PlatformUtils.GetDirectorySeparatorChar();

			return directoryString[directoryString.Length - 1] == dirSep
				? directoryString.Substring(0, directoryString.Length - 1) : directoryString;
		}

		/// <summary>
		/// This method pulls off the last directory segment or file name from a path
		/// specification.
		/// </summary>
		/// <param name="directoryOrFileSpec">
		/// A string containing a directory spec (or not) that is crawled to find the
		/// last directory segment or a filename at the end. Can be rooted or not,
		/// but must be normalized for the platform. A directory spec may have a separator
		/// on the end or not.
		/// </param>
		/// <returns> 
		/// The string or <see langword="null"/>. <see langword="null"/> or <see cref="string.Empty"/>
		/// gets the same back without an exception.
		/// </returns>
		public static string GetLastDirectoryOrFile(string directoryOrFileSpec)
		{
			if (string.IsNullOrEmpty(directoryOrFileSpec))
				return null;

			var dirSep = PlatformUtils.GetDirectorySeparatorChar();

			directoryOrFileSpec = EnsureNoDirTerm(directoryOrFileSpec);

			if(StringParsingUtils.FindIsolatedCharacter(directoryOrFileSpec, dirSep) < 0)
				return directoryOrFileSpec;

			var segments = directoryOrFileSpec.Split(new [] { dirSep}, StringSplitOptions.RemoveEmptyEntries);

			return segments[segments.Length - 1];

		}
		/// <summary>
		/// This method looks up a string-ful symbolic directory symbol in a local
		/// or static dictionary. Something like "C_Drive" might return
		/// "c:" on windows.
		/// </summary>
		/// <param name="directorySymbol">
		/// A string containing no colons or directory separators (just alphanumerics and
		/// underscores) that is installed in the static or local dictionary. <see langword="null"/>
		/// returns <see langword="null"/>.
		/// </param>
		/// <param name="localDictionary">
		/// An optional local dictionary to override <see cref="ISymbolicDirectoryMappingDictionary"/>'s
		/// static method, which is called if this is <see langword="null"/>. For low-trust
		/// applications, this must be an <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>.
		/// </param>
		/// <param name="throwException">
		/// <see langword="true"/> to throw an exception if the symbol is not found. This
		/// is the default.
		/// </param>
		/// <returns> 
		/// The symbol or <see langword="null"/>. <see langword="null"/> or <see cref="string.Empty"/>
		/// gets the same back without an exception.
		/// </returns>
		/// <remarks>
		/// "SecurityCritical" version for security.
		/// </remarks>
		[SecurityCritical]
		public static string GetMappedDirectorySymbol(string directorySymbol,
			ISymbolicDirectoryMappingDictionary localDictionary = null,
			bool throwException = true)
		{
			return GetMappedDirectorySymbolInternal(directorySymbol,
				localDictionary, throwException);
		}
		/// <remarks>
		/// internal version for security.
		/// </remarks>
		internal static string GetMappedDirectorySymbolInternal(string directorySymbol,
						ISymbolicDirectoryMappingDictionary localDictionary = null,
						bool throwException = true)
		{
			if (string.IsNullOrEmpty(directorySymbol)) return null;

			var driveMapping = "";
			if (localDictionary != null)
			{
				var localInternalDictionary
					= localDictionary as ISymbolicDirectoryMappingDictionaryInternal;

				// Use the internal one if it's here.
				if (localInternalDictionary != null)
					driveMapping
						= localInternalDictionary.GetMapping(directorySymbol);
				else
					driveMapping
						= localDictionary.GetMapping(directorySymbol);
			}
			if (string.IsNullOrEmpty(driveMapping))
				driveMapping
					= s_DirectoryMappings.GetMapping(directorySymbol);

			// symbols of length 1 may be drive letters and have a literal interpretation.
			if ((string.IsNullOrEmpty(driveMapping)) && (throwException) && (directorySymbol.Length != 1))
			{
				var data = new FIOED(directorySymbol);
				throw new PAFStandardException<IFIOED>(data);
			}
			return driveMapping;
		}

		/// <summary>
		/// This method indentifies a volume specification or a UNC specification on
		/// the front of a file path. It captures the specification into a string
		/// and returns it. The returned string never has a terminating <c>DirSep</c>.
		/// This method assumes that the UNC string is on the front of the filePath
		/// (if it's not it's an error anyway) and that the same thing holds true for
		/// a volume spec, which may be any text string (not containing a UNC, please)
		/// followed by a volume separator.
		/// </summary>
		/// <param name="filePath">
		/// A file path which may contain a file name or directory spec or both. It
		/// may be absolute, rooted, or neither.
		/// </param>
		/// <param name="returnedEndOfSpec">
		/// This "out" parameter returns the 0-based index of the last character
		/// in any volume spec or UNC.
		/// </param>
		/// <returns></returns>
		/// <remarks>
		/// This method assumes that the UNC string is on the front of the filePath
		/// (if it's not it's an error anyway) and that the same thing holds true for
		/// a volume spec, which may be any text string (not containing a UNC, please)
		/// followed by a volume separator.
		/// Examples:
		/// <list type="number">
		/// <item>
		/// <description>
		/// Input: "c:" -> Output: "c:" and returnedEndOfSpec = 1.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Input: "c:/" -> Output: "c:" and returnedEndOfSpec = 1.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Input: "c:\MyDir\MySubDir" -> Output: "c:" and returnedOfSpec = 1.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Input: "\\MyShare\MyDir" -> Output: "\\Myshare" and returnedEndOfSpec = 8.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Input: "\\MyShare\" -> Output: "\\Myshare" and returnedEndOfSpec = 8.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Input: "\\MyShare" -> Output: "\\Myshare"
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// Input: "\MyDir" -> Output: <see langword="null"/>
		/// </description>
		/// </item>
		/// </list>
		/// </remarks>
		// KRM - on Golea, this will pull off an SD, then you need to check for
		// just a single letter (indicating a drive, not an SD), then check for
		// known URNs.
		internal static string GetVolumeOrUNCSpec(string filePath, out int returnedEndOfSpec)
		{
			// Assume the worst.
			returnedEndOfSpec = -1;
			// Safety valve.
			if (string.IsNullOrEmpty(filePath))
				return null;
			// Set up for matching.
			var stringLength = filePath.Length;

			// Try for a volume.
			if ((returnedEndOfSpec = filePath.IndexOf(PlatformUtils.GetVolumeSeparatorChar())) != -1) {
				// If it is a volume, all we need is to return the volume separator with the
				// preceding characters.
				if (returnedEndOfSpec > -1) {
					// Yep, just grab everything up to it.
					return filePath.Substring(0, returnedEndOfSpec + 1);
				}
			}
			// Try for a UNC.
			var volumeOrUNCMatch = StringParsingUtils.Match(
				filePath, 0, PlatformUtils.GetUNCPrefixes(),
				1, false, null);
			// If we got a UNC, work with it.
			if (volumeOrUNCMatch.NumMatches > 0) {
				// If it's just a UNC indicator, it's no good.
				if (stringLength == volumeOrUNCMatch.OffsetOfMatchEnd + 1)
					return null;
				// If it's not on the front, it's no good.
				if (volumeOrUNCMatch.OffsetOfMatchStart != 0)
					return null;
				var end = volumeOrUNCMatch.OffsetOfMatchEnd;
				// We want to go just to the the end of the share name and capture the
				// UNC prefix along with the share name. We look for the next piece of
				// punctuation.
				var punctuationMatch = StringParsingUtils.Match(filePath, end + 1,
					PlatformUtils.GetDirectoryPunctuationStrings(), 1, false, null);
				// Found punctuation?
				if (punctuationMatch.NumMatches == 1) {
					// Yep, just grab everything up to it.
					returnedEndOfSpec = punctuationMatch.OffsetOfMatchStart - 1;
					return filePath.Substring(0, returnedEndOfSpec + 1);
				}
				// Nope, didn't punctuation, the end must be just text.
				returnedEndOfSpec = stringLength - 1;
				return filePath;
			}
			// No UNC or volume.
			return null;
		}
		/// <summary>
		/// This method verifies a filename is valid. Checks certain prohibited
		/// characters and whether filename is just a drive, etc.
		/// </summary>
		/// <param name="fileNameWithOptionalPath">
		/// Filename to check. 
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the filename is OK.
		/// </returns>
		public static bool IsValidFileName(string fileNameWithOptionalPath)
		{ return PlatformUtils.ValidFileName(fileNameWithOptionalPath); }

		/// <summary>
		/// This method determines if a filename or directory specification is absolute
		/// by checking to see if the path is rooted either with a UNC or with a volume
		/// separator followed immediately with a DirSep and then by scanning for any
		/// <c>UpDir</c> (e.g. "..\") strings or any <c>CurDir</c> (e.g. ".\") characters.
		/// <c>UpDir</c> or <c>CurDir</c> characters mean the path is not absolute.
		/// The filepath is not checked otherwise for validity. A rooted path is not
		/// necessarily absolute if it begins with a <c>DirSep</c> character
		/// (e.g. "/" or "\"). An absolute path must contain either a drive or a UNC.
		/// </summary>
		/// <param name="filePath">
		/// Filepath to check. Can be just a path, or filename or both. Can be
		/// <see langword="null"/>, in which case false is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the filepath is in absolute format.
		/// </returns>
		public static bool IsPathSpecAbsolute(string filePath)
		{
			// First things first - check for null, then rooted.
			if (filePath == null)
				return false;
			// If path is not rooted, it can't be absolute.
			if (!IsPathSpecRooted(filePath))
				return false;
			// Any UpDirspec means relative.
			if (StringParsingUtils.MultiplePatternWildCardMatch(filePath, 0,
				PlatformUtils.GetDirectoryUpStrings(), null, null, false, 1, false, null).NumMatches > 0)
				return false;
			// Any CurDirspec means relative.
			if (StringParsingUtils.MultiplePatternWildCardMatch(filePath, 0,
				PlatformUtils.GetCurrentDirectoryStrings(), null, null, false, 1, false, null).NumMatches > 0)
				return false;
			// Any prefixed UNC spec means rooted.
			if (StringParsingUtils.MultiplePatternWildCardMatch(filePath, 0,
				PlatformUtils.GetUNCPrefixes(), null, null, false, 1, false, null).NumMatches > 0)
				return true;
			// We already know the path is rooted. At this point, we need to check for
			// a volume separator.
			if (filePath.IndexOf(PlatformUtils.GetVolumeSeparatorChar()) == -1)
				return false;
			// The path must be absolute at this point.
			return true;
		}

		/// <summary>
		/// This method determines if a file path is relative just by checking if it
		/// has any drectory navigation strings in it (e.g. ".\" or "..\").
		/// The filepath is not checked otherwise for validity.
		/// </summary>
		/// <param name="filePath">
		/// Filepath to check. Can be just a path, or filename or both. Can be
		/// <see langword="null"/>, in which case false is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the filename is non-<see langword="null"/> and contains relative
		/// path info..
		/// </returns>
		public static bool IsPathSpecRelative(string filePath)
		{
			// Can't do anything without a filename.
			if (filePath == null)
				return false;
			// See if we've got any relative strings.
			var match = StringParsingUtils.Match(filePath,
				PlatformUtils.GetDirectoryNavigationStrings(), 1, null);
			if (match.NumMatches > 0)
				return true;
			// Nope, we're clean.
			return false;
		}

		/// <summary>
		/// This method determines if a filename or directory specification is rooted
		/// just by checking the front for the following:
		/// <list type="number">
		/// <item>
		/// <description>
		/// A volume separator anywhere followed immediately by a DirSep (e.g. "abcxyz:\").
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// A leading DirSep, (e.g. "\").
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// A leading UNC indicator, (e.g. "\\").
		/// </description>
		/// </item>
		/// </list>
		/// The filepath is not checked otherwise for validity.
		/// </summary>
		/// <param name="filePath">
		/// Filepath to check. Can be just a path, or filename or both. Can be
		/// <see langword="null"/>, in which case false is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the filename or path is rooted.
		/// </returns>
		/// <remarks>
		/// Examples:
		/// <list type="number">
		/// <item>
		/// <description>
		/// The path ":/" is rooted (but certainly not legal).
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// The path "c:MyFile.txt" is not rooted.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// The path "c:..\..\MyDir" is not rooted.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// The path "\MyDir\..\MyOtherDir" is rooted (and useful if the Dirs exist).
		/// </description>
		/// </item>
		/// </list>
		/// </remarks>
		public static bool IsPathSpecRooted(string filePath)
		{
			// Can't do anything without a filename.
			if (filePath == null)
				return false;
			// We'll need to do some matching.
			// Any prefixed dirspec means rooted.
			var match = StringParsingUtils.MultiplePatternWildCardMatch(filePath, 0, PlatformUtils.GetDirectorySeparatorStrings(), null, null, false, 0, false, null);
			if ((match.NumMatches > 0) && (match.OffsetOfMatchStart == 0))
				return true;
			// Any prefixed UNC spec means rooted.
			if (StringParsingUtils.MultiplePatternWildCardMatch(filePath, 0,
				PlatformUtils.GetUNCPrefixes(), null, null, false, 0, false, null).NumMatches > 0)
				return true;
			//// Check for a volume.
			int index;
			// No volume at this point means not rooted.
			if ((index = filePath.IndexOf(PlatformUtils.GetVolumeSeparatorChar())) == -1)
				return false;
			// If we have a volume, we must have a directory separator immediately following it.
			match = StringParsingUtils.MultiplePatternWildCardMatch(filePath, index + 1,
				PlatformUtils.GetDirectorySeparatorStrings(), null, null, false, 1, false, null);
			if ((match.NumMatches > 0) && (match.OffsetOfMatchStart == index + 1))
				return true;
			return false;
		}

		/// <summary>
		/// Just kills the "file:///" if it is there. For MS, also takes
		/// the <c>file:\</c> off <c>file:\c:FILENAME</c>
		/// </summary>
		/// <param name="potentiallyPrefixedFilePath">
		/// Name with the unwanted URI prefix on the front. <see langword="null"/> gets blank
		/// output.
		/// </param>
		/// <returns>
		/// The string with the prefix removed or original string if no prefix.
		/// </returns>
		public static string KillLocalFileURIPrefix(string potentiallyPrefixedFilePath)
		{
			if (potentiallyPrefixedFilePath == null) return "";
			int found;
			// Microsoft hack. Of course, MS doesn't follow the spec....
			if ((found = potentiallyPrefixedFilePath.LastIndexOf(@"file:\", StringComparison.Ordinal)) >= 0)
			{
				var end = potentiallyPrefixedFilePath.Substring(found);
				if ((found = end.LastIndexOf(@":", StringComparison.Ordinal)) > 0)
				{
					return end.Substring(found - 1);
				}
			}
			if ((found = potentiallyPrefixedFilePath.LastIndexOf("file:///", StringComparison.Ordinal)) < 0) return potentiallyPrefixedFilePath;
			return (potentiallyPrefixedFilePath.Substring(found + 8));
		}
		/// <summary>
		/// Just kills the directory (if present), leaving the filename with possible extension.
		/// </summary>
		/// <returns>
		/// <see langword="null"/> gets <see langword="null"/>.
		/// </returns>
		public static string KillDirectory(string fileNameWithPossibleDirectory)
		{
			if (fileNameWithPossibleDirectory == null)
				return null;
			fileNameWithPossibleDirectory = NormalizeFilePath(fileNameWithPossibleDirectory);
			int found;
			if ((found = fileNameWithPossibleDirectory.LastIndexOf(PlatformUtils.GetDirectorySeparatorChar())) < 0) return fileNameWithPossibleDirectory;
			return (fileNameWithPossibleDirectory.Substring(found + 1, fileNameWithPossibleDirectory.Length - found - 1));
		}
		/// <summary>
		/// Just kills the dot and the file extension if it is there.
		/// </summary>
		/// <returns>
		/// The string with the extension removed.
		/// </returns>
		public static string KillExtension(string fileNameWithPossibleExtension)
		{
			int found;
			if ((found = fileNameWithPossibleExtension.LastIndexOf('.')) < 0) return fileNameWithPossibleExtension;
			return (fileNameWithPossibleExtension.Substring(0, found));
		}

		/// <summary>
		/// This method is fairly simple - it just takes a filename and replaces
		/// all of the "alternative" punctuation strings with standard strings
		/// (e.g. "../", "./" and "/" on MS are replaced with "..\", ".\" and "\").
		/// Any leading UNC is not touched. An optional conversion of the
		/// non-punctuation characters to uppercase is performed with the culture
		/// on the current thread.
		/// </summary>
		/// <param name="filePath">
		/// A file path which may contain punctuation strings.
		/// </param>
		/// <param name="toUpper">
		/// <see langword="true"/> causes the string to be converted to upper case. This is
		/// useful for comparing two file paths that might have different cases and
		/// different styles of punctuation (e.g. mixed "/" and "\") but might
		/// still be considered equal.
		/// </param>
		/// <remarks>
		/// <para>
		/// This routine is probably OS-dependent. What we really should do is
		/// scan the filepath for each occurrence of each type of punctuation
		/// and replace it it with the "standard" punctuation. Instead we just
		/// replace the "alternative" directory separator anywhere it occurs with
		/// the "standard" directory separator. This is a shortcut, since there may
		/// be some conceivable OS which does not represent <c>CurDir</c>s and
		/// <c>UpDirs</c>s with dots followed by <c>DirSeps</c>.
		/// </para>
		/// <para>
		/// This routine is helpful is combating the "canonicalization" security
		/// problem in file access. It is best to normalize filePaths before using
		/// them so crackers can't use an alternative string to end up with the
		/// same target filePath.
		/// </para>
		/// </remarks>
		// todo (KRM) Also, a filePath comparison should really be made by examining
		// the filePaths "in place" so as not to perform unnecessary allocations. We
		// really need a "FilePathCompare(FP1, FP2)" that does the comparison in place
		// with an FP map. We'll do this when we get more parser routines converted.
		public static string NormalizeFilePath(string filePath, bool toUpper = false)
		{
		    // Move past any possible UNC.
			var uNcEnd = StringParsingUtils.Match(filePath, 0, PlatformUtils.GetUNCPrefix()
				, 1, false, null).OffsetOfMatchEnd;
			// We assume no UNC is just one character.
			if (uNcEnd == 0)
				uNcEnd = -1;
			// Hand everything off to our utility.......
			var normalizedFilePath = StringParsingUtils.Replace(filePath, uNcEnd + 1,
				PlatformUtils.GetAlternativeDirectorySeparatorChar(),
				PlatformUtils.GetDirectorySeparatorChar(), -1, out _);
			if (!toUpper)
				return normalizedFilePath;
			// We are assuming that any UNCs can be passed through ToUpper() without
			// harming control/special characters.
			return normalizedFilePath.ToUpper();
		}
		/// <summary>
		/// This method just maps a leading drive letter or symbol to a directory before
		/// calling <see cref="NormalizeFilePath"/>. It does not convert the path to
		/// upper case characters. Something like <c>c:\MyDirectory\MyFile</c> is converted
		/// to something like <c>/usr/c_drive/MyDirectory/Myfile</c>.
		/// </summary>
		/// <param name="filePath">
		/// A file path which may contain punctuation strings and/or a leading drive letter.
		/// <see langword="null"/> returns <see langword="null"/>. The filepath MUST have a
		/// directory separator after the symbol (be rooted). <c>file:</c> is always rejected
		/// as a symbol and the original string is simply returned.
		/// </param>
		/// <param name="localDictionary">
		/// An optional local dictionary to override <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>'s
		/// static dictionary, which is used if this is <see langword="null"/>. For low-trust
		/// applications, this must be an <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>.
		/// </param>
		/// <param name="throwException">
		/// <see langword="true"/> to throw an exception if the symbol is not found. This
		/// is the default.
		/// </param>
		/// <returns> 
		/// The normalized path. <see langword="null"/> or <see cref="string.Empty"/>
		/// gets the same back.
		/// </returns>
		/// <remarks>
		/// "SecurityCritical" version for security.
		/// </remarks>
		[SecurityCritical]
		public static string NormalizeFilePathWithDriveOrDirectory(string filePath,
			ISymbolicDirectoryMappingDictionary localDictionary = null,
			bool throwException = true)
		{
			return NormalizeFilePathWithDriveOrDirectoryInternal(filePath,
				localDictionary, throwException);
		}

		/// <exception cref="ArgumentNullException">Thrown for null or blank filepath.</exception>
		/// <remarks>
		/// Internal version for security.
		/// </remarks>
		internal static string NormalizeFilePathWithDriveOrDirectoryInternal(string filePath,
			ISymbolicDirectoryMappingDictionary localDictionary = null,
			bool throwException = true)
		{
			if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

			var index = filePath.IndexOf("file:", StringComparison.OrdinalIgnoreCase);
			// file URI is raw.
			if (index != -1) return filePath.Substring(index + 5);

			index = StringParsingUtils.FindIsolatedCharacter(filePath, DIR_SYM_SEP);

			// Easy if no symbol.
			if (index < 0)
			{
				var normalizedFilePath =  NormalizeFilePath(filePath);
				return normalizedFilePath;
			}


			var baseFilePath = filePath.Substring(index + 1);
			var directorySymbol = filePath.Substring(0, index);

			directorySymbol = WalkDirectorySymbol(directorySymbol, localDictionary, throwException);

			var fullFilePath = directorySymbol;
			if (!IsPathSpecRooted(baseFilePath))
				fullFilePath += PlatformUtils.GetDirectorySeparatorChar();
			fullFilePath += baseFilePath;

			return NormalizeFilePath(fullFilePath);
		}
		/// <summary>
		/// This utility walks a directory spec, looking up segments, one at a time,
		/// and combining them into a full physical directory path spec WITHOUT a
		/// trailing separator.
		/// </summary>
		/// <param name="directorySymbol">
		/// Directory specification, which may contain other symbols when looked up.
		/// </param>
		/// <param name="localDictionary">optional local symbol dictionary.</param>
		/// <param name="throwException">
		/// Default is true to throw an exception when a symbol is not found.
		/// </param>
		/// <returns>
		/// Returns a <see cref="string.Empty"/> if there are no directory symbols on
		/// the front of the spec. Otherwise a directory PREFIX is returned that can
		/// be used to combine with the remainder of the file path.
		/// </returns>
		public static string WalkDirectorySymbol(string directorySymbol,
			ISymbolicDirectoryMappingDictionary localDictionary = null,
			bool throwException = true)
		{
			if (string.IsNullOrEmpty(directorySymbol))
				throw new ArgumentNullException(nameof(directorySymbol));

			IList<string> directorySegments = null;

			// We must build up the directory spec. if we are recursive.
			// ReSharper disable once JoinDeclarationAndInitializer
			string finishedSpec;

			// Cater to the fact that directory symbols can be nested.
			while (true)
			{
				var directorySpec = GetMappedDirectorySymbolInternal(directorySymbol, localDictionary, throwException);

			    var symbolAndRemainder = SeparateDirectorySymbol(directorySpec);

				if (directorySegments == null) directorySegments = new Collection<string>();

				// Quit if no embedded symbols.
				if (symbolAndRemainder == null)
				{
					if (!string.IsNullOrEmpty (directorySpec))
						directorySegments.Add (directorySpec);
					break;
				}


				// Load the physical segment.
				directorySegments.Add(symbolAndRemainder[1]);
				// Load the symbol for further processing.
				directorySymbol = symbolAndRemainder[0];

				// All done?
				if (string.IsNullOrEmpty(directorySymbol)) break;
			}

			// gotta' build it up.
			finishedSpec = "";
			foreach (var dirSegment in directorySegments.Reverse())
			{
				finishedSpec += dirSegment;
			}
			finishedSpec = EnsureNoDirTerm(finishedSpec);
			return finishedSpec;
		}
		/// <summary>
		/// Checks to see if a file path has a URL (not just a URI) by looking
		/// up in our catalog of URLs. (e.g. "http:", "https:", "ftp:")
		/// </summary>
		/// <param name="filePath">
		/// Filepath which might have a URL on the front or anywhere inside. This
		/// argument can be <see langword="null"/> or blank, in which case <see langword="false"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if we find a URl.
		/// </returns>
		public static bool PathHasEmbeddedURLSpec(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return false;
			// Just find a match.
			if (StringParsingUtils.ContainsAPattern(filePath, 0, PlatformUtils.GetURLSchemeStrings())) { return true; }
			return false;
		}

		/// <summary>
		/// This method separates the directory off the front end of a
		/// filespec, if it exists. It returns it along with the remainder of the
		/// file path - an empty string if there is none. Something like
		/// <c>C_Drive:\MyDirectory\MyFile</c> is converted to <c>C_Drive:\MyDirectory</c> and
		/// <c>C_Drive:\MyDirectory\Myfile</c>. No directory separator conversions are applied.
		/// </summary>
		/// <param name="filePath">
		/// A file path which may contain punctuation strings and/or a leading drive letter.
		/// <see langword="null"/> returns <see langword="null"/>. <see cref="string.Empty"/>
		/// returns two blank strings.
		/// </param>
		/// <returns> 
		/// A two-element array containing the separated directory spec with no
		/// trailing separator followed by the filename. <see langword="null"/>
		/// or <see cref="string.Empty"/> gets the same back.
		/// If the first element in the returned array is empty, this means that the
		/// second element should be used literally - the input was just a filename.
		/// </returns>
		public static string[] SeparateDirectoryFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;
            filePath = NormalizeFilePath(filePath);
            var index = filePath.LastIndexOf(PlatformUtils.GetDirectorySeparatorChar());
            var filepathLength = filePath.Length;

            var output = new string[2];
            
            // No directory?
            if (index < 0)
            {
                output[0] = string.Empty;
                output[1] = filePath;
            }
            // Got a directory, output a filename if there is one.
            else
            {
                output[0] = filePath.Substring(0, index + 1);

                if (index + 1 == filepathLength)
                    output[1] = string.Empty;
                else
                    output[1] = filePath.Substring(index + 1);
            }

            return output;
        }
		/// <summary>
		/// This method either replaces a directory or adds one at the beginning
		/// of a file path.
		/// </summary>
		/// <param name="directoryString">
		/// Directory to add or replace old one with. Can be <see langword="null"/>,
		/// which just causes any directory spec to be removed.
		/// </param>
		/// <param name="fileNames">
		/// These are file names, with or without a leading directory spec.
		/// <see langword = "null"/> returns an empty list.
		/// </param>
		/// <returns>
		/// Original files with directory either added, replaced or removed.
		/// </returns>
		public static IList<string> SetOrReplaceDirectorySpecOnFiles(
			string directoryString, IEnumerable<string> fileNames)
		{
			var outputFileNames = new List<string>();
			if (fileNames == null)
				return outputFileNames;
 
			var sep = PlatformUtils.GetDirectorySeparatorChar();

			foreach (var filePath in fileNames)
			{
				if (filePath == null)
					continue;

				var newFilePath = KillDirectory(filePath);

				if (!string.IsNullOrEmpty(directoryString))
				{

					newFilePath = directoryString
						+ sep + newFilePath;
				}
				outputFileNames.Add(newFilePath);
			}
			return outputFileNames;
		}
		/// <summary>
		/// This method separates the symbolic directory symbol off the front end of a
		/// filespec, if it exists. It returns it along with the remainder of the
		/// file path - an empty string if there is none. Something like
		/// <c>C_Drive:\MyDirectory\MyFile</c> is converted to <c>C_Drive</c> and
		/// <c>\MyDirectory\Myfile</c>. No directory separator conversions are applied.
		/// </summary>
		/// <param name="fileOrDirectorySpec">
		/// A file path which may contain punctuation strings and/or a leading drive letter.
		/// <see langword="null"/> returns <see langword="null"/>. If the spec contains a
		/// leading <see cref="DIR_SYM_SEP"/> with no symbol in front of it, a 
		/// <see cref="string.Empty"/> is returned for the directory symbol.
		/// </param>
		/// <returns> 
		/// A two-element array containing the separated directory symbol with no
		/// trailing separator followed by the remainder of the filespec. <see langword="null"/>
		/// or <see cref="string.Empty"/> gets the same back. If the filename contains
		/// no <see cref="DIR_SYM_SEP"/> the method returns <see langref="null"/>,
		/// indicating the spec has no symbolic directory attached and can be used directly.
		/// If the first element in the returned array is empty, this means that the
		/// second element should be used literally - there is no more lookup to do.
		/// </returns>
		public static string[] SeparateDirectorySymbol(string fileOrDirectorySpec)
        {
            if (string.IsNullOrEmpty(fileOrDirectorySpec)) return null;
            int index;
            if ((index = StringParsingUtils.FindIsolatedCharacter(fileOrDirectorySpec, DIR_SYM_SEP)) < 0)
                return null;

            var output = new string[2];

            bool literal = (fileOrDirectorySpec.Length == index + 1);

            // Map to "default" directory.
            if ((index == 0) || literal)
            {
                output[0] = string.Empty;
            }
            else
            {
                output[0] = fileOrDirectorySpec.Substring(0, index);
            }

            if (literal)
                output[1] = fileOrDirectorySpec;
            else
                output[1] = fileOrDirectorySpec.Substring(index + 1);
            return output;
        }
        #endregion // FileAndIOUtils
    }
}