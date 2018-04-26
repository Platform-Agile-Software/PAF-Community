using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// Interface allows collections to have access to "plugable" enumerator
	/// generators that they can load. Every class implementing must have a constructor
	/// accepting a single argument of type <see cref="IEnumerable&lt;T&gt;"/>.
	/// </summary>
	/// <typeparam name="T">Generic type in the collection.</typeparam>
	public interface IEnumeratorFactory<T>
	{
		/// <summary>
		/// Builds an enumerator from an enumerable.
		/// </summary>
		/// <param name="enumerator">The incoming enumerator.</param>
		/// <returns>The manufactured enumerator.</returns>
		IEnumerator<T> BuildEnumerator(IEnumerable<T> enumerator);
	}
}