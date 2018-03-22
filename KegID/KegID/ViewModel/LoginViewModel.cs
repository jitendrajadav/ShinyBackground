using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Analytics;
using System;
using System.Diagnostics;
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

        public IAccountService _accountService { get; set; }

        #endregion

        #region Commands

        public RelayCommand LoginCommand { get; }
        public RelayCommand KegIDCommand { get; }

        #endregion

        #region Constructor
        public LoginViewModel(IAccountService accountService)
        {
            LoginCommand = new RelayCommand(LoginCommandRecieverAsync);
            KegIDCommand = new RelayCommand(KegIDCommandReciever);
            _accountService = accountService;

            //Username = "test@kegid.com";
            //Password = "beer2keg";
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
                if (model.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    AppSettings.User = model.LoginModel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
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
        #endregion
    }
}
