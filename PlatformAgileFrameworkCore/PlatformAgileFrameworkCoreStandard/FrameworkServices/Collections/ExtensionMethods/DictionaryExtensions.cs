//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PlatformAgileFramework.Collections.ExtensionMethods
{
	/// <summary>
	/// This class contain some extension methods for the <see cref="IDictionary{T,U}"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 13dec2018 </date>
	/// <description>
	/// Finally added DOCs to this file. Also added a couple more methods so we can
	/// use dictionaries instead of some of the newer collection types, since there
	/// are problems with some of these on some platforms.
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable InconsistentNaming
	public static class DictionaryExtensions
// ReSharper restore InconsistentNaming
	{
		/// <summary>
		/// This method allows dictionary items to be added to an existing dictionary
		/// only if they are not present. Key is same as value.
		/// </summary>
		/// <typeparam name="TKey">The type of item to be added.
		/// </typeparam>
		/// <param name="dict">
		/// The dictionary to be added to (this).
		/// May be <see langword="null"/> - just returns.
		/// </param>
		/// <param name="newEntry">The new entry to be added. This argument may be
		/// already in the dictionary, in which case <see langword="false"/> is returned.
		/// </param>
		/// <threadsafety>
		/// Unsafe. Lock the dictionary if thread safety is needed.
		/// </threadsafety>
		/// <returns>
		/// <see langword="true"/>if entry was added.
		/// </returns>
		public static bool AddUniqueAsThoughCollection<TKey>(this IDictionary<TKey, TKey> dict,
			TKey newEntry)
		{
			if (dict == null) return false;
			if (dict.TryGetValue(newEntry, out var outValue)) return false;
			dict.Add(newEntry, newEntry);
			return true;
		}

		/// <summary>
		/// This method allows dictionary items to be added to an existing dictionary
		/// only if their value is also unique. This allows a dictionary to be used as
		/// a sort of associative array with unique mapping.
		/// </summary>
		/// <typeparam name="TKey">The type of items in the incoming enumeration.</typeparam>
		/// <typeparam name="UValue">The type of the items in this collection.</typeparam>
		/// <param name="dict">
		/// The dictionary to be added to (this).
		/// May be <see langword="null"/> - just returns.
		/// </param>
		/// <param name="newEntries">The new entries to be added. This argument may be
		/// <see langword="null"/>, in which case nothing is added and no exception is thrown.
		/// </param>
		/// <threadsafety>
		/// Unsafe. Lock the dictionary if thread safety is needed.
		/// </threadsafety>
		/// <exceptions>
		/// <exception cref="ArgumentException">
		/// "Duplicate value" if any of the values in the incoming enumeration are
		/// duplicates of what is already in the dictionary or of themselves.
		/// </exception>
		/// Exceptions are thrown by the dictionary if an attempt is made to install
		/// entries with duplicate keys.
		/// </exceptions>
		public static void AddUniqueValueEntries<TKey, UValue>(this IDictionary<TKey, UValue> dict,
			IEnumerable<KeyValuePair<TKey, UValue>> newEntries)
		{
			if ((newEntries == null) || (dict == null)) return;
			newEntries = newEntries.IntoArray();

			var values = dict.Values.IntoList();
			foreach (var entry in newEntries)
			{
				if (values.Contains(entry.Value))
					throw new ArgumentException("Duplicate value: " + entry.Value);
				dict.Add(entry);
			}
		}
		/// <summary>
		/// This method returns a dictionary if the incoming key/value collection have
		/// unique keys.
		/// </summary>
		/// <typeparam name="TKey">The type of keys in the incoming enumeration.</typeparam>
		/// <typeparam name="UValue">The type of the value in the incoming enumeration.</typeparam>
		/// <param name="pairs">
		/// Incoming collection. (this).
		/// May be <see langword="null"/> - just returns empty dictionary.
		/// </param>
		/// <returns>
		/// <see langword="null"/> for non-unique keys.
		/// </returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public static Dictionary<TKey, UValue> BuildDictionaryIfKeysUnique<TKey, UValue>
			(this IEnumerable<KeyValuePair<TKey, UValue>> pairs)
		{
			var dict = new Dictionary<TKey, UValue>();

			if (pairs == null) return dict;

			foreach (var pair in pairs)
			{
				if (dict.ContainsKey(pair.Key))
					return null;
				dict.Add(pair.Key, pair.Value);
			}
			return dict;
		}

		/// <summary>
		/// This method returns a <see langword="null"/> if a reference type
		/// is not found.
		/// </summary>
		/// <typeparam name="TKey">
		/// The type of keys in the incoming dictionary.
		/// </typeparam>
		/// <typeparam name="UValue">
		/// The type of the value in the dictionary, which must be a reference type.
		/// </typeparam>
		/// <param name="dict">
		/// The incoming dictionary (this).
		/// </param>
		/// <param name="key">
		/// The key.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if not found.
		/// </returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public static UValue GetTypeOrNull<TKey, UValue>
			(this IDictionary<TKey, UValue> dict, TKey key ) where UValue: class
		{
			UValue value;
			dict.TryGetValue(key, out value);
			return value;
		}
		/// <summary>
		/// Adds (if not present) or sets (if present) a dictionary item.
		/// </summary>
		/// <typeparam name="TKey">Type of Key</typeparam>
		/// <typeparam name="TValue">Type of Value</typeparam>
		/// <param name="self">One of us.</param>
		/// <param name="key">Key</param>
		/// <param name="val">Value</param>
		public static void AddOrSet<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> self, TKey key, TValue val)
		{
			self.AddOrUpdate(key, val, (t1, t2) => val);
		}

		/// <summary>
		/// Safely adds an item to a list looked up by a key. A <see cref="List{T}"/> is created
		/// for each list value if not already present. Then <paramref name="item"/> is added.
		/// </summary>
		/// <typeparam name="TKey">Type of Key.</typeparam>
		/// <typeparam name="TValue">Type of element in list.</typeparam>
		/// <param name="dictionary">One of us. <see langword = "null"/> is OK - method does nothing.</param>
		/// <param name="key">Key for accessing the item.</param>
		/// <param name="item">The item.</param>
		public static void AddToListValue<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, TKey key, TValue item)
		{
			if (dictionary == null)
				return;
			if (!dictionary.ContainsKey(key))
				dictionary[key] = new List<TValue>();
			dictionary[key].Add(item);
		}
		/// <summary>
		/// Provides a sorted list of <see cref="KeyValuePair{TKey,TValue}"/> values by ascending
		/// <typeparamref name="TKey"/>. Least in value is at index 0.
		/// </summary>
		/// <typeparam name="TKey">Type of Key. Must implement <see cref="IComparable"/></typeparam>
		/// <typeparam name="TValue">Type of element in list.</typeparam>
		/// <param name="dictionary">One of us. <see langword = "null"/> is OK - method returns <see langword="null"/>.</param>
		/// <returns>
		/// <see langword = "null"/> if input dictionary is <see langword = "null"/>.
		/// </returns>
		/// <remarks>
		/// Uses old-fashioned array sort, since it works everywhere.
		/// </remarks>
		public static IReadOnlyList<KeyValuePair<TKey, TValue>> SortEntriesByAscendingKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
			where TKey : IComparable
		{
			if (dictionary == null)
				return null;

			var keyArray = dictionary.Keys.ToArray();
			var entryArray = dictionary.ToArray();

			Array.Sort(keyArray, entryArray);

			return entryArray;
		}
		/// <summary>
		/// Provides a sorted list of <see cref="KeyValuePair{TKey,TValue}"/> values by ascending
		/// <typeparamref name="TKey"/>. Highest in value is at index 0.
		/// </summary>
		/// <typeparam name="TKey">Type of Key. Must implement <see cref="IComparable"/></typeparam>
		/// <typeparam name="TValue">Type of element in list.</typeparam>
		/// <param name="dictionary">One of us. <see langword = "null"/> is OK - method returns <see langword="null"/>.</param>
		/// <returns>
		/// <see langword = "null"/> if input dictionary is <see langword = "null"/>.
		/// </returns>
		/// <remarks>
		/// Uses old-fashioned array sort, since it works everywhere.
		/// </remarks>
		public static IReadOnlyList<KeyValuePair<TKey, TValue>> SortEntriesByDescendingKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
			where TKey : IComparable
		{
			if (dictionary == null)
				return null;

			var entryArray = dictionary.SortEntriesByAscendingKey();

			entryArray =  entryArray.Reverse().ToArray();

			return entryArray;
		}
		/// <summary>
		/// Provides a sorted list of <typeparamref name="TValue"/> values by ascending
		/// <typeparamref name="TKey"/>. Least in value is at index 0.
		/// </summary>
		/// <typeparam name="TKey">Type of Key. Must implement <see cref="IComparable"/></typeparam>
		/// <typeparam name="TValue">Type of element in list.</typeparam>
		/// <param name="dictionary">One of us. <see langword = "null"/> is OK - method returns <see langword="null"/>.</param>
		/// <returns>
		/// <see langword = "null"/> if input dictionary is <see langword = "null"/>.
		/// </returns>
		/// <remarks>
		/// Uses old-fashioned array sort, since it works everywhere.
		/// </remarks>
		public static IReadOnlyList<TValue> SortValuesByAscendingKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
			where TKey : IComparable
		{
			if (dictionary == null)
				return null;

			var keyArray = dictionary.Keys.ToArray();
			var valueArray = dictionary.Values.ToArray();

			Array.Sort(keyArray, valueArray);

			return valueArray;
		}
		/// <summary>
		/// Provides a sorted list of <typeparamref name="TValue"/> values by descending
		/// <typeparamref name="TKey"/>. Highest in value is at index 0.
		/// </summary>
		/// <typeparam name="TKey">Type of Key. Must implement <see cref="IComparable"/></typeparam>
		/// <typeparam name="TValue">Type of element in list.</typeparam>
		/// <param name="dictionary">One of us. <see langword = "null"/> is OK - method returns <see langword="null"/>.</param>
		/// <returns>
		/// <see langword = "null"/> if input dictionary is <see langword = "null"/>.
		/// </returns>
		/// <remarks>
		/// Uses old-fashioned array sort, since it works everywhere.
		/// </remarks>
		public static IReadOnlyList<TValue> SortValuesByDescendingKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
			where TKey : IComparable
		{
			if (dictionary == null)
				return null;

			var valueArray = dictionary.SortValuesByAscendingKey();
			valueArray = valueArray.Reverse().ToArray();

			return valueArray;
		}
	}
}
