using GalaSoft.MvvmLight.Ioc;
using KegID.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartnersView : ContentPage
    {
        public PartnersView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            SimpleIoc.Default.GetInstance<PartnersViewModel>().LoadPartnersAsync();
            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            SimpleIoc.Default.GetInstance<PartnersViewModel>().Cleanup();
            return base.OnBackButtonPressed();
        }
    }
}