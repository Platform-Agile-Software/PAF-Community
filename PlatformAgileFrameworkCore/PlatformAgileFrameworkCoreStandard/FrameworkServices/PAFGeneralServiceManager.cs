//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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

#region Using Statements

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

#region Exception shorthand.
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
using PlatformAgileFramework.FrameworkServices.Exceptions;
#endregion // Exception shorthand.

#endregion // Using Statements
namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This Type handles basic services within the PAF system. It is intended
	/// to be used to hold framework services such as file service, resource service
	/// and other basic services. It provides a default implementation of
	/// <see cref="IPAFServiceManager{T}"/> and <see cref="IPAFServiceManager"/>
	/// so that legacy applications that use non-Generic services can be supported
	/// in a single manager.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// This type parameter allows specialization of the service manager to satisfy
	/// requests for service types that are more specific than <see cref="IPAFService"/>.
	/// Normally a "core" or "root" service manager just closes this with
	/// <see cref="IPAFService"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 09jun2019 </date>
	/// <description>
	/// Added "lazy" constructor support so a service description can be defined with the
	/// <see cref="Lazy{T}"/> construct. Needed for new view controller system.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 10oct2018 </date>
	/// <description>
	/// Put "addorreplace" back in to support Golea project.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 21feb2012 </date>
	/// <description>
	/// Made necessary mods to use one base service manager for both remote
	/// and local services - code consolidation.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27jan2012 </date>
	/// <description>
	/// Converted from 3.5 and cleaned up.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	// ReSharper disable PartialTypeWithSinglePart
	//Core
	public partial class PAFGeneralServiceManager<T>
		: PAFServiceManager, IPAFServiceManagerInternal<T>
		// ReSharper restore PartialTypeWithSinglePart
		where T : class, IPAFService
	{
		/// <summary>
		/// Generic version.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See non-Generic version.
		/// </param>
		/// <param name="serviceDescription">
		/// See non-Generic version.
		/// </param>
		/// <param name="typeFilter">
		/// See non-Generic version.
		/// </param>
		/// <param name="assemblyList">
		/// See non-Generic version.
		/// </param>
		/// <returns>
		/// See non-Generic version.
		/// </returns>
		public delegate IPAFServiceDescription<U> PAFLocalServiceInstantiator1<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription serviceDescription, IPAFTypeFilter typeFilter = null,
			IEnumerable<Assembly> assemblyList = null) where U : class, T;
		#region Class Fields And Autoproperties
		///// <summary>
		///// Pluggable service instantiator. Immutable field needs no synchronization.
		///// </summary>
		//protected internal readonly PAFLocalServiceInstantiator1<T>
		//	m_TypedLocalServiceInstantiator;
		#endregion // Class Fields And Autoproperties
		#region Constructors

		/// <summary>
		/// Constructor that gives the service manager a name and a set of
		/// initial services. Noted that services are not initialized as they
		/// are placed into the service manager.
		/// </summary>
		/// <param name="guid">
		/// The key that the class instantiator supplies so that the instantiator
		/// is the only one that can dispose the instance. The default
		/// for no argument supplied is "default(Guid)" which is the very
		/// same as <see cref="Guid.Empty"/>.
		/// </param>
		/// <param name="serviceManagerType">
		/// A type for the service. Under almost all circumstances, this should be
		/// an interface type. This should follow the same convention as any other
		/// service. See base.
		/// </param>
		/// <param name="serviceManagerName">
		/// Name for a named service manager or blank or <see langword="null"/> for no name.
		/// </param>
		/// <param name="services">
		/// Set of services to install.
		/// </param>
		/// <param name="serviceCreator">
		/// Loads <see cref="PAFServiceManager.ServiceCreator"/>. Default = <see langword="null"/>
		/// causes <see cref="PAFServiceManager.DefaultServiceCreator"/> to be used.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// Loads <see cref="PAFServiceManager.LocalServiceInstantiator"/>. Default = <see langword="null"/>
		/// causes <see cref="PAFServiceManager.DefaultLocalServiceInstantiator"/> to be used.
		/// </param>
		[SecurityCritical]
		protected PAFGeneralServiceManager(
			Type serviceManagerType = null, string serviceManagerName = null,
			Guid guid = default(Guid),
			IEnumerable<IPAFServiceDescription<T>> services = null,
			PAFServiceCreator serviceCreator = null,
			//PAFLocalServiceInstantiator1<T> typedLocalServiceInstantiator = null,
			PAFLocalServiceInstantiator localServiceInstantiator = null)
			: this(0,  serviceManagerType, serviceManagerName, guid, services,
			serviceCreator, localServiceInstantiator)
		{
			//m_TypedLocalServiceInstantiator = typedLocalServiceInstantiator;
		}

		// ReSharper disable CSharpWarnings::CS1580
		/// <summary>
		/// Constructor that gives the service manager a name and a set of
		/// initial services. Internal version of
		/// <see cref="PAFGeneralServiceManager{T}"/>
		/// </summary>
		/// <param name="myFakeInternalConstructorArgument">
		/// Just exists so we can have another internal constructor that does
		/// the same thing as the public one. Doesn't do a thing.
		/// </param>
		/// <param name="guid">
		/// <see cref="PAFGeneralServiceManager{T}"/>
		/// </param>
		/// <param name="serviceManagerType">
		/// <see cref="PAFGeneralServiceManager{T}"/>
		/// </param>
		/// <param name="serviceManagerName">
		/// <see cref="PAFGeneralServiceManager{T}"/>
		/// </param>
		/// <param name="services">
		/// <see cref="PAFServiceManager"/>
		/// </param>
		/// <param name="serviceCreator">
		/// Loads <see cref="PAFServiceManager.ServiceCreator"/>. Default = <see langword="null"/>
		/// causes <see cref="PAFServiceManager.DefaultServiceCreator"/> to be used.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// Loads <see cref="PAFGeneralServiceManager{T}.m_TypedLocalServiceInstantiator"/>.
		/// Default = <see langword="null"/>
		/// causes <see cref="PAFGeneralServiceManager{T}.DefaultTypedLocalServiceInstantiator"/> to be used.
		/// </param>
		/// <param name="typedLocalServiceInstantiator">
		/// Loads <see cref="PAFServiceManager.LocalServiceInstantiator"/>.
		/// Default = <see langword="null"/>
		/// causes <see cref="PAFServiceManager.DefaultLocalServiceInstantiator"/> to be used.
		/// </param>
		/// <param name="typedServiceCreator">
		/// This is the service creator for Generically typed services.
		/// </param>
		// ReSharper restore CSharpWarnings::CS1580
		// ReSharper disable UnusedParameter.Local
		protected internal PAFGeneralServiceManager(int myFakeInternalConstructorArgument,
			Type serviceManagerType = null, string serviceManagerName = null,
			Guid guid = default(Guid),
			IEnumerable<IPAFServiceDescription<T>> services = null,
			PAFServiceCreator serviceCreator = null,
			PAFLocalServiceInstantiator localServiceInstantiator = null,
			PAFLocalServiceInstantiator<T> typedLocalServiceInstantiator = null,
			PAFServiceCreator<T> typedServiceCreator = null)
			: base(serviceManagerType, serviceManagerName, guid)
		// ReSharper restore UnusedParameter.Local			
		{
		}
		#endregion
		#region Novel Members
		#region Novel Properties
		/// <summary>
		/// Just an accessor for the service list.
		/// </summary>
		protected internal virtual IEnumerable<IPAFServiceDescription>
			ServiceArrayPIV
		{
			get
			{
				using (var dict = m_ServiceDictionaryWrapper.GetReadLockedObject())
				{
					return dict.ReadLockedNullableObject.GetRegisteredTypedServiceDescriptions<T>();
				}
			}
		}
		#endregion // Novel Properties
		#region Novel Methods
		/// <remarks>
		/// Backing for the interfaces.
		/// </remarks>
		protected internal virtual bool AddOrReplaceServicePIV<U>(IPAFServiceDescription<U> iFservice)
			where U : class, T
		{
			using (var manipulator = m_ServiceDictionaryWrapper.GetWriteLockedObject())
			{
				// Bit of syntax clarity.....
				var outerServiceDictionary = manipulator.WriteLockedNullableObject;

				var retval = RemoveServiceIfExistsPIV(iFservice, outerServiceDictionary);

				AddServiceHelper(iFservice);

				return retval;
			}
		}

		/// <remarks>
		/// Backing for the interfaces.
		/// </remarks>
		protected virtual void AddServicePV<U>(IPAFServiceDescription<U> iFservice) where U : class, T
		{
			AddServiceHelper(iFservice);
		}
		/// <remarks>
		/// Backing for the interfaces.
		/// </remarks>
		protected virtual void AddServicesPV<U>(IEnumerable<IPAFServiceDescription<U>> iFservices) where U : class, T
		{
			foreach (var iFservice in iFservices)
			{
				AddServicePV(iFservice);
			}
		}
		/// <remarks>
		/// Backing for the interfaces.
		/// </remarks>
		protected virtual IEnumerable<IPAFServiceDescription> GetServiceDescriptionsPV<U>()
			where U : class, T
		{
			return GetAnyServiceDescriptionsPV<U>();
		}
		/// <remarks>
		/// Backing for the interfaces. This method has been updated to deal with lazy constructors.
		/// </remarks>
		[CanBeNull]
		protected virtual U GetTypedServicePV<U>(string serviceName = "",
			bool registeredTypesOnly = true, object securityObject = null)
			where U : class, T
		{
			var returnedServices = new Collection<U>();

			using (var dict = m_ServiceDictionaryWrapper.GetReadLockedObject())
			{
				// Get services that implement U.
				var implementingServices
					= dict.ReadLockedNullableObject.GetServiceImplementations<U>
						(registeredTypesOnly, serviceName);
				var genericImplementingServices
					= implementingServices.EnumIntoSubtypeList<IPAFServiceDescription,IPAFServiceDescription<U>>();

				foreach (var genericImplementingService in genericImplementingServices)
				{
					// If we are an extended service, we don't return ourselves unless
					// we are initialized.
					IPAFServiceExtended extendedService;
					if (
						(genericImplementingService.ServiceObject != null)
						&&
						(extendedService = (genericImplementingService.ServiceObject as IPAFServiceExtended)) != null)
					{
						if (extendedService.ServiceIsInitialized)
							returnedServices.AddNoNulls((U)genericImplementingService.ServiceObject);
					}
					else
					{
						if (genericImplementingService.Service == null)
						{
							if (genericImplementingService.ServiceBuilder != null)
								genericImplementingService.Service = genericImplementingService.ServiceBuilder.Value;
						}

						returnedServices.AddNoNulls((U)genericImplementingService.ServiceObject);
					}
				}
			}

			return returnedServices.GetFirstElement();
		}
		/// <remarks>
		/// Backing for the interfaces.
		/// </remarks>
		//TODO arg exceptions.
		//TODO DOCUMENT exceptions.
		protected virtual void MakeServiceDefaultForInterfacePV<U>(
			IPAFServiceDescription<U> iFservice) where U : class, T
		{
			using (var manipulator = m_ServiceDictionaryWrapper.GetWriteLockedObject())
			{
				// Bit of syntax clarity.....
				var outerServiceDictionary = manipulator.WriteLockedNullableObject;
				// Fetch only registered services (true).
				// This is a check to see if the service exists somewhere
				// in the dictionary.
				var newDefaultService = outerServiceDictionary.GetService(iFservice, true);
				if (newDefaultService == null)
				{
					throw new PAFStandardException<IPAFSED>(new PAFSED(iFservice),
						PAFServiceExceptionMessageTags.SERVICE_NOT_FOUND);
				}

				var innerServiceDictionary = outerServiceDictionary[iFservice];

				// By construction of the dictionary, service is guaranteed to be here.
				var innerServiceList = innerServiceDictionary.BuildCollection();
				var oldDefaultService = innerServiceList[0];
				innerServiceDictionary.Remove(oldDefaultService.Key);

				// Here is a bit of ugliness we have for maintaining backward compatibility
				// with non-Generic SM.
				var newDefaultServiceNTO = iFservice.GetServiceNTO();
				innerServiceDictionary.Remove(newDefaultServiceNTO);

				// Clear the "default" bit.
				((IPAFServiceDescriptionInternal) (oldDefaultService.Value)).SetIsDefault(false);

				// Set "default" bit on new default service.
				((IPAFServiceDescriptionInternal)newDefaultService).SetIsDefault(true);

				// Add services back in back in to trigger resort.
				innerServiceDictionary.Add(oldDefaultService);
				innerServiceDictionary.Add(newDefaultServiceNTO, newDefaultService);
			}
		}
		/// <remarks>
		/// Backing for the interfaces.
		/// </remarks>
		protected internal virtual bool RemoveServiceIfExistsPIV<U>(IPAFServiceDescription<U> iFservice)
			where U : class, T
		{
			using (var manipulator = m_ServiceDictionaryWrapper.GetWriteLockedObject())
			{
				// Bit of syntax clarity.....
				var outerServiceDictionary = manipulator.WriteLockedNullableObject;

				var retval = RemoveServiceIfExistsPIV(iFservice, outerServiceDictionary);

				return retval;
			}
		}

		/// <summary>
		/// Helper to remove a service from the manager, if present. Only registered
		/// services are found.
		/// </summary>
		/// <typeparam name="U">Type of the service.</typeparam>
		/// <param name="iFservice">The service to be removed, if found.</param>
		/// <param name="serviceDictionary">
		/// The dictionary to be searched.
		/// </param>
		/// <threadsafety>Unsafe. Must be used inside the lock.</threadsafety>
		//TODO arg exceptions.
		//TODO DOCUMENT exceptions.
		protected internal virtual bool RemoveServiceIfExistsPIV<U>(
			IPAFServiceDescription<U> iFservice, IPAFServiceDictionary serviceDictionary)
			where U : class, T
		{
			// Fetch only interfaces with an exact type match (true).
			// This is a check to see if the service exists somewhere
			// in the dictionary.
			var foundService = serviceDictionary.GetService(iFservice, true);
			if (foundService == null)
			{
				return false;
			}

			var innerServiceDictionary = serviceDictionary[iFservice];
			// By construction of the dictionary sorter, service is guaranteed to be here.
			innerServiceDictionary.Remove(iFservice);
			return true;
		}
		#endregion // Novel Methods
		#endregion // Novel Members
		#region Implementation of IPAFServiceManager{T}
		#region Methods

		#region Implementation of IPAFServiceManager<in T>
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// </remarks>
		void IPAFServiceManager<T>.AddTypedService<U>(U service)
		{
			var description = new PAFServiceDescription<U>(service, "");
			AddServicePV(description);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// </remarks>
		void IPAFServiceManager<T>.AddTypedService<U>(U service, string serviceName)
		{
			var description = new PAFServiceDescription<U>(service, serviceName);
			AddServicePV(description);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// </remarks>
		void IPAFServiceManager<T>.AddTypedService<U>(Lazy<U> serviceBuilder)
		{
			var description = new PAFServiceDescription<U>(serviceBuilder, "");
			AddServicePV(description);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// </remarks>
		void IPAFServiceManager<T>.AddTypedService<U>(Lazy<U> serviceBuilder, string serviceName)
		{
			var description = new PAFServiceDescription<U>(serviceBuilder, serviceName);
			AddServicePV(description);
		}
		#endregion
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// The default service is identified by a blank (<see cref="string.Empty"/>)
		/// name or by being first in the set of installed services of a given type.
		/// </remarks>
		U IPAFServiceManager<T>.GetTypedService<U>(string serviceName,
			bool registeredServicesOnly, object securityObject)
		{
			return GetTypedServicePV<U>(serviceName, registeredServicesOnly,
				securityObject);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// </remarks>
		U IPAFServiceManager<T>.GetTypedService<U>(bool registeredServicesOnly,
			object securityObject)
		{
			return GetTypedServicePV<U>("", registeredServicesOnly, securityObject);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// </remarks>
		U IPAFServiceManager<T>.GetTypedService<U>(string serviceName)
		{
			return GetTypedServicePV<U>(serviceName);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManager{T}"/>.
		/// </remarks>
		U IPAFServiceManager<T>.GetTypedService<U>()
		{
			return GetTypedServicePV<U>();
		}
		#endregion // Methods
		#endregion // Implementation of IPAFServiceManager{T}
		#region Implementation of IPAFServiceManagerExtended
		#region Properties
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		IEnumerable<IPAFServiceDescription>
			IPAFServiceManagerExtended<T>.ServiceArray
		{
			[SecurityCritical]
			get { return ServiceArrayPIV; }
		}
		#endregion
		#region Methods
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// Checks to see if it is a pure interface type and ignores it if it is.
		/// This avoids the problem that occurs when scanning an assembly through
		/// refection for IPAFFrameworkServices and loading them all - some of them
		/// may be just interfaces and bad things happen when we try to instantiate them!!
		/// </remarks>
		[SecurityCritical]
		void IPAFServiceManagerExtended<T>.AddService<U>(IPAFServiceDescription<U> iFservice)
		{
			AddServicePV(iFservice);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		[SecurityCritical]
		void IPAFServiceManagerExtended<T>.AddServices<U>(IEnumerable<IPAFServiceDescription<U>> iFservices)
		{
			AddServicesPV(iFservices);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		[SecurityCritical]
		IPAFServiceDescription<U> IPAFServiceManagerExtended<T>.CreateService<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription<U> serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator<U> localServiceInstantiator)
		{
			return CreateServicePV(servicePipelineObject, serviceDescription,
									localServiceInstantiator, typeFilter);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		[SecurityCritical]
		IEnumerable<IPAFServiceDescription> IPAFServiceManagerExtended<T>.GetServiceDescriptions<U>()
		{
			return GetServiceDescriptionsPV<U>();
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		[SecurityCritical]
		IPAFServiceDescription<U> IPAFServiceManagerExtended<T>.InstantiateLocalService<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription serviceDescription, IPAFTypeFilter typeFilter)
		{
			return InstantiateLocalServicePV<U>(servicePipelineObject,
				serviceDescription, typeFilter);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		[SecurityCritical]
		void IPAFServiceManagerExtended<T>.MakeServiceDefaultForInterface<U>(
			IPAFServiceDescription<U> iFservice)
		{
			MakeServiceDefaultForInterfacePV(iFservice);
		}
		#endregion // Methods
		#endregion // Implementation of IPAFServiceManagerExtended
		#region Implementation of IPAFServiceManagerInternal
		#region Properties
		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		IEnumerable<IPAFServiceDescription> IPAFServiceManagerInternal<T>.ServiceArrayInternal
		{
			get { return ServiceArrayPIV; }
		}

		#endregion
		#region Methods
		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		bool IPAFServiceManagerInternal<T>.AddOrReplaceServiceInternal<U>(IPAFServiceDescription<U> iFservice)
		{
			return AddOrReplaceServicePIV(iFservice);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		void IPAFServiceManagerInternal<T>.AddServiceInternal<U>(IPAFServiceDescription<U> iFservice)
		{
			AddServicePV(iFservice);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		void IPAFServiceManagerInternal<T>.AddServicesInternal<U>(IEnumerable<IPAFServiceDescription<U>> iFservices)
		{
			AddServicesPV(iFservices);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		IPAFServiceDescription<U> IPAFServiceManagerInternal<T>.CreateServiceInternal<U>
			(IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription<U> serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator<U> localServiceInstantiator)
		{
			return CreateServicePV(servicePipelineObject, serviceDescription,
				localServiceInstantiator, typeFilter);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		IEnumerable<IPAFServiceDescription> IPAFServiceManagerInternal<T>.GetServiceDescriptionsInternal<U>()
		{
			return GetServiceDescriptionsPV<U>();
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		IPAFServiceDescription<U> IPAFServiceManagerInternal<T>.InstantiateLocalServiceInternal<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription serviceDescription, IPAFTypeFilter typeFilter)
		{
			return InstantiateLocalServicePV<U>(servicePipelineObject,
				serviceDescription, typeFilter);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		void IPAFServiceManagerInternal<T>.MakeServiceDefaultForInterfaceInternal<U>(
			IPAFServiceDescription<U> iFservice)
		{
			MakeServiceDefaultForInterfacePV(iFservice);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal{T}"/>.
		/// </remarks>
		bool IPAFServiceManagerInternal<T>.RemoveServiceInternal<U>(IPAFServiceDescription<U> iFservice)
		{
			return RemoveServiceIfExistsPIV(iFservice);
		}
		#endregion // Methods
		#endregion // Implementation of IPAFServiceManagerInternal
		#region Class Helper Methods
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// This version in Core only builds services with default constructors.
		/// Common implementation for both interfaces. safe/critical so we can
		/// load stuff.
		/// </remarks>
		[SecuritySafeCritical]
		protected virtual IPAFServiceDescription<U> CreateServicePV<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription<U> serviceDescription,
			PAFLocalServiceInstantiator<U> localServiceInstantiator = null,
			IPAFTypeFilter typeFilter = null) where U : class, T

		{
			Exception caughtException = null;
			IPAFServiceDescription<U> createdService = null;
			try
			{
				// KRM hook up full instantiator.
				createdService = DefaultTypedServiceCreator(servicePipelineObject,
					serviceDescription, typeFilter);
			}
			catch (Exception ex)
			{
				caughtException = ex;
			}
			if (createdService == null)
			{
				var data = new PAFSED(null);
				throw new PAFStandardException<IPAFSED>(data, PAFServiceExceptionMessageTags.SERVICE_CREATION_FAILED, caughtException);
			}
			return createdService;
		}
		/// <summary>
		/// This is a helper method  It retrieves a specified service by type.
		/// </summary>
		/// <returns>
		/// The found <see cref="IPAFService"/>.
		/// </returns>
		[CanBeNull]
		protected internal virtual IPAFServiceDescription<U> GetAnyServiceDescription<U>()
			where U : class, T
		{
			var serviceList = GetAnyServiceDescriptionsPV<U>();
			return serviceList.GetFirstElement();
		}
		/// <summary>
		/// This is a helper method. It retrieves a set of registered services by type, irrespective
		/// of name.
		/// </summary>
		/// <returns>
		/// The found <typeparamref name="T"/>s, or an empty collection.
		/// </returns>
		[NotNull]
		protected virtual ICollection<IPAFServiceDescription<U>>
			GetAnyServiceDescriptionsPV<U>() where U : class, T
		{
			var services = new Collection<IPAFServiceDescription<U>>();

			if (!typeof(U).IsTypeAnInterfaceType()) return services;

			// Query the dictionary.
			using (var accessor = m_ServiceDictionaryWrapper.GetReadLockedObject())
			{
				// Bit of syntax clarity.....
				var serviceDictionary = accessor.ReadLockedNullableObject;
				services.AddNoDupes(serviceDictionary.GetRegisteredTypedServiceDescriptions<U>());
			}
			return services;
		}
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// This method only builds services with default constructors.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </returns>
		/// <remarks>
		/// Common implementation for both interfaces.
		/// </remarks>
		protected virtual IPAFServiceDescription<U> InstantiateLocalServicePV<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription serviceDescription,
			IPAFTypeFilter typeFilter = null)
			where U: class, T
		{
			//if (m_TypedLocalServiceInstantiator != null)
			//{
			//	return m_TypedLocalServiceInstantiator(servicePipelineObject, serviceDescription,
 
			//		typeFilter);
			//}
			return DefaultTypedLocalServiceInstantiator<U>(servicePipelineObject, serviceDescription, typeFilter);
		}
		#region Static Helper Methods
		/// <summary>
		/// This method requests a specific service by its name.
		/// </summary>
		/// <param name="serviceName">
		/// Textual name of the service.
		/// </param>
		/// <param name="iFServiceDescriptions">
		/// Set of <see cref="IPAFServiceDescription{T}"/>s.
		/// </param>
		/// <returns>
		/// The found <see cref="IPAFService"/>.
		/// </returns>
		/// <remarks>
		/// This method is handy when used in conjunction with GetServices to get
		/// an array of services that inherit from a given Type. After gathering
		/// an array of services, those services can then be checked by name.
		/// </remarks>
		protected internal static IPAFServiceDescription<T> GetNamedServiceDescription(
			string serviceName, IEnumerable<IPAFServiceDescription<T>> iFServiceDescriptions)
		{
			// Scan incoming services to check names.
			foreach (var iFservice in iFServiceDescriptions)
			{
				var name = iFservice.ObjectName;
				if ((!(string.IsNullOrEmpty(name))) && (name == serviceName))
				{
					return iFservice;
				}
			}
			return null;
		}
		#endregion // Static Helper Methods
		/// <summary>
		/// Creates a service from loaded assemblies. This method only builds
		/// services with default constructors.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// Callees need <see cref="IPAFServicePipelineObject.ServiceManager"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// Standard service description.
		/// </param>
		/// <param name="typeFilter">
		/// Optional filter on the type. Default = <see langword="null"/>.
		/// </param>
		/// <param name="assemblyList">
		/// Optional list of assemblies to constrain the search to. Default = <see langword="null"/>
		/// causes all assemblies in current "AppDomain" to be searched.
		/// </param>
		/// <returns>
		/// A constructed service, never <see langword="null"/>.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> for <paramref name="servicePipelineObject"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException"> for <paramref name="serviceDescription"/>.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFSED}">
		/// <see cref="PAFServiceExceptionMessageTags.MULTIPLE_IMPLEMENTATIONS_FOUND"/> if the service discovery process
		/// discovers multiple implementations.
		/// </exception>
		/// </exceptions>
		/// 		// TODO - KRM - Need to make all type filters deep-copyable.
		public virtual IPAFServiceDescription<U> DefaultTypedLocalServiceInstantiator<U>(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription, IPAFTypeFilter typeFilter = null,
			IEnumerable<Assembly> assemblyList = null) where U: class, T
		{
			var desc = LocalServiceInstantiator(servicePipelineObject, serviceDescription,
				typeFilter, assemblyList);
			return (IPAFServiceDescription<U>)desc;
		}

		/// <summary>
		/// Creates a service from loaded assemblies. This method only builds
		/// services with default constructors.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// Callees need <see cref="IPAFServicePipelineObject.ServiceManager"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// Standard service description.
		/// </param>
		/// <param name="typeFilter">
		/// Optional filter on the type. Default = <see langword="null"/>.
		/// </param>
		/// <param name="assemblyList">
		/// Optional list of assemblies to constrain the search to. Default = <see langword="null"/>
		/// causes all assemblies in current "AppDomain" to be searched.
		/// </param>
		/// <returns>
		/// A constructed service, never <see langword="null"/>.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> for <paramref name="servicePipelineObject"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException"> for <paramref name="serviceDescription"/>.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFSED}">
		/// <see cref="PAFServiceExceptionMessageTags.MULTIPLE_IMPLEMENTATIONS_FOUND"/> if the service discovery process
		/// discovers multiple implementations.
		/// </exception>
		/// </exceptions>
		/// 		// TODO - KRM - Need to make all type filters deep-copyable.
		public IPAFServiceDescription<U> DefaultTypedServiceCreator<U>(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription<U> serviceDescription, IPAFTypeFilter typeFilter = null,
			IEnumerable<Assembly> assemblyList = null) where U : class, T

		{
			var desc = DefaultServiceCreator(servicePipelineObject, serviceDescription,
				typeFilter, DefaultLocalServiceInstantiator);
			return (IPAFServiceDescription<U>)desc;
		}

		#endregion // Class Helper Methods
	}

}
