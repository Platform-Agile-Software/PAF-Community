using System.Collections;
using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// Interface allows collections to have access to "plugable" enumerator
	/// generators that they can load. Implementing classes typically have
	/// two constructors accepting appropriate arguments.
	/// </summary>
	/// <typeparam name="T">Generic type in the collection.</typeparam>
	/// <remarks>
	/// One reason for this factory is that we don't trust anybody's
	/// yield implementation for thread-safety, let alone the consistency
	/// issue across platforms.
	/// </remarks>
	public interface IEnumerableFactory<T>
	{
		/// <summary>
		/// Builds a Generic enumerable from a Generic enumerable.
		/// </summary>
		/// <param name="enumerable">The incoming enumerable.</param>
		/// <returns>The manufactured enumerable.</returns>
		IEnumerable<T> BuildEnumerable(IEnumerable<T> enumerable);
		/// <summary>
		/// Builds a Generic enumerable from an enumerable.
		/// </summary>
		/// <param name="enumerable">The incoming enumerable.</param>
		/// <returns>The manufactured enumerable.</returns>
		IEnumerable<T> BuildEnumerable(IEnumerable enumerable);
	}
}