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
	/// Just an interface to supply a Generic payload and delegate to a
	/// thread or task. We need a Generic so we can overload the non - Generic
	/// in a type that contains the delegate and payload.
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
	public interface IAsyncGenericWorkPayloadObject<T>: IAsyncWorkPayloadObject
		where T : class
	{
		#region Properties
		/// <summary>
		/// This is the "payload" object passed into the thread delegate.
		/// If <see cref="IDisposable"/>, this object may be disposed more
		/// than once, since it may be shared. The object must withstand
		/// multiple dispose calls.
		/// </summary>
		T GenericPayload { get; }
		/// <summary>
		/// Thread delegate accepting argument. Needed for
		/// <see cref="System.Threading.Thread"/> - based concurrency.
		/// </summary>
		Action<T> GenericThreadDelegate { get; }
		#endregion // Properties
	}
}
