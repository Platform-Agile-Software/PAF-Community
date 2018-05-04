using System;



namespace PlatformAgileFramework.MultiProcessing.Threading.NullableObjects
{
	/// <summary>
	/// This interface prescribes members for a type that is designed to wrap an object that
	/// requires synchronized access. When a type wearing this interface is either created
	/// or handed out, an exclusive lock is taken out on the object's container so that
	/// the internal object may be manipulated. This interface inherits from <see cref="IDisposable"/>
	/// so that it can be used in a <see langword="using"/> statement. It is imperative to ensure
	/// that <see cref="IDisposable.Dispose"/> be called. Otherwise the containing
	/// type will remain locked forever. Thus, a using statement or the equivalent
	/// is highly recommended.
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
	public interface IPAFDisposableWriteLockedObjectAccessor<T>: IDisposable
	{
		#region Properties
		/// <summary>
		/// Get/set the wrapped object. May be <see langword="null"/>.
		/// </summary>
		/// <remarks>
		/// Although this property can be extremely useful, it can also be
		/// very dangerous. Any manipulations necessary can be done on a
		/// reference type "in place". A value type must be unboxed by using
		/// the get method, modified, then reboxed by using the set method.
		/// </remarks>
		T WriteLockedNullableObject { get; set; }
		/// <summary>
		/// Returns <see langword="false"/> if the object has ever been set,
		/// either by a constructor or a method.
		/// </summary>
		bool HasBeenSet { get; }
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
