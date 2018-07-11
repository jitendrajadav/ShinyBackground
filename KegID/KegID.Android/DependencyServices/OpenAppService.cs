//using System.Threading.Tasks;
//using Android.App;
//using Android.Content;
//using KegID.DependencyServices;
//using KegID.Droid.DependencyServices;

//[assembly: Xamarin.Forms.Dependency(typeof(OpenAppService))]
//namespace KegID.Droid.DependencyServices
//{
//    [Activity(Label = "OpenAppService")]
//    public class OpenAppService : Activity, IOpenAppService
//    {
//        public Task<bool> Launch(string stringUri)
//        {
//            bool result = false;

//            try
//            {
//                var aUri = Android.Net.Uri.Parse(stringUri.ToString());
//                var intent = new Intent(Intent.ActionView, aUri);
//                Application.Context.StartActivity(intent);
//                result = true;
//            }
//            catch (ActivityNotFoundException)
//            {
//                result = false;
//            }

//            return Task.FromResult(result);
//        }
//    }
//}