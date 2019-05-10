//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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

#region Using Directives
using System;
using System.Collections.Generic;
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	///	A little add-on for processing files. This does the final
	/// processing for a set of log files, but can be used for
	/// more general purposes..
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 06jan2019 </date>
	/// <description>
	/// New. Initially built to allow better implementation of JSON logger or
	/// other formatted or other "dispatching" loggers.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe if directories are locked during the dispatch.
	/// </threadsafety>
	public interface IPAFFormattingFileDispatcher
	{
		#region Properties
		/// <summary>
		/// This is the formatting delegate. If this is
		/// <see langword="null"/>, files are typically just moved.
		/// </summary>
		Func<string,string> FileFormatter { get; }
		/// <summary>
		/// Storage area, typically a directory or a URL to transmit to.
		/// </summary>
		string DispatchDirectory { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// This method processes files, typically to a directory <see cref="DispatchDirectory"/>
		/// where they could be acted upon by another process or thread. Do NOT put anything in
		/// here that takes a long time. If asynchronicty is needed , do it on the dispatch
		/// directory on a separate thread.
		/// </summary>
		/// <param name="filesToMove">
		/// The set of files to move. These must have full directory specifications
		/// on the front. Otherwise the read will be done from the CWD, which is
		/// NOT supported on all platforms.
		/// </param>
		/// <remarks>
		/// <see langword="null"/> file names are ignored. This is a method that is
		/// often hooked as a delegate in dispatching loggers.
		/// </remarks>
		void DispatchFiles(IList<string> filesToMove = null);
		#endregion // Methods
	}
}
