using Prism.Navigation;
using System;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryView : Xamarin.Forms.TabbedPage ,INavigationAware
    {
        public InventoryView()
        {
            InitializeComponent();
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

        private void TabbedPage_PagesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sender.ToString() == "cnEmpty")
            {
                //cnEmpty
            }
            else
            {

            }

        }
    }
}