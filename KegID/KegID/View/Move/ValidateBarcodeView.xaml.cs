using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;

namespace KegID.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ValidateBarcodeView : PopupPage
    {
		public ValidateBarcodeView ()
		{
			InitializeComponent ();
		}
	}
}