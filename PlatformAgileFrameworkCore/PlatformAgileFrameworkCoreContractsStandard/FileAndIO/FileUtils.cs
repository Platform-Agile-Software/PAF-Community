//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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

// TODO -KRM. Most stuff in here is invalidated by the new security model in 4.0.
// TODO - this stuff needs to be reworked for 4.0 (and cleaned up).

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using PlatformAgileFramework.FileAndIO.SymbolicDirectories;

#region Imported Namespaces

using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.StringParsing;
#endregion // Imported Namespaces

#region Exception Shorthand

using IFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFFileAndDirectoryIOExceptionData;
using FIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFFileAndDirectoryIOExceptionData;
#endregion // Exception Shorthand

namespace PlatformAgileFramework.FileAndIO
{

	#region class FileAndIOUtils
	/// <summary>
	/// A utility class for performing IO to/from files and streams. This
	/// class deals with streams rather than streamwriters or more specialized
	/// classes/functionality. This is the "basic" service class.
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	public partial class FileUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
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
		protected internal static ISymbolicDirectoryMappingDictionary s_DirectoryMappings;
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
		protected internal static void SetMappingsInternal(ISymbolicDirectoryMappingDictionary mappings)
		{
			s_DirectoryMappings = mappings;
		}
		#endregion // Preload
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
			if (directoryString == null) return null;
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
		/// The string or <see langword="null"/>. <see langword="null"/> or <see cref="String.Empty"/>
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
		/// This method looks up a stringful symbolic directory symbol in a local
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
		/// The symbol or <see langword="null"/>. <see langword="null"/> or <see cref="String.Empty"/>
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
		/// The string with the prefix removed or origininal string if no prefix.
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
		/// Just kills the dot and the file extension if it is there.
		/// </summary>
		/// <returns>
		/// The string with the prefix removed.
		/// </returns>
		public static string KillExtension(string fileNameWithPossibleExtension)
		{
			int found;
			if ((found = fileNameWithPossibleExtension.LastIndexOf('.')) < 0) return fileNameWithPossibleExtension;
			return (fileNameWithPossibleExtension.Substring(0, found ));
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
		/// useful for comparing two filepaths that might have different cases and
		/// different styles of punctuation (e.g. mixed "/" and "\") but might
		/// still be considered equal.
		/// </param>
		/// <remarks>
		/// <para>
		/// This routine is probably OS-dependent. What we really should do is
		/// scan the filepath for each occurrance of each type of punctuation
		/// and replace it it with the "standard" punctuation. Instead we just
		/// replace the "alternative" directory separator anywhere it occurs with
		/// the "standard" directory separator. This is a shortcut, since there may
		/// be some conceivable OS which does not represent <c>CurDir</c>s and
		/// <c>UpDirs</c>s with dots followed by <c>DirSeps</c>.
		/// </para>
		/// <para>
		/// This routine is helpful is combatting the "canonicalization" security
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
        /// The normalized path. <see langword="null"/> or <see cref="String.Empty"/>
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
			// reject file URI
			if (index != -1) return filePath;

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
			if (string.IsNullOrEmpty(directorySymbol)) throw new ArgumentNullException("directorySymbol");

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
        /// <see langword="null"/> returns <see langword="null"/>.
        /// </param>
        /// <returns> 
        /// A two-element array containing the separated directory spec with no
        /// trailing separator followed by the filename. <see langword="null"/>
        /// or <see cref="String.Empty"/> gets the same back.
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
        /// <see cref="String.Empty"/> is returned for the directory symbol.
        /// </param>
        /// <returns> 
        /// A two-element array containing the separated directory symbol with no
        /// trailing separator followed by the remainder of the filespec. <see langword="null"/>
        /// or <see cref="String.Empty"/> gets the same back. If the filename contains
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
        #region OldStuff
        ///// <summary>
        ///// Checks to see if a file path has a URI by looking up in our catalog
        ///// of URLs. and also our catalog of non-URL URIs.
        ///// (e.g. "http:", "https:", "ftp:", "mailto:").
        ///// </summary>
        ///// <param name="filePath">
        ///// Filepath which might have a URI on the front or anywhere inside. This
        ///// argument can be <see langword="null"/> or blank, in which case <see langword="false"/> is returned.
        ///// </param>
        ///// <returns>
        ///// <see langword="true"/> if we find a URI.
        ///// </returns>
        //// Use this method on Golea filespecs to determine if they are URIs, not SDs.
        //public static bool PathHasEmbeddedURISpec(string filePath)
        //{
        //    if (String.IsNullOrEmpty(filePath))
        //        return false;
        //    // Just find a match of either.
        //    if (StringParsingUtils.ContainsAPattern(filePath, 0, PlatformUtils.GetURLSchemeStrings())) { return true; }
        //    if (StringParsingUtils.ContainsAPattern(filePath, 0, PlatformUtils.GetURISchemeStrings())) { return true; }
        //    return false;
        //}

        //public static StringCollection SearchDirectory(string directory, string filemask, bool searchSubdirectories)
        //{
        //    StringCollection collection = new StringCollection();
        //    FileAndIOUtils.SearchDirectory(directory, filemask, collection, searchSubdirectories);
        //    return collection;
        //}

        //public static StringCollection SearchDirectory(string directory, string filemask)
        //{
        //    return SearchDirectory(directory, filemask, true);
        //}

        ///// <summary>
        ///// Finds all files which match the mask <see paramref name="fileMask"/> in the set of directories in
        ///// <cref="directoryList"/>. The discovered files are added to the <cref="directoryCollection"/>.
        ///// </summary>
        ///// <param name="directoryList">
        ///// List of directory paths to search. Method returns safely if this is <see langword="null"/>.
        ///// </param>
        ///// <param name="fileMask">
        ///// Mask for selecting files, (e.g., *.doc).
        ///// </param>
        ///// <param name="directoryCollection">
        ///// Collection of directories to add to. If this parameter is passed as <see langword="null"/>, a
        ///// new StringCollection is created and returned. If it is not <see langword="null"/>, the collection
        ///// is extended with any directories located by the method.
        ///// </param>
        ///// <param name="recurseSubdirectories">
        ///// Tells whether to search down nested subdirectories to find the files.
        ///// </param>
        ///// <returns>
        ///// A <cref="StringCollection"/> containing a list of directories.
        ///// </returns>
        ///// <remarks>
        ///// If <cref="directoryCollection"/> is not <see langword="null"/>, it is extended and the original
        ///// reference is returned in the method return value. No new copies or allocations are
        ///// performed. If the <cref="directoryCollection"/>  is passed as <see langword="null"/>, the returned
        ///// reference is a freshly allocated <cref="StringCollection"/>.
        ///// </returns>
        //public static StringCollection SearchDirectoryList(string[] directoryList, string fileMask,
        //    StringCollection directoryCollection, bool recurseSubdirectories)
        //{
        //    // Safety valve.
        //    if (directoryList == null)
        //        return null;

        //    // We'll need a string collection if we don't have one.
        //    if (directoryCollection == null) {
        //        directoryCollection = new StringCollection();
        //    }

        //    // Scan directories one at a time.
        //    foreach (String dir in directoryList) {
        //        SearchDirectory(dir, fileMask, directoryCollection, recurseSubdirectories);
        //    }
        //    return directoryCollection;
        //}

        ///// <summary>
        ///// Finds all files which are valid to the mask <code>filemask</code> in the path
        ///// <code>directory</code> and all subdirectories if searchSubdirectories
        ///// is true. The found files are added to the <cref="collection"/>.
        ///// </summary>
        //public static void SearchDirectory(string directory, string filemask, StringCollection collection,
        //    bool searchSubdirectories)
        //{
        //    try {
        //        string[] file = Directory.GetFiles(directory, filemask);
        //        foreach (string f in file) {
        //            collection.Add(f);
        //        }

        //        if (searchSubdirectories) {
        //            string[] dir = Directory.GetDirectories(directory);
        //            foreach (string d in dir) {
        //                SearchDirectory(d, filemask, collection, searchSubdirectories);
        //            }
        //        }
        //    }
        //    catch (Exception e) {
        //        //fixme				IMessageService messageService = (IMessageService)ProteusServiceManager.Services.GetService(typeof(IMessageService));
        //        //				messageService.ShowError(e, );
        //        PAFException mpex = new PAFException("Can't access directory " + directory, e);
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, mpex);
        //    }
        //}
        ///// <summary>
        ///// This method checks the status of a given file. The client passes the
        ///// filename along with the requested operation and/or access mode and the
        ///// method returns an indicator of whether the client will be able to perform
        ///// the required operation on the given file. The method takes into account
        ///// file existance, READONLY status and various permissions needed to access
        ///// the file in question. It reconciles both Windows file permissions and
        ///// .Net FileIO permissions and determines if the client can do what it
        ///// wants to do.
        ///// </summary>
        ///// <param name="fileName">
        ///// The name of the file the client wishes to access.
        ///// </param>
        ///// <param name="fileAccessRequested">
        ///// The <see cref="FileAccess"/> enumeration Type indicating what the client
        ///// wants to do with the file (read and/or write). This should be set to
        ///// request the rights that are needed for any requested <see paramref name="fileMode"/>
        ///// if they are known in advance. There are isolated cases where the clients
        ///// don't know what they want to do ahead of time. This is sometimes the case
        ///// with<see cref="FileMode.OpenOrCreate"/>, for example. Otherwise,
        ///// <see paramref name="fileAccess"/> should be explicitly set for this method to
        ///// do the most effective pre-screening.
        ///// </param>
        ///// <param name="fileMode">
        ///// The <see cref="FileMode"/> enumeration Type indicating what the client
        ///// wants to do with the file. The corresponding access bits should be
        ///// set in the <see paramref name="fileAccessRequested"/> parameter if they are
        ///// known in advance.
        ///// </param>
        ///// <param name="fileStatusCode">
        ///// The <see cref="FileStatusCode"/> enumeration Type indicating the Type
        ///// of problem (or none) encountered in the proposed operation. This
        ///// out parameter is set to 0 for a successful operation.
        ///// </param>
        ///// <param name="fileAttributes">
        ///// The <see cref="FileAttributes"/> enumeration Type indicating the
        ///// attributes of the file if it was successfully inspected. It is set
        ///// to 0 to indicate the file was not found.
        ///// </param>
        ///// <param name="fileProbeStatus">
        ///// <para>
        ///// This code will be non-zero if the requestor has no permission to receive
        ///// file status information. All other "out" values will be set to 0.
        ///// </param>
        ///// <param name="fileIOPermissionAccessNeeded">
        ///// This return value will be loaded with a non-zero value if the client
        ///// did not have required requested permissions for the attempted operation.
        ///// Caller can compare this value with <see paramref name="fileIOPermissionAccessCurrentlyGranted"/>
        ///// to determine deficiencies in permissions.
        ///// </param>
        ///// <param name="fileIOPermissionAccessCurrentlyGranted">
        ///// This return value will be loaded with a non-zero value if the client
        ///// did not have required requested permissions for the attempted operation.
        ///// Caller can compare this value with <see paramref name="fileIOPermissionAccessNeeded"/>
        ///// to determine deficiencies in permissions.
        ///// </param>
        ///// <returns>
        ///// <see langword="true"/> if all is well. <see langword="false"/> means that you need to check
        ///// the "out" params.
        ///// </returns>
        ///// <remarks>
        ///// <para>
        ///// We only work with an absolute directory spec at this time. No CWDs or
        ///// AppDomain load directory searches, etc..
        ///// </para>
        ///// </remarks>
        //public static bool CheckFileStatus(string fileName, FileAccess fileAccessRequested,
        //    FileMode fileMode, out FileStatusCode fileStatusCode, out FileAttributes fileAttributes,
        //    out Enum fileProbeStatus, out FileIOPermissionAccess fileIOPermissionAccessNeeded,
        //            out FileIOPermissionAccess fileIOPermissionAccessCurrentlyGranted)
        //{
        //    // Security barrier - do not load data into the caller's memory space even momentarily......
        //    FileStatusCode fileStatusCodeOut;
        //    FileAttributes fileAttributesOut;
        //    FileIOPermissionAccess fileIOPermissionAccessNeededOut;
        //    FileIOPermissionAccess fileIOPermissionAccessCurrentlyGrantedOut;
        //    SecurityUtilsInternal.FileSecurityProbeStatus fileProbeStatusOut;
        //    bool status = CheckFileStatusInternal(fileName, fileAccessRequested, fileMode,
        //        out fileStatusCodeOut, out fileAttributesOut, out fileProbeStatusOut,
        //            out fileIOPermissionAccessNeededOut, out fileIOPermissionAccessCurrentlyGrantedOut);

