//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using PlatformAgileFramework.FrameworkServices;
using Xamarin.Forms;
using Xamarin.FormsTestRunner.Services;
using Xamarin.FormsTestRunner.Views;

namespace Xamarin.FormsTestRunner
{
	/// <summary>
	/// This is the platform-independent top-level application that gets
	/// instantiated in one way or another by the platform pieces and is
	/// accessible to them. This module supports iOS and Android, not
	/// Windows phone, since our customer base doesn't use it. Contributors
	/// could extend this.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 15feb18 </date>
	/// <description>
	/// Porting the "xUnit emulator" to the mobiles.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe, but gets called only in app initialization path on the main thread.
	/// </threadsafety>
	// ReSharper disable once RedundantExtendsListEntry
	//// want folks to understand the relationship, ReSharper.
	public partial class SharedXamlApplication : Application //, ISharedUIApplication
	{
        public static bool s_UseMockDataStore = true;
        public static string s_BackendUrl = "https://localhost:5000";

		/// <summary>
		/// Backing for the SM. Set by this class or by trusted platform classes.
		/// </summary>
		protected internal static IPAFServiceManager<IPAFService>
			s_ServiceManager;
		/// <summary>
		/// Backing for the UI thread <see cref="SynchronizationContext"/>. Set
		/// by trusted platform classes.
		/// </summary>
		protected internal static SynchronizationContext
			s_UISynchronizationContext;

		/// <summary>
		/// This default constructor sets the service manager to be the
		/// static ROOT Generic service manager. This must be loaded in the
		/// platform-specific apps before this constructor is called.
		/// </summary>
		public SharedXamlApplication()
		{
            LoadServiceManagerXamarinMobile.PreLoadManager();

			InitializeComponent();

//            if (s_UseMockDataStore)
                DependencyService.Register<MockDataStore>();
  //          else
    //            DependencyService.Register<CloudDataStore>();

	        if (Device.RuntimePlatform == Device.iOS)
	        {
		        MainPage = new XamarinTestRunnerMainPage();
	        }
	        else
	        {
				// Note: KRM even at this late date (15Feb2018), NavigationPage acts
				// by loading/unloading a single activity on Android.
		        MainPage = new NavigationPage(new XamarinTestRunnerMainPage());
	        }
        }
		/// <summary>
		/// Holds the <see cref="SynchronizationContext"/> pushed in from the UI
		/// initialization code on each platform. This allows posts or sends to
		/// the UI thread.
		/// </summary>
		public static SynchronizationContext UISynchronizationContext
		{
			get { return s_UISynchronizationContext; }
		}

	}
}
