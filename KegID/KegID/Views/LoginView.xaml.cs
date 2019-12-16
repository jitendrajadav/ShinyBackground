
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : ContentPage
	{
        public LoginView()
        {
			//try
			//{
				InitializeComponent();
				NavigationPage.SetHasNavigationBar(this, false);
			//}
			//catch (System.Exception ex)
			//{

			//}
		}
	}
}