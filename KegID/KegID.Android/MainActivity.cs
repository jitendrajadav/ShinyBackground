using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using CarouselView.FormsPlugin.Android;
using KegID.Droid.Services;
using KegID.Messages;
using Microsoft.AppCenter.Crashes;
using Plugin.CrossPlatformTintedImage.Android;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.Droid
{
    [Activity(Label = "KegID", Icon = "@drawable/icon", Theme = "@style/MainTheme.Base", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static Activity myActivity;

        // Storage Permissions
        private static readonly int REQUEST_EXTERNAL_STORAGE = 1;
        private static readonly string[] PERMISSIONS_STORAGE =
            {
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage
            };
        protected override void OnCreate(Bundle bundle)
        {
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            //base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            myActivity = this;

            Forms.SetFlags("FastRenderers_Experimental");

            Forms.Init(this, bundle);
            UserDialogs.Init(this);
            CarouselViewRenderer.Init();
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Xamarin.FormsMaps.Init(this, bundle);
            TintedImageRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);

            try
            {
                LoadApplication(new App(new AndroidInitializer()));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            WireUpLongRunningTask();
        }
        public static Activity GetActivity()
        {
            return myActivity;
        }

        void WireUpLongRunningTask()
        {
            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                intent.PutStringArrayListExtra("Barcode", message.Barcode);
                intent.PutExtra("PageName", message.PageName);
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });
        }

        protected async override void OnStart()
        {
            base.OnStart();
            bool resutl = await VerifyStoragePermissions();
        }

        public Task<bool> VerifyStoragePermissions()
        {
            // Check if we have write permission
            Permission permission = Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.WriteExternalStorage);

            if (permission != Permission.Granted)
            {
                // We don't have permission so prompt the user
                ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity,
                        PERMISSIONS_STORAGE,
                        REQUEST_EXTERNAL_STORAGE
                );
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}

