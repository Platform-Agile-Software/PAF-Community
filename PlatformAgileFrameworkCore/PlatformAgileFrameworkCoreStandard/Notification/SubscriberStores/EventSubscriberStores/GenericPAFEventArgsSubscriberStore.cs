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
using PlatformAgileFramework.Events;

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
	public class GenericPAFEventArgsSubscriberStore<TPayload, TSource>
		: WeakableSubscriberStore<Action<object, IPAFEventArgsProvider<TPayload>>, TPayload>,
			IGenericNotificationSourcedSubscriberStore<Action<object, IPAFEventArgsProvider<TPayload>>, TSource>
		where TSource : class
	{
		#region Fields and Autoproperties
		/// <summary>
		/// <see cref="IGenericNotificationSourcedSubscriberStore{TDelegate, TSource}"/>
		/// </summary>
		public TSource NotificationSourceItem { get; protected set; }

		/// <summary>
		/// A little bit of weirdness in this class, since we support
		/// value and reference types. We hold the actual implementation,
		/// and preload it with the default for type. It has to be resettable.
		/// </summary>
		protected EventArgs<TPayload> m_GenericEventArgsImplementation
			= new EventArgs<TPayload>(default(TPayload));

		/// <summary>
		/// More weirdness - this what we expose - remember the interface thing, folks.
		/// </summary>
		protected IPAFEventArgsProvider<TPayload> m_GenericEventArgs;

		#endregion Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Attaches our interface to the impementation of
		/// <see cref="IPAFEventArgsProvider{T}"/> and loads the
		/// event source properties.
		/// </summary>
		/// <param name="eventSource">
		/// The incoming event source. This class is immutable WRT
		/// to this property.
		/// </param>
		/// <param name="purgeIntervalInMilliseconds">
		/// See base.
		/// </param>
		/// <param name="eventDispatherPlugin">See Base.</param>
		public GenericPAFEventArgsSubscriberStore(TSource eventSource,
			int purgeIntervalInMilliseconds = -1,
			[CanBeNull] Action<WeakableSubscriberStore<Action<object, IPAFEventArgsProvider<TPayload>>>> eventDispatherPlugin = null
			)
			:base(purgeIntervalInMilliseconds, eventDispatherPlugin)
		{
			NotificationSourceItem = eventSource;
			m_GenericEventArgs = m_GenericEventArgsImplementation;
			
		}

		#endregion // Constructors
		#region Properties
		/// <summary>
		/// <see cref="INotificationSourcedSubscriberStore"/>
		/// </summary>
		public object NotificationSource
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
			get { return m_GenericEventArgs.Value; }
			set { m_GenericEventArgsImplementation.Value = value; }
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
		/// <remarks>
		/// This method is just a little bit costly, since we have to
		/// generate wrappers for all the payloads. This method is handy,
		/// though, when the payload needs to be supplied dynamically.
		/// Use the other one unless this is really needed.
		/// </remarks>
#pragma warning restore 1584
		public override void NotifySubscribers(TPayload payload)
		{
			var selectedPayload = m_PayloadWrapper.TryGetItem(out var hasValue);
			if (!hasValue)
				selectedPayload = payload;
			var genericEventArgs = new EventArgs<TPayload>(selectedPayload);
			foreach (var subscriber in GetLiveHandlers())
			{
				subscriber(this, genericEventArgs);
			}
		}

		#endregion // Methods

	}
}