//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2015 Icucom Corporation
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
using System.IO;
using System.Reflection;
using PlatformAgileFramework.Application;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FileAndIO.SymbolicDirectories;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.UserInterface;
using PlatformAgileFramework.UserInterface.ConsoleUI;

namespace PlatformAgileFramework.FrameworkServices
{

	/// <summary>
	/// Version for Xamarin mobile that loads the service manager with
	/// pre-configured services.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01aug2018 </date>
	/// <desription>
	/// Changed the name of the class for iOS/Android integration. No
	/// longer need Windows Phone support.
	/// </desription>
	/// </contribution>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 18jan2016 </date>
	/// <desription>
	/// New.
	/// </desription>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// NOT thread-safe.
	/// </threadsafety>
	public static class LoadServiceManagerXamarinMobile
	{
	    /// <summary>
	    /// Backing for the SM. Set by this class or by trusted platform classes.
	    /// </summary>
	    internal static IPAFServiceManager<IPAFService>
	        s_ServiceManager;

        private static volatile bool s_IsManagerPreloaded;
		public static void PreLoadManager()
		{
		    if (s_IsManagerPreloaded)
		        return;
		    s_IsManagerPreloaded = true;

			// Load platform mappings.
			PlatformUtils.PlatformInfo = new XamarinFormsPlatformInfo();

			// Set the app root to our special folder.
			PlatformUtils.ApplicationRoot = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

			var initialServices = PAFServiceManager.InitialServicesInternal;
			if(initialServices == null)
				PAFServiceManager.InitialServices = new Collection<IPAFServiceDescription>();

		    var mu = new XamarinManufacturingUtilsInstance();
		    // Net standard deficiencies causes us to need to push in a couple of things.
		    mu.PushAssemblyLister(AppDomain.CurrentDomain.GetAssemblies);
		    mu.PushAssemblyLoadFromLoader(Assembly.LoadFrom);

			// We are loading from code.
			//SymbolicDirectoryMappingDictionary.DirectoryMappingFileName = null;
			var name = SymbolicDirectoryMappingDictionary.DirectoryMappingFileName;

			PAFServiceManager.InitialServices.Add(new PAFServiceDescription<IManufacturingUtils>
				(new PAFNamedAndTypedObject<IManufacturingUtils>
				(null, null, mu,true)));

			var storageService = new PAFStorageServiceAndroid();
			PAFServices.s_SiPAFStorageService
				= new PAFServiceDescription<IPAFStorageService>(
					storageService.GetServiceNTOFromServiceObject());

			var mainLoggerFilePath = Path.Combine(PlatformUtils.ApplicationRoot, "LogFile.txt");
			var emergencyLoggerFilePath = Path.Combine(PlatformUtils.ApplicationRoot, "EmergencyLogFile.txt");

			var mainLoggingService = new PAFLoggingService(PAFLoggingLevel.Error, true, null, mainLoggerFilePath);

			//// Some android deveices have a little trouble probing, so we give them some help.
			var mainLoggingServiceDescription = mainLoggingService.GetServiceDescription();
			// Preload the service object into the description so the loader won't have to look for it.
			mainLoggingServiceDescription.ServiceObject = mainLoggingService.ObjectValue;

			var emergencyLoggingService
				= new EmergencyLoggingService(false, mainLoggingService.GetServiceDescription(), emergencyLoggerFilePath);

		    emergencyLoggingService.TruncateFileOnStart = true;
			emergencyLoggingService.MainService = mainLoggingService;
			PAFServices.s_SiPAFLoggingService
				= new PAFServiceDescription<IPAFLoggingService>(
					emergencyLoggingService.GetServiceNTOFromServiceObject());

			var uiService = new ConsoleUserInteractionService();
			PAFServices.s_SiPAFUIService
				= new PAFServiceDescription<IPAFUIService>(
					uiService.GetServiceNTOFromServiceObject());

			// Bootstrapper loads core services only if they are not already loaded.
			ServiceBootStrapper.Instance.LoadCoreServices();

		    s_ServiceManager = PAFServiceManagerContainer.ServiceManager;

        }
        /// <summary>
        /// Just holds the manager. Constructed at application load time. This
        /// one just points into the static ROOT manager.
        /// </summary>
        public static IPAFServiceManager<IPAFService> ServiceManager
	    {
	        get { return s_ServiceManager; }
	    }

    }
}
