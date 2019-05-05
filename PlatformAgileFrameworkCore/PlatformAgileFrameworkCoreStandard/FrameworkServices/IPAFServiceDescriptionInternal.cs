
using System;
using System.Security;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// Internal version of the interface for the serializer and for internal
	/// sets.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 21feb2012 </date>
	/// <contribution>
	/// New. - built with the public interface.
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	//Core
	internal partial interface IPAFServiceDescriptionInternal
		: IPAFServiceDescription
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Methods
		/// <summary>
		/// See corresponding prop on <see cref="IPAFServiceDescription"/>.
		/// </summary>
		bool SetIsDefault(bool isDefault);
		/// <summary>
		/// See corresponding prop on <see cref="IPAFServiceDescription"/>.
		/// </summary>
		void SetServiceInterfaceType(IPAFTypeHolder typeHolder);
		/// <summary>
		/// See corresponding prop on <see cref="IPAFServiceDescription"/>.
		/// </summary>
		void SetServiceImplementationType(IPAFTypeHolder typeHolder);
		/// <summary>
		/// See corresponding prop on <see cref="IPAFServiceDescription"/>.
		/// </summary>
		void SetServiceName(string serviceName);
		/// <summary>
		/// See corresponding prop on <see cref="IPAFServiceDescription"/>.
		/// </summary>
		void SetServiceObject(object obj);
		#endregion // Methods
		///// <summary>
		///// This provides filtering capabilities for the IMPLEMENTATION type.
		///// </summary>
		//// TODO - KRM removed in Core - make sure it gets back in extended.
		//IPAFTypeFilter ServiceImplementationTypeFilter { get; }
	}
}
