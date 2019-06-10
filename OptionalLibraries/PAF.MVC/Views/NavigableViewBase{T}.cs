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
	public class NavigableViewBase<T> : ViewBase, INavigableViewBase<T>
		where T : INavigableController
	{
		internal T m_ViewController;
		#region Class Fields and Autoproperties
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor just initializes the weak event stores.
		/// </summary>
		public NavigableViewBase() : base()
		{
		}
		#endregion Constructors
		/// <summary>
		/// Disposes managed and unmanaged objects.
		/// </summary>
		/// <param name="disposing">
		/// If <see langword="true"/>, disposes managed objects.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!disposing) return;
		}
		public virtual T ViewController
		{
			get => m_ViewController;
			set => m_ViewController = value;
		}
	}
}
