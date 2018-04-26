using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Dictionaries
{
	/// <summary>
	///	Extends dictionaries with helpers.
	/// </summary>
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
		public static void Add<T,U>(this IDictionary<T,U> dictionary,
			T key, U value)
		{
			dictionary.Add(new KeyValuePair<T, U>(key, value));
		}
		#endregion Extension Methods
	}
}