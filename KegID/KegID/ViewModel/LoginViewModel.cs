using KegID.Common;
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
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IAccountService _accountService;
        private readonly IMaintainService _maintainService;
        private readonly INavigationService _navigationService;
        private readonly IGetIconByPlatform _getIconByPlatform;

        #region Username

        /// <summary>
        /// The <see cref="Username" /> property's name.
        /// </summary>
        public const string UsernamePropertyName = "Username";

        private string _Username = default;

        /// <summary>
        /// Sets and gets the Username property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Username
        {
            get
            {
                return _Username;
            }

            set
            {
                if (_Username == value)
                {
                    return;
                }

                _Username = value;
                RaisePropertyChanged(UsernamePropertyName);
            }
        }

        #endregion

        #region Password

        /// <summary>
        /// The <see cref="Password" /> property's name.
        /// </summary>
        public const string PasswordPropertyName = "Password";

        private string _Password = default;

        /// <summary>
        /// Sets and gets the Password property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                if (_Password == value)
                {
                    return;
                }

                _Password = value;
                RaisePropertyChanged(PasswordPropertyName);
            }
        }

        #endregion

        #region BgImage

        /// <summary>
        /// The <see cref="BgImage" /> property's name.
        /// </summary>
        public const string BgImagePropertyName = "BgImage";

        private string _BgImage = "Assets/kegbg.png";

        /// <summary>
        /// Sets and gets the BgImage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BgImage
        {
            get
            {
                return _BgImage;
            }

            set
            {
                if (_BgImage == value)
                {
                    return;
                }

                _BgImage = value;
                RaisePropertyChanged(BgImagePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand LoginCommand { get; }
        public DelegateCommand KegIDCommand { get; }
        public bool IsLogOut { get; private set; }

        #endregion

        #region Constructor

        public LoginViewModel(IAccountService accountService, INavigationService navigationService, IMaintainService maintainService, IPageDialogService dialogService, IGetIconByPlatform getIconByPlatform)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _accountService = accountService;
            _maintainService = maintainService;
            _getIconByPlatform = getIconByPlatform;

            LoginCommand = new DelegateCommand(LoginCommandRecieverAsync);
            KegIDCommand = new DelegateCommand(KegIDCommandReciever);

            //Username = "test@kegid.com";//"demo@kegid.com";
            //Password = "beer2keg";

            BgImage = _getIconByPlatform.GetIcon("kegbg.png");
        }

        #endregion

        #region Methods

        private async Task GetLocation()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium,new TimeSpan(0,0,30));
                    ConstantManager.Location = await Geolocation.GetLocationAsync(request);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

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
            await GetLocation();

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
                        // If it is Android or iOS
                        if (AppSettings.IsFreshInstall)
                            await _navigationService.NavigateAsync(new Uri("/NavigationPage/WhatIsNewView", UriKind.Absolute), useModalNavigation: true, animated: false);
                        else
                            await _navigationService.NavigateAsync(new Uri("/NavigationPage/MainPage", UriKind.Absolute), useModalNavigation: true, animated: false);

                        // If it is UWP
                        //if (AppSettings.IsFreshInstall)
                        //    await _navigationService.NavigateAsync(new Uri("/NavigationPage/WhatIsNewView", UriKind.RelativeOrAbsolute), useModalNavigation: true, animated: false);
                        //else
                        //    await _navigationService.NavigateAsync(new Uri("/NavigationPage/MainPage", UriKind.RelativeOrAbsolute), useModalNavigation: true, animated: false);

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
                Loader.StopLoading();
                Analytics.TrackEvent("Loged In");
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("IsLogOut"))
            {
                IsLogOut = true;
                AppSettings.RemoveUserData();
                AppSettings.IsFreshInstall = false;
            }
            else
                IsLogOut = false;
        }

        #endregion
    }
}
