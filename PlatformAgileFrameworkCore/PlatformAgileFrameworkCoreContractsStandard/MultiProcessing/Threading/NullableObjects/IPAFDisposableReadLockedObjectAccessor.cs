using System;



namespace PlatformAgileFramework.MultiProcessing.Threading.NullableObjects
{
	/// <summary>
	/// This interface prescribes members for a type that is designed to wrap an object that
	/// requires synchronized access. When a type wearing this interface is either created
	/// or handed out, a non-exclusive read lock is taken out on the object's container so that
	/// the internal object may be accessed and read, but not modified. This interface inherits
	/// from <see cref="IDisposable"/> so that it can be used in a <see langword="using"/>
	/// statement. It is imperative to ensure that <see cref="IDisposable.Dispose"/> be called.
	/// Otherwise the containing type will remain locked forever. Thus, a using statement or
	/// the equivalent is highly recommended.
	/// </summary>
	/// <threadsafety>
	/// <para>
	/// Thread-safe by design.
	/// </para>
	/// </threadsafety>
	/// <history>
	/// <author> BMC </author>
	/// <date> 04sep2011 </date>
	/// <contribution>
	/// <para>
	/// New. Needed a side lock on the synchronized objects.
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFDisposableReadLockedObjectAccessor<out T>: IDisposable
	{
		#region Properties
		/// <summary>
		/// Returns <see langword="false"/> if the object has ever been set,
		/// either by a constructor or a method.
		/// </summary>
		bool HasBeenSet { get; }
		/// <summary>
		/// Get the wrapped object. May be <see langword="null"/>.
		/// </summary>
		/// <remarks>
		/// It is imperative that the developer understand that no UNSYNCHRONIZED
		/// members of the returned object be modified when the read lock is held.
		/// The object may have an hierarchy of elements/members which may or may
		/// not be synchronized themselves. Concurrency errors will occur if this
		/// rule is not followed. The read lock is a NON-EXCLUSIVE lock.
		/// </remarks>
		T ReadLockedNullableObject { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Checks the nullable to see if it's <see langword="null"/>. Needed
		/// for value types.
		/// </summary>
		bool ObjectIsNull();
		#endregion // Methodss

	}
}
