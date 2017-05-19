using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using Gcm.Client;

namespace XFAuth.Droid
{
    [Activity(Label = "XFAuth", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //Push Notification: http://ithelp.ithome.com.tw/articles/10188320
        //public static MainActivity CurrentActivity { get; private set; }
        //private void CreateAndShowDialog(string message,string title)
        //{
        //    AlertDialog.Builder builder = new AlertDialog.Builder(this);
        //    builder.SetMessage(message);
        //    builder.SetTitle(title);
        //    builder.Create().Show();
        //}
        protected override void OnCreate(Bundle bundle)
        {
            //CurrentActivity = this;
            //AppData.AppData.tmp = this.LocalClassName;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new XFAuth.App());
            Xamarin.FormsMaps.Init(this, bundle);
            AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
            {
                System.Diagnostics.Trace.WriteLine($"Error:\r\n{e}");
                App.Current.MainPage.DisplayAlert("Error", $"{e}", "OK");
            };
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += delegate (object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
            {
                System.Diagnostics.Trace.WriteLine($"Error:\r\n{e}");
                App.Current.MainPage.DisplayAlert("Error", $"{e}", "OK");
            };
            //try
            //{
            //    // Check to ensure everything's set up right
            //    GcmClient.CheckDevice(this);
            //    GcmClient.CheckManifest(this);

            //    // Register for push notification
            //    System.Diagnostics.Debug.WriteLine("Registering...");
            //    gcmclient.register(this, pushhandlerbroadcastreceiver.sender_ids);
            //}
            //catch(Java.Net.MalformedURLException)
            //{
            //    CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            //}
            //catch(Exception e)
            //{
            //    CreateAndShowDialog(e.Message, "Error");
            //}
        }
    }
}
