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
	/// Default implementation of <see cref="IAsyncGenericWorkPayloadObject{T}"/>.
	/// We generally want to have the payload as an object and a Generic
	/// so we can choose to handle it in a variegated fashion, or in
	/// a typed fashion. We create the Generic and non - Generic in a little
	/// package (this) to avoid most type un-safety issues.
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
	public class AsyncGenericWorkPayloadObject<T>
		: AsyncWorkPayloadObject, IAsyncGenericWorkPayloadObject<T>
		where T: class
	{
		/// <summary>
		/// Backing for the prop. - internal for testing ONLY.
		/// </summary>
		internal T m_GenericPayload;
		/// <summary>
		/// Backing for the prop. - internal for testing ONLY.
		/// </summary>
		internal Action<T> m_GenericThreadDelegate;
		/// <summary>
		/// Constructor just loads fields. Loads the Generic payload into the non-Generic.
		/// </summary>
		/// <param name="genericPayload">Loads <see cref="GenericPayload"/>.</param>
		/// <param name="threadDelegate">Loads <see cref="AsyncWorkPayloadObject.ThreadDelegate"/>.</param>
		/// <param name="genericThreadDelegate">Loads <see cref="GenericThreadDelegate"/>.</param>
		/// <param name="payload">
		/// This is an optional argument. If it's missing, the non - Generic <see cref="IAsyncWorkPayloadObject.Payload"/>
		/// will simply be set to the Generic as in the standard Generic cover procedure for non - Generics. If
		/// it is specified, the pattern here allows the supply of a payload that can somehow be converted
		/// to the generic within the <see cref="threadDelegate"/> or could be arbitrary. This is a bit
		/// of type-un-safety, but allows a lot of code reduction.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> <paramref name="genericPayload"/></exception>
		/// Also see base constructor.
		/// </exceptions>
		public AsyncGenericWorkPayloadObject
			([NotNull] T genericPayload,
			[NotNull] Action<object> threadDelegate,
			[NotNull] Action<T> genericThreadDelegate,
			object payload = null)
		:base(genericPayload, threadDelegate)
		{
			m_GenericThreadDelegate
				= genericThreadDelegate ?? throw new ArgumentNullException(nameof(genericThreadDelegate));
			if (payload != null)
				m_Payload = payload;
		}

		/// <summary>
		/// <see cref="IAsyncGenericWorkPayloadObject{T}"/>
		/// </summary>
		public virtual T GenericPayload
		{
			get => m_GenericPayload;
			protected set => m_GenericPayload = value;
		}
		/// <summary>
		/// <see cref="IAsyncGenericWorkPayloadObject{T}"/>
		/// </summary>
		public virtual Action<T> GenericThreadDelegate
		{
			get => m_GenericThreadDelegate;
			protected set => m_GenericThreadDelegate = value;
		}
	}
}