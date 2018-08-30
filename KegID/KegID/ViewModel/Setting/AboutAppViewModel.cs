using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class AboutAppViewModel : BaseViewModel
    {
        #region Propreties

        private readonly INavigationService _navigationService;

        #region Version

        /// <summary>
        /// The <see cref="Version" /> property's name.
        /// </summary>
        public const string VersionPropertyName = "Version";

        private string _Version = default(string);

        /// <summary>
        /// Sets and gets the Version property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Version
        {
            get
            {
                return _Version;
            }

            set
            {
                if (_Version == value)
                {
                    return;
                }

                _Version = value;
                RaisePropertyChanged(VersionPropertyName);
            }
        }

        #endregion

        #region Environment

        /// <summary>
        /// The <see cref="Environment" /> property's name.
        /// </summary>
        public const string EnvironmentPropertyName = "Environment";

        private string _Environment = default(string);

        /// <summary>
        /// Sets and gets the Environment property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Environment
        {
            get
            {
                return _Environment;
            }

            set
            {
                if (_Environment == value)
                {
                    return;
                }

                _Environment = value;
                RaisePropertyChanged(EnvironmentPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructor

        public AboutAppViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);

            Version = AppInfo.VersionString;
            Environment = Configuration.ServiceUrl;
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }
        }

        #endregion
    }
}
