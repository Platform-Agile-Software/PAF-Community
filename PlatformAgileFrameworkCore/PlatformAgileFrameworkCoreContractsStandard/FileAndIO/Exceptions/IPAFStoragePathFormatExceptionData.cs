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
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.FileAndIO.Exceptions
{
	/// <summary>
	///	Exceptions that occur during storage access related to improperly
	/// formed storage path names or fragments.
	/// </summary>
	[PAFSerializable]
	public interface IPAFStoragePathFormatExceptionData: IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic storage path or fragment. In the case of a bad symbolic
		/// directory, this string will contain that directory symbol with
		/// its colon (:).
		/// </summary>
		string ProblematicStoragePath { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFStoragePathFormatExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFStoragePathFormatExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. Issued when a symbolic directory mapping
        /// symbol (e.g. C_DRIVE:, MyData:, etc.) is not found in the
        /// dictionary.
        /// </summary>
        public const string DIRECTORY_SYMBOL_NOT_FOUND = "Directory symbol not found";
        /// <summary>
        /// Error message. Issued when an operation can take a filename only
        /// or a symbolic directory followed by a filename. This means specifically
        /// that no directory specification characters ("/" or "\") are allowed
        /// </summary>
        public const string DIRECTORY_SPECIFICATION_NOT_ALLOWED = "Directory specification not allowed";
        /// <summary>
        /// Error message. Issued when a file path cannot be resolved to a correct normalized
        /// file path. Usually accompanied by an inner exception.
        /// </summary>
        public const string BAD_FILE_PATH = "Bad file path";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFStoragePathFormatExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                DIRECTORY_SYMBOL_NOT_FOUND,
                DIRECTORY_SPECIFICATION_NOT_ALLOWED,
                BAD_FILE_PATH
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFStoragePathFormatExceptionData));
        }

    }
}