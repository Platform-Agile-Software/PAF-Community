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
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.FrameworkServices.Exceptions;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;
// Exception shorthand.
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;



namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// <para>
	///	This is the emergency logger that writes to a text file.
	/// It is a temporary logger that can log startup failures before the normal
	/// logger is fully loaded and log problems after the main logger is unloaded.
	/// This class is designed to live in the same assembly as any "core" service
	/// manager or allow access to its internals so the SM or application class
	/// may fire up a SINGLE copy of it.
	/// </para>
	/// <para>
	/// Implementors can subclass this class to write to console, etc., but
	/// this provides a base that is synchronized to a single file.
	/// </para>
	/// <para>
	/// This service class also implements
	/// <see cref="IPAFEmergencyServiceProvider{T}"/>,
	/// so it's possible to switch out the temp emergency logger without
	/// switching service references within the SM. Otherwise, the service
	/// update message must be broadcast to advise clients of the change
	/// and client logic must be supplied, etc..
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 20may2014 </date>
	/// <description>
	/// Added elimination of recursion because someone had done something strange we did
	/// not anticipate.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21sep2013 </date>
	/// <description>
	/// Added preload capability for storage service.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 21jun2011 </date>
	/// <description>
	/// Converted from 3.5 and cleaned up. Converted to delayed initialization pattern.
	/// Moved the initialization into this class so the SM doesn't have to know about
	/// two loggers.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// Lots of unprotected public stuff here, since this is normally only set up
	/// by an application's internals.
	/// </remarks>
	/// <threadsafety>
	/// Safe if implementation of main logger is safe.
	/// </threadsafety>
	// ReSharper disable once PartialTypeWithSinglePart
	// partial - winddown is now in extended.
	public partial class EmergencyLoggingServiceBase : PAFService,
		IPAFLoggingService, IPAFEmergencyServiceProvider<IPAFLoggingService>, IPAFLogfileReader
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Tells whether we should throw an exception if main logger cannot be built.
		/// When loggers are built in an "AppDomain", the creator of
		/// the "AppDomain" may wish to allow the failure of the main logger.
		/// Default = <see langword="false"/>.
		/// </summary>
		protected internal static bool CanRunWithoutMain { get; set; }
		/// <summary>
		/// Holds the default name. File will be created in the startup CWD. This will be
		/// the addin root for the addin framework, the isolated storage root
		/// for SL and the app dir for ECMA/CLR.
		/// </summary>
		public const string DEFAULT_EMERGENCY_FILE = "EmergencyLog.txt";
		/// <summary>
		/// Holds the default log header text.
		/// </summary>
		public const string DEFAULT_HEADER_TEXT = "Emergency Logger Entry";
		/// <summary>
		/// This is the message that is put into the file wen the emergency logger
		/// laods on startup.
		/// </summary>
		public const string LOAD_MESSAGE = "Emergency Logger Loaded";
		/// <summary>
		/// Holds the emergency file path with terminating name. Can be loaded by the constructor.
		/// To ensure thread safety, load this before publishing the service.
		/// </summary>
		protected internal readonly string m_EmergencyLogFilePath;
		/// <summary>
		/// This needs to be static, since we are contending for one file. The lock should
		/// really be a system-wide mutex, but the emergency logger is normally only instantiated
		/// in the Main AppDomain or process, anyway, so we don't worry about it.
		/// </summary>
		private static readonly object s_FileLockObject = new object();
		/// <summary>
		/// Handle on myself.
		/// </summary>
		protected internal IPAFLoggingService MeAsLogger { get; private set; }
		/// <summary>
		/// The main logger is <see langword="null"/> until constructed.
		/// </summary>
		public IPAFLoggingService MainService { get; protected internal set; }
		/// <summary>
		/// This is an extension point for the logging system. A description of a main logger that
		/// is installed when the requisite services it needs are available.
		/// </summary>
		public IPAFServiceDescription MainServiceDescription
		{ get; protected internal set; }
		/// <summary>
		/// This is the text that is printed above each log message. date/time is printed at the end of this text.
		/// </summary>
		public string HeaderPrefixText
		{ get; protected internal set; }
		/// <summary>
		/// Tells whether service has been created. We are not designed with a singleton
		/// pattern. Thus just prevents us from being created multiple times
		/// by somebody on the OUTSIDE - the SM won't do it. PAF supports
		/// the concept of hierarchical loggers. This is the "root" logger
		/// and is the only one supplied in Core. There is only one of these
		/// allowed in any app domain. four-byte data is atomic.
		/// </summary>
		protected internal static bool s_ServiceIsCreated;
		/// <summary>
		/// Copy of the default storage service. This may be installed internally through
		/// service lookup, since it is generally assumed that the file service has no dependencies.
		/// It can also be installed after construction, prior to any pipeline stages.
		/// </summary>
		protected internal static IPAFStorageService StorageService { get; set; }
		/// <summary>
		/// <see langword="true"/> to truncate file when logger is constructed. Set this before
		/// service is loaded to maintain thread safety.
		/// </summary>
		protected internal bool TruncateFileOnStart { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Initializes a new instance of us.
		///  </summary>
		/// <param name="mainServiceDescription">
		/// This optional parameter provides a description of the "main" service to be switched to in 
		/// the service "initialize" stage. Default is to just look for somebody implementing
		/// <see cref="IPAFLoggingService"/> interface inside loaded assemblies, other than us.
		/// </param>
		/// <param name="emergencyLogFilePath">
		/// This optional parameter allows the name of an emergency plain text logging file
		/// to be specified. Default = <see cref="DEFAULT_EMERGENCY_FILE"/>.
		/// </param>
		///  <exception> <see cref="PAFStandardException{T}"/> is thrown if
		///  an instance of the class is already constructed.
		///  <see>
		///      <cref>PAFServiceExceptionData.SERVICE_ALREADY_CREATED</cref>
		///  </see>
		///      .
		///  </exception>
		protected internal EmergencyLoggingServiceBase(IPAFServiceDescription mainServiceDescription = null,
			string emergencyLogFilePath = null)
		{
			if (s_ServiceIsCreated)
			{
				var exceptionData
					= new PAFSED(new PAFServiceDescription(PAFTypeHolder.IHolder(GetType())));
                throw new PAFStandardException<IPAFSED>(exceptionData, PAFServiceExceptionMessageTags.SERVICE_ALREADY_CREATED);
			}

			// Shove in the default header.
			HeaderPrefixText = DEFAULT_HEADER_TEXT;

			// Default if not incoming.
			m_EmergencyLogFilePath = emergencyLogFilePath ?? DEFAULT_EMERGENCY_FILE;

			// Default if not incoming.
			MainServiceDescription = mainServiceDescription ??
				new PAFServiceDescription(PAFTypeHolder.IHolder(typeof(IPAFLoggingService)));

			MeAsLogger = this;

			ObjectName = "DefaultEmergencyLogger";

			s_ServiceIsCreated = true;
		}
		#endregion // Constructors
		#region Properties
		#region IPAFFrameworkServiceExtended Overrides
		/// <summary>
		/// Override to tell the SM that we need ourselves before we can be loaded.
		/// The reason is because the manufacturing utils that we use to create loggers
		/// through loading and reflection need to log their progress.
		/// </summary>
		protected internal override IEnumerable<IPAFServiceDescription>
			ServicesRequiredForLoadPIV
		{
			get
			{
				return new EnumeratedSingleton<IPAFServiceDescription>
					(new PAFServiceDescription(PAFTypeHolder.IHolder(typeof(IPAFStorageService))));
			}
		}
		#endregion // IPAFFrameworkServiceExtended Overrides
		#endregion // Properties
		#region Methods
		#region IPAFFrameworkServiceExtended Overrides
		/// <summary>
		/// Override to attach the file service. We attach the service here, since
		/// we cannot request it from the static service manager until after the
		/// manager has been completely initialized with BASIC services, including US.
		/// We fiddle with the log file here, getting it into thew right state and
		/// finding out if we can open it.
		/// </summary>
		/// <param name="serviceObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		/// <remarks>
		/// If <see cref="StorageService"/> is already loaded, it is used.
		/// </remarks>
		protected internal override void LoadServicePIV(
			IPAFServicePipelineObject<IPAFService> serviceObject)
		{
			if(StorageService == null)
				StorageService = (IPAFStorageService) serviceObject.ServiceManager.GetService(typeof(IPAFStorageService));
			if ((StorageService.PAFFileExists(m_EmergencyLogFilePath))
			    && (TruncateFileOnStart))
			{
				StorageService.PAFDeleteFile(m_EmergencyLogFilePath);
				var newFileStream = StorageService.PAFCreateFile(m_EmergencyLogFilePath);
				newFileStream.Dispose();
			}
			if (!StorageService.PAFFileExists(m_EmergencyLogFilePath))
			{
				var newFileStream = StorageService.PAFCreateFile(m_EmergencyLogFilePath);
				newFileStream.Dispose();
			}
			// Open it so problems occur early on, when we can catch them in
			// the load process.
			using (var stream = StorageService.PAFOpenFile(m_EmergencyLogFilePath, PAFFileAccessMode.APPEND))
			{
				stream.PAFWriteString(LOAD_MESSAGE + PlatformUtils.LTRMN);			
			}
			base.LoadServicePIV(serviceObject);
		}
		/// <summary>
		/// Override to initialize ourselves with the main service.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		protected internal override void InitializeServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			var serviceDescription = CreateMainLogger(servicePipelineObject);
			if (serviceDescription != null)
			{
				MainServiceDescription = serviceDescription;
				MainService = (IPAFLoggingService) MainServiceDescription.ServiceObject;
			}
			base.InitializeServicePIV(servicePipelineObject);
		}
		#endregion // IPAFFrameworkServiceExtended Overrides
		#region IPAFLoggingService Implementation
		/// <summary>
		///	<see cref="IPAFLoggingService"/>.
		/// </summary>
		/// <param name="message"><see cref="IPAFLoggingService"/>.</param>
		/// <param name="logLevel">
		///	<see cref="IPAFLoggingService"/>.
		/// Ignored in the "emergency" implementation - everything logged.
		/// </param>
		/// <param name="exception"><see cref="IPAFLoggingService"/>.</param>
		public virtual void LogEntry(object message = null, PAFLoggingLevel logLevel = PAFLoggingLevel.Default,
			Exception exception = null)
		{
			// Handle in the main logger, if active, avoiding recursion.
			if((MainService != null) && (MainService != this))
			{
				MainService.LogEntry(message, logLevel, exception);
				return;
			}
			var loggedString = HeaderPrefixText + "  (" + DateTime.Now + ")" + ":" + PlatformUtils.LTRMN;
			if (message != null) loggedString += "Message: " + message + PlatformUtils.LTRMN;
// TODO awaiting general exception renderer conversion.
//			if (exception != null) loggedString += "Exception: " + EAndEUtils.RecursiveExceptionRenderer(exception);
			if (exception != null) loggedString += "Exception Message: " + exception.Message;
			lock (s_FileLockObject)
			{
				IPAFStorageService storageService;
				if (PAFServices.IsInitialized)
				{
					storageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
				}
				else
				{
					storageService = PAFServices.SiPAFStorageServiceInternal.Service;
				}

				using (var stream = storageService.PAFOpenFile(m_EmergencyLogFilePath, PAFFileAccessMode.APPEND))
				{
					stream.PAFWriteString(loggedString);
				}
			}
		}
		#endregion // IPAFLoggingService Implementation
		#region Implementation of IPAFLogfileReader
		/// <summary>
		/// See <see cref="IPAFLogfileReader"/>
		/// </summary>
		public string ReadInstanceLogFile()
		{
		    return (MainService as IPAFLogfileReader)?.ReadInstanceLogFile();
		}
		#endregion // Implementation of IPAFLogfileReader
		#region IPAFEmergencyServiceProvider Implementation
		/// <summary>
		///	Returns me, since I am an emergency logger. Generally this is the pattern to
		/// be followed for a staged service, since the SM doesn't have to change interface
		/// references if we do it this way.
		/// </summary>
		/// <remarks>
		/// Note that this implementation does not allow access to the original text logger
		/// after the main logger has been built and while it is still in existance.
		/// </remarks>
		IPAFLoggingService IPAFEmergencyServiceProvider<IPAFLoggingService>.EmergencyService
		{
			get { return this; }
		}
		#endregion // IPAFEmergencyServiceProvider Implementation
		#endregion // Methods
		/// <summary>
		/// Initializes the main service and logs a message telling
		/// that we failed or not.
		/// </summary>
		/// <param name="serviceObject">
		/// Callees need  <see cref="IPAFServicePipelineObject.ServiceManager"/>.
		/// </param>
		/// <returns>
		/// The instantiated main logger.
		/// </returns>
		[CanBeNull]
		protected virtual IPAFServiceDescription CreateMainLogger(
			IPAFServicePipelineObject<IPAFService> serviceObject)
		{
			// Allow for preload.
			if (MainServiceDescription?.ServiceObject != null)
				return MainServiceDescription;

			// TODO KRM - provide ability to override with config file.
			// TODO We will wait to access log4net and whatnot until we have config
			// TODO capabilities
			//
			// Stuff in a default creator if we don't have access to manager
			// internals.
			PAFServiceCreator creator = PAFServiceManager.DefaultServiceCreator;
			var meAsServiceInternals = (IPAFServiceInternal) this;
			var manager = meAsServiceInternals.ServiceManagerInternal;
			var managerInternal = manager as IPAFServiceManagerInternal;
			// Use a custom creator on our manager if we can get one.
			if(managerInternal != null) creator = managerInternal.CreateServiceInternal;

			// We want to filter out any emergency loggers, including ourselves.
			var prohibitedInterfaceType
				= typeof (IPAFEmergencyServiceProvider<IPAFLoggingService>);
			var interfaceFilter = new PAFTypeFilter(
				TypeExtensions.DoesTypeNotImplementInterface, "Exclude Emergency Loggers",
				prohibitedInterfaceType);
			try
			{
				if (MainServiceDescription == null)
				{
					MainServiceDescription = creator
						(serviceObject, MainServiceDescription, interfaceFilter);
				}
				if (MainServiceDescription.ServiceObject == null)
				{
					PAFServiceManager.DefaultLocalServiceInstantiator(serviceObject, MainServiceDescription, interfaceFilter);
				}
			}
			catch (Exception ex) {
				MeAsLogger.LogEntry("From Create Local Logger.", PAFLoggingLevel.Error, ex);
				if (!CanRunWithoutMain)
					throw;
			}
			return MainServiceDescription;
		}
	}
}