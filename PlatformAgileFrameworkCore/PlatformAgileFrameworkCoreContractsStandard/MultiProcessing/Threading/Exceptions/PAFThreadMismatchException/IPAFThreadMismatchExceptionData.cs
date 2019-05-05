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

using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.MultiProcessing.Threading.Exceptions
{
	/// <summary>
	///	Exceptions that occur on threads during asynchronous processing.
	/// These exceptions usually involve a method being called on an
	/// incorrect thread.
	/// </summary>
	public interface IPAFThreadMismatchExceptionData:  IPAFThreadExecutionExceptionData
	{
		#region Properties
		/// <summary>
		/// Name of the managed thread that was in conflict. Can be <see langword="null"/>
		/// if the thread was never named. Then the ID is the only representation of the thread.
		/// </summary>
		/// <remarks>
		/// This is normally the name of the thread that SHOULD have been executing,
		/// as opposed to the thread that is ACTUALLY executing in the case of a
		/// single-thread method.
		/// </remarks>
		string ConflictingThreadName { get; }
		/// <summary>
		/// ID of the managed thread that was in conflict.
		/// </summary>
		/// <remarks>
		/// This is normally the ID of the thread that SHOULD have been executing,
		/// as opposed to the thread that is ACTUALLY executing in the case of a
		/// single-thread method.
		/// </remarks>
		int ConflictingThreadID { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFThreadMismatchExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFThreadMismatchExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. This is an error condition caused by a method being called
        /// by other than the task that first that created its object or other than the
        /// first task that called the method. This is used to warn when other than a single
        /// designated task is calling a method that accesses unsynchronized data. If this
        /// condition occurs, the application should normally be terminated. Thread name
        /// property is used to hold stringful representation of <see cref="Task.Id"/>s.
        /// ThreadId property of the exception is set to -1.
        /// </summary>
        public const string MULTIPLE_TASK_ACCESS = "Multiple Task Access";
        /// <summary>
        /// Error message. This is an error condition caused by a method being called
        /// by other than the thread that created its object or other than the first thread
        /// that called the method. This is used to warn when other than a single
        /// designated thread is calling a method that accesses unsynchronized data. If
        /// this condition occurs, the application should normally be terminated.
        /// </summary>
        public const string MULTIPLE_THREAD_ACCESS = "Multiple Thread Access";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFThreadMismatchExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                MULTIPLE_TASK_ACCESS,
                MULTIPLE_THREAD_ACCESS
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFThreadMismatchExceptionData));
        }

    }


}