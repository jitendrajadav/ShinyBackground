using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Prism.Navigation;
using System.Linq;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ValidateBarcodeView : PopupPage
    {
		public ValidateBarcodeView ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.ModalStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "CancelCommandRecievierAsync", "CancelCommandRecievierAsync" }
                    });
            return true;
        }
    }
}