using KegID.DependencyServices;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace KegID.iOS.DependencyServices
{
    public class IOSPrintService : IPrintService
    {
        public IOSPrintService()
        {
        }

        public void Print(WebView viewToPrint)
        {
            var appleViewToPrint = Platform.CreateRenderer(viewToPrint).NativeView;

            var printInfo = UIPrintInfo.PrintInfo;

            printInfo.OutputType = UIPrintInfoOutputType.General;
            printInfo.JobName = "Forms Print";
            printInfo.Orientation = UIPrintInfoOrientation.Portrait;
            printInfo.Duplex = UIPrintInfoDuplex.None;

            var printController = UIPrintInteractionController.SharedPrintController;

            printController.PrintInfo = printInfo;
            printController.ShowsPageRange = true;
            printController.PrintFormatter = appleViewToPrint.ViewPrintFormatter;

            printController.Present(true, (printInteractionController, completed, error) => { });
        }
    }

}