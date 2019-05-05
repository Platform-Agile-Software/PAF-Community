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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
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

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Exceptions that occur handling services.
	/// </summary>
	[PAFSerializable]
	public interface IPAFServiceExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic service. <see cref="IPAFServiceDescription"/>).
		/// </summary>
		IPAFServiceDescription ProblematicService { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFServiceExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFServiceExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. Normally used when we need to locate an unique service
        /// and we find multiples with the same characteristics.
        /// </summary>
        public const string MULTIPLE_IMPLEMENTATIONS_FOUND = "Multiple implementations found";
	    /// <summary>
	    /// Error message. Normally used when a client attempts to install
	    /// a service object that does not inherit from <see cref="IPAFService"/>.
	    /// </summary>
	    public const string OBJECT_NOT_A_SERVICE = "Object not a service";
		/// <summary>
	    /// Error message. Normally used when a client attempts to install
	    /// an interface type or an implementation type that does not inherit
	    /// from <see cref="IPAFService"/>.
	    /// </summary>
	    public const string TYPE_NOT_A_SERVICE = "Type not a service";
        /// <summary>
		/// Error message.
		/// </summary>
		public const string SERVICE_ALREADY_CREATED = "Service already created";
        /// <summary>
        /// Error message. General message used in an exception that often wraps another.
        /// </summary>
        public const string SERVICE_CREATION_FAILED = "Service creation failed";
        /// <summary>
        /// Used when a concrete class implementing a service interface cannot be located.
        /// </summary>
        public const string SERVICE_IMPLEMENTATION_NOT_FOUND = "Service implementation not found";
        /// <summary>
        /// Error message. This error message is the only one to be used when a
        /// service cannot be found within the manager. Use this when no construction
        /// exception or other exceptions occur, but the service is not contained
        /// in the SM's catalog.
        /// </summary>
        public const string SERVICE_NOT_FOUND = "Service not found";
	    /// <summary>
	    /// Error message. Normally used when a service infrastructure method
	    /// expects a pure interface type.
	    /// </summary>
	    public const string TYPE_NOT_AN_INTERFACE_TYPE = "Type not an interface type";
        /// <summary>
		/// Error message. Normally used when an object intended to be used as
		/// a service does not implement a required interface.
		/// </summary>
		public const string TYPE_DOES_NOT_IMPLEMENT_INTERFACE
            = "Type does not implement interface";
	    /// <summary>
	    /// Error message. Issued when a type must be resolved as a <see cref="Type"/>
	    /// and it is not.
	    /// </summary>
	    public const string TYPE_IS_NOT_RESOLVED = "Type is not resolved";
	    /// <summary>
	    /// Error message. Issued when a type string is not specified.
	    /// </summary>
	    public const string TYPE_IS_NOT_SPECIFIED = "Type is not specified";
		/// <summary>
		/// Error message. Used when an attempt is made to install a service with the
		/// same interface type and name as one already held by the service manager.
		/// </summary>
		public const string DUPLICATE_SERVICE = "Duplicate service";
        #endregion // Fields and Autoproperties
		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFServiceExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                    MULTIPLE_IMPLEMENTATIONS_FOUND,
                    OBJECT_NOT_A_SERVICE,
                    SERVICE_ALREADY_CREATED,
                    SERVICE_CREATION_FAILED,
                    SERVICE_NOT_FOUND,
                    TYPE_DOES_NOT_IMPLEMENT_INTERFACE,
                    TYPE_IS_NOT_RESOLVED,
                    TYPE_NOT_AN_INTERFACE_TYPE,
					DUPLICATE_SERVICE
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFServiceExceptionData));
        }

    }
}