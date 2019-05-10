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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.TypeHandling.Disposal;

// Exception shorthand.
// ReSharper disable once IdentifierTypo
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// This class provides simple control functionality for multi-threaded operations.
	/// <see cref="IAsyncControlObject"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 29mar2019 </date>
	/// <description>
	/// Put this into .Net standard mostly for stochastic testing
	/// for new concurrent functionality.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 06jul2012 </date>
	/// <description>
	/// Redid this to use a disposal surrogate instead of implementing all the
	/// disposal plumbing directly.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04jun2012 </date>
	/// <description>
	/// Refactored this out of "ThreadControlObject" so PAFCore could have
	/// a simple thread controller.
	/// </description>
	/// </contribution>
	/// </history>
	public class AsyncControlObject : IAsyncControlObjectInternal,
		IAsyncControlObjectProvider
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Atomic field is backing for the prop.
		/// </summary>
		private bool m_IsAborting;
		/// <summary>
		/// Atomic field is backing for the prop.
		/// </summary>
		private int m_TaskOrThreadID;
		/// <summary>
		/// Weakable pseudo delegate store.
		/// </summary>
		public IPropertyChangedEventArgsSubscriberStore m_PceStore;
		/// <summary>
		/// Synchronization wrapper for <see cref="ProcessException"/> property.
		/// It is NOT atomic if we access the dictionary. Use
		/// <see cref="NullableSynchronizedWrapper{T}.GetWriteLockedObject()"/>
		/// if manipulating the exception.
		/// </summary>
		protected NullableSynchronizedWrapper<Exception> m_NullableThreadExceptionWrapper
			= new NullableSynchronizedWrapper<Exception>(null);
		//////////////////////////////////////////////////////////////////////////////////
		// Following fields are protected for a special reason. Do not set
		// outside of control properties/methods.
		//////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Atomic field is backing for the prop.
		/// </summary>
		protected bool m_ProcessShouldStart;
		/// <summary>
		/// Atomic field is backing for the prop.
		/// </summary>
		protected bool m_ProcessHasStarted;
		/// <summary>
		/// Atomic field is backing for the prop.
		/// </summary>
		protected bool m_ProcessShouldTerminate;
		/// <summary>
		/// Atomic field is backing for the prop.
		/// </summary>
		protected bool m_ProcessHasTerminated;
		/// <summary>
		/// Holds our surrogate disposer.
		/// </summary>
		protected PAFDisposer<Guid> m_PAFDisposer;
		/// <summary>
		/// Need to hold our secret key because of hierarchical composition.
		/// </summary>
		protected Guid SecretKey { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Main constructor that supplies a <see cref="Guid"/> key for the
		/// <see cref="DisposalRegistry"/>. Supply a <see cref="Guid.Empty"/> for
		/// unsecured disposal.
		/// </summary>
		/// <param name="guid">
		/// The key that the class instantiator supplies so that the instantiator
		/// is the only one that can dispose the instance. The default
		/// for no argument supplied is "default(Guid)" which is the very
		/// same as <see cref="Guid.Empty"/>.
		/// </param>
		public AsyncControlObject(Guid guid = default(Guid))
		{
			// We build the disposer with the Guid and our instance and the disposal delegate.
			m_PAFDisposer = new PAFDisposer<Guid>(guid, this, AsyncControlObjectDispose);
			DisposalRegistry.RegisterForDisposal(m_PAFDisposer);

			// Now build and start our store.
			m_PceStore = new PropertyChangedEventArgsSubscriberStore(this);
			m_PceStore.Start();
		}
		#endregion // Constructors
		#region Properties
		#region IAsynControlObjectProvider Implementation
		/// <summary>
		/// See <see cref="IAsyncControlObjectProvider"/>.
		/// This implementation allows us to provide OURSELVES in non-aggregation
		/// scenarios.
		/// </summary>
		public virtual IAsyncControlObject ProvidedControlObject
		{ get { return this; } }
		#endregion // IAsynControlObjectProvider Implementation
		/// <summary>
		/// See <see cref="IAsyncControllerObject"/>. "set" is one-way. We
		/// can never undo an abort. If abort is set to <see langwora="true"/>,
		/// <see cref="ProcessShouldTerminate"/> will also be set.
		/// </summary>
		public virtual bool IsAborting
		{
			get { return m_IsAborting; }
			set
			{
				if (value)
				{
					ProcessShouldTerminate = true;
					m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_IsAborting, true);
				}
			}
		}
		public virtual int TaskOrThreadId
		{
			get { return m_TaskOrThreadID; }
			set { m_TaskOrThreadID = value; }
		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// </summary>
		public virtual Exception ProcessException
		{
			get { return m_NullableThreadExceptionWrapper.NullableObject; }
			set { m_NullableThreadExceptionWrapper.NullableObject = value; }
		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>. "set" is one-way. We
		/// can never un-start a process. 
		/// </summary>
		public virtual bool ProcessHasStarted
		{
			get { return m_ProcessHasStarted; }
			set
			{
				if (value)
				{
					m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_ProcessHasStarted, true);
					// If we have started, we are through the gate.
					ProcessShouldStart = false;
				}
			}

		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// </summary>
		public virtual bool ProcessShouldStart
		{
			get { return m_ProcessShouldStart; }
			set
			{
				m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_ProcessShouldStart, value);
			}

		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>. "set" is one-way. We
		/// can never un-terminate a process. 
		/// </summary>
		public virtual bool ProcessHasTerminated
		{
			get { return m_ProcessHasTerminated; }
			set
			{
				if (value)
				{
					m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_ProcessHasTerminated, true);
					// If we have terminated, we are through the gate.
					ProcessShouldTerminate = false;

				}
			}
		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// </summary>
		/// <remarks>
		/// Set to <see langword="true"/> clears
		/// <see cref="ProcessShouldStart"/>.
		/// </remarks>
		public virtual bool ProcessShouldTerminate
		{
			get { return m_ProcessShouldTerminate; }
			set
			{
				if (value)
				{
					ProcessShouldStart = false;
					m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_ProcessShouldTerminate, value);
				}
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// <para>
		/// This method is not virtual. The developer of any subclass must not be
		/// allowed to change the logic.
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
		/// This class should be disposed only after <see cref="ProcessHasTerminated"/> has been
		/// set. If this cannot be guaranteed (aborts fail or running under Silverlight and
		/// aborts aren't supported), this method should probably be called anyway, even though
		/// exceptions may be generated due to thread collisions. The locks held by this class
		/// can use significant resources DEPENDING ON THE IMPLEMENTATION.
		/// </para>
		/// <para>
		/// Exceptions are caught and recorded in the registry.
		/// </para>
		/// </remarks>
		protected virtual Exception AsyncControlObjectDispose(bool disposing, object obj)
		{
			var eList = new List<Exception>();
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_NullableThreadExceptionWrapper, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_PceStore, true));

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0)
			{
				var exceptions = new PAFAED(eList);
				var ex = new PAFStandardException<PAFAED>(exceptions);
				// Seal the list.
				exceptions.AddException(null);
				// We just put these in the registry.
				DisposalRegistry.RecordDisposalException(GetType(), ex);
				return ex;
			}
			return null;
		}
		#endregion // Methods
		#region IAsyncControlObjectInternal Implementation
		/// <summary>
		/// See <see cref="IAsyncControllerObjectInternal"/>.
		/// </summary>
		void IAsyncControlObjectInternal.SignalAbort()
		{
			IsAborting = true;
		}
		/// <summary>
		/// See <see cref="IAsyncControllerObjectInternal"/>.
		/// </summary>
		Guid IAsyncControlObjectInternal.GetSecretDisposalKey()
		{
			return SecretKey;
		}
		#endregion // IAsyncControlObjectInternal Implementation
		#region IUnprotectedDisposableProvider Implementation
		/// <summary>
		/// See <see cref="IUnprotectedDisposableProvider"/>.
		/// </summary>
		/// <param name="secretKey">
		/// See <see cref="IUnprotectedDisposableProvider"/>.
		/// </param>
		/// <remarks>
		/// This implementation just calls into the disposer, which is already
		/// protected with the secret key, so we don't have to do much.
		/// </remarks>
		public virtual IDisposable GetUnprotectedDisposable(object secretKey)
		{
			return m_PAFDisposer.GetUnprotectedDisposable(secretKey);
		}
		#endregion // IUnprotectedDisposableProvider Implementation
		#region Implementation of INotifyPropertyChanged
		/// <summary>
		/// Implemented with our weakable store.
		/// </summary>
		public virtual event PropertyChangedEventHandler PropertyChanged
		{
			add { m_PceStore.WeaklySubscribe(value);}
			remove {  m_PceStore.Unsubscribe(value);}
		}
		#endregion // Implementation of INotifyPropertyChanged
	}
}
