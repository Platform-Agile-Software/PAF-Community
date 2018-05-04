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
using System.Security;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Interface supporting the PAF dispose pattern. Useful for
	/// class composition. Needed for one more dimension of flexibility
	/// in the disposal needed for a specific app. The method is designed
	/// to check for the correct security key before handing out the
	/// unprotected (not marked with <see cref="SecurityCriticalAttribute"/>)
	/// <see cref="IDisposable"/> implementation for a type. This
	/// interface allows the use of a class instance in a "using" block,
	/// ensuring that the instance is disposed after use.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 29may2012 </date>
	/// <contribution>
	/// New.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Any implementations should NORMALLY be made thread-safe.
	/// </threadsafety>
	public interface IUnprotectedDisposableProvider
	{
		#region Properties
		/// <summary>
		/// Fetches an <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="secretKey">
		/// Object containing the secret disposal key.
		/// </param>
		IDisposable GetUnprotectedDisposable(object secretKey);
		#endregion // Properties
	}
}

