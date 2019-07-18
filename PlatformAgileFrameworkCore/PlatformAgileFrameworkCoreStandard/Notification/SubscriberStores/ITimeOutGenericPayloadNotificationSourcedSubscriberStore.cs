
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

using PlatformAgileFramework.Events;
namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// This interface is for a publishing store that must implement
	/// <see cref="IPAFEventTimeoutReceiver"/> to receive timeout notifications.
	/// </summary>
	/// <typeparam name="TDelegate">See base interface.</typeparam>
	/// <typeparam name="TSource">
	/// Must be a reference type. Must implement <see cref="IPAFEventTimeoutReceiver"/>.
	/// </typeparam>
	/// <typeparam name="TPayload">An unconstrained Generic.</typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 18jul2019 </date>
	/// <description>
	/// New. Converted from Golea.
	/// </description>
	/// </contribution>
	/// </history>
	public interface ITimeOutGenericPayloadNotificationSourcedSubscriberStore<TDelegate, TPayload, out TSource>
		:IGenericPayloadNotificationSourcedSubscriberStore<TDelegate, TPayload, TSource>
		where TDelegate: class where TSource : class, IPAFEventTimeoutReceiver
	{
	}
}
