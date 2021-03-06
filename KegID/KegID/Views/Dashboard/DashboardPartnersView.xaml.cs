﻿
using KegID.ViewModel;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DashboardPartnersView : ContentPage
	{
        public DashboardPartnersView()
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
            ((DashboardPartnersViewModel)Prism.PrismApplicationBase.Current.MainPage.Navigation.NavigationStack.LastOrDefault()?.BindingContext).SelectedSegmentCommand.Execute(e.NewValue);
        }
    }
}