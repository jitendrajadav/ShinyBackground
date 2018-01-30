using System;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Services;
using KegID.View;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SettingViewModel : ViewModelBase
    {
        #region Properties
        public IDashboardService _dashboardService { get; set; }

        #endregion

        #region Commands

        public RelayCommand PrinterSettingCommand { get; set; }
        public RelayCommand WhatsNewCommand { get; set; }
        public RelayCommand SupportCommand { get; set; }
        public RelayCommand LogOutSettingCommand { get; set; }
        public RelayCommand RefreshSettingCommand { get; set; }

        #endregion

        #region Constructor

        public SettingViewModel(IDashboardService dashboardService)
        {
            RefreshSettingCommand = new RelayCommand(RefreshSettingCommandRecieverAsync);
            WhatsNewCommand = new RelayCommand(WhatsNewCommandRecieverAsync);
            SupportCommand = new RelayCommand(SupportCommandRecieverAsync);
            PrinterSettingCommand = new RelayCommand(PrinterSettingCommandRecieverAsync);
            LogOutSettingCommand = new RelayCommand(LogOutSettingCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private void RefreshSettingCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<DashboardViewModel>().RefreshDashboardRecieverAsync(true);
        }

        private async void LogOutSettingCommandRecieverAsync()
        {
            try
            {
                var result = await Application.Current.MainPage.DisplayAlert("Warning", "You have at least on draft item that will be deleted if you log out.", "Stay", "Log out");
                if (!result)
                {
                    await Application.Current.MainPage.Navigation.PopPopupAsync();
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void PrinterSettingCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
            await Application.Current.MainPage.Navigation.PushModalAsync(new PrinterSettingView());
        }

        private async void WhatsNewCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
            await Application.Current.MainPage.Navigation.PushModalAsync(new WhatIsNewView());
        }

        private async void SupportCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
            // You can remove the switch to UI Thread if you are already in the UI Thread.
            Device.BeginInvokeOnMainThread(() =>
            {
                Device.OpenUri(new Uri("https://www.slg.com/"));
            });
        }
        #endregion

    }
}
