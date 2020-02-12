using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutAppView : ContentPage
    {
        public AboutAppView()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (System.Exception ex)
            {

            }
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "CancelCommandRecieverAsync", "CancelCommandRecieverAsync" }
                    });
            return true;
        }
    }
}