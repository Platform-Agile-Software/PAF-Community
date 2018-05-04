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
using PlatformAgileFramework.Remoting;
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This interface must be implemented by all services wishing to provide
	/// a PAF Framework service. These are the secured properties and methods
	/// that are only exposed as security-critical.
	/// </para>
	/// <para>
	/// The service manager implementation follows a pipeline model with load, initialize,
	/// uninitialize and unload stages. Any service can be constructed at any time. The service
	/// is then responsible for declaring any services it needs to access during the
	/// load stage. After the service is loaded, it is compelled to declare any services it
	/// needs to access during the initialize stage.
	/// </para>
	/// <para>
	/// The service manager uses the service dependency information to resolve deadlocks by
	/// scheduling service load/initialization for the various services. Services can provide
	/// "emergency" versions of themselves that need few or no other services to be loaded
	/// and/or initialized. The service manager can then provide an early instantiation of the
	/// service to break deadlocks.
	/// </para>
	/// </summary>
	/// <remarks>
	/// <para>
	/// In PAF core, there are no out-of-the-box services provided that implement the
	/// <see cref="IPAFServiceExtended"/> interface in a non-trivial way.
	/// However, the standard base <see cref="PAFGeneralServiceManager{T}"/> supports manipulating
	/// such services.
	/// </para>
	/// <para>
	/// <see cref="IDisposable"/> is normally implemented by all framework services. It
	/// is not placed on the contract inerface, since we don't want the outside world
	/// to necessarily have access to it through the contract interface.
	/// </para>
	/// </remarks>
	/// <history>
	/// <author> DAP </author>
	/// <date> 04jan2012 </date>
	/// <contribution>
	/// Changed to a public interface. 4.0 security allows us to expose it and simply
	/// leave the methods security critical. Documented the service dependency mechanism
	/// more carefully. Also changed all references to "service broker" to "service manager"
	/// to avoid confusion with MS stuff. We used the name long before MS did, but that's
	/// just tough, I guess.
	/// </contribution>
	/// </history>
	public interface IPAFServiceExtended : IPAFService, IDisposable
	{
		#region Properties
		/// <summary>
		/// Manipulates the LMO for remote services. Implementation not provided in Core.
		/// </summary>
		IPAFLifetimeManagedObject<IPAFSecretKeyProvider> LifetimeManagedObject { get; }
		/// <summary>
		/// This property gets the manager associated with the service. It is often
		/// used to climb a tree of managers that are constructed hierarchically.
		/// </summary>
		IPAFServiceManager ServiceManager
		{ get; [SecurityCritical]set; }
		//////////////////////////////////////////////////////////////////////////////////////////
		// Following props may be set by either the service manager or the service itself. Service
		// manager will set if it calls the corresponding method, service will do it if it
		// wants to short-circuit any steps.
		//////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This property determines whether a service has already been initialized.
		/// </summary>
		bool ServiceIsInitialized
		{ get; [SecurityCritical] set; }
		/// <summary>
		/// This property determines whether a service has already been loaded.
		/// </summary>
		bool ServiceIsLoaded
		{ get; [SecurityCritical] set; }
		/// <summary>
		/// This property determines whether a service has already been uninitialized.
		/// </summary>
		bool ServiceIsUninitialized
		{ get; [SecurityCritical] set; }
		/// <summary>
		/// This property determines whether a service has already been unloaded.
		/// </summary>
		bool ServiceIsUnloaded
		{ get; [SecurityCritical] set; }
		//////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// These are the services that must be present in order for this service to be
		/// initialized. These are the names of the service interfaces or classes prefixed
		/// with namespaces. These can also be assembly-qualified names or a mix. Can
		/// return <see langword="null"/> if no prerequisites for initialization.
		/// </summary>
		IEnumerable<IPAFServiceDescription> ServicesRequiredForInitialization
		{ [SecurityCritical] get; }
		/// <summary>
		/// These are the services that must be present in order for this service to be
		/// loaded. These are the names of the service interfaces or classes prefixed
		/// with namespaces. Can return <see langword="null"/> if no prerequisites. If these
		/// minimal prerequisites cannot be met, the service manager will generally
		/// throw an exception.
		/// </summary>
		IEnumerable<IPAFServiceDescription> ServicesRequiredForLoad { [SecurityCritical] get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// This method is called to create the "full" service, which may be different
		/// than the "emergency" service.
		/// </summary>
		/// <param name="pipelineObject">
		/// <see cref="IPAFServicePipelineObject"/> that can carry application-specific data.
		/// </param>
		[SecurityCritical]
		void InitializeService(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// This method is called to load service without initializing it.
		/// Service manager needs to load/initialize services in certain order
		/// sometimes. Note that after some services are loaded, they must be able
		/// to be called upon to perform their advertised services. In these cases,
		/// an "emergency service" may be used. For example, an emergency version
		/// of a logging service may simply write to a local file.
		/// </summary>
		/// <param name="pipelineObject">
		/// <see cref="IPAFServicePipelineObject"/> that can carry application-specific data.
		/// </param>
		[SecurityCritical]
		void LoadService(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// This method is called before the service is unloaded. In some special cases,
		/// we need uninitialize and unload stages.
		/// </summary>
		/// <param name="pipelineObject">
		/// <see cref="IPAFServicePipelineObject"/> that can carry application-specific data.
		/// </param>
		[SecurityCritical]
		void UninitializeService(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// This method is called to unload the service.
		/// </summary>
		/// <param name="pipelineObject">
		/// <see cref="IPAFServicePipelineObject"/> that can carry application-specific data.
		/// </param>
		[SecurityCritical]
		void UnloadService(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// This method is called whenever a service is updated, say with a culture
		/// change. It is also used to call services after an initial call to InitializeService
		/// has indicated a deadlock had to be broken by overriding a "soft" dependency
		/// in order to get services initialized. This gives the deadlock-breaking service
		/// a chance to complete its initialization.
		/// </summary>
		/// <param name="pipelineObject">
		/// <see cref="IPAFServicePipelineObject"/> that can carry application-specific data.
		/// </param>
		[SecurityCritical]
		void UpdateService(IPAFServicePipelineObject<IPAFService> pipelineObject);
		#endregion // Methods
		#region Events
		/// <summary>
		/// Event to broadcast "Initialize". This event is fired after initialization is complete.
		/// </summary>
		event EventHandler Initialize;

		/// <summary>
		/// Event to broadcast "Load". This event is fired after loading is complete.
		/// </summary>
		event EventHandler Load;

		/// <summary>
		/// Event to broadcast "Uninitialize". This event is fired after uninitialization is complete.
		/// </summary>
		event EventHandler Uninitialize;

		/// <summary>
		/// Event to broadcast "Unload". This event is fired after unloading is complete.
		/// </summary>
		event EventHandler Unload;

		/// <summary>
		/// Event to broadcast "Update". This event is fired after updating is complete.
		/// </summary>
		event EventHandler Update;
		#endregion // Events
	}
}
