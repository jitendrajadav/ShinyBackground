using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;

namespace KegID.ViewModel
{
    public class VolumeViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        public IList<string> VolumeCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<string> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public VolumeViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            ItemTappedCommand = new DelegateCommand<string>((model)=>ItemTappedCommandRecieverAsync(model));
            LoadAssetVolumeAsync();
        }

        #endregion

        #region Methods

        private void LoadAssetVolumeAsync()
        {
            try
            {
                VolumeCollection = new List<string>
                {
                    "bbl",
                    "hl",
                    "gal"
                };
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(string model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model))
                {
                    await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "VolumeModel", model }
                    }, animated: false);
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error", "Error: Please select volume.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ItemTappedCommandRecieverAsync"))
            {
                ItemTappedCommandRecieverAsync(default);
            }
        }

        #endregion
    }
}
