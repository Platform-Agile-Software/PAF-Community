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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using PlatformAgileFramework.Events;
using PlatformAgileFramework.MultiProcessing.Tasking;
using PlatformAgileFramework.Notification.SubscriberStores;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.Properties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		// ReSharper disable InvalidXmlDocComment
		/// <summary>
		/// Plugin for the
		/// <see cref="IGenericPayloadNotificationSourcedSubscriberStore{Action{object, IPAFEventArgsProvider{TPayload}},IPAFEventArgsProvider{TPayload}, TSource}"/>
		/// </summary>
		/// <param name="genericEventArgsSubscriberStore">
		/// The store that we are handling notifications for.
		/// </param>
		/// <param name="timeoutInMilliseconds">
		/// The amount of time the task will be allowed to complete. After this time,
		/// the task will stop and the subscriber's reference will be pushed into
		/// <see cref="IPAFEventTimeoutReceiver"/>.
		/// </param>
		// ReSharper restore InvalidXmlDocComment
		public static void NotifySubscribersWithTimeout<TPayload, TSource>
		([NotNull] this IGenericPayloadNotificationSourcedSubscriberStore<Action<object, TPayload>,TPayload, TSource>
			genericEventArgsSubscriberStore, int timeoutInMilliseconds) where TSource : class, IPAFEventTimeoutReceiver
		{
			var subscriberTasks = new List<Tuple<object, Task>>();
			foreach (var subscriber in genericEventArgsSubscriberStore.GetLivePDs())
			{
				var task = Task.Run(() =>
				{
					subscriber.DelegateMethod.Invoke
					(subscriber.Target,
						new[]
						{
							genericEventArgsSubscriberStore.NotificationSource,
							genericEventArgsSubscriberStore.Payload
						});
				});
				subscriberTasks.Add(new Tuple<object, Task>(subscriber, task));
			}

			foreach (var subscriberTask in subscriberTasks)
			{
				// In this case, we don't want to await anything, since we are watching how long completion takes.
				var timedOut = subscriberTask.Item2.WaitTaskWithTimeoutAsync(timeoutInMilliseconds).Result;
				if (timedOut)
				{
					genericEventArgsSubscriberStore.NotificationSourceItem.LogTimeout(subscriberTask.Item1);
				}
			}
		}
		// ReSharper disable InvalidXmlDocComment
		/// <summary>
		/// Plugin for the
		/// <see cref="IGenericPayloadNotificationSourcedSubscriberStore{Action{object, IPAFEventArgsProvider{TPayload}},IPAFEventArgsProvider{TPayload}, TSource}"/>
		/// This is a method that waits for all subscribers to complete, one at a time. It pushes timed out subscribers
		/// back onto the source, which wears the <see cref="IPAFEventTimeoutReceiver"/> interface.
		/// </summary>
		/// <param name="genericEventArgsSubscriberStore">
		/// The store that we are handling notifications for.
		/// </param>
		/// <param name="timeoutInMilliseconds">
		/// The amount of time the task will be allowed to complete. After this time,
		/// the task will stop and the subscriber's reference will be pushed into
		/// <see cref="IPAFEventTimeoutReceiver"/>.
		/// </param>
		// ReSharper restore InvalidXmlDocComment
		public static void NotifyPAFEventArgsSubscribersWithTimeout<TPayload, TSource>
		([NotNull] this IGenericPayloadNotificationSourcedSubscriberStore<Action<object, IPAFEventArgsProvider<TPayload>>,IPAFEventArgsProvider<TPayload>, TSource>
			genericEventArgsSubscriberStore, int timeoutInMilliseconds) where TSource : class, IPAFEventTimeoutReceiver
		{
			var subscriberTasks = new List<Tuple<object, Task>>();
			foreach (var subscriber in genericEventArgsSubscriberStore.GetLivePDs())
			{
				var task = Task.Run(() =>
				{
					subscriber.DelegateMethod.Invoke
					(subscriber.Target,
						new[]
						{
							genericEventArgsSubscriberStore.NotificationSource,
							genericEventArgsSubscriberStore.Payload
						});
				});
				subscriberTasks.Add(new Tuple<object, Task>(subscriber, task));
			}

			foreach (var subscriberTask in subscriberTasks)
			{
				// In this case, we don't want to await anything, since we are watching how long completion takes.
				var timedOut = subscriberTask.Item2.WaitTaskWithTimeoutAsync(timeoutInMilliseconds).Result;
				if (timedOut)
				{
					genericEventArgsSubscriberStore.NotificationSourceItem.LogTimeout(subscriberTask.Item1);
				}
			}
		}

		/// <summary>
		/// Plugin for the <see cref="GenericEventArgsSubscriberStore{TPayload, TSource}"/>
		/// </summary>
		/// <param name="genericEventArgsSubscriberStore">
		/// The store that we are handling notifications for.
		/// </param>
		public static void NotifySubscribers<TPayload, TSource>
		([NotNull] this IGenericPayloadNotificationSourcedSubscriberStore<Action<object, TPayload>,TPayload, TSource>
			genericEventArgsSubscriberStore) where TSource : class
		{
			foreach (var subscriber in genericEventArgsSubscriberStore.GetLivePDs())
			{
				subscriber.DelegateMethod.Invoke
					(subscriber.Target, new[] { genericEventArgsSubscriberStore.NotificationSource, genericEventArgsSubscriberStore.Payload });
			}
		}
		/// <summary>
		/// Plugin for the <see cref="GenericPAFEventArgsSubscriberStore{TPayload, TSource}"/>
		/// </summary>
		/// <param name="genericPAFEventArgsSubscriberStore">
		/// The store that we are handling notifications for.
		/// </param>
		public static void NotifySubscribers<TPayload, TSource>
		([NotNull] this IGenericPayloadNotificationSourcedSubscriberStore<Action<object, IPAFEventArgsProvider<TPayload>>,TPayload, TSource>
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