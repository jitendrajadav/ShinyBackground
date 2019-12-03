using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanditScanView : ContentPage
    {
        public ScanditScanView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //private void ZXingDefaultOverlay_FlashButtonClicked(Button sender, System.EventArgs e)
        //{
        //    zxing.IsTorchOn = !zxing.IsTorchOn;
        //}
    }
}