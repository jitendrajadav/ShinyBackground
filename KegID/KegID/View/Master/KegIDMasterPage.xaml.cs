using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KegIDMasterPage : MasterDetailPage
    {
        public KegIDMasterPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelectedAsync;

            if (Device.RuntimePlatform == Device.UWP)
                MasterBehavior = MasterBehavior.Popover;
        }

        private async void ListView_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as KegIDMasterPageMenuItem;
            if (item == null)
                return;

            switch (item.Id)
            {
                case 1:
                    SimpleIoc.Default.GetInstance<MoveViewModel>().GetUuId();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                  var result = await DisplayAlert("Warning","You have at least on draft item that will be deleted if you log out.","Stay","Log out");
                    if (!result)
                     await Application.Current.MainPage.Navigation.PopModalAsync();

                    IsPresented = false;
                    MasterPage.ListView.SelectedItem = null;
                    return;
                default:
                    break;
            }

            var page = (Page)Activator.CreateInstance(item.TargetType);

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}