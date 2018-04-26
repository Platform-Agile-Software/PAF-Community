using System.Runtime.InteropServices;
using System.Security;

namespace PlatformAgileFramework.TypeHandling.WeakBindings
{
	/// <summary>
	/// Represents the protocol for a weak reference, which references an object while still
	/// allowing that object to be reclaimed by garbage collection. This is a neutered version
	/// of Microsoft's weak reference. Implementing classes should not be marked as serializable,
	/// since there is no guarantee the payload is serializable. There is no reason for user
	/// code to ever track objects after finalization, so we don't allow that, either.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item referenced.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// Turned this into a consolidated interface. Now it can wrap both weak
	/// and strong references. Just added the <see cref="IsWeak"/> property.
	/// Took out the setter for the target, since this should always be an immutable
	/// implementation.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 02sep2014 </date>
	/// <description>
	/// New.
	/// I made this thing interface-based, since we MAY have to reimplement it if
	/// <see cref="SecurityCriticalAttribute"/> issues pop up with <see cref="GCHandle"/>.
	/// Also took out isalive from the interface due to problems with inexperienced
	/// programmers.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFWeakableReference<out T> where T : class
	{
		/// <summary>
		/// Gets or sets the object (the target) referenced or <see langword="null"/>.
		/// </summary>
		/// <returns>
		/// <see langword="null"/> if the referenced object has been garbage collected;
		/// otherwise, a reference to the object.
		/// </returns>
		/// <remarks>
		/// If an item is passed into the set method and that item has no other references to it, it may
		/// get garbage-collected before we can hook it up to the handle. This would indicate an error
		/// in programming, but we catch the exception anyway. The user can "get" the target after it has
		/// been set to ensure this did not happen. The user must copy the target into a strong
		/// reference before checking it. In other words, "if(pAFWeakReference.Target != null)" is
		/// always wrong.
		/// </remarks>
		T Target { get; }
		/// <summary>
		/// Tells if the reference is actually weak as opposed to a strong reference. If
		/// it is strong, <see cref="Target"/> should always be non-<see langword="null"/>.
		/// </summary>
		bool IsWeak { get; }
	}
}