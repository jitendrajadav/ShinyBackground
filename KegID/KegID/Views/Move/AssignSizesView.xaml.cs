
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AssignSizesView : ContentPage
	{
		public AssignSizesView ()
		{
			InitializeComponent ();
		}

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.ModalStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "DoneCommandRecieverAsync", "DoneCommandRecieverAsync" }
                    });
            return true;
        }
    }
}