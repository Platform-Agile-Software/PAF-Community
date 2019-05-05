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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
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
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// A few extensions that are necessary for accessing services as Generics.
	/// </summary>
	/// <threadsafety>
	///  Not thread-safe. Lock dictionary.
	/// </threadsafety>
	/// <remarks>
	/// All of the methods require the interface types in the dictionary
	/// ReSharper disable once InvalidXmlDocComment
	/// to be early-bound - i.e. <see cref="IPAFServiceDescription.ServiceInterfaceType.TypeType"/>
	/// is not <see langword="null"/>.
	/// Note: KRM - <see cref="Type"/> is now a token in the new reflection library. This was
	/// actually what <see cref="IPAFServiceDescription"/> was designed for. Instantiation must
	/// ReSharper disable once InvalidXmlDocComment
	/// now ensure that <see cref="Type.GetTypeInfo()"/> be called. However, our extension methods
	/// here result in calls to this method to load the type.
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>01jan2019 </date>
	/// <description>
	/// Added <see cref="GetServiceImplementations{T}"/> so we can properly search for
	/// other than registered interfaces.
	/// </description>
	/// </contribution>
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
				if (dict == null) continue;

				foreach (var service in dict.Values)
				{
					serviceList.Add(service);
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
		/// <param name="registeredServicesOnly">
		/// <see cref="GetServiceInterfacesOfType"/>
		/// Default = <see langword="false"/> because we generally want to dig out
		/// all implementations. Registered services can be obtained directly from
		/// the outer dictionary.
		/// </param>
		/// <remarks>
		/// Services are returned in sort order.
		/// </remarks>
		/// <threadsafety>
		/// Unsafe lock unsynchronized dictionary before use.
		/// </threadsafety>
		public static IList<IPAFServiceDescription> GetAnyTypedServices(
			this IPAFServiceDictionary serviceDictionary,
			Type interfaceType, bool registeredServicesOnly = false)
		{
			var services = new Collection<IPAFServiceDescription>();

			if (!interfaceType.IsTypeAnInterfaceType()) return null;

			// Query the dictionary.
			services.AddNoDupes(serviceDictionary.GetServiceInterfacesOfType(
				interfaceType, registeredServicesOnly));
			return services;
		}


		/// <summary>
		/// For the generic dictionary/manager implementation. Selects Generic services
		/// that can be cast to a given Generic safely.
		/// </summary>
		/// <param name="serviceDictionary">
		/// Dictionary to pull the Generic services out of.
		/// </param>
		/// <param name="registeredServicesOnly">
		/// <see cref="GetServiceInterfacesOfType"/>
		/// Default = <see langword="false"/> because we generally want to dig out
		/// all implementations. Registered services can be obtained directly from
		/// the outer dictionary.
		/// </param>
		/// <param name="name">
		/// If this is specified, services are filtered by name.
		/// </param>
		/// <typeparam name="T">
		/// Type constrained to be a <see cref="IPAFService"/> and a reference type.
		/// </typeparam>
		/// <returns>
		/// Collection of non-Generic services whose service objects can be safely cast
		/// to <typeparamref name="T"/>.
		/// </returns>
		[NotNull]
		public static ICollection<IPAFServiceDescription> GetServiceImplementations<T>
		(this IPAFServiceDictionary serviceDictionary,
			bool registeredServicesOnly = false, string name = null)
			where T : class, IPAFService
		{
			var services = new Collection<IPAFServiceDescription>();

			// Gather service implementations that implement "T".
			services.AddItems(serviceDictionary.GetServiceInterfacesOfType(typeof(T),
				registeredServicesOnly));

			// Don't care about name?
			if (name == null)
				return services;

			// Filter by name. Note that if we were searching only registered types
			// (exactMatch = true), we would only find one named service, if any.
			var serviceScratch = services.BuildCollection();
			services.Clear();
			foreach (var service in serviceScratch)
			{
				if (service.ServiceName.Equals(name, StringComparison.Ordinal))
					services.Add(service);
			}

			return services;
		}

		/// <summary>
		/// For the generic dictionary/manager implementation. Selects Generic services
		/// of a given type from the dictionary that have already been constructed.
		/// </summary>
		/// <param name="serviceDictionary">
		/// Dictionary to pull the Generic services out of.
		/// </param>
		/// <param name="registeredServicesOnly">
		/// <see cref="GetServiceInterfacesOfType"/>
		/// Default = <see langword="false"/> because we generally want to dig out
		/// all implementations. Registered services can be obtained directly from
		/// the outer dictionary.
		/// </param>
		/// <typeparam name="T">Type constrained to be a service and a class.</typeparam>
		/// <returns>
		/// Collection of Generic services that are active (instantiated/constructed).
		/// </returns>
		[NotNull]
		public static ICollection<T> GetInstantiatedServices<T>
		(this IPAFServiceDictionary serviceDictionary,
			bool registeredServicesOnly = false)
			where T : class, IPAFService
		{
			var discoveredInterfaceList = new Collection<T>();

			// Gather service implementations that implement "T".
			var services = serviceDictionary.GetServiceInterfacesOfType(typeof(T), registeredServicesOnly);
			if (services == null) return discoveredInterfaceList;

			foreach (var service in services)
			{
				// Grab only service descriptions that are constructed.
				// All are guaranteed to be castable to "T".
				if (service.ServiceObject != null)
					discoveredInterfaceList.Add((T)service.ServiceObject);
			}

			return discoveredInterfaceList;
		}

		/// <summary>
		/// Gets all services of a given interface type.
		/// </summary>
		/// <param name="serviceDictionary">One of us.</param>
		/// <param name="interfaceType">type to look for.</param>
		/// <param name="registeredInterfacesOnly">
		/// Search only registered interfaces. Default = <see langword="false"/>. Note that
		/// searching for implemented interfaces other than the type the service
		/// is registered as is a slow process. Services can be registered multiple
		/// times, each corresponding to the type of interface that the service
		/// implements that the client needs to use. If the specific service
		/// interface is needed often, register it.
		/// </param>
		/// <returns>
		/// These are non-Generic descriptions that can have their implementation object
		/// safely cast to the desired Generic type.
		/// </returns>
		[NotNull]
		public static ICollection<IPAFServiceDescription> GetServiceInterfacesOfType(
			this IPAFServiceDictionary serviceDictionary,
			Type interfaceType, bool registeredInterfacesOnly = false)
		{
			var nonGeneric = new Collection<IPAFServiceDescription>();
			// If an exact type match, the job is easy....
			if (registeredInterfacesOnly)
			{
				var nto = new PAFNamedAndTypedObject(interfaceType);

				if (!serviceDictionary.ContainsKey(nto))
					return nonGeneric;

				var innerDict = serviceDictionary[nto];
				var services = innerDict?.Values;
				nonGeneric.AddItems(services);
				return nonGeneric;
			}
			// Otherwise we gotta' go through every sucker in the dict.
			else
			{
				var services = serviceDictionary.GetAllServices();
				foreach (var service in services)
				{
					if (service.ServiceImplementationType == null) continue;
					if (service.ServiceImplementationType.TypeType.DoesTypeImplementInterface(interfaceType))
					{
						nonGeneric.Add(service);
					}
				}

				return nonGeneric;
			}
		}

		/// <summary>
		/// For the generic dictionary/manager implementation. Selects Generic services
		/// of a given type from the dictionary that have been registered according to that type.
		/// </summary>
		/// <param name="serviceDictionary">
		/// Dictionary to pull the Generic services out of.
		/// </param>
		/// <typeparam name="T">Type constrained to be a service and a reference type.</typeparam>
		/// <returns>
		/// Collection of Generic services.
		/// </returns>
		[NotNull]
		public static ICollection<IPAFServiceDescription<T>> GetRegisteredTypedServiceDescriptions<T>
			(this IPAFServiceDictionary serviceDictionary)
			where T : class, IPAFService
		{
			Collection<IPAFServiceDescription<T>> typedServiceList = null;

			var services = serviceDictionary.GetServiceInterfacesOfType(typeof(T));

			if (services == null) return null;

			foreach (var service in services)
			{
				IPAFServiceDescription<T> typedService;

				// Was service registered as type sought?
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
		/// Finds all un-constructed services in a service dictionary.
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
		/// See <see cref="IPAFServiceDictionary"/>
		/// </remarks>
		public static void ReplaceService(this IPAFServiceDictionary serviceDictionary,
			IPAFServiceDescription serviceDescription)
		{
			IDictionary<IPAFNamedAndTypedObject, IPAFServiceDescription> nameDictionary;
			if (serviceDictionary.ContainsKey(serviceDescription))
			{
				nameDictionary = serviceDictionary[serviceDescription];
			}
			else
			{
				nameDictionary = PAFGenericServiceDictionary.NewInnerDictionary();
				serviceDictionary.Add(serviceDescription, nameDictionary);
			}
			if (nameDictionary.ContainsKey(serviceDescription))
			{
				nameDictionary.Remove(serviceDescription);
			}

			// Recall that service is it's own key.
			nameDictionary.Add(serviceDescription, serviceDescription);
		}


		/// <remarks>
		/// See <see cref="IPAFServiceDictionary.GetService"/>. This extension
		/// method has exactly the same functionality without the exceptions.
		/// Factored into an extension method so it can be used in anybody's
		/// dictionary. No exceptions are thrown or caught.
		/// </remarks>
		public static IPAFServiceDescription TryLocateService(
			this IPAFServiceDictionary serviceDictionary,
			IPAFNamedAndTypedObject nto,
			bool exactInterfaceTypeMatch = true)
		{
			if (exactInterfaceTypeMatch)
			{
				if (!serviceDictionary.ContainsKey(nto))
					return null;
				var innerDictionary = serviceDictionary[nto];
				if (!innerDictionary.ContainsKey(nto))
					return null;
				// This is the easy case.
				return innerDictionary[nto];
			}

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
