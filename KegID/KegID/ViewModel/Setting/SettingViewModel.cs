using System;
using KegID.Common;
using KegID.Messages;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
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
            await _navigationService.NavigateAsync("AboutAppView", animated: false);
            await _navigationService.ClearPopupStackAsync("SettingView", null);
        }

        private void RefreshSettingCommandRecieverAsync()
        {
            SettingToDashboardMsg msg = new SettingToDashboardMsg
            {
                IsRefresh = true
            };
            MessagingCenter.Send(msg, "SettingToDashboardMsg");
        }

        private async void LogOutSettingCommandRecieverAsync()
        {
            bool accept = await _dialogService.DisplayAlertAsync("Warning", "You have at least on draft item that will be deleted if you log out.", "Stay", "Log Out");
            if (!accept)
            {

                Settings.RemoveUserData();

                var param = new NavigationParameters
                    {
                        { "IsLogOut",true}
                    };
                await _navigationService.ClearPopupStackAsync(animated: false);
                await _navigationService.NavigateAsync("/NavigationPage/LoginView", param, animated: false);
            }
        }

        private async void PrinterSettingCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("PrinterSettingView", animated: false);
            await _navigationService.ClearPopupStackAsync("SettingView", null);
        }

        private async void WhatsNewCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("WhatIsNewView", animated: false);
            await _navigationService.ClearPopupStackAsync("SettingView", null, animated: false);
        }

        private async void SupportCommandRecieverAsync()
        {
            string mWebRoot = string.Empty;

            if (Preferences.Get("BaseURL", "BaseURL").Contains("https://api.kegid.com/api/"))
                mWebRoot = "https://www.kegid.com";
            else if (Preferences.Get("BaseURL", "BaseURL").Contains("https://stageapi.kegid.com/api/"))
                mWebRoot = "https://stage.kegid.com";
            else
                mWebRoot = "https://test.kegid.com";

            await _navigationService.ClearPopupStackAsync(animated: false);
            //await Application.Current.MainPage.Navigation.PopPopupAsync();
            // You can remove the switch to UI Thread if you are already in the UI Thread.
            Device.BeginInvokeOnMainThread(() =>
            {
                Launcher.OpenAsync(new Uri(mWebRoot + "/Account/Login/ZendeskSingleSignOnMobile?sessionid=" + Settings.SessionId));
            });
        }

        #endregion
    }
}
