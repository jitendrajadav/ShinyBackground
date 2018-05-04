using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {
        #region Properties

        public IDashboardService _dashboardService { get; set; }

        #endregion

        #region Commands

        public RelayCommand PrinterSettingCommand { get; }
        public RelayCommand WhatsNewCommand { get; }
        public RelayCommand SupportCommand { get; }
        public RelayCommand LogOutSettingCommand { get; }
        public RelayCommand RefreshSettingCommand { get; }

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
                    try
                    {
                        AppSettings.RemoveUserData();
                    }
                    catch (Exception ex)
                    {
                         Crashes.TrackError(ex);
                    }
                    await Application.Current.MainPage.Navigation.PopPopupAsync();
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        private async void PrinterSettingCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
            await Application.Current.MainPage.Navigation.PushModalAsync(new PrinterSettingView(), animated: false);
        }

        private async void WhatsNewCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync();
            await Application.Current.MainPage.Navigation.PushModalAsync(new WhatIsNewView(), animated: false);
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
