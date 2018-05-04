using System;
using System.Security;
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.Remoting
{
	/// <summary>
	/// <para>
	/// This interface describes the functionality needed to manipulate
	/// objects whose lifetime needs to be controlled. It is useful for
	/// local objects. When combined with other PAF interfaces, it is useful
	/// for controlling remote objects.
	/// </para>
	/// <para>
	/// PAF applications have required the ability to manage object lifetime
	/// both by reference counting and by time expiry. BOTH are required for
	/// many clients. An example use case is a client that can go dormant
	/// for a period of time, but may be resurrected periodically. If the
	/// client fails to respond to a renewal call within the required time,
	/// but is registered as a reference, the LMO can be kept alive. If
	/// the LMO corresponds to a persistent stateful service, client-specific
	/// state information may be persisted at that time for later re-hydration.
	/// If all clients are dormant, the service itself can be made dormant.
	/// This is important on "small machines".
	/// </para>
	/// <para>
	/// Implementations of this interface can be used in a wide variety of
	/// ways. Implementors should document usage limitations and rules carefully.
	/// The interface can be used for anything from local services within an
	/// "AppDomain" or classical remoting across "AppDomain"
	/// boundaries or most other scenarios involving some sort of loosely-coupled
	/// exchange.
	/// </para>
	/// </summary>
	/// <threadsafety>
	/// Implementations should be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <author> DAP </author>
	/// <date> 07nov2011 </date>
	/// <contribution>
	/// <para>
	/// New. Major reformulation of the "IMBRO" interface and combination with
	/// "ISmallMachine" interface. Supports all new remote services and the old
	/// ones, too. This has been done as part of the code compaction/consolidation
	/// for SL model.
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFLifetimeManagedObject<T> : IPAFLeaseSponsor, IDisposable
        where T: IPAFSecretKeyProvider
	{
        #region Properties
        /// <summary>
        /// Gets/sets the remaining lease time. Set to <c>-1</c>
        /// to ensure permanence. This is a signal value that allows implementations
        /// to disable callback timers and the like, so, it's important to set
        /// exactly this value for objects with infinite persistence. Infinite
        /// persistance should be un-doable by the implementation by simply resetting
        /// this property to something less than <c>-1</c>.
        /// </summary>
        int RemainingLeaseTimeInMilliseconds
		{
			get;
			[SecurityCritical]
			set;
		}
		/// <summary>
		/// Gets/sets the amount of time lease should be extended for on each call to
		/// this LMO. This applies to the situation when the LMO is employed as
		/// as backing for a service or some other object that accepts calls on its
		/// methods. In either a local or loosely-coupled scenario, a client renews its
		/// interest in continuing to use the object by calling methods on the object.
		/// This property specifies how long the lease is extended each time a call is
		/// made on the object.
		/// </summary>
		int RenewOnCallTimeInMilliseconds
		{
			get;
			[SecurityCritical]
			set;
		}
		/// <summary>
		/// Gets/sets the DEFAULT amount of time lease should be extended for each callback
		/// to a registered <see cref="IPAFLeaseSponsor"/>. When the time extension request
		/// by the client is <see langword="null"/>, this is the amount of time the lease is extended.
		/// </summary>
		int DefaultSponsorCallExtensionTimeInMilliseconds
		{
			get;
			[SecurityCritical]
			set;
		}
		/// <summary>
		/// Gets the amount of time lease manager will wait for this LMO's
		/// sponsors to respond to a renewal call.
		/// </summary>
		/// <remarks>
		/// The typical implementation looks to see if the client holds a reference
		/// if the timeout expires. If it does and it is a client-specific stateful
		/// service, the client's state data will be hydrated.
		/// </remarks>
		int SponsorshipTimeoutInMilliseconds
		{
			get;
			[SecurityCritical]
			set;
		}
        #endregion // Properties
        #region Methods
        /// <summary>
        /// Client provides/receives an unique identifier that it stores until the object
        /// is no longer needed. Then call <see cref="RelinquishReference"/>
        /// with the token <typeparamref name="T"/> to release the claim on the object.
        /// </summary>
        /// <param name="sponsor">
        /// The lease sponsor wishing to acquire the reference. If the <typeparamref name="T"/>
        /// is default.<typeparamref name="T"/>, a <typeparamref name="T"/> is generated
        /// internally and handed back to the client. If the TimeSpan is non-zero, the sponsor
        /// is registered in the sponsor registry with the initial time extension.
        /// </param>
        /// <returns>
        /// A <typeparamref name="T"/> unique to the client. If the <typeparamref name="T"/> on the sponsor
        /// is not default.<typeparamref name="T"/> and it is already found in the internal
        /// dictionary, the reference is not taken and default.<typeparamref name="T"/> is
        /// returned. This means that an <see cref="IPAFLifetimeManagedObject{T}"/> can
        /// only use it's internal <typeparamref name="T"/> once if it is self-referenced.
        /// </returns>
        [SecurityCritical]
		T AcquireReference(IPAFLeaseSponsor sponsor);
		/// <summary>
		/// Registers a sponsor in the sponsor registry. Note that this does not
		/// establish a reference to the object. This must be done through
		/// <see cref="AcquireReference"/>, etc.
		/// </summary>
		/// <param name="sponsor">
		/// The lease sponsor to register.
		/// </param>
		[SecurityCritical]
		void RegisterLeaseSponsor(IPAFLeaseSponsor sponsor);
        /// <summary>
        /// Remove the reference to the LMO.
        /// </summary>
        /// <param name="referenceID">
        /// This is the <typeparamref name="T"/> that was associated with the requestor in the
        /// <see cref="AcquireReference"/> method.
        /// </param>
        void RelinquishReference(T referenceID);
        /// <summary>
        /// Client provides/receives an unique identifier token that registers it until the object
        /// is no longer needed by the client. Then call <see cref="RelinquishReference"/>
        /// with the token to release the claim on the object. This method is used when
        /// the server desires to allow access only to certain <typeparamref name="T"/>'s.
        /// </summary>
        /// <param name="sponsor">
        /// Depending on the implementation, the server may allow access to only certain
        /// clients. In these cases, the <typeparamref name="T"/> must not be empty and must be correct.
        /// </param>
        /// <returns>
        /// See <see cref="AcquireReference"/>. This method is not marked with
        /// <see cref="SecurityCriticalAttribute"/>. An implementation is free to
        /// return default.<typeparamref name="T"/> and refuse the reference request in
        /// cases where the caller is not trusted. The scenario here (for example)
        /// is where a service manager or some higher-level entity establishes
        /// trust for a set of plugins and assigns them <typeparamref name="T"/>s for access to services.
        /// This prevents rogue plugins from compromising the system.
        /// </returns>
        T SafeAcquireReference(IPAFLeaseSponsor sponsor);
		/// <summary>
		/// See <see cref="RegisterLeaseSponsor"/>. This version is slightly
		/// different in that only certain clients may be allowed to register.
		/// </summary>
		/// <param name="sponsor">
		/// See <see cref="RegisterLeaseSponsor"/>.
		/// </param>
		/// <returns>
		/// Implementation is free to reject registration for a specific client
		/// and return <see langword="false"/>.
		/// </returns>
		bool SafeRegisterLeaseSponsor(IPAFLeaseSponsor sponsor);
		/// <summary>
		/// Removes a sponsor from the sponsor registry. Note that this does not
		/// remove the reference to the object, if one was taken through
		/// <see cref="AcquireReference"/>, etc.
		/// </summary>
		/// <param name="sponsor">
		/// The lease sponsor to unregister.
		/// </param>
		void UnregisterLeaseSponsor(IPAFLeaseSponsor sponsor);
		#endregion // Methods
	}

}
