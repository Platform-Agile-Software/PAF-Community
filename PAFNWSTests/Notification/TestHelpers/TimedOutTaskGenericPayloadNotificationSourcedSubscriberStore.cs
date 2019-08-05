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
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Events;
using PlatformAgileFramework.MultiProcessing.Tasking;
using PlatformAgileFramework.Notification.SubscriberStores;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.Properties;

namespace PlatformAgileFramework.Notification.TestHelpers
{
	// ReSharper disable once InvalidXmlDocComment
	/// <summary>
	/// Closed specialization for sending callbacks to a source that implements
	/// <see cref="IPAFEventCallbackReceiver{ITimedOutTask}"/> which subscribers
	/// can discover on the object in the <see cref="Action{object, TPayload}"/>
	/// sent to typical event subscribers. In this case, the store itself is the
	/// notification receiver.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04aug2019 </date>
	/// <description>
	/// New. Built for async/await support. Credit to JAW(P), since
	/// the callback receiver was originally his concept.
	/// </description>
	/// </contribution>
	/// </history>
	public class TimedOutTaskGenericPayloadNotificationSourcedSubscriberStore<TPayload>
		:   GenericEventArgsSubscriberStore<TPayload, IPAFEventCallbackReceiver<ITimedOutTask>>, IPAFEventCallbackReceiver<ITimedOutTask>
			,ITimedOutTaskGenericPayloadNotificationSourcedSubscriberStore<Action<object, TPayload>,
				TPayload, IPAFEventCallbackReceiver<ITimedOutTask>>

	{
		#region Fields and Autpproperties
		/// <summary>
		/// Collection of timed-out subscribers. It's on the store in this base class.
		/// This needs to be locked, since it's accessed concurrently.
		/// </summary>
		protected internal IList<ITimedOutTask>
			m_TimedOutSubscribers = new List<ITimedOutTask>();
		#endregion // Fields and Autpproperties
		#region Constructors
		/// <summary>
		/// Constructor just loads the per - instance payload. Sets the source to us.
		/// </summary>
		/// <param name="payload">The payload for this instance.</param>
		/// <param name="purgeIntervalInMilliseconds">
		/// See base.
		/// </param>
		/// <param name="eventDispatcherPlugin">See Base.</param>
		public TimedOutTaskGenericPayloadNotificationSourcedSubscriberStore(
			TPayload payload, int purgeIntervalInMilliseconds = -1,
			[CanBeNull] Action<IWeakableSubscriberStore<Action<object, TPayload>>> eventDispatcherPlugin = null
			)
			:base(null, purgeIntervalInMilliseconds, eventDispatcherPlugin)
		{
			m_GenericEventArgs = payload;

			// We load this late, because it's us.
			NotificationSourceItem = this;
		}
		#endregion // Constructorsv
		#region Methods
		/// <summary>
		/// The override that just broadcasts with the contained payload
		/// value.
		/// </summary>
		public override void NotifySubscribers()
		{
			// Work the purge.
			base.NotifySubscribers();

			foreach (var subscriber in GetLiveHandlers())
			{
				subscriber(NotificationSourceItem, Payload);
			}
		}
		/// <summary>
		/// We make ourselves a timeout receiver.
		/// </summary>
		/// <param name="obj">Don't do a thing with it.</param>
		public virtual void LogEventPing(object obj)
		{
		}
		/// <summary>
		/// We make ourselves a timeout receiver, so we can be the source by default.
		/// </summary>
		/// <param name="data">
		/// We stick this in our internal list. We also broadcast it. Because of
		/// legacy usage, we turn a timeout indication into a <see cref="TimeoutException"/>,
		/// so we can publish it.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"data".</exception>
		/// </exceptions>
		public virtual void LogEventPingData([Annotations.NotNull] ITimedOutTask data)
		{
			if (data == null) throw new ArgumentNullException(nameof(data));
			lock (m_TimedOutSubscribers)
			{
				m_TimedOutSubscribers.Add(data);
			}

			Exception returnedException;

			if (data.TimedOut)
			{
				returnedException = new TimeoutException(null, data.CaughtException);
			}
			else
			{
				returnedException = data.CaughtException;
			}

			TransmitException?.Invoke(returnedException);
		}
		#endregion // Methods
		#region Implementation of IExceptionPublisher
		/// <summary>
		/// <see cref="IExceptionPublisher"/>. Ironically, we use a standard (unreliable) event
		/// to implement a piece of our reliable event infrastructure. This is because we expect
		/// all subscribers to be reliable - part of a UI or something like that. Override it if
		/// you need something different.
		/// </summary>
		public virtual event Action<Exception> TransmitException;
		#endregion // Implementation of IExceptionPublisher
	}
}