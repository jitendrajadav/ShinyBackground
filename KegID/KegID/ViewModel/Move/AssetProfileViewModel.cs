using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace KegID.ViewModel
{
    public class AssetProfileViewModel : BaseViewModel
    {
        #region Properties

        public string SelectedOwner { get; set; }

        #endregion

        #region Commands

        public DelegateCommand ApplyToAllCommand { get; }
        public DelegateCommand DoneCommand { get; }

        #endregion

        #region Constructor

        public AssetProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void DoneCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            { "AssignSizesValue", ConstantManager.VerifiedBarcodes }
                        }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        #endregion

    }
}
