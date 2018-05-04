using System;
using System.Reflection;
using PlatformAgileFramework.Notification.Exceptions;

namespace PlatformAgileFramework.TypeHandling.Delegates
{
	/// <summary>
	/// An <see cref="IPseudoDelegate{TDelegate}"/> contains some of the same
	/// information that a <see cref="Delegate"/> contains, but no invocation list.
	/// Weak references to the target can be maintained. This interface
	/// does not allow any components of the pseudodelegate to be modified. It is
	/// designed to be immutable so methods and targets can never get mismatched
	/// from the <typeparamref name="TDelegate"/>. We need to regenerate the
	/// <typeparamref name="TDelegate"/> from the <see cref="IPseudoDelegate{TDelegate}"/>,
	/// so we can't have ANY mismatches.
	/// </summary>
	/// <typeparam name="TDelegate">
	/// This Generic must always be checked to see that it is a subclass of
	/// <see cref="MulticastDelegate"/>, since there is no way to do this
	/// at compile time. This is usually done in a static constructor for the
	/// implementing class.
	/// </typeparam>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 25aug2013 </date>
	/// <description>
	/// Created weak pseudodelegates.
	/// </description>
	/// </contribution>
	/// <remarks>
	/// <para>
	/// A <see cref="IPseudoDelegate{TDelegate}"/> can very well be built from
	/// a <typeparamref name="TDelegate"/>, but the delegate's invocation
	/// list is not extracted. This is done to explicitly avoid the chaining of
	/// delegate invocation lists, which is a problem in our concurrency
	/// testing work. Implementations can choose to throw exceptions such as
	/// <see cref="PAFDelegateExceptionMessageTags.DELEGATE_HAS_SUBSCRIBERS"/>
	/// </para>
	/// <para>
	/// It should be noted that there is no need to do a dynamic invoke on a
	/// CLOSED PD, since the PD generates the proper Invoke(.....) method.
	/// </para>
	/// </remarks>
	public interface IPseudoDelegate <out TDelegate>
		where TDelegate: class
	{
		/// <summary>
#pragma warning disable 1584, 1711, 1572, 1581, 1580
		/// The <see cref="MethodInfo"/> from the original
		/// <see cref="Delegate.GetMethodInfo()"/> property.
#pragma warning restore 1584, 1711, 1572, 1581, 1580
		/// </summary>
		MethodInfo DelegateMethod { get; }
		/// <summary>
		/// If this is <see langword="true"/> for a static method.
		/// </summary>
		bool IsStatic { get; }
		/// <summary>
		/// Tells if the delegate's target is held with a weak reference.
		/// Needed to know whether we need to monitor it for collection.
		/// </summary>
		bool IsWeak { get; }
		/// <summary>
		/// This is the state of the subscriber - may be an undisciplined subscriber.....
		/// </summary>
		SubscriberState State { get; set; }
		/// <summary>
		/// Will be the instance of a class or a <see cref="Type"/> for
		/// static methods. This will be <see langword="null"/> for
		/// an instance delegate whose instance has been garbage collected.
		/// This happens with weak pseudodelegates.
		/// </summary>
		object Target { get; }

		/// <summary>
		/// Gets the original delegate from its components, if the delegate is
		/// a static delegate or if the instance pointer, <see cref="Target"/>
		/// is non-<see langword="null"/>. Otherwise <see langword="null"/>
		/// is returned. 
		/// </summary>
		TDelegate GetDelegate();

	}
}