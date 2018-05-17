
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Droid;
using KegID.Droid.DependencyServices;
using KegID.Droid.Services;
using KegID.Messages;
using Plugin.CrossPlatformTintedImage.Android;
using Xamarin.Forms;

namespace KegID.Droid
{
    [Activity(Label = "KegID", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            DependencyService.Register<OpenAppService>();

            Forms.SetFlags("FastRenderers_Experimental");

            Forms.Init(this, bundle);
            UserDialogs.Init(this);
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init(true);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Xamarin.FormsMaps.Init(this, bundle);
            TintedImageRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, bundle);
            LoadApplication(new App());

            //Commented for Background Services
            //var alarmIntent = new Intent(this, type: typeof(BackgroundReceiver));

            //var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            //var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            //alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 3 * 1000, pending);

            //var intent = new Intent(this, typeof(PeriodicService));
            //StartService(intent);

            //New xamarin forms BG services
            WireUpLongRunningTask();
            WireUpLongDownloadTask();

        }

        void WireUpLongRunningTask()
        {
            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                intent.PutStringArrayListExtra("Barcode", message.Barcode);
                intent.PutExtra("Page", message.Page.ToString());
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });
        }

        void WireUpLongDownloadTask()
        {
            MessagingCenter.Subscribe<DownloadMessage>(this, "Download", message => {
                var intent = new Intent(this, typeof(DownloaderService));
                intent.PutExtra("url", message.Url);
                StartService(intent);
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

