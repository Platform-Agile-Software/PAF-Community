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
using System.Drawing;
using PlatformAgileFramework.MVC.Controllers;
using PlatformAgileFramework.MVC.Notifications;
using PlatformAgileFramework.Notification.AbstractViewControllers;
namespace PlatformAgileFramework.MVC.Views
{
	/// <summary>
	/// This interface provides a prescription for what any view in the
	/// PAF MVC model must implement.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 18may19 </date>
	/// Removed stuff for XAML support from this base interface. Mostly don't need it for
	/// components we build in code.
	/// </contribution>
	/// <author> KRM </author>
	/// <date> 01apr18 </date>
	/// New. Refactored original to be more modular.
	/// </contribution>
	/// </history>
	/// <remarks>
	/// Both view and controller must broadcast changed events. Binding changing needs
	/// to be paired with a binding changed - Microsoft missed it. Need to know when the
	/// change has been made so the events can be turned off.
	/// </remarks>
	public interface IViewBase : IPropertyChangedNotificationBase,
		IPropertyChangingNotificationBase, IBindingChangedNotificationBase,
		IBindingChangingNotificationBase, IObjectBindable
	{
		/// <summary>
		/// Reference to this view's controller.
		/// </summary>
		IControllerBase ControllerBase { get; set; }
		/// <summary>
		/// Traditional reference to upper right corner.
		/// </summary>
		double XUpperRight { get; set; }
		/// <summary>
		/// Traditional reference to upper right corner.
		/// </summary>
		double YUpperRight { get; set; }
		/// <summary>
		/// Width of the element.
		/// </summary>
		double Width { get; set; }
		/// <summary>
		/// Height of the element.
		/// </summary>
		double Height { get; set; }
		/// <summary>
		/// Determines whether the view is being animated.
		/// </summary>
		bool IsAnimating { get; set; }
		/// <summary>
		/// Determines whether the view takes up space in a layout.
		/// </summary>
		bool IsEnabled { get; set; }
		/// <summary>
		/// Determines whether the view can be seen. Good for switching overlaid views on/off.
		/// </summary>
		bool IsVisible { get; set; }
		/// <summary>
		/// Platform-independent background color of the view.
		/// </summary>
		uint BackgroundColor { get; set; }
		/// <summary>
		/// See-through.
		/// </summary>
		double Opacity { get; set; }

	}
}
