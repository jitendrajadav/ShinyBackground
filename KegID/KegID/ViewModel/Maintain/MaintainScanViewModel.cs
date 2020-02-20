using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using KegID.Common;
using KegID.Delegates;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using Shiny;
using Shiny.Locations;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainScanViewModel : BaseViewModel, IDestructible
    {
        #region Properties

        private const string Maintenace = "maintenace.png";
        private const string ValidationOK = "validationok.png";
        private const string Cloud = "collectionscloud.png";

        public string Notes { get; private set; }
        private string ManifestId;

        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly IUuidManager _uuidManager;
        private readonly IManifestManager _manifestManager;
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;
        public Position LocationMessage { get; set; }
        public string ManaulBarcode { get; set; }
        public ObservableCollection<BarcodeModel> BarcodeCollection { get; set; } = new ObservableCollection<BarcodeModel>();
        private IList<MaintenanceTypeModel> SelectedMaintainenace { get; set; }

        #endregion

        #region Commands

        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand BarcodeManualCommand { get; }
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> LabelItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> DeleteItemCommand { get; }

        #endregion

        #region Constructor

        public MaintainScanViewModel(INavigationService navigationService, IGetIconByPlatform getIconByPlatform, IUuidManager uuidManager, IManifestManager manifestManager, IGpsManager gpsManager, IGpsListener gpsListener) : base(navigationService)
        {
            _getIconByPlatform = getIconByPlatform;
            _uuidManager = uuidManager;
            _manifestManager = manifestManager;
            _gpsManager = gpsManager;
            _gpsListener = gpsListener;
            _gpsListener.OnReadingReceived += OnReadingReceived;

            SubmitCommand = new DelegateCommand(async () => await RunSafe(SubmitCommandRecieverAsync()));
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));
            DeleteItemCommand = new DelegateCommand<BarcodeModel>((model) => DeleteItemCommandReciever(model));

            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            LocationMessage = e.Reading.Position;
        }

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
        }

        internal async Task AssignValidatedValueAsync(Partner model)
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

            await NavigationService.GoBackAsync(animated: false);

            foreach (var item in BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode))
            {
                item.Kegs.Partners.Remove(unusedPerner);
            }
        }

        private async void LabelItemTappedCommandRecieverAsync(BarcodeModel model)
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
                await NavigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.MaintainScanView },
                        {"AddTagsViewInitialValue",model }
                    }, animated: false);
            }
        }

        private async void IconItemTappedCommandRecieverAsync(BarcodeModel model)
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
                await NavigationService.NavigateAsync("ScanInfoView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
            }
        }

        private async Task NavigateToValidatePartner(List<BarcodeModel> models)
        {
            await NavigationService.NavigateAsync("ValidateBarcodeView", new NavigationParameters
                    {
                        { "model", models }
                    }, animated: false);
        }

        private void BarcodeManualCommandRecieverAsync()
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
                    // IJobManager can and should be injected into your viewmodel code
                    ShinyHost.Resolve<Shiny.Jobs.IJobManager>().RunTask("MaintainJob" + ManaulBarcode, async _ =>
                    {
                        // your code goes here - async stuff is welcome (and necessary)
                        var response = await ApiManager.GetValidateBarcode(ManaulBarcode, Settings.SessionId);
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var data = await Task.Run(() => JsonConvert.DeserializeObject<BarcodeModel>(json, GetJsonSetting()));

                            if (data.Kegs != null)
                            {
                                using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                                {
                                    var oldBarcode = BarcodeCollection.FirstOrDefault(x => x.Barcode == data?.Kegs?.Partners?.FirstOrDefault().Kegs?.FirstOrDefault()?.Barcode);
                                    oldBarcode.Pallets = data.Pallets;
                                    oldBarcode.Kegs = data.Kegs;
                                    oldBarcode.Icon = data?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : data?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                                    if (oldBarcode.Icon == "validationerror.png")
                                        Vibration.Vibrate();
                                    oldBarcode.IsScanned = true;
                                    db.Commit();
                                }
                            }
                        }
                    });

                }

                ManaulBarcode = string.Empty;
            }
        }

        private async Task GetPostedManifestDetail()
        {
            await NavigationService.NavigateAsync("MaintainDetailView",
                              new NavigationParameters
                              {
                                {
                                    "BarcodeModel", BarcodeCollection
                                },
                                {
                                    "SelectedMaintainenace",SelectedMaintainenace
                                }
                              }, animated: false);
        }

        private void AddorUpdateManifestOffline(ManifestModel manifestPostModel, bool queue)
        {
            string manifestId = manifestPostModel.ManifestId;
            var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestId);
            if (isNew != null)
            {
                manifestPostModel.IsDraft = false;
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() =>
                {
                    RealmDb.Add(manifestPostModel, update: true);
                });
            }
            else
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
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("ScanditScanView", new NavigationParameters
                    {
                        { "Tags", null },{ "TagsStr", string.Empty },{ "ViewTypeEnum", ViewTypeEnum.MaintainScanView }
                    }, animated: false);
        }

        internal void AssignBarcodeScannerValue(IList<BarcodeModel> models)
        {
            foreach (var item in models)
                BarcodeCollection.Add(item);
        }

        private async void BackCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
        }

        public async Task SubmitCommandRecieverAsync()
        {
            UserDialogs.Instance.ShowLoading("Loading");
            ManifestModel manifestPostModel = null;

            var result = BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList();
            if (result?.Count > 0)
                await NavigateToValidatePartner(result.ToList());

            else
            {
                manifestPostModel = GenerateManifest(LocationMessage ?? new Position(0, 0), ManifestId);

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var response = await ApiManager.PostMaintenanceDone(manifestPostModel.MaintenanceModels.MaintenanceDoneRequestModel, Settings.SessionId);

                    AddorUpdateManifestOffline(manifestPostModel, false);
                    await GetPostedManifestDetail();
                }
                else
                {
                    AddorUpdateManifestOffline(manifestPostModel, true);
                    await GetPostedManifestDetail();
                }

            }
            UserDialogs.Instance.HideLoading();
        }

        public ManifestModel GenerateManifest(Position location, string manifestId = "")
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

            foreach (var item in SelectedMaintainenace.Where(x => x.IsToggled).Select(y => y.Id).ToList())
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
                barcodeCollection: BarcodeCollection, (long)location.Latitude, (long)location.Longitude, string.Empty, string.Empty, tags: ConstantManager.Tags, ConstantManager.TagsStr, partnerModel: ConstantManager.Partner,
                                                                newPallets: new List<NewPallet>(), batches: new List<NewBatch>(),
                                                                closedBatches: new List<string>(), model, validationStatus: 4, null, contents: "");
        }

        internal void AssignAddTagsValue(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Barcode"))
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
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }

            if (_gpsManager.IsListening)
            {
                await _gpsManager.StopListener();
            }

            await _gpsManager.StartListener(new GpsRequest
            {
                UseBackground = true,
                Priority = GpsPriority.Highest,
                Interval = TimeSpan.FromSeconds(5),
                ThrottledInterval = TimeSpan.FromSeconds(3) //Should be lower than Interval
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<MaintainScanMessage>(this, "MaintainScanMessage");
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
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
            }
        }

        private void AssignMaintenanceViewValue(INavigationParameters parameters)
        {
            BarcodeCollection.Clear();
            Notes = parameters.GetValue<string>("Notes");
            ConstantManager.Partner = parameters.GetValue<PartnerModel>("PartnerModel");
            var value = parameters.GetValue<ManifestModel>("ManifestModel");
            SelectedMaintainenace = parameters.GetValue<List<MaintenanceTypeModel>>("selectedMaintenance");

            if (value != null)
            {
                ManifestId = value.ManifestId;
                foreach (var item in value.MaintenanceModels.MaintenanceDoneRequestModel.Kegs)
                {
                    BarcodeCollection.Add(new BarcodeModel { Barcode = item.Barcode });
                }
            }
        }

        public void Destroy()
        {
            _gpsListener.OnReadingReceived -= OnReadingReceived;
        }

        #endregion
    }
}
