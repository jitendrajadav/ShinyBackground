
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaintainView : ContentPage
	{
		public MaintainView ()
		{
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (System.Exception ex)
            {
                 Crashes.TrackError(ex);
            }
		}

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "HomeCommandRecieverAsync", "HomeCommandRecieverAsync" }
                    });
            return true;
        }
    }
}