        //    // Default return.
        //    if ((fileProbeStatusOut == 0) || ((fileIOPermissionAccessCurrentlyGrantedOut & FileIOPermissionAccess.PathDiscovery) != 0)) {
        //        fileProbeStatus = fileProbeStatusOut;
        //        fileStatusCode = fileStatusCodeOut;
        //        fileAttributes = fileAttributesOut;
        //        fileIOPermissionAccessNeeded = fileIOPermissionAccessNeededOut;
        //        fileIOPermissionAccessCurrentlyGranted = fileIOPermissionAccessCurrentlyGrantedOut;
        //    }
        //    // Security return - requestor has no priviledges to receive information.
        //    else {
        //        fileProbeStatus = SecurityUtilsInternal.FileSecurityProbeStatus.GeneralSecurityFailure;
        //        fileStatusCode = 0;
        //        fileAttributes = 0;
        //        fileIOPermissionAccessNeeded = 0;
        //        fileIOPermissionAccessCurrentlyGranted = 0;
        //    }
        //    return status;
        //}

        ///// <summary>
        ///// Method to prepare a file by creaeting it if it does not exist and truncating
        ///// it if it does.
        ///// </summary>
        ///// <param name="fileName">
        ///// Name of the file to be opened.
        ///// </param>
        ///// <param name="throwException">
        ///// Set to true for this method to throw an exception if cannot be opened.
        ///// </param>
        ///// <returns>
        ///// <see langword="true"/> if successful.
        ///// </returns>
        //public static Boolean CreateOrTruncateFile(string fileName, bool throwException)
        //{
        //    // Return from GetOrCheckFileStream.
        //    Enum enumOut;
        //    // Call our main workhorse routine.
        //    var stream = InteractiveGetFileStream(fileName, FileMode.Create, FileAccess.Write,
        //        false, false, throwException, out enumOut);
        //    if (stream != null) { stream.Close(); return true; }
        //    return false;
        //}
        ///// <summary>
        ///// Method to open a file for reading.
        ///// </summary>
        ///// <param name="fileName">
        ///// Name of the file to be opened.
        ///// </param>
        ///// <param name="throwException">
        ///// Set to true for this method to throw a ProteusGeneralFileError exception if the file does not exist
        ///// or cannot be opened.
        ///// </param>
        ///// <returns> 
        ///// A valid <see cref="FileStream"/> if successful.
        ///// </returns>
        //public static FileStream OpenFileForRead(string fileName, bool throwException)
        //{
        //    // Return from GetOrCheckFileStream.
        //    Enum enumOut;
        //    // Call our main workhorse routine.
        //    return InteractiveGetFileStream(fileName, FileMode.Open, FileAccess.Read,
        //        false, false, throwException, out enumOut);
        //}
        ///// <summary>
        ///// Method to open a file for writing.
        ///// </summary>
        ///// <param name="fileName">
        ///// Name of the file to be opened.
        ///// </param>
        ///// <param name="throwException">
        ///// Set to true for this method to throw an exception cannot be opened.
        ///// </param>
        ///// <returns>
        ///// A valid <see cref="FileStream"/> if successful.
        ///// </returns>
        //public static FileStream OpenFileForWrite(string fileName, bool throwException)
        //{
        //    // Return from GetOrCheckFileStream.
        //    Enum enumOut;
        //    // Call our main workhorse routine.
        //    return InteractiveGetFileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write,
        //        false, false, throwException, out enumOut);
        //}


