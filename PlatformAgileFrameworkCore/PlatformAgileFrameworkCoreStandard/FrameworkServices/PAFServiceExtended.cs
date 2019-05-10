//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
	/// <author> Brian T. </author>
	/// <date> 07jan2018 </date>
	/// <contribution>
	/// New. Necessitated by making "IsDefault" internal and to allow
	/// "non-Extended" services to be built with <see cref="PAFService"/>.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	// ReSharper disable PartialTypeWithSinglePart
// core part.
	public abstract partial class PAFServiceExtended: PAFService,
		IPAFServiceExtendedInternal
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties
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
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor loads name and type and an optional disposal guid.
		/// </summary>
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
		/// <param name="guid">
		/// The key that the class instantiator supplies so that the instantiator
		/// is the only one that can dispose the instance. The default
		/// for no argument supplied is "default(Guid)" which is the very
		/// same as <see cref="Guid.Empty"/>.
		/// </param>
		protected internal PAFServiceExtended(Type serviceType = null, string serviceName = null,
			Guid guid = default(Guid))
			:base(serviceType, serviceName, guid)
		{
		}
		#endregion
		#region Novel Members
		#region Novel Properties
		/// <summary>
		/// Backing for the "LifetimeManagedObject" and "LifetimeManagedObjectInternal"
		/// property. Synchronized implementation.
		/// </summary>
		// ReSharper disable once InconsistentNaming
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
		protected internal bool ServiceIsInitializedPV
		{
			get { return m_ServiceIsInitialized.NullableObject; }
			set
			{
				// Service is initialized can short-circuit load.
				if (value)
					m_ServiceIsLoaded.NullableObject = true; 
				m_ServiceIsInitialized.NullableObject = value;
			}
		}
		/// <summary>
		/// Backing for the "ServiceIsLoaded" property. Synchronized implementation.
		/// </summary>
		protected internal bool ServiceIsLoadedPV
		{
			get { return m_ServiceIsLoaded.NullableObject; }
			set { m_ServiceIsLoaded.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServiceIsUninitialized" property. Synchronized implementation.
		/// </summary>
		protected internal bool ServiceIsUninitializedPV
		{
			get { return m_ServiceIsUninitialized.NullableObject; }
			set { m_ServiceIsUninitialized.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServiceIsUnloaded" property. Synchronized implementation.
		/// </summary>
		protected internal bool ServiceIsUnloadedPV
		{
			get { return m_ServiceIsUnloaded.NullableObject; }
			set { m_ServiceIsUnloaded.NullableObject = value; }
		}
		/// <summary>
		/// Backing for the "ServicesRequiredForInitialize" property. Base class just
		/// returns <see langword="null"/>.
		/// </summary>
		protected virtual IEnumerable<IPAFServiceDescription> ServicesRequiredForInitializationPV
		{
			get { return null; }
		}
		/// <summary>
		/// Backing for the "ServicesRequiredForLoad" property. Base version
		/// returns <see langword="null"/>.
		/// </summary>
		protected virtual IEnumerable<IPAFServiceDescription> ServicesRequiredForLoadPV
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
		protected virtual void InitializeServicePV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnInitialize(EventArgs.Empty);
			ServiceIsInitializedPV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Load"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected virtual void LoadServicePV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnLoad(EventArgs.Empty);
			ServiceIsLoadedPV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Uninitialize"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected virtual void UninitializeServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnUninitialize(EventArgs.Empty);
			ServiceIsUninitializedPV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Unload"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected virtual void UnloadServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnUnload(EventArgs.Empty);
			ServiceIsUnloadedPV = true;
		}
		/// <summary>
		/// Just fires the <see cref="Update"/> event.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// The standard pipeline object.
		/// </param>
		protected virtual void UpdateServicePIV(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{
			OnUpdate(EventArgs.Empty);
		}
		#region Disposal-Related
		/// <summary>
		/// <see cref="IPAFDisposable"/>. This is a method that is supplied as a delegate
		/// to the disposer to call during disposal. We add our disposable stuff in this
		/// override.
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
		protected override Exception PAFFrameworkServiceDispose(bool disposing, object obj)
		{
			var eList = new List<Exception>();
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_LifetimeManagedObjectInternal, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_SecurityObjectInternal, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsInitialized, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsLoaded, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsUninitialized, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceIsUnloaded, true));

			// Gather exceptions from base.
			var baseExceptions = base.PAFFrameworkServiceDispose(disposing, obj);
			eList.AddNoNulls(baseExceptions);

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count <= 0) return null;

			var exceptions = new PAFAggregateExceptionData(eList);
			var ex = new PAFStandardException<PAFAggregateExceptionData>(exceptions);
			// Seal the list.
			exceptions.AddException(null);
			// We just put these in the registry.
			DisposalRegistry.RecordDisposalException(GetType(), ex);
			return ex;
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
			get { return ServiceManagerPV; }
			[SecurityCritical]
			set { ServiceManagerPV = value; }
		}

		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsInitialized
		{
			get { return ServiceIsInitializedPV; }
			[SecurityCritical]
			set { ServiceIsInitializedPV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsLoaded
		{
			get { return ServiceIsLoadedPV; }
			[SecurityCritical]
			set { ServiceIsLoadedPV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsUninitialized
		{
			get { return ServiceIsUninitializedPV; }
			[SecurityCritical]
			set { ServiceIsUninitializedPV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		bool IPAFServiceExtended.ServiceIsUnloaded
		{
			get { return ServiceIsUnloadedPV; }
			[SecurityCritical]
			set { ServiceIsUnloadedPV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceExtended.ServicesRequiredForInitialization
		{
			[SecurityCritical]
			get { return ServicesRequiredForInitializationPV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceExtended.ServicesRequiredForLoad
		{
			[SecurityCritical]
			get { return ServicesRequiredForLoadPV; }
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
		{ InitializeServicePV(servicePipelineObject); }
		/// <summary>
		/// See <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceExtended"/>.
		/// </param>
		[SecurityCritical]
		void IPAFServiceExtended.LoadService(IPAFServicePipelineObject<IPAFService> servicePipelineObject)
		{ LoadServicePV(servicePipelineObject); }
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
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		IPAFSecretKey IPAFServiceExtendedInternal.SecurityObjectInternal
		{
			get { return SecurityObjectPIV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		IPAFLifetimeManagedObjectInternal<IPAFSecretKeyProvider> IPAFServiceExtendedInternal.LifetimeManagedObjectInternal
		{
			get { return LifetimeManagedObjectIV; }
			set { LifetimeManagedObjectIV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceExtendedInternal.ServicesRequiredForInitializationInternal
		{
			get { return ServicesRequiredForInitializationPV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		IPAFServiceManager IPAFServiceExtendedInternal.ServiceManagerInternal
		{
			get { return ServiceManagerPV; }
			set { ServiceManagerPV = value; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> IPAFServiceExtendedInternal.ServicesRequiredForLoadInternal
		{
			get { return ServicesRequiredForLoadPV; }
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="serviceIsInitialized">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.SetServiceIsInitialized(bool serviceIsInitialized)
		{
			ServiceIsInitializedPV = serviceIsInitialized;
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="serviceIsLoaded">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.SetServiceIsLoaded(bool serviceIsLoaded)
		{
			ServiceIsLoadedPV = ServiceIsLoadedPV;
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="serviceIsUninitialized">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.SetServiceIsUninitialized(bool serviceIsUninitialized)
		{
			ServiceIsUninitializedPV = serviceIsUninitialized;
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="serviceIsUnloaded">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.SetServiceIsUnloaded(bool serviceIsUnloaded)
		{
			ServiceIsUnloadedPV = serviceIsUnloaded;
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.InitializeServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			InitializeServicePV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.LoadServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			LoadServicePV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.UninitializeServiceInternal
			(IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			UninitializeServicePIV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.UnloadServiceInternal(
			IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			UnloadServicePIV(pipelineObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See <see cref="IPAFServiceExtendedInternal"/>.
		/// </param>
		void IPAFServiceExtendedInternal.UpdateServiceInternal(
			IPAFServicePipelineObject<IPAFService> pipelineObject)
		{
			UpdateServicePIV(pipelineObject);
		}
		#endregion
	}
}
