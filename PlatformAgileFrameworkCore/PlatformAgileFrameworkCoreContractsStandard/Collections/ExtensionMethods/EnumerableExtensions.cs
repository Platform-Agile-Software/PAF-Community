//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.Collections.KeyedCollections;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

// ReSharper disable CheckNamespace
// Extension methods should be implicitly in the base collections namespace.
namespace PlatformAgileFramework.Collections
// ReSharper restore CheckNamespace
{
	/// <summary>
	/// This class contain some extension methods for the Generic and non-Generic
	/// IEnumerable interface. Makes it easier to convert to/from different types
	/// of collections, iterators, etc..
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 19jun2015 </date>
	/// <desription>
	/// <para>
	/// Had to go in to redo all the IList stuff because of Microsoft's
	/// wacko bullshit of not having List castable to IList anymore
	///  - I am pissed..............
	/// </para>
	/// <para>
	/// Fuckin' Linq stopped working right, too. Had to rewrite in foreach's.
	/// Nice job assholes! Some things I had to put in here should be in linq, but
	/// they don't work right.
	/// </para>
	/// </desription>
	/// </contribution>
	/// </history>
	public static class EnumerableExtensions
	{
        /// <summary>
        /// This method builds an <see cref="IList{T}"/> from an enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="enumerable">The enumeration to build the list from (this).</param>
        /// <returns>
        /// Empty collection for <see langword="null"/> <paramref name="enumerable"/>.
        /// Otherwise a loaded collection.
        /// </returns>
        public static IList<T> BuildIList<T>(this IEnumerable<T> enumerable)
        {
            var collection = new Collection<T>();
            if(enumerable != null) collection.AddItems(enumerable);
            return collection;
        }
        /// <summary>
        /// This method loads a <see cref="Collection{T}"/> from an enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="enumerable">The enumeration to build the collection from (this).</param>
        /// <returns>
        /// Empty collection for <see langword="null"/> <paramref name="enumerable"/>.
        /// Otherwise a loaded collection.
        /// </returns>
        public static Collection<T> BuildCollection<T>(this IEnumerable<T> enumerable)
        {
            var collection = new Collection<T>();
            if(enumerable != null) collection.AddItems(enumerable);
            return collection;
        }

		/// <summary>
		/// Divides an enumeration into a number of columns.
		/// </summary>
		/// <param name="enumeration">
		/// One of us (this).
		/// </param>
		/// <param name="numColumns">
		/// Number of columns to output, populated in the order first, second, third....
		/// </param>
		/// <returns>
		/// Array of lists of <see typeparamref="T"/>s. Returns empty columns if incoming
		/// is <see langword="null"/>.
		/// </returns>
		public static IList<T>[] ToArrayOfColumns<T>(this IEnumerable<T> enumeration, int numColumns)
		{
			var columns = new IList<T>[numColumns];
			for (var columnNum=0; columnNum < columns.Length; columnNum++)
			{
				columns[columnNum] = new List<T>();
			}

			if (enumeration == null)
				return columns;
			var enumerationArray = enumeration.ToArray();
			var enumerationIndex = -1;

			foreach (var item in enumerationArray)
			{
				enumerationIndex++;
				if (enumerationIndex == enumerationArray.Length)
					return columns;
				columns[enumerationIndex % (numColumns)].Add(item);
			}

			return columns;

		}

		/// <summary>
		/// This extension method combines two enumerations, possibly excluding
		/// duplicates.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerables.</typeparam>
		/// <param name="iEnumerable">
		/// One of us.
		/// </param>
		/// <param name="otherEnumerable">
		/// Another <see cref="IEnumerable{T}"/> to combine with. May be <see langword="null"/>.
		/// </param>
		/// <param name="excludeDuplicates"></param>
		/// <returns>The intersection of the two sets or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, a <see langword="null"/> is returned.
		/// If the enumerations have no common elements, a <see langword="null"/> is returned.
		/// </remarks>
		public static IList<T> Combine<T>(this IEnumerable<T> iEnumerable,
			IEnumerable<T> otherEnumerable, bool excludeDuplicates = false)
		{
			Collection<T> collection;
			if (iEnumerable == null) return null;
			if (otherEnumerable == null) return null;

			iEnumerable = iEnumerable.IntoArray();
			otherEnumerable = otherEnumerable.IntoArray();

			if ((collection = iEnumerable.IntoArray().BuildCollection()) == null)
			{
				collection = otherEnumerable.BuildCollection();
				return collection;
			}

			if (excludeDuplicates)
				collection.AddRangeNoDupes(otherEnumerable);
			else
				collection.AddItems(otherEnumerable);

			return collection;
		}

		/// <summary>
		/// This extension method determines whether a specified enumeration is
		/// contained within "this" one.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <param name="thisEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <param name="otherEnumerable"> </param>
		/// <returns>
		/// <see langword="true"/> if the <paramref name="otherEnumerable"/> is entirely contained
		/// within "this" enumeration.
		/// </returns>
		/// <remarks>
		/// If <paramref name="thisEnumerable"/> is <see langword="null"/>, <see langword="false"/>
		/// is returned. If <paramref name="thisEnumerable"/> returns no items, or is
		/// <see langword="null"/>, <see langword="true"/> is returned.
		/// </remarks>
		public static bool ContainsSet<T>(this IEnumerable<T> thisEnumerable,
			IEnumerable<T> otherEnumerable)
		{
			if (thisEnumerable == null) return false;
			if (otherEnumerable == null) return true;
			var collection = BuildCollection(thisEnumerable);
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in otherEnumerable)
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				if (!collection.Contains(item)) return false;
			}
			return true;
		}
		/// <summary>
		/// This extension method converts any <see cref="IEnumerable{T}"/> into a
		/// list of elements that are of a different type, with possibly fewer
		/// elelemnts. It checks each element to see if it convertible
		/// (<see cref="TypeHandling.TypeExtensionMethods.IsTypeAssignableFrom"/>) and converts it if it is.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <typeparam name="U">Generic type of the outgoing list.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The list or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, an empty array is returned.
		/// Noted that method requires the type <typeparamref name="T"/> to be
		/// convertable to the type <typeparamref name="U"/>.
		/// </remarks>
		public static IEnumerable<U> ConvertableEnumElementsIntoList<T, U>(this IEnumerable<T> iEnumerable)
			where T: class where U: class
		{
			// List in case we need it - lazy creation.
			ICollection<U> collection = null;
			if (iEnumerable == null) return null;
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in iEnumerable)
			{
				if (collection == null) collection = new Collection<U>();
				U outputElement;
				if ((outputElement = ((object)item) as U) == null) continue;
				collection.Add(outputElement);
			}
			return collection;
		}
		/// <summary>
		/// This extension method converts any <see cref="IEnumerable{T}"/> into a
		/// list of a compatible type.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <typeparam name="U">Generic type of the outgoing list.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The list or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, an empty array is returned.
		/// Noted that method requires the type <typeparamref name="T"/> to be
		/// convertable to the type <typeparamref name="U"/>.
		/// </remarks>
		/// <exceptions>
		/// <exception>
		/// <see cref="InvalidOperationException"/> thrown if <typeparamref name="U"/>
		/// is not assignable from <typeparamref name="T"/>.
		/// "Type not assignable."
		/// </exception>
		/// </exceptions>
		public static IEnumerable<U> ConvertableEnumIntoList<T, U>(this IEnumerable<T> iEnumerable)
		{
			if (!(typeof(U).IsTypeAssignableFrom(typeof(T))))
				throw new InvalidOperationException("Type: " + typeof(T) + " not assignable to : " + typeof(U));
			// List in case we need it - lazy creation.
			ICollection<U> collection = null;
			if (iEnumerable == null) return null;
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in iEnumerable)
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				if (collection == null) collection = new Collection<U>();
				collection.Add(((U)((object)item)));
			}
			return collection;
		}

		/// <summary>
		/// This extension method converts any <see cref="IEnumerable{T}"/> into a
		/// list of a compatible type. Type-safe version.
		/// </summary>
		/// <typeparam name="TSubtype">
		/// Generic Type of the enumerable. This must be a subtype of <typeparamref name="USupertype"/>
		/// </typeparam>
		/// <typeparam name="USupertype">Generic type of the outgoing list.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{TSubtype}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The list. Never <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, an empty array is returned.
		/// Better to use this rather than <c>ConvertableEnumIntoList</c> if inheritance
		/// relationship is known.
		/// </remarks>
		public static IList<USupertype> EnumIntoSupertypeList<USupertype, TSubtype>
	        (this IEnumerable<TSubtype> iEnumerable) where TSubtype : USupertype
	    {
	        if (iEnumerable == null) return null;

	        // Never return null.
	        Collection<USupertype> collection = new Collection<USupertype>();

	        foreach (var item in iEnumerable)
	        {
	            collection.Add(((item)));
	        }
	        return collection;
	    }
	    /// <summary>
	    /// This extension method converts any <see cref="IEnumerable{T}"/> into a
	    /// list of a contained subtypes.
	    /// </summary>
	    /// <typeparam name="TSupertype">
	    /// Generic Type of the incoming enumerable. This must be a supertype
	    /// of <typeparamref name="USubtype"/>
	    /// </typeparam>
	    /// <typeparam name="USubtype">Generic type of the outgoing list.</typeparam>
	    /// <param name="iEnumerable">
	    /// An object implementing <see cref="IEnumerable{TSuperType}"/>. May be <see langword="null"/>.
	    /// </param>
	    /// <returns>The list. Never <see langword="null"/>.</returns>
	    /// <remarks>
	    /// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
	    /// If <paramref name="iEnumerable"/> returns no items, an empty array is returned.
	    /// Better to use this rather than <c>ConvertableEnumIntoList</c> if inheritance
	    /// relationship is known.
	    /// </remarks>
	    public static IList<USubtype> EnumIntoSubtypeList<TSupertype, USubtype>
	        (this IEnumerable<TSupertype> iEnumerable) where USubtype : TSupertype
	    {
	        if (iEnumerable == null) return null;

	        // Never return null.
	        Collection<USubtype> collection = new Collection<USubtype>();

	        // ReSharper disable LoopCanBeConvertedToQuery
	        foreach (var item in iEnumerable)
	        {
	            if (item is USubtype subtype)
	            {
	                collection.Add(subtype);
	            }
	        }
	        return collection;
	    }
		/// <summary>
        /// This extension method determines whether any <see cref="IEnumerable{T}"/> 
        /// contains a certain element.
        /// </summary>
        /// <typeparam name="T">Generic Type of the enumerable.</typeparam>
        /// <param name="iEnumerable">
        /// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
        /// </param>
        /// <param name="element">
        /// An element to check for. May be <see langword="null"/>.
        /// </param>
        /// <returns>The array.</returns>
        /// <remarks>
        /// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="false"/> is returned.
        /// If <paramref name="iEnumerable"/> returns no items, <see langword="false"/> is returned.
        /// </remarks>
        public static bool ContainsElement<T>(this IEnumerable<T> iEnumerable, T element)
		{
			if (iEnumerable == null) return false;
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in iEnumerable)
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				if (element.Equals(item)) return true;
			}
			return false;
		}
		/// <summary>
		/// This extension method looks for <see cref="IPAFEnumKeyedObject"/>'s that
		/// "cover" a selected set of bit fields. The test is made with the
		/// <see cref="PAFEnumExtensions.Covers"/> method.
		/// </summary>
		/// <typeparam name="T">
		/// Generic Type of the enumerable. The type is constrained to wear
		/// <see cref="IPAFEnumKeyedObject"/>.
		/// </typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <param name="enumToCover">
		/// This is an <see cref="Enum"/> whose bits are anded with each element in
		/// the enumerable to see if their bit pattern "covers" the bits in
		/// <paramref name="enumToCover"/>.
		/// </param>
		/// <returns>The list or <see langword="null"/>.</returns>
		/// <exceptions>
		/// Exceptions will be thrown from the framework if the type of the <see cref="Enum"/>'s
		/// do not match.
		/// </exceptions>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, a <see langword="null"/>.
		/// </remarks>
		public static IList<T> GetCoveringElements<T>(this IEnumerable<T> iEnumerable,
			Enum enumToCover) where T : IPAFEnumKeyedObject
		{
			Collection<T> collection = null;
			if (iEnumerable == null) return null;
			foreach (var item in iEnumerable)
			{
				if (item.GetItemEnumKey().Covers(enumToCover))
				{
					if (collection == null) collection = new Collection<T>();
					collection.Add(item);
				}
			}
			return collection;
		}
		/// <summary>
		/// This extension method returns the last element in an enumeration
		/// of reference types.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable. Must be reference type</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The element or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// </remarks>
		public static T GetLastElement<T>(this IEnumerable<T> iEnumerable) where T : class
		{
			if (iEnumerable == null) return null;
			T foundItem = null;
			foreach (var item in iEnumerable)
			{
				foundItem = item;
			}
			return foundItem;
		}
		/// <summary>
		/// This extension method returns the first element in an enumeration
		/// of reference types.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable. Must be reference type</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The element or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// </remarks>
		public static T GetFirstElement<T>(this IEnumerable<T> iEnumerable) where T : class
		{
			if (iEnumerable == null) return null;
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in iEnumerable)
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				return item;
			}
			return null;
		}
		/// <summary>
		/// This extension method converts any <see cref="IEnumerable"/> into an
		/// array of <see cref="Object"/>'s.
		/// </summary>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The array.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, an empty array is returned.
		/// </remarks>
		public static object[] IntoArray(this IEnumerable iEnumerable)
		{
			// List in case we need it - lazy creation.
			List<object> list = null;
			if (iEnumerable == null) return null;
			foreach (var item in iEnumerable)
			{
				if (list == null) list = new List<object>();
				list.Add(item);
			}
			// Handle the edge case that is sometimes platform-specific.
			if ((list == null) || (list.Count == 0)) return new object[0];
			return list.ToArray();
		}
		/// <summary>
		/// This extension method converts any <see cref="IEnumerable{T}"/> into an
		/// array.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The array.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, an empty array is returned.
		/// </remarks>
		public static T[] IntoArray<T>(this IEnumerable<T> iEnumerable)
		{
			// List in case we need it - lazy creation.
			List<T> list = null;
			if (iEnumerable == null) return null;
			foreach (var item in iEnumerable)
			{
				if (list == null) list = new List<T>();
				list.Add(item);
			}
			// Handle the edge case that is sometimes platform-specific.
			if ((list == null) || (list.Count == 0)) return new T[0];
			return list.ToArray();
		}
		/// <summary>
		/// This extension method converts any <see cref="IEnumerable{T}"/> into a
		/// <see cref="ICollection{T}"/>. A <see cref="Collection{T}"/> is used
		/// as the backing container.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The collection or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, an empty collection is returned.
		/// </remarks>
		public static ICollection<T> IntoCollection<T>(this IEnumerable<T> iEnumerable)
		{
			var col = new Collection<T>();
			if (iEnumerable == null) return null;
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in iEnumerable)
			{
				// ReSharper restore LoopCanBeConvertedToQuery
				col.Add(item);
			}
			return col;
		}

		/// <summary>
		/// This method removes a set of elements from a collection, if
		/// any are present.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="enumerable">One of us.</param>
		/// <param name="elementsToRemove">Set of elements to remove.</param>
		/// <returns>
		/// Empty collection for for <see langword="null"/> <paramref name="enumerable"/>.
		/// </returns>
		public static IList<T> RemoveElementsIfPresent<T>(this IEnumerable<T> enumerable,
		IEnumerable<T> elementsToRemove)
		{
			var collection = new List<T>();
			if (enumerable == null) return collection;
			collection.RemoveIfPresent(elementsToRemove);
			return collection;
		}
		/// <summary>
		/// This method removes <see langword="null"/>'s from a collection.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the items in the collection. Must be a reference type.
		/// </typeparam>
		/// <param name="enumerable">One of us.</param>
		/// <returns>
		/// Empty collection for for <see langword="null"/> <paramref name="enumerable"/>.
		/// </returns>
		public static IList<T> RemoveNullElements<T>(this IEnumerable<T> enumerable)
			where T : class
		{
			var collection = new Collection<T>();
			if (enumerable == null) return collection;
			foreach (var t in enumerable)
			{
				if (t != null) collection.Add(t);
			}
			return collection;
		}
		/// <summary>
		/// This extension method builds a dictionary with the same elements and
		/// types in both the keys and values.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <param name="iEnumerable">
		/// One of us.
		/// </param>
		/// <returns>The dictionary or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, a <see langword="null"/> is returned.
		/// </remarks>
		public static IDictionary<T, T> ToOneDimensionalDictionary<T>(this IEnumerable<T> iEnumerable)
		{
			if (iEnumerable == null) return null;

			Dictionary<T, T> dict = null;

			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var item in iEnumerable)
			{
				if (dict == null) dict = new Dictionary<T, T>();
				// ReSharper restore LoopCanBeConvertedToQuery
				dict.Add(item, item);
			}
			return dict;
		}

		/// <summary>
		/// This extension method finds the intersection of two enumerables.
		/// Equality is based on the "Equals()" method for the Generic.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerables.</typeparam>
		/// <param name="iEnumerable">
		/// One of us.
		/// </param>
		/// <param name="otherEnumerable">
		/// Another <see cref="IEnumerable{T}"/> to intersect with. May be <see langword="null"/>.
		/// </param>
		/// <returns>The intersection of the two sets or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, a <see langword="null"/> is returned.
		/// If the enumerations have no common elements, a <see langword="null"/> is returned.
		/// </remarks>
		public static IEnumerable<T> Intersect<T>(this IEnumerable<T> iEnumerable, IEnumerable<T> otherEnumerable)
		{
			Collection<T> collection = null;
			if (iEnumerable == null) return null;
			if (otherEnumerable == null) return null;

			var dictionary = iEnumerable.ToOneDimensionalDictionary();

			if (dictionary == null) return null;

			foreach (var item in otherEnumerable)
			{
				if (dictionary.TryGetValue(item, out var foundItem))
				{
					if (collection == null) collection = new Collection<T>();
					collection.Add(foundItem);
				}
			}
			return collection;
		}
		/// <summary>
		/// This extension method converts any <see cref="IEnumerable{T}"/> into a
		/// <see cref="IList{T}"/>. A <see cref="Collection{T}"/> is used  as the
		/// backing container.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <returns>The list or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// If <paramref name="iEnumerable"/> returns no items, an empty list is returned.
		/// </remarks>
		public static IList<T> IntoList<T>(this IEnumerable<T> iEnumerable)
		{
			Collection<T> collection = null;
			if (iEnumerable == null) return null;
			foreach (var item in iEnumerable)
			{
				if (collection == null) collection = new Collection<T>();
				collection.Add(item);
			}
			return collection;
		}
		/// <summary>
		/// This extension method just tells if an enumerable reference
		/// is <see langword="null"/> or enumeration is empty.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if a vacuous collection is passed in.
		/// </returns>
		public static bool NullOrEmpty<T>(this IEnumerable<T> iEnumerable)
		{
			if (iEnumerable == null) return true;
			return !iEnumerable.Any();
		}
		/// <summary>
		/// This extension method returns the first element in an enumeration
		/// of generic types. TryGet style to accomodate value types.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <param name="found">
		/// The item, if found, or <c>default{T}</c>.
		/// </param>
		/// <returns>The element or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// </remarks>
		public static bool TryGetFirstElement<T>(this IEnumerable<T> iEnumerable, out T found)
		{
			found = default(T);
			if (iEnumerable == null) return false;
			foreach (var item in iEnumerable)
			{
				found = item;
				return true;
			}
			return false;
		}

		/// <summary>
		/// This extension method returns an element that is looked up by its
		/// name. TryGet style to accomodate value types.
		/// </summary>
		/// <typeparam name="T">Generic Type of the enumerable.</typeparam>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <param name="name">
		/// Name of the item. Case-sensitive ordinal match is required. <see langword="null"/>
		/// returns <see langword="false"/>.
		/// </param>
		/// <param name="found">
		/// The item, if found, or <c>default{T}</c>.
		/// </param>
		/// <returns>The element or <see langword="null"/>.</returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> is <see langword="null"/>, <see langword="null"/> is returned.
		/// </remarks>
		public static bool TryGetNamedElement<T>(this IEnumerable<T> iEnumerable, string name,
			out T found) where T : IPAFNamedObject
		{
			found = default(T);
			if (iEnumerable == null) return false;
			if (name == null) return false;
			foreach (var item in iEnumerable)
			{
				if (string.Compare(name, item.ObjectName, StringComparison.Ordinal) != 0) continue;
				found = item;
				return true;
			}
			return false;
		}

		/// <summary>
		/// This extension method checks to see if a given string is in our enumeration.
		/// </summary>
		/// <param name="iEnumerable">
		/// An object implementing <see cref="IEnumerable{T}"/>. May be <see langword="null"/>.
		/// </param>
		/// <param name="str">
		/// String that is to be compared to the enumeration of strings in a case-sensitive
		/// manner. Ordinal comparison.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the string matches a .
		/// </returns>
		/// <remarks>
		/// If <paramref name="iEnumerable"/> or <paramref name="str"/> is <see langword="null"/>,
		/// <see langword="false"/> is returned.
		/// </remarks>
		public static bool StringIsPresent(this IEnumerable<string> iEnumerable, string str)
		{
			if ((iEnumerable == null) || (str == null)) return false;
			foreach (var item in iEnumerable)
			{
				if (string.Compare(str, item, StringComparison.Ordinal) != 0) continue;
				return true;
			}
			return false;
		}
	}
}
