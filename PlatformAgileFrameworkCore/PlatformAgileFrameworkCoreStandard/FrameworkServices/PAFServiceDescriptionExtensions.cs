using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// Extensions and helpers for service descriptions and related types.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 21feb2012 </date>
	/// <contribution>
	/// New - moved repetitive code into this class.
	/// </contribution>
	/// </history> 
// ReSharper disable PartialTypeWithSinglePart
	//Core
	public static partial class PAFServiceDescriptionExtensions
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Methods
		/// <summary>
		/// This generates a new description from an old one with
		/// just the default flag modified.
		/// </summary>
		/// <param name="serviceDescription">
		/// Incoming description that is to be cloned with just the
		/// <see cref="IPAFServiceDescription{T}.IsDefault"/> property
		/// mofified.
		/// </param>
		/// <param name="isDefault">
		/// <see langword="null"/> flips the flag.
		/// <see langword="true"/> sets the flag.
		/// <see langword="false"/> clears the flag.
		/// </param>
		/// <returns>
		/// A description with the default flag changed appropriately.
		/// </returns>
		/// <remarks>
		/// Generates a <see cref="PAFServiceDescription{T}"/> to implement
		/// the interface. Note that a reference copy of the service object
		/// is made. A copy of the entire service is made to avoid external
		/// clients needing full trust or having access to our internals.
		/// </remarks>
		public static IPAFServiceDescription<T> ChangeDefaultFlag<T>(
			this IPAFServiceDescription<T> serviceDescription, bool? isDefault = null)
			where T: class, IPAFService
		{
			bool flagToBeSet;

			if (isDefault == null)
			{
				if (serviceDescription.IsDefault) flagToBeSet = false;
				else flagToBeSet = true;
			}
			else flagToBeSet = !serviceDescription.IsDefault;
			return new PAFServiceDescription<T>(
				serviceDescription.ServiceInterfaceType,
				serviceDescription.ServiceImplementationType,
				serviceDescription.ServiceName,
				flagToBeSet);
		}
		/// <summary>
		/// This generates a full description from the nto.
		/// </summary>
		/// <param name="nto">Defaulted nto.</param>
		/// <returns>
		/// A description with the name, default flag and the interface type.
		/// Implementation type is <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// Generates a <see cref="PAFServiceDescription"/> to implement
		/// the interface.
		/// </remarks>
		public static IPAFServiceDescription GetServiceDescriptionInterface(
			this IPAFNamedAndTypedObject nto)
		{
			return GetServiceDescription(nto);
		}
		/// <summary>
		/// This generates a full description from the nto.
		/// </summary>
		/// <param name="nto">Defaulted nto.</param>
		/// <returns>
		/// A description with the name, default flag and the interface type.
		/// Implementation type is <see langword="null"/>.
		/// </returns>
		public static PAFServiceDescription GetServiceDescription(
			this IPAFNamedAndTypedObject nto)
		{
			return new PAFServiceDescription(nto);
		}


		/// <summary>
		/// Makes a safe call into the service description property. Useful for user-supplied services without
		/// access to the internal interface.
		/// </summary>
		/// <param name="serviceDescription">One of us.</param>
		/// <param name="isDefault">param to set.</param>
		[SecuritySafeCritical]
		internal static void SafeSetIsDefault(this IPAFServiceDescription serviceDescription, bool isDefault)
		{
			serviceDescription.IsDefault = isDefault;
		}
		/// <summary>
		/// Makes a safe call into the service description property. Useful for user-supplied services without
		/// access to the internal interface.
		/// </summary>
		/// <param name="serviceDescription">One of us.</param>
		/// <param name="serviceObject">param to set.</param>
		[SecuritySafeCritical]
		internal static void SafeSetServiceObject(this IPAFServiceDescription serviceDescription,
			object serviceObject)
		{
			serviceDescription.ServiceObject = serviceObject;
		}
		/// <summary>
		/// Finds a service interface in a collection of services.
		/// </summary>
		/// <param name="serviceDescriptions">Collection to search.</param>
		/// <returns>
		/// First located service or <see langword="null"/>.
		/// </returns>
		[SecurityCritical]
		public static IPAFServiceDescription FindServiceInterfaceInCollection<T>(
			this IEnumerable<IPAFServiceDescription> serviceDescriptions)
			where T : class, IPAFService
		{
			return FindServiceImplementationTypeInCollectionInternal<T>(
				serviceDescriptions);
		}
		/// <summary>
		/// Finds a service interface in a collection of services.
		/// </summary>
		/// <param name="serviceDescriptions">Collection to search.</param>
		/// <returns>
		/// First located service or <see langword="null"/>.
		/// </returns>
		[SecuritySafeCritical]
		internal static IPAFServiceDescription FindServiceInterfaceInCollectionInternal<T>(
			this IEnumerable<IPAFServiceDescription> serviceDescriptions)
			where T : class, IPAFService
		{
		    return serviceDescriptions?.FirstOrDefault(serviceDescription => serviceDescription.ServiceInterfaceType.TypeType == typeof(T));
		}
		/// <summary>
		/// Finds a service implementation type in a collection of services.
		/// </summary>
		/// <param name="serviceDescriptions">Collection to search.</param>
		/// <returns>
		/// First located service or <see langword="null"/>.
		/// </returns>
		[SecuritySafeCritical]
		internal static IPAFServiceDescription FindServiceImplementationTypeInCollectionInternal<T>(
			this IEnumerable<IPAFServiceDescription> serviceDescriptions)
			where T : class, IPAFService
		{
			if (serviceDescriptions == null) return null;
			foreach (var serviceDescription in serviceDescriptions)
			{
				// Descriptions not need have a type
			    if (serviceDescription.ServiceImplementationType?.TypeType == null) continue;
				if (serviceDescription.ServiceImplementationType.TypeType == typeof(T)) return serviceDescription;
			}
			return null;
		}
		/// <summary>
		/// Finds a service implementation type in a collection of services.
		/// </summary>
		/// <param name="serviceDescriptions">Collection to search.</param>
		/// <returns>
		/// First located service or <see langword="null"/>.
		/// </returns>
		[SecurityCritical]
		public static IPAFServiceDescription FindServiceImplementationTypeInCollection<T>(
			this IEnumerable<IPAFServiceDescription> serviceDescriptions)
			where T : class, IPAFService
		{
			return FindServiceImplementationTypeInCollectionInternal<T>(serviceDescriptions);
		}
		#endregion // Methods
	}
}
