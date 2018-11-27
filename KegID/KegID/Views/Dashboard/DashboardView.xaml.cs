
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DashboardView : ContentView
	{
		public DashboardView()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

    }
}