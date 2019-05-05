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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;
namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur handling types.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 07feb2019 </date>
	/// <description>
	/// Added history. Added one error message.
	/// </description>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public interface IPAFTypeExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic type. This may be <see langword="null"/> if the type
		/// cannot be found.
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
	    /// This message is issued whenever a <see cref="Type"/>  is <see langword="null"/>
	    /// and it should not be.
	    /// </summary>
	    public const string TYPE_CANNOT_BE_NULL = "Type cannot be null.";
		/// <summary>
		/// This message is issued whenever a string representation of a
		/// <see cref="Type"/> is <see langword="null"/> or <see cref="string.Empty"/>
		/// and it should not be.
		/// </summary>
		public const string TYPE_STRING_CANNOT_BE_NULL = "Type string representation cannot be null.";
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
					TYPE_CANNOT_BE_NULL,
					TYPE_STRING_CANNOT_BE_NULL,
                    GENERIC_MUST_BE_SIGNED_INTEGER_TYPE
                };
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFTypeExceptionData));
        }

    }
}