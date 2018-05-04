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

using System.Collections.Generic;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.ErrorAndException;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur handling types.
	/// </summary>
	[PAFSerializable]
	public interface IPAFTypeExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic type.
		/// </summary>
		IPAFTypeHolder ProblematicType { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFTypeExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFTypeExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. Usually results from trying to locate a type in a set
        /// of assemblies.
        /// </summary>
        public const string ERROR_RESOLVING_TYPE = "Error Resolving Type";
        /// <summary>
        /// Error message. Indicates the type was not found.
        /// </summary>
        public const string FAILURE_TO_LOCATE_INFORMATION = "Failure to locate information about a Type";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string INTERNAL_ERROR_CREATING_TYPE = "Internal Error Creating Type";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string TYPE_NOT_AN_INTERFACE_TYPE = "Type not an interface Type";
        /// <summary>
        /// Error message. Normally used within static constructors to constrain a data type.
        /// </summary>
        public const string GENERIC_MUST_BE_SIGNED_INTEGER_TYPE = "Generic must be a signed integer Type";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFTypeExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
                {
                    ERROR_RESOLVING_TYPE,
                    FAILURE_TO_LOCATE_INFORMATION,
                    INTERNAL_ERROR_CREATING_TYPE,
                    TYPE_NOT_AN_INTERFACE_TYPE,
                    GENERIC_MUST_BE_SIGNED_INTEGER_TYPE
                };
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFTypeExceptionData));
        }

    }
}