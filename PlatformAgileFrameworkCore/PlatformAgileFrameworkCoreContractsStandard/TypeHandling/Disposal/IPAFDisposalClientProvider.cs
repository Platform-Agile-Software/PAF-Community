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

using System;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Interface supporting the PAF dispose pattern. Useful for
	/// class composition.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 04mar2012 </date>
	/// <contribution>
	/// Factored out of typehandling for use in core. Anybody doing .Net
	/// programming should have this.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not needed in implementations - only a get from a class that is normally immutable.
	/// </threadsafety>
	public interface IPAFDisposalClientProvider
	{
		#region Properties
		/// <summary>
		/// <para>
		/// Fetches an <see cref="object"/> that is reference to a client
		/// for <see cref="PAFDisposerBase{T}"/>.
		/// </para>
		/// <para>
		/// Can be <see langword="null"/>. This is used to hold references to clients,
		/// but it is not needed if a disposal key or reference to an
		/// <see cref="IDisposable"/> is not needed. This is often the case
		/// when a disposal method is supplied by the client. In that case,
		/// the method is simply called. When surrogate disposers like
		/// <see cref="PAFDisposer{T}"/> are used, it is a good idea to
		/// load the instance of the class that is ultimately being disposed
		/// or it's type if a static class. This allows more detailed error
		/// reporting.
		/// </para>
		/// <para>
		/// This returned object is also used as an implementor of
		/// an internal interface that allows secure disposal.
		/// </para>
		/// </summary>
		object DisposalClient { get; }
		#endregion // Properties
	}
}

