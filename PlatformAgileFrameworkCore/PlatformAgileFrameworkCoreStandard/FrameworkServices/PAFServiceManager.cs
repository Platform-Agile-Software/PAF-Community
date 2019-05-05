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
using System.Reflection;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.FrameworkServices.Exceptions;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.Manufacturing.Exceptions;
using PlatformAgileFramework.MultiProcessing.Threading.Delegates;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

#region Exception Shorthand
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
using PAFSEDB = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionDataBase;
#endregion // Exception Shorthand



namespace PlatformAgileFramework.FrameworkServices
{
    /// <summary>
    /// <para>
    /// This class is a base implementation of the <see cref="IPAFServiceManager"/>
    /// interface for use through inheritance. The class also contains necessary
    /// methods for startup and shutdown of the manager by deriving from
    /// <see cref="PAFService"/>.
    /// </para>
    /// </summary>
    /// <history>
    /// <contribution>
    /// <author> JAW(P) </author>
    /// <date> 05mar2016 </date>s
    /// <description>
    /// Rewrote to exclude stuff not in PCL profile 344 to support Xamarin.Forms.
    /// </description>
    /// </contribution>
    /// <contribution>
    /// <author> DAP </author>
    /// <date> 07jan2012 </date>
    /// <description>
    /// Rewrote the class to provide a base class for supporting all scenarios
    ///  - core/extended and local/remote.
    /// </description>
    /// </contribution>
    /// </history>
    /// <threadsafety>
    /// Safe.
    /// </threadsafety>
    // ReSharper disable PartialTypeWithSinglePart
    // core part. Does not support lazy loading of individual services. All installed
    // services are staged at construction time of the manager. Services
    // added "on-the-fly" are immediately staged by the manager.
     public partial class PAFServiceManager : PAFServiceExtended,
		IPAFServiceManagerInternal
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties
		/////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This is an optional set of services that can be loaded by an "AppDomain"
		/// creator if these are to be created when the service first starts. Usually loaded
		/// by a worker if done from afar. This is also an extensibility point for framework
		/// builders. Basic low-level services that are platform-specific can be pre-created
		/// and loaded here at application start. In core, these types must have parameter-less
		/// constructors.
		/// </summary>
		public static IList<IPAFServiceDescription> InitialServices
		{
			get { return InitialServicesInternal; }
			[SecurityCritical]
			set { InitialServicesInternal = value; }
		}

		protected internal static IList<IPAFServiceDescription> InitialServicesInternal
		{ get; set; }

		/// <summary>
		/// A dictionary to handle the services.
		/// </summary>
		protected internal IPAFNullableSynchronizedWrapper<IPAFServiceDictionary>
			m_ServiceDictionaryWrapper
				= new NullableSynchronizedWrapper<IPAFServiceDictionary>();

		/// <summary>
		///  Backing for ParentManagerPIV.
		/// </summary>
		protected internal IPAFServiceManager m_ParentManager;

		/// <summary>
		/// Backing.
		/// </summary>
		protected internal virtual IPAFServiceManager ParentManagerPIV { get; set; }

