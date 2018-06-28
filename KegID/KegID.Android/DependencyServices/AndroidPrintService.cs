//using Android.Content;
//using Android.Print;
//using KegID.DependencyServices;
//using KegID.Droid.DependencyServices;
//using Microsoft.AppCenter.Crashes;
//using System;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//[assembly: Dependency(typeof(AndroidPrintService))]
//namespace KegID.Droid.DependencyServices
//{
//    public class AndroidPrintService : IPrintService
//    {
//        public AndroidPrintService()
//        {
//        }

//        public void Print(WebView viewToPrint)
//        {
//            try
//            {

//                if (Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) is Android.Webkit.WebView droidViewToPrint)
//                {
//                    // Only valid for API 19+
//                    var version = Android.OS.Build.VERSION.SdkInt;

//                    if (version >= Android.OS.BuildVersionCodes.Kitkat)
//                    {
//                        var printMgr = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);

//                        printMgr.Print("Manifest", droidViewToPrint.CreatePrintDocumentAdapter(), null);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Crashes.TrackError(ex);
//            }
//        }
//    }

//}