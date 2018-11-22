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
using Prism.Services;
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

        public string Notes { get; private set; }
        private string ManifestId;

        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;
        private readonly IMaintainService _maintainService;
        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly IUuidManager _uuidManager;
        private readonly IManifestManager _manifestManager;
        private readonly IGeolocationService _geolocationService;

        private IList<MaintainTypeReponseModel> MaintainTypeReponseModel { get; set; }

        #region ManaulBarcode

        /// <summary>
        /// The <see cref="ManaulBarcode" /> property's name.
        /// </summary>
        public const string ManaulBarcodePropertyName = "ManaulBarcode";

        private string _ManaulBarcode = default;

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
        public DelegateCommand<BarcodeModel> DeleteItemCommand { get;}


        #endregion

        #region Constructor

        public MaintainScanViewModel(IMoveService moveService, IMaintainService maintainService, INavigationService navigationService, IGetIconByPlatform getIconByPlatform, IUuidManager uuidManager, IManifestManager manifestManager, IGeolocationService geolocationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;
            _maintainService = maintainService;
            _getIconByPlatform = getIconByPlatform;
            _uuidManager = uuidManager;
            _manifestManager = manifestManager;
            _geolocationService = geolocationService;

            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));
            DeleteItemCommand = new DelegateCommand<BarcodeModel>((model) => DeleteItemCommandReciever(model));

            LoadMaintenanceType();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void DeleteItemCommandReciever(BarcodeModel model)
        {
            BarcodeCollection.Remove(model);
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<MaintainScanMessage>(this, "MaintainScanMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                        {
                            var oldBarcode = BarcodeCollection.Where(x => x.Barcode == value.Barcodes.Barcode).FirstOrDefault();
                            oldBarcode.Pallets = value.Barcodes.Pallets;
                            oldBarcode.Kegs = value.Barcodes.Kegs;
                            oldBarcode.Icon = value?.Barcodes?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : value?.Barcodes?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                            oldBarcode.IsScanned = true;
                            db.Commit();
                        };
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
                    await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.MaintainScanView },
                        {"AddTagsViewInitialValue",model }
                    }, useModalNavigation: true, animated: false);
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
                    await _navigationService.NavigateAsync(new Uri("ScanInfoView", UriKind.Relative), new NavigationParameters
                    {
                        { "model", model }
                    }, useModalNavigation: true, animated: false);
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
                await _navigationService.NavigateAsync(new Uri("ValidateBarcodeView", UriKind.Relative), new NavigationParameters
                    {
                        { "model", models }
                    }, useModalNavigation: true, animated: false);
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
                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        var message = new StartLongRunningTaskMessage
                        {
                            Barcode = new List<string>() { ManaulBarcode },
                            PageName = ViewTypeEnum.MaintainScanView.ToString()
                        };
                        MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                    }
                   
                    ManaulBarcode = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task GetPostedManifestDetail()
        {
            await _navigationService.NavigateAsync(new Uri("MaintainDetailView", UriKind.Relative),
                              new NavigationParameters
                              {
                                {
                                    "BarcodeModel", BarcodeCollection
                                }
                              }, useModalNavigation: true, animated: false);
        }

        private void AddorUpdateManifestOffline(ManifestModel manifestPostModel, bool queue)
        {
            string manifestId = manifestPostModel.ManifestId;
            var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestId);
            if (isNew != null)
            {
                try
                {
                    manifestPostModel.IsDraft = false;
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestPostModel, update: true);
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                try
                {
                    if (queue)
                    {
                        manifestPostModel.IsQueue = true;
                    }
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestPostModel);
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("ScanditScanView", UriKind.Relative), new NavigationParameters
                    {
                        { "Tags", null },{ "TagsStr", string.Empty },{ "ViewTypeEnum", ViewTypeEnum.MaintainScanView }
                    }, useModalNavigation: true, animated: false);
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
                await _navigationService.GoBackAsync(useModalNavigation:true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void SubmitCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();

                var location = await _geolocationService.GetLastLocationAsync();

                ManifestModel manifestPostModel = null;

                var result = BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList();
                if (result?.Count > 0)
                    await NavigateToValidatePartner(result.ToList());

                else
                {
                    try
                    {
                        manifestPostModel = GenerateManifest(location??new Xamarin.Essentials.Location(0,0), ManifestId);

                        var current = Connectivity.NetworkAccess;
                        if (current == NetworkAccess.Internet)
                        {
                            KegIDResponse kegIDResponse = await _maintainService.PostMaintenanceDoneAsync(manifestPostModel.MaintenanceModels.MaintenanceDoneRequestModel, AppSettings.SessionId, Configuration.PostedMaintenanceDone);
                            try
                            {
                                AddorUpdateManifestOffline(manifestPostModel, false);
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
                            await GetPostedManifestDetail();
                        }
                        else
                        {
                            try
                            {
                                AddorUpdateManifestOffline(manifestPostModel, true);
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
                            await GetPostedManifestDetail();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    finally
                    {
                        Cleanup();
                    }
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                Loader.StopLoading();
            }
        }

        public ManifestModel GenerateManifest(Xamarin.Essentials.Location location, string manifestId = "")
        {
                List<MaintainKeg> kegs = new List<MaintainKeg>();
                MaintainKeg keg = null;

                MaintenanceModel model = new MaintenanceModel
                {
                    MaintenanceDoneRequestModel = new MaintenanceDoneRequestModel()
                };

                foreach (var item in BarcodeCollection)
                {
                    keg = new MaintainKeg
                    {
                        Barcode = item.Barcode,
                        ScanDate = DateTimeOffset.Now,
                        ValidationStatus = 4
                    };
                    kegs.Add(keg);
                    model.MaintenanceDoneRequestModel.Kegs.Add(keg);
                }

                foreach (var item in ConstantManager.MaintainTypeCollection.Where(x => x.IsToggled == true).Select(y => y.Id).ToList())
                {
                    model.MaintenanceDoneRequestModel.ActionsPerformed.Add(item);
                }

                model.MaintenanceDoneRequestModel.DatePerformed = DateTimeOffset.Now.AddDays(-2);
                model.MaintenanceDoneRequestModel.LocationId = ConstantManager.Partner.PartnerId;
                model.MaintenanceDoneRequestModel.MaintenancePostingId = _uuidManager.GetUuId();
                model.MaintenanceDoneRequestModel.Latitude = (long)location.Latitude;
                model.MaintenanceDoneRequestModel.Longitude = (long)location.Longitude;
                model.MaintenanceDoneRequestModel.Notes = Notes;
                model.MaintenanceDoneRequestModel.PartnerModel = ConstantManager.Partner;

                return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.REPAIR_MANIFEST, manifestId: !string.IsNullOrEmpty(manifestId) ? manifestId : _uuidManager.GetUuId(),
                    barcodeCollection: BarcodeCollection, (long)location.Latitude, (long)location.Longitude, tags: ConstantManager.Tags, ConstantManager.TagsStr, partnerModel: ConstantManager.Partner,
                                                                    newPallets: new List<NewPallet>(), batches: new List<NewBatch>(),
                                                                    closedBatches: new List<string>(), model, validationStatus: 4, contents: "");
        }

        internal void AssignAddTagsValue(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("Barcode"))
                {
                    try
                    {
                        string barcode = parameters.GetValue<string>("Barcode");
                        var oldBarcode = BarcodeCollection.Where(x => x.Barcode == barcode).FirstOrDefault();

                        for (int i = oldBarcode.Tags.Count - 1; i >= 0; i--)
                        {
                            oldBarcode.Tags.RemoveAt(i);
                        }

                        using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                        {
                            foreach (var item in ConstantManager.Tags)
                            {
                                oldBarcode.Tags.Add(item);
                            }
                            oldBarcode.TagsStr = ConstantManager.TagsStr;
                            db.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void Cleanup()
        {
            BarcodeCollection.Clear();
            MaintainDTToMaintMsg message = new MaintainDTToMaintMsg
            {
                CleanUp = true
            };
            MessagingCenter.Send(message, "MaintainDTToMaintMsg");
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("MaintainHome"))
            {
                await _navigationService.GoBackAsync(parameters, useModalNavigation: true, animated: false);
            }
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<MaintainScanMessage>(this, "MaintainScanMessage");
            MessagingCenter.Unsubscribe<CancelledMessage>(this, "CancelledMessage");
        }

        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "Partner":
                    await AssignValidatedValueAsync(parameters.GetValue<Partner>("Partner"));
                    break;
                case "models":
                    AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
                    break;
                case "AddTags":
                    AssignAddTagsValue(parameters);
                    break;
                case "Notes":
                    AssignMaintenanceViewValue(parameters);
                    break;
                default:
                    break;
            }
        }

        private void AssignMaintenanceViewValue(INavigationParameters parameters)
        {
            try
            {
                BarcodeCollection.Clear();
                Notes = parameters.GetValue<string>("Notes");
                ConstantManager.Partner = parameters.GetValue<PartnerModel>("PartnerModel");
                var value = parameters.GetValue<ManifestModel>("ManifestModel");

                if (value != null)
                {
                    ManifestId = value.ManifestId;
                    foreach (var item in value.MaintenanceModels.MaintenanceDoneRequestModel.Kegs)
                    {
                        BarcodeCollection.Add(new BarcodeModel { Barcode = item.Barcode });
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
