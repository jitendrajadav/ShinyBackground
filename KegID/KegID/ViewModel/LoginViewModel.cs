using Acr.UserDialogs;
using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IGetIconByPlatform _getIconByPlatform;

        public string Username { get; set; }
        public string Password { get; set; }
        public string BgImage { get; set; }
        public string APIBase { get; set; }
        public double KegRefresh { get; set; }

        #endregion

        #region Commands

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand KegIDCommand { get; }
        public bool IsLogOut { get; private set; }

        #endregion

        #region Constructor

        public LoginViewModel(INavigationService navigationService, IPageDialogService dialogService, IGetIconByPlatform getIconByPlatform) : base(navigationService)
        {
            _dialogService = dialogService;
            _getIconByPlatform = getIconByPlatform;

            LoginCommand = new DelegateCommand(async () => await RunSafe(LoginCommandRecieverAsync()));
            KegIDCommand = new DelegateCommand(KegIDCommandReciever);
#if DEBUG
            Username = "test@kegid.com"; //"demo@kegid.com";//
            Password = "beer2keg";
#endif
            BgImage = _getIconByPlatform.GetIcon("kegbg.png");
            APIBase = ConstantManager.BaseUrl.Contains("Prod") ? string.Empty : ConstantManager.BaseUrl;
        }

        #endregion

        #region Methods


        private void KegIDCommandReciever()
        {
            // You can remove the switch to UI Thread if you are already in the UI Thread.
            Launcher.OpenAsync(new Uri("https://www.kegid.com/"));
        }

        private async Task LoginCommandRecieverAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loging");
                var loginResponse = await ApiManager.GetAuthenticate(Username, Password);
                if (loginResponse.IsSuccessStatusCode)
                {
                    var response = await loginResponse.Content.ReadAsStringAsync();
                    var model = await Task.Run(() => JsonConvert.DeserializeObject<LoginModel>(response, GetJsonSetting()));
                    try
                    {
                        var overDues = model.Preferences.Where(x => x.PreferenceName == "OVERDUE_DAYS").Select(x => x.PreferenceValue).FirstOrDefault();
                        var atRisk = model.Preferences.Where(x => x.PreferenceName == "AT_RISK_DAYS").Select(x => x.PreferenceValue).FirstOrDefault();
                        var appDataWebServiceUrl = model.Preferences.Where(x => x.PreferenceName == "AppDataWebServiceUrl").Select(x => x.PreferenceValue).FirstOrDefault();
                        if (appDataWebServiceUrl != null)
                        {
                            ConstantManager.BaseUrl = ConstantManager.BaseUrl;// appDataWebServiceUrl;
                        }
                        AppSettings.SessionId = model.SessionId;
                        AppSettings.CompanyId = model.CompanyId;
                        AppSettings.MasterCompanyId = model.MasterCompanyId;
                        AppSettings.UserId = model.UserId;
                        AppSettings.SessionExpires = model.SessionExpires;
                        AppSettings.Overdue_days = !string.IsNullOrEmpty(overDues) ? long.Parse(overDues) : 0;
                        AppSettings.At_risk_days = !string.IsNullOrEmpty(atRisk) ? long.Parse(atRisk) : 0;

                        KegRefresh = Convert.ToDouble(model.Preferences.ToList().Find(x => x.PreferenceName == "KEG_REFRESH")?.PreferenceValue);
                        await RunSafe(DeviceCheckIn());
                        UserDialogs.Instance.HideLoading();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                        Crashes.TrackError(ex);
                    }
                    try
                    {
                        var versionUpdated = VersionTracking.CurrentVersion.CompareTo(VersionTracking.PreviousVersion);
                        if (versionUpdated > 0 && VersionTracking.PreviousVersion != null && VersionTracking.IsFirstLaunchForCurrentVersion)
                        {
                            await _navigationService.NavigateAsync("../WhatIsNewView", animated: false);
                        }
                        else
                        {
                            await _navigationService.NavigateAsync("../MainPage", animated: false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        UserDialogs.Instance.HideLoading();
                    }
                    try
                    {
                        if (!IsLogOut)
                        {
                            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                            await RealmDb.WriteAsync((realmDb) => realmDb.Add(model));
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error", "Error while login please check", "Ok");
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                Debug.WriteLine(ex.Message);
                await _dialogService.DisplayAlertAsync("Error", AsyncErrorHandler.Message + "\n\nError while login please check", "Ok");
            }
            finally
            {
                Analytics.TrackEvent("Loged In");
            }
        }

        private async Task DeviceCheckIn()
        {
            Device.StartTimer(TimeSpan.FromDays(1), () =>
            {
                Func<Task> value = async () => await RunSafe(DeviceCheckIn());
                return true; // True = Repeat again, False = Stop the timer
            });

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                DeviceCheckinRequestModel deviceModel = new DeviceCheckinRequestModel
                {
                    AppVersion = AppInfo.VersionString,
                    DeviceId = DeviceInfo.Manufacturer,
                    DeviceModel = DeviceInfo.Model,
                    OS = DeviceInfo.VersionString,
                    PushToken = "",
                    UserId = AppSettings.UserId
                };
                var deviceCheckInResponse = await ApiManager.PostDeviceCheckin(deviceModel, AppSettings.SessionId);
                if (!deviceCheckInResponse.IsSuccessStatusCode)
                {
                    var param = new NavigationParameters
                    {
                        { "IsLogOut",true}
                    };
                    await _navigationService.NavigateAsync("/NavigationPage/LoginView", param);
                }

                Device.StartTimer(TimeSpan.FromMilliseconds(KegRefresh), () =>
                {
                    var model = new KegRequestModel
                    {
                        KegId = string.Empty,
                        Barcode = string.Empty,
                        OwnerId = ConstantManager.Partner?.PartnerId,
                        AltBarcode = string.Empty,
                        Notes = "",
                        ReferenceKey = "",
                        ProfileId = "",
                        AssetType = string.Empty,
                        AssetSize = string.Empty,
                        AssetVolume = "",
                        AssetDescription = "",
                        OwnerSkuId = "",
                        FixedContents = "",
                        Tags = new List<Tag>(),
                        MaintenanceAlertIds = new List<string>(),
                        LessorId = "",
                        PurchaseDate = DateTimeOffset.Now,
                        PurchasePrice = 0,
                        PurchaseOrder = "",
                        ManufacturerName = "",
                        ManufacturerId = "",
                        ManufactureLocation = "",
                        ManufactureDate = DateTimeOffset.Now,
                        Material = "",
                        Markings = "",
                        Colors = ""
                    };

                    Task.Factory.StartNew(async () =>
                    {
                        var value = await ApiManager.PostKeg(model, string.Empty, AppSettings.SessionId);
                    });
                    return false; // True = Repeat again, False = Stop the timer
                });
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("IsLogOut"))
            {
                IsLogOut = true;
                AppSettings.RemoveUserData();
            }
            else
            { IsLogOut = false; }

            return base.InitializeAsync(parameters);

            #endregion
        }

    }
}
