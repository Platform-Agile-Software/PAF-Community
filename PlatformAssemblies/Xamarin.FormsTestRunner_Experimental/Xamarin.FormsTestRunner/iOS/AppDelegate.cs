using System;
using System.IO;
using System.Threading;
using Foundation;
using PlatformAgileFramework.Application;
using PlatformAgileFramework.Platform;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.FormsTestRunner.XamarinFormsHelpers;

namespace Xamarin.FormsTestRunner.iOS
{
	/// <summary>
	/// Folks should keep in mind that an objective-c "delegate" is very different from
	/// a .Net "delegate". An objective-C delegate is not quite a class, It is an object that
	/// implements a "protocol" (sort of like a C# interface) that acts on behalf of
	/// another object. C# does not have the very same model, so it's implemented
	/// as a class with virtual methods that can be overridden. It all works just fine....
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
	[Register("AppDelegate")]
	// ReSharper disable once PartialTypeWithSinglePart
    public partial class AppDelegate : FormsApplicationDelegate
	{

		/// <summary>
		/// This is the traditional point at which application setup is done.
		/// We load the platform-independent application to let iOS use the Xaml and
		/// other platform-independent parts of the app in the way it wants to
		/// interpret them.
		/// </summary>
		/// <param name="app">
		/// The iOS app that we act as a delegate for.
		/// </param>
		/// <param name="options">
		/// Options that are used at startup, but are not touched in this application.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if there is trouble. iOS needs this to signal
		/// the user that there is a problem.
		/// </returns>
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Forms.Init();

	        var sA = new SharedXamlApplication();
	        // We are on a UI thread, so we can set the global synchronization context.
	        SharedXamlApplication.s_UISynchronizationContext = SynchronizationContext.Current;

	        LoadApplication(sA);
	        var documents =
		        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

	        var cDirectoryname = Path.Combine(documents, "CDrive");
	        var cDir = Directory.CreateDirectory(cDirectoryname);
	        PlatformUtils.s_C_DriveMapping = cDirectoryname;

	        var dDirectoryname = Path.Combine(documents, "DDrive");
	        var dDir = Directory.CreateDirectory(dDirectoryname);
	        PlatformUtils.s_D_DriveMapping = dDirectoryname;

			return base.FinishedLaunching(app, options);
        }
    }
}
