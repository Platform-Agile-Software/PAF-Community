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
namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// Just an interface to supply a non - Generic payload and delegate to a
	/// thread or task. We need a non - Generic so we can variegated work objects.
	/// We don't constrain them to have a single Generic type. We group the
	/// payload and method in this interface just to get a lessor chance of mismatch
	/// of the objects, since they are normally loaded together.
	/// </summary>
	/// <threadsafety>
	/// Must be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30mar2019 </date>
	/// <description>
	/// Built this as a convenience for new stochastic tests.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IAsyncWorkPayloadObject
	{
		#region Properties
		/// <summary>
		/// This is the "payload" object passed into the thread delegate.
		/// If <see cref="IDisposable"/>, this object may be disposed more
		/// than once, since it may be shared. The object must withstand
		/// multiple dispose calls. When using Generic thread payloads, this
		/// is often loaded with a Generic.
		/// </summary>
		object Payload { get; }
		/// <summary>
		/// Thread delegate accepting argument. Needed for
		/// <see cref="System.Threading.Thread"/> - based concurrency.
		/// When using Generic thread payloads, this
		/// is often passed a Generic.
		/// </summary>
		Action<object> ThreadDelegate { get; }
		#endregion // Properties
	}
}
