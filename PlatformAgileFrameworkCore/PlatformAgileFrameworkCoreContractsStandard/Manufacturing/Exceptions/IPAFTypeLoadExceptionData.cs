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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Manufacturing.Exceptions
{
	/// <summary>
	///	Exceptions that occur loading types from assemblies.
	/// </summary>
	[PAFSerializable]
	public interface IPAFTypeLoadExceptionData : IPAFAssemblyLoadExceptionData
	{
		#region Properties
		/// <summary>
		/// The target type.
		/// </summary>
		IPAFTypeHolder ProblematicType { get; }
		#endregion // Properties
	}

    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFTypeLoadExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFTypeLoadExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. Error finding a <see cref="Type"/> within an assembly.
        /// </summary>
        public const string TYPE_NOT_FOUND_IN_ASSEMBLY
            = "Type not found in assembly";
        /// <summary>
        /// Error message. Bad type string.
        /// </summary>
        public const string TYPE_STRING_NOT_RESOLVABLE_EXTERNALLY
            = "Type string not resolvable externally";
        /// <summary>
        /// Error message. Bad type string.
        /// </summary>
        public const string TYPE_STRING_NOT_RESOLVABLE_INTERNALLY
            = "Type string not resolvable internally";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFTypeLoadExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                TYPE_NOT_FOUND_IN_ASSEMBLY,
                TYPE_STRING_NOT_RESOLVABLE_EXTERNALLY,
                TYPE_STRING_NOT_RESOLVABLE_INTERNALLY
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFTypeLoadExceptionData));
        }

    }

}