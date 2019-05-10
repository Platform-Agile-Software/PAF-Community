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
using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.KeyedCollections
{
	/// <summary>
	/// Subinterface of the <see cref="ICollection{T}"/> interface that uses
	/// keyed objects. This particular collection requires elements that have
	/// exposed comparers on them.
	/// </summary>
	/// <typeparam name="T">
	/// Generic type held in the collection.
	/// </typeparam>
	/// <typeparam name="U">
	/// Generic type of the key that items in the collection must have.
	/// </typeparam>
	/// <remarks>
	/// Noted that this interface does not constrain elements to
	/// implement an equality comparision. This usually means
	/// the collection must have an equality comparision function
	/// attached to it.
	/// </remarks>
	public interface IPAFKeyedCollection<T, in U> : ICollection<T>
		where T : IPAFKeyedObject<U>
	{
		/// <summary>
		/// Attempts to locate and return a named item. TryGet style needed for
		/// value types.
		/// </summary>
		/// <param name="key">Key of the item.</param>
		/// <param name="item">The item.</param>
		/// <returns>Whether the item was successfully located.</returns>
		bool TryGetKeyedItem(U key, out T item);
		/// <summary>
		/// Optional equality comparer used if elements do not have
		/// equality comparer attached.
		/// </summary>
		IEqualityComparer<U> KeyEqualityComparer { get; }
	}
}
