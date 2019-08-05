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
using System.Threading.Tasks;
using PlatformAgileFramework.Events;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.Notification.SubscriberStores;
namespace PlatformAgileFramework.MultiProcessing.Tasking.TestClasses{
	/// <summary>
	/// This class to test the  <see cref="TimedOutTaskPayload{T}"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17jul2019 </date>
	/// <description>
	/// Modified to be a <see cref="IPAFEventCallbackReceiver"/> so we can also test
	/// <see cref="NotificationDispatchExtensionMethods"/>.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 14jul2019 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	public class TimedOutPayloadTestClass
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Reference to the store.
		/// </summary>
		public ITimeOutGenericPayloadNotificationSourcedSubscriberStore
			<Action<object, IPAFEventArgsProvider<int>>, IPAFEventArgsProvider<int>, IPAFEventCallbackReceiver> m_Store;
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
		public TimedOutPayloadTestClass(
			ITimeOutGenericPayloadNotificationSourcedSubscriberStore<
				Action<object, IPAFEventArgsProvider<int>>, IPAFEventArgsProvider<int>, IPAFEventCallbackReceiver> store  = null)
		{
			m_Store = store;

			// Take a weak subscription.
			m_Store?.Subscribe(SubscriberMethod, false);
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
		/// This is the time constraint to be attached to the store for a particular broadcast.
		/// If a subscriber takes longer than this to respond, it is marked as expired by the source.
		/// </summary>
		/// <param name="timeoutInMilliseconds"></param>
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
		#endregion // Methods
	}}