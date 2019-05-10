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

using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Enumerators
{	/// <summary>
	/// <para>
	/// Please see <see cref="IPAFResettableEnumerableProvider{T}"/> for the rationale
	/// of the resettable enumeration interfaces. This interface places the reset
	/// functionality directly on the <see cref="IEnumerator{T}"/>.
	/// </para>
	/// <para>
	/// This interface allows an enumerator to be directly reset, so it can be handed
	/// out over and over again after reset, possibly with different data. It is useful
	/// in cases where enumerators are costly to produce. This is different than the
	/// <see cref="IPAFResettableEnumerableProvider{T}"/>, which essentially resets an
	/// enumerator factory to produce enumerators which hand out different values,
	/// perhaps only from time-to-time.
	/// </para>
	/// </summary>
	/// <typeparam name="T">Type that is to be enumerated.</typeparam>
	/// <threadsafety>
	/// Implementations need not necessarily be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 14oct2012 </date>
	/// <contribution>
	/// <para>
	/// Added history and DOCs - people did not understand this interface.
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFResettableEnumerator<T>: IPAFResettableEnumeration<T>, IEnumerator<T>
	{
	}
}