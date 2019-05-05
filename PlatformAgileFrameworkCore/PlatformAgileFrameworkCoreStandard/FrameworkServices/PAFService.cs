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

using System;
using System.Collections.Generic;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.TypeHandling.Disposal;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This class is a base implementation of the <see cref="IPAFServiceInternal"/>
	/// interface for use through inheritance.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 02jan2019 </date>
	/// <description>
	/// "IsDefault" now being internal rippled down to this class.
	/// Refactored to move extended stuff into sub-class and add "IsDefault"
	/// stuff.
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
	// core part.
	public abstract partial class PAFService
		: IPAFServiceInternal
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// General interface exposed to inheritors. Mainly for proxy
		/// support.
		/// </summary>
		internal IPAFNamedAndTypedObjectInternal m_IPAFNamedAndTypedObjectInternal;
		/// <summary>
		/// <see cref="IPAFService"/>. This is a shared variable
		/// and thus must be synchronized.
		/// </summary>
		protected internal NullableSynchronizedWrapper<bool> m_IsDefaultObject
			= new NullableSynchronizedWrapper<bool>();
		protected NullableSynchronizedWrapper<IPAFServiceManager> m_ServiceManager
			= new NullableSynchronizedWrapper<IPAFServiceManager>();
		#region Disposal-Related
		/// <summary>
		/// Holds our surrogate disposer.
		/// </summary>
		protected PAFDisposer<Guid> m_PAFDisposer;
		protected internal Guid m_SecretKey;
		/// <summary>
		/// Needed to push in our secret key for safe disposal.
		/// </summary>
		public Guid SecretKey
		{
			set => m_SecretKey = value;
		}
		#endregion // Disposal-Related
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
		protected internal PAFService(Type serviceType = null, string serviceName = null,
			Guid guid = default(Guid))
		{
			if (serviceName == null)
			{
				serviceName = "";
			}
			if(serviceType == null)
				serviceType = GetType();

			m_IPAFNamedAndTypedObjectInternal = new PAFNamedAndTypedObject(serviceType, serviceName, this);

			// We build the disposer with the Guid and our instance and the disposal delegate.
			m_PAFDisposer = new PAFDisposer<Guid>(guid, this, PAFFrameworkServiceDispose);
			DisposalRegistry.RegisterForDisposal(m_PAFDisposer);
		}
		#endregion
		#region Novel Members
		#region Novel Properties
		/// <summary>
		/// Backing for the "ServiceManager" property. Synchronized implementation.
		/// </summary>
		protected internal virtual IPAFServiceManager ServiceManagerPV
		{
			get { return m_ServiceManager.NullableObject; }
			set { m_ServiceManager.NullableObject = value; }
		}
		#endregion // Novel Properties
		#region Novel Methods
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
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ServiceManager, true));

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0) {
				var exceptions = new PAFAggregateExceptionData(eList);
				var ex = new PAFStandardException<PAFAggregateExceptionData>(exceptions);
				// Seal the list.
				exceptions.AddException(null);
				// We just put these in the registry.
				DisposalRegistry.RecordDisposalException(GetType(), ex);
				return ex;
			}
			return null;
		}
		#endregion // Disposal-Related
		#endregion // Novel Methods
		#endregion // Novel Members
		#region Implementation of IPAFFrameworkServiceInternal
		#region Methods
		/// <summary>
		/// See <see cref="IPAFServiceInternal"/>.
		/// </summary>
		bool IPAFServiceInternal.SetServiceAsDefault(bool isDefault)
		{
			var retval = IsDefaultObject != isDefault;
			m_IPAFNamedAndTypedObjectInternal.SetIsDefault(isDefault);
			return retval;
		}
		IPAFServiceManagerInternal IPAFServiceInternal.ServiceManager
		{ get; set;}
		#endregion // Methods
		#endregion // Implementation of IPAFFrameworkServiceInternal

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
			get { return m_IPAFNamedAndTypedObjectInternal.ObjectName; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObjectInternal.ObjectName = value; }
		}
		#endregion
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>.
		/// </summary>
		public string AssemblyQualifiedObjectType
		{
			get { return m_IPAFNamedAndTypedObjectInternal.AssemblyQualifiedObjectType; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObjectInternal.AssemblyQualifiedObjectType = value; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>.
		/// </summary>
		public bool IsDefaultObject
		{
			get { return m_IPAFNamedAndTypedObjectInternal.IsDefaultObject; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>. This should normally be
		/// an interface type.
		/// </summary>
		public Type ObjectType
		{
			get { return m_IPAFNamedAndTypedObjectInternal.ObjectType; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObjectInternal.ObjectType = value; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>. Returns the actual
		/// service object.
		/// </summary>
		public object ObjectValue
		{
			get { return m_IPAFNamedAndTypedObjectInternal.ObjectValue; }
			[SecurityCritical]
			set { m_IPAFNamedAndTypedObjectInternal.ObjectValue = value; }
		}
		#endregion
	}
}
