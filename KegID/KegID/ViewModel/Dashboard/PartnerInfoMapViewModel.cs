using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace KegID.ViewModel
{
    public class PartnerInfoMapViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        #endregion

        #region Commands

        public DelegateCommand PartnerInfoCommand { get; }

        #endregion

        #region Constructor

        public PartnerInfoMapViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

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
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
