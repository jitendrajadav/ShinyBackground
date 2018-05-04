
using Microsoft.AppCenter.Crashes;
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
            }
            catch (System.Exception ex)
            {
                 Crashes.TrackError(ex);
            }
		}
	}
}