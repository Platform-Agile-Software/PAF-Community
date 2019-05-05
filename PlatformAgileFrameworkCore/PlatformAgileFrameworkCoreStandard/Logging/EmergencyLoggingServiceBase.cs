//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using System.Diagnostics.CodeAnalysis;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.FrameworkServices.Exceptions;
using PlatformAgileFramework.Logging.Exceptions;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;
// Exception shorthand.
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using IPAFELED = PlatformAgileFramework.Logging.Exceptions.IPAFEmergencyLoggerExceptionData;
using PAFELED = PlatformAgileFramework.Logging.Exceptions.PAFEmergencyLoggerExceptionData;



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
	/// <author> KRM </author>
	/// <date> 12dec2018 </date>
	/// <description>
	/// Refactored to always allow access to the emergency logger. Other
	/// loggers need it.
	/// </description>
	/// </contribution>
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
	/// Added pre-load capability for storage service.
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
	// partial - wind down is now in extended.
	public partial class EmergencyLoggingServiceBase : PAFServiceExtended,
		IPAFLoggingService, IPAFEmergencyServiceProvider<IPAFLoggingService>, IPAFLogfileReader
	{
		#region Implementing Nested Class
		/// <summary>
		/// Nested class exists just to provide the "emergency" functionality of the logger
		/// which always needs to be accessible. Errors in the main loggers may require
		/// logging to the emergency logger. We provide this as a one-off, since only one
		/// instance is anticipated.
		/// </summary>
		protected class EmergencyLoggerImplementation : PAFService, IPAFLoggingService
		{
			/// <summary>
			/// Enclosing class access..........
			/// </summary>
			protected internal readonly EmergencyLoggingServiceBase m_LoggerBase;
			/// <summary>
			/// We don't need any inputs, since this is never put in the SM dictionary.
			/// </summary>
			public EmergencyLoggerImplementation(EmergencyLoggingServiceBase loggerBase)
			{
				m_LoggerBase = loggerBase;
			}

			/// <summary>
			/// Emergency logger is never paused by default.
			/// </summary>
			public virtual bool IsPaused
			{
				get => false;
				set { }
			}

			#region Implementation of IPAFLoggingService
			/// <remarks>
			/// <see cref="IPAFLoggingService"/> 
			/// </remarks>
			public virtual void LogEntry(object message, PAFLoggingLevel logLevel = PAFLoggingLevel.Default,
				Exception exception = null)
			{
				// ReSharper disable once InconsistentlySynchronizedField
				var loggedString = m_LoggerBase.m_HeaderPrefixText + "  (" + DateTime.Now + ")" + ":" + PlatformUtils.LTRMN;
				if (message != null) loggedString += "Message: " + message + PlatformUtils.LTRMN;
				// TODO awaiting general exception renderer conversion.
				if (exception != null) loggedString += "Exception Message: " + exception.Message;
				lock (s_FileLockObject)
				{
					using (var stream = m_LoggerBase.GetStorageService().PAFOpenFile(m_LoggerBase.m_EmergencyLogFilePath, PAFFileAccessMode.APPEND))
					{
						stream.PAFWriteString(loggedString);
					}
				}
			}
			#region Implementation of IPAFFileWriter
			/// <remarks>
			/// <see cref="IPAFFileWriter"/> 
			/// </remarks>
			public void WriteDataEntry(string dataEntry)
			{
				m_LoggerBase.WriteDataEntry(dataEntry);
			}
			#endregion // Implementation of IPAFFileWriter
			#endregion // Implementation of IPAFLoggingService
		}
		#endregion // Implementing Nested Class
		#region Class Fields and Autoproperties
		/// <summary>
		/// Tells whether we should throw an exception if main logger cannot be built.
		/// When loggers are built in an "AppDomain", the creator of
		/// the "AppDomain" may wish to allow the failure of the main logger.
		/// Default = <see langword="false"/>.
		/// </summary>
		protected internal static bool s_CanRunWithoutMain { get; set; }
		/// <summary>
		/// Holds the default name. File will be created in the startup CWD. This will be
		/// the add-in root for the add-in framework, the isolated storage root
		/// for SL and the app dir for ECMA/CLR. Mobiles define their own.
		/// </summary>
		public const string DEFAULT_EMERGENCY_FILE = "EmergencyLog.log";
		/// <summary>
		/// Holds the default log header text.
		/// </summary>
		public const string DEFAULT_HEADER_TEXT = "Emergency Logger Entry";
		/// <summary>
		/// Emergency functionality to delegate to.
		/// </summary>
		protected EmergencyLoggerImplementation m_EmergencyLoggerImplementation;
		/// <summary>
		/// This is the message that is put into the file when the emergency logger
		/// loads on startup.
		/// </summary>
		public const string LOAD_MESSAGE = "Emergency Logger Loaded";
		/// <summary>
		/// Holds the emergency file path with terminating name. Can be loaded by the constructor.
		/// To ensure thread safety, load this before publishing the service.
		/// </summary>
		protected internal readonly string m_EmergencyLogFilePath;
		/// <summary>
		/// This is the lookup name in the service dictionary for the emergency logging
		/// service which is always there.
		/// </summary>
		public const string EMERGENCY_LOGGER_NAME = "DefaultEmergencyLogger";
		/// <summary>
		/// This needs to be static, since we are contending for one file. The lock should
		/// really be a system-wide mutex, but the emergency logger is normally only instantiated
		/// in the Main AppDomain or process, anyway, so we don't worry about it.
		/// </summary>
		private static readonly object s_FileLockObject = new object();
		/// <summary>
		/// Handle on myself.
		/// </summary>
		protected internal IPAFLoggingService MeAsLogger { get; }
		/// <summary>
		/// The main logger is <see langword="null"/> until constructed.
		/// </summary>
		public IPAFLoggingService MainService { get; set; }
		/// <summary>
		/// This is an extension point for the logging system. A description of a main logger that
		/// is installed when the requisite services it needs are available.
		/// </summary>
		public IPAFServiceDescription MainServiceDescription
		{ get; set; }

		/// <summary>
		/// This is the text that is printed above each log message. date/time is printed at the end of this text.
		/// </summary>
		protected internal readonly string m_HeaderPrefixText;
		/// <summary>
		/// Tells whether service has been created. We are not designed with a singleton
		/// pattern. Thus just prevents us from being created multiple times
		/// by somebody on the OUTSIDE - the SM won't do it. PAF supports
		/// the concept of hierarchical loggers. This is the "root" logger
		/// and is the only one supplied in Core. There is only one of these
		/// allowed in any app domain. four-byte data is atomic.
		/// </summary>
		protected internal static volatile bool s_ServiceIsCreated;
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
		public bool TruncateFileOnStart { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Initializes a new instance of us.
		/// </summary>
		/// <param name="mainServiceDescription">
		/// This optional parameter provides a description of the "main" service to be switched to in 
		/// the service "initialize" stage. Default is to just look for somebody implementing
		/// <see cref="IPAFLoggingService"/> interface inside loaded assemblies, other than us.
		/// </param>
		/// <param name="emergencyLogFilePath">
		/// This optional parameter allows the name of an emergency plain text logging file
		/// to be specified. Default = <see cref="DEFAULT_EMERGENCY_FILE"/> with
		/// <see cref="PlatformUtils.ApplicationRoot"/> prepended. If supplied through the
		/// constructor, it must be a complete file path, including directory.
		/// </param>
		/// <param name="truncateOnStart">Sets <see cref="TruncateFileOnStart"/>.</param>
		/// <param name="headerText">Sets <see cref="m_HeaderPrefixText"/>.</param>
		/// <exception> <see cref="PAFStandardException{IPAFServiceExceptionData}"/> is thrown if
		/// an instance of the class is already constructed.
		/// <see cref="PAFServiceExceptionMessageTags.SERVICE_ALREADY_CREATED"/>
		/// </exception>
		protected internal EmergencyLoggingServiceBase(IPAFServiceDescription mainServiceDescription = null,
			string emergencyLogFilePath = null, bool truncateOnStart = true, string headerText = null)
		{
			if (s_ServiceIsCreated)
			{
				var exceptionData
					= new PAFSED(new PAFServiceDescription(PAFTypeHolder.IHolder(GetType())));
				throw new PAFStandardException<IPAFSED>(exceptionData, PAFServiceExceptionMessageTags.SERVICE_ALREADY_CREATED);
			}

			// Default if not incoming.
			m_HeaderPrefixText = DEFAULT_HEADER_TEXT;
			if (headerText != null)
				m_HeaderPrefixText = headerText;

			// Default if not incoming.
			if (emergencyLogFilePath == null)
			{
				m_EmergencyLogFilePath = DEFAULT_EMERGENCY_FILE;
				if (!string.IsNullOrEmpty(PlatformUtils.ApplicationRoot))
				{
					m_EmergencyLogFilePath = PlatformUtils.ApplicationRoot
						+ PlatformUtils.GetDirectorySeparatorChar() + m_EmergencyLogFilePath;
				}
			}
			else
			{
				m_EmergencyLogFilePath = emergencyLogFilePath;
			}

			// Default if not incoming.
			MainServiceDescription = mainServiceDescription ??
				new PAFServiceDescription(PAFTypeHolder.IHolder(typeof(IPAFLoggingService)));

			TruncateFileOnStart = truncateOnStart;

			MeAsLogger = this;

			ObjectName = EMERGENCY_LOGGER_NAME;

			m_EmergencyLoggerImplementation = new EmergencyLoggerImplementation(this);

			s_ServiceIsCreated = true;
		}
		#endregion // Constructors
		#region Properties
		#region IPAFFrameworkServiceExtended Overrides
		/// <summary>
		/// Override to tell the SM that we need storage service before we can be loaded.
		/// The reason is because the manufacturing utils that we use to create loggers
		/// through loading and reflection need to log their progress.
		/// </summary>
		protected override IEnumerable<IPAFServiceDescription>
			ServicesRequiredForLoadPV
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
		#region Novel Methods

		/// <summary>
		/// Gets storage service used to write logs with. If <see cref="StorageService"/> has
		/// not been loaded, we'll look it up on the service manager if the manager is initialized.
		/// If not, we look it up as <see cref="PAFServices.SiPAFStorageService.Service"/> which must be
		/// preloaded. If the SM has been initialized, it's always assumed that the final storage
		/// service has been provisioned. In that case, we load <see cref="StorageService"/> from
		/// the provisioned service.
		/// </summary>
		/// <returns>
		/// Can be <see langword="null"/>, but there is no way to recover in this class.
		/// </returns>
		[SuppressMessage("ReSharper", "InvalidXmlDocComment")]
		[CanBeNull]protected virtual IPAFStorageService GetStorageService()
		{
			if (StorageService != null)
				return StorageService;
			IPAFStorageService storageService;
			if (PAFServices.IsInitialized)
			{
				storageService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
				StorageService = storageService;
			}
			else
			{
				storageService = PAFServices.SiPAFStorageServiceInternal.Service;
			}

			return storageService;
		}
		#endregion // Novel Methods

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
		// This is called only by the initialization thread.
		[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
		protected override void LoadServicePV(
			IPAFServicePipelineObject<IPAFService> serviceObject)
		{
			if (StorageService == null)
				StorageService = (IPAFStorageService)serviceObject.ServiceManager.GetService(typeof(IPAFStorageService));
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
			base.LoadServicePV(serviceObject);
		}
		/// <summary>
		/// Override to initialize ourselves with the main service.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		protected override void InitializeServicePV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			var serviceDescription = CreateMainLogger(servicePipelineObject);
			if (serviceDescription != null)
			{
				MainServiceDescription = serviceDescription;
				MainService = (IPAFLoggingService)MainServiceDescription.ServiceObject;
			}
			base.InitializeServicePV(servicePipelineObject);
		}
		#endregion // IPAFFrameworkServiceExtended Overrides
		/// <remarks>
		/// Pause the logger.
		/// </remarks>
		public bool IsPaused { get; set; }
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
			if ((MainService != null) && (MainService != this))
			{
				// We never pause the emergency logger - just the main.
				if (IsPaused) return;
				MainService.LogEntry(message, logLevel, exception);
				return;
			}
			m_EmergencyLoggerImplementation.LogEntry(message, logLevel, exception);
		}
		#region IPAFFileWriter Implementation
		/// <summary>
		/// <see cref="IPAFFileWriter"/>
		/// </summary>
		/// <param name="dataEntry">Entry to write.</param>
		/// <remarks>
		/// We invert the logic path in this one - just writes through <see cref="LogEntry"/>.
		/// </remarks>
		public virtual void WriteDataEntry(string dataEntry)
		{
			LogEntry(dataEntry);
		}
		#endregion // IPAFFileWriter Implementation
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
		///	Returns the emergency logger, which writes to the application root.
		/// This is normally ONLY needed when other loggers fail.
		/// </summary>
		IPAFLoggingService IPAFEmergencyServiceProvider<IPAFLoggingService>.EmergencyService
		{
			get { return m_EmergencyLoggerImplementation; }
		}
		#endregion // IPAFEmergencyServiceProvider Implementation
		#endregion // Methods
		/// <summary>
		/// Initializes the main service and logs a message with the
		/// emergency logger telling if we failed.
		/// </summary>
		/// <param name="serviceObject">
		/// Callees need <see cref="IPAFServicePipelineObject.ServiceManager"/>.
		/// </param>
		/// <returns>
		/// The instantiated main logger. Will return <see langword="null"/> if
		/// main logger can't be created and <see cref="s_CanRunWithoutMain"/> is <see langword="true"/>
		/// </returns>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFELED}">
		/// <see cref="PAFEmergencyLoggerExceptionMessageTags.ERROR_WRITING_WITH_EMERGENCY_LOGGER"/>
		/// if emergency logger doesn't even work to log main logger construction failure.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFELED}">
		/// <see cref="PAFEmergencyLoggerExceptionMessageTags.CANNOT_CREATE_MAIN_LOGGER"/>
		/// if main logger can't be created and <see cref="s_CanRunWithoutMain"/> is <see langword="false"/>.
		/// </exception>
		/// </exceptions>
		[CanBeNull]
		protected virtual IPAFServiceDescription CreateMainLogger(
			IPAFServicePipelineObject<IPAFService> serviceObject)
		{
			// Allow for pre-load.
			if (MainServiceDescription?.ServiceObject != null)
				return MainServiceDescription;

			// Reporting for main logger failure if we can't run without it.
			Exception mainLoggerConstructionException = null;

			// TODO KRM - provide ability to override with config file.
			// TODO We will wait to access log4net and whatnot until we have config
			// TODO capabilities
			//
			// Stuff in a default creator if we don't have access to manager
			// internals.
			PAFServiceCreator creator = PAFServiceManager.DefaultServiceCreator;
			var meAsServiceInternals = (IPAFServiceExtendedInternal)this;
			var manager = meAsServiceInternals.ServiceManager;
			// Use a custom creator on our manager if we can get one.
			if (manager is IPAFServiceManagerInternal managerInternal)
				creator = managerInternal.CreateServiceInternal;

			// We want to filter out any emergency loggers, including ourselves.
			var prohibitedInterfaceType
				= typeof(IPAFEmergencyServiceProvider<IPAFLoggingService>);
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
			catch (Exception ex)
			{
				mainLoggerConstructionException = ex;
			}

			if (mainLoggerConstructionException == null)
				return MainServiceDescription;

			/////////////////////////////////////////////////////////////////////////////////////
			// If we failed to construct the main logger, we at least have to report the failure.
			/////////////////////////////////////////////////////////////////////////////////////
			Exception emergencyLoggerLoggingException = null;
			try
			{
				MeAsLogger.LogEntry("From Create Local Logger.", PAFLoggingLevel.Error, mainLoggerConstructionException);
			}
			catch (Exception ex)
			{
				emergencyLoggerLoggingException = ex;
			}

			if (emergencyLoggerLoggingException != null)
			{
				// If the emergency logger is broke, we MUST fail the app.
				var exceptionData
					= new PAFELED(emergencyLoggerLoggingException, mainLoggerConstructionException);
				throw new PAFStandardException<IPAFELED>(exceptionData, PAFEmergencyLoggerExceptionMessageTags.ERROR_WRITING_WITH_EMERGENCY_LOGGER);
			}

			/////////////////////////////////////////////////////////////////////////////////////
			// If we got here, we reported the main logger creation failure correctly. If
			// we can't run without it, we throw an exception.
			/////////////////////////////////////////////////////////////////////////////////////
			if (!s_CanRunWithoutMain)
			{
				var exceptionData
					= new PAFELED(null, mainLoggerConstructionException);
				throw new PAFStandardException<IPAFELED>(exceptionData, PAFEmergencyLoggerExceptionMessageTags.CANNOT_CREATE_MAIN_LOGGER);

			}
			return null;
		}
	}
}