using System;
using System.Threading;
using PlatformAgileFramework.TypeHandling.Disposal;

// Exception shorthand.
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;


namespace PlatformAgileFramework.MultiProcessing.Threading.NullableObjects
{
	/// <summary>
	/// This interface prescribes members for a type that is designed to wrap an object that
	/// possibly has a null value. Object can be a value type or a reference type. The
	/// inteface prescribes access to disposable locks so that they can be conveniently
	/// be used in a <see langword="using"/> statement.
	/// </summary>
	/// <typeparam name="T">
	/// Reference or value type.
	/// </typeparam>
	/// <remarks>
	/// Wears <see cref="IPAFDisposable"/> so we can track leaks.
	/// </remarks>
	/// <threadsafety>
	/// <para>
	/// Implementations must be thread-safe.
	/// </para>
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 22apr2015 </date>
	/// <description>
	/// <para>
	/// Re-installed the one-way valve (HasBeenSet) from NullableWrapper - not sure why it
	/// was taken out. It's needed for the set once only flag.
	/// </para>
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 04sep2011 </date>
	/// <description>
	/// <para>
	/// Added history.
	/// </para>
	/// <para>
	/// Added the ability to put in a custom lock. Added the PAF disposal pattern. This
	/// was an attempt to reduce code bloat and streamline our synchronization facilities.
	/// Before this, we had a wrapper for nullable objects and a synchronization wrapper.
	/// Let's hope it works......
	/// </para>
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFNullableSynchronizedWrapper<T>:
		IUnprotectedDisposableProvider, IDisposable
	{
		#region Properties
		/// <summary>
		/// This method takes a read lock on the internal object so it can be
		/// read. In order for locking to be effective, references to the
		/// accessed object must not be retained. This is much the same in the
		/// use of a <see cref="Monitor"/>. The return value is disposable, in
		/// order to use the value within a using block that releases the lock
		/// when the using block is exited. We recommend ALWAYS using this
		/// technique.
		/// </summary>
		/// <returns>
		/// The disposable accessor.
		/// </returns>
		/// <remarks>
		/// This lock must be disposed after use.
		/// See <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>.
		/// </remarks>
		IPAFDisposableReadLockedObjectAccessor<T> GetReadLockedObject();
		/// <summary>
		/// <see cref="GetReadLockedObject()"/> This method just includes a timeout.
		/// </summary>
		/// <param name="millisecondsToWait">
		/// This is the number of milliseconds to wait before returning
		/// <see langword="null"/>
		/// </param>
		/// <returns>
		/// The disposable accessor, or <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// This lock must be disposed after use.
		/// See <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>.
		/// </remarks>
		IPAFDisposableReadLockedObjectAccessor<T> GetReadLockedObject(int millisecondsToWait);
		/// <summary>
		/// This method takes a write lock on the internal object so it can be
		/// manipulated. In order for locking to be effective, references to the
		/// accessed object must not be retained. This is much the same in the
		/// use of a <see cref="Monitor"/>. The return value is disposable, in
		/// order to use the value within a using block that releases the lock
		/// when the using block is exited. We recommend ALWAYS using this
		/// technique.
		/// </summary>
		/// <returns>
		/// The disposable accessor.
		/// </returns>
		/// <remarks>
		/// This lock must be disposed after use.
		/// See <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>.
		/// </remarks>
		IPAFDisposableWriteLockedObjectAccessor<T> GetWriteLockedObject();
		/// <summary>
		/// <see cref="GetWriteLockedObject()"/> This method just includes a timeout.
		/// </summary>
		/// <param name="millisecondsToWait">
		/// This is the number of milliseconds to wait before returning
		/// <see langword="null"/>
		/// </param>
		/// <returns>
		/// The disposable accessor, or <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// This lock must be disposed after use.
		/// See <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>.
		/// </remarks>
		IPAFDisposableWriteLockedObjectAccessor<T> GetWriteLockedObject(int millisecondsToWait);
		/// <summary>
		/// Determines whether the object has EVER been set, either by a constructor or a property.
		/// This method is synchronized. NEVER call it from within a lock.
		/// </summary>
		bool HasBeenSet { get;}
		/// <summary>
		/// Get/Set for the wrapped object. If the internal object is <see langword="null"/>, the
		/// get method simply returns <see langword="null"/>. If the <typeparamref name="T"/> is a value
		/// type, the getter returns <c>default(T)</c> if the object is not set. Note that the get/set
		/// take a reader/writer lock on the object, respectively, so this cannot be set when
		/// either lock is taken through the "Get....LockedObject methods are undisposed and may
		/// not be gotten if a GetWriteLockedObject is still undisposed.
		/// This method is synchronized. NEVER call it from within a lock.
		/// </summary>
		T NullableObject { get; set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Tells if the wrapped item is set.
		/// </summary>
		/// <returns><see langword="true"/> if set.</returns>
		/// <remarks>
		/// This method is actually rather useless in a multi-threaded environment,
		/// since someone else could have set/cleared the item between the time this
		/// method is called and the item is accessed. It is here for legacy apps
		/// that used monitor locks on this class.
		/// This method is synchronized. NEVER call it from within a lock.
		/// </remarks>
		bool HasValue();
		/// <summary>
		/// Nulls the internal object. This is needed for value types.
		/// This method is synchronized. NEVER call it from within a lock.
		/// </summary>
		void NullTheObject();
		/// <summary>
		/// This is a method that is needed for value types. It combines the fetch
		/// of an item with the check to see if it is set to something.
		/// This method is synchronized. NEVER call it from within a lock.
		/// </summary>
		/// <param name="success">
		/// Indicates whether the item is set.
		/// </param>
		/// <returns>
		/// The value of the wrapped item or <c>default(T)</c> if it is not set.
		/// </returns>
		T TryGetItem(out bool success);
		#endregion // Methods
	}
}
