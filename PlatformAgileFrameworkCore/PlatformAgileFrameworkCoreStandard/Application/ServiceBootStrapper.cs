//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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
using System.Collections.ObjectModel;
using System.Linq;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.UserInterface;

namespace PlatformAgileFramework.Application
{
	/// <summary>
	/// Classic "bootstrapper" that solves the chicken/egg problem for the service
	/// manager. For SO apps, this is the bootstrapper for the app, really. Loads
	/// things into the service manager in proper order to get basic services
	/// loaded and make the SM ready for use. Lazy singleton can have statics loaded
	/// before instantiation. Note that bootstrapper is "bootstrapped" by the
	/// <see cref="ManufacturingUtils"/> class. See that class for important setup
	/// details.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04nov2017 </date>
	/// <description>
	/// Broke out platform assembly loading to better support testing. Changed the name
	/// for clarity of purpose. This class is for initializing of the service manager.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01apr2016 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class ServiceBootStrapper
	{
		#region Fields and Autoprops
		/// <summary>
		/// This a thread-safe wrapper for constructing the singleton.
		/// </summary>
		/// <remarks>
		/// Lazy class is thread-safe by default.
		/// </remarks>
		private static readonly Lazy<ServiceBootStrapper> s_Singleton =
			new Lazy<ServiceBootStrapper>(ConstructBootStrapper);

		/// <summary>
		/// Set when the core services are loaded.
		/// </summary>
		private static volatile bool s_AreCoreServicesLoaded;

	    #endregion // Fields and Autoprops
		#region Constructors
		/// <summary>
		/// For the singleton.
		/// </summary>
		private ServiceBootStrapper()
		{
		}

		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Get the singleton instance of the class.
		/// </summary>
		/// <returns>The singleton.</returns>
		public static ServiceBootStrapper Instance
		{
			get { return s_Singleton.Value; }
		}

		#endregion Properties
		#region Methods
		/// <summary>
		/// Not quite a constructor - a factory for the lazy construction.
		/// </summary>
		private static ServiceBootStrapper ConstructBootStrapper()
		{
			return new ServiceBootStrapper();
		}


		/// <summary>
		/// This method will load the default logging, storage and UI service. These must be
		/// loaded before almost any other service, since most other services depend on these
		/// three. The method will load the platform assembly, if it's not loaded already.
		/// </summary>
		public void LoadCoreServices()
		{
			if (s_AreCoreServicesLoaded)
				return;
			s_AreCoreServicesLoaded = true;
			var initialServices = PAFServiceManager.InitialServicesInternal;
			if (initialServices == null)
				PAFServiceManager.InitialServicesInternal = new Collection<IPAFServiceDescription>();

		    /////////////////////////////////////////////////////////////////////
		    // Service 0 - Need to make certain manufacturing services available
            // for the contracts assy.
		    /////////////////////////////////////////////////////////////////////
            PAFServiceManager.InitialServicesInternal.Add(new PAFServiceDescription<IManufacturingUtils>
		    (new PAFNamedAndTypedObject<IManufacturingUtils>
		        (null, null, new ManufacturingUtilsInstance(), true)));

            /////////////////////////////////////////////////////////////////////
            // Service 1 - logging. Main logger has a dependency on storage service.
            // emergency logger is used when building the infrastructure, like in
            // this method.
            /////////////////////////////////////////////////////////////////////		
            // The "emergency" logging service can run before storage is active.
            if (PAFServiceManager.InitialServicesInternal.FindServiceImplementationTypeInCollection<EmergencyLoggingService>() == null)
			{
				var mainLoggerDescription
					= new PAFServiceDescription<IPAFLoggingService>
					(new PAFTypeHolder(typeof(IPAFLoggingService)), new PAFTypeHolder(typeof(PAFLoggingService)));
				PAFLoggingService.s_PreloadedWriters = new Collection<Action<string>>();

				// So we can see stuff during tests.
				// KRM - move PAFLoggingService.s_PreloadedWriters.Add((Console.WriteLine));

				var loggingService = new EmergencyLoggingService(mainLoggerDescription);
				PAFServices.s_SiPAFLoggingServiceInternal
					= new PAFServiceDescription<IPAFLoggingService>(
						loggingService.GetServiceNTOFromServiceObject());
			}

			/////////////////////////////////////////////////////////////////////
			// Service 2 - storage. This one has dependency on the logger, and must have the
			// symbolic mapping dictionary loaded before symbolic directories can
			// be used.
			// Let the service manager instantiate this one internally.
			/////////////////////////////////////////////////////////////////////
			var storageTypeToInstantiate = ManufacturingUtils.Instance.LocateReflectionServices(typeof(IPAFStorageService).FullName).FirstOrDefault();
			//var storageService = Activator.CreateInstance(storageTypeToInstantiate);
			var storageServiceDescription = new PAFServiceDescription<IPAFStorageService>(PAFTypeHolder.IHolder(typeof(IPAFStorageService)),
				new PAFTypedObject(null, storageTypeToInstantiate));
			PAFServiceManager.InitialServicesInternal.Add(storageServiceDescription);

			/////////////////////////////////////////////////////////////////////
			// Service 3 - UI. UI typically has a dependency on storage and logging.
			// Search for this one by it's interface in loaded assys.
			/////////////////////////////////////////////////////////////////////		
			// Even though we don't have an initialized service manager yet, we have the
			// facilities to find our needed services in our platform assembly.
			var uiTypeToInstantiate
				= ManufacturingUtils.Instance.LocateReflectionServices(typeof(IPAFUIService).FullName).FirstOrDefault();
			// ReSharper disable once AssignNullToNotNullAttribute
			var uiService = Activator.CreateInstance(uiTypeToInstantiate);
			var uiServiceDescription = new PAFServiceDescription<IPAFUIService>(PAFTypeHolder.IHolder(typeof(IPAFUIService)),
				new PAFTypedObject(uiService, uiService.GetType()));
			uiServiceDescription.ServiceObject = uiService;
			PAFServiceManager.InitialServicesInternal.Add(uiServiceDescription);

			// Touch the service manager so we know it's ok right here.
			var theManager = PAFServices.Manager;

            // We must wait until the SM is accessible to call this, since it uses services.
            ManufacturingUtils.DirectoryMappingsInternal.PopulateStaticDictionaryFromXMLInternal(ManufacturingUtils.DirectoryMappingFilePathWithFile);
 
            // Stuff it upstairs.
            PAFServiceManagerContainer.ServiceManager = theManager;

		}
		#endregion Methods
	}
}
