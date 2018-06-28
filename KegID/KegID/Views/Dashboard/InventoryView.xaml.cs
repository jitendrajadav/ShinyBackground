using Prism.Navigation;
using System;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryView : Xamarin.Forms.TabbedPage ,INavigationAware
    {
        public InventoryView()
        {
            InitializeComponent();
            //CurrentPage = Children[SimpleIoc.Default.GetInstance<InventoryViewModel>().CurrentPage];
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            CurrentPage = Children[Convert.ToInt32(parameters["CurrentPage"])];
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            CurrentPage = Children[Convert.ToInt32(parameters["CurrentPage"])];
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {

        }
    }
}