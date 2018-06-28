using Xamarin.Forms;

namespace KegID.PrintTemplates
{
    public class MainNavigation : NavigationPage
    {
        Page tabbedDemoPage;

        public MainNavigation()
        {
            tabbedDemoPage = new TabbedDemoPage();
            BaseDemoView.OnChoosePrinterChosen += BaseDemoView_OnChoosePrinterChosen;
            PushAsync(tabbedDemoPage);
        }
        private void BaseDemoView_OnChoosePrinterChosen()
        {
            //SelectPrinterView selectPrinterView = new SelectPrinterView();
            //SelectPrinterView.OnPrinterSelected += SelectPrinterView_OnPrinterSelected;
            //PushAsync(selectPrinterView);
        }

        private void SelectPrinterView_OnPrinterSelected(LinkOS.Plugin.Abstractions.IDiscoveredPrinter printer)
        {
            //SelectPrinterView.OnPrinterSelected -= SelectPrinterView_OnPrinterSelected;
            PopAsync();
        }

        private void App_OnBackToMainPage()
        {
            //SelectPrinterView.OnPrinterSelected -= SelectPrinterView_OnPrinterSelected;
            PopAsync();
        }
    }

}
