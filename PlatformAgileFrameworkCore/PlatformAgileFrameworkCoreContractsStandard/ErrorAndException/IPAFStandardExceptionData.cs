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
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.StringParsing;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	The base interface which must be implemented by all Standard PAF exceptions.
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 22apr2011 </date>
	/// <contribution>
	/// Refactored this out of the monolithic program so Core extenders could have
	/// an extensibility capability.
	/// </contribution>
	/// </history>
	public interface IPAFStandardExceptionData: IPAFFormattable
	{
		#region Properties
		/// <summary>
		///	Gets the optional data associated with exception extensions.
		/// </summary>
		object ExtensionData { get; }
		/// <summary>
		///	Gets a value indicating that this exception is fatal. Fatal
		/// exceptions halt application execution. Defaults to <see langword="null"/>.
		/// </summary>
		bool? IsFatal { get; }
		/// <summary>
		///	Gets the log level. This indicates if and when this exception is logged. 
		///	Defaults to <see langword="null"/>, which always logs.
		/// </summary>
		PAFLoggingLevel? LogLevel { get; }
		/// <summary>
		/// Gets the tags that go into the resource system for specific exceptions.
		/// </summary>
		IEnumerable<string> SpecificExceptionTags { get; }
		#endregion Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFStandardExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFStandardExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. This indicates an unknown failure.
        /// </summary>
        public const string UNKNOWN_FAILURE = "Unknown failure";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFStandardExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                UNKNOWN_FAILURE
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFStandardExceptionData));


        }
    }

}