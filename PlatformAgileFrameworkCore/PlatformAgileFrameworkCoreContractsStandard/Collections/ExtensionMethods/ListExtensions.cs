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

namespace PlatformAgileFramework.Collections.ExtensionMethods
{
	/// <summary>
	/// This class contain some extension methods for the <see cref="IList{T}"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 19JAN2018 </date>
	/// <description>
	/// Updated to include finding the element number in a list, which
	/// LInQ libs don't seem to have.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 10jul2015 </date>
	/// <description>
	/// New.
	/// Had to be built because of some missing constructors for the collection classes
	/// in PCL libs for Windows Store apps.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable InconsistentNaming
	public static class ListExtensions
// ReSharper restore InconsistentNaming
	{
		/// <summary>
		/// This method allows an element to be added to a <see cref="IList{T}"/> with
		/// a certain ordering imposed. If an item with the same compared value is attempted
		/// to be added, <see langword="false"/> is returned.
		/// </summary>
		/// <typeparam name="T">The type of items in the incoming list.</typeparam>
		/// <param name="list">
		/// The list to be added to (this). The list must already be in sorted order.
		/// </param>
		/// <param name="itemToAdd"> The new item to be added. This argument may be
		/// <see langword="null"/>, depending on the comparer.
		/// </param>
		/// <param name="comparer">
		/// Comparer that is used to determine the ordering of the list.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the addition to the list was successful.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"list"</exception>
		/// </exceptions>
		/// <threadsafety>
		/// Not thread-safe if multiple access is allowed on <paramref name="list"/> by
		/// caller.
		/// </threadsafety>
		/// <remarks>
		/// This method was necessary due to a problem with PCLs that don't let us
		/// construct collections with comparators.
		/// </remarks>
		public static bool AddItemInOrder<T>(this IList<T> list,
			T itemToAdd, IComparer<T> comparer)
		{
			if (list == null) throw new ArgumentNullException("list");

			// This just a linear search for now. We'll replace it with
			// something else when compiler/linker/whatever gets fixed.
			for (var index = 0; index <= list.Count - 1; index++)
			{
				if (comparer.Compare(list[index], itemToAdd) == 0)
				{
					return false;
				}

				// if new item is less than list element, insert new element right after.
				if (comparer.Compare(itemToAdd, list[index]) < 0)
				{
					list.Insert(index, itemToAdd);
					return true;
				}
			}
			// Greater than all elements
			list.Add(itemToAdd);
			return true;
		}
		/// <summary>
		/// This method allows an element to be found in a <see cref="IList{T}"/> ,
		/// if present.
		/// </summary>
		/// <typeparam name="T">
		/// The type of items in the incoming list. Constrained to be a reference type.
		/// </typeparam>
		/// <param name="list">
		/// The list to be searched (this).
		/// </param>
		/// <param name="itemToLocate"> The item to be searched for. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// -1 if the element was not found in the list.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"list"</exception>
		/// <exception cref="ArgumentNullException">"itemToLocate"</exception>
		/// </exceptions>
		/// <threadsafety>
		/// Not thread-safe if multiple access is allowed on <paramref name="list"/> by
		/// caller.
		/// </threadsafety>
		public static int ItemIndexInList<T>(this IList<T> list,
			T itemToLocate) where T: class
		{
			if (list == null) throw new ArgumentNullException("list");
			if (list == null) throw new ArgumentNullException("itemToLocate");

			for (var index = 0; index <= list.Count - 1; index++)
			{
				if (list[index] == itemToLocate)
				{
					return index;
				}
			}
			return -1;

		}
	}
}
