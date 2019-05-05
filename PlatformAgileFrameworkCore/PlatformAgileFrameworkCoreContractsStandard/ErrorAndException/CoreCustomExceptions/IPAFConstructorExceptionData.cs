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
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur during construction, mostly triggered during reflection
	/// operations. Interface allows aggregating of exception types.
	/// </summary>
	/// <remarks>
	/// Always use distinguishable names in interfaces members that will be
	/// aggregated.
	/// </remarks>
	public interface IPAFConstructorExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// Representation of the type we had a problem with.
		/// </summary>
		PAFTypeHolder ConstructionFailureType { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFConstructorExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFConstructorExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message
        /// </summary>
        public const string ATTEMPT_TO_INSTANTIATE_ABSTRACT_TYPE = "Attempt to instantiate pure interface";
        /// <summary>
        /// Error message
        /// </summary>
        public const string ATTEMPT_TO_INSTANTIATE_PURE_INTERFACE = "Attempt to instantiate pure interface";
        /// <summary>
        /// Error message
        /// </summary>
        public const string FAILED_TO_CONSTRUCT_TYPE = "Failed to construct type";
        /// <summary>
        /// Error message
        /// </summary>
        public const string PARAMETERLESS_CONSTRUCTOR_NOT_FOUND = "Parameterless constructor not found";
        /// <summary>
        /// Error message
        /// </summary>
        public const string CONSTRUCTOR_NOT_FOUND = "Constructor not found";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFConstructorExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                ATTEMPT_TO_INSTANTIATE_PURE_INTERFACE,
                ATTEMPT_TO_INSTANTIATE_ABSTRACT_TYPE,
                FAILED_TO_CONSTRUCT_TYPE,
                PARAMETERLESS_CONSTRUCTOR_NOT_FOUND,
                CONSTRUCTOR_NOT_FOUND
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFConstructorExceptionData));
        }

    }


}