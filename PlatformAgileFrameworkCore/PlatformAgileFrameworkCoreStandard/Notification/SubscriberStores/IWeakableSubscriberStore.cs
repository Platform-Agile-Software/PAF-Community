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

namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// Protocol for a collection of either weak or strong subscribers. An
	/// implementation can pull the essential components of a delegate
	/// out of it - the target and the method info. It's the target we
	/// wish to hold with a weak reference.
	/// </summary>
	/// <typeparam name="TDelegate">
	/// This must be checked at LOAD time to see if it is a <see cref="MulticastDelegate"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// New. Created one unified store for weak and strong subscribers.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// <para>
	/// Implementing classes can solve a problem with every <see cref="Delegate"/>
	/// having to be a <see cref="MulticastDelegate"/>. Microsoft made a
	/// big mistake when they designed the delegate system, since there
	/// was no way to build something one could invoke that was just a
	/// reference to a method and its target. Implementations CAN refuse
	/// to allow delegates with an invocation list to subscribe. This
	/// solves a lot of problems in concurrent systems.
	/// </para>
	/// <para>
	/// We derive from the the dispose pattern, to try to enforce
	/// dispoasal of resources. Implementations can use R/W locks,
	/// which are typically disposable. We need to dispose just from
	/// the interface.
	/// </para>
	/// </remarks>
	// ReSharper disable once TypeParameterCanBeVariant
	//// Sorry, ReSharper, we need an exact match.
	public interface IWeakableSubscriberStore<TDelegate>: IDisposable
		where TDelegate : class
	{
		#region Methods
		/// <summary>
		/// Removes all subscribers from the store.
		/// </summary>
		void ClearSubscribers();
		/// <summary>
		/// This method broadcasts to subscribers.
		/// </summary>
		void NotifySubscribers();
		/// <summary>
		/// Adds a subscriber to the invocation list.
		/// </summary>
		/// <param name="addedDelegate">Del to add.</param>
		/// <param name="isWeak">
		/// Indicates weak/strong subscription.
		/// </param>
		void Subscribe(TDelegate addedDelegate, bool isWeak);
		/// <summary>
		/// This method is provided because the store typically
		/// cannot be started in the constructor and that's a
		/// very bad thing to do, anyway. 
		/// </summary>
		void Start();
		/// <summary>
		/// Removess a subscriber from the invocation list.
		/// </summary>
		/// <param name="removedDelegate">Del to remove.</param>
		void Unsubscribe(TDelegate removedDelegate);
		/// <summary>
		/// Adds a subscriber to the invocation list.
		/// </summary>
		/// <param name="addedDelegate">Del to add.</param>
		/// <remarks>
		/// This one adds a weak subscription.
		/// This additional signature is needed for explicit implementation.
		/// </remarks>
		void WeaklySubscribe(TDelegate addedDelegate);
		#endregion Methods
	}
}