        #region Internal Helpers
        //// Silly internal helper that we shouldn't need if MS share defaults
        //// worked right. Please pass me a correct absolute filename.
        //internal static FileStream InternalGetFileStream(string fileName,
        //    FileMode fileModeRequested, FileAccess fileAccessRequested)
        //{
        //    // Convert the old-fashion FileAccess to the new-fangled FileSystemRights.
        //    FileSystemRights fileSystemRights = 0;
        //    if ((fileAccessRequested & FileAccess.Read) != 0)
        //        fileSystemRights |= FileSystemRights.Read;
        //    if ((fileAccessRequested & FileAccess.Write) != 0)
        //        fileSystemRights |= FileSystemRights.Write;
        //    // MS default share option for FileStream(string, FileMode, FileAccess)
        //    // is supposed to be Read, but it's Read/Write. So we have to set it
        //    // manually.
        //    return new FileStream(fileName, fileModeRequested, fileSystemRights,
        //        FileShare.Read, 8192, FileOptions.None, null);
        //}
        #region Internal SecurityCritical Helpers
        ///// <summary>
        ///// This method checks the status of a given file. The client passes the
        ///// filename along with the requested operation and/or access mode and the
        ///// method returns an indicator of whether the client will be able to perform
        ///// the required operation on the given file. The method takes into account
        ///// file existance, READONLY status and various permissions needed to access
        ///// the file in question. It reconciles both Windows file permissions and
        ///// .Net FileIO permissions and determines if the client can do what it
        ///// wants to do. The method is internal and is designed to be used by fully
        ///// trusted callers. The caller can determine what returned information should
        ///// be returned to the end client/user based on the trust level of that end
        ///// client/user.
        ///// </summary>
        ///// <param name="fileName">
        ///// The name of the file the client wishes to access.
        ///// </param>
        ///// <param name="fileAccessRequested">
        ///// The <see cref="FileAccess"/> enumeration Type indicating what the client
        ///// wants to do with the file (read and/or write). This should be set to
        ///// request the rights that are needed for any requested <see paramref name="fileMode"/>
        ///// if they are known in advance. There are isolated cases where the clients
        ///// don't know what they want to do ahead of time. This is sometimes the case
        ///// with<see cref="FileMode.OpenOrCreate"/>, for example. Otherwise,
        ///// <see paramref name="fileAccess"/> should be explicitly set for this method to
        ///// do the most effective pre-screening.
        ///// </param>
        ///// <param name="fileMode">
        ///// The <see cref="FileMode"/> enumeration Type indicating what the client
        ///// wants to do with the file. The corresponding access bits should be
        ///// set in the <see paramref name="fileAccessRequested"/> parameter if they are
        ///// known in advance.
        ///// </param>
        ///// <param name="fileStatusCode">
        ///// The <see cref="FileStatusCode"/> enumeration Type indicating the Type
        ///// of problem (or none) encountered in the proposed operation. This
        ///// out parameter is set to 0 for a successful operation.
        ///// </param>
        ///// <param name="fileAttributes">
        ///// The <see cref="FileAttributes"/> enumeration Type indicating the
        ///// attributes of the file if it was successfully inspected. It is set
        ///// to 0 to indicate the file was not found.
        ///// </param>
        ///// <param name="fileSecurityProbeStatus">
        ///// <para>
        ///// This code will be non-zero if the operation attempted fails due to a
        ///// security problem. If the return is <see cref="SecurityUtilsInternal.FileSecurityProbeStatus"/>,
        ///// this indicates that the framework has not been installed with sufficient
        ///// permissions to examine files and the system administrator should be
        ///// contacted. Any other non-zero return value will indicate a client/user
        ///// security failure and the <see paramref name="fileIOPermissionNeeded"/> and
        ///// <see paramref name="fileIOPermissionGranted"/> will be set to indicate needed
        ///// and granted security permissions for the TestBench.
        ///// </para>
        ///// <para>
        ///// If this parameter is returned as a non-zero value, this generally means
        ///// that any information regarding file status should not be returned to
        ///// untrusted clients. If this parameter is returned as 0, any information
        ///// regarding file state could normally be returned to the clients, since
        ///// they have permission to have it.
        ///// </para>
        ///// </param>
        ///// <param name="fileIOPermissionNeeded">
        ///// This return value will be loaded with a non-zero value if the client
        ///// did not have required requested permissions for the attempted operation.
        ///// Caller can compare this value with <see paramref name="fileIOPermissionGranted"/>
        ///// to determine deficiencies in permissions.
        ///// </param>
        ///// <param name="fileIOPermissionGranted">
        ///// This return value will be loaded with a non-zero value if the client
        ///// did not have required requested permissions for the attempted operation.
        ///// Caller can compare this value with <see paramref name="fileIOPermissionNeeded"/>
        ///// to determine deficiencies in permissions.
        ///// </param>
        ///// <returns>
        ///// <see langword="true"/> if all is well. <see langword="false"/> means that you need to check
        ///// the error code.
        ///// </returns>
        ///// <remarks>
        ///// <para>
        ///// This method is an attempt to pre-qualify file operations to the extent
        ///// possible so we don't have to simply try a file operation and catch
        ///// exceptions of various types when things fail. There are so many conditions
        ///// to check for and trap that we thought we'd do it in one place and
        ///// include the security checks here, too.
        ///// </para>
        ///// <para>
        ///// If the filespec is not absolute, the AppDomain load directory is prepended.
        ///// </para>
        ///// <para>
        ///// With all the MSWindows code access stuff in here, it's almost sure to be
        ///// platform-specific, so it is marked as such.
        ///// </para>
        ///// </remarks>
        //// KRM todo - put in the ACL stuff now that it's
        //// accessible from managed code. Insist that the filePath be absolute and
        //// canonical.
        //[SecurityCriticalAttribute()]
        //[SecurityTreatAsSafeAttribute()]
        //internal static bool CheckFileStatusInternal(string fileName, FileAccess fileAccessRequested,
        //    FileMode fileMode, out FileStatusCode fileStatusCode, out FileAttributes fileAttributes,
        //     out SecurityUtilsInternal.FileSecurityProbeStatus fileSecurityProbeStatus,
        //        out FileIOPermissionAccess fileIOPermissionNeeded,
        //            out FileIOPermissionAccess fileIOPermissionGranted)
        //{
        //    // This is the Windows file permission we will need.
        //    FileAccess fileAccessNeeded = 0;
        //    // Working permission set.
        //    FileIOPermissionAccess fileIOPermissionNeededWorking
        //        = FileIOPermissionAccess.NoAccess;
        //    // Needed to figure out if it's there already instead of using attributes.
        //    bool doesFileExist = false;
        //    // Preset output vars and we'll OR in things as we work our way through
        //    // various permissions, access rights needed, etc. as we go down toward
        //    // the bottom. These must be preset here, since we sometimes bail out
        //    // early in cases of bad params, etc..
        //    fileStatusCode = 0;
        //    fileAttributes = 0;
        //    fileSecurityProbeStatus = 0;
        //    fileIOPermissionGranted = FileIOPermissionAccess.NoAccess;
        //    fileIOPermissionNeeded = FileIOPermissionAccess.NoAccess;

