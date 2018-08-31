using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KegSearchView : ContentPage
	{
		public KegSearchView ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.ModalStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "HomeCommandRecieverAsync", "HomeCommandRecieverAsync" }
                    });
            return true;
        }
    }
}