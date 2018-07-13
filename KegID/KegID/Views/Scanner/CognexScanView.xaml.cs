using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CognexScanView : ContentPage
	{
        public CognexScanView ()
		{
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void ZXingDefaultOverlay_FlashButtonClicked(Button sender, System.EventArgs e)
        {
            zxing.IsTorchOn = !zxing.IsTorchOn;
        }
    }
}