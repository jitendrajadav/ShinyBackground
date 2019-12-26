using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using KegID.Droid.Services;
using KegID.Messages;
using Plugin.CrossPlatformTintedImage.Android;
using Plugin.CurrentActivity;
using SegmentedControl.FormsPlugin.Android;
using Xamarin.Forms;

namespace KegID.Droid
{
    [Activity(Label = "KegID", Icon = "@drawable/icon", Theme = "@style/MainTheme.Base", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const int AccessCoarseLocationPermissionRequestCode = 0;

        private static Activity myActivity;

        protected override void OnCreate(Bundle bundle)
        {
            base.Window.RequestFeature(WindowFeatures.ActionBar);

            TabLayoutResource = Android.Resource.Layout.Tabbar;
            ToolbarResource = Android.Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            myActivity = this;

            FormsMaterial.Init(this, bundle);
            UserDialogs.Init(this);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Xamarin.FormsMaps.Init(this, bundle);
            TintedImageRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            //Forms9Patch.Droid.Settings.Initialize(this);
            SegmentedControlRenderer.Init();
            Forms.Init(this, bundle);
            LoadApplication(new App());

            WireUpLongRunningTask();
            GetAccessCoarseLocationPermission();
        }

        private void GetAccessCoarseLocationPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == Permission.Granted)
            {
                return;
            }

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessCoarseLocation))
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Permission Required")
                    .SetMessage("Zebra Developer Demos requires permission to access your location in order to perform Bluetooth discovery. Please accept this permission to allow Bluetooth discovery to function properly.")
                    .SetPositiveButton("OK", OnPermissionRequiredDialogOkClicked)
                    .SetCancelable(false)
                    .Show();

                return;
            }

            RequestAccessCoarseLocationPermission();
        }

        private void OnPermissionRequiredDialogOkClicked(object sender, DialogClickEventArgs e)
        {
            RequestAccessCoarseLocationPermission();
        }

        private void RequestAccessCoarseLocationPermission()
        {
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessCoarseLocation }, AccessCoarseLocationPermissionRequestCode);
        }

        public static Activity GetActivity()
        {
            return myActivity;
        }

        private void WireUpLongRunningTask()
        {
            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                intent.PutStringArrayListExtra("Barcode", message.Barcode);
                intent.PutExtra("PageName", message.PageName);
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", _ =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Shiny.AndroidShinyHost.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

