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

using System.Threading;
using PlatformAgileFramework.Connections.BaseConnectors;
using PlatformAgileFramework.MVC.Notifications;
using PlatformAgileFramework.Notification.AbstractViewControllers;
using IGenericViewLifecycle = PlatformAgileFramework.MVC.Views.IGenericViewLifecycle;
namespace PlatformAgileFramework.MVC.Controllers
{
	/// <summary>
	/// This interface prescribes base functionality for a Graphics
	/// view controller. Binding changing needs
	/// to be paired with a binding changed - Microsoft missed it. Need to know when the
	/// change has been made so the events can be turned off.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 18may19 </date>
	/// Removed stuff for XAML support from this base interface. Mostly don't need it for
	/// components we build in code.
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11aug18 </date>
	/// New. Factored old interface to make more modular.
	/// </contribution>
	/// </history>
	public interface IControllerBase: IPropertyChangedNotificationBase,
		IPropertyChangingNotificationBase, IBindingChangedNotificationBase,
		IBindingChangingNotificationBase,
		IGenericViewLifecycle
	{
		IPAFConnector Connector { get; set; }
		/// <summary>
		/// Normally attached to any controller from the UI thread at startup.
		/// </summary>
		SynchronizationContext UISynchronizationContext { get; set; }
	}
}