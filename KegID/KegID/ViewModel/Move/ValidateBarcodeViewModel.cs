using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using KegID.Model;
using KegID.Common;
using Rg.Plugins.Popup.Extensions;
using KegID.SQLiteClient;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace KegID.ViewModel
{
    public class ValidateBarcodeViewModel : BaseViewModel
    {
        #region Properties
        public List<Barcode> models { get; set; }

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
            switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
            {
                case ViewTypeEnum.ScanKegsView:
                    SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
                    SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().PartnerCount = 1;
                    break;
                case ViewTypeEnum.FillScanView:
                    SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
                    SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().PartnerCount = 1;
                    break;
                case ViewTypeEnum.MaintainScanView:
                    SimpleIoc.Default.GetInstance<MaintainScanViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
                    SimpleIoc.Default.GetInstance<MaintainScanViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().PartnerCount = 1;
                    break;
            }
            models.RemoveAt(0);
            if (models.Count == 0)
            {
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                {
                    case ViewTypeEnum.ScanKegsView:
                        await Application.Current.MainPage.Navigation.PopPopupAsync();
                        await Application.Current.MainPage.Navigation.PopModalAsync();
                        break;
                    case ViewTypeEnum.FillScanView:
                        SimpleIoc.Default.GetInstance<AddPalletsViewModel>().PalletCollection.Add(new PalletModel() { Barcode = SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection, Count = SimpleIoc.Default.GetInstance<FillScanViewModel>().BarcodeCollection.Count(), ManifestId = SimpleIoc.Default.GetInstance<FillScanViewModel>().ManifestId });

                        if (SimpleIoc.Default.GetInstance<AddPalletsViewModel>().PalletCollection.Sum(x => x.Count) > 1)
                            SimpleIoc.Default.GetInstance<AddPalletsViewModel>().Kegs = string.Format("({0} Kegs)", SimpleIoc.Default.GetInstance<AddPalletsViewModel>().PalletCollection.Sum(x => x.Count));
                        else
                            SimpleIoc.Default.GetInstance<AddPalletsViewModel>().Kegs = string.Format("({0} Keg)", SimpleIoc.Default.GetInstance<AddPalletsViewModel>().PalletCollection.Sum(x => x.Count));

                        await Application.Current.MainPage.Navigation.PopPopupAsync();
                        await Application.Current.MainPage.Navigation.PopModalAsync();
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

        public async Task LoadBarcodeValue(List<Barcode> _models)
        {
            models = _models;
            await ValidateScannedBarcode();
        }

        private async Task ValidateScannedBarcode()
        {
            string BarcodeId = default(string);
            try
            {
                BarcodeId = models.FirstOrDefault().Id;
                var value = await SQLiteServiceClient.Db.Table<BarcodeModel>().Where(x => x.Barcode == BarcodeId).FirstOrDefaultAsync();
                var validateBarcodeModel = JsonConvert.DeserializeObject<ValidateBarcodeModel>(value.BarcodeJson);
                PartnerCollection = validateBarcodeModel.Kegs.Partners;

                MultipleKegsTitle = string.Format(" Multiple kgs were found with \n barcode {0}. \n Please select the correct one.", models.FirstOrDefault().Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                BarcodeId = default(string);
            }
        }

        #endregion
    }
}
