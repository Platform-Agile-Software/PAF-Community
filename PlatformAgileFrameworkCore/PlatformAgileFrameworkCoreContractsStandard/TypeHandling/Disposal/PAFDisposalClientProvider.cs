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
	/// Default implementation of the interface.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 04mar2012 </date>
	/// <contribution>
	/// Built so Clients can be easily provided.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe if developers do not attempt reset of content
	/// during application operation.
	/// </threadsafety>
	public class PAFDisposalClientProvider: IPAFDisposalClientProvider
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		protected object m_DisposalClient;
		/// <summary>
		/// See <see cref="IPAFDisposalClientProvider"/>.
		/// </summary>
		public virtual object DisposalClient
		{ get { return m_DisposalClient; } protected internal set { m_DisposalClient = value; } }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Just installs our client.
		/// </summary>
		/// <param name="disposalClient">
		/// See <see cref="IPAFDisposalClientProvider"/>.
		/// </param>
		public PAFDisposalClientProvider(object disposalClient)
		{
			m_DisposalClient = disposalClient;
		}
		#endregion // Constructors
	}
}



