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
	/// Closed specialization for PAF-style Generic events using
	/// an interface-based payload. 
	/// </summary>
	/// <typeparam name="TPayload">
	/// Unconstrained Generic.
	/// </typeparam>
	/// <typeparam name="TSource">
	/// Generic must be a reference type. Make it an <see cref="object"/>
	/// to reproduce the standard .Net style.
	/// <see cref="IGenericNotificationSourcedSubscriberStore{TDelegate,TSource}"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Built new event args suport.
	/// </description>
	/// </contribution>
	/// </history>
	public class GenericEventArgsSubscriberStore<TPayload, TSource>
		: WeakableSubscriberStore<Action<object, TPayload>, TPayload>,
			IGenericPayloadNotificationSourcedSubscriberStore<Action<object, TPayload>, TPayload, TSource>
		where TSource : class
	{
		#region Fields and Autoproperties
		/// <summary>
		/// <see cref="IGenericNotificationSourcedSubscriberStore{TDelegate, TSource}"/>
		/// </summary>
		public TSource NotificationSourceItem { get; protected set; }

		/// <summary>
		/// Backing for payload.
		/// </summary>
		protected TPayload m_GenericEventArgs;

		#endregion Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Attaches our interface to the action and loads the
		/// event source properties.
		/// </summary>
		/// <param name="eventSource">
		/// The incoming event source. This class is immutable WRT
		/// to this property.
		/// </param>
		/// <param name="purgeIntervalInMilliseconds">
		/// See base.
		/// </param>
		/// <param name="eventDispatcherPlugin">See Base.</param>
		public GenericEventArgsSubscriberStore(TSource eventSource,
			int purgeIntervalInMilliseconds = -1,
			[CanBeNull] Action<IWeakableSubscriberStore<Action<object, TPayload>>> eventDispatcherPlugin = null
			)
			:base(purgeIntervalInMilliseconds, eventDispatcherPlugin)
		{
			NotificationSourceItem = eventSource;
			
		}

		#endregion // Constructors
		#region Properties
		/// <summary>
		/// <see cref="INotificationSourcedSubscriberStore"/>
		/// </summary>
		public virtual object NotificationSource
		{
			get { return NotificationSourceItem; }
		}

#pragma warning disable 1584
		/// <summary>
		/// <see cref="IWeakableSubscriberStore{TDelegate}"/>
		/// </summary>
#pragma warning restore 1584
		public override TPayload Payload
		{
			get { return m_GenericEventArgs; }
			set { m_GenericEventArgs = value; }
		}
		#endregion // Properties

		#region Methods

#pragma warning disable 1584
		/// <summary>
		/// <see cref="IWeakableSubscriberStore{TDelegate}"/>
		/// </summary>
		/// <remarks>
		/// This method can ONLY work with arguments that have been pushed in
		/// to the store.
		/// </remarks>
#pragma warning restore 1584
		public override void NotifySubscribers()
		{
			// Work the purge.
			base.NotifySubscribers();

			foreach (var subscriber in GetLiveHandlers())
			{
				subscriber(this, m_GenericEventArgs);
			}
		}
#pragma warning disable 1584
		/// <summary>
		/// <see cref="IWeakableSubscriberStore{TDelegate}"/>
		/// </summary>
#pragma warning restore 1584
		public override void NotifySubscribers(TPayload payload)
		{
			var selectedPayload = m_PayloadWrapper.TryGetItem(out var hasValue);
			if (!hasValue)
				selectedPayload = payload;
			var genericEventArgs = selectedPayload;
			foreach (var subscriber in GetLiveHandlers())
			{
				subscriber(this, genericEventArgs);
			}
		}

		#endregion // Methods

	}
}