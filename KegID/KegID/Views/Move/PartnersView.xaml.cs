using KegID.ViewModel;
using Prism.Navigation;
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
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "BackCommandRecieverAsync", "BackCommandRecieverAsync" }
                    });
            return true;
        }

        private void SegControl_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            ((PartnersViewModel)BindingContext).SelectedSegmentCommand.Execute(e.NewValue);
        }
    }
}