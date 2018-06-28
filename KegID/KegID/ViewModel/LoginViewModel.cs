using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using System;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {

        #region Properties

        public static IAccountService _accountService { get; set; }
        private static IMoveService _moveService { get; set; }
        private static IMaintainService _maintainService { get; set; }
        private static IDashboardService _dashboardService { get; set; }
        private static IFillService _fillService { get; set; }
        INavigationService _navigationService;

        #region Username

        /// <summary>
        /// The <see cref="Username" /> property's name.
        /// </summary>
        public const string UsernamePropertyName = "Username";

        private string _Username = default(string);

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

        private string _Password = default(string);

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

        #endregion

        #region Constructor

        public LoginViewModel(IAccountService accountService, INavigationService navigationService, IMoveService moveService, IMaintainService maintainService, IDashboardService dashboardService, IFillService fillService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _accountService = accountService;
            _moveService = moveService;
            _maintainService = maintainService;
            _dashboardService = dashboardService;
            _fillService = fillService;
            LoginCommand = new DelegateCommand(LoginCommandRecieverAsync);
            KegIDCommand = new DelegateCommand(KegIDCommandReciever);

            Username = "test@kegid.com";
            Password = "beer2keg";
            BgImage = GetIconByPlatform.GetIcon("kegbg.png");
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
                        AppSettings.User = new UserInfoModel
                        {
                            SessionId = model.LoginModel.SessionId,
                            CompanyId = model.LoginModel.CompanyId,
                            MasterCompanyId = model.LoginModel.MasterCompanyId,
                            UserId = model.LoginModel.UserId,
                            SessionExpires = model.LoginModel.SessionExpires,
                            Overdue_days = !string.IsNullOrEmpty(overDues) ? Convert.ToInt64(overDues) : 0,
                            At_risk_days = !string.IsNullOrEmpty(atRisk) ? Convert.ToInt64(atRisk) : 0
                        };
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    try
                    {
                        var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
                        var value1=  formsNav.Navigation.ModalStack.LastOrDefault();
                        var value2 = formsNav.Navigation.NavigationStack.LastOrDefault();

                        await _navigationService.NavigateAsync(new Uri("../MainPage", UriKind.Relative),useModalNavigation:true, animated: false);

                        var formsNav1 = ((Prism.Common.IPageAware)_navigationService).Page;
                        var value3 = formsNav1.Navigation.ModalStack.LastOrDefault();
                        var value4 = formsNav1.Navigation.NavigationStack.LastOrDefault();

                    }
                    catch (Exception ex)
                    {
                    }                    
                    //await Application.Current.MainPage.Navigation.PushModalAsync(page: new MainPage(), animated: false);
                    try
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                        RealmDb.Write(() =>
                        {
                            RealmDb.Add(model.LoginModel);
                        });
                        var vAllEmployees = RealmDb.All<LoginModel>();

                        var maintenance = await _maintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);

                        RealmDb.Write(() =>
                        {
                           foreach (var item in maintenance.MaintainTypeReponseModel)
                           {
                               RealmDb.Add(item);
                           }
                        });

                        await InitializeMetaData.LoadInitializeMetaData(_moveService,_dashboardService,_maintainService, _fillService);
                        //SimpleIoc.Default.GetInstance<DashboardViewModel>().RefreshDashboardRecieverAsync(true);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    //Application.Current.MainPage = new MainPage();
                }
                else
                {
                    Loader.StopLoading();
                    await Application.Current.MainPage.DisplayAlert("Error", "Error while login please check", "Ok");
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

        internal async void InvalideServiceCallAsync()
        {
            try
            {
                AppSettings.RemoveUserData();
                await Application.Current.MainPage.Navigation.PushModalAsync(new LoginView(), animated: false);
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
