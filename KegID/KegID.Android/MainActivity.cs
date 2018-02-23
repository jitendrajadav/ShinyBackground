
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Droid;
using KegID.Droid.DependencyServices;
using Plugin.Permissions;

namespace KegID.Droid
{
    [Activity(Label = "KegID", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Xamarin.Forms.DependencyService.Register<OpenAppService>();
            Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init(true);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Xamarin.FormsMaps.Init(this, bundle);

            LoadApplication(new App());

            var alarmIntent = new Intent(this, typeof(BackgroundReceiver));

            var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 3 * 1000, pending);

            var intent = new Intent(this, typeof(PeriodicService));
            StartService(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

