//using System.Threading.Tasks;
//using Foundation;
//using KegID.DependencyServices;
//using KegID.iOS.DependencyServices;
//using UIKit;


//[assembly: Xamarin.Forms.Dependency(typeof(OpenAppService))]
//namespace KegID.iOS.DependencyServices
//{
//    public class OpenAppService : IOpenAppService
//    {
//        public Task<bool> Launch(string stringUri)
//        {
//            var canOpen = UIApplication.SharedApplication.CanOpenUrl(new NSUrl(stringUri));

//            if (!canOpen)
//                return Task.FromResult(false);

//            return Task.FromResult(UIApplication.SharedApplication.OpenUrl(new NSUrl(stringUri)));
//        }
//    }
//}