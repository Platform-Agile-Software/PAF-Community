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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;

// Exceptions
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.TypeHandling.Disposal;

// Exception shorthand.
// ReSharper disable IdentifierTypo
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;
using PTMED = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionData;
// ReSharper restore IdentifierTypo

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// Default implementation of <see cref=" IAsyncControllerObject"/> for <see cref="Task"/>'s.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31mar2019 </date>
	/// <description>
	/// Built this as a convenience for new stochastic tests.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe.
	/// </threadsafety>
	/// <serialization>
	/// Class is not anticipated to be serialized.
	/// </serialization>
	public class AsyncTaskControllerObject : AsyncControlObject,
		IAsyncWorkControllerObject
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Default if constructor argument is missing.
		/// </summary>
		public const long DEFAULT_MAX_ITERATIONS = -1;
		/// <summary>
		/// Default if constructed argument is missing.
		/// </summary>
		public const int DEFAULT_PROCESS_RUN_TIME_IN_MILLISECONDS = int.MaxValue;
		/// <summary>
		/// Default if constructed argument is missing.
		/// </summary>
		public const int DEFAULT_ABORT_TIME_IN_MILLISECONDS = 100;
		/// <summary>
		/// Default if constructed argument is missing.
		/// </summary>
		public const int DEFAULT_CALLBACK_DELAY_IN_MILLISECONDS = 25;
		/// <summary>
		/// Atomic field is backing for the prop.
		/// </summary>
		protected readonly TimeSpan m_AbortTime;
		/// <summary>
		/// This is a non-atomic variable and thus must be synchronized.
		/// </summary>
		private NullableSynchronizedWrapper<TimeSpan> m_AbortTimeRemaining
			= new NullableSynchronizedWrapper<TimeSpan>();
		/// <summary>
		/// <see cref="IAsyncControllerObject"/>.
		/// </summary>
		/// <threadsafety>
		/// NOT synchronized. expected to be readonly.
		/// </threadsafety>
		public TimeSpan ProcessCheckInterval
		{ get; protected set; }
		/// <summary>
		/// Property for the delegate plugin that will be called back periodically. This
		/// may be <see langword="null"/>, indicating that an internal default delegate will be used.
		/// This delegate has access to only the <see cref="IAsyncControllerObject"/> by
		/// default and can examine controlled threads and manipulate them.
		/// </summary>
		public Action<IAsyncControllerObject> ControllerDelegate { get; protected set; }
		/// <summary>
		/// Delegate used to call <see cref="ControllerDelegate"/>. Can be <see langword="null"/>, in
		/// which case an internal default will be used.
		/// </summary>
		public Action<object> ControllerDelegateCallerDelegate { get; protected set; }
		/// <summary>
		/// <see cref="IAsyncControllerObject"/>.
		/// </summary>
		protected ICollection<IAsyncControlObject> m_ControlObjects
			 = new Collection<IAsyncControlObject>();
		/// <summary>
		/// This is a non-atomic variable and thus must be synchronized.
		/// </summary>
		private NullableSynchronizedWrapper<long> m_IterationsRemaining
			= new NullableSynchronizedWrapper<long>();
		/// <summary>
		/// For iteration-based stopping criterion.
		/// </summary>
		protected readonly long m_MaxIterations;
		/// <summary>
		/// This is a non-atomic variable and thus must be synchronized.
		/// </summary>
		private NullableSynchronizedWrapper<TimeSpan> m_ProcessRunTimeRemaining
			= new NullableSynchronizedWrapper<TimeSpan>();
		/// <summary>
		/// Time to be counted down for process termination.
		/// </summary>
		protected readonly TimeSpan m_ProcessRunTime;
		/// <summary>
		/// Backing.
		/// </summary>
		private Timer m_ProcessRunTimer;
		/// <summary>
		/// This is a cache of <see cref="IAsyncControlObject"/>s that have been discovered
		/// to be <see cref="IAsyncWorkControlObject"/>s.
		/// </summary>
		private IList<IAsyncWorkControlObject> m_WorkControlObjects;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor sets time intervals and optionally sets control objects.
		/// </summary>
		/// <param name="callbackDelayInMilliseconds">
		/// Sets <see cref="ProcessCheckInterval"/>.
		/// </param>
		/// <param name="processRunTimeInMilliseconds">
		/// Sets <see cref="m_ProcessRunTime"/>
		/// </param>
		/// <param name="maxIterations">
		/// Sets the iteration countdown. -1 disables iteration-based control.
		/// </param>
		/// <param name="abortTimeInMilliseconds">
		/// Sets abort time countdown.
		/// </param>
		/// <param name="secretKey">
		/// Optional "secret" disposal key. Use <see cref="Guid.Empty"/>
		/// for no secret key protection. This is the default.
		/// </param>
		/// <param name="controlObjects">
		/// Optional set of <see cref="IAsyncControlObject"/>s. May be <see langword="null"/>.
		/// </param>
		/// <param name="controllerDelegate">
		/// An optional delegate that may be supplied to perform supervision of
		/// the running tasks. If <see langword="null"/>, <see cref="AsyncControllerObjectExtensions.TaskStartControllerDelegateMethod"/>
		/// is used.
		/// </param>
		/// <param name="controllerDelegateCallerDelegate">
		/// An optional delegate that may be supplied to call <paramref name="controllerDelegate"/>.
		/// If <see langword="null"/>, <see cref="AsyncControllerObjectExtensions.TaskControllerDelegateCallerDelegateMethod"/>
		/// is used.
		/// </param>
		public AsyncTaskControllerObject(
			IEnumerable<IAsyncControlObject> controlObjects = null,
			Action<IAsyncControllerObject> controllerDelegate = null,
			Action<object> controllerDelegateCallerDelegate = null,
			int callbackDelayInMilliseconds = DEFAULT_CALLBACK_DELAY_IN_MILLISECONDS,
			int processRunTimeInMilliseconds = DEFAULT_PROCESS_RUN_TIME_IN_MILLISECONDS,
			long maxIterations = DEFAULT_MAX_ITERATIONS,
			int abortTimeInMilliseconds = DEFAULT_ABORT_TIME_IN_MILLISECONDS,
			Guid secretKey = default(Guid)
			)
			: base(secretKey)
		{
			SecretKey = secretKey;
			if (controlObjects != null)
			{
				foreach (var cObj in controlObjects)
				{
					AddControlObject(cObj);
				}
			}
			ProcessCheckInterval = TimeSpan.FromMilliseconds(callbackDelayInMilliseconds);

			m_ProcessRunTime = TimeSpan.FromMilliseconds(processRunTimeInMilliseconds);
			if (processRunTimeInMilliseconds == DEFAULT_PROCESS_RUN_TIME_IN_MILLISECONDS)
				m_ProcessRunTime = TimeSpan.MaxValue;
			ProcessRunTimeRemaining = m_ProcessRunTime;

			m_MaxIterations = maxIterations;
			IterationsRemaining = m_MaxIterations;
			m_AbortTime = TimeSpan.FromMilliseconds(abortTimeInMilliseconds);
			AbortTimeRemaining = m_AbortTime;
			ControllerDelegateCallerDelegate = controllerDelegateCallerDelegate;

			//
			ControllerDelegate = AsyncControllerObjectExtensions.TaskStartControllerDelegateMethod;
			if (controllerDelegate != null)
				ControllerDelegate = controllerDelegate;
			//
			ControllerDelegateCallerDelegate = AsyncControllerObjectExtensions.TaskControllerDelegateCallerDelegateMethod;
			if (controllerDelegateCallerDelegate != null)
				ControllerDelegateCallerDelegate = controllerDelegateCallerDelegate;
		}
		#endregion // Constructors
		#region Implementation of IAsyncWorkControllerObject
		#region Implementation of IAsyncControllerObject
		#region Properties
		/// <summary>
		/// See <see cref="IAsyncControllerObject"/>.
		/// </summary>
		public TimeSpan AbortTimeRemaining
		{
			get { return m_AbortTimeRemaining.NullableObject; }
			set { m_AbortTimeRemaining.NullableObject = value; }
		}
		/// <summary>
		/// See <see cref="IAsyncControllerObject"/>.
		/// </summary>
		/// <threadsafety>
		/// Not synchronized, since it is immutable during a run.
		/// </threadsafety>
		public IEnumerable<IAsyncControlObject> ControlObjects
		{ get { return m_ControlObjects; } }
		/// <summary>
		/// See <see cref="IAsyncControllerObject"/>.
		/// </summary>
		public long IterationsRemaining
		{
			get { return m_IterationsRemaining.NullableObject; }
			set { m_IterationsRemaining.NullableObject = value; }
		}
		/// <summary>
		/// See <see cref="IAsyncControllerObject"/>.
		/// </summary>
		public TimeSpan ProcessRunTimeRemaining
		{
			get { return m_ProcessRunTimeRemaining.NullableObject; }
			set { m_ProcessRunTimeRemaining.NullableObject = value; }
		}
		/// <summary>
		/// Internal timer that we use to call us back to check the task completion.
		/// </summary>
		public Timer ProcessRunTimer
		{
			get { return m_ProcessRunTimer; }
			set { m_ProcessRunTimer = value; }
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// See <see cref="IAsyncControllerObject"/>. This controller does
		/// not create threads or set them running. Control objects added in this base class are
		/// expected to already be complete and running.
		/// </summary>
		/// <param name="controlObject">
		/// A properly initialized, running <see cref="IAsyncControlObject"/>
		/// </param>
		public void AddControlObject([NotNull] IAsyncControlObject controlObject)
		{
			lock (m_ControlObjects)
			{
				controlObject.PropertyChanged += ReceiveChildSignal;
				m_ControlObjects.Add(controlObject);
			}
		}
		/// <summary>
		/// This is the <see cref="Action{Object}"/> method that is passed to
		/// a managed thread to be executed. This method is the sole place where the times
		/// are counted down and the timer for this controller is manipulated. This is also
		/// the method in which the iteration count is decremented if iteration-based stopping
		/// criteria is enabled.
		/// </summary>
		/// <param name="obj">
		/// An <see cref="object"/>, which must always be an <see cref="IAsyncControllerObject"/>.
		/// If <see langword="null"/>, we just use ourselves.
		/// </param>
		/// <remarks>
		/// The times are counted down BEFORE the delegate is invoked, because
		/// <see cref="ProcessCheckInterval"/> will have elapsed before the method is called
		/// the first time. Also note that the callback times are not perfectly
		/// periodic in this simple controller, since time is suspended during the
		/// processing of the <see cref="ControllerDelegate"/> and restarted when 
		/// it is complete. Also note that the <see cref="AbortTimeRemaining"/> is not counted
		/// down in the same callback as the <see cref="ProcessRunTimeRemaining"/>. The ramification
		/// of this is that a thread will be given at least one iteration to
		/// respond to any termination command that is issued before an abort is forced
		/// unless <see cref="AbortTimeRemaining"/> is 0, which is a signal value.
		/// </remarks>
		public virtual void ControlProcess(object obj)
		{
			if (obj == null)
				obj = this;

			ProcessShouldStart = true;
			while (!ProcessHasTerminated)
			{
				ControllerDelegateCallerDelegate(obj);
			}
		}
		#region Implementation of IAsyncControlObject
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// Calls base, then sets any children, if value = true.
		/// value = false does nothing. This is one-way.
		/// </summary>
		public override bool IsAborting
		{
			get { return base.IsAborting; }
			set
			{
				base.IsAborting = value;
				if (value)
				{
					if (m_ControlObjects.SafeCount() == 0)
						return;

					// Do my children.
					foreach (var controlObject in m_ControlObjects)
					{
						controlObject.IsAborting = true;
					}
				}
			}
		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// Calls base, then sets any children.
		/// </summary>
		public override bool ProcessShouldStart
		{
			get { return base.ProcessShouldStart; }
			set
			{
				base.ProcessShouldStart = value;
				if (m_ControlObjects.SafeCount() == 0)
					return;

				// Do my children.
				foreach (var controlObject in m_ControlObjects)
				{
					controlObject.ProcessShouldStart = true;
				}
			}
		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// Set is disabled, since this property is set by child callbacks.
		/// </summary>
		public override bool ProcessHasStarted
		{
			get { return base.ProcessHasStarted; }
			set
			{
			}
		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// Calls base, then sets any children, if value = true.
		/// value = false does nothing. This is one-way.
		/// </summary>
		public override bool ProcessShouldTerminate
		{
			get { return base.ProcessShouldTerminate; }
			set
			{
				base.ProcessShouldTerminate = value;
				if (value)
				{
					if (m_ControlObjects.SafeCount() == 0)
						return;

					// Do my children.
					foreach (var controlObject in m_ControlObjects)
					{
						controlObject.ProcessShouldTerminate = true;
					}
				}
			}
		}
		/// <summary>
		/// See <see cref="IAsyncControlObject"/>.
		/// Set is disabled, since this property is set by child callbacks.
		/// </summary>
		public override bool ProcessHasTerminated
		{
			get { return base.ProcessHasTerminated; }
			set
			{
			}
		}
		#endregion // Implementation of IAsyncControlObject
		#endregion // Implementation of IAsyncControllerObject
		/// <summary>
		/// <see cref="IAsyncWorkControllerObject"/>. This implementation
		/// creates a cache of objects. If dynamically loading
		/// <see cref="IAsyncControlObject"/>s, don't call this until
		/// all <see cref="IAsyncWorkControllerObject"/>s have been loaded.
		/// </summary>
		public virtual IList<IAsyncWorkControlObject> WorkControlObjects
		{
			get
			{
				if (m_WorkControlObjects != null)
					return m_WorkControlObjects;

				// Grab just the work control objects.
				m_WorkControlObjects
					= m_ControlObjects.EnumIntoSubtypeList<IAsyncControlObject, IAsyncWorkControlObject>();
				return m_WorkControlObjects;
			}
		}
		#endregion // Implementation of IAsyncWorkControllerObject
		#region IPAFDisposable Implementation
		/// <summary>
		/// <see cref="IPAFDisposable"/>. This is a virtual "backing" class for
		/// the explicit implementation. It is an override of the base class's method
		/// that also disposes resources on this subclass.
		/// </summary>
		/// <param name="disposing">
		/// <see cref="IPAFDisposable"/>.
		/// </param>
		/// <param name="obj">
		/// <see cref="IPAFDisposable"/>.
		/// This is not used in this class.
		/// </param>
		/// <returns>
		/// <see cref="IPAFDisposable"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// <see cref="IPAFDisposable"/>.
		/// When sub-classing this class (or a class like it), this is the method that should
		/// be overridden. Obviously the designer of the subclass should keep in mind the order
		/// of resource disposal that should be followed and call the base at the appropriate
		/// point (usually after the subclass call, but not always). The call to the base
		/// implementation should be wrapped in a try/catch block.
		/// </para>
		/// <para>
		/// This class should be disposed only after <see cref="IAsyncControlObject.ProcessHasTerminated"/>
		/// has been set. If this cannot be guaranteed (aborts fail or running under Silverlight and
		/// aborts aren't supported), this method should probably be called anyway, even though
		/// exceptions may be generated due to thread collisions. The locks held by this class
		/// can use significant resources DEPENDING ON THE IMPLEMENTATION.
		/// </para>
		/// <para>
		/// This virtual override has the responsibility of calling <see cref="IPAFDisposable.PAFDispose"/>
		/// method on all children.
		/// </para>
		/// </remarks>
		protected override Exception AsyncControlObjectDispose(bool disposing, object obj)
		{
			PAFStandardException<PAFAED> retval = null;

			// Dispose children first.
			// ReSharper disable once InconsistentlySynchronizedField
			//// Not synchronized on dispose.
			foreach (var childObject in m_ControlObjects)
			{
				DisposeControlObject(childObject);
			}
			var eList = new List<Exception>();
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ProcessRunTimer, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ProcessRunTimeRemaining, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_AbortTimeRemaining, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IterationsRemaining, true));

			//////////////////////////////////////////////////////////////////////////////
			// Now my superclass. Note that we do not record exceptions from the superclass
			// here. This is an arbitrary decision. We allow the superclass to do its own
			// exception recording. Exceptions will be installed in the registry
			// in both places, and each would be attached to a different object, allowing
			// their discrimination.
			var baseException = base.AsyncControlObjectDispose(disposing, obj);

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0)
			{
				var exceptions = new PAFAED(eList);
				var ex = new PAFStandardException<PAFAED>(exceptions);
				// We just put these in the registry.
				DisposalRegistry.RecordDisposalException(GetType(), ex);
				retval = ex;
			}
			// We don't record base's, but we aggregate it.
			if (baseException != null)
			{
				eList.AddNoNulls(baseException);
				var exceptions = new PAFAED(eList);
				retval = new PAFStandardException<PAFAED>(exceptions);

				// Seal the list.
				exceptions.AddException(null);
			}
			return retval;
		}

		#endregion // IPAFDisposable Implementation
		/// <summary>
		/// This receiver method will examine the changes broadcast by children
		/// control objects in response to <see cref="ProcessShouldStart"/> and
		/// <see cref="ProcessShouldTerminate"/> commands, which are issued
		/// this controller.
		/// </summary>
		/// <param name="obj">
		/// Must be a child <see cref="IAsyncControlObject"/> to have any
		/// effect.
		/// </param>
		/// <param name="args">Standard args.</param>
		protected virtual void ReceiveChildSignal(object obj, PropertyChangedEventArgs args)
		{
			if (SenderIsAChildOfOurs(obj))
			{
				if (args.PropertyName == nameof(ProcessHasStarted))
				{
					if (this.AreChildrenStarted())
						SetProcessHasStarted();

					return;
				}
				if (args.PropertyName == nameof(ProcessHasTerminated))
				{
					if (this.AreChildrenTerminated())
						SetProcessHasTerminated();
				}
			}
		}
		/// <summary>
		/// Just figures out if an object is a child of ours by examining the
		/// child collection.
		/// </summary>
		/// <param name="obj">Object to be checked.</param>
		/// <returns>
		/// <see langword="true"/> if one of our children.
		/// </returns>
		protected bool SenderIsAChildOfOurs(object obj)
		{
			if (ControlObjects.SafeCount() == 0)
				return false;
			if (ControlObjects.ContainsElement(obj))
				return true;
			return false;
		}
		/// <summary>
		/// This is special set method we need so we can disable set of the
		/// property from outside the class. Raises the property changed event
		/// for outside observers.
		/// </summary>
		protected virtual void SetProcessHasStarted()
		{
			m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_ProcessHasStarted,
				true, nameof(ProcessHasStarted));
		}
		/// <summary>
		/// This is special set method we need so we can disable set of the
		/// property from outside the class. Raises the property changed event
		/// for outside observers.
		/// </summary>
		protected virtual void SetProcessHasTerminated()
		{
			m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_ProcessHasTerminated,
				true, nameof(ProcessHasTerminated));
		}
		#region Static Helpers
		/// <summary>
		/// This method is normally called after cooperative task termination has failed.
		/// It calls <see cref="Thread.Abort()"/> on managed threads. If a timer is used,
		/// it's settings are modified to generate an immediate callback, at which time the
		/// timer is destroyed.
		/// </summary>
		/// <param name="controllerObject">
		/// Controller object whose children must be killed. <see langword="null"/> returns without processing.
		/// </param>
		/// <remarks>
		/// This method is sometimes called on an external thread - not the thread which
		/// the <paramref name="controllerObject"/> is associated with. For this reason, this
		/// method only accesses synchronized objects.
		/// </remarks>
		protected internal static void AbortBranch(IAsyncControllerObject controllerObject)
		{
			if (controllerObject == null) return;
			// Signal a stop first, since this method may be called in isolation
			// - not as a step in the shutdown procedure.
			// This gives tasks the chance to stop cooperatively.
			controllerObject.ProcessShouldTerminate = true;
			if (controllerObject.ProcessHasTerminated) return;
			// Signal an abort.
			((IAsyncControllerObjectInternal)controllerObject).SignalAbort();
			foreach (var controlObject in controllerObject.ControlObjects)
			{
				// If the control object is a controller object, we must abort it's
				// children first.
				if (controlObject is IAsyncControllerObject cntrlrObj) AbortBranch(cntrlrObj);
			}
		}
		/// <summary>
		/// This method is called to dispose an IAsyncControlObject. No check is made
		/// to determine whether the children have stopped processing. This is normally
		/// indicated by <see cref="IAsyncControlObject.ProcessHasTerminated"/>. The
		/// method checks to see if the object is a controller, then disposes its
		/// children first. After that, the incoming <see cref="IAsyncControlObject"/>
		/// itself is disposed.
		/// </summary>
		/// <param name="controlObject">
		/// If not a <see cref="IAsyncControlObjectInternal"/>, a security critical
		/// Dispose call will be made.
		/// </param>
		public static void DisposeControlObject([NotNull] IAsyncControlObject controlObject)
		{
			if (controlObject is IAsyncControllerObject controllerObject)
			{
				foreach (var cntrlObj in controllerObject.ControlObjects)
				{
					if (cntrlObj != null) DisposeControlObject(cntrlObj);
				}
			}

			// Get at our internals to get the key.
			if ((controlObject is IAsyncControlObjectInternal controlObjectInternal))
			{
				var disposer = controlObjectInternal.GetUnprotectedDisposable
					(controlObjectInternal.GetSecretDisposalKey());

				// TODO design a specific security key exception of some type, use it
				// TODO here and document the exceptions.
				if (disposer == null)
					throw new Exception("Security Key failure");
				disposer.Dispose();
			}
			else
			{
				controlObject.Dispose();
			}
		}
		#endregion // Static Helpers
		#endregion // Methods
	}
}
