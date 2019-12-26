using System.Collections.ObjectModel;
using KegID.Model;
using Xamarin.Forms;
using KegID.Common;
using System;
using KegID.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using KegID.Messages;
using Prism.Services;
using Realms;
using KegID.LocalDb;
using Xamarin.Essentials;
using Acr.UserDialogs;
using Shiny.Locations;
using KegID.Delegates;
using Shiny;

namespace KegID.ViewModel
{
    public class AddPalletsViewModel : BaseViewModel, IDestructible
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IManifestManager _manifestManager;
        private readonly IUuidManager _uuidManager;
        //private readonly IGeolocationService _geolocationService;
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;
        public string ManifestId { get; set; }
        public NewBatch BatchModel { get; private set; }
        public string SizeButtonTitle { get; private set; }
        public PartnerModel PartnerModel { get; private set; }
        public string AddPalletsTitle { get; set; }
        public string Pallets { get; set; }
        public ObservableCollection<PalletModel> PalletCollection { get; set; } = new ObservableCollection<PalletModel>();
        public string Kegs { get; set; }
        public string ContainerType { get; set; }
        public string ContainerTypes { get; set; }

        public Position LocationMessage { get; set; }

        #endregion

        #region Commands

        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand FillScanCommand { get; }
        public DelegateCommand FillKegsCommand { get; }
        public DelegateCommand<PalletModel> ItemTappedCommand { get; }
        public DelegateCommand<PalletModel> DeleteItemCommand { get; }

        #endregion

        #region Constructor

