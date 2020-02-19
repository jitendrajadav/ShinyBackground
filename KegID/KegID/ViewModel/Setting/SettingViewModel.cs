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
            await NavigationService.NavigateAsync("AboutAppView", animated: false);
            await NavigationService.ClearPopupStackAsync("SettingView", null);
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
                await NavigationService.ClearPopupStackAsync(animated: false);
                await NavigationService.NavigateAsync("/NavigationPage/LoginView", param, animated: false);
            }
        }

        private async void PrinterSettingCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("PrinterSettingView", animated: false);
            await NavigationService.ClearPopupStackAsync("SettingView", null);
        }

        private async void WhatsNewCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("WhatIsNewView", animated: false);
            await NavigationService.ClearPopupStackAsync("SettingView", null, animated: false);
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

            await NavigationService.ClearPopupStackAsync(animated: false);
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