        //    #region Unrestricted Permission Region
        //    ///////////////////////////////////////////////////////////////////////////////
        //    //// In this section, we don't do anything but inspect things. ////////////////
        //    ///////////////////////////////////////////////////////////////////////////////
        //    /////////////////   Unrestricted permission.   //////////////////////
        //    // We may fail due to lack of proper FileIOPermission, but we want to
        //    // report details of the file's status back to the caller anyway. Thus
        //    // we want to give ourselves the permissions we need to snoop around
        //    // for the file and figure out it's status. We have to do this here
        //    // because we need to see if the file exists before we can evaluate
        //    // a request made with FileMode.OpenOrCreate, for example, to see what
        //    // permissions we have to include in a stack walk.
        //    //// KRM todo this needs to be replaced with whatever directories
        //    //// the administrator has assigned to MP.
        //    FileIOPermission fIOP
        //        = new FileIOPermission(PermissionState.Unrestricted);
        //    try {
        //        fIOP.Assert();
        //        //// Examine the file with full priviledge.
        //        // if file not absolute, make it so. 
        //        if (!FileAndIOUtils.IsPathSpecAbsolute(fileName)) { fileName = GetAbsoluteBaseFilePath(fileName, false); }
        //        if (FileAndIOUtils.IsValidFileName(fileName)) {
        //            // We have wide-open security, so PlatformUtils will get us
        //            // what we need.
        //            if ((doesFileExist = PlatformUtils.FileExists(fileName)))
        //                // Get the attributes if it's here.
        //                fileAttributes = File.GetAttributes(fileName);
        //        }
        //        else
        //            fileStatusCode = FileStatusCode.BadFileName;
        //    }
        //    catch (Exception ex) {
        //        // Set the error and prepare to go home. If we can't do the assert,
        //        // there is something wrong with the framework permission settings.
        //        fileSecurityProbeStatus = SecurityUtilsInternal.FileSecurityProbeStatus.FrameworkSecurityAccessFailure;
        //        // Unknown error is always returned to the client in case of a framework
        //        // security problem.
        //        fileStatusCode = FileStatusCode.UnknownError;
        //        // this indicates an invalid install. The server has to know about this.
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, ex);
        //    }
        //    finally {
        //        try {
        //            // We have to have a try/catch because the assert may not be in
        //            // effect. Assert will disappear upon return to the caller, but
        //            // we are cautious about security, so we remove it anyway.
        //            CodeAccessPermission.RevertAssert();
        //        }
        //        catch (Exception ex) {
        //            ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, ex);
        //        }
        //    }
        //    // Bail out if we had a problem. This is a problem we can't recover
        //    // from since we can't examine anything.
        //    if (fileStatusCode != 0)
        //        return false;
        //    ///////////////////   End Unrestricted permission.   /////////////////
        //    #endregion

        //    ///////////////////////////////////////////////////////////////////////////////
        //    //////// Determine if there are preconceived read/write requirements. /////////
        //    ///////////////////////////////////////////////////////////////////////////////
        //    // Definate read?
        //    if ((fileAccessRequested & FileAccess.Read) != 0) {
        //        // If we definately want read, stick it in.
        //        fileAccessNeeded |= FileAccess.Read;
        //        fileIOPermissionNeededWorking |= FileIOPermissionAccess.Read;
        //    }
        //    // Definate write?
        //    if ((fileAccessRequested & FileAccess.Write) != 0) {
        //        // If we definately want write, stick it in.
        //        fileAccessNeeded |= FileAccess.Write;
        //        fileIOPermissionNeededWorking |= FileIOPermissionAccess.Write;
        //    }
        //    ///////////////////////////////////////////////////////////////////////////////
        //    //// In this section, we process malformed or inconsistent requests. //////////
        //    ///////////////////////////////////////////////////////////////////////////////
        //    //// Things we can always get out early for and report without checking
        //    //// any security issues. These are basically requests that can be
        //    //// recognized as malformed without checking any file state or
        //    //// permissions. These are safe to return because the user doesn't
        //    //// get any information that they didn't have already.
        //    if (fileStatusCode == FileStatusCode.BadFileName) {
        //        return false;
        //    }
        //    // We can also get out early if the user asked for an inconsistent combination.
        //    // We do this as we check various attributes of a request.
        //    if (fileMode == FileMode.Append) {
        //        fileAccessNeeded |= FileAccess.Write;
        //        fileIOPermissionNeededWorking |= FileIOPermissionAccess.Write;
        //        // Can't read with append.
        //        if ((fileAccessRequested & FileAccess.Read) != 0) {
        //            fileStatusCode = FileStatusCode.AppendReadInconsistency;
        //            return false;
        //        }
        //    }
        //    if (fileMode == FileMode.Truncate) {
        //        fileAccessNeeded |= FileAccess.Write;
        //        fileIOPermissionNeededWorking |= FileIOPermissionAccess.Write;
        //        // Can't read with truncate.
        //        if ((fileAccessRequested & FileAccess.Read) != 0) {
        //            fileStatusCode = FileStatusCode.TruncateReadInconsistency;
        //            return false;
        //        }
        //    }
        //    // CreateNew requires write acess to a non-existing file.
        //    if (fileMode == FileMode.CreateNew) {
        //        if (doesFileExist) {
        //            fileStatusCode = FileStatusCode.FileAlreadyExists;
        //            return false;
        //        }
        //        fileAccessNeeded |= FileAccess.Write;
        //        fileIOPermissionNeededWorking |= FileIOPermissionAccess.Write;
        //    }
        //    // Results for OpenOrCreate depend on file existance.
        //    if ((fileMode == FileMode.OpenOrCreate) && (!doesFileExist)) {
        //        // If it's not here, we'll definately need write access to create
        //        // it.
        //        fileAccessNeeded |= FileAccess.Write;
        //        fileIOPermissionNeededWorking |= FileIOPermissionAccess.Write;
        //    }
        //    // File's gotta' be here for Open or Truncate.
        //    if ((fileMode == FileMode.Open) || (fileMode == FileMode.Truncate)) {
        //        if (!doesFileExist) {
        //            fileStatusCode = FileStatusCode.FileNotFound;
        //            return false;
        //        }
        //        if (fileMode == FileMode.Open) {
        //            fileIOPermissionNeededWorking |= FileIOPermissionAccess.Read;
        //            fileAccessNeeded |= FileAccess.Read;
        //        }
        //        else {
        //            fileIOPermissionNeededWorking |= FileIOPermissionAccess.Write;
        //            fileAccessNeeded |= FileAccess.Write;
        //        }
        //    }

        //    ///////////////////////////////////////////////////////////////////////////////
        //    ////////// Process access errors that require knowledge of file state. ////////
        //    ///////////////////////////////////////////////////////////////////////////////
        //    //// The following set of failures are not always exclusive, but we treat them
        //    //// as if they are and return a single error code, otherwise it's just too
        //    //// complicated.
        //    // Problem if it's a directory.
        //    if ((fileAttributes & FileAttributes.Directory) != 0) {
        //        // Here we are going to assume that the user needs path discovery
        //        // to know we are only a directory. Note that PathDiscovery is
        //        // orthogonal to read/write/append, since files could be being
        //        // located by looking up Environment.SpecialDirectory or using
        //        // CWD or any of a number of things.
        //        fileIOPermissionNeededWorking |= FileIOPermissionAccess.PathDiscovery;
        //        fileStatusCode = FileStatusCode.FileIsDirectory;
        //    }
        //    // Any operations definately needing a write?
        //    else if ((fileMode == FileMode.Create) || (fileMode == FileMode.CreateNew)
        //             || (fileMode == FileMode.Append) || (fileMode == FileMode.Truncate)) {
        //        fileAccessNeeded |= FileAccess.Write;
        //        if (fileMode == FileMode.Append)
        //            fileIOPermissionNeededWorking |= FileIOPermissionAccess.Append;
        //        else
        //            fileIOPermissionNeededWorking |= FileIOPermissionAccess.Write;
        //        // Can't do it with readonly file.
        //        if (doesFileExist && (fileAttributes & FileAttributes.ReadOnly) != 0) {
        //            fileStatusCode = FileStatusCode.FileIsReadonly;
        //        }
        //    }

        //    ///////////////////////////////////////////////////////////////////////////////
        //    /////////////// Figure out if we have the required permissions. ///////////////
        //    ///////////////////////////////////////////////////////////////////////////////
        //    //// In this section we demand the needed permissions. If we fail, we then
        //    // call GetCurrentFilePermissions() to report to client just what we are
        //    // currently allowed to do.
        //    try {
        //        FileIOPermission neededFIOP
        //            = new FileIOPermission(fileIOPermissionNeededWorking, fileName);
        //        neededFIOP.Demand();
        //    }
        //    catch (Exception ex) {
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, ex);
        //        //// We failed.
        //        // Set security probe status for client security failure.
        //        fileSecurityProbeStatus = SecurityUtilsInternal.FileSecurityProbeStatus.ClientSecurityAccessFailure;
        //        // Hand back IO permissions needed.
        //        fileIOPermissionNeeded = fileIOPermissionNeededWorking;
        //        // Set the granted permissions for the caller before we go.
        //        fileIOPermissionGranted = GetGrantedFileIOPermissions(fileName);
        //        return false;
        //    }
        //    // If we got here, we didn't have a security failure. If we have a non-zero
        //    // fileStatusCode, return false, otherwise we're OK.
        //    if (fileStatusCode != 0)
        //        return false;
        //    return true;
        //}
        ///// <summary>
        ///// This method checks the granted security permissions of a given file.
        ///// The caller passes the filename and the method does a stack walk to see
        ///// what accesses the user/client has.
        ///// </summary>
        ///// <param name="fileName">
        ///// The name of the file the user/client wishes to access.
        ///// </param>
        ///// <returns>
        ///// The <see cref="FileIOPermissionAccess"/> enum with the appropraite bits
        ///// set, depending which combination of write, read,append or path discovery
        ///// permissions the user/client currently has.
        ///// </returns>
        //internal static FileIOPermissionAccess GetGrantedFileIOPermissions(string fileName)
        //{
        //    // This is the file permission that we fill in as we go.
        //    FileIOPermissionAccess fileIOPermissionCurrentlyGranted
        //        = FileIOPermissionAccess.NoAccess;
        //    // Try the 4 types of permissions to see what we have permissions for.
        //    // Can't use SecurityManager.IsGranted() here since it only checks
        //    // the immediate caller - we have to actually do the stack walks.
        //    try {
        //        (new FileIOPermission(FileIOPermissionAccess.Append, fileName)).Demand();
        //        // If we got it, OR it in.
        //        fileIOPermissionCurrentlyGranted |= FileIOPermissionAccess.Append;
        //    }
        //    catch (Exception ex) {
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, ex);
        //    }
        //    try {
        //        (new FileIOPermission(FileIOPermissionAccess.Read, fileName)).Demand();
        //        // If we got it, OR it in.
        //        fileIOPermissionCurrentlyGranted |= FileIOPermissionAccess.Read;
        //    }
        //    catch (Exception ex) {
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, ex);
        //    }
        //    try {
        //        (new FileIOPermission(FileIOPermissionAccess.Write, fileName)).Demand();
        //        // If we got it, OR it in.
        //        fileIOPermissionCurrentlyGranted |= FileIOPermissionAccess.Write;
        //    }
        //    catch (Exception ex) {
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, ex);
        //    }
        //    try {
        //        (new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fileName)).Demand();
        //        // If we got it, OR it in.
        //        fileIOPermissionCurrentlyGranted |= FileIOPermissionAccess.PathDiscovery;
        //    }
        //    catch (Exception ex) {
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, ex);
        //    }
        //    return fileIOPermissionCurrentlyGranted;
        //}
        //public static System.Enum DelegatedIOOP<TIOArg>(FileAndIOUtils.IOOperationDelegate<TIOArg> fileDelegate,
        //    ref TIOArg IOArg, PAFError mPError, System.Boolean announce,
        //        System.Boolean recover, System.Boolean throwException)
        //{
        //    // We are going to be optimistic.
        //    FileStatusCode fileStatusCode = FileStatusCode.Success;
        //    // Recover implies announce.
        //    if(recover) announce = true;
        //    // We will assign this a generic error if the client has not specified
        //    // anything. This is the one we will use in an exception, if thrown.
        //    if(mPError == null) mPError = FileAndIOUtils.FileOperationError;
        //    // For reporting inner exceptions that may have bubbled up from the CLR
        //    // and we want to wrap and report.
        //    Exception innerException = null;
        //    // Build a string to print the filename.
        //    string detailString = "File: " + "NULL" + LTRMN;
        //    if(fileName != null)
        //        detailString = "File: " + fileName + LTRMN;
        //    // This is the working file name we use for the recovery operation.
        //    string testFileName = fileName;
        //    // For recovering with FileErrorAndRecover.
        //    bool tryAgain = false;

        //    do {
        //        // Attempt the operation.
        //        try {
        //            fileDelegate(testFileName);
        //            // we're OK, so get out.
        //            fileName = testFileName;
        //            return FileStatusCode.Success;
        //        }
        //        // Catch any exceptions here so we can translate them.
        //        catch(Exception e) {
        //            fileStatusCode = FileStatusCode.UnknownError;
        //            // We do not export any information about security errors and
        //            // we do not continue.
        //            ////KRM todo - we want a log entry on the server, though.
        //            if(e is SecurityException) goto errorexit;
        //            // If it's not a security exception, it's OK to wrap it.
        //            innerException = e;
        //            if(!announce) goto errorexit;
        //        }
        //        if(announce) tryAgain
        //            = FileErrorAndRecover(ref testFileName, mPError, recover, true);
        //    } while(tryAgain);

        ////// We have not been successful with the file operation, so come here
        //// for final error handling.
        //errorexit:
        //    // Generate the exception if this is what the caller wants.
        //    if(throwException)
        //        FileAndIOUtils.IOErrorHandler.ProcessError(
        //            mPError, innerException,
        //                null, true, detailString, null);
        //    // Else just return the failure code.
        //    return fileStatusCode;
        //}

        ///// <summary>
        ///// This little helper reports an error to the user and optionally allows
        ///// the user to recover. It presents an interactive dialog to the user.
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="mPError"></param>
        ///// <param name="recover"></param>
        ///// <param name="specifyNewFile"></param>
        ///// <returns>
        ///// <see langword="true"/> to retry the operation again. This means that the user has
        ///// specified a new fileName, closed a file, deleted a file or some other
        ///// recovery operation that allows the operation to be tried again by the
        ///// caller.
        ///// </returns>
        //private static bool FileErrorAndRecover(ref string fileName, PAFException mPError, bool recover, bool specifyNewFile)
        //{
        //    // We will assign this a generic error if the client has not specified
        //    // anything. This is what will be displayed to the user.
        //    if (mPError == null) {
        //        mPError = new PAFFileIOException("File operation error.");
        //    }

        //    // Used to capture details of the error if appropriate.
        //    // We prepare the information to present to the user by pulling it from
        //    // the PAFError and from the detail string.
        //    string detailString = "File: " + fileName + LTRMN;
        //    string userQuery = mPError.Message + " " + detailString;

        //    // Get the default UI service.
        //    IPAFUIService iUI = (IPAFUIService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFUIService), false);

        //    if (!recover) {
        //        using (IMessageAndDispatch iMAD = iUI.GetMessageAndDispatch()) {
        //            iMAD.PresentToUser(userQuery);
        //        }
        //        return false;
        //    }

        //    //// Cook up an actual request to recover.
        //    // If allowing new filename, tell user.
        //    if (specifyNewFile)
        //        userQuery += "Enter Filename ";
        //    userQuery += "(CR to retry 'I' to ignore): ";
        //    string userResponse;
        //    using (IMessageAndStringResponse iMASR = iUI.GetMessageAndStringResponse()) {
        //        iMASR.PresentToUser(userQuery, out userResponse);
        //    }

        //    // If just a CR, just try again.
        //    if (userResponse.Length == 0) {
        //        return true;
        //    }
        //    if ((userResponse.Length == 1) && (userResponse[0] == 'I')) {
        //        // No recovery.
        //        return false;
        //    }
        //    if (specifyNewFile) {
        //        // Reload and try again.
        //        fileName = userResponse;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// This method takes a rooted filePath (see <c>IsPathRooted()</c> which may
        ///// contain directory navigation characters (e.g. "./", ".\", "../", "..\")
        ///// and converts it to a rooted path with no navigation characters. In order
        ///// for this to be possible, the navigation characters must not cause the
        ///// the directory to go past the top (e.g. "\..\MyDir" would be illegal).
        ///// </summary>
        ///// <param name="rootedFilePath">
        ///// This is incoming rooted path that is to be navigated/converted.
        ///// </param>
        ///// <param name="throwException">
        ///// If <see langword="true"/>, the method will throw an exception
        ///// </param>
        ///// <returns>
        ///// A normalized (see <c>NormalizeFilePath()</c>), rooted filepath with no
        ///// directory navigation characters. Will return <see langword="null"/> is there is
        ///// a problem and <see paramref name="throwException"/> is <see langword="false"/>.
        ///// </returns>
        //// The strategy for this method is to pull the filePath apart into a
        //// prefix containing no navigation characters and a remainder portion
        //// which may have navigation characters and then send both into
        //// GetAbsoluteFilePath() which does the real work.
        //public static string FilePathCrawler(string rootedFilePath, bool throwException)
        //{
        //    string outputRootedFilePath = null;
        //    // In case we have to translate an inner exception.
        //    Exception innerException = null;
        //    // Default error string is nothing.
        //    string errorString = "";
        //    // null gets us out.....
        //    if (rootedFilePath == null) {
        //        errorString = LTRMN + "rootedFilePath: " + "(NULL)" + LTRMN;
        //        goto errorout;
        //    }
        //    // If we aren't rooted, we aren't going anywhere.
        //    if (!IsPathSpecRooted(rootedFilePath)) {
        //        errorString = LTRMN + "rootedFilePath: " + rootedFilePath + " (NOT ROOTED)" + LTRMN;
        //        goto errorout;
        //    }
        //    // First get normalized.
        //    rootedFilePath = NormalizeFilePath(rootedFilePath, false);
        //    // Let's see if we have to do anything more.
        //    MatchDescriptor navigationMatch
        //        = StringParsingUtils.Match(rootedFilePath, 0,
        //        PlatformUtils.GetDirectoryNavigationStrings(), 1, false, null);
        //    // Get out if no more DirNav's.
        //    if (navigationMatch.NumMatches <= 0)
        //        return rootedFilePath;
        //    // The front end is guaranteed to have no DirNav characters.
        //    string nonrelativeFrontEnd
        //        = rootedFilePath.Substring(0, navigationMatch.OffsetOfMatchStart);
        //    // The rest is guaranteed to have a directory navigation string at the
        //    // beginning. This is important, since if it had a root (e.g. "\") at the
        //    // beginning, <c>GetAbsoluteFilePath()</c> would call us to crawl it and we'd
        //    // get an infinite recursion.
        //    string theRelativeRemainder
        //        = rootedFilePath.Substring(navigationMatch.OffsetOfMatchStart);
        //    // Now just pass through GetAbsoluteFilePath() with no forced volume (false).
        //    try {
        //        outputRootedFilePath = GetAbsoluteFilePath(theRelativeRemainder, nonrelativeFrontEnd, false, throwException);
        //        if (outputRootedFilePath == null) {
        //            errorString = String.Format("\nrootedFilePath: {0} (Problem in GetAbsoluteFilePath())", rootedFilePath);
        //            throw new PAFFileIOException(errorString);
        //        }
        //    }
        //    catch (Exception e) {
        //        // Repackage for our own exception.
        //        innerException = e;
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, e);
        //    }
        //    return outputRootedFilePath;

        //errorout:
        //    if (throwException) {
        //        PAFFileNotFoundException mpex = new PAFFileNotFoundException(String.Format("Bad filepath. {0}", errorString), innerException);
        //        ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, mpex);
        //        throw mpex;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Transforms an incoming filePath specification, which can be just a path
        ///// or a path with a filename at the end. An absolute filePath is formed
        ///// by using the "current AppDomain base directory" (See <c>PlatformUtils</c>).
        ///// For a normal application this is the same as the"installed directory",
        ///// which is usually something like "c:\MyApp\bin". This will be the same
        ///// as the "current working directory" if the application has not changed
        ///// the CWD. For other than the base applications AppDomain, the default
        ///// base directory can be different if the AppDomain has been created
        ///// with a specific <c>CodeBase</c> property, which is what we use to
        ///// establish the default load directory.
        ///// </summary>
        ///// <param name="filePath">
        ///// Valid filename which may contain embedded OS-specific
        ///// relative directory specification strings (e.g.  "..\" or "../") mixed.
        ///// If the <see paramref name="filePath"/> is already an absolute file path, it is
        ///// "normalized" to remove directory navigation characters and use "standard"
        ///// directory separators. If <see paramref name="filePath"/> is already rooted,
        ///// as defined by <c>IsPathSpecRooted</c> the load directory is ignored.
        ///// This argument can be <see langword="null"/> or blank, in which case <see langword="null"/>
        ///// is returned.
        ///// </param>
        ///// <param name="throwException">
        ///// If set to <see langword="true"/>, an exception will be thrown if an invalid path is
        ///// supplied.
        ///// </param>
        ///// <returns>
        ///// Absolute path if one could be constructed, <see langword="null"/> if not and
        ///// <see paramref name="throwException"/> is <see langword="false"/>.
        ///// </returns>
        //public static string GetAbsoluteBaseFilePath(string filePath, bool throwException)
        //{
        //    // The strategy for this method is to get the current base directory and
        //    // then pass it down to <c>GetAbsoluteFilePath()<c> with the filepath.
        //    // <c>GetAbsoluteFilePath()<c> does all the dirtywork.
        //    return GetAbsoluteFilePath(filePath, PlatformUtils.GetCurrentAppDomainBaseDirectory(),
        //        true, throwException);
        //}

        ///// <summary>
        ///// Just an interface to <see cref="GetAbsoluteFilePath(string, string, bool, bool)"/>
        ///// with fixed parameters. This method passes <see langword="null"/> for "rootedFilePath,
        ///// <see langword="false"/> for addVolume, <see langword="true"/> for "throwException".
        ///// </summary>
        ///// <param name="filePath">
        ///// <see cref="GetAbsoluteFilePath(string, string, bool, bool)"/>
        ///// </param>
        ///// <returns>
        ///// <see cref="GetAbsoluteFilePath(string, string, bool, bool)"/>
        ///// </returns>
        //public static string GetAbsoluteFilePath(string filePath)
        //{
        //    return GetAbsoluteFilePath(filePath, null, false, true);
        //}

        ///// <summary>
        ///// Constructs an absolute path from incoming path specifications that may
        ///// contain embedded OS-specific relative directory specification strings
        ///// (e.g.  "..\" or "../") mixed. It navigates the path specification to
        ///// find the root and then builds the path down from there. The output
        ///// absolute path has either a volume specification on it or a UNC.
        ///// The method also calls <see cref="NormalizeFilePathWithDrive"/> to
        ///// map drive letters and symbolic directories and remove <c>file:///</c>.
        ///// </summary>
        ///// <param name="filePath">
        ///// Valid filename which may contain an absolute or relative path specification.
        ///// If this path is absolute, it is simply returned after processing to
        ///// remove relative directory navigation characters and "normalize" the
        ///// directory separators. Can be <see langword="null"/> or blank, in which case
        ///// <see langword="null"/> is returned.
        ///// </param>
        ///// <param name="rootedFilePath">
        ///// This argument is ignored if filePath is itself a "rooted" specification
        ///// as defined in <c>IsPathRooted</c>. Otherwise it is used as the "current"
        ///// directory from which to calculate an absolute file path based on the
        ///// relative specification in filePath. It does not have to be an absolute
        ///// file path itself, but must be rooted with either a UNC or a leading
        ///// <c>DirSep</c> (e.g. "/" or "\"). If it has a leading <c>DirSep</c>
        ///// (no UNC), the current volume will be prepended to create the absolute path.
        ///// The current volume is determined by pulling the volume spec off the front
        ///// of the CWD (accessed through <c>PlatformUtils.GetWorkingDirectory()</c>).
        ///// This filePath cannot be relative, as defined in <c>IsPathSpecRelative</c>.
        ///// </param>
        ///// <param name="addVolume">
        ///// By setting this parameter to <see langword="false"/>, a default volume will not be
        ///// added to the front of the returned filepath if one does not exist
        ///// on the incoming <see paramref name="rootedFilePath"/>.
        ///// </param>
        ///// <param name="throwException">
        ///// This argument deterimines whether bad strings will simply cause a <see langword="null"/>
        ///// to be returned or an exception to be thrown. If this argument is <see langword="true"/>,
        ///// an exception will be thrown under the following conditions:
        ///// <list type="number">
        ///// <item>
        ///// <description>
        ///// rootedFilePath is present, but not rooted.
        ///// </description>
        ///// </item>
        ///// <item>
        ///// <description>
        ///// rootedFilePath is present, but is relative.
        ///// </description>
        ///// </item>
        ///// <item>
        ///// <description>
        ///// "current working volume" is needed but not available.
        ///// </description>
        ///// </item>
        ///// <item>
        ///// <description>
        ///// An internal error in the string matching routines
        ///// </description>
        ///// </item>
        ///// </list>
        ///// </param>
        ///// <returns>
        ///// Absolute path if the navigation to a valid directory or file can be made, 
        ///// <see langword="null"/> if not. The file or directory do not have to exist. If the
        ///// navigation of the specified path brings the reference above the root
        ///// directory of the drive, <see langword="null"/> is returned if <see paramref name="throwException"/>
        ///// is <see langword="false"/>. This is different than the MS GetFullPath() function which
        ///// truncates the navigation to the root and starts back down from there. In
        ///// this implementation, there can be intermediate directories in a relative
        ///// path string that do not exist and the final path will still be formed,
        ///// as long as those directories are not part of the final subdirectory
        ///// hierarchy. For example:
        ///// <c>..\\..\\..\\MissingDir\\../main/MainProgram/bin/CoreDll.dll</c>
        ///// will form the directory <c>c:\MyProject\Source\main\MainProgram\bin\CoreDll.dll</c>
        ///// if the rooted directory was <c>c:\MyProject\Source\someDir0\someDir1\someDir2</c>.
        ///// This is the way MS's GetFullPath works and we need this behavior for backward
        ///// compatibility.
        ///// </returns>
        ///// <remarks>
        ///// This method has the side effect of transforming any embedded directory
        ///// separator characters to the "standard" directory separator character for
        ///// the current platform. If, for some reason, the client wishes to transform
        ///// a filePath that is already absolute, but with mixed dirsep characters,
        ///// simply pass it through this method.
        ///// </remarks>
        //// todo KRM - this has turned into a "kitchen sink" method. Break it up
        //// when we can.
        //public static string GetAbsoluteFilePath(string filePath,
        //    string rootedFilePath, bool addVolume, bool throwException)
        //{
        //    // Default error string is nothing.
        //    string errorString = "";
        //    ///////////////////////////////////////////////////////////////////////////////
        //    // Part 1: Preliminary checks and just convert filePath if it's rooted.
        //    ///////////////////////////////////////////////////////////////////////////////
        //    // Safety valve.
        //    if (String.IsNullOrEmpty(filePath))
        //        return null;
        //    filePath = NormalizeFilePathWithDrive(filePath);
        //    // If the filePath is absolute, not much to do......
        //    if (IsPathSpecAbsolute(filePath))
        //        return filePath;
        //    // We need to check out what we are going to do with the two paths.
        //    if (!IsPathSpecRooted(filePath)) {
        //        //// If we are here, we need to check rootedFilePath out because we
        //        //// are going to use it.
        //        // Safety valve - we exit if without a needed rootedFilePath.
        //        if (String.IsNullOrEmpty(rootedFilePath))
        //            return null;
        //        // Make sure is rooted.
        //        if (!IsPathSpecRooted(rootedFilePath)) {
        //            errorString = LTRMN + "rootedFilePath: " + rootedFilePath + " (NOT ROOTED)" + LTRMN;
        //            goto errorout;
        //        }
        //        // Make sure not relative.
        //        if (IsPathSpecRelative(rootedFilePath)) {
        //            errorString = LTRMN + "rootedFilePath: " + rootedFilePath + " (RELATIVE)" + LTRMN;
        //            goto errorout;
        //        }
        //    }
        //    else {
        //        // filePath is rooted, so convert it.
        //        return FilePathCrawler(filePath, throwException);
        //    }

        //    ///////////////////////////////////////////////////////////////////////////////
        //    // Part 2: Setup for walking the directories.
        //    ///////////////////////////////////////////////////////////////////////////////
        //    // If we got this far, put in both the filenames.
        //    errorString = LTRMN + "filePath: " + filePath + "  rootedFilePath: " + rootedFilePath + LTRMN;
        //    //// The first order of business is to save a leading UNC or volume.
        //    string volumeOrUNCString = null;
        //    int endOfPrefix;
        //    // See if we can capture a prefix on the beginning.
        //    volumeOrUNCString = GetVolumeOrUNCSpec(rootedFilePath, out endOfPrefix);
        //    if ((endOfPrefix == -1) && (addVolume)) {
        //        // We had no UNC or volume - we'll use the current volume.
        //        volumeOrUNCString = PlatformUtils.GetFrameworkWorkingVolume();
        //        // If we couldn't get it, we have a problem...
        //        // This should really never happen.
        //        if (volumeOrUNCString == null) {
        //            errorString = LTRMN + "Problem with CWV" + LTRMN;
        //            goto errorout;
        //        }
        //    }
        //    // This is the filePath that we are building. It starts life as a copy of
        //    // the absolute directory.
        //    string workingFilePath;
        //    int endIndex = rootedFilePath.Length - 1;
        //    // Check to see if we have a DirSep on the end.
        //    if (rootedFilePath.IndexOfAny(PlatformUtils.GetDirectorySeparatorChars(), endIndex) >= 0)
        //        workingFilePath = rootedFilePath.Substring(endOfPrefix + 1);
        //    else
        //        // We need a terminating DirSep for our work.
        //        workingFilePath = rootedFilePath.Substring(endOfPrefix + 1)
        //            + PlatformUtils.GetDirectorySeparatorChar();
        //    // We must normalize the working path.
        //    workingFilePath = NormalizeFilePath(workingFilePath, false);
        //    // This is a map of the directory separators in the working file path.
        //    MatchPartition workingFpPartition = new MatchPartition();
        //    // Find out how many levels down we are and record the separator indices.
        //    int currentDirNavigationLevel = StringParsingUtils.MultiplePatternWildCardMatch(
        //        workingFilePath, 0, PlatformUtils.GetDirectorySeparatorStrings(), null, null,
        //        true, -1, false, workingFpPartition).NumMatches - 1;
        //    // Set up a match for detecting "UpDir" strings.
        //    MatchDescriptor upDirMatch;
        //    // Set up a match for detecting "CurrentDir" strings.
        //    MatchDescriptor currentDirMatch;
        //    // Set up a match and a string index for navigating the path.
        //    MatchDescriptor match;
        //    int stringIndex = 0;

        //    ///////////////////////////////////////////////////////////////////////////////
        //    // Part 3: Main loop for walking the directories.
        //    ///////////////////////////////////////////////////////////////////////////////
        //    // Each iteration of the loop either processes a navigation string or adds
        //    // a new directory string (which may be a filename if on the end).
        //    while (true) {
        //        // See if it's an UpDir or CurrentDir - we only grab one at a time at
        //        // our current offset.
        //        upDirMatch = StringParsingUtils.MultiplePatternWildCardMatch(filePath, stringIndex,
        //            PlatformUtils.GetDirectoryUpStrings(), null, null, false, 0, false, null);
        //        currentDirMatch = StringParsingUtils.MultiplePatternWildCardMatch(filePath, stringIndex,
        //            PlatformUtils.GetCurrentDirectoryStrings(), null, null, false, 0, false, null);
        //        // If a navigation character, we peel it off.
        //        if ((upDirMatch.NumMatches > 0) || (currentDirMatch.NumMatches > 0)) {
        //            if (upDirMatch.NumMatches > 0) {
        //                // It's an Up, check that we didn't go off the top.
        //                if (--currentDirNavigationLevel < 0)
        //                    goto errorout;
        //                // Next search position is one past the end of the current UpDir.
        //                stringIndex = upDirMatch.OffsetOfMatchEnd + 1;
        //            }
        //            // We just move past a "CurrentDir" if we have one at the current position.
        //            else if (currentDirMatch.NumMatches > 0) {
        //                // Next search position is one past the end of the current CurrentDir.
        //                stringIndex = currentDirMatch.OffsetOfMatchEnd + 1;
        //            }
        //            // We now check to see if we are at the end of the filePath. If so,
        //            // we must truncate the working FP to the current navigation level and
        //            // return it. It will have a "standard" terminating directory separator
        //            // on it.
        //            if (stringIndex == filePath.Length) {
        //                workingFilePath = workingFilePath.Substring(
        //                    0, workingFpPartition[currentDirNavigationLevel].OffsetOfMatchEnd + 1);
        //                return volumeOrUNCString + workingFilePath;
        //            }
        //            continue;
        //        }
        //        // Find the next piece of punctuation and add the intervening characters
        //        // to the path.
        //        match = StringParsingUtils.MultiplePatternWildCardMatch(filePath, stringIndex,
        //            PlatformUtils.GetDirectoryPunctuationStrings(), null, null, false, 1, false, null);
        //        if (match.NumMatches < 0)
        //            goto errorout;
        //        if (match.NumMatches == 0) {
        //            // In preparation for leaving, we must truncate the working FP to it's
        //            // current level.
        //            workingFilePath = workingFilePath.Substring(
        //                0, workingFpPartition[currentDirNavigationLevel].OffsetOfMatchEnd + 1);
        //            // If there isn't any more punctuation, just tack the whole thing on
        //            // and leave.
        //            workingFilePath = workingFilePath + filePath.Substring(stringIndex);
        //            return volumeOrUNCString + workingFilePath;
        //        }
        //        if (match.NumMatches == 1) {
        //            // The other case we allow here is if there is a terminating DirSep
        //            // on the filePath. This shows up as a punctuation, but now we must check
        //            // if it's a DirSep and is on the very end. This indicates that the client
        //            // wants to definately indicate a directory as opposed to a file. We just
        //            // tack it on after the new directory and send it out.
        //            MatchDescriptor termDirSep = StringParsingUtils.MultiplePatternWildCardMatch(filePath, stringIndex,
        //                PlatformUtils.GetDirectorySeparatorStrings(), null, null, false, 1, false, null);
        //            if ((termDirSep.NumMatches == 1) && (termDirSep.OffsetOfMatchEnd == filePath.Length - 1)) {
        //                // In preparation for leaving, we must truncate the working FP to it's
        //                // current level.
        //                workingFilePath = workingFilePath.Substring(
        //                    0, workingFpPartition[currentDirNavigationLevel].OffsetOfMatchEnd + 1);
        //                workingFilePath = workingFilePath + filePath.Substring(stringIndex);

        //                return volumeOrUNCString + workingFilePath;
        //            }
        //        }
        //        // If we are here, we must be adding something (moving downward) in the
        //        // path. We will first recalculate the working path by digging out the
        //        // indices for the last working path and truncating it to whatever level
        //        // we are at now and then add the new piece.
        //        workingFilePath = workingFilePath.Substring(
        //            0, workingFpPartition[currentDirNavigationLevel].OffsetOfMatchEnd + 1)
        //            + filePath.Substring(stringIndex, match.OffsetOfMatchStart - stringIndex)
        //            + PlatformUtils.GetDirectorySeparatorChar();
        //        // Recompute the partition.
        //        workingFpPartition.ClearMatches();
        //        currentDirNavigationLevel = StringParsingUtils.MultiplePatternWildCardMatch(
        //            workingFilePath, 0, PlatformUtils.GetDirectorySeparatorStrings(), null, null,
        //            true, -1, false, workingFpPartition).NumMatches - 1;
        //        // Set index past the match.
        //        stringIndex = match.OffsetOfMatchStart;
        //        // Swallow the character after the text if it was a dirsep.
        //        if (StringParsingUtils.CheckChars(filePath[stringIndex], PlatformUtils.GetDirectorySeparatorChars()))
        //            stringIndex++;
        //    }
        //errorout:
        //    if (throwException) {
        //        throw new PAFFileNotFoundException(String.Format("Bad filepath. {0}", errorString));
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Transforms an incoming filePath specification, which can be just a path
        ///// or a path with a filename at the end. An absolute filePath is formed
        ///// by using the "current working directory" (See <c>PlatformUtils</c>).
        ///// Please note that this is different from the "installed directory", which
        ///// is usually something like "c:\MyApp\bin". They will be the same if the
        ///// application has not changed the CWD.
        ///// </summary>
        ///// <param name="filePath">
        ///// Valid filename that may contain embedded OS-specific
        ///// relative directory specification strings (e.g.  "..\" or "../") mixed.
        ///// If the <see paramref name="filePath"/> is already an absolute file path, it is
        ///// "normalized" to remove directory navigation characters and use "standard"
        ///// directory separators. If <see paramref name="filePath"/> is already rooted,
        ///// as defined by <c>IsPathSpecRooted</c> the CWD is ignored. This argument
        ///// can be <see langword="null"/> or blank, in which case <see langword="null"/> is returned.
        ///// </param>
        ///// <param name="throwException">
        ///// If set to <see langword="true"/>, an exception will be thrown if an invalid path is
        ///// supplied.
        ///// </param>
        ///// <returns>
        ///// Absolute path if one could be constructed, <see langword="null"/> if not and
        ///// <see paramref name="throwException"/> is <see langword="false"/>.
        ///// </returns>
        ///// <remarks>
        ///// This method has the side effect of transforming any embedded directory
        ///// separator characters to the "standard" directory separator character for
        ///// the current platform. If, for some reason, the client wishes to transform
        ///// a filePath that is already absolute, but with mixed dirsep characters,
        ///// simply pass it through this method.
        ///// </remarks>
        //public static string GetAbsoluteWorkingFilePath(string filePath, bool throwException)
        //{
        //    // The strategy for this method is to get the current working directory and
        //    // then pass it down to <c>GetAbsoluteFilePath()<c> with the filepath.
        //    // <c>GetAbsoluteFilePath()<c> does all the dirtywork.
        //    return GetAbsoluteFilePath(filePath, PlatformUtils.GetFrameworkWorkingDirectory(),
        //        true, throwException);
        //}

        ///// <summary>
        ///// Checks if an incoming filename results in a valid file, then constructs the
        ///// OS-dependent full absolute path if it does. This is a bit more complex
        ///// than a simple string search could provide, since we have to handle multiple
        ///// types of directory separators within the same path specification. We have tried
        ///// to make this method a bit more lightweight than the MS "Path.GetFullPath()".
        ///// </summary>
        ///// <param name="fileName">
        ///// Valid filename which may contain an absolute or relative directory spec. with
        ///// "\" specifications allowed. Can be <see langword="null"/>. This string is checked verbatim
        ///// for existence. If found, it returns the OS-dependent filename including the absolute
        ///// path.
        ///// </param>
        ///// <returns>
        ///// Absolute path if the file exists, <see langword="null"/> if not.
        ///// </returns>
        //public static string GetAbsolutePathIfFileExists(string fileName)
        //{

        //    string fullFileName = GetAbsoluteWorkingFilePath(fileName, false);
        //    // GetAbsoluteFilePath can return a proper rooted filename even though
        //    // the file does not exist. It's OK, folks, that's the way it was designed.
        //    if ((String.IsNullOrEmpty(fullFileName)) || !PlatformUtils.FileExists(fullFileName))
        //        return null;
        //    return fullFileName;
        //}
        ///// <summary>
        ///// Main utility routine to open a <see cref="FileStream"/>. The method
        ///// captures various errors and failures which may happen along the way
        ///// of opening a file and reports them in a consistent format, either as
        ///// an error code or an <see cref="PAFException"/>
        ///// KRM todo - noncanonical filename test.
        ///// </summary>
        ///// <param name="absoluteFileName">
        ///// Filename with a prepended absolute directory specification. This filename
        ///// will be put into "canonical" form internally.
        ///// </param>
        ///// <param name="fileModeRequested">
        ///// This is the standard <see cref="System.IO.FileMode"/> enum.
        ///// </param>
        ///// <param name="fileAccessRequested">
        ///// This is the standard <see cref="System.IO.FileAccess"/> enum.
        ///// </param>
        ///// <param name="announce">
        ///// <see langword="true"/> if any failure should be reported to the user interactively.
        ///// The user is required to dispatch the error window or respond at the console.
        ///// </param>
        ///// <param name="recover">
        ///// <see langword="true"/> if the user is allowed to correct the error interactively by
        ///// specifying a new filename or perhaps inserting a CD, etc.. This method
        ///// allows the user to "Ignore" or "Retry". If <see langword="true"/>,
        ///// <see paramref name="announce"/> is set to <see langword="true"/> automatically.
        ///// </param>
        ///// <param name="throwException">
        ///// <see langword="true"/> if an FileAndIOUtils exception is to be thrown in the case of failure.
        ///// If an internal exception created the error and this exception is available
        ///// to the caller, it will be wrapped inside the thrown exception. If this
        ///// parameter is set to <see langword="true"/>, an exception will also be thrown if any
        ///// errors are not successfully corrected by the user if <see paramref name="recover"/>
        ///// is <see langword="true"/>.
        ///// </param>
        ///// <param name="fileStatusCode">
        ///// <see cref="FileStatusCode.Success"/> if the operation was ultimately
        ///// successful, and a non-zero error code otherwise.
        ///// </param>
        ///// <returns>
        ///// The created <see cref="FileStream"/> if successful.
        ///// </returns>
        //public static FileStream InteractiveGetFileStream(string absoluteFileName, FileMode fileModeRequested, FileAccess fileAccessRequested,
        //    bool announce, bool recover, bool throwException, out Enum fileStatusCode)
        //{
        //    // Set default return values.
        //    FileStream fileStream;
        //    // We are going to be optimistic.
        //    fileStatusCode = FileStatusCode.Success;
        //    // Recover implies announce.
        //    if (recover)
        //        announce = true;

        //    // out parameters needed for CheckFileStatus.
        //    FileAttributes fileAttributes;
        //    FileStatusCode fileStatusCodeOut = FileStatusCode.Success;
        //    SecurityUtilsInternal.FileSecurityProbeStatus fileSecurityProbeStatus;
        //    FileIOPermissionAccess fileIOPermissionNeeded;
        //    FileIOPermissionAccess fileIOPermissionGranted;

        //    // We will assign this dynamically, depending what kind of error we get.
        //    // It is set by default to the generic error, which will be used to announce
        //    // general failures if we don't have anything more specific to tell user.
        //    PAFException dynamicPAFError = null;

        //    // Same sort of thing for the detail string.
        //    // Transform null to blank first, though.
        //    if (absoluteFileName == null)
        //        absoluteFileName = "";
        //    // This string is not dynamic in this method, but could be.
        //    string dynamicDetailString = "File: " + absoluteFileName + LTRMN;
        //    // For reporting inner exceptions that may have bubbled up from the CLR
        //    // and we want to wrap and report to the logfile and/or user.
        //    Exception innerException = null;
        //    bool checksOutOK = true;
        //    bool tryAgain = false;

        //    do {
        //        // Let's first check to see if we can do what we're attempting.
        //        checksOutOK = CheckFileStatusInternal(absoluteFileName, fileAccessRequested, fileModeRequested,
        //        out fileStatusCodeOut, out fileAttributes, out fileSecurityProbeStatus,
        //        out fileIOPermissionNeeded, out fileIOPermissionGranted);
        //        // OK, try to open it.
        //        if (checksOutOK) {
        //            try {
        //                fileStream = InternalGetFileStream(absoluteFileName,
        //                    fileModeRequested, fileAccessRequested);
        //                // This should never be null, but we are always defensive.
        //                if (fileStream == null) {
        //                    fileStatusCodeOut = FileStatusCode.FileOpenError;
        //                }

        //                fileStatusCode = fileStatusCodeOut;
        //                return fileStream;
        //            }
        //            catch (Exception e) {
        //                PAFFileIOException mpex = new PAFFileIOException(e.Message, e);
        //                ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Default, mpex);
        //                // We have already checked everything six ways to Sunday in
        //                // CheckFileStatus and we should've been OK for the operation.
        //                // We'll just tell the user we couldn't open it. Must be locked
        //                // or something........
        //                fileStatusCodeOut = FileStatusCode.FileOpenError;
        //                // Not if we got a security exception, though.
        //                if (e is SecurityException) {
        //                    fileSecurityProbeStatus = SecurityUtilsInternal.FileSecurityProbeStatus.ClientSecurityAccessFailure;
        //                }
        //                // If it's not a security exception or our marker exception, we can
        //                // wrap it for the client.
        //                else {
        //                    innerException = e;
        //                }
        //            }
        //        }

        //        // We don't do a thing with a security error. It will have been logged
        //        // inside CheckFileStatus.
        //        if (fileSecurityProbeStatus != 0) {
        //            fileStatusCodeOut = FileStatusCode.UnknownError;
        //            goto errorexit;
        //        }

        //        //// We must now prepare any error messages to be put in an exception or shown
        //        //// to the user if we had a failure and we are supposed to be doing either of
        //        //// these things.
        //        if (announce || throwException) {
        //            // Filename wasn't valid.
        //            if ((fileStatusCodeOut == FileStatusCode.BadFileName) || (fileStatusCodeOut == FileStatusCode.FileIsDirectory)) {
        //                dynamicPAFError = new PAFFileNotFoundException("Bad file path.");
        //            }
        //            // See if it's missing.
        //            else if (fileStatusCodeOut == FileStatusCode.FileNotFound) {
        //                dynamicPAFError = new PAFFileNotFoundException("File not found.");
        //            }
        //            // See if it's here, but not supposed to be.
        //            else if (fileStatusCodeOut == FileStatusCode.FileAlreadyExists) {
        //                dynamicPAFError = new PAFFileExistsException("File already exists.");
        //            }
        //            // We wanted to write it.
        //            else if (fileStatusCodeOut == FileStatusCode.FileIsReadonly) {
        //                dynamicPAFError = new PAFFileAccessException("File is read only.");
        //            }
        //            // Problem opening even after we've checked it out.
        //            else if (fileStatusCodeOut == FileStatusCode.FileOpenError) {
        //                dynamicPAFError = new PAFFileAccessException("Error opening file.");
        //            }
        //            // Silly errors that are otherwise hard to track down. We can't
        //            // recover from them since they are malformed requests.
        //            else if (fileStatusCodeOut == FileStatusCode.AppendReadInconsistency) {
        //                dynamicPAFError = new PAFFileAccessException("Read/append access inconsistency.");
        //                recover = false;
        //            }
        //            else if (fileStatusCodeOut == FileStatusCode.TruncateReadInconsistency) {
        //                dynamicPAFError = new PAFFileAccessException("Read/truncate access inconsistency.");
        //                recover = false;
        //            }
        //            else if (fileStatusCodeOut == FileStatusCode.CreateReadInconsistency) {
        //                dynamicPAFError = new PAFFileAccessException("Read/create access inconsistency.");
        //                recover = false;
        //            }
        //        }
        //        if (announce) {
        //            tryAgain = FileErrorAndRecover(ref absoluteFileName, dynamicPAFError, recover, true);
        //        }
        //    }
        //    while (tryAgain);

        //    //// We have not been successful with the file operation, so come here
        //// for final error handling.
        //errorexit:
        //    // Translate the code for return.
        //    fileStatusCode = fileStatusCodeOut;
        //    if (dynamicPAFError == null) {
        //        dynamicPAFError = new PAFFileIOException(String.Format("File operation error.\n{0}", dynamicDetailString), innerException);
        //    }
        //    ((IPAFLoggingService)CoreServiceBrokerProvider.Services.GetService(typeof(IPAFLoggingService))).LogException(LoggingLevel.Error, dynamicPAFError);
        //    // Generate the exception if this is what the caller wants.
        //    if (throwException) {
        //        throw dynamicPAFError;
        //    }
        //    // Else just return the null stream.
        //    return null;
        //}
        ///// <summary>
        ///// Removes the last segment of a path if the path is not terminated with
        ///// a directory separator. If it is terminated with a directory separator,
        ///// it is a directory specification and is not touched. There is, unfortunately,
        ///// no way to determine a full file spec from a directory spec if there is
        ///// no terminator.
        ///// </summary>
        ///// <param name="filePathWithPotentialFileName">
        ///// Path will be truncated if and only if it has no terminating directory
        ///// separator and it has at least one directory separator embedded in it.
        ///// A path like "c:MyFile.ext" won't be touched. Neither will "c:\MyDir\".
        ///// "c:\MyDor\MySubdir" will become "c:\MyDir\". Equivalent behavior on unix.
        ///// The path must be rooted. A path like "c:MyFile.ext" will generate an
        ///// exception. A path like "\MyDir\" is OK. This method calls <see cref="GetAbsoluteFilePath"/>.
        ///// </param>
        ///// <returns>
        ///// The string with the trailing segment removed or origininal string if no segment.
        ///// </returns>
        ///// <remarks>
        ///// This method does NOT normalize the file path. We take special care not to do so.
        ///// </remarks>
        //public static String KillFileName(String filePathWithPotentialFileName)
        //{
        //    // First normalize the file path so we can search it.
        //    var normalizedPath = GetAbsoluteFilePath(filePathWithPotentialFileName, null, false, true);
        //    int found;
        //    if ((found = normalizedPath.LastIndexOf(PlatformUtils.GetDirectorySeparatorChar())) < 0) return filePathWithPotentialFileName;
        //    if (found == normalizedPath.Length - 1) return filePathWithPotentialFileName;
        //    var filenameToRemove = normalizedPath.Substring(found + 1, normalizedPath.Length - found - 1);
        //    found = filePathWithPotentialFileName.LastIndexOf(filenameToRemove, System.StringComparison.Ordinal);
        //    var path = filePathWithPotentialFileName.Substring(0, found);
        //    return path;
        //}

        #endregion // Internal SecurityCritical Helpers
        #endregion // Internal Helpers
        ///// <summary>
        ///// This is the standard PAF enum that is used to communicate information about
        ///// the status of attempted file operations or proposed file operations.
        ///// </summary>
        //public enum FileStatusCode
        //{
        //    /// <summary>
        //    /// File operation was successful.
        //    /// </summary>
        //    Success = 0,
        //    /// <summary>
        //    /// This is the generic error that can be safely handed back to an
        //    /// underpriviledged client whose attempts at file access have resulted
        //    /// in a security failure. It is also the error code that should be
        //    /// handed back to priviledged clients when the error is truly unknown.
        //    /// </summary>
        //    UnknownError = 1,
        //    /// <summary>
        //    /// Bad filename.
        //    /// </summary>
        //    BadFileName = 2,
        //    /// <summary>
        //    /// File was not found.
        //    /// </summary>
        //    FileNotFound = 3,
        //    /// <summary>
        //    /// File is readonly. This code is normally returned after it has
        //    /// been determined that a file access operation (e.g. opening with
        //    /// OpenOrCreate status) requires write permission, but the client
        //    /// only requested read.
        //    /// </summary>
        //    FileIsReadonly = 4,
        //    /// <summary>
        //    /// This code is returned when client requested CreateNew and the file
        //    /// already exists.
        //    /// </summary>
        //    FileAlreadyExists = 5,
        //    /// <summary>
        //    /// Error opening the file.
        //    /// </summary>
        //    FileOpenError = 6,
        //    /// <summary>
        //    /// Attempt to open a file for append with readonly access requested.
        //    /// </summary>
        //    AppendReadInconsistency = 7,
        //    /// <summary>
        //    /// Attempt to truncate a file for append with readonly access requested.
        //    /// </summary>
        //    TruncateReadInconsistency = 8,
        //    /// <summary>
        //    /// Attempt to create a file with readonly access requested.
        //    /// </summary>
        //    CreateReadInconsistency = 9,
        //    /// <summary>
        //    /// File spec is a directory.
        //    /// </summary>
        //    FileIsDirectory = 9,
        //    /// <summary>
        //    /// Error saving a file - usually no permission or readonly.
        //    /// </summary>
        //    FileSaveError = 10
        //}
        #endregion // OldStuff
        #endregion // FileAndIOUtils
    }
}