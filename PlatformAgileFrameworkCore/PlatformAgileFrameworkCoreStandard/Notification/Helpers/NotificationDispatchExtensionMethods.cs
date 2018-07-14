//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.Properties;

namespace PlatformAgileFramework.Notification.Helpers
{
	/// <summary>
	/// Helpers for notification dispatch. These all serve the role of
	/// <see cref="Action{WeakableSubscriberStore}"/>
	///  - thanks to delegate variance.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 05may2018 </date>
	/// <contribution>
	/// <description>
	/// New. refactored the old for the upgraded Weak reference store.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe.
	/// </threadsafety>
	public static class NotificationDispatchExtensionMethods
	{
		/// <summary>
		/// Plugin for the <see cref="EventArgsSubscriberStore"/>
		/// </summary>
		/// <param name="eventSubscriberStore">
		/// The store that we are handling notifications for.
		/// </param>
		public static void NotifySubscribers
			([NotNull] this EventArgsSubscriberStore eventSubscriberStore)
		{
			foreach (var subscriber in eventSubscriberStore.GetLivePDs())
			{
				subscriber.DelegateMethod.Invoke
					(subscriber.Target, new[] { eventSubscriberStore.NotificationSource, EventArgs.Empty });
			}
		}
		/// <summary>
		/// Plugin for the <see cref="GenericPAFEventArgsSubscriberStore{TPayload, TSource}"/>
		/// </summary>
		/// <param name="genericPAFEventArgsSubscriberStore">
		/// The store that we are handling notifications for.
		/// </param>
		public static void NotifySubscribers<TPayload, TSource>
		([NotNull] this GenericPAFEventArgsSubscriberStore<TPayload, TSource>
			genericPAFEventArgsSubscriberStore) where TSource : class
		{
			foreach (var subscriber in genericPAFEventArgsSubscriberStore.GetLivePDs())
			{
				subscriber.DelegateMethod.Invoke
					(subscriber.Target, new[] { genericPAFEventArgsSubscriberStore.NotificationSource, genericPAFEventArgsSubscriberStore.Payload });
			}
		}
		/// <summary>
		/// Plugin for the <see cref="PropertyChangedEventArgsSubscriberStore"/>
		/// </summary>
		/// <param name="propertyChangedEventArgsSubscriberStore">
		/// The store that we are handling notifications for.
		/// </param>
		public static void NotifySubscribers
		([NotNull] this PropertyChangedEventArgsSubscriberStore
			propertyChangedEventArgsSubscriberStore)
		{
			foreach (var subscriber in propertyChangedEventArgsSubscriberStore.GetLivePDs())
			{
				subscriber.DelegateMethod.Invoke
					(subscriber.Target, new[] { propertyChangedEventArgsSubscriberStore.NotificationSource, propertyChangedEventArgsSubscriberStore.Payload });
			}
		}
	}
}