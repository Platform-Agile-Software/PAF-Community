// Exception shorthand.
using System;
using System.Collections.Generic;
using System.Linq;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;


namespace PlatformAgileFramework.Collections.Enumerators
{
	// ReSharper disable once CSharpWarnings::CS1584
	// ReSharper problem.
	/// <summary>
	/// This class contains extension methods for
	/// <see cref="IPAFNullableSynchronizedWrapper{IEnumerable{KeyValuePair{Tkey, TValue}}}"/>.
	/// </summary>
	/// <threadsafety>
	/// Implementations must be thread-safe. Methods contain provisons for safe
	/// enumerators to be included. Developers must determine the nature of
	/// these enumerators, if needed. Generally the service manager is the only
	/// entity that should be making changes to dictionary items and the
	/// dictionary should never have external access.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 06sep2011 </date>
	/// <description>
	/// <para>
	/// New.
	/// </para>
	/// <para>
	/// Needed a couple of extra methods, mostly for unsynchronized dictionaries,
	/// since synchronized stuff is mostly in Extended now.
	/// </para>
	/// </description>
	/// </contribution>
	/// </history>
	public static class LockedDictionaryExtensions
	{
		// ReSharper disable CSharpWarnings::CS1584
		/// <summary>
		/// This method applies a read lock to a
		/// <see cref="IPAFNullableSynchronizedWrapper{IEnumerable{KeyValuePair{TKey, TValue}}}"/>,
		/// then takes a snapshot of its contents with a copy operation. If
		/// <paramref name="enumerableFactory"/> is not <see langword="null"/>, an arbitrary
		/// copy operation is made. Otherwise an ordinary assignment is made.
		/// </summary>
		/// <param name="wrappedPairs">The wrapped, synchronized collection of pairs.</param>
		/// <param name="enumerableFactory">
		/// Pluggable thread-safe enumerator factory. <see langword="null"/> results
		/// in a reference or value copy.
		/// </param>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <returns>
		/// Never <see langword="null"/> - sometimes an empty list.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"wrappedPairs"</exception>
		/// </exceptions>
		// ReSharper restore CSharpWarnings::CS1584
		public static IList<KeyValuePair<TKey, TValue>> GetEnumeratedPairs<TKey, TValue>
			(this IPAFNullableSynchronizedWrapper<IEnumerable<KeyValuePair<TKey, TValue>>> wrappedPairs,
			IEnumerableFactory<KeyValuePair<TKey, TValue>> enumerableFactory = null)
		{
			if(wrappedPairs == null) throw new ArgumentNullException("wrappedPairs");
			var pairList = new List<KeyValuePair<TKey, TValue>>();

			using (var accessor = wrappedPairs.GetReadLockedObject())
			{
				// Safety valves.......
				if (accessor == null) return pairList;
				var pairs = accessor.ReadLockedNullableObject;
				if (pairs == null) return pairList;

				if (enumerableFactory != null) pairs = enumerableFactory.BuildEnumerable(pairs);
				pairList.AddRange(pairs);
			}
			return pairList;
		}

		// ReSharper disable CSharpWarnings::CS1584
		// ReSharper problem.
		/// <summary>
		/// This method applies a read lock to a
		/// <see cref="IPAFNullableSynchronizedWrapper{IEnumerable{KeyValuePair{TKey, TValue}}}"/>,
		/// then takes a snapshot of its contents with a copy operation. If
		/// <paramref name="enumerableFactory"/> is not <see langword="null"/>, an arbitrary
		/// copy operation is made. Otherwise an ordinary assignment is made.
		/// </summary>
		/// <param name="wrappedPairs">The wrapped, synchronized collection of pairs.</param>
		/// <param name="enumerableFactory">
		/// Pluggable thread-safe enumerator factory.
		/// </param>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <returns>
		/// List of <typeparamref name="TValue"/>s.
		/// Never <see langword="null"/> - sometimes an empty list.
		/// </returns>
		// ReSharper restore CSharpWarnings::CS1584
		public static IList<TKey> GetEnumeratedKeys<TKey, TValue>
			(this IPAFNullableSynchronizedWrapper<IEnumerable<KeyValuePair<TKey, TValue>>> wrappedPairs,
			IEnumerableFactory<KeyValuePair<TKey, TValue>> enumerableFactory = null)
		{
			// We must take a snapshot of the service dictionary here, since we are not using
			// a synchronized dictionary.
			var pairs = wrappedPairs.GetEnumeratedPairs(enumerableFactory);

			return pairs.Select(pair => pair.Key).ToList();
		}
		// ReSharper disable CSharpWarnings::CS1584
		// ReSharper problem.
		/// <summary>
		/// This method applies a read lock to a
		/// <see cref="IPAFNullableSynchronizedWrapper{IEnumerable{KeyValuePair{TKey, TValue}}}"/>,
		/// then takes a snapshot of its contents with a copy operation. If
		/// <paramref name="enumerableFactory"/> is not <see langword="null"/>, an arbitrary
		/// copy operation is made. Otherwise an ordinary assignment is made.
		/// </summary>
		/// <param name="wrappedPairs">The wrapped, synchronized collection of pairs.</param>
		/// <param name="enumerableFactory">
		/// Pluggable thread-safe enumerator factory.
		/// </param>
		/// <typeparam name="TKey">Type of the key.</typeparam>
		/// <typeparam name="TValue">Type of the value.</typeparam>
		/// <returns>
		/// List of <typeparamref name="TValue"/>s.
		/// Never <see langword="null"/> - sometimes an empty list.
		/// </returns>
		// ReSharper restore CSharpWarnings::CS1584
		public static IList<TValue> GetEnumeratedValues<TKey, TValue>
			(this IPAFNullableSynchronizedWrapper<IEnumerable<KeyValuePair<TKey, TValue>>> wrappedPairs,
			IEnumerableFactory<KeyValuePair<TKey, TValue>> enumerableFactory = null)
		{
			// We must take a snapshot of the service dictionary here, since we are not using
			// a synchronized dictionary.
			var pairs = wrappedPairs.GetEnumeratedPairs(enumerableFactory);
			var valueList = pairs.Select(pair => pair.Value).ToList();

			return valueList;
		}

		/// <summary>
		/// Just pulls pairs from dictionary, since the enumerable is implemented
		/// explicitly on the dictionary. Saves just a little typing.
		/// </summary>
		/// <typeparam name="TKey">See <see cref="IDictionary{TKey, TValue}"/></typeparam>
		/// <typeparam name="TValue">See <see cref="IDictionary{TKey, TValue}"/></typeparam>
		/// <param name="dictionary">Incoming dictionary.</param>
		/// <returns>Pairs in the dictionary.</returns>
		public static IEnumerable<KeyValuePair<TKey, TValue>> Pairs<TKey, TValue>
			(this IDictionary<TKey, TValue> dictionary)
		{
			IEnumerable<KeyValuePair<TKey, TValue>> enumerable = dictionary;
			return enumerable;
		}
	}
}
