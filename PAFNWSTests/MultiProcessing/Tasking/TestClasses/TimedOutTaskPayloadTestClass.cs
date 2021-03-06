﻿//@#$&+
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
using System.Threading.Tasks;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Events;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.Notification.SubscriberStores;
namespace PlatformAgileFramework.MultiProcessing.Tasking.TestClasses{
	/// <summary>
	/// This class to test the <see cref="TimedOutTaskPayload{T}"/> and
	/// <see cref=" IPAFEventCallbackReceiver{ITimedOutTask}"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04aug2019 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	public class TimedOutTaskPayloadTestClass
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Collection of excepted subscribers. This is to test <see cref="IExceptionPublisher"/>.
		/// </summary>
		public IList<Exception>
			m_SubscribersWithExceptions = new List<Exception>();
		/// <summary>
		/// Reference to the store. In this case, the class holds a store directly on it.
		/// </summary>
		public ITimedOutTaskGenericPayloadNotificationSourcedSubscriberStore
			<Action<object, IPAFEventArgsProvider<int>>, IPAFEventArgsProvider<int>,
				IPAFEventCallbackReceiver<ITimedOutTask>> m_Store;
		/// <summary>
		/// Provides a value type for testing.
		/// </summary>        public int IntElement { get; set; }

		/// <summary>
		/// Provides a reference type for testing.
		/// </summary>
		public string StringElement { get; set; }

		/// <summary>
		/// This is the delay in the return of the data types.
		/// </summary>
		public int DelayInMilliseconds { get; set; }
		#endregion // Fields and Autoproperties
		#region Constructors
		#endregion // Constructors
		/// <summary>
		/// This class is used for multiple test scenarios, involving async/await fixup
		/// tests.
		/// </summary>
		/// <param name="store">Not <see langword="null"/>.</param>
		public TimedOutTaskPayloadTestClass(
			[NotNull] ITimedOutTaskGenericPayloadNotificationSourcedSubscriberStore<
				Action<object, IPAFEventArgsProvider<int>>, IPAFEventArgsProvider<int>,
					IPAFEventCallbackReceiver<ITimedOutTask>> store)
		{
			m_Store = store ?? throw new ArgumentNullException(nameof(store));

			// Take a weak subscription.
			m_Store?.Subscribe(SubscriberMethod, false);

			// Subscribe to the store's exception broadcasts.
			m_Store.TransmitException += ExceptionBroadcastSubscriberMethod;
		}
		#region Methods
		// ReSharper disable once InvalidXmlDocComment
		/// <summary>
		/// A method with delay to fetch the int.
		/// </summary>
		/// <returns><see cref="Task{int}"/></returns>
		public virtual Task<int> GetTheInt()
		{
			var tcs = new TaskCompletionSource<int>();

			Task.Delay(DelayInMilliseconds).ContinueWith
				((task, o) => { tcs.SetResult(IntElement); }, null);
			return tcs.Task;
		}
		// ReSharper disable once InvalidXmlDocComment
		/// <summary>
		/// A method with delay to fetch the string.
		/// </summary>
		/// <returns><see cref="Task{string}"/></returns>
		public virtual Task<string> GetTheString()
		{
			var tcs = new TaskCompletionSource<string>();

			Task.Delay(DelayInMilliseconds).ContinueWith
				((task, o) => { tcs.SetResult(StringElement); }, null);
			return tcs.Task;
		}
		/// <summary>
		/// This method simply calls on the store to publish with a timeout.
		/// </summary>
		/// <param name="timeoutInMilliseconds">
		/// This is the time constraint to be attached to the store for a particular broadcast.
		/// If a subscriber takes longer than this to respond, it is marked as expired by the source.
		/// </param>
		public virtual void Publish(int timeoutInMilliseconds)
		{
			m_Store.NotifyPAFEventArgsSubscribersWithTimeout(timeoutInMilliseconds);
		}
		/// <summary>
		/// This subscriber method simply waits <see cref="DelayInMilliseconds"/> before returning.
		/// </summary>
		/// <param name="obj">Standard event object arg.</param>
		/// <param name="provider">Our interface-based provider.</param>
		public virtual void SubscriberMethod(object obj, IPAFEventArgsProvider<int> provider)
		{
			Task.Delay(DelayInMilliseconds).Wait();
		}
		public virtual async void GenerateAnException(bool throwException)
		{
			// Note that is a task returning a task.
			var wrappedTask = Task.Factory.StartNew(() =>
			{
				if (throwException)
					throw new Exception();
				return Task.Delay(DelayInMilliseconds);
			});
			await await wrappedTask;

		}
		/// <summary>
		/// This subscriber method collects exceptions from failed subscribers to the main
		/// event.
		/// </summary>
		/// <param name="exception">Exception to log.</param>
		public virtual void ExceptionBroadcastSubscriberMethod(Exception exception)
		{
			lock (m_SubscribersWithExceptions)
			{
				m_SubscribersWithExceptions.Add(exception);
			}
		}
		#endregion // Methods
	}}