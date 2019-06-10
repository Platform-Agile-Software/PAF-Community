//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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


#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.MultiProcessing.Threading.Locks;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling.Delegates;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

#region exception shorthand
using PDED = PlatformAgileFramework.Notification.Exceptions.PAFDelegateExceptionData;
using IPDED = PlatformAgileFramework.Notification.Exceptions.IPAFDelegateExceptionData;
using PDEMT = PlatformAgileFramework.Notification.Exceptions.PAFDelegateExceptionMessageTags;
#endregion // exception shorthand
#endregion // Using directives


namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// Provides a collection of either weak or strong subscribers. Provided
	/// as an abstract class, since the "raise" method tends to be delegate-specific.
	/// This backing store makes it look to the outside world like they are
	/// working with normal <see cref="MulticastDelegate"/>s. We convert to/from
	/// our internal <see cref="IPseudoDelegate{TDelegate}"/> implemenation so
	/// the user never sees it.
	/// </summary>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// New. Created one unified store for weak and strong subscribers.
	/// Added documentation so users can understand how weak delegates/references
	/// work. Return to the "pseudodelegate" approach. Timing tests showed
	/// we didn't need open delegates at all.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// We use a reader/writer lock on this class, since access is not balanced.
	/// We need to enumerate over the handlers when calling them much more
	/// often than adding/deleting/purging handlers.
	/// </remarks>
	public class WeakableSubscriberStore<TDelegate>
		: IWeakableSubscriberStoreInternal<TDelegate>
		where TDelegate : class
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing. This wrapper is designed to give us a "side door" on the synchronized
		/// item, so we can use the item in a "using" scope. It's kind of like the "lock"
		/// pattern for a monitor. It's a form of shorthand that makes it harder to forget
		/// to release the lock. This is protected, because we need to subclass this.
		/// If you access this from your own subclass, you'd better know what you're doing.
		/// </summary>
		/// <remarks>
		/// Please note that taking even a write lock on the handlers does NOT prevent
		/// subscribers from being nullified when the lock is held. In order to get a
		/// collection of subscribers that will NOT be nullified, call <see cref="GetLiveHandlers"/>.
		/// A write lock is taken when we have to actually add or remove handlers to/from
		/// the store. This occurs during subscribe/unsubscribe and purging dead handlers.
		/// A read lock is used when a notification is sent to subscribers (when an event
		/// is raised in event-speak).
		/// </remarks>
		protected internal NullableSynchronizedWrapper<IList<IPseudoDelegate<TDelegate>>>
			m_SubscribersWrapper;
		/// <summary>
		/// Our recurring purge executor.
		/// </summary>
		protected internal IRecurringActionTimer m_RecurringPurge;
		/// <summary>
		/// This is the default purge interval.
		/// </summary>
		public const int PURGE_INTERVAL_IN_MILLISECONDS = 250;
		/// <summary>
		/// Start guard.
		/// </summary>
		protected internal volatile bool m_IsStoreStarted;
		/// <summary>
		/// Keeps track of the number of subscriber notifications
		/// for the purge.
		/// </summary>
		protected internal int m_NumBroadcasts;
		/// <summary>
		/// This is the one that can be reset for testing....
		/// </summary>
		protected internal int m_PurgeIntervalInMilliseconds;
		/// <summary>
		/// Indicator to any concurrent processes that it's time to shut
		/// down. This is used as a gate on some methods.
		/// </summary>
		protected internal volatile bool m_ShouldStoreStop;

		/// <summary>
		/// This is the method that will be called to dispatch events.
		/// If <see langword="null"/>, notification method must be overridden.
		/// </summary>
		protected Action<WeakableSubscriberStore<TDelegate>> EventDispatcherPlugin { get; set; }
		#endregion Fields and Autoproperties

		/// <summary>
		/// Verifies that the incoming Generic is, indeed, a delegate type.
		/// Since there are no Generic constraints associated with delegates,
		/// this must be done in a static constructor.
		/// </summary>
		/// <exceptions>
		/// <exception cref="InvalidOperationException">
		///"TDelegate must derive from System.MulticastDelegate."
		/// </exception>
		/// </exceptions>
		static WeakableSubscriberStore()
		{
			if (!typeof(TDelegate).IsTypeSubclassOf(typeof(MulticastDelegate)))
				throw new InvalidOperationException("TDelegate must derive from System.MulticastDelegate.");
		}

        /// <summary>
        /// Defaul constructor sets up the list and its read/write container.
        /// </summary>
        /// <param name="eventDispatcherPlugin">
        /// Plugin for dispatching events or "notifying" subscribers. If
        /// <see langword="null"/> <see cref="NotifySubscribers"/> MUST be overridden.
        /// </param>
        /// <param name="purgeIntervalInMilliseconds">
        /// Default of <see cref="int.MaxValue"/> results in use of
        /// internal static value. 0 results
        /// in purging before each notification. Positive values result in
        /// purging of dead references on a time schedule. Running the purge
        /// on a timer is useful, for example in applications that involve
        /// high-speed graphics. It does take some time to purge. In some
        /// applications, there are a large number of subscribers. 
        /// </param>
        /// <remarks>
        /// <para>
        /// This implementation is designed with sufficient flexibility so
        /// you can do things the old-fashioned way by passing
        /// in 0 for the time and calling <see cref="PurgeDeadSubscribers"/>
        /// in the mandatory override of <see cref="NotifySubscribers"/>
        /// </para>
        /// <para>
        /// This class had to be temporarily crippled due to a problem in the TPL on .Net Standard.
        /// NOTE: KRM now purge interval is forced to be 0.
        /// </para>
		/// </remarks>
        public WeakableSubscriberStore(
			int purgeIntervalInMilliseconds = int.MaxValue,
			[CanBeNull] Action<WeakableSubscriberStore<TDelegate>> eventDispatcherPlugin = null
			)
		{
			EventDispatcherPlugin = eventDispatcherPlugin;

			if (purgeIntervalInMilliseconds == int.MaxValue)
				m_PurgeIntervalInMilliseconds = PURGE_INTERVAL_IN_MILLISECONDS;
			else
				m_PurgeIntervalInMilliseconds = purgeIntervalInMilliseconds;

			var handlers = new List<IPseudoDelegate<TDelegate>>();

			// Build the R/W lock around the handler collection.
			m_SubscribersWrapper
				= new NullableSynchronizedWrapper<IList<IPseudoDelegate<TDelegate>>>
					(handlers, new ReaderWriterLockSlimWrapper());

			// Note: KRM - we have a problem right now with an element in the
			// NetStandard2.0 TPL that prevents us from using the timer.
			// m_PurgeIntervalInMilliseconds = -10;
			// Note: KRM 24mar2019 This seems to be fixed, but we don't trust it....

			// Hook up to the async wrapper so we can stop the action
			// immediately. The timer is never started if in "purge countdown"
			// mode, and it's lightweight, so we just staple it in.
			m_RecurringPurge = new RecurringActionTimer(PurgeDeadSubscribersAsyncWrapper);
		}

		#region IWeakableSubscriberStore Implementation
		/// <summary>
		/// <see cref="IWeakableSubscriberStore{TDelegate}"/>.
		/// </summary>
		public virtual void ClearSubscribers()
		{
			if (m_ShouldStoreStop)
				return;

			Start();
			using (var handlerWrapper = m_SubscribersWrapper.GetWriteLockedObject())
			{
				var handlers = handlerWrapper.WriteLockedNullableObject;
				handlers.Clear();
			}
		}

		/// <summary>
		/// <see cref="IWeakableSubscriberStore{TDelegate}"/>. This base
		/// method counts down the purge timer and purges/resets if we are
		/// in "count" mode. Doesn't matter much if called before or
		/// after logic in the override.
		/// </summary>
		/// <remarks>
		/// This one is specific to the delegate type.
		/// </remarks>
		public virtual void NotifySubscribers()
		{
			PurgeCountdown();
			// Do the notification if the developer decided to use a plugin.
			EventDispatcherPlugin?.Invoke(this);
		}

		/// <summary>
		/// In this implementation, the method kicks off the purge timer,
		/// if the interval is greater than zero. Doesn't do a thing if
		/// we are starting or stopping.
		/// </summary>
		public virtual void Start()
		{
			if (m_ShouldStoreStop)
				return;

			if (m_IsStoreStarted)
				return;

			if(m_PurgeIntervalInMilliseconds > 0)
				m_RecurringPurge.SetRecurranceTime(m_PurgeIntervalInMilliseconds);
			m_IsStoreStarted = true;
		}

		/// <summary>
		/// <see cref="IWeakableSubscriberStore{TDelegate}"/>. Does nothing if
		/// store is stopping.
		/// </summary>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFDelegateExceptionData}">
		/// <see cref="PDEMT.DELEGATE_HAS_SUBSCRIBERS"/>
		/// The delegate that is entered into the store cannot have subscribers.
		/// This backing store is used to shed "undisciplined subscribers" and
		/// if a delegate has subscribers, we don't want to deal with them.
		/// This philosophy works in all but the weirdest scenarios.
		/// </exception>
		/// </exceptions>
		public virtual void Subscribe(TDelegate addedDelegate, bool isWeak = true)
		{
			if (m_ShouldStoreStop)
				return;

			Start();
			if (addedDelegate == null) return;
			var mcd = addedDelegate as MulticastDelegate;
			// ReSharper disable once PossibleNullReferenceException
			//// Always a MCD in our case, ReSharper.
			var invocationList = mcd.GetInvocationList();
			if ((invocationList != null) && (invocationList.Length > 1))
			{
				var data = new PDED(typeof(TDelegate).ToTypeholder());
				throw new PAFStandardException<IPDED>(data, PDEMT.DELEGATE_HAS_SUBSCRIBERS);
			}

			var addedPD = addedDelegate.GetPseudoDelegate(isWeak);
			SubscribePD(addedPD);
		}

		/// <summary>
		/// Adds a <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		/// <param name="removedDelegate">Delegate to remove.</param>
		public virtual void Unsubscribe(TDelegate removedDelegate)
		{
			if (m_ShouldStoreStop)
				return;

			Start();
			if (removedDelegate == null) return;
			var removedPD = removedDelegate.GetPseudoDelegate();
			UnsubscribePD(removedPD);
		}

		/// <summary>
		/// <see cref="IWeakableSubscriberStore{TDelegate}"/>
		/// </summary>
		public virtual void WeaklySubscribe(TDelegate addedDelegate)
		{
			if (m_ShouldStoreStop)
				return;

			Start();
			Subscribe(addedDelegate);
		}
		#endregion // IWeakableSubscriberStore Implementation

		/// <summary>
		/// This helper method counts down the purge timer and purges/resets
		/// if we are in "count" mode. Normally called at the top of
		/// <see cref="NotifySubscribers"/>.
		/// </summary>
		protected virtual void PurgeCountdown()
		{
			// If not in count mode, we do nothing.
			if (m_PurgeIntervalInMilliseconds > 0)
				return;

			// Increment number of broadcasts in a thread-safe manner.
			m_NumBroadcasts = Interlocked.Add(ref m_NumBroadcasts, 1);

			// Need local for thread-safety.
			var localNumBroadcasts = m_NumBroadcasts;

			// Exit if no need to purge.
			if ((m_PurgeIntervalInMilliseconds != 0)
			    &&
			    (localNumBroadcasts % -m_PurgeIntervalInMilliseconds != 0)) return;

			// Purge and reset.
			PurgeDeadSubscribers();
			m_NumBroadcasts = 0;
		}

		/// <summary>
		/// Adds a <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		/// <param name="addedPD">PD to add.</param>
		protected virtual void SubscribePD(IPseudoDelegate<TDelegate> addedPD)
		{
			if (m_ShouldStoreStop)
				return;

			Start();
			if (addedPD == null) return;

			using (var subscribersWrapper = m_SubscribersWrapper.GetWriteLockedObject())
			{
				subscribersWrapper.WriteLockedNullableObject.AddNoDupes(addedPD);
			}
		}

		/// <summary>
		/// Removes a <see cref="IPseudoDelegate{TDelegate}"/>.
		/// </summary>
		/// <param name="removedPD">PD to remove.</param>
		protected virtual void UnsubscribePD(IPseudoDelegate<TDelegate> removedPD)
		{
			if (m_ShouldStoreStop)
				return;

			Start();
			if (removedPD == null) return;

			var subscriberIndexToRemove = -1;

			// We need to write to the list.
			using (var subscribersWrapper = m_SubscribersWrapper.GetWriteLockedObject())
			{
				// Grab a reference to the list.
				var subscriberList = subscribersWrapper.WriteLockedNullableObject;
				for (var subscriberNum = 0; subscriberNum < subscriberList.Count; subscriberNum++)
				{
					var pD = subscriberList[subscriberNum];

					// Only one subscription per subscriber can be in here.
					if (pD.Equals(removedPD))
					{
						subscriberIndexToRemove = subscriberNum;
						break;
					}
				}

				// Nothing to do?
				if (subscriberIndexToRemove < 0)
					return;

				subscriberList.RemoveAt(subscriberIndexToRemove);
			}
		}

		/// <summary>
		/// This method retrieves a list of the strong delegates and the
		/// weak delegates that have not been nullified.
		/// </summary>
		/// <returns>
		/// A list, never <see langword="null"/>, empty if store is disposed.
		/// </returns>
		/// <remarks>
		/// This method is public but NOT part of the interface. This is an
		/// implementation detail, but needs to be public so we can build
		/// extension methods to implement plugins for the event dispatcher
		/// and other things.
		/// </remarks>
		[NotNull]
		public virtual IList<TDelegate> GetLiveHandlers()
		{
			IList<TDelegate> liveDelegates = new List<TDelegate>();

			if (m_ShouldStoreStop)
				return liveDelegates;

			Start();

			using (var subscriberWrapper = m_SubscribersWrapper.GetReadLockedObject())
			{
				var subscribers = subscriberWrapper.ReadLockedNullableObject;
				// ReSharper disable once LoopCanBePartlyConvertedToQuery
				foreach (var pseudoDelegate in subscribers)
				{
					// Target is a Type (never null) for static delegates.....
					var target = pseudoDelegate.GetPseudoDelegateTargetIfAlive();
					if (target == null) continue;

					var del = pseudoDelegate.GetDelegateIfAlive();
					// Instance delegate can't be dead, since we are holding a strong
					// reference to its target.
					liveDelegates.Add(del);
				}
			}

			return liveDelegates;
		}
		/// <summary>
		/// This method retrieves a list of the strong PDs and the
		/// weak PDs that have not been nullified.
		/// </summary>
		/// <returns>
		/// A list, never <see langword="null"/>, empty if store is disposed.
		/// </returns>
		/// <remarks>
		/// This method is public but NOT part of the interface. This is an
		/// implementation detail, but needs to be public so we can build
		/// extension methods to implement plugins for the event dispatcher
		/// and other things.
		/// </remarks>
		[NotNull]
		public virtual IList<IPseudoDelegate<TDelegate>> GetLivePDs()
		{
			IList<IPseudoDelegate<TDelegate>> livePDs = new List<IPseudoDelegate<TDelegate>>();
			if (m_ShouldStoreStop)
				return livePDs;

			Start();

			using (var subscriberWrapper = m_SubscribersWrapper.GetReadLockedObject())
			{
				var subscribers = subscriberWrapper.ReadLockedNullableObject;
				// ReSharper disable once LoopCanBePartlyConvertedToQuery
				//// NOTE: It's harder to read, ReSharper!
				foreach (var pseudoDelegate in subscribers)
				{
					// Target is a Type (never null) for static delegates.....
					var target = pseudoDelegate.GetPseudoDelegateTargetIfAlive();
					if (target != null)
					{
						// Instance delegate can't be dead, since we are holding a strong
						// reference to its target.
						livePDs.Add(pseudoDelegate);
					}
				}
				// Note: Do NOT do this:
				//foreach (var pseudoDelegate in from pseudoDelegate in subscribers let target = pseudoDelegate.GetPseudoDelegateTargetIfAlive() where target != null select pseudoDelegate)
				//{
				//	// Instance delegate can't be dead, since we are holding a strong
				//	// reference to its target.
				//	livePDs.Add(pseudoDelegate);
				//}
			}

			return livePDs;
		}
		/// <summary>
		/// This is the method that is called to purge the store of "dead" weak delegates.
		/// "Strong" delegates are not affected. This is a separate method, since it is
		/// often run on a timer, since it has to use a write lock and nullification tends
		/// not to happen very frequently, so this shouldn't be done too often.
		/// </summary>
		/// <threadsafety>
		/// Safe. This locks the collection with a write lock
		/// so it should never be called within another lock.
		/// </threadsafety>
		protected virtual void PurgeDeadSubscribers()
		{
			// We've got to modify the collection, so a write lock is needed.
			using (var subscriberWrapper = m_SubscribersWrapper.GetWriteLockedObject())
			{
				var subscribers = subscriberWrapper.WriteLockedNullableObject;

				var listToRemove = new List<int>();
				// First figure out the ones that are dead.
				for (var subscriberNum = 0; subscriberNum < subscribers.Count; subscriberNum++)
				{
					var pseudoDelegate = subscribers[subscriberNum];

					var target = pseudoDelegate.GetPseudoDelegateTargetIfAlive();
					if (target == null)
						listToRemove.Add(subscriberNum);
				}
				// Now remove them.
				foreach (var indexToRemove in listToRemove)
				{
					subscribers.RemoveAt(indexToRemove);
				}
			}
		}
		/// <summary>
		/// Control wrapper that returns if stopping has been signaled.
		/// </summary>
		protected virtual void PurgeDeadSubscribersAsyncWrapper()
		{
			if (m_ShouldStoreStop)
				return;
			Start();
			PurgeDeadSubscribers();
		}

		#region Implementation of the internal interface
		//// We employ explicit interface implementation with virtual backing.
		/// <summary>
		/// <see cref="IWeakableSubscriberStoreInternal{TDelegate}"/>
		/// </summary>
		int IWeakableSubscriberStoreInternal<TDelegate>.PurgeIntervalInMillisecondsInternal
		{
			get { return PurgeIntervalInMillisecondsPV; }
		}
		/// <summary>
		/// Virtual backing for the interface.
		/// </summary>
		protected virtual int PurgeIntervalInMillisecondsPV
		{
			get { return m_PurgeIntervalInMilliseconds; }
		}
		/// <summary>
		/// <see cref="IWeakableSubscriberStoreInternal{TDelegate}"/>
		/// </summary>
		void IWeakableSubscriberStoreInternal<TDelegate>.SetPurgeIntervalInMillisecondsInternal
			(int purgeIntervalInMilliseconds)
		{
			SetPurgeIntervalInMillisecondsPV(purgeIntervalInMilliseconds);
		}
		/// <summary>
		/// Virtual backing for the interface.
		/// </summary>
		protected virtual void SetPurgeIntervalInMillisecondsPV
			(int purgeIntervalInMilliseconds)
		{
			m_PurgeIntervalInMilliseconds = purgeIntervalInMilliseconds;
		}

		/// <summary>
		/// <see cref="IWeakableSubscriberStoreInternal{TDelegate}"/>
		/// </summary>
		/// <param name="numAlive">
		/// <see cref="IWeakableSubscriberStoreInternal{TDelegate}"/>
		/// </param>
		/// <param name="numDead">
		/// <see cref="IWeakableSubscriberStoreInternal{TDelegate}"/>
		/// </param>
		void IWeakableSubscriberStoreInternal<TDelegate>.GetNumSubscribersInternal(out int numAlive, out int numDead)
		{
			GetNumSubscribersPV(out numAlive, out numDead);
		}
		/// <summary>
		/// Virtual backing for the interface.
		/// </summary>
		/// <exception cref="PAFStandardException{IPAFDelegateExceptionData}">
		/// <see cref="PDEMT.SUBSCRIBER_STORE_HAS_NOT_BEEN_STARTED"/>
		/// The start method was not called.
		/// </exception>
		protected virtual void GetNumSubscribersPV(out int numAlive, out int numDead)
		{
			Start();
			numAlive = 0;
			numDead = 0;
			using (var handlerWrapper = m_SubscribersWrapper.GetReadLockedObject())
			{
				var handlers = handlerWrapper.ReadLockedNullableObject;

				foreach (var target in handlers.Select(pseudoDelegate => pseudoDelegate.GetPseudoDelegateTargetIfAlive()))
				{
					if (target != null)
					{
						numAlive++;
						continue;
					}

					numDead++;
				}
			}
		}
		/// <summary>
		/// <see cref="IWeakableSubscriberStoreInternal{TDelegate}"/>
		/// </summary>
		void IWeakableSubscriberStoreInternal<TDelegate>.PurgeDeadSubscribersInternal()
		{
			Start();
			PurgeDeadSubscribers();
		}
		#endregion // Implementation of the internal interface
		#region Implementation of the Dispose pattern
		protected virtual void ReleaseUnmanagedResources()
		{
			// TODO release unmanaged resources here in derived classes.
		}

		/// <summary>
		/// We dispose the R/W lock and the purger.
		/// </summary>
		/// <param name="disposing">
		/// <see langword = "true"/> if we called dispose.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			ReleaseUnmanagedResources();
			if (disposing)
			{
				if (m_RecurringPurge != null)
				{
					m_RecurringPurge.SetRecurranceTime(0);

					m_RecurringPurge.Dispose();
					m_RecurringPurge = null;
				}

				ClearSubscribers();
				m_ShouldStoreStop = true;
				m_SubscribersWrapper?.Dispose();
				m_SubscribersWrapper = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// We can't implement a finalizer in this class, since we don't
		// have direct references to unmanaged handles.
		//~WeakableSubscriberStore()
		//{
		//	Dispose(false);
		//}
		#endregion // Implementation of the Dispose pattern
	}
}