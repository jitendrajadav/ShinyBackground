using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using KegID.Model;
using Rg.Plugins.Popup.Extensions;
using KegID.SQLiteClient;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Microsoft.AppCenter.Crashes;
using Realms;

namespace KegID.ViewModel
{
    public class ValidateBarcodeViewModel : BaseViewModel
    {
        #region Properties

        public List<Barcode> Models { get; set; }

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

        public RelayCommand CancelCommand { get; }
        public RelayCommand<Partner> ItemTappedCommand { get; }
        
        #endregion

        #region Constructor

        public ValidateBarcodeViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecievierAsync);
            ItemTappedCommand = new RelayCommand<Partner>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void CancelCommandRecievierAsync() => await Application.Current.MainPage.Navigation.PopPopupAsync();

        private async void ItemTappedCommandRecieverAsync(Partner model)
        {
            try
            {
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                {
                    case ViewTypeEnum.ScanKegsView:
                        SimpleIoc.Default.GetInstance<ScanKegsViewModel>().AssignValidatedValueAsync(model);
                        break;

                    case ViewTypeEnum.FillScanView:
                        SimpleIoc.Default.GetInstance<FillScanViewModel>().AssignValidatedValue(model);
                        break;

                    case ViewTypeEnum.MaintainScanView:
                        SimpleIoc.Default.GetInstance<MaintainScanViewModel>().AssignValidatedValue(model);
                        break;
                }

                Models.RemoveAt(0);

                if (Models.Count == 0)
                {
                    switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                    {
                        //case ViewTypeEnum.ScanKegsView:
                        //    await Application.Current.MainPage.Navigation.PopPopupAsync();
                        //    await Application.Current.MainPage.Navigation.PopModalAsync();
                        //    break;

                        case ViewTypeEnum.FillScanView:
                            SimpleIoc.Default.GetInstance<FillScanViewModel>().AssignValidateBarcodeValueAsync();
                            break;

                        case ViewTypeEnum.MaintainScanView:
                            await Application.Current.MainPage.Navigation.PopPopupAsync();
                            SimpleIoc.Default.GetInstance<MaintainScanViewModel>().SubmitCommandRecieverAsync();
                            break;
                    }
                }
                else
                    await ValidateScannedBarcode();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task LoadBarcodeValue(List<Barcode> _models)
        {
            try
            {
                Models = _models;
                await ValidateScannedBarcode();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task ValidateScannedBarcode()
        {
            var vRealmDb = Realm.GetInstance();
            string BarcodeId = default(string);
            try
            {
                BarcodeId = Models.FirstOrDefault().Id;
                var value = vRealmDb.All<BarcodeModel>().Where(x => x.Barcode == BarcodeId).FirstOrDefault();
                    //await SQLiteServiceClient.Db.Table<BarcodeModel>().Where(x => x.Barcode == BarcodeId).FirstOrDefaultAsync();
                var validateBarcodeModel = JsonConvert.DeserializeObject<ValidateBarcodeModel>(value.BarcodeJson);
                PartnerCollection = validateBarcodeModel.Kegs.Partners;

                MultipleKegsTitle = string.Format(" Multiple kgs were found with \n barcode {0}. \n Please select the correct one.", Models.FirstOrDefault().Id);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                BarcodeId = default(string);
            }
        }

        #endregion
    }
}
