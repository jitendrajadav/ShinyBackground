

using KegID.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanKegsView : ContentPage
	{
		public ScanKegsView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //ViewModelLocator.Cleanup();
            //SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Cleanup();
        }
    }
}