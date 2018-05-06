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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-


using System;
using System.ComponentModel;
using PlatformAgileFramework.Annotations;

namespace PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores
{
	/// <summary>
	/// Closed specialization for standard <see cref="PropertyChangedEventArgs"/>. 
	/// Makes things easier, since we use it a lot.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Built mostly for Model/View/Controller suport.
	/// </description>
	/// </contribution>
	/// </history>
	public class PropertyChangedEventArgsSubscriberStore
		: WeakableSubscriberStore<PropertyChangedEventHandler, PropertyChangedEventArgs>,
			IPropertyChangedEventArgsSubscriberStore
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
		/// <param name="eventDispatherPlugin">See Base.</param>
		public PropertyChangedEventArgsSubscriberStore(object eventSourceObject,
			int purgeIntervalInMilliseconds = -1,
			[CanBeNull] Action<WeakableSubscriberStore<PropertyChangedEventHandler>> eventDispatherPlugin = null
			)
			: base(purgeIntervalInMilliseconds, eventDispatherPlugin)
		{
			NotificationSource = eventSourceObject;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// The override that just broadcasts with the
		/// <see cref="IPayloadWeakableSubscriberStore{TDelegate,TPayload}.Payload"/>.
		/// </summary>
		public override void NotifySubscribers()
		{
			// Work the purge.
			base.NotifySubscribers();

			foreach (var subscriber in GetLiveHandlers())
			{
				subscriber(NotificationSource, Payload);
			}
		}
		/// <summary>
		/// This is the fancy-schmatzy custom broadcast method that we don't
		/// need on this particular store. We just delegate to the parameterless one.
		/// </summary>
		/// <param name="payload">Not used.</param>
		public override void NotifySubscribers(PropertyChangedEventArgs payload)
		{
			NotifySubscribers();
		}
		#endregion // Methods
	}
}