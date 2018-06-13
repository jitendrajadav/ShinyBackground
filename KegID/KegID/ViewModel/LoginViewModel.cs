//using Fusillade;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Dtos;
using KegID.Model;
using KegID.Services;
//using KegID.SQLiteClient;
using KegID.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Realms;
//using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    //[ImplementPropertyChanged]
    public class LoginViewModel : BaseViewModel
    {

        #region Properties

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

        //public readonly ILoginService _loginService;
        public readonly IAccountService AccountService;
        public readonly IMaintainService MaintainService;

        #endregion

        #region Commands

        public RelayCommand LoginCommand { get; }
        public RelayCommand KegIDCommand { get; }

        #endregion

        #region Constructor

        public LoginViewModel(IAccountService _accountService, IMaintainService _maintainService)
        {
            //_loginService = loginService;
            AccountService = _accountService;
            MaintainService = _maintainService;
            LoginCommand = new RelayCommand(LoginCommandRecieverAsync);
            KegIDCommand = new RelayCommand(KegIDCommandReciever);

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

        //private async void LoginCommand1RecieverAsync()
        //{
        //    try
        //    {
        //        Loader.StartLoading();
        //        var login =  await _loginService
        //                                    .GetLogin(Priority.Background,Username, Password)
        //                                    .ConfigureAwait(false);
        //        //CacheLogin(login);

        //        Application.Current.MainPage = new MainPage();
        //        await Application.Current.MainPage.Navigation.PopToRootAsync(true);
        //    }
        //    catch (Exception ex)
        //    {
        //         Crashes.TrackError(ex);
        //    }
        //    finally
        //    {
        //        Loader.StopLoading();
        //        Analytics.TrackEvent("Loged In");
        //    }
        //}

        private void CacheLogin(LoginDto login)
        {

        }

        private async void LoginCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();
                var model = await AccountService.AuthenticateAsync(Username, Password);
                if (model.StatusCode == System.Net.HttpStatusCode.OK)
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

                    await Application.Current.MainPage.Navigation.PushModalAsync(page: new MainPage(), animated: false);
                    try
                    {
                        var RealmDb = Realm.GetInstance();

                        //Befor RealmDB..
                        //var value = await SQLiteServiceClient.Db.InsertAllAsync(model.LoginModel.Preferences);
                        //var maintenance = await MaintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);
                        //await SQLiteServiceClient.Db.InsertAllAsync(maintenance.MaintainTypeReponseModel);
                        //await LoadAssetSizeAsync();
                        //await LoadAssetTypeAsync();
                        //await LoadAssetVolumeAsync();
                        //await LoadOwnerAsync();
                        await RealmDb.WriteAsync((realmDb) => 
                        {
                            realmDb.Add(model.LoginModel);
                        });
                        var vAllEmployees = RealmDb.All<LoginModel>();

                        var maintenance = await MaintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);
                        RealmDb.Refresh();
                        await RealmDb.WriteAsync((realmDb) =>
                        {
                            foreach (var item in maintenance.MaintainTypeReponseModel)
                            {
                                realmDb.Add(item);
                            }
                        });

                        await LoadAssetSizeAsync();
                        await LoadAssetTypeAsync();
                        await LoadAssetVolumeAsync();
                        await LoadOwnerAsync();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                else
                {
                    Loader.StopLoading();
                    await Application.Current.MainPage.DisplayAlert("Error", "Error while login please check", "Ok");
                }
                Application.Current.MainPage = new MainPage();
                await Application.Current.MainPage.Navigation.PopToRootAsync(true);
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

        private async Task LoadAssetSizeAsync()
        {
            var RealmDb = Realm.GetInstance();
            List<AssetSizeModel> assetSizeModel = null;
            var service = SimpleIoc.Default.GetInstance<IMoveService>();
            try
            {
                var model = await service.GetAssetSizeAsync(AppSettings.User.SessionId, false);
                assetSizeModel = new List<AssetSizeModel>();
                foreach (var item in model)
                {
                    assetSizeModel.Add(new AssetSizeModel { AssetSize = item });
                }
                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in assetSizeModel)
                    {
                        realmDb.Add(item);
                    }
                });
                RealmDb.Refresh();
                var value = RealmDb.All<AssetSizeModel>().ToList();
                //await SQLiteServiceClient.Db.InsertAllAsync(assetSizeModel);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                //assetSizeModel = null;
                service = null;
            }
        }

        private async Task LoadAssetTypeAsync()
        {
            var RealmDb = Realm.GetInstance();
            List<AssetTypeModel> assetTypeModels = null;
            var service = SimpleIoc.Default.GetInstance<IMoveService>();
            try
            {
                var model = await service.GetAssetTypeAsync(AppSettings.User.SessionId, false);
                assetTypeModels = new List<AssetTypeModel>();
                foreach (var item in model)
                {
                    assetTypeModels.Add(new AssetTypeModel { AssetType = item });
                }

                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in assetTypeModels)
                    {
                        realmDb.Add(item);
                    }
                });
                //await SQLiteServiceClient.Db.InsertAllAsync(assetTypeModels);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                //assetTypeModels = null;
                service = null;
            }
        }

        private async Task LoadAssetVolumeAsync()
        {
            var RealmDb = Realm.GetInstance();
            List<AssetVolumeModel> assetVolumeModel = null;
            var service = SimpleIoc.Default.GetInstance<IDashboardService>();
            try
            {
                var model = await service.GetAssetVolumeAsync(AppSettings.User.SessionId, false);

                assetVolumeModel = new List<AssetVolumeModel>();
                foreach (var item in model)
                {
                    assetVolumeModel.Add(new AssetVolumeModel { AssetVolume = item });
                }
                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in assetVolumeModel)
                    {
                        realmDb.Add(item);
                    }
                });
                //await SQLiteServiceClient.Db.InsertAllAsync(assetVolumeModel);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                //assetVolumeModel = null;
                service = null;
            }
        }

        private async Task LoadOwnerAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance();
                var value = await SimpleIoc.Default.GetInstance<IMoveService>().GetOwnerAsync(AppSettings.User.SessionId);
                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in value.OwnerModel)
                    {
                        realmDb.Add(item);
                    }
                });
                //await SQLiteServiceClient.Db.InsertAllAsync(items: (await SimpleIoc.Default.GetInstance<IMoveService>().GetOwnerAsync(AppSettings.User.SessionId)).OwnerModel);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
            }
        }

        #endregion
    }
}
