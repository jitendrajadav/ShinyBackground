using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using CarouselView.FormsPlugin.Android;
using KegID.Droid.Services;
using KegID.Messages;
using Microsoft.AppCenter.Crashes;
using Plugin.CrossPlatformTintedImage.Android;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using SegmentedControl.FormsPlugin.Android;
using System;
using Xamarin.Forms;

namespace KegID.Droid
{
    [Activity(Label = "KegID", Icon = "@drawable/icon", Theme = "@style/MainTheme.Base", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static Activity myActivity;

        protected override void OnCreate(Bundle bundle)
        {
            base.Window.RequestFeature(WindowFeatures.ActionBar);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            myActivity = this;

            Forms.SetFlags("FastRenderers_Experimental");
            Forms.Init(this, bundle);
            FormsMaterial.Init(this, bundle);
            UserDialogs.Init(this);
            CarouselViewRenderer.Init();
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            try
            {
                FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            }
            catch (Exception)
            {

            }
            Xamarin.FormsMaps.Init(this, bundle);
            TintedImageRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            Forms9Patch.Droid.Settings.Initialize(this);
            SegmentedControlRenderer.Init();
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
            try
            {
                MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message =>
                {
                    var intent = new Intent(this, typeof(LongRunningTaskService));
                    intent.PutStringArrayListExtra("Barcode", message.Barcode);
                    intent.PutExtra("PageName", message.PageName);
                    StartService(intent);
                });
            }
            catch (Exception ex)
            {

            }

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });
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

