using System;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Remoting
{
	/// <summary>
	/// This class provides the ability for a client to provide information
	/// about lease renewal to a <see cref="IPAFLifetimeManagedObject"/>. The
	/// implementation controls how much of this information must be present.
	/// In elevated-trust, local environments, the LMO may not require the
	/// presence of a correct, unique Guid. For token registration with the
	/// service, the <see cref="Guid"/> must always be present and unique.
	/// </summary>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public class PAFLeaseRenewalData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Gets/sets the client's <see cref="Guid"/>.
		/// </summary>
		public Guid ClientGuid { get; protected internal set; }
		/// <summary>
		/// Gets/sets the client's extension time. If this is <see langword="null"/>
		/// or zero, it indicates that the client no longer wishes to maintain
		/// a connection to the remote service.
		/// </summary>
		public TimeSpan? LeaseExtensionTime { get; protected internal set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor just set properties.
		/// </summary>
		/// <param name="clientGuid">
		/// Default = <see cref="Guid.Empty"/>.
		/// </param>
		/// <param name="leaseExtensionTime">
		/// Default = <see langword="null"/>.
		/// </param>
		public PAFLeaseRenewalData(Guid clientGuid = default(Guid), TimeSpan? leaseExtensionTime = null)
		{
			ClientGuid = clientGuid;
			LeaseExtensionTime = leaseExtensionTime;
		}
		#endregion // Constructors
	}
}
