using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;

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
            try
            {
                PartnerInfoCommand = new DelegateCommand(PartnerInfoCommandRecieverAsync);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }            
        }

        #endregion

        #region Methods

        private async void PartnerInfoCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
