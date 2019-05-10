using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// This is an enumerator that makes a copy of an enumerable before enumerating
	/// over it. It is designed to provide a thread-safe way to enumerate over a
	/// collection. Noted that the enumerator itself is never thread-safe. Different
	/// threads must not call the enumerator methods on any instance of this enumerator.
	/// </summary>
	/// <typeparam name="T">
	/// This is the generic type of items on the enumerable. The type is required
	/// to implement <see cref="IPAFGenericDeepCloneable{T}"/> in order to make deep copies
	/// of the types before creating the output enumerable. This insures that indiviaual
	/// items are thread-safe, in addition to whatever structure the enumerable is
	/// pulling the data from being thread-safe. Noted that the cloning implementation
	/// on the items must make a deep copy for this enumerator to accomplish this
	/// goal. In order to use this enumerator correctly, lock the underlying collection
	/// before creating and returning this class and unlock it after.
	/// </typeparam>
	/// <threadsafety>
	/// This class is NOT thread-safe - only one thread should be accessing it.
	/// </threadsafety>
	public class DeepCloneEnumerable<T> : PAFEnumeratorWrapperBase<T> where T : IPAFGenericDeepCloneable<T>
	{
		#region Constructors
		/// <summary>
		/// Builds a copy of the incoming enumeration by casting elements to
		/// <see cref="IPAFGenericDeepCloneable{T}"/>.
		/// </summary>
		/// <param name="enumerable">
		/// The incoming enumeration. <see langword="null"/> yields an empty enumerator.
		/// </param>
		public DeepCloneEnumerable(IEnumerable<T> enumerable)
			:base(enumerable, true){}

		#endregion // Constructors
	}
}