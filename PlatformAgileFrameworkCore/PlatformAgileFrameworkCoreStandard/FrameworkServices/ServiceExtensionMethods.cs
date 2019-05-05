//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2015 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// Extension methods for the service types.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 07mar2015 </date>s
	/// <description>
	/// Added history and added GetServiceNTO.
	/// </description>
	/// </contribution>
	/// </history>
	public static class ServiceExtensionMethods
	{
		/// <summary>
		/// Climbs the service hierarchy tree to find the root.
		/// </summary>
		/// <param name="service">
		/// The service that is POTENTIALLY in a hierarchy to find the root
		/// for. This method acts by calling either
		/// <see cref="IPAFServiceExtended.ServiceManager"/>
		/// or
		/// <see cref="IPAFServiceInternal.ServiceManagerInternal"/>
		/// to locate the managers in an upward chain.
		/// </param>
		/// <returns>
		/// The topmost manager that this service is allowed access to. Possibly
		/// <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// Note that if the service is not contained within a manager, this
		/// method will always return <see langword="null"/>.
		/// </remarks>
		public static IPAFServiceManager GetRootManager(
			this IPAFService service)
		{
			IPAFServiceManager rootManager = null;
			IPAFServiceManager tempManager = null;
			while(true)
			{
				IPAFServiceInternal serviceInternal;
				if((serviceInternal = service as IPAFServiceInternal) != null)
					tempManager = serviceInternal.ServiceManager;

				// Quit if we are not climbing anymore.
				if (tempManager == null) break;

				// Climb up.
				rootManager = tempManager;
			}
			return rootManager;
		}

		/// <summary>
		/// Returns the set of uninitialized services in the incoming set.
		/// </summary>
		/// <typeparam name="T">
		/// Can be any Type implementing <see cref="IPAFService"/>.
		/// </typeparam>
		/// <param name="services">
		/// The services. <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <param name="pipelineStage">
		/// The stage we want to check as being completed. Supported stages
		/// are <see cref="ServicePipelineStage.CONSTRUCTION"/>,
		/// <see cref="ServicePipelineStage.LOAD"/> and
		/// <see cref="ServicePipelineStage.INITIALIZE"/>.
		/// </param>
		/// <returns>
		/// The services that are as yet unconstructed/loaded/unitialized.
		/// </returns>
		public static ICollection<IPAFServiceDescription<T>> GetUnstagedServices<T>(
			this IEnumerable<IPAFServiceDescription<T>> services,
			ServicePipelineStage pipelineStage) where T : class, IPAFService
		{
			var retval = new Collection<IPAFServiceDescription<T>>();
			if (services == null) return retval;
			services = services.IntoArray();
			foreach (var svc in services) {
				if (svc.ServiceObject == null)
				{
					retval.Add(svc);
					continue;
				}
				var svcE = svc.ServiceObject as IPAFServiceExtended;
				if (svcE == null) continue;
				if ((pipelineStage == ServicePipelineStage.LOAD)
					&& (!svcE.ServiceIsLoaded)) retval.Add(svc);
				if ((pipelineStage == ServicePipelineStage.INITIALIZE)
					&& (!svcE.ServiceIsInitialized)) retval.Add(svc);
			}
			return retval;
		}
		/// <summary>
		/// This method returns <see langword="null"/> if the incoming service is not
		/// initialized and does not implement <see cref="IPAFEmergencyServiceProvider{U}"/>.
		/// Services that are not <see cref="IPAFServiceExtended"/> are automatically
		/// considered initialized.
		/// </summary>
		/// <param name="service">A service to check.</param>
		/// <returns><see langword="null"/> if service not initialized. </returns>
		public static IPAFServiceDescription<T> FilterUninitializedService<T>
			(this IPAFServiceDescription<T> service) where T : class, IPAFService
		{
			if (service.ServiceObject == null) return null;
			var svcE = service.ServiceObject as IPAFServiceExtended;
			if ((svcE != null) && !svcE.ServiceIsInitialized) {
				// We'll escape being filtered out if we will provide an emergency service.
				var eSvcP = svcE as IPAFEmergencyServiceProvider<T>;
				if (eSvcP != null) return service;
				return null;
			}
			return service;
		}
		/// <summary>
		/// This method returns the subset of the incoming services that are not
		/// initialized and do not implement <see cref="IPAFEmergencyServiceProvider{U}"/>.
		/// Services that are not <see cref="IPAFServiceExtended"/> are automatically
		/// considered initialized.
		/// </summary>
		/// <param name="services">Services to check.</param>
		/// <returns>
		/// Set of initialized services. Never <see langword="null"/>.
		/// </returns>
		public static IList<IPAFServiceDescription<T>> FilterUninitializedServices<T>
			(this IEnumerable<IPAFServiceDescription<T>> services) where T : class, IPAFService
		{
			var filteredServices = new Collection<IPAFServiceDescription<T>>();
			if (services == null) return filteredServices;
			foreach (var svc in services) {
				filteredServices.AddNoNulls(svc.FilterUninitializedService());
			}
			return filteredServices;
		}
		/// <summary>
		/// This method creates a <see cref="IPAFNamedAndTypedObject"/> from an
		/// incoming <see cref="IPAFServiceDescription"/>.
		/// </summary>
		/// <param name="serviceDescription">
		/// One of us. <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <returns> The constructed NTO. </returns>
		public static IPAFNamedAndTypedObject GetServiceNTO(this IPAFServiceDescription serviceDescription)
		{
			return serviceDescription;
//			return new PAFNamedAndTypedObject(serviceDescription.ServiceInterfaceType.TypeType,
//				serviceDescription.ServiceName);
		}

		/// <summary>
		/// This method creates a <see cref="IPAFNamedAndTypedObject"/> from an
		/// incoming <see cref="IPAFService"/>.
		/// </summary>
		/// <param name="serviceObject">
		/// One of us. <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <param name="registeredType">
		/// Specific type (usually an interface type) that the object inherits
		/// or implements. Default = <see langword="null"/> causes type of
		/// <typeparamref name="U"/> to be used, which is usually not what is
		/// wanted.
		/// </param>
		/// <returns> The constructed NTO. </returns>
		public static IPAFNamedAndTypedObject<U> GetServiceNTOFromServiceObject<U>
			(this U serviceObject, Type registeredType = null) where U: class, IPAFService
		{
			if (serviceObject == null)
				return null;
			if(registeredType == null)
				registeredType = typeof(U);
			return new PAFNamedAndTypedObject<U>(registeredType, null, serviceObject, true);
		}
		/// <summary>
		/// This method determines whether a service has been initialized.
		/// Services that are not <see cref="IPAFServiceExtended"/> are automatically
		/// considered initialized.
		/// </summary>
		/// <param name="service">
		/// Service to check. <see langword="null"/> returns <see langword="false"/>.
		/// </param>
		/// <returns> <see langword="true"/> if initialized. </returns>
		public static bool ServiceIsInitialized(this IPAFService service)
		{
			var xSvc = service as IPAFServiceExtended;
			if ((xSvc == null) || (xSvc.ServiceIsInitialized)) return true;
			return false;
		}
		/// <summary>
		/// This method determines whether a service has been loaded.
		/// Services that are not <see cref="IPAFServiceExtended"/> are automatically
		/// considered loaded.
		/// </summary>
		/// <param name="service">Service to check. </param>
		/// <returns> <see langword="true"/> if loaded. </returns>
		public static bool ServiceIsLoaded(this IPAFService service)
		{
			var xSvc = service as IPAFServiceExtended;
			if ((xSvc == null) || (xSvc.ServiceIsLoaded)) return true;
			return false;
		}
	}
}
