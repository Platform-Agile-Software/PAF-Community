using System.Collections.Generic;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	///	Carries a list of items. For security purposes, this list is "sealable". 
	/// If the list is sealed, no more itemss may be added. Typical usage is to
	/// pass this container around in trusted code, allowing items to be added
	/// at various points, then seal the list when passing the container out into
	/// untrusted environments.
	/// </summary>
	/// <typeparam name="T">
	/// Constrained to be a reference type. Use nullables to wrap value types.
	/// </typeparam>
	/// <history>
	/// <author> KRM </author>
	/// <date> 24jun2013 </date>
	/// <contribution>
	/// Factored this out of several places in which it was replicated
	/// in one form or another - code consolidation.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Designed to be loaded by one accessing thread at a time. Needn't be
	/// thread-safe usually.
	/// </threadsafety>
	public interface IPAFSealableEnumerable<T> where T : class
	{
		/// <summary>
		/// Gets the items.
		/// </summary>
		IEnumerable<T> Items { get; }

		/// <summary>
		/// This method adds an item to the list of items if the list is not
		/// sealed. If the item passed to this method is <see langword="null"/>,
		/// this seals the list. The <see langword="null"/> item is not
		/// added to the list.
		/// </summary>
		/// <param name="item">Item to be added.</param>
		void AddItem(T item);
	}
}