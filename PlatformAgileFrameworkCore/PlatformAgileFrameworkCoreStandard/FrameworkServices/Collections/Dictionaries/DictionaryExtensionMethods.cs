using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Dictionaries
{
	/// <summary>
	///	Extends dictionaries with helpers.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31dec2017 </date>
	/// <description>
	/// New - actually reintegrated into .Net standard PAF core.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe.
	/// </threadsafety>
	public static class DictionaryExtensionMethods
	{
		#region Extension Methods
		/// <summary>
		/// Adds a key and a value as a key/value pair to the dictionary.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the key used to access values in the dictionary.
		/// </typeparam>
		/// <typeparam name="U">
		/// The type of the value contained in the dictionary.
		/// </typeparam>
		/// <param name="dictionary"> "this" </param>
		/// <param name="key">
		/// The key associated with the entry.
		/// </param>
		/// <param name="value">
		/// The value located by the key.
		/// </param>
		/// <remarks>
		/// Same rules apply as adding to a dictionary - no exceptions caught.
		/// </remarks>
		public static void Add<T, U>(this IDictionary<T, U> dictionary,
			T key, U value)
		{
			dictionary.Add(new KeyValuePair<T, U>(key, value));
		}
		/// <summary>
		/// Ensures that a certain entry exists in the dictionary. Note that
		/// the existence of the key only is checked for. If the key exists,
		/// nothing is done.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the key used to access values in the dictionary.
		/// </typeparam>
		/// <typeparam name="U">
		/// The type of the value contained in the dictionary.
		/// </typeparam>
		/// <param name="dictionary"> "this" </param>
		/// <param name="key">
		/// The key associated with the desired entry.
		/// </param>
		/// <param name="value">
		/// The value located by the key. This is installed ONLY if the corresponding
		/// key is not found.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if the key did exist. In this case, the dictionary is
		/// not touched.
		/// </returns>
		public static bool EnsureEntry<T, U>(this IDictionary<T, U> dictionary,
			T key, U value)
		{
			if (dictionary.ContainsKey(key))
				return false;
			dictionary.Add(new KeyValuePair<T, U>(key, value));
			return true;
		}
		/// <summary>
		/// Ensures that a certain entry exists in the dictionary. If it
		/// is not found it is created. If it is found, it is overwritten.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the key used to access values in the dictionary.
		/// </typeparam>
		/// <typeparam name="U">
		/// The type of the value contained in the dictionary.
		/// </typeparam>
		/// <param name="dictionary"> "this" </param>
		/// <param name="key">
		/// The key associated with the desired entry.
		/// </param>
		/// <param name="value">
		/// The value located by the key.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the key did exist. In this case, the dictionary is
		/// updated with the value. Also returns the current value in <paramref name="value"/>.
		/// </returns>
		public static bool EnsureEntryOrReplace<T, U>(this IDictionary<T, U> dictionary,
			T key, ref U value)
		{
			if (dictionary.ContainsKey(key))
			{
				var localValue = dictionary[key];
				dictionary[key] = value;
				value = localValue;
				return true;
			}
			dictionary.Add(new KeyValuePair<T, U>(key, value));
			return true;
		}
		#endregion Extension Methods
	}
}