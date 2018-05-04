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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Collections.Exceptions
{
	/// <summary>
	///	Exceptions that occur when an attempt is made to add a bad entry to a
	/// collection or it is determined that a collection has an inappropriate entry.
	/// </summary>
	[PAFSerializable]
	public interface IPAFBadCollectionEntryExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic entry that is somehow incompatible with the
		/// collection.
		/// </summary>
		object BadCollectionEntry { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFBadCollectionEntryExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFBadCollectionEntryExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message.
        /// </summary>
        public const string BAD_COLLECTION_ENTRY
            = "Bad collection entry";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFBadCollectionEntryExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                BAD_COLLECTION_ENTRY
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFBadCollectionEntryExceptionData));
        }

    }

}