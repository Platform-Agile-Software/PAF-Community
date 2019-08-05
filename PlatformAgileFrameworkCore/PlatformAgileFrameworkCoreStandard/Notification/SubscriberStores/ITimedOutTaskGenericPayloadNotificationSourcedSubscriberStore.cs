
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

using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Events;
using PlatformAgileFramework.MultiProcessing.Tasking;
namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// This interface is for a publishing store that must implement
	/// <see cref="IPAFEventCallbackReceiver{ITimedOutTask}"/> to receive
	/// timeout notifications.
	/// </summary>
	/// <typeparam name="TDelegate">See base interface.</typeparam>
	/// <typeparam name="TSource">
	/// Must be a reference type. Must implement <see cref="IPAFEventCallbackReceiver"/>.
	/// </typeparam>
	/// <typeparam name="TPayload">An unconstrained Generic.</typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 03aug2019 </date>
	/// <description>
	/// New. Added the <see cref="IExceptionPublisher"/> as per JAW(S)'s work.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// This store captures <see cref="ITimedOutTask"/>s and also rebroadcasts them as
	/// exceptions so subscribers which may be way up the stack can receive them reliably
	/// even when they are thrown on thread pool threads from async methods, which has
	/// always been problematic.
	/// </remarks>
	public interface ITimedOutTaskGenericPayloadNotificationSourcedSubscriberStore<TDelegate, TPayload, out TSource>
		:IGenericPayloadNotificationSourcedSubscriberStore<TDelegate, TPayload, TSource>, IExceptionPublisher
		where TDelegate: class where TSource : class, IPAFEventCallbackReceiver<ITimedOutTask>
	{
	}
}
