#region Exception shorthand.
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;
#endregion // Exception shorthand.

using System;
using System.Collections.ObjectModel;
using System.Security;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.MultiProcessing.Threading.Locks;
using PlatformAgileFramework.TypeHandling.Disposal;


namespace PlatformAgileFramework.MultiProcessing.Threading.NullableObjects
{
	/// <summary>
	/// This class is designed to wrap an object that possibly has a null value. Object
	/// can be a value type or a reference type.
	/// </summary>
	/// <remarks>
	/// Wears <see cref="IPAFDisposable"/> so we can track leaks.
	/// </remarks>
	/// <threadsafety>
	/// <para>
	/// This class uses an <see cref="IReaderWriterLock"/> for synchronization. A vacuous
	/// lock (one that doesn't lock anything) can be installed so that legacy code can be
	/// converted to use this class instance inside a monitor lock instead of our internal
	/// lock.
	/// </para>
	/// <para>
	/// "PAFDisposeCaller" method is NOT thread-safe. See the DOCs.
	/// </para>
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 22apr2015 </date>
	/// <description>
	/// <para>
	/// Re-installed the one-way valve (HasBeenSet) from NullableWrapper - not sure why it
	/// was taken out. It's needed for the set once only flag.
	/// </para>
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04apr2012 </date>
	/// <description>
	/// <para>
	/// Made partial class and made a partial method for getting the standard lock for the
	/// environment, if one is defined in another class part.
	/// </para>
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 04sep2011 </date>
	/// <description>
	/// <para>
	/// Added history.
	/// </para>
	/// <para>
	/// Added the ability to put in a custom lock. Added the PAF disposal pattern. This
	/// was an attempt to reduce code bloat and streamline our synchronization facilities.
	/// Before this, we had a wrapper for nullable objects and a synchronization wrapper.
	/// Let's hope it works......
	/// </para>
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	// Core.
	public partial class NullableSynchronizedWrapperBase<T>:
		IPAFNullableSynchronizedWrapper<T>
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Internal Write Locked Object Class
		/// <summary>
		/// This is a simple implementation of
		/// <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>.
		/// </summary>
		private class DisposableWriteLockedObject
			: IPAFDisposableWriteLockedObjectAccessor<T>
		{
			#region Class Fields And Autoproperties
			/// <summary>
			/// Alllows multiple disposal calls.
			/// </summary>
			private volatile bool m_IsDisposed;
			/// <summary>
			/// Needed to hold our parent for manipulation.
			/// </summary>
			private readonly NullableSynchronizedWrapperBase<T> m_Wrapper;
			#endregion //Class Fields And Autoproperties
			#region Constructors
			/// <summary>
			/// Constructor simply takes an exclusive lock on the wrapper, synchronously.
			/// </summary>
			/// <param name="wrapper">
			/// The wrapper that we lock for the client.
			/// </param>
			public DisposableWriteLockedObject(NullableSynchronizedWrapperBase<T> wrapper)
			{
				m_Wrapper = wrapper;
				m_Wrapper.m_IReaderWriterLock.EnterWriteLock();
				m_IsDisposed = false;
			}

			/// <summary>
			/// Constructor takes an exclusive lock on the wrapper with
			/// a timeout and a failure indicator.
			/// </summary>
			/// <param name="wrapper">
			/// The wrapper that we lock for the client.
			/// </param>
			/// <param name="timeoutInMilliseconds">
			/// The time to wait before returning <see langword="false"/>.
			/// </param>
			/// <param name="isLocked">
			/// <see langword="false"/> if the time threshold has been exceeded.
			/// </param>
			public DisposableWriteLockedObject
				(NullableSynchronizedWrapperBase<T> wrapper, int timeoutInMilliseconds, out bool isLocked)
			{
				isLocked = wrapper.m_IReaderWriterLock.TryEnterWriteLock(TimeSpan.FromMilliseconds(timeoutInMilliseconds));
				if (!isLocked)
				{
					m_IsDisposed = true;
					return;
				}
				m_Wrapper = wrapper;
				m_IsDisposed = false;
			}
			#endregion // Constructors
			#region Implementation of IDisposable
			/// <summary>
			/// This method unlocks the containing wrapper. This MUST be called
			/// or the container will remain forever locked. This method may be
			/// called multiple times. It will not attempt to release the lock
			/// multiple times.
			/// </summary>
			/// <remarks>
			/// This class is designed to be disposable so that the lock it provides
			/// may be employed in a "using" statement. This can provide a better
			/// guarantee that the lock is unlocked resolutely, preventing some
			/// awful errors.
			/// </remarks>
			public void Dispose()
			{
				if (m_IsDisposed) return;
				m_IsDisposed = true;
				m_Wrapper.m_IReaderWriterLock.ExitWriteLock();
			}
			#endregion
			#region Implementation of IPAFDisposableWriteLockedObjectAccessor
			/// <summary>
			/// See <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>
			/// </summary>
			public T WriteLockedNullableObject
			{
				get { return (T)m_Wrapper.m_Object; }
				set
				{
					m_Wrapper.m_Object = value;
					m_Wrapper.m_HasBeenSetIntAsBool = 1;
				}
			}

			/// <summary>
			/// See <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>
			/// </summary>
			public bool HasBeenSet
			{
				get { return m_Wrapper.HasBeenSet; }
			}
			/// <summary>
			/// See <see cref="IPAFDisposableWriteLockedObjectAccessor{T}"/>
			/// </summary>
			public bool ObjectIsNull()
			{
				return !m_Wrapper.HasValue();
			}
			#endregion //Implementation of IPAFDisposableWriteLockedObjectAccessor
		}
		#endregion // Internal Write Locked Object Class
		#region Internal Read Locked Object Class
		/// <summary>
		/// This is a simple implementation of
		/// <see cref="IPAFDisposableReadLockedObjectAccessor{T}"/>.
		/// </summary>
		/// <remarks>
		/// This class is designed to be disposable so that the lock it provides
		/// may be employed in a "using" statement. This can provide a better
		/// guarantee that the lock is unlocked resolutely, preventing some
		/// awful errors.
		/// </remarks>
		private class DisposableReadLockedObject
			: IPAFDisposableReadLockedObjectAccessor<T>
		{
			#region Class Fields And Autoproperties
			/// <summary>
			/// Alllows multiple disposal calls.
			/// </summary>
			private volatile bool m_IsDisposed;
			/// <summary>
			/// Needed to hold our parent for reading.
			/// </summary>
			private readonly NullableSynchronizedWrapperBase<T> m_Wrapper;
			#endregion // Class Fields And Autoproperties
			#region Constructors
			/// <summary>
			/// Constructor simply takes a read lock on the wrapper.
			/// </summary>
			/// <param name="wrapper">
			/// The wrapper that we lock for the client.
			/// </param>
			public DisposableReadLockedObject(NullableSynchronizedWrapperBase<T> wrapper)
			{
				m_Wrapper = wrapper;
				m_Wrapper.m_IReaderWriterLock.EnterReadLock();
				m_IsDisposed = false;
			}
			/// <summary>
			/// Constructor takes a read lock lock on the wrapper with
			/// a timeout and a failure indicator.
			/// </summary>
			/// <param name="wrapper">
			/// The wrapper that we lock for the client.
			/// </param>
			/// <param name="timeoutInMilliseconds">
			/// The time to wait before returning <see langword="false"/>.
			/// </param>
			/// <param name="isLocked">
			/// <see langword="false"/> if the time threshold has been exceeded.
			/// </param>
			public DisposableReadLockedObject
				(NullableSynchronizedWrapperBase<T> wrapper, int timeoutInMilliseconds, out bool isLocked)
			{
				isLocked = wrapper.m_IReaderWriterLock.TryEnterReadLock(TimeSpan.FromMilliseconds(timeoutInMilliseconds));
				if (!isLocked)
				{
					m_IsDisposed = true;
					return;
				}
				m_Wrapper = wrapper;
				m_IsDisposed = false;
			}
			#endregion // Constructors
			#region Implementation of IDisposable
			/// <summary>
			/// This method unlocks the containing wrapper. This MUST be called
			/// or the container will remain forever locked. This method may be
			/// called multiple times. It will not attempt to release the lock
			/// multiple times.
			/// </summary>
			public void Dispose()
			{
				if (m_IsDisposed) return;
				m_IsDisposed = true;
				m_Wrapper.m_IReaderWriterLock.ExitReadLock();
			}
			#endregion
			#region Implementation of IPAFDisposableReadLockedObjectAccessor
			/// <summary>
			/// See <see cref="IPAFDisposableReadLockedObjectAccessor{T}"/>
			/// </summary>
			public bool HasBeenSet
			{
				get { return m_Wrapper.HasBeenSet; }
			}
			/// <summary>
			/// See <see cref="IPAFDisposableReadLockedObjectAccessor{T}"/>
			/// </summary>
			public T ReadLockedNullableObject
			{
				get { return (T)m_Wrapper.m_Object; }
			}
			/// <summary>
			/// See <see cref="IPAFDisposableReadLockedObjectAccessor{T}"/>
			/// </summary>
			public bool ObjectIsNull()
			{
				return !m_Wrapper.HasValue();
			}
			#endregion //Implementation of IPAFDisposableReadLockedObjectAccessor
		}
		#endregion // Internal Read Locked Object Class
		#region Class Fields and Autoproperties
		///<remarks/>
		private int m_HasBeenSetIntAsBool;
		///<remarks/>
		private IReaderWriterLock m_IReaderWriterLock;
		///<remarks/>
		private volatile object m_Object;
		/// <summary>
		/// Holds our surrogate disposer.
		/// </summary>
		protected PAFDisposer<Guid> m_PAFDisposer;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Allows setting of a custom lock. Leaves the internal object without
		/// content.
		/// </summary>
		/// <param name="iReaderWriterLock">
		/// Lock to be used for read/write access. If <see langword="null"/>, the default will be
		/// used. If no locking is desired, a non-functional lock can be installed.
		/// Default = <see cref="MonitorReaderWriterLock"/>.
		/// </param>
		/// <param name="secretKey">
		/// Set to <see cref="Guid.Empty"/> to disable security.
		/// Default = "default(Guid)", which is <see cref="Guid.Empty"/>.
		/// </param>
		public NullableSynchronizedWrapperBase(IReaderWriterLock iReaderWriterLock = null,
			Guid secretKey = default(Guid))
		{
			m_IReaderWriterLock = iReaderWriterLock ?? new MonitorReaderWriterLock();
			// We build the disposer with the Guid and our instance and the disposal delegate.
			m_PAFDisposer = new PAFDisposer<Guid>(secretKey, this, NullableSynchronizedWrapperBaseDispose);
			DisposalRegistry.RegisterForDisposal(m_PAFDisposer);
		}
		/// <summary>
		/// Sets the internal object. Allows setting of a custom lock.
		/// </summary>
		/// <param name="t">
		/// The wrapped item (may be <see langword="null"/>) for reference types.
		/// Default = "default(T)".
		/// </param>
		/// <param name="iReaderWriterLock">
		/// Lock to be used for read/write access. If <see langword="null"/>, the default will be
		/// used. If no locking is desired, a non-functional lock can be installed.
		/// Default = <see cref="MonitorReaderWriterLock"/>.
		/// </param>
		/// <param name="secretKey">
		/// Set to <see cref="Guid.Empty"/> to disable security.
		/// Default = "default(Guid)", which is <see cref="Guid.Empty"/>.
		/// </param>
		public NullableSynchronizedWrapperBase(T t = default(T),
			IReaderWriterLock iReaderWriterLock = null, Guid secretKey = default(Guid))
			:this(iReaderWriterLock, secretKey)
		{
			m_Object = t;
			m_HasBeenSetIntAsBool = 1;
		}
		#endregion // Constructors
		#region Properties

		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		public bool HasBeenSet
		{
			get { return m_HasBeenSetIntAsBool != 0;}
		}
		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		public virtual T NullableObject
		{
			get
			{
				m_IReaderWriterLock.EnterReadLock();
				T returnValue;
				if (m_Object == null) returnValue =  default(T);
				else returnValue = (T)m_Object;
				m_IReaderWriterLock.ExitReadLock();
				return returnValue;
			}
			set
			{
				m_IReaderWriterLock.EnterWriteLock();
				m_Object = value;
				m_HasBeenSetIntAsBool = 1;
				m_IReaderWriterLock.ExitWriteLock();
			}
		}
		#endregion // Properties
		#region Methods
		#region PartialMethods
// ReSharper disable PartialMethodWithSinglePart
		static partial void PartialGetLock(Type type, ref IReaderWriterLock readerWriterLock);
// ReSharper restore PartialMethodWithSinglePart
		#endregion // PartialMethods

		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </returns>
		public virtual IPAFDisposableWriteLockedObjectAccessor<T> GetWriteLockedObject()
		{
			return new DisposableWriteLockedObject(this);
		}

		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		/// <param name="millisecondsToWait">
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </returns>
		public virtual IPAFDisposableWriteLockedObjectAccessor<T> GetWriteLockedObject(int millisecondsToWait)
		{
			var wrl = new DisposableWriteLockedObject(this, millisecondsToWait, out var lockTaken);
			if (lockTaken)
				return wrl;
			return null;
		}
		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </returns>
		public virtual IPAFDisposableReadLockedObjectAccessor<T> GetReadLockedObject()
		{
			return new DisposableReadLockedObject(this);
		}
		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		/// <param name="millisecondsToWait">
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </returns>
		public virtual IPAFDisposableReadLockedObjectAccessor<T> GetReadLockedObject(int millisecondsToWait)
		{
			var rdl = new DisposableReadLockedObject(this, millisecondsToWait, out var lockTaken);
			if (lockTaken)
				return rdl;
			return null;
		}
		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </returns>
		/// <remarks>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </remarks>
		public virtual bool HasValue()
		{
			m_IReaderWriterLock.EnterReadLock();
			var returnValue = m_Object != null;
			m_IReaderWriterLock.ExitReadLock();
			return returnValue;
		}
		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		public virtual void NullTheObject()
		{
			m_IReaderWriterLock.EnterWriteLock();
			m_Object = null;
			m_IReaderWriterLock.ExitWriteLock();
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
		/// </remarks>
		protected virtual Exception NullableSynchronizedWrapperBaseDispose(bool disposing, object obj)
		{
			var eList = new Collection<Exception>();

			// First dispose fields that are IDisposable for sure.
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IReaderWriterLock, true));

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0) {
				var exceptions = new PAFAED(eList);
				var ex = new PAFStandardException<PAFAED>(exceptions);
                eList.Add(ex);
				// Seal the list.
				exceptions.AddException(null);
				// We just put these in the registry. If a framework is in use, it
				// should dig these out and report them.
				DisposalRegistry.RecordDisposalException(GetType(), ex);
				return ex;
			}
			return null;
		}
		/// <summary>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </summary>
		/// <param name="success">
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFNullableSynchronizedWrapper{T}"/>.
		/// </returns>
		public T TryGetItem(out bool success)
		{
			T retval;
			m_IReaderWriterLock.EnterReadLock();
			if (m_Object == null)
			{
				retval = default(T);
				success = false;
			}
			else
			{
				retval = (T) m_Object;
				success = true;
			}
			m_IReaderWriterLock.ExitReadLock();
			return retval;
		}
		#endregion // Methods
		#region IDisposable Imnplementation
		/// <summary>
		/// See <see cref="IDisposable"/>.
		/// </summary>
		/// <remarks>
		/// Method is <see cref="SecurityCriticalAttribute"/>. This is the
		/// security "gateway" for elevated-priviledge callers.
		/// </remarks>
		[SecurityCritical]
		public void Dispose()
		{
			m_PAFDisposer.Dispose();
		}
		#endregion // IDisposable Imnplementation
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
		#region Static Helpers
		/// <summary>
		/// Gets the dummy lock that is appropriate for the environment
		/// and the Type.
		/// </summary>
		/// <param name="type">
		/// Type to be locked. Not used in core.
		/// </param>
		/// <returns>
		/// The lock. Must never be <see langword="null"/>.
		/// </returns>
		public static IReaderWriterLock GetNecessaryLock(Type type)
		{
// ReSharper doesn't know about partial method useage pattern yet....
// ReSharper disable RedundantAssignment
			IReaderWriterLock rwLock = null;
// ReSharper restore RedundantAssignment
// ReSharper disable InvocationIsSkipped
			PartialGetLock(type, ref rwLock);
// ReSharper restore InvocationIsSkipped
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
			if(rwLock != null) return rwLock;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse
			return new MonitorReaderWriterLock();
		}
		#endregion //  Static Helpers
	}
}
