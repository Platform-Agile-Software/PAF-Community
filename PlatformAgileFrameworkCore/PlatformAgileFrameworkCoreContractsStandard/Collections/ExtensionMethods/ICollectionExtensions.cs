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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlatformAgileFramework.Collections.ExtensionMethods
{
	/// <summary>
	/// This class contain some extension methods for the <see cref="ICollection{T}"/>.
	/// </summary>
// ReSharper disable InconsistentNaming
	public static class ICollectionExtensions
// ReSharper restore InconsistentNaming
	{
		/// <summary>
		/// This method allows an <see cref="IEnumerable{T}"/> to be added to an arbitrary
		/// <see cref="ICollection{U}"/>.
		/// </summary>
		/// <typeparam name="T">The type of items in the incoming enumeration.</typeparam>
		/// <typeparam name="U">The type of the items in this collection.</typeparam>
		/// <param name="coll">The collection to be added to (this). </param>
		/// <param name="items">The new items to be added. This argument may be <see langword="null"/>,
		/// in which case nothing is added and no exception is thrown.  The usual admonishion
		/// about not changing the contents of an <see cref="IEnumerable"/> while it is
		/// being enumerated apply here.
		/// </param>
		/// <exceptions>
		/// Normal casting exceptions will occur if the elements are not castable to
		/// <typeparamref name="T"/>, thus this is not a type-safe method.
		/// </exceptions>
		public static void AddCastableItems<T, U>(this ICollection<T> coll, IEnumerable<U> items)
		{
			if (items != null)
				foreach (var item in items) {
					coll.Add((T)(object)item);
				}
		}
		/// <summary>
		/// This method allows an <see cref="IEnumerable{T}"/> to be added to an arbitrary
		/// <see cref="ICollection{U}"/>.
		/// </summary>
		/// <typeparam name="T">The type of items in the incoming enumeration.</typeparam>
		/// <typeparam name="U">The type of the items in this collection.</typeparam>
		/// <param name="coll">The collection to be added to (this). </param>
		/// <param name="items">The new items to be added. This argument may be <see langword="null"/>,
		/// in which case nothing is added and no exception is thrown.  The usual admonishion
		/// about not changing the contents of an <see cref="IEnumerable"/> while it is
		/// being enumerated apply here.
		/// </param>
		/// <exceptions>
		/// <exception>
		/// <see cref="InvalidOperationException"/> thrown if <typeparamref name="T"/>
		/// is not assignable from <typeparamref name="U"/>.
		/// "No KeyEqualityComparer found."
		/// </exception>
		/// </exceptions>
		public static void AddConvertableItems<T, U>(this ICollection<T> coll, IEnumerable<U> items)
		{
			var list = items.ConvertableEnumIntoList<U, T>();
			//
			if (list != null)
				foreach (var item in list) {
					coll.Add(item);
				}
		}
		/// <summary>
		/// This method allows an <see cref="IEnumerable{T}"/> to be added to an arbitrary
		/// <see cref="ICollection{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of items in the collection.</typeparam>
		/// <param name="coll">The collection to be added to (this). </param>
		/// <param name="items">The new items to be added. This argument may be <see langword="null"/>,
		/// in which case nothing is added and no exception is thrown.  The usual admonishion
		/// about not changing the contents of an <see cref="IEnumerable"/> while it is
		/// being enumerated apply here.
		/// </param>
		public static void AddItems<T>(this ICollection<T> coll, IEnumerable<T> items)
		{
			foreach (var item in items) {
				coll.Add(item);
			}
		}
		/// <summary>
		/// This method adds an item after first checking whether it is already there.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="collection">The incoming collection.</param>
		/// <param name="item">Item to potentially add.</param>
		public static void AddNoDupes<T>(this ICollection<T> collection, T item)
		{
			if (collection.Contains(item)) return;
			collection.Add(item);
		}
		/// <summary>
		/// This method adds items after first checking whether they are already there.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="collection">The incoming collection.</param>
		/// <param name="items">Items to potentially add or <see langword="null"/>.</param>
		public static void AddNoDupes<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			if (items == null) return;
			foreach (var item in items) {
				collection.AddNoDupes(item);
			}
		}
		/// <summary>
		/// This method adds an item after first checking whether it is <see langword="null"/>.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the items in the collection. No value types.
		/// </typeparam>
		/// <param name="collection">The incoming collection.</param>
		/// <param name="item">
		/// Item to potentially add if it is not <see langword="null"/>.
		/// </param>
		public static void AddNoNulls<T>(this ICollection<T> collection, T item) where T: class
		{
			if (item == null) return;
			collection.Add(item);
		}
		/// <summary>
		/// This method removes items if present in a collection.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="coll">The collection to be removed from (this).</param>
		/// <param name="items">Items to potentially remove. May be <see langword="null"/>,
		/// in which case nothing is removed and no exception is thrown.
		/// </param>
		public static void RemoveIfPresent<T>(this ICollection<T> coll, IEnumerable<T> items)
		{
			if (coll == null) return;
			if (items == null) return;
			foreach (var item in items.Where(coll.Contains))
			{
				coll.Remove(item);
			}
		}
		/// <summary>
		/// This method adds items after first checking whether each is already there.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="coll">The collection to be added to (this).</param>
		/// <param name="items">Items to potentially add. May be <see langword="null"/>,
		/// in which case nothing is added and no exception is thrown.
		/// </param>
		public static void AddRangeNoDupes<T>(this ICollection<T> coll, IEnumerable<T> items)
		{
			if (items == null) return;
			foreach (var item in items)
			{
				if (!coll.Contains(item)) coll.Add(item);
			}
		}
		/// <summary>
		/// This method creates an array from a collection.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="coll">The collection to be added to (this).</param>
		/// <returns>
		/// <see langword="null"/> for <see langword="null"/> <paramref name="coll"/>. Otherwise an
		/// array with the appropriate number of elements.
		/// </returns>
		/// <remarks>
		/// Named to avoid namespace conflicts with Linq.
		/// </remarks>
		public static T[] PAFToArray<T>(this ICollection<T> coll)
		{
			if (coll == null) return null;
			var array = new T[coll.Count];
			var count = 0;
			foreach (var item in coll)
			{
				if (count == coll.Count) return array;
				array[count] = item;
				count++;
			}
			return array;
		}
	}
}
