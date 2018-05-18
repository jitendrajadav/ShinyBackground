using Android.Content;
using Android.Print;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(AndroidPrintService))]
namespace KegID.Droid.DependencyServices
{
    public class AndroidPrintService : IPrintService
    {
        public AndroidPrintService()
        {
        }

        public void Print(WebView viewToPrint)
        {
            var droidViewToPrint = Platform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

            if (droidViewToPrint != null)
            {
                // Only valid for API 19+
                var version = Android.OS.Build.VERSION.SdkInt;

                if (version >= Android.OS.BuildVersionCodes.Kitkat)
                {
                    var printMgr = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);

                    printMgr.Print("Manifest", droidViewToPrint.CreatePrintDocumentAdapter(), null);
                }
            }
        }
    }

}