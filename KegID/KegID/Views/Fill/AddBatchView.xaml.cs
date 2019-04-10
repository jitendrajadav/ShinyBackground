
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddBatchView : ContentPage
	{
		public AddBatchView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            bestDatePicker.Unfocused += BestDatePicker_Unfocused;
        }

        private void BestDatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            // Needs to debbug and assing the correct value
            //var tmp = Date;
            //Date = DateTime.MinValue;
            //Date = tmp;
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "CancelCommandRecievierAsync", "CancelCommandRecievierAsync" }
                    });
            return true;
        }
    }
}