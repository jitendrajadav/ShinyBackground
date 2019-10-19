using KegID.ViewModel;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManifestsView : ContentPage
	{
		public ManifestsView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "HomeCommandRecieverAsync", "HomeCommandRecieverAsync" }
                    });
            return true;
        }

        private void SegControl_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            ((ManifestsViewModel)Prism.PrismApplicationBase.Current.MainPage.Navigation.NavigationStack.LastOrDefault()?.BindingContext).SelectedSegmentCommand.Execute(e.NewValue);
        }
    }
}