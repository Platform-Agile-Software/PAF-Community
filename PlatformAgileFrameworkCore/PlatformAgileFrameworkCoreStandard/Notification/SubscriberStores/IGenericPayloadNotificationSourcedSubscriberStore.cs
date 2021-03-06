﻿
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
namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// This interface just provides a means to grab the
	/// typed publishing class that may be using a subscriber store.
	/// </summary>
	/// <typeparam name="TDelegate">See base interface.</typeparam>
	/// <typeparam name="TSource">Must be a reference type.</typeparam>
	/// <typeparam name="TPayload">An unconstrained Generic.</typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Built new event args support. Made this general
	/// for notifications.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IGenericPayloadNotificationSourcedSubscriberStore<TDelegate, TPayload, out TSource>
	 : IPayloadWeakableSubscriberStore<TDelegate, TPayload>, INotificationSourcedSubscriberStore
	where TDelegate: class where TSource : class
	{
		/// <summary>
		/// This is for Generic publisher <typeparamref name="TSource"/> argument.
		/// It refers to the reference type that published the notification.
		/// </summary>
		TSource NotificationSourceItem { get; }
	}
}
