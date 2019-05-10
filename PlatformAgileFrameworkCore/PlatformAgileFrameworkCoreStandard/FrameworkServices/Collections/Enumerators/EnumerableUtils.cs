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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// Utils for helping with enumerables.
	/// </summary>
	public static class EnumerableUtils
	{
		#region Methods
		/// <summary>
		/// Little helper that checks whether an incoming enumeration is
		/// definately finite. Checks to see if <see cref="ICollection{T}"/>,
		/// or if a <see cref="IPAFResettableEnumerable{T}"/> with finite length.
		/// </summary>
		/// <param name="enumerable">
		/// Incoming enumerable to check.
		/// </param>
		/// <returns>
		/// Results of the analysis. Returns <see langword="true"/> if argument is
		/// <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// This method is important when we want to copy an enumeration
		/// into an array.
		/// </remarks>
		public static bool CheckEnumerationIsFinite<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null) return true;
			if (enumerable is ICollection<T>) return true;
			var resettable = enumerable as IPAFResettableEnumerable<T>;
		    if (resettable?.IsEnumerationFinite == null) return false;
			if (resettable.IsEnumerationFinite.Value == false) return false;
			return true;
		}

		/// <summary>
		/// Method picks out <typeparamref name="U"/>'s from the incoming
		/// set.
		/// </summary>
		/// <typeparam name="U">
		/// Type we are looking for.
		/// </typeparam>
		/// <param name="enumerable">
		/// Incoming set. <see langword="null"/> returns an empty set.
		/// </param>
		/// <returns>
		/// Never <see langword="null"/>.
		/// </returns>
		public static IEnumerable<U> FilterEnumerable<U>(this IEnumerable enumerable)
		{
			var col = new Collection<U>();
			if (enumerable != null)
			{
				foreach (var item in enumerable)
				{
					if ((item != null) && (item is U))
						col.Add((U) item);
				}
			}
			return col;
		}

		/// <summary>
		/// <para>
		/// Reloads or creates a collection in a FINITE enumerable container. This
		/// method is needed to ensure that an enumerable is loaded with a finite
		/// collection of items. Some resettable enumerators are finite by nature
		/// (if they must be sorted, for example) and they must be reloaded with
		/// a finite set of elements. The purpose of this method is to ensure
		/// that finite resettable enumerators are reset with finite data.
		/// It does this by examining the incoming enumeration and determining
		/// if it is either an <see cref="ICollection{T}"/> or an
		/// <see cref="IPAFResettableEnumerable{T}"/> with finite content.
		/// If the input is not finite, an exception is thrown.
		/// </para>
		/// </summary>
		/// <typeparam name="T">
		/// Type we are dealing with.
		/// </typeparam>
		/// <param name="incomingEnumerable">
		/// Incoming enumerable. <see langword="null"/> returns <see langword="null"/> with no action. if this is
		/// not either collection or a finite resettable an exception is thrown.
		/// </param>
		/// <param name="targetEnumerable">
		/// Enumerable to be reset. If it is a <see cref="ICollection{T}"/> it is
		/// emptied and reloaded. If it is a <see cref="IPAFResettableEnumerable{T}"/>
		/// the <paramref name="incomingEnumerable"/> is simply passed to it. If this
		/// parameter is <see langword="null"/>, a new collection is instantiated and filled
		/// with the incoming <paramref name="incomingEnumerable"/>.
		/// </param>
		/// <returns>
		/// Never <see langword="null"/>.
		/// </returns>
		/// <threadsafety>
		/// This method is not synchronized. It assumes that neither
		/// <paramref name="incomingEnumerable"/> and <paramref name="targetEnumerable"/>
		/// are not accessed during the operation of this helper method. This
		/// method is often used for locked update of a Type's internal
		/// data during a reset operation, in which circumstance, the class
		/// internals are locked.
		/// </threadsafety>
		/// <exceptions>
		/// <exception>
		/// <see cref="ArgumentException"/> is thrown if the incoming
		/// <paramref name="incomingEnumerable"/> is not finite.
		/// "incomingEnumerable must be finite".
		/// </exception>
		/// </exceptions>
		public static IEnumerable<T> ReloadFiniteEnumerable<T>(
			this IEnumerable<T> incomingEnumerable, IEnumerable<T> targetEnumerable)
		{
			if (incomingEnumerable == null) return null;
// ReSharper disable PossibleMultipleEnumeration
			if(!CheckEnumerationIsFinite(incomingEnumerable))
// ReSharper restore PossibleMultipleEnumeration
				throw new ArgumentException("incomingEnumerable must be finite");

			// Reload if we are a collection.
			var col = targetEnumerable as ICollection<T>;
			if (col != null)
			{
				col.Clear();
// ReSharper disable PossibleMultipleEnumeration
				foreach (var t in incomingEnumerable)
// ReSharper restore PossibleMultipleEnumeration
				{
					col.Add(t);
				}
				return col;
			}

			// If the target enumerable is resettable, just reset it.
			var resettableEnumerable = targetEnumerable as IPAFResettableEnumerable<T>;
			if (resettableEnumerable != null)
			{
// ReSharper disable PossibleMultipleEnumeration
				resettableEnumerable.SetData(incomingEnumerable);
// ReSharper restore PossibleMultipleEnumeration
				return resettableEnumerable;
			}

			// We must be null;
			col = new Collection<T>();
// ReSharper disable PossibleMultipleEnumeration
			foreach (var t in incomingEnumerable) {
// ReSharper restore PossibleMultipleEnumeration
				col.Add(t);
			}
			return col;
		}
		#endregion // Methods
	}
}