using System;
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.Remoting
{
    /// <summary>
    /// This interface is an internal extension of <see cref="IPAFLifetimeManagedObject{T}"/>
    /// allowing extensibility for framework builders.
    /// </summary>
    internal interface IPAFLifetimeManagedObjectInternal<T> : IPAFLifetimeManagedObject<T>
        where T: IPAFSecretKeyProvider
	{
        #region Methods
        /// <summary>
        /// Internal version of <see cref="IPAFLifetimeManagedObject{T}.AcquireReference"/>
        /// that does not require elevated trust.
        /// </summary>
        /// <param name="sponsor">
        /// See <see cref="IPAFLifetimeManagedObject{T}.AcquireReference"/>.
        /// </param>
        /// <returns>
        /// See <see cref="IPAFLifetimeManagedObject{T}.AcquireReference"/>.
        /// </returns>
        T AcquireReferenceInternal(IPAFLeaseSponsor sponsor);
        /// <summary>
        /// Registers a sponsor in the sponsor registry. Note that this does not
        /// establish a reference to the object. This must be done through
        /// <see cref="IPAFLifetimeManagedObject{T}.AcquireReference"/>, etc.
        /// </summary>
        /// <param name="sponsor">
        /// See <see cref="IPAFLifetimeManagedObject{T}.RegisterLeaseSponsor"/>.
        /// </param>
        void RegisterLeaseSponsorInternal(IPAFLeaseSponsor sponsor);
        /// <summary>
        /// Internal version of <see cref="IPAFLifetimeManagedObject{T}.DefaultSponsorCallExtensionTimeInMilliseconds"/>
        /// that does not require elevated trust.
        /// </summary>
        /// <param name ="defaultSponsorCallExtensionTimeInMilliseconds">
        /// See <see cref="IPAFLifetimeManagedObject{T}.DefaultSponsorCallExtensionTimeInMilliseconds"/>.
        /// </param>
        void SetDefaultSponsorCallExtensionTimeInMilliseconds(int defaultSponsorCallExtensionTimeInMilliseconds);
        /// <summary>
        /// Internal version of <see cref="IPAFLifetimeManagedObject{T}.RemainingLeaseTimeInMilliseconds"/>
        /// that does not require elevated trust.
        /// </summary>
        /// <param name ="remainingLeaseTimeInMilliseconds">
        /// The remaining lease time. This can increase or decrease the remaining time.
        /// </param>
        void SetRemainingLeaseTimeInMilliseconds(int remainingLeaseTimeInMilliseconds);
        /// <summary>
        /// Internal version of <see cref="IPAFLifetimeManagedObject{T}.RenewOnCallTimeInMilliseconds"/>
        /// that does not require elevated trust.
        /// </summary>
        /// <param name ="renewOnCallTimeInMilliseconds">
        /// See <see cref="IPAFLifetimeManagedObject{T}.RenewOnCallTimeInMilliseconds"/>.
        /// </param>
        void SetRenewOnCallTime(int renewOnCallTimeInMilliseconds);
        /// <summary>
        /// Internal version of <see cref="IPAFLifetimeManagedObject{T}.SponsorshipTimeoutInMilliseconds"/>
        /// that does not require elevated trust.
        /// </summary>
        /// <param name ="sponsorshipTimeoutInMilliseconds">
        /// See <see cref="IPAFLifetimeManagedObject{T}.SponsorshipTimeoutInMilliseconds"/>.
        /// </param>
        void SetSponsorShipTimeout(int sponsorshipTimeoutInMilliseconds);
		#endregion // Methods
	}
}
