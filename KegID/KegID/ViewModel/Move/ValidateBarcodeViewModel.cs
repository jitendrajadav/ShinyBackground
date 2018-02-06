﻿using GalaSoft.MvvmLight;
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
    public class ValidateBarcodeViewModel : ViewModelBase
    {
        #region Properties

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

        public List<Barcode> models { get; set; }
        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }

        public RelayCommand<Partner> ItemTappedCommand { get; set; }
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
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().PartnerCount = 1;

            models.RemoveAt(0);
            if (models.Count == 0)
                await Application.Current.MainPage.Navigation.PopPopupAsync();
            else
                await ValidateScannedBarcode();
        }

        public async void LoadBardeValue(List<Barcode> _models)
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
