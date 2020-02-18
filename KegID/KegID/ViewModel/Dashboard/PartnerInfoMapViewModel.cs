using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class PartnerInfoMapViewModel : BaseViewModel
    {
        #region Properties

        #endregion

        #region Commands

        public DelegateCommand PartnerInfoCommand { get; }

        #endregion

        #region Constructor

        public PartnerInfoMapViewModel(INavigationService navigationService) : base(navigationService)
        {
            PartnerInfoCommand = new DelegateCommand(PartnerInfoCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void PartnerInfoCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PartnerInfoCommandRecieverAsync"))
            {
                PartnerInfoCommandRecieverAsync();
            }
        }

        #endregion
    }
}
