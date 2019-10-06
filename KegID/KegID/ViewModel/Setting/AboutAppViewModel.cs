using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class AboutAppViewModel : BaseViewModel
    {
        #region Propreties

        public string Version { get; set; }
        public string Environment { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructor

        public AboutAppViewModel(INavigationService navigationService) : base(navigationService)
        {
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            Version = AppInfo.VersionString;
            Environment = Configuration.ServiceUrl;
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
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
