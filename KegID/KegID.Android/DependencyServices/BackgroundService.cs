//using Android.Content;
//using KegID.DependencyServices;
//using KegID.Droid.DependencyServices;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//[assembly: Dependency(typeof(BackgroundService))]
//namespace KegID.Droid.DependencyServices
//{
//    public class BackgroundService : FormsApplicationActivity, IBackgroundService
//    {
//        public void Start()
//        {
//            var intent = new Intent(Android.App.Application.Context, typeof(PeriodicService));
//            StartService(intent);
//        }

//        public void Stop()
//        {
//            var intent = new Intent(Android.App.Application.Context, typeof(PeriodicService));
//            StopService(intent);
//        }
//    }
//}