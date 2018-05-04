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

using System;
using System.Security;

namespace PlatformAgileFramework.FileAndIO.SymbolicDirectories
{
	/// <summary>
	/// <para>
	/// This interface accesses a dictionary of symbolic directory mappings. It is useful when
	/// we wish to retrofit old code that was not originally designed with cross-platform
	/// abstractions in place. This particular class caters to old Windows applications
	/// that use drive letters like <c>c:</c>. It provides a central place to change
	/// drive letter mappings to something like: <c>/usr/c_drive</c> or <c>/usr/d_drive</c>,
	///  etc.. This class is used by various FileandIO utilities.
	/// </para>
	/// <para>
	/// The lookup is done by drive letter only (no colon). A rooted directory specification
	/// like that above is returned with no terminating directory separator.
	/// Noted that either an upper case or lower case can be used to specify the drive letter.
	/// </para>
	/// <para>
	/// The dictionary can also be used tp map symbolic directory symbols like "C_Drive"
	/// and "ResourceDirectory: and anything else at all.
	/// </para>
	/// </summary>
	public interface ISymbolicDirectoryMappingDictionary
	{
		#region Methods
        /// <summary>
        /// Adds a mapping to the mapping dictionary.
        /// </summary>
        /// <param name="token">
        /// The token for the drive or other string key.
        /// </param>
        /// <param name="directory">
        /// The directory that this token should be replaced with in
        /// file/directory operations.
        /// </param>
        /// <returns>
        /// <see langword="false"/> if another thread snuck in before
        /// us and already stored it.
        /// </returns>
        [SecurityCritical]
        bool AddMapping(string token, string directory);
        /// <summary>
        /// Gets a mapping from the mapping dictionary.
        /// </summary>
        /// <param name="token">
        /// The token for the drive or other string key.
        /// </param>
        /// <returns>
        /// The mapping if found or <see langword="null"/>.
        /// </returns>
        [SecurityCritical]
        string GetMapping(string token);
        /// <summary>
        /// Populates the static dictionary from xml.
        /// </summary>
        /// <param name="filePath">
        /// File path to the xml file, including filename. This one must
        /// obviously be absolute (non-symbolic).
        /// </param>
        [SecurityCritical]
        void PopulateStaticDictionaryFromXML(string filePath);
		#endregion // Methods
	}
	/// <summary>
	/// Internal version of the interface - not security-critical.
	/// </summary>
	internal interface ISymbolicDirectoryMappingDictionaryInternal
		:ISymbolicDirectoryMappingDictionary
	{
		#region Methods
        /// <summary>
        /// Adds a mapping to the mapping dictionary.
        /// </summary>
        /// <param name="token">
        /// The token for the drive or other string key.
        /// </param>
        /// <param name="directory">
        /// The directory that this token should be replaced with in
        /// file/directory operations.
        /// </param>
        /// <returns>
        /// <see langword="false"/> if another thread snuck in before
        /// us and already stored it.
        /// </returns>
        bool AddMappingInternal(string token, string directory);
		/// <summary>
		/// See <see cref="ISymbolicDirectoryMappingDictionary.GetMapping"/>.
		/// </summary>
		/// <param name="token">
		/// <summary>
		/// See <see cref="ISymbolicDirectoryMappingDictionary.GetMapping"/>.
		/// </summary>
		/// </param>
		/// <returns>
		/// <summary>
		/// See <see cref="ISymbolicDirectoryMappingDictionary.GetMapping"/>.
		/// </summary>
		/// </returns>
		string GetMappingInternal(string token);
        /// <summary>
        /// See <see cref="ISymbolicDirectoryMappingDictionary.PopulateStaticDictionaryFromXML"/>.
        /// </summary>
        /// <param name="filePath">
        /// See <see cref="ISymbolicDirectoryMappingDictionary.PopulateStaticDictionaryFromXML"/>.
        /// </param>
        void PopulateStaticDictionaryFromXMLInternal(string filePath);
		#endregion // Methods
	}

	/// <summary>
	/// Just holds a few platform-independant constants.
	/// </summary>
	public struct SymbolicDirectoryMappingConstants
	{
		#region Names for XML entities.
		/// <summary>
		/// This is the name of the mapping section.
		/// </summary>
		public const string MAPPING_SECTION_ELEMENT_NAME = "symbolicdirectorymappings";
		/// <summary>
		/// This is the name of an individual mapping.
		/// </summary>
		public const string MAPPING_ELEMENT_NAME = "mapping";
		/// <summary>
		/// This is the name of the directory symbol attribute.
		/// </summary>
		public const string DIRECTORY_SYMBOL_ATTRIBUTE_NAME = "symbol";
		/// <summary>
		/// This is the physical platform-specific string the directory symbol maps to.
		/// </summary>
		public const string PHYSICAL_DIRECTORY_ATTRIBUTE_NAME = "mapto";
		#endregion // Names for XML entities.
	}
}
