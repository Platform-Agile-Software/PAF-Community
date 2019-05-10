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
using System.Diagnostics;
using System.IO;
#endregion // Using Directives

namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// This interface is normally implemented in a "full" ECMA assembly, since
	/// .Net standard only exposes <see cref="TraceListener"/>, but not its
	/// subclasses that allow constructing a writer.
	/// </summary>
	/// <remarks>
	/// We manufacture <c>TextWriterTraceListener</c>s. This requires
	/// a <see cref="Stream"/> input. Clients can use this as a model
	/// to construct other types of writers.
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04mar2019 </date>
	/// <description>
	/// New. Trace is now accessible on all platforms we use so we put
	/// this in .Net standard core.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFTraceLoggerFactory
	{
		#region Methods
		/// <summary>
		/// Instantiates a new listener and adds it to the listener collection.
		/// </summary>
		/// <param name="outputStream">
		/// We provide a stream (not a file name) so that we can handle all
		/// storage allocation abstractly from the portable core. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if something went wrong. The client
		/// should NOT call any of the methods on the returned listener
		/// if thread-safety is to be achieved. These methods should ONLY
		/// be called internally by the <see cref="Trace"/> infrastructure.
		/// The synchronization model inside <see cref="Trace"/> is complex
		/// and we don't expose it to the client. The only thing that should
		/// be done with the <see cref="TraceListener"/> is to pass it to
		/// <see cref="DestroyListener"/>. Do NOT call the
		/// <see cref="IDisposable.Dispose"/> method on <see cref="TraceListener"/>.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"><c>outputStream</c></exception>
		/// </exceptions>
		TraceListener BuildTextWriterListener(Stream outputStream);
		/// <summary>
		/// This method disposes the listener and removes it from the listener
		/// collection.
		/// </summary>
		/// <param name="traceListener">
		/// Listener to be destroyed. 
		/// </param>
		void DestroyListener(TraceListener traceListener);

		#endregion // Methods
	}
}