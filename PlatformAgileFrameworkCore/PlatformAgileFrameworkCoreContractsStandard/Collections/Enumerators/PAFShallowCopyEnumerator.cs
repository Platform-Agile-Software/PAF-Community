using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// This is an enumerator that makes an assignment copy of an enumerable before
	/// enumerating over it. It is designed to provide a thread-safe way to enumerate over a
	/// collection. Noted that the enumerator itself is never thread-safe. Different
	/// threads must not call the enumerator methods on any instance of this enumerator.
	/// This enumerator makes only a shallow copy of a collection to enumerate over.
	/// In order to use this enumerator correctly, lock the underlying collection
	/// before creating and returning this class and unlock it after.
	/// </summary>
	/// <typeparam name="T">
	/// This is the generic type of items on the enumerable.
	/// </typeparam>
	/// <threadsafety>
	/// This class is NOT thread-safe - only one thread should be accessing it.
	/// </threadsafety>
	public class ShallowCopyEnumerator<T> : PAFEnumeratorWrapperBase<T>
	{
		#region Constructors
		/// <summary>
		/// Builds a copy of the incoming enumeration in a list.
		/// </summary>
		/// <param name="enumerable">
		/// The incoming enumeration. <see langword="null"/> builds an empty list.
		/// </param>
		public ShallowCopyEnumerator(IEnumerable<T> enumerable)
			:base(enumerable, true)
		{}
		#endregion // Constructors
	}
}