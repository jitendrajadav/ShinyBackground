using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
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

        public IAccountService AccountService { get; set; }
        public IMaintainService MaintainService { get; set; }

        #endregion

        #region Commands

        public RelayCommand LoginCommand { get; }
        public RelayCommand KegIDCommand { get; }

        #endregion

        #region Constructor

        public LoginViewModel(IAccountService _accountService, IMaintainService _maintainService)
        {
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
                        AppSettings.User = new Model.UserInfoModel
                        {
                            SessionId = model.LoginModel.SessionId,
                            CompanyId = model.LoginModel.CompanyId,
                            MasterCompanyId = model.LoginModel.MasterCompanyId,
                            UserId = model.LoginModel.UserId,
                            SessionExpires = model.LoginModel.SessionExpires
                        };
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                    try
                    {
                        var value = await SQLiteServiceClient.Db.InsertAllAsync(model.LoginModel.Preferences);
                        var maintenance = await MaintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);
                        await SQLiteServiceClient.Db.InsertAllAsync(maintenance.MaintainTypeReponseModel);
                        await LoadAssetSizeAsync();
                        await LoadAssetTypeAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                  await Application.Current.MainPage.DisplayAlert("Error","Error while login please check","Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
                Analytics.TrackEvent("Loged In");
            }
        }

        internal async void InvalideServiceCallAsync()
        {
            AppSettings.RemoveUserData();
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoginView());
        }

        private async Task LoadAssetSizeAsync()
        {
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
                await SQLiteServiceClient.Db.InsertAllAsync(assetSizeModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                assetSizeModel = null;
                service = null;
            }
        }

        private async Task LoadAssetTypeAsync()
        {
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
                await SQLiteServiceClient.Db.InsertAllAsync(assetTypeModels);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                assetTypeModels = null;
                service = null;
            }
        }

        #endregion
    }
}
