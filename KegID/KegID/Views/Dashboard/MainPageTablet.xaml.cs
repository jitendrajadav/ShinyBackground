using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageTablet : ContentPage
    {
        public MainPageTablet()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
