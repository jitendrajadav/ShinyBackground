﻿using KegID.Common;
using KegID.LocalDb;
using KegID.Services;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IAccountService _accountService;
        private readonly IGetIconByPlatform _getIconByPlatform;

        public string Username { get; set; }
        public string Password { get; set; }
        public string BgImage { get; set; }
        public string APIBase { get; set; }

        #endregion

        #region Commands

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand KegIDCommand { get; }
        public bool IsLogOut { get; private set; }

        #endregion

        #region Constructor

        public LoginViewModel(IAccountService accountService, INavigationService navigationService, IPageDialogService dialogService, IGetIconByPlatform getIconByPlatform) : base(navigationService)
        {
            _dialogService = dialogService;
            _accountService = accountService;
            _getIconByPlatform = getIconByPlatform;

            LoginCommand = new DelegateCommand(LoginCommandRecieverAsync);
            KegIDCommand = new DelegateCommand(KegIDCommandReciever);
#if DEBUG
            Username = "test@kegid.com";//"demo@kegid.com";
            Password = "beer2keg";
#endif
            BgImage = _getIconByPlatform.GetIcon("kegbg.png");
            APIBase = AppSettings.BaseURL.Contains("Prod") ? string.Empty : AppSettings.BaseURL;
        }

        #endregion

        #region Methods


        private void KegIDCommandReciever()
        {
            // You can remove the switch to UI Thread if you are already in the UI Thread.
            Device.BeginInvokeOnMainThread(() =>
            {
                Device.OpenUri(new Uri("https://www.kegid.com/"));
            });
        }

        private async void LoginCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();
                var model = await _accountService.AuthenticateAsync(Username, Password);
                if (model.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    try
                    {
                        var overDues = model.LoginModel.Preferences.Where(x => x.PreferenceName == "OVERDUE_DAYS").Select(x => x.PreferenceValue).FirstOrDefault();
                        var atRisk = model.LoginModel.Preferences.Where(x => x.PreferenceName == "AT_RISK_DAYS").Select(x => x.PreferenceValue).FirstOrDefault();
                        var appDataWebServiceUrl = model.LoginModel.Preferences.Where(x => x.PreferenceName == "AppDataWebServiceUrl").Select(x => x.PreferenceValue).FirstOrDefault();
                        if (appDataWebServiceUrl != null)
                        {
                            AppSettings.BaseURL = Configuration.DemoAPIUrl;
                        }

                        AppSettings.SessionId = model.LoginModel.SessionId;
                        AppSettings.CompanyId = model.LoginModel.CompanyId;
                        AppSettings.MasterCompanyId = model.LoginModel.MasterCompanyId;
                        AppSettings.UserId = model.LoginModel.UserId;
                        AppSettings.SessionExpires = model.LoginModel.SessionExpires;
                        AppSettings.Overdue_days = !string.IsNullOrEmpty(overDues) ? Convert.ToInt64(overDues) : 0;
                        AppSettings.At_risk_days = !string.IsNullOrEmpty(atRisk) ? Convert.ToInt64(atRisk) : 0;
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    try
                    {
                        string v = Xamarin.Essentials.AppInfo.Version.ToString();
                        if (AppSettings.IsFreshInstall == false && AppSettings.WhatsNewVersion != v)
                        {
                            await _navigationService.NavigateAsync("../WhatIsNewView", animated: false);
                            Loader.StopLoading();
                        }
                        else
                            await _navigationService.NavigateAsync("../MainPage", animated: false);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    try
                    {
                        if (!IsLogOut)
                        {
                            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                            await RealmDb.WriteAsync((realmDb) =>
                            {
                                realmDb.Add(model.LoginModel);
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                else
                {
                    Loader.StopLoading();
                    await _dialogService.DisplayAlertAsync("Error", "Error while login please check", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Analytics.TrackEvent("Loged In");
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("IsLogOut"))
            {
                IsLogOut = true;
                AppSettings.RemoveUserData();
                AppSettings.IsFreshInstall = false;
                AppSettings.WhatsNewVersion = null;
            }
            else
                IsLogOut = false;

            return base.InitializeAsync(parameters);
        }

        #endregion
    }
}
