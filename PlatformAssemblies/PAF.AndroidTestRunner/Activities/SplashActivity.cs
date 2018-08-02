using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Activities
{
    [Activity(Label = "PAF", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            LoadServiceManagerXamarinMobile.PreLoadManager();
            base.OnCreate(savedInstanceState);
			Console.WriteLine("I'm in Android");
			var fileStore = new FileStore ();
			fileStore.SaveData(new [] {"I'm in Android"});
			Console.WriteLine("I'm in Android");
	        try
	        {
		        throw new Exception("Will I work In XamarinForms?");
	        }
	        catch (Exception ex)
	        {
		        PAFServices.Manager.GetTypedService<IPAFLoggingService>().LogEntry("Let's see....", PAFLoggingLevel.Error, ex);

	        }


		}
	}
}
