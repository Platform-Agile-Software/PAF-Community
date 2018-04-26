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

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Exceptions that occur where multiple services must be identified
	/// as problematic.
	/// </summary>
	/// <remarks>
	/// Implementations should normally be at least "[PAFSerializable]".
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11Oct2014 </date>
	/// <description>
	/// Documented.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Implementations should be safe.
	/// </threadsafety>
	public interface IPAFServicesExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// Gets the problematic services.
		/// </summary>
		IEnumerable<IPAFServiceDescription> Services { get; }
		#endregion // Properties
		#region  Methods
		/// <summary>
		/// This method adds an service to the list of services if the list is not
		/// sealed. If a service passed to this method is <see langword="null"/>,
		/// this seals the list. The <see langword="null"/> service is not added
		/// to the list.
		/// </summary>
		/// <param name="service">Service to be added.</param>
		/// <exceptions>
		/// <exception> <see cref="InvalidOperationException"/> is thrown if
		/// the list has been sealed by adding a <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		void AddService(IPAFServiceDescription service);
		#endregion // Methods
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFServicesExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFServicesExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. There was an attempt to add a duplicate type to a
        /// service type dictionary.
        /// name.
        /// </summary>
        public const string DUPLICATE_TYPES_IN_SERVICE_TYPE_DICTIONARY
            = "Duplicate types in service type dictionary";
        /// <summary>
        /// Error message. There was an attempt to load a service dictionary
        /// with more than one service tagged as "default".
        /// </summary>
        public const string ONLY_ONE_DEFAULT_SERVICE_IMPLEMENTATION_IS_ALLOWED
            = "Only one default service implementation is allowed";
        /// <summary>
        /// Error message. There was an attempt to load a service dictionary
        /// with more than one service with a blank or <see langword="null"/>
        /// name.
        /// </summary>
        public const string ONLY_ONE_UNNAMED_SERVICE_IMPLEMENTATION_IS_ALLOWED
            = "Only one default service implementation is allowed";
        /// <summary>
        /// Error message. The types of two services needed to match and they
        /// did not.
        /// </summary>
        public const string SERVICE_TYPES_DO_NOT_MATCH
            = "Service types do not match";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFServicesExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                DUPLICATE_TYPES_IN_SERVICE_TYPE_DICTIONARY,
                ONLY_ONE_DEFAULT_SERVICE_IMPLEMENTATION_IS_ALLOWED,
                ONLY_ONE_UNNAMED_SERVICE_IMPLEMENTATION_IS_ALLOWED,
                SERVICE_TYPES_DO_NOT_MATCH
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFServicesExceptionData));
        }

    }

}