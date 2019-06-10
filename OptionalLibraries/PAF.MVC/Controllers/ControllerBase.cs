//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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
using System.Threading;
using System.Threading.Tasks;
using PlatformAgileFramework.Connections.BaseConnectors;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.MVC.FrameworkServices;
using PlatformAgileFramework.MVC.Views;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
namespace PlatformAgileFramework.MVC.Controllers
{
	/// <summary>
	/// This class provides the lowest-level implementation of a base implementing
	/// class for controllers.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17may19 </date>
	/// Updated to support the updated interface.
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01apr18 </date>
	/// New. Refactored original to be more modular.
	/// </contribution>
	/// </history>

	public class ControllerBase : IControllerBase
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private readonly IPropertyChangedEventArgsSubscriberStore m_PropertyChangedStore;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private readonly IPropertyChangingEventArgsSubscriberStore m_PropertyChangingStore;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private readonly IEventSubscriberStore m_BindingChangingStore;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private readonly IEventSubscriberStore m_BindingChangedStore;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private SynchronizationContext m_UISynchronizationContext;
		private IPAFConnector m_Connector;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor just initializes the weak event stores.
		/// Recurring purge time is 0, since we have just one subscriber.
		/// </summary>
		public ControllerBase()
		{
			m_PropertyChangedStore = new PropertyChangedEventArgsSubscriberStore
				(this, 0);
			m_PropertyChangingStore = new PropertyChangingEventArgsSubscriberStore
				(this, 0);
			m_BindingChangingStore = new EventSubscriberStore
				(this, 0);
			m_BindingChangedStore = new EventSubscriberStore
				(this, 0);
		}
		#endregion Constructors
		#region Implementation of IDisposable
		/// <summary>
		/// <see cref="IDisposable"/>
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion // Implementation of IDisposable
		/// <summary>
		/// Disposes managed and unmanaged objects.
		/// </summary>
		/// <param name="disposing">
		/// If <see langword="true"/>, disposes managed objects.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;
			m_PropertyChangedStore.Dispose();
			m_PropertyChangingStore.Dispose();
			m_BindingChangingStore.Dispose();
			m_BindingChangedStore.Dispose();
		}
		#region Implementation of IPropertyChangedNotificationBase
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual IPropertyChangedEventArgsSubscriberStore PropertyChangedStore
		{
			get => m_PropertyChangedStore;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default.
		/// </summary>
		public virtual event PropertyChangedEventHandler PropertyChanged
		{
			add { m_PropertyChangedStore.WeaklySubscribe(value); }
			remove { m_PropertyChangedStore.Unsubscribe(value); }
		}
		#endregion // Implementation of IPropertyChangedNotificationBase
		#region Implementation of IPropertyChangingNotificationBase
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual IPropertyChangingEventArgsSubscriberStore PropertyChangingStore
		{
			get => m_PropertyChangingStore;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default.
		/// </summary>
		public virtual event PropertyChangingEventHandler PropertyChanging
		{
			add { m_PropertyChangingStore.WeaklySubscribe(value); }
			remove { m_PropertyChangingStore.Unsubscribe(value); }
		}
		#endregion // Implementation of IPropertyChangingNotificationBase
		#region Implementation of IBindingChangingNotificationBase
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default.
		/// </summary>
		public virtual event EventHandler BindingContextChanging
		{
			add { m_BindingChangingStore.WeaklySubscribe(value); }
			remove { m_BindingChangingStore.Unsubscribe(value); }
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default.
		/// </summary>
		public virtual IEventSubscriberStore BindingChangingStore
		{
			get => m_BindingChangingStore;
		}
		#endregion // Implementation of IBindingChangingNotificationBase
		#region Implementation of IBindingChangedNotificationBase
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default.
		/// </summary>
		public virtual event EventHandler BindingContextChanged
		{
			add { m_BindingChangedStore.WeaklySubscribe(value); }
			remove { m_BindingChangedStore.Unsubscribe(value); }
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default.
		/// </summary>
		public virtual IEventSubscriberStore BindingChangedStore
		{
			get => m_BindingChangedStore;
		}
		#endregion // Implementation of IBindingChangedNotificationBase
		public virtual SynchronizationContext UISynchronizationContext
		{
			get => m_UISynchronizationContext;
			set => m_UISynchronizationContext = value;
		}
		#region Implementation of IGenericViewLifeCycle
		public virtual Task OnAppearingAsync()
		{
			return Task.FromResult(0);
		}
		public virtual Task OnCreatingAsync(IPAFServiceManager<IViewBaseService> viewManager)
		{
			return Task.FromResult(0);
		}
		public virtual Task OnDisappearingAsync()
		{
			return Task.FromResult(0);
		}
		public virtual Task OnDestroyingAsync()
		{
			return Task.FromResult(0);
		}
		#endregion // Implementation of IGenericViewLifeCycle
		public virtual IPAFConnector Connector
		{
			get => m_Connector;
			set => m_Connector = value;
		}
	}
}