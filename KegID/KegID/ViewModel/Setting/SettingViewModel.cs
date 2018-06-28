using System;
using KegID.Common;
using KegID.Messages;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        public IDashboardService _dashboardService { get; set; }

        #endregion

        #region Commands

        public DelegateCommand PrinterSettingCommand { get; }
        public DelegateCommand WhatsNewCommand { get; }
        public DelegateCommand SupportCommand { get; }
        public DelegateCommand LogOutSettingCommand { get; }
        public DelegateCommand RefreshSettingCommand { get; }

        #endregion

        #region Constructor

        public SettingViewModel(IDashboardService dashboardService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            RefreshSettingCommand = new DelegateCommand(RefreshSettingCommandRecieverAsync);
            WhatsNewCommand = new DelegateCommand(WhatsNewCommandRecieverAsync);
            SupportCommand = new DelegateCommand(SupportCommandRecieverAsync);
            PrinterSettingCommand = new DelegateCommand(PrinterSettingCommandRecieverAsync);
            LogOutSettingCommand = new DelegateCommand(LogOutSettingCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private void RefreshSettingCommandRecieverAsync()
        {
            try
            {
                //SimpleIoc.Default.GetInstance<DashboardViewModel>().RefreshDashboardRecieverAsync(true);
                SettingToDashboardMsg msg = new SettingToDashboardMsg
                {
                    IsRefresh = true
                };
                MessagingCenter.Send(msg, "SettingToDashboardMsg");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
                    //await Application.Current.MainPage.Navigation.PopPopupAsync();
                    //await Application.Current.MainPage.Navigation.PopModalAsync();
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);

                }
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        private async void PrinterSettingCommandRecieverAsync()
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PopPopupAsync();
                //await Application.Current.MainPage.Navigation.PushModalAsync(new PrinterSettingView(), animated: false);
                //await _navigationService.GoBackAsync(useModalNavigation: true);
                try
                {
                    await _navigationService.NavigateAsync(new Uri("PrinterSettingView", UriKind.Relative), useModalNavigation: true, animated: false);
                }
                catch (Exception ex)
                {

                }
                await _navigationService.ClearPopupStackAsync("SettingView", null);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void WhatsNewCommandRecieverAsync()
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PopPopupAsync();
                //await Application.Current.MainPage.Navigation.PushModalAsync(new WhatIsNewView(), animated: false);
                await _navigationService.NavigateAsync(new Uri("WhatIsNewView", UriKind.Relative), useModalNavigation: true, animated: false);
                await _navigationService.ClearPopupStackAsync("SettingView", null);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SupportCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopPopupAsync();
                // You can remove the switch to UI Thread if you are already in the UI Thread.
                Device.BeginInvokeOnMainThread(() =>
                {
                    Device.OpenUri(new Uri("https://www.slg.com/"));
                });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }

        #endregion
    }
}
