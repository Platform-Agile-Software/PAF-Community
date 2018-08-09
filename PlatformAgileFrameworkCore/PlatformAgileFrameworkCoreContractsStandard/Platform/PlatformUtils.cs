//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2016 Icucom Corporation
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.StringParsing;

namespace PlatformAgileFramework.Platform
{
	/// <summary>
	/// <para>
	/// A utility class for performing platform-specific operations. This class
	/// contains some functions that are obviously platform-specific. It also contains
	/// some functions that ought not to be, but have turned out not to be implemented
	/// uniformly, so we abstract them.
	/// </para>
	/// <para>
	/// This class is designed primarily as a helper class for the framework. Many
	/// of the methods here require full trust from the caller. Many methods are
	/// marked as "internal" and are designed primarity to be called by other
	/// methods that do security checks and assertions before calling into them.
	/// .Net security will generally do it's job of protecting secure resources, but
	/// does not always provide consistent and understandable error messages and
	/// exceptions. So try to identify problems before calling methods from this
	/// utility class.
	/// </para>
	/// <para>
	/// Methods in this class attempt to be CLS-compliant. We deal with <see cref="System.Enum"/>s
	/// wherever enumerations are exposed.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16feb2016 </date>
	/// <description>
	/// Rebuilt this thing as a lazy singleton, as it probably should have been all along.
	/// Lazy singleton allows statics to be set by platforms before instantiation.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 09aug2015 </date>
	/// <description>
	/// Made huge changes to allow flexibility for adding platforms. We now define the
	/// platform within an external "PlatformInfo" file. We can have as many of these
	/// as we ever want.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 09jan2012 </date>
	/// <description>
	/// <para>
	/// Added history - original author unknown, probably Golea project....
	/// </para>
	/// <para>
	/// Started reworking this for the extensibility model. First mods were to
	/// the "current platform" stuff to escape the restriction of the
	/// "CLRPlatformsSupported" Enum.
	/// </para>
	/// <para>
	/// TODO - make all of the private readonly arrays internal collections or lists
	/// TODO so extenders can load them. Also need to make both internal and public
	/// TODO //[SecurityCritical] versions of most methods/props.
	/// </para>
	/// </description>
	/// </contribution>
	/// </history>
	// SL
	// ReSharper disable PartialTypeWithSinglePart
	public partial class PlatformUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and AutoProps
		/// <summary>
		/// This a thread-safe wrapper for constructing the singleton.
		/// </summary>
		/// <remarks>
		/// Lazy class is thread-safe by default.
		/// </remarks>
		private static readonly Lazy<PlatformUtils> s_Singleton =
			new Lazy<PlatformUtils>(ConstructPlatformUtils);

		/// <summary>
		/// Public version has security critical setter.
		/// </summary>
		public static string ApplicationRoot
		{
			get { return ApplicationRootInternal;}
			////[SecurityCritical]
			set { ApplicationRootInternal = value; }
		}
		/// <summary>
		/// non - <see langword="null"/> only when needs to be forced to other than the CWD.
		/// </summary>
		internal static string ApplicationRootInternal { get; set; }
		/// <summary>
		/// Platform-specific information. Public version has security critical setter.
		/// </summary>
		public static IPlatformInfo PlatformInfo
		{ get { return PlatformInfoInternal;}
			////[SecurityCritical]
			set { PlatformInfoInternal = value; } }
		/// <summary>
		/// Platform-specific information. Internal version is wide-open.
		/// </summary>
		internal static IPlatformInfo PlatformInfoInternal { get; set; }

		//// These four are platform-specific, but not part of .Net.
		//// Noted (krm), these have all been changed to currentplatform, since these
		//// are basic sanity rules, anyway.
		// We actually want files to be transported across platforms, so we want
		// to limit length for all.
		private const int MAX_FILENAME_LENGTH = 260;
		private const int MAX_DIRECTORYNAME_LENGTH = 248;

		// Needed to validate a filename.
		private const string FILENAME_REGEX = @"^(([a-zA-Z]:)|.)[^:]*$";

