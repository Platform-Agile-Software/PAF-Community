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

namespace PlatformAgileFramework.MultiProcessing.Threading.Exceptions
{
	/// <summary>
	///	Exceptions that occur on threads during asynchronous processing.
	/// </summary>
	public interface IPAFThreadExecutionExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// Name of the managed thread that had the problem. Can be <see langword="null"/>
		/// if the exception did not happen on a specific thread, but perhaps
		/// within the framework. May also be <see langword="null"/> if the thread was
		/// never named. Then the ID is the only representation of the thread.
		/// </summary>
		string ExecutingThreadName { get; }
		/// <summary>
		/// ID of the managed thread that had the problem. Can be -1
		/// if the exception did not happen on a specific thread, but perhaps
		/// within the framework.
		/// </summary>
		int ExecutingThreadID { get; }
		#endregion // Properties
	}

    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFThreadExecutionExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFThreadExecutionExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message
        /// </summary>
        public const string NON_UNIQUE_THREAD_NAME = "Non-unique thread name.";
        /// <summary>
        /// Error message. Not really an error - we use exceptions to communicate
        /// normal terminations, also.
        /// </summary>
        public const string NORMAL_TERMINATION = "Normal Termination.";
        /// <summary>
        /// Error message. This can be an exception condition or not. A task may be
        /// set to run for a maximum time. The task timing out may be a normal
        /// condition.
        /// </summary>
        public const string PROCESSING_TIMEOUT = "Processing timeout.";
        /// <summary>
        /// Error message. This is an error condition caused by the need to abort
        /// a task. In the SL low-trust model, thread aborts are not able to be
        /// initiated from user code. In elevated-trust environments, they can be.
        /// </summary>
        public const string PROCESSING_ABORT = "Processing abort.";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFThreadExecutionExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                NON_UNIQUE_THREAD_NAME,
                NORMAL_TERMINATION,
                PROCESSING_ABORT,
                PROCESSING_TIMEOUT
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFThreadExecutionExceptionData));
        }

    }

}