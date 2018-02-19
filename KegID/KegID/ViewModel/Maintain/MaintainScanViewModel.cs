using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainScanViewModel : ViewModelBase
    {
        #region Properties
        public IMoveService _moveService { get; set; }

        #region ManaulBarcode

        /// <summary>
        /// The <see cref="ManaulBarcode" /> property's name.
        /// </summary>
        public const string ManaulBarcodePropertyName = "ManaulBarcode";

        private string _ManaulBarcode = default(string);

        /// <summary>
        /// Sets and gets the ManaulBarcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManaulBarcode
        {
            get
            {
                return _ManaulBarcode;
            }

            set
            {
                if (_ManaulBarcode == value)
                {
                    return;
                }

                _ManaulBarcode = value;
                RaisePropertyChanged(ManaulBarcodePropertyName);
            }
        }

        #endregion

        #region BarcodeCollection

        /// <summary>
        /// The <see cref="BarcodeCollection" /> property's name.
        /// </summary>
        public const string BarcodeCollectionPropertyName = "BarcodeCollection";

        private ObservableCollection<Barcode> _BarcodeCollection = new ObservableCollection<Barcode>();

        /// <summary>
        /// Sets and gets the BarcodeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Barcode> BarcodeCollection
        {
            get
            {
                return _BarcodeCollection;
            }

            set
            {
                if (_BarcodeCollection == value)
                {
                    return;
                }

                _BarcodeCollection = value;
                RaisePropertyChanged(BarcodeCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand SubmitCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand BarcodeScanCommand { get; set; }
        public RelayCommand BarcodeManualCommand { get; set; }
        public RelayCommand<Barcode> IconItemTappedCommand { get; set; }
        public RelayCommand<Barcode> LabelItemTappedCommand { get; set; }

        #endregion

        #region Constructor
        public MaintainScanViewModel(IMoveService moveService)
        {
            SubmitCommand = new RelayCommand(SubmitCommandRecieverAsync);
            BackCommand = new RelayCommand(BackCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandReciever);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new RelayCommand<Barcode>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>((model) => IconItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods
        private async void LabelItemTappedCommandRecieverAsync(Barcode model)
        {

            if (model.PartnerCount > 1)
            {
                List<Barcode> modelList = new List<Barcode>
                    {
                        model
                    };
                await NavigateToValidatePartner(modelList);
            }
            else
            {
                //IsFromScanned = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
            }
        }

        private async void IconItemTappedCommandRecieverAsync(Barcode model)
        {
            if (model.PartnerCount > 1)
            {
                List<Barcode> modelList = new List<Barcode>
                    {
                        model
                    };
                await NavigateToValidatePartner(modelList);
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView());
                var value = await SQLiteServiceClient.Db.Table<ValidatePartnerModel>().Where(x => x.Barcode == model.Id).ToListAsync();

                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Barcode = string.Format(" Barcode {0} ", model.Id);
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Ownername = value.FirstOrDefault().FullName;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Size = value.FirstOrDefault().Size;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Contents = value.FirstOrDefault().Contents;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Batch = value.FirstOrDefault().Batch;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Location = value.FirstOrDefault().Location;
            }
        }

        private static async Task NavigateToValidatePartner(List<Barcode> model)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
            await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBardeValue(model);
        }

        private async void BarcodeManualCommandRecieverAsync()
        {
            await ValidateBarcodeInsertIntoLocalDB(ManaulBarcode);
            ManaulBarcode = string.Empty;
        }
        private async Task ValidateBarcodeInsertIntoLocalDB(string barcodeId)
        {
            ValidateBarcodeModel validateBarcodeModel = await _moveService.GetValidateBarcodeAsync(Configuration.SessionId, barcodeId);

            Barcode barcode = new Barcode
            {
                Id = barcodeId,
                PartnerCount = validateBarcodeModel.Kegs.Partners.Count,
                Icon = validateBarcodeModel.Kegs.Partners.Count > 1 ? GetIconByPlatform.GetIcon("validationerror.png") : GetIconByPlatform.GetIcon("validationquestion.png"),
            };

            BarcodeModel barcodeModel = new BarcodeModel()
            {
                Barcode = barcodeId,
                BarcodeJson = JsonConvert.SerializeObject(validateBarcodeModel)
            };
            try
            {
                // The item does not exists in the database so lets insert it
                await SQLiteServiceClient.Db.InsertAsync(barcodeModel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var isNew = BarcodeCollection.ToList().Any(x => x.Id == barcode.Id);
            if (!isNew)
                BarcodeCollection.Add(barcode);
        }

        private void BarcodeScanCommandReciever()
        {
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeScanCommandReciever();
        }

        private async void BackCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void SubmitCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();

                ManifestModel manifestPostModel = null;
                if (manifestPostModel != null)
                {

                    try
                    {
                        var result = await _moveService.PostManifestAsync(manifestPostModel, Configuration.SessionId, Configuration.NewManifest);

                        var manifest = await _moveService.GetManifestAsync(Configuration.SessionId, result.ManifestId);
                        if (manifest.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().TrackingNumber = manifest.TrackingNumber;

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ManifestTo = manifest.CreatorCompany.FullName + "\n" + manifest.CreatorCompany.PartnerTypeName;

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ShippingDate = manifest.ShipDate;
                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ItemCount = manifest.ManifestItems.Count;
                            SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = manifest.ManifestItems;

                            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().Contents = !string.IsNullOrEmpty(manifest.ManifestItems.FirstOrDefault().Contents) ? manifest.ManifestItems.FirstOrDefault().Contents : "No contens";

                            Loader.StopLoading();
                            await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
                        }
                        else
                        {
                            Loader.StopLoading();
                            SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Alert", "Something goes wrong please check again", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
            }

        }

        #endregion
    }
}