		// Prohibited base filenames.
		private static readonly string[] s_ProhibitedBaseFileNames
			= {"CON", "PRN", "AUX", "NUL",
				"COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
				"LPT0", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"};

		// All the filename and directory characters we don't like. These are loaded dynamically.
		private static readonly char[] s_ProhibitedFileNameChars;
		private static readonly char[] s_ProhibitedPathChars;

		// Eliminate wildcards, .bak, etc.
		private static readonly string[] s_ProhibitedFileNameExtensions;

		// These are the ones we never like, no matter what OS.
		private static readonly char[] s_GenerallyProhibitedFileNameChars
			= { '\"', '<', '>', '|', '\0', (char)1, (char)2, (char)3, (char)4, (char)5, (char)6,
				  (char)7, (char)8, (char)9, (char)10, (char)11, (char)12, (char)13, (char)14,
				  (char)15, (char)16, (char)17, (char)18, (char)19, (char)20, (char)21, (char)22,
				  (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
				  (char)31, ':', '*', '?', '\\', '/' };
		private static readonly char[] s_GenerallyProhibitedPathChars = { };

		// We don't want .bak files accessed without special permission.
		private static readonly string[] s_GenerallyProhibitedFilenameExtensions = { "*", "BAK" };

		// Volume separator character. This is loaded dynamically.
		private static readonly char s_VolumeSeparatorChar;

		// End of line terminator. This is loaded dynamically.
		// ReSharper disable once InconsistentNaming
		private static readonly string s_LTRMN = Environment.NewLine;

		// Indicates a "missing character" in methods that expect
		// char args.
		private static readonly char s_NoChar;

		// "Standard" directory separator character. This is loaded dynamically.
		private static char s_DirectorySeparatorChar;

		// "Alternative" directory separator character. This is loaded dynamically.
		// Update for .Net 2.0.
		private static char s_AlternativeDirectorySeparatorChar;

		// A set of characters that can be used as path separators, not including
		// a volume separator. These are loaded dynamically.
		private static char[] s_DirectorySeparatorChars;

		// String versions of the above, so we have them for input to method
		// calls that require strings and we don't have to burden the GC by
		// creating them on the fly.
		private static string[] s_DirectorySeparatorStrings;

		// Path separator characters including volume separator. These are loaded
		// dynamically.
		private static char[] s_FilePathSeparatorChars;

		// Directory "up" indicators used in relative directory spcifications.
		// (e.g. "..\" and "../". Loaded dynamically.
		private static string[] s_DirectoryUpStrings;

		// "Current" directory indicators used in relative directory spcifications.
		// (e.g. ".\" and "./". Loaded dynamically.
		private static string[] s_CurrentDirectoryStrings;
		///////////////////////////////////////////////////////////////////////
		// Endian stuff.
		///////
		// for chars.
		internal static bool s_BigEndianChar;
		///////////////////////////////////////////////////////////////////////
		//
		///////////////////////////////////////////////////////////////////////
		//// This is the UNC/URN/URL/URI section. We are trying to simultaneously follow
		//// the guidance in RFC 3986 and also support our own SDs. We probably still
		//// don't have it right......
		// UNC indicator string (e.g. "\\"). Noted: Unix/Linux systems sometimes use "//".
		// .Net does not support discovering these, so we have to a little bit of
		// extra work and put these in PlatformUtils. UNC's are often used to describe
		// "shared" drives on an intranet, for example: \\SomeSharedDrive\SomeTextFile.txt.
		private const string UNC_PREFIX = "\\\\";
		// Alternative UNC prefix is null for MS.
		internal static string s_AlternativeUNCPrefix;
		// The standard internet locator prefixes. These indicate a locator, not just a URN.
		// These are the ones we currently care about - client can add more.....
		internal static string[] s_StandardInternetURLSchemeStrings
			= { "http:", "https:", "ftp:", "tcp:", "udp:", "nntp:", "gopher:", "snmp:",
				"telnet:", "nfs:", "pop:"};
		// "Custom" URL prefixes. Supplied by client.
		internal static string[] s_CustomInternetURLSchemeStrings;

		// The total set of internet and/or remote URL prefixes. Loaded dynamically.
		private static readonly string[] s_RemoteURLSchemeStrings;

		// The standard "local" locator prefixes. These indicate a locator, but
		// one that is known not to resolve to a remote location.
		private static readonly string[] s_LocalURLSchemeStrings
			= { "file:" };

		// All locator prefixes, both internet and non-internet. Built dynamically.
		private static readonly string[] s_URLSchemeStrings;
		// This string, found anywhere, is used to detect a URN that is resolved with
		// a locator. Use this to capture things like file://SomeFilePath as well as
		// the internet URL's.
		private const string URL_INDICATOR_STRING = "://";

		// UNC indicator string array. This is constructed dynamically in the static
		// class constructor and will depend on what is loaded as an alternative
		// prexfix (including possibly a null value). The platform dependency is
		// marked as "current", but obviously that might not hold if a system supports
		// three UNC strings, for example.
		private static readonly string[] s_UNCPrefixes;

		// The total set of URI prefixes. These are URIs but not URLs. Supplied by
		// client. These are needed instead of just a check for a ":" past the
		// volume descriptor since we must be backward compatible with "symbolic"
		// directories.
		private static readonly string[] s_URISchemeStrings = { "mailto:", "urn:", "data:" // Client load more data here.
		};

		// The set of known URNs that we deal with and want to identify.
		// Golea clients: For handling SD's, scan these first to see if the string
		// in question is a known URN. If not, assume it is a SD. These prefixes
		// should never be used as SDs. It is useful to check a SD that a user
		// wants to define against this set. Built dynamically.
		private static readonly string[] s_KnownURNStrings;
		// Holds installed version strings. protected internal for extensions.
// Awaiting rewrite for Non-Windows.
// ReSharper disable UnassignedReadonlyField
		/// <remarks/>
		protected internal static readonly IList<string> s_InstalledVersions;

		// ReSharper restore UnassignedReadonlyField
		///////////////////////////////////////////////////////////////////////
		// (e.g. ".\" and "./" and "../" and "..\"). Loaded dynamically.
		private static string[] s_DirectoryNavigationStrings;

		// Total set of directory punctuation strings, including up, current
		// and separators. (e.g. ".\" and "./" and "../" and "..\" and "/" and "\".
		// Loaded dynamically.
		private static string[] s_DirectoryPunctuationStrings;
		///////////////////////////////////////////////////////////////////////
		// (e.g. ".exe" and ".app"). Loaded dynamically or initialized by somebody.
		internal static string s_ExecutableFileExtensionStringWithDot;
		// (e.g. ".dll" and ".dylib"). Loaded dynamically or initialized by somebody.
		internal static string s_DynamicLoadFileExtensionStringWithDot;
		// The default mapping for the "c:" drive letter. Loaded dynamically or initialized by somebody.
		// ReSharper disable once InconsistentNaming
		internal static string s_C_DriveMapping;
		// The default mapping for the "d:" drive letter. Loaded dynamically or initialized by somebody.
		// ReSharper disable once InconsistentNaming
		internal static string s_D_DriveMapping;
		#endregion // Class Fields and AutoProps

		#region Constructors
		/// <summary>
		/// Static constructor for all our platform-specific data setup.
		/// </summary>
		static PlatformUtils()
		{
			// We need a couple of working collections.
			var charList = new List<char>();
			var stringList = new List<string>();

			// Using <see langword="null"/> as the missing char.
			s_NoChar = (char) 0;
			// Build up a list of all prohibited file chars.
			// Note: Put filename chars directly in upstairs - they are the same for
			// Mac/Unix/Windows/Silverlight. Add any platform-specific ones
			// here with a partial method.
			// Don't want redundancies....
			charList.AddRangeNoDupes(s_GenerallyProhibitedFileNameChars);
			s_ProhibitedFileNameChars = charList.ToArray();

			// TODO KRMs_InstalledVersions = CreateInstalledVersions();

			// Build up a list of all prohibited dir chars.
			charList.Clear();
//			charList.AddRange(Path.GetInvalidPathChars());
			// Don't want redundancies....
			charList.AddRangeNoDupes(s_GenerallyProhibitedPathChars);
			s_ProhibitedPathChars = charList.ToArray();

			// Build up a list of all prohibited extensions.
			// Don't want redundancies....
			stringList.AddRangeNoDupes(s_GenerallyProhibitedFilenameExtensions);
			s_ProhibitedFileNameExtensions = stringList.ToArray();
			// Note that everything is done in terms of global symbolic directories
			// now.
			s_VolumeSeparatorChar = FileUtils.DIR_SYM_SEP;
			stringList.Clear();
			stringList.AddNoDupes(UNC_PREFIX);
			// Need to check if no alternative string, like on MS.
			if(!string.IsNullOrEmpty(s_AlternativeUNCPrefix))
				stringList.AddNoDupes(s_AlternativeUNCPrefix);
			s_UNCPrefixes = stringList.ToArray();
			//// URL strings.
			stringList.Clear();
			stringList.AddRange(s_StandardInternetURLSchemeStrings);
			// No redundancies, please.
			stringList.AddRangeNoDupes(s_CustomInternetURLSchemeStrings);
			s_RemoteURLSchemeStrings = stringList.ToArray();
			// Add the non-internet URLs, with no redundancies, please.
			stringList.AddRangeNoDupes(s_LocalURLSchemeStrings);
			s_URLSchemeStrings = stringList.ToArray();
			//// URN strings.
			// Just add in the URNs and spit out again.
			stringList.AddRangeNoDupes(s_URISchemeStrings);
			s_KnownURNStrings = stringList.ToArray();
		}
		/// <summary>
		/// Not quite a constructor - a factory for the lazy construction.
		/// </summary>
		private static PlatformUtils ConstructPlatformUtils()
		{
			return new PlatformUtils();
		}

		/// <summary>
		/// Constructor builds with platform-specific info. This constructor must be called
		/// once at app initialization time. Subsequent calls generate an exception.
		/// </summary>
		private PlatformUtils()
		{
			s_DynamicLoadFileExtensionStringWithDot = PlatformInfo.DllExtension;
			s_ExecutableFileExtensionStringWithDot = PlatformInfo.ExecutableExtension;

			s_C_DriveMapping = PlatformInfo.CDriveMapping;
			s_D_DriveMapping = PlatformInfo.DDriveMapping;

			s_DirectorySeparatorChar = PlatformInfo.MainDirSepChar;
			s_AlternativeDirectorySeparatorChar = PlatformInfo.AltDirSepChar;
			// We don't want two copies if they are the same, like on Unix.
			var charList = new Collection<char>();
			charList.AddRangeNoDupes(
				new[] { s_DirectorySeparatorChar, s_AlternativeDirectorySeparatorChar });
			s_DirectorySeparatorChars = charList.ToArray();
			var stringList = new Collection<string>();
			//// Now we build the string list version.
			stringList.Clear();
			foreach (var sepChar in charList)
				stringList.Add(new string(sepChar, 1));
			s_DirectorySeparatorStrings = stringList.ToArray();
			//// Now build up a list of "up" directory specification strings. There
			// will be as many as separator chars.
			stringList.Clear();
			foreach (var dirChar in s_DirectorySeparatorChars)
				stringList.Add(".." + dirChar);
			s_DirectoryUpStrings = stringList.ToArray();
			//// Same deal for "Current directory" strings.
			stringList.Clear();
			foreach (var dirChar in s_DirectorySeparatorChars)
				stringList.Add("." + dirChar);
			s_CurrentDirectoryStrings = stringList.ToArray();
			//// Add the volume separator to the list and spit it out again.
			charList.Add(s_VolumeSeparatorChar);
			s_FilePathSeparatorChars = charList.ToArray();
			//// Need the navigation strings - updir and currentdir.
			stringList.Clear();
			foreach (var navString in s_DirectoryUpStrings)
				stringList.AddNoDupes(navString);
			foreach (var navString in s_CurrentDirectoryStrings)
				stringList.AddNoDupes(navString);
			s_DirectoryNavigationStrings = stringList.ToArray();
			//// Make punctuation by just adding seps and spitting out again.
			foreach (var navString in s_DirectorySeparatorStrings)
				stringList.AddNoDupes(navString);
			s_DirectoryPunctuationStrings = stringList.ToArray();
			//// UNC strings.
		}
		#endregion
		#region Properties
		/// <summary>
		/// Get the singleton instance of the class.
		/// </summary>
		/// <returns>The singleton.</returns>
		public static PlatformUtils Instance => s_Singleton.Value;

		/// <summary>
		/// Gets the matching pattern that validates a filename with an optional
		/// directory specification on the front.
		/// </summary>
		internal static string FileNameRegexPattern => FILENAME_REGEX;

		/// <summary>
		/// Gets the "newline" string for the current platform. (e.g. "\r\n"
		/// or just "\n").
		/// </summary>
		/// <remarks>
		/// Attributed as platform-independent, but only if the data is
		/// changed!!
		/// </remarks>
		// ReSharper disable once InconsistentNaming
		public static string LTRMN
		{ get { return s_LTRMN; } }

		/// <summary>
		/// Gets the character that indicates a <see langword="null"/> character that normally
		/// is passed to a method accepting char arguments to indicate that this
		/// character is not to be used.
		/// </summary>
		public static char NoChar
		{ get { return s_NoChar; } }

		#endregion
		#region Methods
		#region Partial Methods
		/// <summary>
		/// Adds supported versions of the EMCA (non-Silverlight) CLI. Implemented in
		/// ECMA.
		/// </summary>
		/// <param name="versions">
		/// Collection of strings to add to. Should have a format of:
		/// "vn.n.n.n".. Example is "v4.0.0.0" for version 4 of the ECMA CLR.
		/// </param>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void AddInstalledVerions(ref ICollection<string> versions );
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		#endregion // Partial Methods

		/// <summary>
		/// Method simply checks a character against both platform
		/// and alt directory sep character.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if a match..
		/// </returns>

		public static bool IsADirSepChar(char characterToCheck)
		{
			if (characterToCheck == GetDirectorySeparatorChar())
				return true;
			if (characterToCheck == GetAlternativeDirectorySeparatorChar())
				return true;
			return false;

		}
		/// <summary>
		/// Returns the "alternative" directory separator character.
		/// </summary>
		/// <returns>
		/// Character such as <c>\</c>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Noted that this character may be the same as the "standard" directory
		/// separator. This can cause problems sometimes and so we provide the
		/// <c>DirectorySeparatorChars</c> and <c>FilePathSeparatorChars</c> methods
		/// that do not duplicate the directory separator characters if they are
		/// the same.
		/// </para>
		/// </remarks>
		internal static char GetAlternativeDirectorySeparatorChar()
		{ return s_AlternativeDirectorySeparatorChar; }

		/// <summary>
		/// Returns the string associated with a "alternative" Universal Naming
		/// Convention prefix. This might be "//", like on some Unix/Linux systems.
		/// It is <see langword="null"/> on MS systems.
		/// </summary>
		/// <returns>
		/// String such as "//" or <see langword="null"/> if a platform does not support an
		/// alternative UNC string.
		/// </returns>
		
		internal static string GetAlternativeUNCPrefix()
		{ return s_AlternativeUNCPrefix; }

		/// <summary>
		/// This reads from the <see cref="PlatformInfo"/> class, wich is different for
		/// each platform supported.
		/// </summary>		
		public static string GetCurrentPlatformName()
		{
			return PlatformInfo.CurrentPlatformName;
		}
		/// <summary>
		/// This is a method that gets the set of strings that can indicate
		/// "current directory" in a relative path spec.
		/// </summary>
		/// <returns>
		/// Examples: ".\" or "./".
		/// </returns>
		
		internal static string[] GetCurrentDirectoryStrings()
		{ return s_CurrentDirectoryStrings; }

		/// <summary>
		/// This is a method that gets the set of strings that can indicate
		/// "current directory" or "up directory" in a relative path spec.
		/// </summary>
		/// <returns>
		/// Examples: ".\" or "./" or "../" or "..\".
		/// </returns>
		
		internal static string[] GetDirectoryNavigationStrings()
		{ return s_DirectoryNavigationStrings; }

		/// <summary>
		/// This is a method that gets the set of strings that can punctuate
		/// a path spec in any way, not including volume separator or UNC.
		/// </summary>
		/// <returns>
		/// Examples: ".\" or "./" or "../" or "..\" or "\" or "/".
		/// </returns>
		
		internal static string[] GetDirectoryPunctuationStrings()
		{ return s_DirectoryPunctuationStrings; }

		/// <summary>
		/// Returns the "standard" directory separator character.
		/// </summary>
		/// <returns>
		/// Character such as <c>/</c>.
		/// </returns>		
		public static char GetDirectorySeparatorChar()
		{ return s_DirectorySeparatorChar; }

		/// <summary>
		/// Returns a set of characters that can be used as path separators, not
		/// including a volume separator. This array does not contain two copies of the
		/// directory separator if the <see cref="s_DirectorySeparatorChar"/>
		/// is the same as the <see cref="s_AlternativeDirectorySeparatorChar"/>
		/// on a particular OS.
		/// </summary>
		/// <returns>
		/// Characters such as <c>\</c> and <c>/</c>.
		/// </returns>
		// TODO - KRM What happened to handling MS file names/paths on non-MS systems? 		
		internal static char[] GetDirectorySeparatorChars()
		{ return s_DirectorySeparatorChars; }

		/// <summary>
		/// Returns a set of characters that can be used as path separators, not
		/// including a volume separator. These are single-character string versions
		/// of GetDirectorySeparatorChars().
		/// </summary>
		/// <returns>
		/// Character such as <c>\</c> and <c>/</c> in string format.
		/// </returns>
		
		internal static string[] GetDirectorySeparatorStrings()
		{ return s_DirectorySeparatorStrings; }

		/// <summary>
		/// Returns the strings that can indicate "up" in a relative directory
		/// specification. (e.g. "..\").
		/// </summary>
		/// <returns>
		/// List of possible up directory strings.
		/// </returns>
		
		internal static string[] GetDirectoryUpStrings()
		{ return s_DirectoryUpStrings; }

		/// <summary>
		/// Gets the platform-specific extension (e.g. ".dll", ".so", ".dylib")
		/// of a dynamically loaded library.
		/// </summary>
		/// <returns>
		/// The platform-specific extension (e.g. ".exe"). The returned string
		/// always has the dot prepended. If executables have no extension, this
		/// method will return a <see cref="String.Empty"/>.
		/// </returns>
		
		public static string GetDynamicLoadFileExtensionStringWithDot()
		{ return s_DynamicLoadFileExtensionStringWithDot; }

		/// <summary>
		/// Gets the platform-specific extension (e.g. ".exe") of an executable.
		/// </summary>
		/// <returns>
		/// The platform-specific extension (e.g. ".exe"). The returned string
		/// always has the dot prepended. If executables have no extension, this
		/// method will return a <see cref="String.Empty"/>.
		/// </returns>
		public static string GetExecutableFileExtensionStringWithDot()
		{ return s_ExecutableFileExtensionStringWithDot; }

		/// <summary>
		/// Returns a set of characters that can be used as path separators, including
		/// a volume separator. This array does not contain two copies of the
		/// directory separator if the <see cref="s_AlternativeDirectorySeparatorChar"/>
		/// is the same as the <see cref="s_DirectorySeparatorChar"/> on a particular
		/// OS.
		/// </summary>
		/// <returns>
		/// Character such as <c>\</c> and <c>/</c> and <c>:</c>.
		/// </returns>		
		internal static char[] GetFilePathSeparatorChars()
		{ return s_FilePathSeparatorChars; }
		/// <summary>
		/// This method returns a list of all "known" URNs. This catalog of strings
		/// is useful for use in manipulating "symbolic directories" that have the
		/// same format as URNs. A SD name should never appear on this list. The list
		/// can be checked to determine if a filePath prefix is some known URN or
		/// a user-defined SD.
		/// </summary>
		/// <returns>
		/// List such as {"mailto:", "http:", "file:"}.
		/// </returns>
		public static string[] GetKnownURNStrings()
		{ return s_KnownURNStrings; }

		/// <summary>
		/// Returns the strings that are associated with URL prefixes that are
		/// resolved locally. (e.g. "file:").
		/// </summary>
		/// <returns>
		/// Cataloged list of URL prefixes associated with resolution of the URI
		/// on a local machine.
		/// </returns>
		/// <remarks>
		/// Attributed as platform-independent, but only if the data is changed!!
		/// </remarks>
		
		internal static string[] GetLocalURLSchemeStrings()
		{ return s_LocalURLSchemeStrings; }

		/// <summary>
		/// This is a method that gets the maximum directory specification length
		/// on the platform.
		/// </summary>
		/// <returns>
		/// A <see cref="Int32"/> describing the maximum length.
		/// </returns>
		
		internal static int GetMaxDirectoryLength()
		{
			return MAX_DIRECTORYNAME_LENGTH;
		}

		/// <summary>
		/// This is a method that gets the maximum file name length on the platform.
		/// The filename does not inculde the directory specification.
		/// </summary>
		/// <returns>
		/// A <see cref="Int32"/> describing the maximum length.
		/// </returns>
		
		internal static int GetMaxFilenameLength()
		{
			return MAX_FILENAME_LENGTH;
		}

		/// <summary>
		/// Returns the filenames that are prohibited on an OS. We don't want to
		/// access the console or COM ports directly.
		/// </summary>
		/// <returns>
		/// List of prohibited names.
		/// </returns>
		
		internal static string[] GetProhibitedBaseFileNames()
		{ return s_ProhibitedBaseFileNames; }

		/// <summary>
		/// Returns the characters that can never be present in a filename. This
		/// refers to the filename without an attached directory. Certain characters
		/// found in this returned array are legitimate in directory specifications,
		/// so the filename has to be separated from the directory portion if the
		/// filename to be checked contains a leading directory spec.
		/// </summary>
		/// <returns>
		/// List of prohibited characters.
		/// </returns>
		
		internal static char[] GetProhibitedFileNameChars()
		{ return s_ProhibitedFileNameChars; }

		/// <summary>
		/// Returns the filename extensions that are prohibited on an OS. We don't want to
		/// allow wildcards or other things that might cause a problem.
		/// </summary>
		/// <returns>
		/// List of prohibited extensions.
		/// </returns>
		
		internal static string[] GetProhibitedFileNameExtensions()
		{ return s_ProhibitedFileNameExtensions; }

		/// <summary>
		/// Returns the characters that can never be present in a path. This
		/// refers to the directory with an optional attached filename.
		/// </summary>
		/// <returns>
		/// List of prohibited characters.
		/// </returns>
		/// <remarks>
		/// Attributed as platform-independent, but only if the data is
		/// changed!!
		/// </remarks>
		
		public static char[] GetProhibitedPathChars()
		{ return s_ProhibitedPathChars; }


		/// <summary>
		/// Returns the array of strings associated with a Universal Naming Convention
		/// prefix. It may be just one, as in MS.
		/// </summary>
		/// <returns>
		/// Strings such as "\\" or "//".
		/// </returns>
		
		internal static string[] GetUNCPrefixes()
		{ return s_UNCPrefixes; }

		/// <summary>
		/// Returns the strings that are associated with URI prefixes that
		/// are not URLs. (e.g. "mailto:"). 
		/// </summary>
		/// <returns>
		/// Cataloged list of URI prefixes.
		/// </returns>
		
		internal static string[] GetURISchemeStrings()
		{ return s_URISchemeStrings; }

		/// <summary>
		/// Returns the strings that are associated with URL prefixes that can
		/// be resolved either locally or remotely.
		/// (e.g. "http:", "file:"). 
		/// </summary>
		/// <returns>
		/// Cataloged list of URL prefixes.
		/// </returns>
		/// <remarks>
		/// Attributed as platform-independent, but only if the data is changed!!
		/// </remarks>
		
		internal static string[] GetRemoteURLSchemeStrings()
		{ return s_RemoteURLSchemeStrings; }

		/// <summary>
		/// Returns the string that is normally associated with a URL. (e.g. "://").
		/// </summary>
		/// <returns>
		/// URL indicator string.
		/// </returns>
		
		internal static string GetURLIndicatorString()
		{ return URL_INDICATOR_STRING; }

		/// <summary>
		/// Returns the strings that are associated with URL prefixes that can
		/// be resolved either locally or remotely.
		/// (e.g. "http:", "file:"). 
		/// </summary>
		/// <returns>
		/// Cataloged list of URL prefixes.
		/// </returns>
		
		internal static string[] GetURLSchemeStrings()
		{ return s_URLSchemeStrings; }

		/// <summary>
		/// Returns the volume separator character.
		/// </summary>
		/// <returns>
		/// Character such as <c>:</c>.
		/// </returns>
		
		internal static char GetVolumeSeparatorChar()
		{ return s_VolumeSeparatorChar; }

		/// <summary>
		/// Returns the string associated with a Universal Naming Convention prefix.
		/// </summary>
		/// <returns>
		/// String such as "\\".
		/// </returns>		
		internal static string GetUNCPrefix()
		{ return UNC_PREFIX; }

		/// <summary>
		/// This method verifies a filePath is valid. Checks ceratin prohibited
		/// characters and whether filePath is just a drive or too long, etc.
		/// </summary>
		/// <param name="fileNameWithOptionalPath">
		/// Filename to check. It can be just a filename or can have a leading
		/// directory specification.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the filename is OK.
		/// </returns>
		/// <remarks>
		/// Attributed as platform-independent, but only if the data is
		/// changed!! Noted: (Destry) changed to internal awaiting retest.
		/// </remarks>

		internal static bool ValidFileName(string fileNameWithOptionalPath)
		{
			if (string.IsNullOrEmpty(fileNameWithOptionalPath))
				return false;
			// This is sort of a wierd loop. The problem is that we have to check
			// the end of the filePath for all possible DirSeps and report the position
			// of the last one. We do this because we cannot ensure that a call like
			// Path.GetFileNameWithoutExtension() will not throw an exception if the
			// filename or directory is too long. (We tested it and it happened!!)
			var lastDirSepPosition = -1;
			foreach (var dSS in GetDirectorySeparatorStrings())
			{
				var match = StringParsingUtils.Match(fileNameWithOptionalPath, 0, dSS,
																 -1, false, null);
				// Replace if we've got one further out.
				int currentLastDirSepPosition;
				if ((match.NumMatches > 0)
					&& ((currentLastDirSepPosition = match.OffsetOfMatchEnd) > lastDirSepPosition))
					lastDirSepPosition = currentLastDirSepPosition;
			}
			// Now we can check if the directory/filename is too long.
			if (lastDirSepPosition > GetMaxDirectoryLength() - 1) return false;
			if ((fileNameWithOptionalPath.Length - lastDirSepPosition) > GetMaxFilenameLength() - 1) return false;

			// Scan for invalid path chars. They can't be anywhere.
			foreach (var invalidChar in GetProhibitedPathChars())
			{
				if (fileNameWithOptionalPath.IndexOf(invalidChar) >= 0)
				{
					return false;
				}
			}
			// Scan for invalid filename chars.
			var fileName = Path.GetFileName(fileNameWithOptionalPath);
			if (string.IsNullOrEmpty(fileName)) return false;
			foreach (var invalidChar in GetProhibitedFileNameChars())
			{
				if (fileName.IndexOf(invalidChar) >= 0) return false;
			}
			// Check the filename against OS pattern.
			if (!Regex.IsMatch(fileNameWithOptionalPath, FileNameRegexPattern)) return false;
			// Now check for problems with the base filename.
			var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithOptionalPath);
			// Can't have just an extension.
			if (nameWithoutExtension == null) return false;
			// We need case-insensitive comparisons for the bad filenames.
			nameWithoutExtension = nameWithoutExtension.ToUpper();
			// Reject any bad names.
			return GetProhibitedBaseFileNames().All(badFileName => nameWithoutExtension != badFileName);
			// Finally, if we got through all that, filename is OK!!!
		}
		/// <summary>
		/// This method creates something like:
		/// ../../../ PlatformAssemblies / PAF.ECMA / bin / Debug /
		/// for debug assembly. This string is needed very early in the "bootstrapping"
		/// process for loading the platform-specific data. If the current platform
        /// assembly name is <see langword="null"/>, a blank string is returned, which
        /// indicates that the platform assembly is statically linked. 
		/// </summary>
		public static string FormPlatformAssemblyLoadFromPath()
		{
			var path =
				".." + PlatformInfo.MainDirSepChar + ".." + PlatformInfo.MainDirSepChar +
				".." + PlatformInfo.MainDirSepChar + ".." + PlatformInfo.MainDirSepChar +
                "PlatformAssemblies" + PlatformInfo.MainDirSepChar +
				PlatformInfo.CurrentPlatformAssyName + PlatformInfo.MainDirSepChar + "bin" +
				PlatformInfo.MainDirSepChar;

#if DEBUG
			path += "Debug";
#else
			path += "Release";
#endif
			path += PlatformInfo.MainDirSepChar; 
			return path;
		}
		/// <summary>
		/// This method creates something like:
		/// ../../../ PlatformAssemblies / PAF.ECMA / bin / Debug / PAF.ECMA.dll
		/// for debug assembly. This string is needed very early in the "bootstrapping"
		/// process for loading the platform-specific assembly so we can pull out the platform-specific
		/// service implementations.
		/// </summary>
		public static string FormPlatformAssemblyLoadPathWithAssembly()
		{
			var path = FormPlatformAssemblyLoadFromPath()
			        + PlatformInfo.CurrentPlatformAssyName;
			return path;
		}
	}
	#endregion
}