		/// <summary>
		/// This is normally loaded at/after construction time for any instance of a
		/// service manager. For a manager that is not the root manager, it is
		/// normally pointed at the root manager, which is already constructed.
		/// </summary>
		internal IPAFServiceManagerInternal<IPAFService> m_RootServiceManagerInternal;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor loads name and type of the manager WHEN
		/// CONSIDERED AS A SERVICE and an optional disposal guid.
		/// </summary>
		/// <param name="guid">
		/// The key that the class instantiator supplies so that the instantiator
		/// is the only one that can dispose the instance. The default
		/// for no argument supplied is "default(Guid)" which is the very
		/// same as <see cref="Guid.Empty"/>.
		/// </param>
		/// <param name="serviceType">
		/// A type for the manager AS A SERVICE. Under almost all circumstances, this should be
		/// an interface type. If the type is the same as another service installed in
		/// a service manager, the name must be different. If the parameter is
		/// <see langword="null"/> the type of "this" is used. Default is the type
		/// of "this". It is best to hide the type of "this" and register the service
		/// by its interface type.
		/// </param>
		/// <param name="serviceManagerName">
		/// A name for the manager AS A SERVICE that is unique within its <paramref name="serviceType"/>.
		/// or <see langword="null"/> or blank. <see langword="null"/> or blank indicates the default service
		/// for the service type. This is entirely adequate for simple scenarios where there
		/// is only one service of a given type. However, there must be only one default
		/// service or the service manager will throw an exception. Default = blank, which
		/// is what is installed in the dictionary. Service managers that allow multiple
		/// instances of the same service type often employ a factory to auto-generate
		/// names.
		/// </param>
		/// <param name="parentManager">
		/// Optional parent when a service manager is in a hierarchy.
		/// </param>
		/// <param name="services">
		/// Set of services to install.
		/// </param>
		/// <param name="serviceCreator">
		/// Loads <see cref="ServiceCreator"/>. Default = <see langword="null"/>
		/// causes <see cref="DefaultServiceCreator"/> to be used.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// Loads <see cref="LocalServiceInstantiator"/>. Default = <see langword="null"/>
		/// causes <see cref="DefaultLocalServiceInstantiator"/> to be used.
		/// </param>
		protected internal PAFServiceManager(
			Type serviceType = null, string serviceManagerName = "",
			Guid guid = default(Guid),
			IPAFServiceManager parentManager = null,
			IEnumerable<IPAFServiceDescription> services = null,
			PAFServiceCreator serviceCreator = null,
			PAFLocalServiceInstantiator localServiceInstantiator = null)
			: base(serviceType, serviceManagerName, guid)
		{
			// Initialize our stuff.
			m_ServiceDictionaryWrapper.NullableObject
				= new PAFGenericServiceDictionary();

			m_ParentManager = parentManager;

			// Get the proper service builders, default or custom.
			ServiceCreator = DefaultServiceCreator;
			LocalServiceInstantiator = DefaultLocalServiceInstantiator;
			if (serviceCreator != null)
				ServiceCreator = serviceCreator;
			if (localServiceInstantiator != null)
				LocalServiceInstantiator = localServiceInstantiator;

			// Bring in the services if we got 'em.
			if (services == null) return;
			foreach (var t in services)
			{
				AddServiceHelper(t);
			}
		}
		#endregion
		#region IPAFService Overrides
		/// <summary>
		/// This method initializes the service manager by initializing all contained
		/// services, then calling the base.InitializeService to broadcast the event.
		/// </summary>
		protected override void InitializeServicePV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			InitializeServicesPIV(null);
			base.InitializeServicePV(servicePipelineObject);
		}
		/// <summary>
		/// This method loads the service manager by loading all contained
		/// services, then calling the base.LoadService to broadcast the event.
		/// </summary>
		protected override void LoadServicePV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			LoadServicesPIV(null);
			base.LoadServicePV(servicePipelineObject);
		}
		#endregion // IPAFService Overrides
		#region Novel Members
		#region Properties
		/// <summary>
		/// This is provided from the root manager if it is under construction
		/// and just returns the static root manager if we are anything
		/// but the root manager. This is the standard implementation. Others
		/// are possible with non-static root managers. (customer requirement).
		/// </summary>
		protected virtual IPAFServiceManagerExtended<IPAFService>
			RootServiceManager
		{
			get { return RootServiceManagerInternal; }
		}
		/// <summary>
		/// See <see cref="RootServiceManager"/>.
		/// </summary>
		internal virtual IPAFServiceManagerInternal<IPAFService>
			RootServiceManagerInternal
		{
			get { return m_RootServiceManagerInternal; }
		}
		/// <summary>
		/// Pluggable service creator. Immutable field needs no synchronization.
		/// </summary>
		/// <threadsafety>
		/// Set only during construction, please!
		/// </threadsafety>
		protected internal PAFServiceCreator ServiceCreator { get; set; }
		/// <summary>
		/// Pluggable service instantiator.
		/// </summary>
		/// <threadsafety>
		/// Set only during construction, please!
		/// </threadsafety>
		protected internal PAFLocalServiceInstantiator LocalServiceInstantiator { get; set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// This version in Core only builds services with default constructors.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </returns>
		/// <remarks>
		/// Common implementation for both interfaces. safe/critical so we can
		/// load stuff.
		/// </remarks>
		[SecuritySafeCritical]
		protected internal virtual IPAFServiceDescription CreateServicePIV(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription,
			PAFLocalServiceInstantiator localServiceInstantiator = null,
			IPAFTypeFilter typeFilter = null)
		{
			Exception caughtException = null;
			IPAFServiceDescription createdService = null;
			try
			{
				createdService = ServiceCreator(servicePipelineObject,
					serviceDescription, typeFilter, localServiceInstantiator);
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
			serviceDescription.ServiceObject = createdService.ServiceObject;
			return serviceDescription;
		}
		#region Service Staging
		/// <summary>
		/// Initializes a set of services "en-masse" after they have been
		/// constructed and loaded. Services are iteratively initialized to
		/// resolve dependencies. If a service needs construction
		/// or loading, it is constructed and/or loaded.
		/// </summary>
		/// <param name="initializeOnThread">
		/// Determines if services are allowed to be initialized on
		/// background threads.
		/// </param>
		protected internal virtual void InitializeServicesPIV(
			bool? initializeOnThread)
		{
			var willIInitializeOnThread = initializeOnThread == true;
			StageDependentServices(RootServiceManager,
				ServicePipelineStage.INITIALIZE, willIInitializeOnThread);
		}
		/// <summary>
		/// Loads a set of services "en-masse" after they have been
		/// constructed. Services are iteratively loaded to
		/// resolve dependencies. If a service needs construction,
		/// it is constructed.
		/// </summary>
		/// <param name="loadOnThread">
		/// Determines if services are allowed to be loaded on
		/// background threads.
		/// </param>
		protected internal virtual void LoadServicesPIV(
			bool? loadOnThread)
		{
			var willLoadOnThread = loadOnThread == true;
			StageDependentServices(RootServiceManager,
				ServicePipelineStage.LOAD, willLoadOnThread);
		}
		#endregion // Service Staging
		#endregion // Methods
		#endregion // Novel Members
		#region IPAFServiceManager Implementation
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended"/>.
		/// </remarks>
		IPAFServiceManager IPAFServiceManagerExtended.ParentManager
		{
			get { return ParentManagerPIV; }
			[SecurityCritical]
			set { ParentManagerPIV = value; }
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManager"/>.
		/// </remarks>
		protected internal virtual IPAFService GetServicePIV(
			Type serviceType, string serviceName = "",
			bool exactTypeMatch = true, object securityObject = null)
		{
			// Query the dictionary.
			using (var accessor = m_ServiceDictionaryWrapper.GetReadLockedObject())
			{
				// Bit of syntax clarity.....
				var serviceDictionary = accessor.ReadLockedNullableObject;
				var services = serviceDictionary.GetAnyTypedServices
					(serviceType, exactTypeMatch);
				var namedService = GetNamedServiceDescription(serviceName, services);
				if (namedService == null)
					return null;
				return (IPAFService)GetNamedServiceDescription(serviceName, services).ServiceObject;
			}
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManager"/>.
		/// </remarks>
		IPAFService IPAFServiceManager.GetService(Type serviceType, string serviceName,
			bool exactTypeMatch, object securityObject)
		{
			return GetServicePIV(serviceType, serviceName, exactTypeMatch, securityObject);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManager"/>.
		/// </remarks>
		IPAFService IPAFServiceManager.GetService(Type serviceType, bool exactTypeMatch,
			object clientObject)
		{
			return GetServicePIV(serviceType, null, exactTypeMatch, clientObject);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManager"/>.
		/// </remarks>
		IPAFService IPAFServiceManager.GetService(Type serviceType, object clientObject)
		{
			return GetServicePIV(serviceType, "", false, clientObject);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManager"/>.
		/// </remarks>
		IPAFService IPAFServiceManager.GetService(Type serviceType)
		{
			return GetServicePIV(serviceType, "", false);
		}
		#endregion // IPAFServiceManager Implementation

		#region IPAFServiceManagerInternal Implementation
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </returns>
		IPAFServiceDescription IPAFServiceManagerInternal.CreateServiceInternal(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator localServiceInstantiator)
		{
			return CreateServicePIV(servicePipelineObject, serviceDescription,
				localServiceInstantiator, typeFilter);
		}

		/// <remarks>
		/// See <see cref="IPAFServiceManagerInternal"/>.
		/// </remarks>
		IPAFServiceManager IPAFServiceManagerInternal.ParentManagerInternal
		{
			get { return ParentManagerPIV; }
			set { ParentManagerPIV = value; }
		}
		#endregion // IPAFServiceManagerInternal Implementation
		#region IPAFServiceManagerExtended Implementation
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		[SecurityCritical]
		IPAFServiceDescription IPAFServiceManagerExtended.CreateService(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator localServiceInstantiator)
		{
			return CreateServicePIV(servicePipelineObject, serviceDescription,
				localServiceInstantiator, typeFilter);
		}
		#endregion // IPAFServiceManagerExtended Implementation

		#region Class Helper Methods
		/// <summary>
		/// Non-virtual helper to add services to the manager. This should be the
		/// only way a service is added to the manager.
		/// </summary>
		/// <param name="serviceDescription">Incoming service.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// "serviceDescription"
		/// </exception>
		/// <exception cref="PAFStandardException{T}">
        /// <see cref="PAFServiceExceptionMessageTags.OBJECT_NOT_A_SERVICE"/> if the service interface type
		/// does not inherit from <see cref="IPAFService"/>.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// We no longer expose a public method to add non-Generic services. This is
		/// to help with type safety. We have to keep the ability for ourselves to
		/// install legacy non-Generic services.
		/// </remarks>
		/// <threadsafety>
		/// Locks the dictionary with a write lock during the addition.
		/// </threadsafety>
		[SecuritySafeCritical]
		protected internal void AddServiceHelper(
			IPAFServiceDescription serviceDescription)
		{
			if (serviceDescription == null)
				throw new ArgumentNullException(nameof(serviceDescription));

			// Interface type is always early-bound.
			var serviceInterfaceTypeType = serviceDescription.ServiceInterfaceType.TypeType;

			// Implementation type will be null for late-bound services.
			var serviceImplementationTypeType = serviceDescription.ServiceImplementationType?.TypeType;

			// Early-bound means we can do type-checking.
			if (serviceImplementationTypeType != null)
			{
				// Check if a bogus service.
				if (!(typeof(IPAFService).IsTypeAssignableFrom(serviceInterfaceTypeType)))
				{
					var data = new PAFSED(serviceDescription);
					throw new PAFStandardException<IPAFSED>(data, PAFServiceExceptionMessageTags.TYPE_NOT_A_SERVICE);
				}

				// Check if service meets Generic constraint.
				if (!(serviceInterfaceTypeType.IsTypeAssignableFrom(serviceImplementationTypeType)))
				{
					var data = new PAFSED(serviceDescription);
					throw new PAFStandardException<IPAFSED>(data,
						PAFServiceExceptionMessageTags.TYPE_DOES_NOT_IMPLEMENT_INTERFACE);
				}
			}

			using (var accessor = m_ServiceDictionaryWrapper.GetWriteLockedObject())
			{
				// Bit of syntax clarity.....
				var services = accessor.WriteLockedNullableObject;
				services.AddService(serviceDescription);
			}
		}

		/// <remarks>
		/// See <see cref="AddServiceHelper"/>
		/// </remarks>
		protected internal void AddServicesHelper(
			IEnumerable<IPAFServiceDescription> serviceDescriptions)
		{
			if (serviceDescriptions == null) return;

			foreach (var serviceDescription in serviceDescriptions)
			{
				AddServiceHelper(serviceDescription);
			}
		}

		/// <summary>
		/// <para>
		/// This is a helper method that builds the services from <see cref="PAFServiceManager.InitialServicesInternal"/>,
		/// one-by-one, in enumeration order and adds them to the service array without
		/// initializing them.
		/// </para>
		/// </summary>
		protected virtual IList<IPAFServiceDescription> CreateInitialServices(
			IPAFServiceManager rootServiceManager)
		{
			var servicesBuilt = new Collection<IPAFServiceDescription>();
			if (InitialServicesInternal != null)
			{
				// We'll need one copy of the service object.
				var iPAFServicePipelineObject
					= new PAFServicePipelineObject(rootServiceManager, ServicePipelineStage.CONSTRUCTION);
				foreach (var svcDescription in InitialServicesInternal)
				{
					var service = CreateServicePIV(
						iPAFServicePipelineObject, svcDescription, LocalServiceInstantiator);
					servicesBuilt.Add(service);
				}
			}
			// If we made them all correctly, put them in.
			foreach (var svc in servicesBuilt)
			{
				AddServiceHelper(svc);
			}
			return servicesBuilt;
		}
		#region Static Helper Methods
		/// <summary>
		/// Creates a service from either loaded assemblies or a specific assembly
		/// in "available assemblies" or anywhere else we have a spec for. This
		/// version in Core only builds services with default constructors.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// Callees need <see cref="IPAFServicePipelineObject.ServiceManager"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// The <see cref="IPAFServiceDescription"/> provides a description
		/// of the service to be created. Please see the documentation for this
		/// interface.
		/// </param>
		/// <param name="typeFilter">
		/// Optional filter on the created service. Default = <see langword="null"/>.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// Constructs services after their type information is verified as
		/// available in current "AppDomain". Default = <see langword="null"/>
		/// causes <see cref="DefaultLocalServiceInstantiator"/> to be used.
		/// </param>
		/// <returns>
		/// A framework service. Never <see langword="null"/>.
		/// </returns>
		public static IPAFServiceDescription DefaultServiceCreator(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription,
			IPAFTypeFilter typeFilter = null,
			PAFLocalServiceInstantiator localServiceInstantiator = null)
		{
			var implementationTypeHolder = serviceDescription.ServiceImplementationType;
			var interfaceTypeHolder = serviceDescription.ServiceInterfaceType;
			// Our returned service.
			IPAFServiceDescription loadedServiceDescription = null;
			// For exceptions.
			string implementationTypeAssemblyName = null;
			// For type load/construction.
			string implementationTypeName = null;
			string implementationTypeNameSpace = null;

			// We wish to apply a filter that culls out services without default constructors.
			var defaultConstructableFilter
				= new PAFTypeFilter(TypeExtensions.IsTypeDefaultConstructable);

			// We must combine the filter with the one the client has provided, if any.
			var filterAggregator = new PAFTypeFilterAggregator("Default service creator");
			filterAggregator.AddFilter(defaultConstructableFilter);

			// Combine with any incoming filter.
			filterAggregator.AddFilter(typeFilter);

			// We use loader exception downstairs to provide detail in thrown exception.
			Exception loaderEx = null;
			// Same deal here.
			Exception creationEx = null;
			try
			{
				if ((implementationTypeHolder != null) && (implementationTypeHolder.IsTypeLoadable()))
				{

					// Now we have a specific target type.
					implementationTypeName = implementationTypeHolder.SimpleTypeName;
					implementationTypeNameSpace = implementationTypeHolder.Namespace;
					implementationTypeAssemblyName = implementationTypeHolder.GetAssemblyHolder().AssemblyNameString;
					try
					{
						// This call will load the assembly only if
						// it is not already in this AppDomain.
						implementationTypeHolder.ResolveType();
					}
					catch (Exception ex)
					{
						loaderEx = ex;
					}

					if (implementationTypeHolder.GetAssemblyHolder().Asmbly == null)
					{
						var data = new PAFAssemblyLoadExceptionData(implementationTypeHolder.GetAssemblyHolder());
						loaderEx = new PAFStandardException<IPAFAssemblyLoadExceptionData>(
							data, PAFAssemblyLoadExceptionMessageTags.GENERAL_ASSEMBLY_LOAD_ERROR, loaderEx);
					}
					else if (implementationTypeHolder.TypeType == null)
					{
						var data = new PAFTypeLoadExceptionData(implementationTypeHolder);
						loaderEx = new PAFStandardException<IPAFTypeLoadExceptionData>(
							data, PAFTypeLoadExceptionMessageTags.TYPE_NOT_FOUND_IN_ASSEMBLY, loaderEx);
					}
				}
				if (loaderEx == null)
				{
					var localInstantiator = localServiceInstantiator;
					if (localServiceInstantiator == null)
						localInstantiator = DefaultLocalServiceInstantiator;
					loadedServiceDescription = localInstantiator(servicePipelineObject,
						serviceDescription, typeFilter);
				}
				if (loadedServiceDescription != null) return loadedServiceDescription;
			}
			catch (Exception ex)
			{
				creationEx = ex;
			}

			// If we get here, we have not created a service, so we need to throw our exception.
			var thrownInnerException = creationEx;
			if (loaderEx != null) thrownInnerException = loaderEx;

			// todo krm - this will throw an exception
			// The exception needs the concrete type.
			var concreteTypeHolder = new PAFTypeHolder(implementationTypeNameSpace + "." + implementationTypeName + "," + implementationTypeAssemblyName);
			var exceptionData = new PAFSED(new PAFServiceDescription(interfaceTypeHolder, concreteTypeHolder));
			throw new PAFStandardException<IPAFSED>(exceptionData,
				PAFServiceExceptionMessageTags.TYPE_DOES_NOT_IMPLEMENT_INTERFACE,
				thrownInnerException);
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
		/// <exception cref="PAFStandardException{IPAFSED}">
		/// <see cref="PAFServiceExceptionMessageTags.SERVICE_IMPLEMENTATION_NOT_FOUND"/> if the service is not found.
		/// </exception>
		/// </exceptions>
		/// 		// TODO - KRM - Need to make all type filters deep-copyable.
		public static IPAFServiceDescription DefaultLocalServiceInstantiator(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription, IPAFTypeFilter typeFilter = null,
			IEnumerable<Assembly> assemblyList = null)
		{
			if (servicePipelineObject == null)
				throw new ArgumentNullException(nameof(servicePipelineObject));
			if (serviceDescription == null)
				throw new ArgumentNullException(nameof(serviceDescription));

			// A bit of thread-safety.
		    assemblyList = assemblyList?.IntoArray();

		    try
			{
				Type serviceImplementationType = null;
				if ((serviceDescription.ServiceImplementationType != null)
					&& (serviceDescription.ServiceImplementationType?.TypeType != null))
				{
					serviceImplementationType = serviceDescription.ServiceImplementationType.TypeType;
				}

				if (serviceImplementationType == null)
				{
					ICollection<Type> col;
					if (serviceDescription.ServiceImplementationType != null)
					{
                        col = ManufacturingUtils.Instance.LocateReflectionServices(
							serviceDescription.ServiceInterfaceType.NamespaceQualifiedTypeName,
							serviceDescription.ServiceImplementationType.Namespace,
							serviceDescription.ServiceImplementationType.SimpleTypeName,
							typeFilter, assemblyList);
					}
					else
					{
						// In this case we have only an interface description - we search the
						// whole world.
                        col = ManufacturingUtils.Instance.LocateReflectionServices(
							serviceDescription.ServiceInterfaceType.NamespaceQualifiedTypeName,
							null,
							null,
							typeFilter, assemblyList);
					}
					if ((col == null) || (col.Count == 0))
					{
						var serviceExceptionData
							= new PAFSED(serviceDescription);
						throw new PAFStandardException<IPAFSED>(serviceExceptionData,
							PAFServiceExceptionMessageTags.SERVICE_IMPLEMENTATION_NOT_FOUND);
					}
					serviceImplementationType = col.GetFirstElement();
					if (col.Count > 1)
					{
						var serviceExceptionData
							= new PAFSED(new PAFServiceDescription(null, PAFTypeHolder.IHolder(serviceImplementationType)));
						throw new PAFStandardException<IPAFSED>(serviceExceptionData,
                            PAFServiceExceptionMessageTags.MULTIPLE_IMPLEMENTATIONS_FOUND);
					}
					serviceImplementationType = col.GetFirstElement();
				}

				object objectInstance;
				try
				{
					objectInstance = Activator.CreateInstance(serviceImplementationType);
				}
				catch (Exception ex)
				{
					var serviceExceptionData = new PAFConstructorExceptionData(serviceImplementationType);
					throw new PAFStandardException<IPAFConstructorExceptionData>(serviceExceptionData,
						PAFConstructorExceptionMessageTags.FAILED_TO_CONSTRUCT_TYPE, ex);
				}
				var frameworkService = objectInstance as IPAFService;
				//// todo krm - shouldn't need this if addservice helper is doong its job.
				if (frameworkService == null)
				{
					var serviceExceptionData
						= new PAFSED(serviceDescription);
					throw new PAFStandardException<IPAFSED>(serviceExceptionData,
                        PAFServiceExceptionMessageTags.OBJECT_NOT_A_SERVICE);
				}
				serviceDescription.ServiceObject = frameworkService;
				return serviceDescription;
			}


			catch (Exception ex)
			{
				var serviceExceptionData
					= new PAFSED(serviceDescription);
				throw new PAFStandardException<IPAFSED>(serviceExceptionData,
                    PAFServiceExceptionMessageTags.SERVICE_CREATION_FAILED, ex);
			}
		}
		/// <summary>
		/// This method requests a specific service by its name.
		/// </summary>
		/// <param name="serviceName">
		/// Textual name of the service. If <see langword="null"/> or blank,
		/// we return the first service found.
		/// </param>
		/// <param name="iFServiceDescriptions">
		/// Set of <see cref="IPAFServiceDescription"/>s.
		/// </param>
		/// <returns>
		/// The found <see cref="IPAFService"/> or <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// This method is handy when used in conjunction with GetServices to get
		/// an array of services that inherit from a given Type. After gathering
		/// an array of services, those services can then be checked by name and the first
		/// one found is returned.
		/// </remarks>
		[CanBeNull]
		protected internal static IPAFServiceDescription GetNamedServiceDescription(
			string serviceName,[CanBeNull] IEnumerable<IPAFServiceDescription> iFServiceDescriptions)
		{
			// Safety valve.
			if (iFServiceDescriptions == null)
				return null;
			// Scan incoming services to check names.
			foreach (var iFservice in iFServiceDescriptions)
			{
				if (string.IsNullOrEmpty(serviceName))
					return iFservice;
				var name = iFservice.ObjectName;
				if ((!(string.IsNullOrEmpty(name))) && (Equals(name, serviceName)))
				{
					return iFservice;
				}
			}
			return null;
		}
		/// <summary>
		/// Loads/initializes services that have dependencies.
		/// </summary>
		/// <param name="serviceManager">
		/// A service manager that services can call upon during the load/initialization process.
		/// This is allowed to be <see langword="null"/> only if <see cref="RootServiceManager"/>
		/// is not <see langword="null"/> or we are not the root service.
		/// </param>
		/// <param name="pipelineStage">
		/// This is the stage we are working on in the pipeline. Currently,
		/// <see cref="ServicePipelineStage.LOAD"/> and <see cref="ServicePipelineStage.INITIALIZE"/>
		/// are supported.
		/// </param>
		/// <param name="initializeOnThread">
		/// <see langword="true"/> to initialize each service on a separate thread. In this
		/// case, the SM must wait for the completion of each thread by examining the
		/// "IsInitialized" property of each service.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{T}">
		/// Message: <see cref="PAFServicePipelineExceptionMessageTags.SERVICE_DEADLOCK_IN_STAGE"/>
		/// if there is a circular dependency between services.
		/// </exception>
		/// Exceptions occurring from the pipeline stage methods are collected and
		/// rethrown from within this method.
		/// </exceptions>
		// Note: DAP - removed all unstaging to Extended. We don't need it for Core.
		[SecuritySafeCritical]
		protected internal virtual void StageDependentServices(
			IPAFServiceManager<IPAFService> serviceManager,
			ServicePipelineStage pipelineStage,
			// ReSharper disable once RedundantAssignment
			bool initializeOnThread)
		{
			if (serviceManager == null)
			{
				serviceManager = RootServiceManager;
			}
			// We'll need one copy of the service object.
			var iPAFServicePipelineObject
				= new PAFServicePipelineObject<IPAFService>(serviceManager, pipelineStage);
			while (true)
			{
				ICollection<IPAFServiceExtended> beginningUnstagedServices;
				// We must re-fetch the services anew each time through the loop,
				// since new services may be added as we go.
				// We must take a snapshot of the service dictionary here, since we are not using
				// a synchronized dictionary.
				using (var accessor = m_ServiceDictionaryWrapper.GetReadLockedObject())
				{
					var services = accessor.ReadLockedNullableObject;
					beginningUnstagedServices = services.GetUnstagedServices(pipelineStage);
				}

				//////////////////////////////////////////////////////////////////////
				// No more work to do if everybody staged.
				//////////////////////////////////////////////////////////////////////
				if (beginningUnstagedServices == null) return;

				foreach (var svc in beginningUnstagedServices)
				{

					if ((pipelineStage == ServicePipelineStage.LOAD) && (svc.ServiceIsLoaded)) continue;
					if ((pipelineStage == ServicePipelineStage.INITIALIZE) && (svc.ServiceIsInitialized)) continue;
					// TODO - KRM. Don't want to use MS crap directly. Wait for
					// TODO - checkout of async stuff.
					// ReSharper disable once RedundantAssignment
					initializeOnThread = false;

					// Load or initialize?
					var pipelineDelegate
						= new Action<IPAFServicePipelineObject<IPAFService>>
							(svc.InitializeService);
					if (pipelineStage == ServicePipelineStage.LOAD)
					{
						pipelineDelegate = svc.LoadService;
					}

					// See if we can stage this service.
					var delegator
						= new CoreStandardServicePipelineDelegator<IPAFService>
							(pipelineDelegate, svc);
					if (!delegator.FetchNeededServices(iPAFServicePipelineObject))
					{
						if (iPAFServicePipelineObject.ExecutionException != null)
							throw iPAFServicePipelineObject.ExecutionException;
						continue;
					}

					/////////////////////////////////////////////////////////////////////////////////
					// Call the setup method, either on the current thread or a background thread.
					/////////////////////////////////////////////////////////////////////////////////
					// ReSharper disable ConditionIsAlwaysTrueOrFalse
					//if (initializeOnThread)
					// ReSharper restore ConditionIsAlwaysTrueOrFalse
					// ReSharper disable HeuristicUnreachableCode
					//ThreadPool.QueueUserWorkItem(delegator.WaitCallbackMethod,
					//	iPAFServicePipelineObject);
					// ReSharper restore HeuristicUnreachableCode
					// else
					delegator.WaitCallbackMethod(iPAFServicePipelineObject);
				}
				// Ending snapshot.
				ICollection<IPAFServiceExtended> endingUnstagedServices;
				// We must refetch the services anew each time through the loop,
				// since new services may be added as we go.
				// We must take a snapshot of the service dictionary here, since we are not using
				// a synchronized dictionary.
				using (var accessor = m_ServiceDictionaryWrapper.GetReadLockedObject())
				{
					var services = accessor.ReadLockedNullableObject;
					endingUnstagedServices = services.GetUnstagedServices(pipelineStage);
				}

				if (!endingUnstagedServices.ContainsSet(beginningUnstagedServices)) continue;

				// If we do not make progress each time through the loop, we have a deadlock.
				var data = new PAFServicePipelineExceptionData(endingUnstagedServices,
					pipelineStage,
					PAFTypeHolder.IHolder(serviceManager.GetType()),
					PAFTypeHolder.IHolder(typeof(IPAFService)));
				throw new PAFStandardException<IPAFServicePipelineExceptionData>(data,
                    PAFServicePipelineExceptionMessageTags.SERVICE_DEADLOCK_IN_STAGE);
			}
		}
		#endregion // Static Helper Methods
		#endregion // Class Helper Methods
	}
}
