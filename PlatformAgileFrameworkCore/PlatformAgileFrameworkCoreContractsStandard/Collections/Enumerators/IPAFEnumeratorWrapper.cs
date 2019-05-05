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
{   /// <summary>
	/// Wraps an <see cref="IEnumerable{T}"/> and manipulates it's enumerator.
	/// </summary>
	/// <typeparam name="T">Type that is to be enumerated.</typeparam>
	/// <threadsafety>
	/// Implementations need not necessarily be thread-safe. The enumerators handed
	/// out by providers like <c>IPAFResettableEnumerableProvider{T}</c> are
	/// normally NOT expected to be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 15oct2012 </date>
	/// <contribution>
	/// <para>
	/// New, so we don't have to use <c>IPAFResettableEnumerable{T}</c>.
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFEnumeratorWrapper<out T>: IEnumerator<T>, IEnumerable<T>
{
		#region Properties
		/// <summary>
		/// Accesses the <see cref="IEnumerable{T}"/> we were built with. This
		/// may be <see langword="null"/> if this class has been disposed.
		/// </summary>
		IEnumerable<T> InnerEnumerable { get; }
		#endregion // Properties
	}
}