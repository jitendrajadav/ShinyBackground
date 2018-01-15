
using GalaSoft.MvvmLight.Ioc;
using KegID.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoveView : ContentPage
	{
		public MoveView ()
		{
			InitializeComponent ();
		}

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}