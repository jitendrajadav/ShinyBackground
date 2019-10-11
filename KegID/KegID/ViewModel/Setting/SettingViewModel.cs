using System;
using KegID.Common;
using KegID.Messages;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SettingViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;

        #endregion

        #region Commands

        public DelegateCommand PrinterSettingCommand { get; }
        public DelegateCommand WhatsNewCommand { get; }
        public DelegateCommand SupportCommand { get; }
        public DelegateCommand LogOutSettingCommand { get; }
        public DelegateCommand RefreshSettingCommand { get; }
        public DelegateCommand AboutAppCommand { get; }

        #endregion

        #region Constructor

        public SettingViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            RefreshSettingCommand = new DelegateCommand(RefreshSettingCommandRecieverAsync);
            WhatsNewCommand = new DelegateCommand(WhatsNewCommandRecieverAsync);
            SupportCommand = new DelegateCommand(SupportCommandRecieverAsync);
            PrinterSettingCommand = new DelegateCommand(PrinterSettingCommandRecieverAsync);
            LogOutSettingCommand = new DelegateCommand(LogOutSettingCommandRecieverAsync);
            AboutAppCommand = new DelegateCommand(AboutAppCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void AboutAppCommandRecieverAsync()
        {
            try
            {
                await _navigationService.ClearPopupStackAsync("SettingView", null);
                try
                {
                    await _navigationService.NavigateAsync("AboutAppView", animated: false);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void RefreshSettingCommandRecieverAsync()
        {
            try
            {
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
                bool accept = await _dialogService.DisplayAlertAsync("Warning", "You have at least on draft item that will be deleted if you log out.", "Stay", "Log Out");
                if (!accept)
                {
                    try
                    {
                        AppSettings.RemoveUserData();
                    }
                    catch (Exception ex)
                    {
                         Crashes.TrackError(ex);
                    }
                    var param = new NavigationParameters
                    {
                        { "IsLogOut",true}
                    };
                    await _navigationService.ClearPopupStackAsync(animated: false);
                    await _navigationService.NavigateAsync("/NavigationPage/LoginView", param, animated: false);
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
                try
                {
                    await _navigationService.NavigateAsync("PrinterSettingView", animated: false);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
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
                await _navigationService.NavigateAsync("WhatIsNewView", animated: false);
                await _navigationService.ClearPopupStackAsync("SettingView", null, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SupportCommandRecieverAsync()
        {
            string mWebRoot = string.Empty;
            try
            {
                if (Configuration.ServiceUrl.Contains("https://api.kegid.com/api/"))
                    mWebRoot = "https://www.kegid.com";
                else if (Configuration.ServiceUrl.Contains("https://stageapi.kegid.com/api/"))
                    mWebRoot = "https://stage.kegid.com";
                else
                    mWebRoot = "https://test.kegid.com";

                await _navigationService.ClearPopupStackAsync(animated:false);
                //await Application.Current.MainPage.Navigation.PopPopupAsync();
                // You can remove the switch to UI Thread if you are already in the UI Thread.
                Device.BeginInvokeOnMainThread(() =>
                {
                    Device.OpenUri(new Uri(mWebRoot + "/Account/Login/ZendeskSingleSignOnMobile?sessionid="+AppSettings.SessionId));
                });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

#endregion
    }
}
