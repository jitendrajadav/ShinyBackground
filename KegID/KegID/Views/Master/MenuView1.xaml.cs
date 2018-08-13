
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuView1 : ContentPage
	{
		public MenuView1 ()
		{
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
		}
	}
}