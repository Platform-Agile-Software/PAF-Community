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

namespace PlatformAgileFramework.TypeHandling.Exceptions
{
	/// <summary>
	///	Exceptions that occur handling types where two types are incompatible.
	/// </summary>
	[PAFSerializable]
	public interface IPAFTypeMismatchExceptionData : IPAFTypeExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic type that is somehow incompatible with the
		/// <see cref="IPAFTypeExceptionData.ProblematicType"/>.
		/// </summary>
		IPAFTypeHolder ProblematicIncompatibleType { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFTypeMismatchExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFTypeMismatchExceptionData>
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
        /// This condition occurs when an <see cref="object"/> is accepted as a paremeter
        /// type and it is verified to be a certainly strongly-typed item, but is not. This
        /// mostly occurs in older interfaces that do not use generics.
        /// </summary>
        public const string IS_WRONG_TYPE_FOR_PARAMETER_TYPE = "Is wrong type for parameter type";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string TYPE_NOT_AN_INTERFACE_TYPE = "Type not an interface Type";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string TYPES_NOT_AN_EXACT_MATCH = "Types not an exact match";
        /// <summary>
        /// Error message. Normally used within static constructors to constrain a data type.
        /// </summary>
        public const string GENERIC_MUST_BE_SIGNED_INTEGER_TYPE = "Generic must be a signed integer Type";
        /// <summary>
        /// Error message. Normally used when a specific is expected to be castable to another, but is not.
        /// </summary>
        public const string FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE = "First type not castable to second Type";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFTypeMismatchExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
                {
                    ERROR_RESOLVING_TYPE,
                    FAILURE_TO_LOCATE_INFORMATION,
                    INTERNAL_ERROR_CREATING_TYPE,
                    IS_WRONG_TYPE_FOR_PARAMETER_TYPE,
                    TYPE_NOT_AN_INTERFACE_TYPE,
                    TYPES_NOT_AN_EXACT_MATCH,
                    GENERIC_MUST_BE_SIGNED_INTEGER_TYPE
                };
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFTypeMismatchExceptionData));
        }

    }


}