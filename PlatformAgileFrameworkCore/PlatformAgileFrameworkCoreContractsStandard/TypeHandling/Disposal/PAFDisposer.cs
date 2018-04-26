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

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Basic class to support <see cref="IPAFDisposable"/>. Sealed subclass of
	/// <see cref="PAFDisposerBase{T}"/>.
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02may2012 </date>
	/// <contribution>
	/// New. Did the unsealed base/sealed subclass thing for the framework.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// thread-safe.
	/// </threadsafety>
	public sealed class PAFDisposer<T>: PAFDisposerBase<T>
	{
		#region Constructors
		/// <summary>
		/// See base class.
		/// </summary>
		/// <param name="clientProvider">
		/// See base class.
		/// </param>
		/// <param name="disposalDelegate">
		/// See base class.
		/// </param>
		public PAFDisposer(IPAFDisposalClientProvider clientProvider,
			PAFDisposerMethod disposalDelegate = null):base(clientProvider, disposalDelegate){}

		/// <summary>
		/// See base class.
		/// </summary>
		/// <param name="disposalClient">
		/// See base class.
		/// </param>
		/// <param name="secretKey">
		/// See base class.
		/// </param>
		/// <param name="disposalDelegate">
		/// See base class.
		/// </param>
		/// <remarks>
		/// See base class.
		/// </remarks>
		public PAFDisposer(T secretKey, object disposalClient = null,
			PAFDisposerMethod disposalDelegate = null)
			: base(secretKey, disposalClient, disposalDelegate){}
		#endregion // Constructors
	}
}

