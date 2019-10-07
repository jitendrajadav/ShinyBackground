using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class ValidateBarcodeViewModel : BaseViewModel
    {
        #region Properties

        public List<BarcodeModel> Models { get; set; }
        public string MultipleKegsTitle { get; set; }
        public IList<Partner> PartnerCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<Partner> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public ValidateBarcodeViewModel(INavigationService navigationService) : base(navigationService)
        {
            CancelCommand = new DelegateCommand(CancelCommandRecievierAsync);
            ItemTappedCommand = new DelegateCommand<Partner>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void CancelCommandRecievierAsync()
        {
            await _navigationService.ClearPopupStackAsync(animated:false);
        }

        private void ItemTappedCommandRecieverAsync(Partner model)
        {
            try
            {
                try
                {
                    var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
                    var page = formsNav.Navigation.NavigationStack.Last();
                    (page?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "Partner", model }
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                if (Models.Count > 0)
                    Models.RemoveAt(0);

                if (Models.Count > 0)
                    ValidateScannedBarcode();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void LoadBarcodeValue(List<BarcodeModel> _models)
        {
            try
            {
                Models = _models;
                ValidateScannedBarcode();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void ValidateScannedBarcode()
        {
            try
            {
                PartnerCollection = Models?.FirstOrDefault()?.Kegs?.Partners;
                MultipleKegsTitle = string.Format(" Multiple kgs were found with \n barcode {0}. \n Please select the correct one.", Models.FirstOrDefault().Barcode);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                List<BarcodeModel> value = parameters.GetValue<List<BarcodeModel>>("model");
                LoadBarcodeValue(value);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecievierAsync"))
            {
                CancelCommandRecievierAsync();
            }
        }

        #endregion
    }
}
