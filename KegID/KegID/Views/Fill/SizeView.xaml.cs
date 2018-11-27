
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SizeView : ContentPage
	{
        public SizeView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "ItemTappedCommandRecieverAsync", "ItemTappedCommandRecieverAsync" }
                    });
            return true;
        }
    }
}