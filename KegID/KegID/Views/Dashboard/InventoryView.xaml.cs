using Prism.Navigation;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryView : TabbedPage, INavigationAware
    {
        public InventoryView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            CurrentPage = Children[Convert.ToInt32(parameters["currentPage"])];
        }

        protected override bool OnBackButtonPressed()
        {
            (Application.Current.MainPage.Navigation.NavigationStack.Last()?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "HomeCommandRecieverAsync", "HomeCommandRecieverAsync" }
                    });
            return true;
        }
    }
}