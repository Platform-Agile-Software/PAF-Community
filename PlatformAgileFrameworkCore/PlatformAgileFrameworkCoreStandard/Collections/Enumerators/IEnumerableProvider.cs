using System;
using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// Interface for a type to act as a provider of an <see cref="IEnumerable{T}"/>
	/// for a collection of items. The enumeration is arbitrary and can be infinite.
	/// </summary>
	/// <typeparam name="T">Type that is to be enumerated.</typeparam>
	public interface IPAFEnumerableProvider<T>: IDisposable
	{
		#region Methods
		/// <summary>
		/// This method outputs the enumerable. This enumerable may
		/// produce an enumerator that runs forever. This is entirely
		/// implementation-dependant.
		/// </summary>
		/// <returns>
		/// The <see cref="IEnumerable{T}"/> that will perform the enumeration.
		/// Noted that this method returns an <see cref="IEnumerable{T}"/>
		/// and not an <see cref="IEnumerator{T}"/>.
		/// </returns>
		IEnumerable<T> GetEnumerable();
		#endregion Methods
	}
}