using UIKit;
using Xamarin.FormsTestRunner.XamarinFormsHelpers;

namespace Xamarin.FormsTestRunner.iOS
{
	/// <summary>
	/// This is the root of the iOS app. We will use it to store a number of application-wide
	/// services that need to be accessed platform-wide. Stuff is mostly loaded in the
	/// app delegate.
	/// </summary>
	// ReSharper disable once InconsistentNaming
    public class iOSApplication
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

    }
}
