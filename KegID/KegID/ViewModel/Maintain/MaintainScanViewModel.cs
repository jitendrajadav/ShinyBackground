﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using Realms;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";
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

        private ObservableCollection<ValidateBarcodeModel> _BarcodeCollection = new ObservableCollection<ValidateBarcodeModel>();

        /// <summary>
        /// Sets and gets the BarcodeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ValidateBarcodeModel> BarcodeCollection
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
        public RelayCommand<ValidateBarcodeModel> IconItemTappedCommand { get;}
        public RelayCommand<ValidateBarcodeModel> LabelItemTappedCommand { get;}

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
            LabelItemTappedCommand = new RelayCommand<ValidateBarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<ValidateBarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));

            LoadMaintenanceType();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<MaintainScanMessage>(this, "MaintainScanMessage", message => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        //var barode = BarcodeCollection.Where(x => x.Barcode == value.Barcodes.Barcode).FirstOrDefault();
                        //barode.Icon = value.Barcodes.Icon;
                        //foreach (var item in value.Barcodes.Partners)
                        //{
                        //    barode.Partners.Add(item);
                        //}
                        //foreach (var item in value.Barcodes.MaintenanceItems)
                        //{
                        //    barode.MaintenanceItems.Add(item);
                        //}
                        //foreach (var item in value.Barcodes.Tags)
                        //{
                        //    barode.Tags.Add(item);
                        //}

                        var oldModel = BarcodeCollection.Where(x => x.Barcode == value.Barcodes.Barcode).FirstOrDefault();
                        oldModel = value.Barcodes;
                    }
                });
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message => {
                Device.BeginInvokeOnMainThread(() => {
                    var value = "Cancelled";
                    if (value == "Cancelled")
                    {

                    }
                });
            });
        }

        private async void LoadMaintenanceType()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                MaintainTypeReponseModel = RealmDb.All<MaintainTypeReponseModel>().ToList();//await SQLiteServiceClient.Db.Table<MaintainTypeReponseModel>().ToListAsync();
                if (MaintainTypeReponseModel.Count == 0)
                {
                    MaintainTypeReponseModel = await LoadMaintenanceTypeAsync();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task<IList<MaintainTypeReponseModel>> LoadMaintenanceTypeAsync()
        {
            var model = await _maintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() =>
                {
                    foreach (var item in model.MaintainTypeReponseModel)
                    {
                        RealmDb.Add(item);
                    }
                });
                // The item does not exists in the database so lets insert it
               // await SQLiteServiceClient.Db.InsertAllAsync(model.MaintainTypeReponseModel);
                MaintainTypeReponseModel = model.MaintainTypeReponseModel;
            }
            catch (Exception ex)    
            {
                 Crashes.TrackError(ex);
            }
            return MaintainTypeReponseModel;
        }

        internal void AssignValidatedValue(Partner model)
        {
            try
            {
                //BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Partners.Clear();
                //BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
                //BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Partners.Add(model);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void LabelItemTappedCommandRecieverAsync(ValidateBarcodeModel model)
        {
            try
            {
                if (model.Kegs.Partners.Count > 1)
                {
                    List<ValidateBarcodeModel> modelList = new List<ValidateBarcodeModel>
                    {
                        model
                    };
                    await NavigateToValidatePartner(modelList);
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView(), animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void IconItemTappedCommandRecieverAsync(ValidateBarcodeModel model)
        {
            try
            {
                if (model.Kegs.Partners.Count > 1)
                {
                    List<ValidateBarcodeModel> modelList = new List<ValidateBarcodeModel>
                    {
                        model
                    };
                    await NavigateToValidatePartner(modelList);
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView(), animated: false);
                    SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static async Task NavigateToValidatePartner(List<ValidateBarcodeModel> models)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
                SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(models);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void BarcodeManualCommandRecieverAsync()
        {
            try
            {
                var isNew = BarcodeCollection.ToList().Any(x => x.Barcode == ManaulBarcode);
                if (!isNew)
                {
                    //Barcode barcode = new Barcode
                    //{
                    //    Id = ManaulBarcode,
                    //    //Tags = null,
                    //    TagsStr = string.Empty,
                    //    Icon = Cloud
                    //};

                    ValidateBarcodeModel model = new ValidateBarcodeModel
                    {
                        Barcode = ManaulBarcode,
                        //    //Tags = null,
                        TagsStr = string.Empty,
                        Icon = Cloud
                    };
                    BarcodeCollection.Add(model);
                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = new List<string>() { ManaulBarcode },
                        Page = ViewTypeEnum.MaintainScanView.ToString()
                    };
                    MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                    ManaulBarcode = string.Empty;

                    //var value = await BarcodeScanner.ValidateBarcodeInsertIntoLocalDB(_moveService, ManaulBarcode, null, string.Empty);
                    //ManaulBarcode = string.Empty;
                    //BarcodeCollection.Add(value);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            try
            {
                await BarcodeScanner.BarcodeScanAsync(_moveService, null, string.Empty, ViewTypeEnum.MaintainScanView.ToString());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignBarcodeScannerValue(IList<ValidateBarcodeModel> models)
        {
            try
            {
                foreach (var item in models)
                    BarcodeCollection.Add(item);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BackCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void SubmitCommandRecieverAsync()
        {
            var result = BarcodeCollection.Where(x => x.Kegs.Partners.Count > 1).ToList();
            if (result.Count > 0)
                await NavigateToValidatePartner(result.ToList());
            else
            {
                try
                {
                    Loader.StartLoading();
                    var location = await Geolocation.GetLocationAsync();
                    List<MaintainKeg> kegs = new List<MaintainKeg>();
                    MaintainKeg keg = null;

                    foreach (var item in BarcodeCollection)
                    {
                        keg = new MaintainKeg
                        {
                            Barcode = item.Barcode,
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
                    model.MaintenanceDoneRequestModel.ActionsPerformed = SimpleIoc.Default.GetInstance<MaintainViewModel>().MaintainTypeCollection.Where(x=>x.IsToggled == true).Select(y=>y.Id).ToList();
                    model.MaintenanceDoneRequestModel.DatePerformed = DateTimeOffset.Now.AddDays(-2);
                    model.MaintenanceDoneRequestModel.Kegs = kegs;
                    model.MaintenanceDoneRequestModel.LocationId = SimpleIoc.Default.GetInstance<MaintainViewModel>().PartnerModel.PartnerId;
                    model.MaintenanceDoneRequestModel.MaintenancePostingId = Uuid.GetUuId();
                    //model.MaintenanceDoneRequestModel.Operator = "Bent Neck";
                    //model.MaintenanceDoneRequestModel.SourceKey = "";
                    //model.MaintenanceDoneRequestModel.SubmittedDate = DateTimeOffset.Now;
                    model.MaintenanceDoneRequestModel.Latitude = (long)location.Latitude;
                    model.MaintenanceDoneRequestModel.Longitude = (long)location.Longitude;
                    model.MaintenanceDoneRequestModel.Tags = new List<MaintenanceDoneRequestModelTag>();

                    KegIDResponse kegIDResponse = await _maintainService.PostMaintenanceDoneAsync(model.MaintenanceDoneRequestModel, AppSettings.User.SessionId, Configuration.PostedMaintenanceDone);

                    if (kegIDResponse.StatusCode == System.Net.HttpStatusCode.NoContent.ToString())
                    {
                        SimpleIoc.Default.GetInstance<MaintainDetailViewModel>().LoadInfo(BarcodeCollection);

                        Loader.StopLoading();
                        await Application.Current.MainPage.Navigation.PushModalAsync(new MaintainDetailView(), animated: false);
                    }
                    else
                    {
                        Loader.StopLoading();
                        //SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                    }
                }
                catch (Exception ex)
                {
                     Crashes.TrackError(ex);
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
