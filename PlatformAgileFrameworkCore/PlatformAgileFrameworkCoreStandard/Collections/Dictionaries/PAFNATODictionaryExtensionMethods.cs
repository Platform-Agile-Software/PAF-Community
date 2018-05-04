using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.Collections.Dictionaries
{
	/// <summary>
	///	Extends NATO dictionaries with a few helpers.
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	public static partial class PAFNATODictionaryExtensionMethods
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Extension Methods
		/// <summary>
		/// Gathers all entries of a particular type <paramref name="objectType"/>.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the object contained in the dictionary.
		/// </typeparam>
		/// <param name="thisDictionary"> "this" </param>
		/// <param name="objectType">
		/// The type of object to look for.
		/// </param>
		/// <param name="exactTypeMatch">
		/// Tells whether a subtype is allowed(<see langword="false"/>).
		/// </param>
		/// <returns>
		/// An enumeration of objects of the required type or <see langword="null"/>.
		/// These <typeparamref name="T"/>'s can safely be cast to the
		/// desired type.
		/// </returns>
		public static T GetTypedObject<T>(this IPAFNamedAndTypedObjectDictionary<T> thisDictionary,
			Type objectType, bool exactTypeMatch) where T: class
		{
			var enumeration = GetTypedObjects(thisDictionary, objectType, exactTypeMatch);
		    return enumeration?.GetFirstElement();
		}
		/// <summary>
		/// Gathers all entries of a particular type <paramref name="objectType"/>.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the object contained in the dictionary.
		/// </typeparam>
		/// <param name="thisDictionary"> "this" </param>
		/// <param name="objectType">
		/// The type of object to look for.
		/// </param>
		/// <param name="exactTypeMatch">
		/// Tells whether a subtype is allowed(<see langword="false"/>).
		/// </param>
		/// <returns>
		/// An enumeration of objects of the required type or <see langword="null"/>.
		/// These <typeparamref name="T"/>'s can safely be cast to the
		/// desired type.
		/// </returns>
		public static IEnumerable<T> GetTypedObjects<T>(this IPAFNamedAndTypedObjectDictionary<T> thisDictionary,
			Type objectType, bool exactTypeMatch)
		{
			if (thisDictionary == null) return null;
			if (thisDictionary.Values.Count == 0) return null;
			var col = thisDictionary.Values.IntoCollection();
			var outCol = new Collection<T>();

			//
			foreach (var t in col) {
				if ((exactTypeMatch) && (typeof(T) == objectType)) outCol.Add(t);
				else if (objectType.IsTypeAssignableFrom(typeof(T))) outCol.Add(t);
			}
			if (outCol.Count == 0) return null;
			return outCol;
		}
		#endregion Extension Methods
	}
}