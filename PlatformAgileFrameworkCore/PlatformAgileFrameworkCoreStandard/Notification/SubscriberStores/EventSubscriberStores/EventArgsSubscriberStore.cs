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


using System;
using PlatformAgileFramework.Properties;

namespace PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores
{
	/// <summary>
	/// Closed specialization for standard .Net non-Generic events. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Built new event args suport.
	/// </description>
	/// </contribution>
	/// </history>
	public class EventArgsSubscriberStore
		: WeakableSubscriberStore<Action<object, EventArgs>>, INotificationSourcedSubscriberStore
	{
		#region Fields and Autoproperties
		/// <summary>
		/// <see cref="INotificationSourcedSubscriberStore"/>.
		/// </summary>
		public object NotificationSource { get; protected set; }
		#endregion // Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Constructor just pushes in the object that the store is
		/// being used as a backing source for. This is the
		/// class that is originator of the event.
		/// </summary>
		/// <param name="eventSourceObject">The source of the event.</param>
		/// <param name="purgeIntervalInMilliseconds">
		/// See base.
		/// </param>
		/// <param name="eventDispatcherPlugin">See Base.</param>
		public EventArgsSubscriberStore(object eventSourceObject,
			int purgeIntervalInMilliseconds = -1,
			[CanBeNull] Action<WeakableSubscriberStore<Action<object, EventArgs>>> eventDispatcherPlugin = null
			)
			:base(purgeIntervalInMilliseconds, eventDispatcherPlugin)
		{
			NotificationSource = eventSourceObject;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// The override that just broadcasts with the <see cref="EventArgs.Empty"/>
		/// value - that's all there is!
		/// </summary>
		public override void NotifySubscribers()
		{
			// Work the purge.
			base.NotifySubscribers();

			foreach (var subscriber in GetLivePDs())
//			foreach (var subscriber in GetLiveHandlers())
			{
					subscriber.DelegateMethod.Invoke(subscriber.Target, new[]{NotificationSource, EventArgs.Empty});
				// NOTE: JMY - we need to do testing to see if creating delegates or just using the
				// PD is fastest. We may have to use a retargetable delegate if neither is fast
				// enough.
				// UPdate: Test reveals that speed is the same for PD's.
//				subscriber(this, EventArgs.Empty);
			}
		}
		#endregion // Methods
	}
}