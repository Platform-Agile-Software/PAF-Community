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
using PlatformAgileFramework.MVC.Controllers;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
namespace PlatformAgileFramework.MVC.Views
{
	/// <summary>
	/// This class provides the lowest-level implementation of a base implementing
	/// class for views.
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
	/// <remarks>
	/// Both view and controller must broadcast property changed events.
	/// </remarks>
	public class ViewBase : IViewBase
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private IControllerBase m_ControllerBase;
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
		private object m_BindingObject;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private double m_XUpperRight;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private double m_YUpperRight;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private double m_Width;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private double m_Height;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private bool m_IsAnimating;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private bool m_IsEnabled;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private bool m_IsVisible;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private uint m_BackgroundColor;
		/// <summary>
		/// Backing for virtual property.
		/// </summary>
		private double m_Opacity;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor just initializes the weak event stores.
		/// </summary>
		public ViewBase()
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
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual IControllerBase ControllerBase
		{
			get => m_ControllerBase;
			set => m_ControllerBase = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double XUpperRight
		{
			get => m_XUpperRight;
			set => m_XUpperRight = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double YUpperRight
		{
			get => m_YUpperRight;
			set => m_YUpperRight = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double Width
		{
			get => m_Width;
			set => m_Width = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double Height
		{
			get => m_Height;
			set => m_Height = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual bool IsAnimating
		{
			get => m_IsAnimating;
			set => m_IsAnimating = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual bool IsEnabled
		{
			get => m_IsEnabled;
			set => m_IsEnabled = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual bool IsVisible
		{
			get => m_IsVisible;
			set => m_IsVisible = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual uint BackgroundColor
		{
			get => m_BackgroundColor;
			set => m_BackgroundColor = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public double Opacity
		{
			get => m_Opacity;
			set => m_Opacity = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual object BindingObject
		{
			get => m_BindingObject;
			set => m_BindingObject = value;
		}
		#region Disposal Pattern 
		public void Dispose()
		{
			Dispose(true);
		}
		/// <summary>
		/// Disposes managed and unmanaged objects.
		/// </summary>
		/// <param name="disposing">
		/// If <see langword="true"/>, disposes the stores.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;

			m_BindingChangedStore.Dispose();
			m_BindingChangingStore.Dispose();
			m_PropertyChangedStore.Dispose();
			m_PropertyChangedStore.Dispose();
		}
		#endregion // Disposal Pattern 
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
	}
}
