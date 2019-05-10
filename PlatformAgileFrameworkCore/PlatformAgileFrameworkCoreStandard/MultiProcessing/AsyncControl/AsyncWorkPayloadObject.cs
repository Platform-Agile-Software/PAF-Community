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
using PlatformAgileFramework.Annotations;
namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// Default implementation of <see cref="IAsyncWorkPayloadObject"/>
	/// </summary>
	/// <threadsafety>
	/// Generally NOT thread-safe if the payload is shared.
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
	public class AsyncWorkPayloadObject: IAsyncWorkPayloadObject
	{
		/// <summary>
		/// Backing for the prop. - internal for testing ONLY.
		/// </summary>
		protected internal object m_Payload;
		/// <summary>
		/// Backing for the prop. - internal for testing ONLY.
		/// </summary>
		internal Action<object> m_ThreadDelegate;

		/// <summary>
		/// Constructor just loads fields.
		/// </summary>
		/// <param name="payload">Loads <see cref="Payload"/>.</param>
		/// <param name="threadDelegate">Loads <see cref="ThreadDelegate"/>.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> <paramref name="payload"/></exception>
		/// <exception cref="ArgumentNullException"> <paramref name="threadDelegate"/></exception>
		/// </exceptions>
		public AsyncWorkPayloadObject([NotNull] object payload, [NotNull] Action<object> threadDelegate)
		{
			m_Payload = payload ?? throw new ArgumentNullException(nameof(payload));
			m_ThreadDelegate = threadDelegate ?? throw new ArgumentNullException(nameof(threadDelegate));
		}

		/// <summary>
		/// <see cref="IAsyncWorkPayloadObject"/>
		/// </summary>
		public virtual object Payload
		{
			get => m_Payload;
			protected set => m_Payload = value;
		}
		/// <summary>
		/// <see cref="IAsyncWorkPayloadObject"/>
		/// </summary>
		public virtual Action<object> ThreadDelegate
		{
			get => m_ThreadDelegate;
			protected set => m_ThreadDelegate = value;
		}
	}
}