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
using System.Diagnostics;
using System.IO;
#endregion // Using Directives

namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// Default implementation of <see cref="IPAFTraceLoggerFactory"/>.
	/// </summary>
	/// <remarks>
	/// This should be a singleton, but doesn't need to implement
	/// any sort of singleton pattern - implementation should be
	/// created once at system startup and pushed into
	/// <see cref="PAFLoggingUtils.TraceLoggerFactory"/>.
	/// </remarks>
	/// <threadsafety>
	/// Not safe. Our usage model doesn't require it. Any listeners should
	/// be held in a common place at the application level and destroyed
	/// upon shutdown.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04mar2019 </date>
	/// <description>
	/// New. Trace is now accessible on all platforms so we put
	/// this in ECMA common.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFTraceLoggerFactory: IPAFTraceLoggerFactory
	{
		#region Methods
		/// <summary>
		/// <see cref="IPAFTraceLoggerFactory"/>.
		/// </summary>
		/// <param name="outputStream">
		/// <see cref="IPAFTraceLoggerFactory"/>.
		/// </param>
		/// <returns>
		/// <see cref="IPAFTraceLoggerFactory"/>.
		/// </returns>
		/// <exceptions>
		/// <see cref="IPAFTraceLoggerFactory"/>.
		/// </exceptions>
		public virtual TraceListener BuildTextWriterListener(Stream outputStream)
		{
			var textWriterTraceListener = new TextWriterTraceListener(outputStream);
			Trace.Listeners.Add(textWriterTraceListener);
			return textWriterTraceListener;
		}
		/// <summary>
		/// <see cref="IPAFTraceLoggerFactory"/>.
		/// </summary>
		/// <param name="traceListener">
		/// <see cref="IPAFTraceLoggerFactory"/>.
		/// </param>
		public virtual void DestroyListener(TraceListener traceListener)
		{
			traceListener.Dispose();
			if(Trace.Listeners.Contains(traceListener))
				Trace.Listeners.Remove(traceListener);
		}

		#endregion // Methods
	}
}