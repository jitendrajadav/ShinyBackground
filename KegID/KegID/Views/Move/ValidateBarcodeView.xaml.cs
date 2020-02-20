using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Prism.Navigation;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ValidateBarcodeView : PopupPage
    {
		public ValidateBarcodeView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "CancelCommandRecievierAsync", "CancelCommandRecievierAsync" }
                    });
            return true;
        }
    }
}