//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// A few extensions that are necessary for accessing services.
	/// </summary>
	/// <threadsafety>
	///  Not thread-safe. Lock dictionary.
	/// </threadsafety>
	/// <remarks>
	/// All of the methods require the interface types in the dictionary
	// ReSharper disable once CSharpWarnings::CS1584
	// resharper mistake
	/// to be early-bound - i.e. <see cref="IPAFServiceDescription.ServiceInterfaceType.TypeType"/>
	/// is not <see langword="null"/>.
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> DAV </author>
	/// <date> 22jun2012 </date>
	/// <description>
	/// Split extensions into core and extended.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	// Core exposes only interfaces.
	// Note: KRM - that's the way it should always have been, everywhere.
	public static partial class PAFServiceDictionaryExtensions
	{
		/// <remarks>
		/// Enumerates all services in the layered dictionary, in their sort
		/// order. See <see cref="IPAFServiceDictionary.GetAllServices"/>
		/// This extension method has exactly the same functionality.
		/// Factored into an extension method so it can be used in anybody's
		/// dictionary. No exceptions are generated or caught.
		/// </remarks>
		public static IEnumerable<IPAFServiceDescription> EnumerateAllServices(
			this IPAFServiceDictionary serviceDictionary)
		{
			var serviceList = new Collection<IPAFServiceDescription>();
			// Loop to find the correct dictionary.
			foreach (var dict in serviceDictionary.Values)
			{
				if (dict != null)
				{
					foreach (var service in dict.Values)
					{
						serviceList.Add(service);
					}
				}
			}
			return serviceList;
		}

		/// <summary>
		/// This is a helper method. It retrieves a set of services by type, irrespective
		/// of name.
		/// </summary>
		/// <param name="serviceDictionary">
		/// One of us.
		/// </param>
		/// <param name="interfaceType">
		/// This is the type of the service interface.
		/// </param>
		/// <param name="exactTypeMatch">
		/// If set to <see langword="true"/>, derived Types will not be returned.
		/// </param>
		/// <remarks>
		/// Services are returned in sort order.
		/// </remarks>
		/// <threadsafety>
		/// Unsafe lock unsynchronized dictionary before use.
		/// </threadsafety>
		public static IList<IPAFServiceDescription> GetAnyServicesUnsafe(
			this IPAFServiceDictionary serviceDictionary,
			Type interfaceType, bool exactTypeMatch = false)
		{
			var services = new Collection<IPAFServiceDescription>();

			if (!interfaceType.IsTypeAnInterfaceType()) return null;

			// Query the dictionary.
			services.AddNoDupes(serviceDictionary.GetServiceInterfacesOfType(
				interfaceType, exactTypeMatch));
			return services;
		}

		/// <summary>
		/// Gets all services of a given interface type.
		/// </summary>
		/// <param name="serviceDictionary">One of us.</param>
		/// <param name="interfaceType">type to look for.</param>
		/// <param name="exactTypeMatch">
		/// Allow derived interfaces. Default = <see langword="true"/>
		/// </param>
		/// <returns>
		/// Found services or <see langword="null"/>.
		/// </returns>
		public static ICollection<IPAFServiceDescription> GetServiceInterfacesOfType(
			this IPAFServiceDictionary serviceDictionary,
			Type interfaceType, bool exactTypeMatch = true)
		{
			// If an exact type match, the job is easy....
			if (exactTypeMatch)
			{
				var nto = new PAFNamedAndTypedObject(interfaceType);

				var innerDict = serviceDictionary[nto];
			    var services = innerDict?.Values;
				return services?.BuildCollection();
			}
			// Otherwise we gotta' go through every sucker in the dict.
			else
			{
				Collection<IPAFServiceDescription> nonGeneric = null;
				var services = serviceDictionary.GetAllServices();
				foreach (var service in services)
				{
					if (service.ServiceImplementationType == null) continue;
					if (service.ServiceImplementationType.TypeType.DoesTypeImplementInterface(interfaceType))
					{
						if (nonGeneric == null) nonGeneric = new Collection<IPAFServiceDescription>();
						nonGeneric.Add(service);
					}
				}
				return nonGeneric;
			}
		}
		/// <summary>
		/// For the generic dictionary/manager implementation. Selects Generic services
		/// of a given type from the dictionary.
		/// </summary>
		/// <param name="serviceDictionary">
		/// Dictionary to pull the Generic services out of.
		/// </param>
		/// <param name="exactTypeMatch">
		/// Allow derived interfaces. Default = <see langword="true"/>
		/// </param>
		/// <typeparam name="T">Type constrained to be a service and a class.</typeparam>
		/// <returns>
		/// Collection of Generic services or <see langword="null"/>
		/// </returns>
		public static ICollection<IPAFServiceDescription<T>> GetTypedServiceDescriptions<T>
			(this IPAFServiceDictionary serviceDictionary, bool exactTypeMatch = true) where T : class, IPAFService
		{
			Collection<IPAFServiceDescription<T>> typedServiceList = null;

			var services = serviceDictionary.GetServiceInterfacesOfType(typeof (T), exactTypeMatch);

			if (services == null) return null;

			foreach (var service in services)
			{
				IPAFServiceDescription<T> typedService;
				if ((typedService = (service as IPAFServiceDescription<T>)) != null)
				{
					if (typedServiceList == null)
						typedServiceList
							= new Collection<IPAFServiceDescription<T>>();

					typedServiceList.Add(typedService);
				}
			}
			return typedServiceList;
		}

		/// <summary>
		/// Finds all unconstructed services in a service dictionary.
		/// </summary>
		/// <param name="serviceDictionary">Dictionary to search.</param>
		/// <returns>Collection of services needing to be instantiated, or <see langword="null"/>.</returns>
		public static IEnumerable<IPAFServiceDescription> GetUnconstructedServices(
			this IPAFServiceDictionary serviceDictionary)
		{
			Collection<IPAFServiceDescription> unconstructedServices = null;
			if (serviceDictionary == null) return null;
			foreach (var service in serviceDictionary.GetAllServices())
			{
				if (service.ServiceObject == null)
				{
					if (unconstructedServices == null) unconstructedServices
						= new Collection<IPAFServiceDescription>();
					unconstructedServices.Add(service);
				}
			}
			return unconstructedServices;
		}
		/// <summary>
		/// Finds all constructed services in a service dictionary which wear
		/// <see cref="IPAFServiceExtended"/> and have not gone through the appropriate
		/// service lifetime stage.
		/// </summary>
		/// <param name="serviceDictionary">Dictionary to search.</param>
		/// <param name="pipelineStage">Service pipeline stage that service has not undergone.</param>
		/// <returns>Collection of services needing to be acted upon, or <see langword="null"/>.</returns>
		public static ICollection<IPAFServiceExtended> GetUnstagedServices(
			this IPAFServiceDictionary serviceDictionary, ServicePipelineStage pipelineStage)
		{
			Collection<IPAFServiceExtended> unstagedServices = null;
			if (serviceDictionary == null) return null;
			foreach (var service in serviceDictionary.GetAllServices())
			{
				IPAFServiceExtended extendedService;
				if ((extendedService = (service.ServiceObject as IPAFServiceExtended)) != null)
				{
					if ((pipelineStage == ServicePipelineStage.LOAD) && (!extendedService.ServiceIsLoaded))
					{
						if (unstagedServices == null) unstagedServices = new Collection<IPAFServiceExtended>();
						unstagedServices.Add(extendedService);
					}
					if ((pipelineStage == ServicePipelineStage.INITIALIZE) && (!extendedService.ServiceIsInitialized))
					{
						if (unstagedServices == null) unstagedServices = new Collection<IPAFServiceExtended>();
						unstagedServices.Add(extendedService);
					}
				}
			}
			return unstagedServices;
		}

		/// <remarks>
		/// See <see cref="IPAFServiceDictionary.GetService"/>. This extension
		/// method has exactly the same functionality without the exceptions.
		/// Factored into an extension method so it can be used in anybody's
		/// dictionary. No exceptions are generated or caught.
		/// </remarks>
		public static IPAFServiceDescription TryLocateService(
			this IPAFServiceDictionary serviceDictionary,
			IPAFNamedAndTypedObject nto,
			bool exactInterfaceTypeMatch = true)
		{
			if (exactInterfaceTypeMatch)
				// This is the easy case.
				return serviceDictionary[nto][nto];

			// Loop to find the correct dictionary.
			foreach (var dict in serviceDictionary.Values)
			{
				if (dict.Count > 0)
				{
					var interfaceType = dict.Keys.GetFirstElement().ObjectType;
					if (TypeHandlingUtils.DoesSecondTypeInheritFromFirstType(nto.ObjectType, interfaceType))
						// Found the right dictionary - get the right name.
						return dict[nto];
				}
			}
			return null;
		}

	}
}
