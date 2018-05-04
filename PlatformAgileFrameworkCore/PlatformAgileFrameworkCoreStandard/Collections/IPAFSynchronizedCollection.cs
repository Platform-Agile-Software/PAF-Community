using System;
using System.Collections.Generic;
using PlatformAgileFramework.Serializing.Interfaces;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Collections
{
    /// <summary>
    /// Interface adds a few methods so operations can be done atomically and with
    /// custom setup. Built primarily to sidestep problems/deficiencies with
    /// Microsoft thread-safe collections. This interface also adds a "side door" which
    /// will lock the collection and allow it's manipulation as a list when needed.
    /// </summary>
    /// <typeparam name="T">Generic type contained in the collection.</typeparam>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 22jan2017 </date>
    /// <description>
    /// Just added "PAF" to the name to avoid naming conflicts (once again).
    /// </description>
    /// </contribution>
    /// <contribution>
    /// <author> DAP </author>
    /// <date> 22jan2012 </date>
    /// <description>
    /// New.
    /// </description>
    /// </contribution>
	/// </history>
    // Same add method for both. serializable interface should be implemented
    // explicitly.
    // ReSharper disable PossibleInterfaceMemberAmbiguity
    public interface IPAFSynchronizedCollection<T> : ICollection<T>, IPAFSerializableCollection<T>
// ReSharper restore PossibleInterfaceMemberAmbiguity
	{
		#region Properties
		/// <summary>
		/// This property allows setting of a custom comparer so the collection can
		/// find its contained types and identify dupes.
		/// </summary>
		IEqualityComparer<T> EqualityComparer { set; }
		/// <summary>
		/// This property allows setting of a custom enumerator factory so the collection can
		/// provide custom enumerators.
		/// </summary>
		IEnumeratorFactory<T> EnumeratorFactory { set; }
		/// <summary>
		/// This property allows setting of a custom cloner (deep or shallow) so the collection can
		/// provide custom cloning operations.
		/// </summary>
		TypeHandlingUtils.TypeCloner<T> TypeCloner { set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Adds an item atomically if it is not already in the collection.
		/// </summary>
		/// <param name="item">The item to attempt to add.</param>
		/// <returns>
		/// <see langword="true"/> if the object was not already in the collection.
		/// </returns>
		bool AddNoDupes(T item);
		/// <summary>
		/// Removes an item atomically if it is in the collection.
		/// </summary>
		/// <param name="item">The item to attempt to remove.</param>
		/// <returns>
		/// <see langword="true"/> if the object was in the collection.
		/// </returns>
		bool RemoveIfPresent(T item);
		/// <summary>
		/// This method returns a synchronized wrapper that allows the internal list to
		/// be locked as though a <see cref="System.Threading.Monitor"/> had been used to
		/// place an exclusive lock on the the list. This lock is held until the client
		/// calls <see cref="IDisposable.Dispose"/> on the accessor after finishing with
		/// it. This design allows the <see cref="IPAFSynchronizedCollection{T}"/> to
		/// be used as a normal list as well as having fast access properties associated
		/// with a read/write lock. An internal write lock is taken on the internal list
		/// and is released when Dispose() is called on the accessor. Put the accessor in
		/// a "using" clause for ease of use and to help ensure that the purpose is understood.
		/// </summary>
		/// <returns>
		/// An accessor for the internal list which can be used as an ordinary
		/// <see cref="IList{T}"/>, but must be disposed after use.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If <see cref="IDisposable.Dispose"/> is not called on the accessor after use,
		/// a deadlock will occur.
		/// </para>
		/// <para>
		/// Obviously, this accessor should be used as infrequently as possible, since
		/// a write lock is taken during its use. Use the atomic methods exposed by
		/// <see cref="IPAFSynchronizedCollection{T}"/> when possible when
		/// manipulating the collection.
		/// </para>
		/// </remarks>
		IDisposableList<T> GetListAccessor();
		/// <summary>
		/// This method allows the list to be cleared and reloaded in one atomic operation.
		/// </summary>
		/// <param name="enumerable">The data to reload the list with.</param>
		void ResetCollection(IEnumerable<T> enumerable);
		#endregion // Methods
	}
}