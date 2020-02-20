using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Navigation;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectPrinterView : ContentPage
	{
        public SelectPrinterView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "GoBackMethod", "GoBackMethod" }
                    });
            return true;
        }
    }

    public enum ConnectionType
    {
        Bluetooth,
        USB,
        Network
    }
}