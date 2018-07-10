using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Maintenace = "maintenace.png";
        private const string ValidationOK = "validationok.png";
        private const string Cloud = "collectionscloud.png";

        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;
        private readonly IMaintainService _maintainService;
        private readonly IGetIconByPlatform _getIconByPlatform;

        private IList<MaintainTypeReponseModel> MaintainTypeReponseModel { get; set; }

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

        private ObservableCollection<BarcodeModel> _BarcodeCollection = new ObservableCollection<BarcodeModel>();

        /// <summary>
        /// Sets and gets the BarcodeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<BarcodeModel> BarcodeCollection
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

        public DelegateCommand SubmitCommand { get;}
        public DelegateCommand BackCommand { get;}
        public DelegateCommand BarcodeScanCommand { get;}
        public DelegateCommand BarcodeManualCommand { get;}
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get;}
        public DelegateCommand<BarcodeModel> LabelItemTappedCommand { get;}

        #endregion

        #region Constructor

        public MaintainScanViewModel(IMoveService moveService, IMaintainService maintainService, INavigationService navigationService, IGetIconByPlatform getIconByPlatform)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;
            _maintainService = maintainService;
            _getIconByPlatform = getIconByPlatform;

            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));

            LoadMaintenanceType();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<MaintainScanMessage>(this, "MaintainScanMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        RealmDb.Write(() =>
                        {
                            var oldBarcode = BarcodeCollection.Where(x => x.Barcode == value.Barcodes.Barcode).FirstOrDefault();
                            oldBarcode.Pallets = value.Barcodes.Pallets;
                            oldBarcode.Kegs = value.Barcodes.Kegs;
                            oldBarcode.Icon = value?.Barcodes?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : value?.Barcodes?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                            oldBarcode.IsScanned = true;
                        });
                    }
                });
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = "Cancelled";
                    if (value == "Cancelled")
                    {

                    }
                });
            });
        }

        private void LoadMaintenanceType()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                MaintainTypeReponseModel = RealmDb.All<MaintainTypeReponseModel>().ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async Task AssignValidatedValueAsync(Partner model)
        {
            try
            {
                var unusedPerner = BarcodeCollection.Where(x => x.Kegs.Partners != model).Select(x => x.Kegs.Partners.FirstOrDefault()).FirstOrDefault();
                if (model.Kegs.FirstOrDefault().MaintenanceItems?.Count > 0)
                {
                    BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(Maintenace);
                }
                else
                {
                    BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(ValidationOK);
                }

                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);

                foreach (var item in BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode))
                {
                    item.Kegs.Partners.Remove(unusedPerner);
                }

                SubmitCommandRecieverAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void LabelItemTappedCommandRecieverAsync(BarcodeModel model)
        {
            try
            {
                if (model.Kegs.Partners.Count > 1)
                {
                    List<BarcodeModel> modelList = new List<BarcodeModel>
                    {
                        model
                    };
                    await NavigateToValidatePartner(modelList);
                }
                else
                {
                    var param = new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.MaintainScanView }
                    };
                    await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), param, useModalNavigation: true, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void IconItemTappedCommandRecieverAsync(BarcodeModel model)
        {
            try
            {
                if (model.Kegs.Partners.Count > 1)
                {
                    List<BarcodeModel> modelList = new List<BarcodeModel>
                    {
                        model
                    };
                    await NavigateToValidatePartner(modelList);
                }
                else
                {
                    //await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView(), animated: false);
                    //SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model);
                    var param = new NavigationParameters
                    {
                        { "model", model }
                    };
                    await _navigationService.NavigateAsync(new Uri("ScanInfoView", UriKind.Relative), param, useModalNavigation: true, animated: false);

                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task NavigateToValidatePartner(List<BarcodeModel> models)
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
                //SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(models);
                var param = new NavigationParameters
                    {
                        { "model", models }
                    };
                await _navigationService.NavigateAsync(new Uri("ValidateBarcodeView", UriKind.Relative), param, useModalNavigation: true, animated: false);

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
                    BarcodeModel model = new BarcodeModel
                    {
                        Barcode = ManaulBarcode,
                        TagsStr = string.Empty,
                        Icon = Cloud
                    };
                    
                    BarcodeCollection.Add(model);
                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = new List<string>() { ManaulBarcode },
                        PageName = ViewTypeEnum.MaintainScanView.ToString()
                    };
                    MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                    ManaulBarcode = string.Empty;
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
                var param = new NavigationParameters
                    {
                        { "Tags", null },{ "TagsStr", string.Empty },{ "ViewTypeEnum", ViewTypeEnum.MaintainScanView }
                    };
                await _navigationService.NavigateAsync(new Uri("CognexScanView", UriKind.Relative), param, useModalNavigation: true, animated: false);

                //await BarcodeScanner.BarcodeScanAsync(_moveService, null, string.Empty, ViewTypeEnum.MaintainScanView.ToString(), _navigationService);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignBarcodeScannerValue(IList<BarcodeModel> models)
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
                //await Application.Current.MainPage.Navigation.PopModalAsync();
                await _navigationService.GoBackAsync(useModalNavigation:true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void SubmitCommandRecieverAsync()
        {
            var result = BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList();
            if (result?.Count > 0)
                await NavigateToValidatePartner(result.ToList());
            else
            {
                try
                {
                    Loader.StartLoading();
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    if (location == null)
                        location = await Geolocation.GetLocationAsync(request);
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
                    model.MaintenanceDoneRequestModel.ActionsPerformed = ConstantManager.MaintainTypeCollection.Where(x=>x.IsToggled == true).Select(y=>y.Id).ToList();
                    model.MaintenanceDoneRequestModel.DatePerformed = DateTimeOffset.Now.AddDays(-2);
                    model.MaintenanceDoneRequestModel.Kegs = kegs;
                    model.MaintenanceDoneRequestModel.LocationId = ConstantManager.Partner.PartnerId;
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
                        Loader.StopLoading();
                        var param = new NavigationParameters
                        {
                            { "BarcodeModel", BarcodeCollection }
                        };
                        await _navigationService.NavigateAsync(new Uri("MaintainDetailView", UriKind.Relative), param, useModalNavigation: true, animated: false);

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

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<MaintainScanMessage>(this, "MaintainScanMessage");
            MessagingCenter.Unsubscribe<CancelledMessage>(this, "CancelledMessage");
        }

        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Partner"))
            {
                await AssignValidatedValueAsync(parameters.GetValue<Partner>("Partner"));
            }
            if (parameters.ContainsKey("models"))
            {
                AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
            }
        }

        #endregion
    }
}
