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
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.Remoting;
using PlatformAgileFramework.Security;
using PlatformAgileFramework.TypeHandling.Disposal;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This class is a base implementation of the <see cref="IPAFService"/>
	/// interface for use through inheritance. The class also contains necessary
	/// methods for startup and shutdown of services by implementing
	/// <see cref="IPAFServiceExtended"/>.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 07jan2012 </date>
	/// <contribution>
	/// Rewrote the class to provide a base class for supporting all scenarios
	///  - core/extended and local/remote.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	// TODO - KRM. This looks like another situation where we need to move an "Extended"
	// interface into the separate "Extended" contract assembly.
	// ReSharper disable PartialTypeWithSinglePart
// core part.
	public abstract partial class PAFService : IPAFServiceInternal
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// General interface exposed to inheritors. Mainly for proxy
		/// support.
		/// </summary>
		protected IPAFNamedAndTypedObject m_IPAFNamedAndTypedObject;
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized.
		/// </summary>
		protected internal NullableSynchronizedWrapper<bool> m_IsDefaultObject
			= new NullableSynchronizedWrapper<bool>();
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized. It may be <see langword="null"/>.
		/// </summary>
		internal NullableSynchronizedWrapper<IPAFLifetimeManagedObjectInternal<IPAFSecretKeyProvider>> m_LifetimeManagedObjectInternal
			= new NullableSynchronizedWrapper<IPAFLifetimeManagedObjectInternal<IPAFSecretKeyProvider>>();
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized. It may be <see langword="null"/>.
		/// </summary>
		protected internal NullableSynchronizedWrapper<IPAFSecretKey> m_SecurityObjectInternal
			= new NullableSynchronizedWrapper<IPAFSecretKey>();
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized.
		/// </summary>
		protected NullableSynchronizedWrapper<bool> m_ServiceIsInitialized
			= new NullableSynchronizedWrapper<bool>();
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized.
		/// </summary>
		protected NullableSynchronizedWrapper<bool> m_ServiceIsLoaded
			= new NullableSynchronizedWrapper<bool>();
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized.
		/// </summary>
		protected NullableSynchronizedWrapper<bool> m_ServiceIsUnloaded
			= new NullableSynchronizedWrapper<bool>();
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized.
		/// </summary>
		protected NullableSynchronizedWrapper<bool> m_ServiceIsUninitialized
			= new NullableSynchronizedWrapper<bool>();
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized.
		/// </summary>
		protected NullableSynchronizedWrapper<IPAFServiceManager> m_ServiceManager
			= new NullableSynchronizedWrapper<IPAFServiceManager>();
		#region Disposal-Related
		/// <summary>
		/// Holds our surrogate disposer.
		/// </summary>
		protected PAFDisposer<Guid> m_PAFDisposer;
		/// <summary>
		/// Need to hold our secret key because of hierarchical composition.
		/// </summary>
		protected Guid SecretKey { get; set; }
		#endregion // Disposal-Related
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor loads name and type and an optional disposal guid.
		/// </summary>
		/// <param name="guid">
		/// The key that the class instantiator supplies so that the instantiator
		/// is the only one that can dispose the instance. The default
		/// for no argument supplied is "default(Guid)" which is the very
		/// same as <see cref="Guid.Empty"/>.
		/// </param>
		/// <param name="serviceType">
		/// A type for the service. Under almost all circumstances, this should be
		/// an interface type. If the type is the same as another service installed in
		/// a service manager, the name must be different. If the parameter is
		/// <see langword="null"/> the type of "this" is used. Default is the type
		/// of "this". It is best to hide the type of "this" and register the service
		/// by its interface type.
		/// </param>
		/// <param name="serviceName">
		/// A name for the service that is unique within its <paramref name="serviceType"/>.
		/// or <see langword="null"/> or blank. <see langword="null"/> or blank indicates the default service
		/// for the service type. This is entirely adequate for simple scenarios where there
		/// is only one service of a given type. However, there must be only one default
		/// service or the service manager will throw an exception. Default = blank, which
		/// is what is installed in the dictionary. Service managers that allow multiple
		/// instances of the same service type often employ a factory to auto-generate
		/// names.
		/// </param>
		protected internal PAFService(Guid guid = default(Guid),
			Type serviceType = null, string serviceName = null)
		{
			if (serviceName == null) serviceName = "";
			if(serviceType == null)
				serviceType = GetType();

			m_IPAFNamedAndTypedObject = new PAFNamedAndTypedObject(serviceType, serviceName, this);

			// We build the disposer with the Guid and our instance and the disposal delegate.
			m_PAFDisposer = new PAFDisposer<Guid>(guid, this, PAFFrameworkServiceDispose);
			DisposalRegistry.RegisterForDisposal(m_PAFDisposer);
		}
		#endregion
		#region Novel Members
		#region Novel Properties
		/// <summary>
		/// Backing for the "LifetimeManagedObject" and "LifetimeManagedObjectInternal"
		/// property. Synchronized implementation.
		/// </summary>
		internal virtual IPAFLifetimeManagedObjectInternal<IPAFSecretKeyProvider> LifetimeManagedObjectIV
		{
			get { return m_LifetimeManagedObjectInternal.NullableObject; }
			set { m_LifetimeManagedObjectInternal.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "SecurityObjectInternal"
		/// property. Synchronized implementation.
		/// </summary>
		protected internal virtual IPAFSecretKey SecurityObjectPIV
		{
			get { return m_SecurityObjectInternal.NullableObject; }
			set { m_SecurityObjectInternal.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServiceIsInitialized" property. Synchronized implementation.
		/// </summary>
		protected internal virtual bool ServiceIsInitializedPIV
		{
			get { return m_ServiceIsInitialized.NullableObject; }
			set { m_ServiceIsInitialized.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServiceIsLoaded" property. Synchronized implementation.
		/// </summary>
		protected internal virtual bool ServiceIsLoadedPIV
		{
			get { return m_ServiceIsLoaded.NullableObject; }
			set { m_ServiceIsLoaded.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServiceIsUninitialized" property. Synchronized implementation.
		/// </summary>
		protected internal virtual bool ServiceIsUninitializedPIV
		{
			get { return m_ServiceIsUninitialized.NullableObject; }
			set { m_ServiceIsUninitialized.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServiceIsUnloaded" property. Synchronized implementation.
		/// </summary>
		protected internal virtual bool ServiceIsUnloadedPIV
		{
			get { return m_ServiceIsUnloaded.NullableObject; }
			set { m_ServiceIsUnloaded.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServiceManager" property. Synchronized implementation.
		/// </summary>
		protected internal virtual IPAFServiceManager ServiceManagerPIV
		{
			get { return m_ServiceManager.NullableObject; }
			set { m_ServiceManager.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServicesRequiredForInitialize" property. Base class just
		/// returns <see langword="null"/>.
		/// </summary>
		protected internal virtual IEnumerable<IPAFServiceDescription> ServicesRequiredForInitializationPIV
		{
			get { return null; }
		}
		/// <summary>
		/// Backing for the "ServicesRequiredForLoad" property. Base version
		/// returns <see langword="null"/>.
		/// </summary>
		protected internal virtual IEnumerable<IPAFServiceDescription> ServicesRequiredForLoadPIV
		{
			get { return null; }
		}
		#endregion // Novel Properties
		#region Novel Methods
		/// <summary>
		/// Just fires the <see cref="Initialize"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected internal virtual void InitializeServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnInitialize(EventArgs.Empty);
			ServiceIsInitializedPIV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Load"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected internal virtual void LoadServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnLoad(EventArgs.Empty);
			ServiceIsLoadedPIV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Uninitialize"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected internal virtual void UninitializeServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnUninitialize(EventArgs.Empty);
			ServiceIsUninitializedPIV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Unload"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected internal virtual void UnloadServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnUnload(EventArgs.Empty);
			ServiceIsUnloadedPIV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Update"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected internal virtual void UpdateServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnUpdate(EventArgs.Empty);
		}
		#region Disposal-Related
		/// <summary>
		/// <para>
		/// This method is not virtual. The developer of any subclass must not be
		/// allowed to change the logic. This method is marked as
		/// <see cref="SecurityCriticalAttribute"/>, so it can only be called in
		/// elevated-trust environments.
		/// </para>
		/// </summary>
		[SecurityCritical]
		public void Dispose()
		{
			m_PAFDisposer.Dispose();
		}
		/// <summary>
		/// <see cref="IPAFDisposable"/>. This is a method that is supplied as a delegate
		/// to the disposer to call during disposal.
		/// </summary>
		/// <param name="disposing">
		/// <see cref="IPAFDisposable"/>.
		/// </param>
		/// <param name="obj">
		/// <see cref="IPAFDisposable"/>.
		/// This is not used in this method.
		/// </param>
		/// <returns>
		/// <see cref="IPAFDisposable"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// When subclassing this class (or a class like it), this is the method that should
		/// be overridden. Obviously the designer of the subclass should keep in mind the order
		/// of resource disposal that should be followed and call the base at the appropriate
		/// point (usually after the subclass call, but not always).
		/// </para>
		/// <para>
		/// Exceptions are caught and recorded in the registry.
		/// </para>
		/// </remarks>
		protected virtual Exception PAFFrameworkServiceDispose(bool disposing, object obj)
		{
			var eList = new List<Exception>();
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_LifetimeManagedObjectInternal, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_SecurityObjectInternal, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsInitialized, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsLoaded, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsUninitialized, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsUnloaded, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceManager, true));

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0) {
				var exceptions = new PAFAggregateExceptionData(eList);
				var ex = new PAFStandardException<PAFAggregateExceptionData>(exceptions);
				// Seal the list.
				exceptions.AddException(null);
				// We just put these in the registry.
				DisposalRegistry.RecordDisposalException(this, ex);
				return ex;
			}
			return null;
		}
		#endregion // Disposal-Related
		#endregion // Novel Methods
		#endregion // Novel Members
		#region Implementation of IPAFFrameworkServiceExtended
		#region Properties
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IPAFLifetimeManagedObject<IPAFSecretKeyProvider> IPAFServiceExtended.LifetimeManagedObject
		{
			get { return LifetimeManagedObjectIV; }
		}

		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IPAFServiceManager IPAFServiceExtended.ServiceManager
		{
			get { return ServiceManagerPIV; }
			[SecurityCritical]
			set { ServiceManagerPIV = value; }
		}

		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsInitialized
		{
			get { return ServiceIsInitializedPIV; }
			[SecurityCritical]
			set { ServiceIsInitializedPIV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsLoaded
		{
			get { return ServiceIsLoadedPIV; }
			[SecurityCritical]
			set { ServiceIsLoadedPIV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsUninitialized
		{
			get { return ServiceIsUninitializedPIV; }
			[SecurityCritical]
			set { ServiceIsUninitializedPIV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsUnloaded
		{
			get { return ServiceIsUnloadedPIV; }
			[SecurityCritical]
			set { ServiceIsUnloadedPIV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceExtended.ServicesRequiredForInitialization
		{
			[SecurityCritical]
			get { return ServicesRequiredForInitializationPIV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceExtended.ServicesRequiredForLoad
		{
			[SecurityCritical]
			get { return ServicesRequiredForLoadPIV; }
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Just fires the <see cref="Initialize"/> event. See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		[SecurityCritical]
		void IPAFServiceExtended.InitializeService(IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{ InitializeServicePIV(servicePipelineObject); }
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		[SecurityCritical]
		void IPAFServiceExtended.LoadService(IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{ LoadServicePIV(servicePipelineObject); }
		/// <summary>
		/// Just fires the <see cref="Uninitialize"/> event. See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		[SecurityCritical]
		void IPAFServiceExtended.UninitializeService(IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{ UninitializeServicePIV(servicePipelineObject); }
		/// <summary>
		/// Just fires the <see cref="Unload"/> event. See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		[SecurityCritical]
		void IPAFServiceExtended.UnloadService(IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{ UnloadServicePIV(servicePipelineObject); }
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// Just fires the <see cref="Update"/> event. See <see cref="IPAFServiceExtended"/>.
		/// </param>
		[SecurityCritical]
		void IPAFServiceExtended.UpdateService(IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{ UpdateServicePIV(servicePipelineObject); }
		#endregion // Methods
		/// <summary>
		/// Dispatches the Initialize event.
		/// </summary>
		protected virtual void OnInitialize(EventArgs e)
		{
		    var eventHandlerSnapshot
		        = AtomicUtils.GetNullableItem(ref Initialize);

		    eventHandlerSnapshot?.Invoke(this, e);
		}
		/// <summary>
		/// Dispatches the Load event.
		/// </summary>
		protected virtual void OnLoad(EventArgs e)
		{
		    Load?.Invoke(this, e);
		}

		/// <summary>
		/// Dispatches the Uninitialize event.
		/// </summary>
		protected virtual void OnUninitialize(EventArgs e)
		{
		    var eventHandlerSnapshot
		        = AtomicUtils.GetNullableItem(ref Uninitialize);

		    eventHandlerSnapshot?.Invoke(this, e);
		}

		/// <summary>
		/// Dispatches the Unload event.
		/// </summary>
		protected virtual void OnUnload(EventArgs e)
		{
		    var eventHandlerSnapshot
		        = AtomicUtils.GetNullableItem(ref Unload);

		    eventHandlerSnapshot?.Invoke(Unload, e);
		}

		/// <summary>
		/// Dispatches the Update event.
		/// </summary>
		protected virtual void OnUpdate(EventArgs e)
		{
		    var eventHandlerSnapshot
		        = AtomicUtils.GetNullableItem(ref Update);

		    eventHandlerSnapshot?.Invoke(Update, e);
		}

		// Placeholders - put events in when we get safeevents converted.
		/// <remarks/>
		public event EventHandler Initialize;
		/// <remarks/>
		public event EventHandler Load;
		/// <remarks/>
		public event EventHandler Uninitialize;
		/// <remarks/>
		public event EventHandler Unload;
		/// <remarks/>
		public event EventHandler Update;
		#endregion // Implementation of IPAFFrameworkServiceExtended

		#region Implementation of IPAFFrameworkServiceInternal

		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		IPAFSecretKey IPAFServiceInternal.SecurityObjectInternal
		{
			get { return SecurityObjectPIV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		IPAFLifetimeManagedObjectInternal<IPAFSecretKeyProvider> IPAFServiceInternal.LifetimeManagedObjectInternal
		{
			get { return LifetimeManagedObjectIV; }
			set { LifetimeManagedObjectIV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceInternal.ServicesRequiredForInitializationInternal
		{
			get { return ServicesRequiredForInitializationPIV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		IPAFServiceManager IPAFServiceInternal.ServiceManagerInternal
		{
			get { return ServiceManagerPIV; }
			set { ServiceManagerPIV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceInternal.ServicesRequiredForLoadInternal
		{
			get { return ServicesRequiredForLoadPIV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="serviceIsInitialized">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.SetServiceIsInitialized(bool serviceIsInitialized)
		{
			ServiceIsInitializedPIV = serviceIsInitialized;
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="serviceIsLoaded">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.SetServiceIsLoaded(bool serviceIsLoaded)
		{
			ServiceIsLoadedPIV = ServiceIsLoadedPIV;
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="serviceIsUninitialized">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.SetServiceIsUninitialized(bool serviceIsUninitialized)
		{
			ServiceIsUninitializedPIV = serviceIsUninitialized;
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="serviceIsUnloaded">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.SetServiceIsUnloaded(bool serviceIsUnloaded)
		{
			ServiceIsUnloadedPIV = serviceIsUnloaded;
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.InitializeServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			InitializeServicePIV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.LoadServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			LoadServicePIV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.UninitializeServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			UninitializeServicePIV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.UnloadServiceInternal(
			IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			UnloadServicePIV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceInternal"/>.
		/// </param>
		void IPAFServiceInternal.UpdateServiceInternal(
			IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			UpdateServicePIV(pipelineObject);
		}
		#endregion
		#region Implementation of IPAFNamedAndTypedObject
		#region Implementation of IPAFNamedObject
		/// <summary>
		/// See <see cref="IPAFNamedObject"/>. Returns the name of the service.
		/// </summary>
		/// <remarks>
		/// See <see cref="IPAFNamedObject"/>. Name can be blank, indicating a default
		/// service for the given service type.
		/// </remarks>
		public string ObjectName
		{
			get { return m_IPAFNamedAndTypedObject.ObjectName; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObject.ObjectName = value; }
		}
		#endregion
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>.
		/// </summary>
		public string AssemblyQualifiedObjectType
		{
			get { return m_IPAFNamedAndTypedObject.AssemblyQualifiedObjectType; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObject.AssemblyQualifiedObjectType = value; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>.
		/// </summary>
		public bool IsDefaultObject
		{
			get { return m_IPAFNamedAndTypedObject.IsDefaultObject; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObject.IsDefaultObject = value; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>. This should normally be
		/// an interface type.
		/// </summary>
		public Type ObjectType
		{
			get { return m_IPAFNamedAndTypedObject.ObjectType; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObject.ObjectType = value; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>. Returns the actual
		/// service object.
		/// </summary>
		public object ObjectValue
		{
			get { return m_IPAFNamedAndTypedObject.ObjectValue; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObject.ObjectValue = value; }
		}
		#endregion
	}
}
