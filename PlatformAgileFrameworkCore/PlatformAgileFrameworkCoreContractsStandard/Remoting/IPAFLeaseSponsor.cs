namespace PlatformAgileFramework.Remoting
{
	/// <summary>
	/// This interface provides the ability for a client to register for lease
	/// renewal callbacks with a <see cref="IPAFLifetimeManagedObject"/>. The
	/// implementation controls how much of this information must be present.
	/// In high-trust environments, the LMO may even allow the returned
	/// <see cref="PAFLeaseRenewalData"/> to be <see langword="null"/>.
	/// </summary>
	public interface IPAFLeaseSponsor
	{
		#region Methods
		/// <summary>
		/// Gets the client's <see cref="PAFLeaseRenewalData"/>.
		/// </summary>
		PAFLeaseRenewalData GetClientLeaseRenewalData();
		#endregion // Methods
	}

}
