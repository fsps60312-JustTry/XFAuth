using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using Gcm.Client;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace XFAuth.Droid
{
    [Activity(Label = "XFAuth", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //Push Notification: http://ithelp.ithome.com.tw/articles/10188320
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new XFAuth.App());
            Xamarin.FormsMaps.Init(this, bundle);
            AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
            {
                var msg = $"TaskScheduler.UnobservedTaskException\r\n{e}";
                System.Diagnostics.Trace.WriteLine(msg);
                App.Current.MainPage.DisplayAlert("AppDomain.CurrentDomain.UnhandledException", $"{e}", "OK");
                LogUnhandledException(new Exception("AppDomain.CurrentDomain.UnhandledException", e.ExceptionObject as Exception));
            };
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += delegate (object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
            {
                var msg = $"TaskScheduler.UnobservedTaskException\r\n{e}";
                System.Diagnostics.Trace.WriteLine(msg);
                App.Current.MainPage.DisplayAlert("TaskScheduler.UnobservedTaskException", $"{e}", "OK");
                LogUnhandledException(new Exception("TaskScheduler.UnobservedTaskException", e.Exception));
            };
            DisplayCrashReport();
        }
        #region Error handling

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        //[Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                Toast.MakeText(this, "Hooray! No error since last launch!", ToastLength.Long).Show();
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
            .SetPositiveButton("Clear", (sender, args) =>
            {
                File.Delete(errorFilePath);
            })
            .SetNegativeButton("Close", (sender, args) =>
            {
                // User pressed Close.
            })
            .SetMessage(errorText)
            .SetTitle("Crash Report")
            .Show();
        }
        #endregion
    }
}
