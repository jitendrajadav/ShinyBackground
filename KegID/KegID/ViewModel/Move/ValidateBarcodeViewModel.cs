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

        private readonly INavigationService _navigationService;
        public List<BarcodeModel> Models { get; set; }

        #region MultipleKegsTitle

        /// <summary>
        /// The <see cref="MultipleKegsTitle" /> property's name.
        /// </summary>
        public const string MultipleKegsTitlePropertyName = "MultipleKegsTitle";

        private string _MultipleKegsTitle = default(string);

        /// <summary>
        /// Sets and gets the MultipleKegsTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MultipleKegsTitle
        {
            get
            {
                return _MultipleKegsTitle;
            }

            set
            {
                if (_MultipleKegsTitle == value)
                {
                    return;
                }

                _MultipleKegsTitle = value;
                RaisePropertyChanged(MultipleKegsTitlePropertyName);
            }
        }

        #endregion

        #region PartnerCollection

        /// <summary>
        /// The <see cref="PartnerCollection" /> property's name.
        /// </summary>
        public const string PartnerCollectionPropertyName = "PartnerCollection";

        private IList<Partner> _PartnerCollection = null;

        /// <summary>
        /// Sets and gets the PartnerCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<Partner> PartnerCollection
        {
            get
            {
                return _PartnerCollection;
            }

            set
            {
                if (_PartnerCollection == value)
                {
                    return;
                }

                _PartnerCollection = value;
                RaisePropertyChanged(PartnerCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<Partner> ItemTappedCommand { get; }
        
        #endregion

        #region Constructor

        public ValidateBarcodeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            CancelCommand = new DelegateCommand(CancelCommandRecievierAsync);
            ItemTappedCommand = new DelegateCommand<Partner>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void CancelCommandRecievierAsync()
        {
            //await Application.Current.MainPage.Navigation.PopPopupAsync();
            await _navigationService.ClearPopupStackAsync(animated:false);
        }

        private void ItemTappedCommandRecieverAsync(Partner model)
        {
            try
            {
                //switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                //{
                //    case ViewTypeEnum.ScanKegsView:
                //        ValidToScanKegPagesMsg scanKegsMsg = new ValidToScanKegPagesMsg
                //        {
                //            Partner = model
                //        };
                //        MessagingCenter.Send(scanKegsMsg, "ValidToScanKegPagesMsg");
                //await SimpleIoc.Default.GetInstance<ScanKegsViewModel>().AssignValidatedValueAsync(model);
                //        break;

                //    case ViewTypeEnum.FillScanView:
                //        ValidToFillScanPagesMsg fillScanMsg = new ValidToFillScanPagesMsg
                //        {
                //            Partner = model
                //        };
                //        MessagingCenter.Send(fillScanMsg, "ValidToFillScanPagesMsg");
                //SimpleIoc.Default.GetInstance<FillScanViewModel>().AssignValidatedValueAsync(model);
                //        break;

                //    case ViewTypeEnum.MaintainScanView:
                //        ValidToMaintainPagesMsg maintainMsg = new ValidToMaintainPagesMsg
                //        {
                //            Partner = model
                //        };
                //        MessagingCenter.Send(maintainMsg, "ValidToMaintainPagesMsg");
                //SimpleIoc.Default.GetInstance<MaintainScanViewModel>().AssignValidatedValueAsync(model);
                //        break;
                //}

                try
                {
                    var param = new NavigationParameters
                    {
                        { "Partner", model }
                    };
                    var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
                    var page = formsNav.Navigation.ModalStack.Last();
                    (page?.BindingContext as INavigationAware)?.OnNavigatingTo(param);
                }
                catch (Exception)
                {
                    
                }
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

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                List<BarcodeModel> value = parameters.GetValue<List<BarcodeModel>>("model");
                LoadBarcodeValue(value);
            }
        }

        #endregion
    }
}
