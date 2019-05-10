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

using System;
using PlatformAgileFramework.MultiProcessing.AsyncControl;
using PlatformAgileFramework.StochasticTestHelpers;

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Closure of the delegator for doing some writing.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 24mar2019 </date>
	/// <description>
	/// New. Built to test file writer on multiple threads.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not thread-safe. The method exposed by this class is designed to be
	/// called by a single thread.
	/// </threadsafety>
	public class FileWriterCallDelegator :
		TimedCallDelegator<IPAFFileWriter>
	{
		#region Constructors
		/// <summary>
		/// Builds with a writer delegate and writer.
		/// </summary>
		/// <param name="fileWriterDelegate">
		/// The delegate, which is typically called on a separate thread on each
		/// instance of this class.
		/// </param>
		/// <param name="fileWriter">
		/// The writer, which is typically shared among instances in order to
		/// provoke thread collisions.
		/// </param>
		/// <param name="randomSeed">
		/// See base class.
		/// </param>
		/// <param name="defaultDelegatePayload">
		/// See base class.
		/// </param>
		/// <param name="millisecondDelayMask">
		/// See base class.
		/// </param>
		public FileWriterCallDelegator(
			Action<IPAFFileWriter> fileWriterDelegate,
			IPAFFileWriter fileWriter,
			object defaultDelegatePayload = null,
			int millisecondDelayMask = int.MinValue,
			int randomSeed = int.MinValue)
			:base(fileWriterDelegate, fileWriter, defaultDelegatePayload,
				millisecondDelayMask, randomSeed)
		{
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// This override runs the base method in a loop until signaled to terminate.
		/// It then stops and sets <see cref="IAsyncControlObject.ProcessHasTerminated"/>.
		/// </summary>
		/// <param name="controlObject">
		/// Must be a <see cref="IAsyncControlObject"/>.
		/// </param>
		public override void DelegateDelayCaller(object controlObject)
		{
			var asyncControlObject = (IAsyncControlObject) controlObject;

			while (!asyncControlObject.ProcessShouldTerminate)
			{
				base.DelegateDelayCaller(ThreadMethodArgument);
			}

			asyncControlObject.ProcessHasTerminated = true;
		}
		#endregion // Methods
	}
}