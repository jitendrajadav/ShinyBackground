using Android.Content;
using Android.Print;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(AndroidPrintService))]
namespace KegID.Droid.DependencyServices
{
    public class AndroidPrintService : IPrintService
    {
        Context Context => CrossCurrentActivity.Current.Activity;
        public AndroidPrintService()
        {
        }

        public void Print(WebView viewToPrint)
        {

            if (Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) is Android.Webkit.WebView droidViewToPrint)
            {
                // Only valid for API 19+
                var version = Android.OS.Build.VERSION.SdkInt;

                if (version >= Android.OS.BuildVersionCodes.Kitkat)
                {
                    var printMgr = (PrintManager)Context.GetSystemService(Context.PrintService);

                    printMgr.Print("Manifest", droidViewToPrint.CreatePrintDocumentAdapter(), null);
                }
            }
        }
    }

}