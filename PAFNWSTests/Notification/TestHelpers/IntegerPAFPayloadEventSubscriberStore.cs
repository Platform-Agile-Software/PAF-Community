//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019Icucom Corporation
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
using PlatformAgileFramework.Events;
using PlatformAgileFramework.Notification.SubscriberStores;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.Properties;

namespace PlatformAgileFramework.Notification.TestHelpers
{
	/// <summary>
	/// Closed specialization for integer notifications and timeout testing. In
	/// this case, the store itself is the notification receiver.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17jul2019 </date>
	/// <description>
	/// New. Built for testing support.
	/// </description>
	/// </contribution>
	/// </history>
	public class IntegerPAFPayloadEventSubscriberStore
		:   GenericPAFEventArgsSubscriberStore<int, IPAFEventTimeoutReceiver>, IPAFEventTimeoutReceiver
			,ITimeOutGenericPayloadNotificationSourcedSubscriberStore<Action<object, IPAFEventArgsProvider<int>>, IPAFEventArgsProvider<int>, IPAFEventTimeoutReceiver>

	{
		#region Fields and Autpproperties
		/// <summary>
		/// Holds the non - Generic integer payload.
		/// </summary>
		public IPAFEventArgsProvider<int> IntegerPayload { get; protected set; }
		/// <summary>
		/// Collection of timed-out subscribers. It's on the store in our test.
		/// </summary>
		public IList<object> m_TimedOutSubscribers = new List<object>();
		#endregion // Fields and Autpproperties
		#region Constructors

		/// <summary>
		/// Constructor just loads the per - instance payload.
		/// </summary>
		/// <param name="integerPayload">The payload for this instance.</param>
		/// <param name="purgeIntervalInMilliseconds">
		/// See base.
		/// </param>
		/// <param name="eventDispatcherPlugin">See Base.</param>
		public IntegerPAFPayloadEventSubscriberStore(
			int integerPayload, int purgeIntervalInMilliseconds = -1,
			[CanBeNull] Action<WeakableSubscriberStore<Action<object, IPAFEventArgsProvider<int>>>> eventDispatcherPlugin = null
			)
			:base(null, purgeIntervalInMilliseconds, eventDispatcherPlugin)
		{
			IntegerPayload = new PAFEventArgs<int>(integerPayload);

			// We load this late, because it's us.
			m_NotificationSourceItem = this;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// The override that just broadcasts with the contained integer
		/// value.
		/// </summary>
		public override void NotifySubscribers()
		{
			// Work the purge.
			base.NotifySubscribers();

			foreach (var subscriber in GetLiveHandlers())
			{
				subscriber(this, IntegerPayload);
			}
		}
		/// <summary>
		/// We make ourselves a timeout receiver.
		/// </summary>
		/// <param name="obj"></param>
		public virtual void LogTimeout(object obj)
		{
			m_TimedOutSubscribers.Add(obj);
		}
		#endregion // Methods
	}
}