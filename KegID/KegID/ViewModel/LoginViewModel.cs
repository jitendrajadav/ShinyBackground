using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using KegID.Response;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.View;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class LoginViewModel : ViewModelBase
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

        public IAccountService _accountService { get; set; }

        #endregion

        #region Commands

        public RelayCommand LoginCommand { get; set; }

        #endregion

        #region Constructor
        public LoginViewModel(IAccountService accountService)
        {
            LoginCommand = new RelayCommand(LoginCommandRecieverAsync);
            _accountService = accountService;

            Username = "test@kegid.com";
            Password = "beer2keg";

            LoginCommandReciever();
        }

        private async void LoginCommandReciever()
        {
            GlobalModel globalData = null;
            try
            {
                Loader.StartLoading();
                globalData = await SQLiteServiceClient.Db.Table<GlobalModel>().FirstOrDefaultAsync();

                if (globalData == null)
                    LoginCommandRecieverAsync();
                else
                {
                    Configuration.SessionId = globalData.SessionId;
                    await Application.Current.MainPage.Navigation.PushModalAsync(new KegIDMasterPage());
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
                globalData = null;
            }
        }
        #endregion

        #region Methods
        private async void LoginCommandRecieverAsync()
        {
            LoginModel Result = null;
            try
            {
                Loader.StartLoading();
                var globalData = await SQLiteServiceClient.Db.Table<GlobalModel>().FirstOrDefaultAsync();
                if (globalData == null)
                {
                    Result = await _accountService.AuthenticateAsync(Username, Password);
                    await SQLiteServiceClient.Db.InsertAsync(new GlobalModel { SessionId = Result.SessionId });
                    Configuration.SessionId = Result.SessionId;
                }
                await Application.Current.MainPage.Navigation.PushModalAsync(new KegIDMasterPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
                Result = null;
            }
        }
        #endregion
    }
}
