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
using System.Security;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.UserInterface;

// Exception shorthand.
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
using PAFSEDB = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionDataBase;
using PlatformAgileFramework.FrameworkServices.Exceptions;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This FrameworkService "manager" contains a singleton version of basic or "core"
	/// services that must be present even to start an application. These services
	/// have to do with loading files, interacting with the user during error
	/// conditions, etc. These services are often initialized with stand-in versions.
	/// As the application loads, the client may overload these services with more
	/// sophisticated versions to service the full application.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 21feb2012 </date>
	/// <contribution>
	/// Made necessary mods to use one base service manager for both remote
	/// and local services - code consolidation. Moved everything but dynamic
	/// deadlock detection/resolution out of Core according to KRM.
	/// </contribution>
	/// <author> KRM </author>
	/// <date> 27jan2012 </date>
	/// <contribution> Converted from 3.5 and cleaned up. </contribution>
	/// </history>
	/// <threadsafety>
	/// Synchronized.
	/// </threadsafety>
	/// <remarks>
	/// This service manager is both a Generic and non-Generic service manager.
	/// We had to build it this way to support legacy applications. But you don't
	/// have to build YOUR service manager this way! Better to require Generic
	/// services for type-safety.
	/// </remarks>
	// ReSharper disable PartialTypeWithSinglePart
	// Core part
	public sealed partial class PAFServices
		: PAFGeneralServiceManager<IPAFService>
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields
		//////////////////////////////////////////////////////////////////////////////////////
		//// These are our singleton versions of the low-level services that are always here
		//// (unless we get an exception in our constructor). These are internal static
		//// properties so they can be set from afar before the services are first accessed.
		//// These services are typically initially set as very simple versions of the services
		//// that do not have service dependencies, or have few.
		//////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		///	This is the UI service. This is the simple console and dialog window service.
		/// It is usually augmented with something more substantial once the app gets running.
		/// This service is usually a singleton (one instance per app domain) so they
		/// are static.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static IPAFServiceDescription<IPAFUIService> s_SiPAFUIService
		{
			[SecurityCritical]
			get { return s_SiPAFUIServiceInternal; }
			[SecurityCritical]
			set { s_SiPAFUIServiceInternal = value; }
		}
		internal static IPAFServiceDescription<IPAFUIService> s_SiPAFUIServiceInternal
		{ get; set; }

		/// <summary>
		///	This is the logging service. It is dependent on the storage service usually.
		/// </summary>
		public static IPAFServiceDescription<IPAFLoggingService> s_SiPAFLoggingService
		{
			[SecurityCritical]
			get { return s_SiPAFLoggingServiceInternal; }
			[SecurityCritical]
			set { s_SiPAFLoggingServiceInternal = value; }
		}
		internal static IPAFServiceDescription<IPAFLoggingService> s_SiPAFLoggingServiceInternal
		{ get; set; }

		/// <summary>
		///	This is the storage service. This is fundamental file/directory access
		/// in simplest cases. This service can be pre-loaded with a basic implementation that
		/// is needed to even read configuration files to get the application parameterized,
		/// then replaced with something more specific.
		/// </summary>
		public static IPAFServiceDescription<IPAFStorageService> s_SiPAFStorageService
		{
			[SecurityCritical]
			get { return SiPAFStorageServiceInternal; }
			[SecurityCritical]
			set { SiPAFStorageServiceInternal = value; }
		}
		internal static IPAFServiceDescription<IPAFStorageService> SiPAFStorageServiceInternal { get; set; }

		///// <summary>
		///// Established by the bootstrapper method. Not synchronized.
		///// </summary>
		//public static ISymbolicDirectoryMappingDictionary DirectoryMappings { get; protected internal set; }

		//////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Lock for multithread access. 1 indicates already initializing.
		/// </summary>
		/// <remarks>
		/// This is a lock variable and must never be touched by anyone except
		/// the initialization method. Not volatile since it is not accessed directly.
		/// </remarks>
		private static int s_1ForInitializing;
		/// <summary>
		/// Just tells our internals if initialization is complete.
		/// </summary>
		public static bool IsInitialized { get { return s_1ForInitializing != 1; } } 
		/// <summary>
		/// Disposal key.
		/// </summary>
		private static Guid s_DisposalKey;
		/// <summary>
		/// Our singleton service set.
		/// </summary>
		private static PAFServices s_Services;
		#endregion Class Fields
		#region Constructors
		/// <summary>
		///	Initializes a new instance of the <see cref="PAFServiceManager"/> class.
		/// This is the standard one that the regular <see cref="Manager"/> property
		/// retrieves.
		/// </summary>
		/// <remarks>
		///	This is private to enforce the singleton condition.
		/// </remarks>
		[SecurityCritical]
		private PAFServices()
			: base(GetStaticGuid())
		{
		}
		#endregion Constructors
		#region Singleton Members
		/// <summary>
		///	Gets the singleton instance after ensuring it is completely constructed.
		/// </summary>
		/// <value>The singleton instance.</value>
		/// <remarks>
		/// Lazy construction needed here, since the "AppDomain" creator
		/// may wish to set static fields before first access.
		/// </remarks>
		/// <security>
		/// Security safe so external app can access services without being trusted.
		/// </security>
		public static IPAFServiceManager<IPAFService> Manager
		{
			[SecuritySafeCritical]
			get
			{
				var manager = 
				ThreadingUtils.LazyConstructionLock<PAFServices, object>
					(ref s_1ForInitializing, ref s_Services, null, InstantiateAndProvision);
				return manager;
			}
		}
		#endregion Singleton Members
		#region Methods
		/// <summary>
		/// Checks to see whether the disposal key has been loaded and generates
		/// a random guid if not.
		/// </summary>
		/// <returns>
		/// Preloaded or new random key.
		/// </returns>
		internal static Guid GetStaticGuid()
		{
			if (s_DisposalKey == Guid.Empty)
				s_DisposalKey = Guid.NewGuid();
			return s_DisposalKey;
		}
		/// <summary>
		/// This method is the "bootstrapper" for the service manager. It installs basic
		/// critical services from service objets that are either pre-loaded as statics
		/// or created by searching assemblies for implementations.
		/// </summary>
		/// <param name="o">An optional provisioning object.</param>
		/// <returns></returns>
		[SecurityCritical]
		internal static PAFServices InstantiateAndProvision(object o)
		{
			var manager = new PAFServices();
			// We must build initial services here, since the staging phases need basic
			// services.
			manager.BuildInitialServices();
			// By using the SM to supply services to itself, we allow iterative
			// load/initialize as service dependencies are negotiated.
			manager.StageDependentServices(manager, ServicePipelineStage.LOAD, true);
			manager.StageDependentServices(manager, ServicePipelineStage.INITIALIZE, true);

			// Set manager and flag to turn on service singleton for everybody.
			s_Services = manager;
			return manager;
		}

		/// <summary>
		/// <para>
		/// This is a helper method that builds the services from <see cref="PAFServiceManager.InitialServicesInternal"/>,
		/// one-by-one, in enumeration order and adds them to the service array without initializing
		/// them.
		/// </para>
		/// <para>
		/// The method will construct the initial set of services. It then checks to see if
		/// these services contain <see cref="IPAFLoggingService"/> or <see cref="IPAFUIService"/>
		/// or <see cref="IPAFStorageService"/>. If they do, <see cref="s_SiPAFLoggingService"/>
		/// and/or <see cref="s_SiPAFUIService"/> and/or <see cref="s_SiPAFStorageService"/>
		/// are replaced with these versions. If they don't and <see cref="s_SiPAFLoggingService"/>
		/// or <see cref="s_SiPAFLoggingService"/> or <see cref="s_SiPAFStorageService"/> are loaded,
		/// they are used instead. If no UI and/or Logger service has been provided from either
		/// input source, defaults are constructed.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Currently, we cannot survive without a storage service being supplied, either in the
		/// <see cref="PAFGeneralServiceManager{IPAFService}.InitialServicesInternal"/> or in the static
		/// in this class.
		/// </remarks>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFSED}">
		/// <see cref="PAFSEDB.SERVICE_NOT_FOUND"/> is thrown if storage service not loaded. It is wrapped
		/// in a general exception.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFSED}">
		/// <see cref="PAFSEDB.SERVICE_CREATION_FAILED"/> is thrown as a wrapper if any exceptions
		/// occur in the method.
		/// </exception>
		/// </exceptions>
		private void BuildInitialServices()
		{
			var servicesBuilt = new List<IPAFServiceDescription>();
			try
			{
				////// We'll need one copy of the service object for any construction work we'll do.
				var iPAFServicePipelineObject
					= new PAFServicePipelineObject(this, ServicePipelineStage.CONSTRUCTION);
				if (InitialServicesInternal != null)
				{
					foreach (var svcDescription in InitialServicesInternal)
					{
						// Skip if we were pushed in already built.
						if (svcDescription.ServiceObject != null)
						{
							servicesBuilt.Add(svcDescription);
							continue;
						}
						var service = CreateServicePIV(
							iPAFServicePipelineObject, svcDescription);
						servicesBuilt.Add(service);
					}

					// Add the services that we may need for further work in this method.
					AddServicesHelper(servicesBuilt);
				}
				/////////////////////////////////////////////////////////////////////////////////////////////////////////
				//// If we do not have storage, logger or UI, put them in. 
				/////////////////////////////////////////////////////////////////////////////////////////////////////////
				// SS
				if (GetAnyService<IPAFStorageService>(false) == null)
				{
					// See if it is incoming on the static.
					if (SiPAFStorageServiceInternal == null)
					{
						// Can't withstand lack of a storage facility.
						var data
							= new PAFSED(new PAFServiceDescription<IPAFStorageService>(
								PAFTypeHolder.IHolder(typeof (IPAFStorageService))));
						throw new PAFStandardException<IPAFSED>(data, PAFServiceExceptionMessageTags.SERVICE_NOT_FOUND);
					}
					IPAFServiceDescription serviceDescription = SiPAFStorageServiceInternal;
					var servicePreload = SiPAFStorageServiceInternal.ServiceObject;

					// Not preloaded?
					if (servicePreload == null)
						serviceDescription =  CreateServicePIV(iPAFServicePipelineObject,
							SiPAFStorageServiceInternal);

					AddServiceHelper(serviceDescription);
				}

				// UI.
				if (GetAnyService< IPAFUIService>(false) == null)
				{
					// See if it is incoming on the static.
					if (s_SiPAFUIServiceInternal == null)
					{
						// Can't withstand lack of a storage facility.
						var data
							= new PAFSED(new PAFServiceDescription<IPAFUIService>(
								PAFTypeHolder.IHolder(typeof(IPAFUIService))));
                        throw new PAFStandardException<IPAFSED>(data, PAFServiceExceptionMessageTags.SERVICE_NOT_FOUND);
					}
					IPAFServiceDescription serviceDescription = s_SiPAFUIServiceInternal;
					var servicePreload = s_SiPAFUIServiceInternal.ServiceObject;

					// Not preloaded?
					if (servicePreload == null)
						serviceDescription = CreateServicePIV(iPAFServicePipelineObject,
							s_SiPAFUIServiceInternal);

					AddServiceHelper(serviceDescription);
				}

				// Logger.
				if (GetAnyService<IPAFLoggingService>(false) == null)
				{
					// See if it is incoming on the static.
					if (s_SiPAFLoggingServiceInternal == null)
					{
						//// Customer requirements specify a need for an "emergency" logger
						//// that does not have requirements on anything being configured yet.
						s_SiPAFLoggingServiceInternal
							= new PAFServiceDescription<IPAFLoggingService>(
							new PAFTypeHolder(typeof (IPAFLoggingService)),
							new PAFTypeHolder(typeof (EmergencyLoggingService)));
					}
					IPAFServiceDescription serviceDescription = s_SiPAFLoggingServiceInternal;
					var servicePreload = s_SiPAFLoggingServiceInternal.ServiceObject;

					// Not preloaded?
					if (servicePreload == null)
						serviceDescription = CreateServicePIV(iPAFServicePipelineObject,
							s_SiPAFLoggingServiceInternal);

					AddServiceHelper(serviceDescription);
				}
			}
			catch (Exception ex)
			{
				var data = new PAFSED(null);
                throw new PAFStandardException<IPAFSED>(data, PAFServiceExceptionMessageTags.SERVICE_CREATION_FAILED, ex);
			}
		}

		#endregion // Methods
	}
}