        public AddPalletsViewModel(INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager, IUuidManager uuidManager, IGpsManager gpsManager, IGpsListener gpsListener) : base(navigationService)
        {
            _dialogService = dialogService;
            _manifestManager = manifestManager;
            _uuidManager = uuidManager;
            _gpsManager = gpsManager;
            _gpsListener = gpsListener;
            _gpsListener.OnReadingReceived += OnReadingReceived;

            SubmitCommand = new DelegateCommand(async()=> await RunSafe(SubmitCommandRecieverAsync()));
            FillScanCommand = new DelegateCommand(FillScanCommandRecieverAsync);
            FillKegsCommand = new DelegateCommand(FillKegsCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<PalletModel>(async (model) => await ItemTappedCommandRecieverAsync(model));
            DeleteItemCommand = new DelegateCommand<PalletModel>((model) => DeleteItemCommandReciever(model));

            PreferenceSetting();
        }

        #endregion

        #region Methods

        void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            LocationMessage = e.Reading.Position;
             //= $"{e.Reading.Position.Latitude}, {e.Reading.Position.Longitude}";
        }

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            ContainerType = preferences.Find(x => x.PreferenceName == "CONTAINER_TYPE").PreferenceValue;
            ContainerTypes = preferences.Find(x => x.PreferenceName == "CONTAINER_TYPES").PreferenceValue;
        }

        private void DeleteItemCommandReciever(PalletModel model)
        {
            PalletCollection.Remove(model);
            CountKegs();
        }

        private async Task ItemTappedCommandRecieverAsync(PalletModel model)
        {
            try
            {
                await _navigationService.NavigateAsync("FillScanView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void FillKegsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(new NavigationParameters { { "PalletCollection", PalletCollection } }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task SubmitCommandRecieverAsync()
        {
            try
            {
                //var location = await _geolocationService.GetLastLocationAsync();
                    var barcodes = ConstantManager.Barcodes;
                    var tags = ConstantManager.Tags;
                    var partnerModel = ConstantManager.Partner;

                    if (PalletCollection.Count == 0)
                    {
                        await _dialogService.DisplayAlertAsync("Error", "Error: Please add some scans.", "Ok");
                        return;
                    }

                    IEnumerable<PalletModel> empty = PalletCollection.Where(x => x.Barcode.Count == 0);
                    if (empty.ToList().Count > 0)
                    {
                        string result = await _dialogService.DisplayActionSheetAsync("Error? \n Some pallets have 0 scans. Do you want to edit them or remove the empty pallets.", null, null, "Remove empties", "Edit");
                        if (result == "Remove empties")
                        {
                            foreach (var item in empty.Reverse())
                            {
                                PalletCollection.Remove(item);
                            }
                            if (PalletCollection.Count == 0)
                            {
                                return;
                            }
                        }
                        if (result == "Edit")
                        {
                            await ItemTappedCommandRecieverAsync(empty.FirstOrDefault());
                            return;
                        }
                    }

                    List<string> closedBatches = new List<string>();
                    List<NewPallet> newPallets = new List<NewPallet>();
                    NewPallet newPallet = null;
                    List<ManifestTItem> palletItems = new List<ManifestTItem>();
                ManifestTItem palletItem = null;

                    foreach (var pallet in PalletCollection)
                    {
                        foreach (var item in pallet.Barcode)
                        {
                            palletItem = new ManifestTItem
                            {
                                Barcode = item.Barcode,
                                ScanDate = DateTimeOffset.UtcNow.Date,
                                TagsStr = item.TagsStr
                            };

                            if (item.Tags != null)
                            {
                                foreach (var tag in item.Tags)
                                {
                                    palletItem.Tags.Add(tag);
                                }
                            }
                            palletItems.Add(palletItem);
                        }

                        newPallet = new NewPallet
                        {
                            Barcode = pallet.BatchId,
                            BuildDate = DateTimeOffset.UtcNow.Date,
                            StockLocation = partnerModel?.PartnerId,
                            StockLocationId = partnerModel?.PartnerId,
                            StockLocationName = partnerModel?.FullName,
                            OwnerId = AppSettings.CompanyId,
                            PalletId = _uuidManager.GetUuId(),
                            ReferenceKey = "",
                        };
                        if (tags != null)
                        {
                            foreach (var item in tags)
                                newPallet.Tags.Add(item);
                        }
                        foreach (var item in palletItems)
                            newPallet.PalletItems.Add(item);
                        newPallets.Add(newPallet);
                    }

                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var canCloseBatches = RealmDb.All<Preference>().Where(x => x.PreferenceName == "CanCloseBatches").FirstOrDefault();
                bool closeSatches = canCloseBatches != null ? bool.Parse(canCloseBatches.PreferenceValue) : false;

                if (closeSatches)
                {
                    bool accept = await _dialogService.DisplayAlertAsync("Close batch", "Mark this batch as completed?", "Yes", "No");
                    if (accept)
                        closedBatches = PalletCollection.Select(x => x.ManifestId).ToList();
                }

                UserDialogs.Instance.ShowLoading("Loading");
                    ManifestModel model = null;
                    try
                    {
                    model = _manifestManager.GetManifestDraft(EventTypeEnum.FILL_MANIFEST, ManifestId ?? PalletCollection.FirstOrDefault().ManifestId,
                    barcodes, LocationMessage != null ? (long)LocationMessage.Latitude : 0, LocationMessage != null ? (long)LocationMessage.Longitude : 0,string.Empty,string.Empty, tags, string.Empty, partnerModel, newPallets, new List<NewBatch>(), closedBatches, null, 4, null);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    if (model != null)
                    {
                        try
                        {
                            var current = Connectivity.NetworkAccess;
                            if (current == NetworkAccess.Internet)
                            {
                                var response = await ApiManager.PostManifest(model, AppSettings.SessionId);

                                try
                                {
                                    AddorUpdateManifestOffline(model, false);
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
                                    AddorUpdateManifestOffline(model, true);
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
                            model = null;
                            barcodes = null;
                            tags = null;
                            partnerModel = null;
                            closedBatches = null;
                            newPallets = null;
                            newPallet = null;
                            palletItems = null;
                            palletItem = null;
                            Cleanup();
                        }
                    }
                    else
                        await _dialogService.DisplayAlertAsync("Alert", "Something goes wrong please check again", "Ok");

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task GetPostedManifestDetail()
        {
            ManifestResponseModel manifest = new ManifestResponseModel();
            string Contents = string.Empty;
            try
            {
                Contents = ConstantManager.Tags.Count > 2 ? ConstantManager.Tags?[2]?.Value ?? string.Empty : string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            if (string.IsNullOrEmpty(Contents))
            {
                try
                {
                    Contents = ConstantManager.Tags.Count > 3 ? ConstantManager.Tags?[3]?.Value ?? string.Empty : string.Empty;
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            try
            {

                manifest.ManifestItems = new List<CreatedManifestItem>();
                foreach (var item in ConstantManager.Barcodes)
                {
                    manifest.ManifestItems.Add(new CreatedManifestItem
                    {
                        Barcode = item.Barcode,
                        Contents = Contents,
                        Keg = new CreatedManifestKeg
                        {
                            Barcode = item.Barcode,
                            Contents = Contents,
                            OwnerName = ConstantManager.Partner.FullName,
                            SizeName = ConstantManager.Tags.LastOrDefault().Value,
                        }
                    });
                }
                manifest.TrackingNumber = ManifestId;
                manifest.ShipDate = DateTimeOffset.UtcNow.Date.ToShortDateString();
                manifest.CreatorCompany = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.ToString(), Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
                manifest.SenderPartner = new CreatorCompany { Address = ConstantManager.Partner.Address/*, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey */};
                manifest.ReceiverPartner = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.ToString(), Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
                manifest.SenderShipAddress = new Address { City = ConstantManager.Partner.City, Country = ConstantManager.Partner.Country, Geocoded = false, Latitude = (long)ConstantManager.Partner.Lat, Line1 = ConstantManager.Partner.Address, Line2 = ConstantManager.Partner.Address1, Longitude = (long)ConstantManager.Partner.Lon, PostalCode = ConstantManager.Partner.PostalCode, State = ConstantManager.Partner.State };
                manifest.ReceiverShipAddress = new Address { City = ConstantManager.Partner.City, Country = ConstantManager.Partner.Country, Geocoded = false, Latitude = (long)ConstantManager.Partner.Lat, Line1 = ConstantManager.Partner.Address, Line2 = ConstantManager.Partner.Address1, Longitude = (long)ConstantManager.Partner.Lon, PostalCode = ConstantManager.Partner.PostalCode, State = ConstantManager.Partner.State };
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            await _navigationService.NavigateAsync("ManifestDetailView", new NavigationParameters
                                {
                                    { "manifest", manifest },{ "Contents", Contents }
                                }, animated: false);
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

        private async void FillScanCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("FillScanView", new NavigationParameters
                    {
                        { "GenerateManifestIdAsync", "GenerateManifestIdAsync" },
                        {"PartnerModel",PartnerModel },
                            {"NewBatchModel",BatchModel },
                            {"SizeButtonTitle",SizeButtonTitle }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignValueToAddPalletAsync(string _batchId, IList<BarcodeModel> barcodes)
        {
            try
            {
                if (barcodes.Count == 0)
                {
                    PalletCollection.Clear();
                    Kegs = default;
                }
                else if (!PalletCollection.Any(x => x.BatchId == _batchId))
                {
                    PalletCollection.Add(new PalletModel() { Barcode = barcodes, Count = barcodes.Count(), BatchId = _batchId });
                    CountKegs();
                }
                else
                {
                    PalletCollection.Where(x => x.BatchId == _batchId).FirstOrDefault().Barcode = barcodes;
                    PalletCollection.Where(x => x.BatchId == _batchId).FirstOrDefault().Count = barcodes.Count;
                    CountKegs();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void CountKegs()
        {
            if (PalletCollection.Sum(x => x.Count) > 1)
                Kegs = string.Format("({0} {1})", PalletCollection.Sum(x => x.Count), ContainerTypes);
            else
                Kegs = string.Format("({0} {1})", PalletCollection.Sum(x => x.Count), ContainerType);
        }

        internal void AssignFillScanValue(IList<BarcodeModel> _barcodes, string _batchId)
        {
            try
            {
                PalletModel pallet = PalletCollection.Where(x => x.BatchId == _batchId).FirstOrDefault();
                if (pallet != null)
                {
                    using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                    {
                        pallet.Barcode = _barcodes;
                        pallet.Count = _barcodes.Count();
                        db.Commit();
                    }
                }
                else
                {
                    PalletCollection.Add(new PalletModel()
                    {
                        Barcode = _barcodes,
                        Count = _barcodes.Count(),
                        BatchId = _batchId,
                    });
                }
                if (PalletCollection.Sum(x => x.Count) > 1)
                    Kegs = string.Format("({0} {1})", PalletCollection.Sum(x => x.Count), ContainerTypes);
                else
                    Kegs = string.Format("({0} {1})", PalletCollection.Sum(x => x.Count), ContainerType);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void Cleanup()
        {
            try
            {
                PalletCollection.Clear();
                Kegs = default;
                ConstantManager.Barcodes.Clear();
                ConstantManager.Tags.Clear();
                ConstantManager.Partner = null;

                AddPalletToFillScanMsg msg = new AddPalletToFillScanMsg
                {
                    CleanUp = true
                };
                MessagingCenter.Send(msg, "AddPalletToFillScanMsg");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
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

            if (parameters.ContainsKey("FillKegsCommandRecieverAsync"))
            {
                FillKegsCommandRecieverAsync();
            }

            switch (parameters.Keys.FirstOrDefault())
            {
                case "Barcodes":
                    AssignFillScanValue(parameters.GetValue<IList<BarcodeModel>>("Barcodes"), parameters.GetValue<string>("BatchId"));
                    break;
                case "SubmitCommandRecieverAsync":
                   await SubmitCommandRecieverAsync();
                    break;
                case "AssignValueToAddPalletAsync":
                    AssignValueToAddPalletAsync(parameters.GetValue<string>("AssignValueToAddPalletAsync"), parameters.GetValue<IList<BarcodeModel>>("BarcodesCollection"));
                    break;
            }
        }

        internal void AssignInitValue(INavigationParameters parameters)
        {
            try
            {
                AddPalletsTitle = parameters.GetValue<string>("AddPalletsTitle");
                ManifestId = parameters.GetValue<string>("ManifestId");
                BatchModel = parameters.GetValue<NewBatch>("NewBatchModel");
                SizeButtonTitle = parameters.GetValue<string>("SizeButtonTitle");
                PartnerModel = parameters.GetValue<PartnerModel>("PartnerModel");

                if (parameters.GetValue<IList<PalletModel>>("PalletCollection") != null)
                {
                    PalletCollection = new ObservableCollection<PalletModel>(parameters.GetValue<IList<PalletModel>>("PalletCollection"));
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("AddPalletsTitle"))
            {
                AssignInitValue(parameters);
            }

            return base.InitializeAsync(parameters);
        }

        public void Destroy()
        {
            _gpsListener.OnReadingReceived -= OnReadingReceived;
        }

        #endregion
    }
}
