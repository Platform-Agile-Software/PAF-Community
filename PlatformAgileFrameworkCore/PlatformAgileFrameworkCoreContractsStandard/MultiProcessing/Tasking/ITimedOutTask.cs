//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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
using System.Threading.Tasks;

#endregion

namespace PlatformAgileFramework.MultiProcessing.Tasking
{
	/// <summary>
	/// This is a protocol for helper class that deals with the issue of methods with async/await
	/// and many awaits which have no timeouts. This is a payload that can be used
	/// in conjunction with a <see cref="Task"/> to return both a timeout indication and
	/// an exception when we don't want the exception to be thrown on a calling thread. This
	/// helper can be used when we don't want to throw exceptions
	/// inside an async method that is awaited. Additionally, sometimes we don't care if a task
	/// has timed out, we just have to know it. This is especially useful for async void
	/// methods, which are all over the place because of MS's mis-design of the event system.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 14jul2019 </date>
	/// <description>
	/// New. Made little helper for async/await problems.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	public interface ITimedOutTask
	{
		/// <summary>
		/// If this value is <see langword="true"/>, the return value will not be valid.
		/// </summary>
		bool TimedOut { get; set; }
		/// <summary>
		/// Place to capture and return an exception so it does not get rethrown on
		/// a calling thread, which doesn't work anyway if the awaits are stacked.
		/// See Blewett and Clymer.This gives an application the ability to push an
		/// exception received from the async method into a receiver at the point of
		/// completion of the method in a reliable fashion. This i good if the developer
		/// needs to signal a UI immediately about some problem from deep within
		/// nested logic that otherwise is difficult to reliably propagate up the
		/// stack.
		/// </summary>
		Exception CaughtException { get; set; }

	}
}
