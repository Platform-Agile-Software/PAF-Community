using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlatformAgileFramework.MVC.Controllers;
using PlatformAgileFramework.MVC.Controllers.Navigation;
using PlatformAgileFramework.MVC.Views;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.Views.XamForms;
using Xamarin.Forms;
using PropertyChangingEventHandler = System.ComponentModel.PropertyChangingEventHandler;
namespace Xamarin.FormsTestRunner.Views
{
	/// <summary>
	/// This is the main page that first emerges when the app is started. No
	/// XAML for this, just sets up things, depending which device...
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01jun2019 </date>
	/// <description>
	/// New for MVC.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe, but gets called only in app initialization path on the main thread.
	/// </threadsafety>
	/// <remarks>
	/// This is a closure of the generic <see cref="BindableObject"/> for our specific
	/// <see cref="XamarinTestRunnerMainPage"/>.
	/// </remarks>
    public class XamarinTestRunnerMainMVCPage<T>
		: XamFormsDelegatingNavigableViewBase<T, XamarinTestRunnerMainPage>
	where T: class, INavigableBaseController
	{
		public XamarinTestRunnerMainMVCPage()
		:base(new XamarinTestRunnerMainPage())
        {
			
        }

	}
}
