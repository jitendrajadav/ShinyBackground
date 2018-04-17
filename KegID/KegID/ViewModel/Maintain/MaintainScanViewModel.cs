﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainScanViewModel : BaseViewModel
    {
        #region Properties

        public IMoveService _moveService { get; set; }
        public IMaintainService _maintainService { get; set; }
        public IList<MaintainTypeReponseModel> MaintainTypeReponseModel { get; set; }

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

        public RelayCommand SubmitCommand { get;}
        public RelayCommand BackCommand { get;}


        public RelayCommand BarcodeScanCommand { get;}
        public RelayCommand BarcodeManualCommand { get;}
        public RelayCommand<Barcode> IconItemTappedCommand { get;}
        public RelayCommand<Barcode> LabelItemTappedCommand { get;}

        #endregion

        #region Constructor

        public MaintainScanViewModel(IMoveService moveService, IMaintainService maintainService)
        {
            _moveService = moveService;
            _maintainService = maintainService;

            SubmitCommand = new RelayCommand(SubmitCommandRecieverAsync);
            BackCommand = new RelayCommand(BackCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandRecieverAsync);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new RelayCommand<Barcode>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>((model) => IconItemTappedCommandRecieverAsync(model));

            LoadMaintenanceTypeAsync();
        }

        #endregion

        #region Methods

        private async void LoadMaintenanceTypeAsync()
        {
            MaintainTypeReponseModel = await SQLiteServiceClient.Db.Table<MaintainTypeReponseModel>().ToListAsync();

            if (MaintainTypeReponseModel.Count == 0)
            {
                var model = await _maintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);
                try
                {
                    // The item does not exists in the database so lets insert it
                    await SQLiteServiceClient.Db.InsertAllAsync(model.MaintainTypeReponseModel);
                    MaintainTypeReponseModel = model.MaintainTypeReponseModel;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        internal void AssignValidatedValue(Partner model)
        {
            BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
            BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().PartnerCount = 1;
        }

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
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model.Id);
            }
        }

        private static async Task NavigateToValidatePartner(List<Barcode> model)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
            await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
        }

        private async void BarcodeManualCommandRecieverAsync()
        {
            var isNew = BarcodeCollection.ToList().Any(x => x.Id == ManaulBarcode);
            if (!isNew)
            {
                var value = await BarcodeScanner.ValidateBarcodeInsertIntoLocalDB(_moveService, ManaulBarcode, null, string.Empty);
                ManaulBarcode = string.Empty;
                BarcodeCollection.Add(value);
            }
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            await BarcodeScanner.BarcodeScanAsync(_moveService, null, string.Empty);
        }

        internal void AssignBarcodeScannerValue(List<Barcode> barcodes)
        {
            foreach (var item in barcodes)
                BarcodeCollection.Add(item);
        }

        private async void BackCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public async void SubmitCommandRecieverAsync()
        {
            var result = BarcodeCollection.Where(x => x.PartnerCount > 1).ToList();
            if (result.Count > 0)
                await NavigateToValidatePartner(result.ToList());
            else
            {
                try
                {
                    Loader.StartLoading();

                    List<MaintainKeg> kegs = new List<MaintainKeg>();
                    MaintainKeg keg = null;

                    foreach (var item in BarcodeCollection)
                    {
                        keg = new MaintainKeg
                        {
                            Barcode = item.Id,
                            //keg.BatchId = Uuid.GetUuId();
                            //keg.Contents = "";
                            //keg.HeldOnPalletId = Uuid.GetUuId();
                            //keg.KegId = Uuid.GetUuId();
                            //keg.Message = SimpleIoc.Default.GetInstance<MaintainViewModel>().Notes;
                            //keg.PalletId = Uuid.GetUuId();
                            ScanDate = DateTimeOffset.Now,
                            //keg.SkuId = Uuid.GetUuId();
                            Tags = new List<KegTag>(),
                            ValidationStatus = 4
                        };
                        kegs.Add(keg);
                    }

                    MaintenanceDoneModel model = new MaintenanceDoneModel
                    {
                        MaintenanceDoneRequestModel = new MaintenanceDoneRequestModel()
                    };
                    model.MaintenanceDoneRequestModel.ActionsPerformed = SimpleIoc.Default.GetInstance<MaintainViewModel>().MaintenancePerformed.ToList();
                    model.MaintenanceDoneRequestModel.DatePerformed = DateTimeOffset.Now.AddDays(-2);
                    model.MaintenanceDoneRequestModel.Kegs = kegs;
                    model.MaintenanceDoneRequestModel.LocationId = SimpleIoc.Default.GetInstance<MaintainViewModel>().PartnerModel.PartnerId;
                    model.MaintenanceDoneRequestModel.MaintenancePostingId = Uuid.GetUuId();
                    //model.MaintenanceDoneRequestModel.Operator = "Bent Neck";
                    //model.MaintenanceDoneRequestModel.SourceKey = "";
                    //model.MaintenanceDoneRequestModel.SubmittedDate = DateTimeOffset.Now;
                    model.MaintenanceDoneRequestModel.Latitude = (long)Geolocation.savedPosition.Latitude;
                    model.MaintenanceDoneRequestModel.Longitude = (long)Geolocation.savedPosition.Longitude;
                    model.MaintenanceDoneRequestModel.Tags = new List<MaintenanceDoneRequestModelTag>();

                    KegIDResponse kegIDResponse = await _maintainService.PostMaintenanceDoneAsync(model.MaintenanceDoneRequestModel, AppSettings.User.SessionId, Configuration.PostedMaintenanceDone);

                    if (kegIDResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        SimpleIoc.Default.GetInstance<MaintainDetailViewModel>().LoadInfo(BarcodeCollection);

                        Loader.StopLoading();
                        await Application.Current.MainPage.Navigation.PushModalAsync(new MaintainDetailView());
                    }
                    else
                    {
                        Loader.StopLoading();
                        //SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                    }
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
        }

        #endregion
    }
}
