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
using PlatformAgileFramework.MVC.Controllers.Navigation;
using PlatformAgileFramework.MVC.Notifications.Events;
using PlatformAgileFramework.MVC.Views;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Views.XamForms
{
	/// <summary>
	/// This class provides the lowest-level implementation of a base implementing
	/// class for views that are bound to a navigable content view controller.This is a
	/// version that Xamarin classes inheriting from <see cref="BindableObject"/>
	/// can delegate to.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30may19 </date>
	/// New. Navigation on controller now.
	/// </contribution>
	/// </history>
	public class XamFormsDelegatingNavigableContentViewBase<T, U>
		: IXamFormsDelegatingNavigableContentViewBase<T, U>
		where T: class, INavigableContentController where U: BindableObject
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Internal backing for testing.
		/// </summary>
		internal T m_NavigableViewController;
		/// <summary>
		/// This is the controller we delegate to for our needs.
		/// </summary>
		private readonly INavigableViewBase<T> m_NavigableViewBaseImplementation;
		/// <summary>
		/// This is the controller we delegate to for our needs.
		/// </summary>
		private U m_XamarinBindableObjectItem;
		/// <summary>
		/// non-Generic version.
		/// </summary>
		private BindableObject m_XamarinBindableObject;
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
		/// This constructor is here to support the manual construction style and the
		/// construct and set style. We need both in different situations.
		/// </summary>
		/// <param name="bindableObject">
		/// If <see langword="null"/>, this must be set with the property before
		/// the controller is set.
		/// </param>
		/// <param name="navigableController">
		/// If <see langword="null"/>, this must be set with the property after the
		/// BO is set.
		/// </param>
		/// <param name="navigableViewBase">
		/// Provides an opportunity to provide a custom <see cref="INavigableViewBase{T}"/>.
		/// <see langword="null"/> gets a <see cref="NavigableViewBase{T}"/>
		/// </param>
		/// <remarks>
		/// Read the above directions. This wrapping technique is just a BIT
		/// tricky, because we have to support several scenarios.
		/// </remarks>
		public XamFormsDelegatingNavigableContentViewBase(U bindableObject = null,
			T navigableController = null, INavigableViewBase<T> navigableViewBase = null)
		{
			m_NavigableViewController = navigableController;

			m_XamarinBindableObject = bindableObject;

			if(navigableViewBase == null)
				navigableViewBase = new NavigableViewBase<T>();

			m_NavigableViewBaseImplementation = navigableViewBase;

			if (m_XamarinBindableObject == null)
				return;

			// If we have a BO, Hook up to our shims, which rebroadcast the events.
			m_XamarinBindableObject.PropertyChanging += PropertyChangingEventHandlerShim;
			m_XamarinBindableObject.PropertyChanged += PropertyChangedEventHandlerShim;
			m_XamarinBindableObject.BindingContextChanged += BindingChangedEventHandlerShim;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="INavigableViewBase{T}"/>.
		/// This controller is a field on THIS class. We don't delegate
		/// to the one on <see cref="m_NavigableViewBaseImplementation"/>.
		/// </summary>
		/// <remarks>
		/// Note that <see cref="BindingObjectItem"/> MUST be set before
		/// the set method here is called.
		/// </remarks>
		public virtual T ViewController
		{
			get => m_NavigableViewController;
			set
			{
				m_NavigableViewController = value;
				BindingObjectItem.BindingContext = m_NavigableViewController;
			}
		}
		/// <summary>
		/// <see cref="IXamFormsDelegatingNavigableViewBase{T, U}"/>.
		/// </summary>
		public virtual U XamBindableObjectItem
		{
			get { return m_XamarinBindableObjectItem; }
			protected internal set
			{
				m_XamarinBindableObject = value;
				HookUpBO(m_XamarinBindableObjectItem);
			}
		}
		#endregion // Properties
		#region Disposal Pattern 
		public void Dispose()
		{
			m_NavigableViewBaseImplementation.Dispose();
			Dispose(true);
		}
		/// <summary>
		/// Disposes managed and unmanaged objects.
		/// </summary>
		/// <param name="disposing">
		/// If <see langword="true"/>, disposes managed objects.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
		}
		#endregion // Disposal Pattern 
		#region Implementation of IPropertyChangingNotificationBase
		event System.ComponentModel.PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
		{
			add => m_NavigableViewBaseImplementation.PropertyChanging += value;
			remove => m_NavigableViewBaseImplementation.PropertyChanging -= value;
		}
		/// <summary>
		/// This is a translation method that receives broadcasts from <see cref="BindableObject"/>
		/// and translates them to PAF-speak so we can use our notification system.
		/// </summary>
		/// <param name="sender">Received from <see cref="BindableObject"/></param>
		/// <param name="e">Received from <see cref="BindableObject"/></param>
		protected virtual void PropertyChangingEventHandlerShim(
			object sender, Xamarin.Forms.PropertyChangingEventArgs e)
		{
			// Somehow, some bozo allowed two version of the same arg class to be in the system.
			// Were any adults around when this was done? KRM
			var args = new System.ComponentModel.PropertyChangingEventArgs(e.PropertyName);
			m_NavigableViewBaseImplementation.PropertyChangingStore.NotifySubscribers(args);
		}
		public IPropertyChangingEventArgsSubscriberStore PropertyChangingStore
		{
			get => m_NavigableViewBaseImplementation.PropertyChangingStore;
		}
		#endregion Implementation of IPropertyChangingNotificationBase

		#region Implementation of IPropertyChangedNotificationBase
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default.
		/// </summary>
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add { PropertyChangedPV += value; }
			remove { PropertyChangedPV -= value; }
		}
		/// <summary>
		/// Virtual backing for the event.
		/// </summary>
		protected virtual event PropertyChangedEventHandler PropertyChangedPV
		{
			add { m_NavigableViewBaseImplementation.PropertyChangedStore.WeaklySubscribe(value); }
			remove { m_NavigableViewBaseImplementation.PropertyChangedStore.Unsubscribe(value); }
		}
		public IPropertyChangedEventArgsSubscriberStore PropertyChangedStore
		{
			get => m_NavigableViewBaseImplementation.PropertyChangedStore;
		}
		#endregion // Implementation of IPropertyChangedNotificationBase

		#region Implementation of IBindinChangedNotificationBase
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// Event hooks up weakly by default. Have to delegate to a translation
		/// event to translate to the non-Xamarin type
		/// </summary>
		event EventHandler INotifyBindingContextChanged.BindingContextChanged
		{
			add { BindingContextChangedPV += value; }
			remove { BindingContextChangedPV -= value; }
		}
		/// <summary>
		/// Virtual backing for the event.
		/// </summary>
		protected virtual event EventHandler BindingContextChangedPV
		{
			add { m_NavigableViewBaseImplementation.BindingChangedStore.WeaklySubscribe(value); }
			remove { m_NavigableViewBaseImplementation.BindingChangedStore.Unsubscribe(value); }
		}
		public IEventSubscriberStore BindingChangedStore
		{
			get => m_NavigableViewBaseImplementation.BindingChangedStore;
		}
		/// <summary>
		/// This is a translation method that receives broadcasts from <see cref="BindableObject"/>
		/// and translates them to PAF-speak so we can use our notification system.
		/// </summary>
		/// <param name="sender">Received from <see cref="BindableObject"/></param>
		/// <param name="e">Received from <see cref="BindableObject"/></param>
		protected virtual void PropertyChangedEventHandlerShim(
			object sender, PropertyChangedEventArgs e)
		{
			m_NavigableViewBaseImplementation.PropertyChangedStore.NotifySubscribers(e);
		}
		#endregion // Implementation of IBindingChangedNotificationBase
		#region  Implementation of IBindingChangingNotificationBase
		public IEventSubscriberStore BindingChangingStore
		{
			get => m_NavigableViewBaseImplementation.BindingChangingStore;
		}
		/// <summary>
		/// Virtual backing for the event.
		/// </summary>
		event EventHandler INotifyBindingContextChanging.BindingContextChanging
		{
			add { BindingContextChangingPV += value; }
			remove { BindingContextChangingPV -= value; }
		}
		/// <summary>
		/// Virtual backing for the event.
		/// </summary>
		protected virtual event EventHandler BindingContextChangingPV
		{
			add { m_NavigableViewBaseImplementation.BindingChangingStore.WeaklySubscribe(value); }
			remove { m_NavigableViewBaseImplementation.BindingChangingStore.Unsubscribe(value); }
		}
		/// <summary>
		/// This is a translation method that receives broadcasts from <see cref="BindableObject"/>
		/// and translates them to PAF-speak so we can use our notification system.
		/// </summary>
		/// <param name="sender">Received from <see cref="BindableObject"/></param>
		/// <param name="e">Received from <see cref="BindableObject"/></param>
		protected virtual void BindingChangedEventHandlerShim(
			object sender, EventArgs e)
		{
			m_NavigableViewBaseImplementation.BindingChangedStore.NotifySubscribers();
		}
		#endregion // Implementation of IBindingChangingNotificationBase
		public virtual IControllerBase ControllerBase
		{
			get => m_NavigableViewBaseImplementation.ControllerBase;
			set => m_NavigableViewBaseImplementation.ControllerBase = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double XUpperRight
		{
			get => m_NavigableViewBaseImplementation.XUpperRight;
			set => m_NavigableViewBaseImplementation.XUpperRight = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double YUpperRight
		{
			get => m_NavigableViewBaseImplementation.YUpperRight;
			set => m_NavigableViewBaseImplementation.YUpperRight = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double Width
		{
			get => m_NavigableViewBaseImplementation.Width;
			set => m_NavigableViewBaseImplementation.Width = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double Height
		{
			get => m_NavigableViewBaseImplementation.Height;
			set => m_NavigableViewBaseImplementation.Height = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual bool IsAnimating
		{
			get => m_NavigableViewBaseImplementation.IsAnimating;
			set => m_NavigableViewBaseImplementation.IsAnimating = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual bool IsEnabled
		{
			get => m_NavigableViewBaseImplementation.IsEnabled;
			set => m_NavigableViewBaseImplementation.IsEnabled = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual bool IsVisible
		{
			get => m_NavigableViewBaseImplementation.IsVisible;
			set => m_NavigableViewBaseImplementation.IsVisible = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual uint BackgroundColor
		{
			get => m_NavigableViewBaseImplementation.BackgroundColor;
			set => m_NavigableViewBaseImplementation.BackgroundColor = value;
		}
		/// <summary>
		/// See <see cref="IViewBase"/>.
		/// </summary>
		public virtual double Opacity
		{
			get => m_NavigableViewBaseImplementation.Opacity;
			set => m_NavigableViewBaseImplementation.Opacity = value;
		}
		#region Implementation of the Binders
		/// <summary>
		/// See <see cref="IObjectBindable{U}"/>.
		/// </summary>
		public virtual U BindingObjectItem
		{
			get => m_XamarinBindableObjectItem;
			protected internal set
			{
				m_XamarinBindableObject = value;
				HookUpBO(m_XamarinBindableObjectItem);
			}
		}
		/// <summary>
		/// See <see cref="IObjectBindable"/>.
		/// We don't set a raw object, since we implement the Generic, but
		/// we need to return it.
		/// </summary>
		public virtual object BindingObject
		{
			get => m_XamarinBindableObjectItem;
			set { }
		}
		#endregion // Implementation of the Binders
		#region Helpers
		/// <summary>
		/// Just a little helper to hook up to our BO's broadcasts.
		/// </summary>
		/// <param name="bindableObject">Our wrapped BO.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"bindableObject".</exception>
		/// </exceptions>
		public virtual void HookUpBO(U bindableObject)
		{
			if (bindableObject == null) throw new ArgumentNullException(nameof(bindableObject));

			// If we have a BO, Hook up to our shims, which rebroadcast the events.
			bindableObject.PropertyChanging += PropertyChangingEventHandlerShim;
			bindableObject.PropertyChanged += PropertyChangedEventHandlerShim;
			bindableObject.BindingContextChanged += BindingChangedEventHandlerShim;

		}
		#endregion // Helpers
	}
}
