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

        #region SelectedOwner

        /// <summary>
        /// The <see cref="SelectedOwner" /> property's name.
        /// </summary>
        public const string SelectedOwnerPropertyName = "SelectedOwner";

        private string _selectedOwner = null;

        /// <summary>
        /// Sets and gets the SelectedOwner property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string SelectedOwner
        {
            get
            {
                return _selectedOwner;
            }

            set
            {
                if (_selectedOwner == value)
                {
                    return;
                }

                _selectedOwner = value;
                RaisePropertyChanged(SelectedOwnerPropertyName);
            }
        }

        #endregion

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
