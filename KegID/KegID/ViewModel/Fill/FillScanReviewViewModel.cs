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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanReviewViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IUuidManager _uuidManager;
        private readonly IMoveService _moveService;
        private readonly IManifestManager _manifestManager;
        private readonly IGeolocationService _geolocationService;

        public IList<BarcodeModel> Barcodes { get; set; }
        public string BatchId { get; set; }
        public string TrackingNumber { get; set; }
        public string ManifestTo { get; set; }
        public int ItemCount { get; set; }
        public string Contents { get; set; }

        #endregion

        #region Commands

        public DelegateCommand ScanCommand { get;}
        public DelegateCommand SubmitCommand { get; }

        #endregion

        #region Constructor

        public FillScanReviewViewModel(INavigationService navigationService, IUuidManager uuidManager, IPageDialogService dialogService, IMoveService moveService, IManifestManager manifestManager, IGeolocationService geolocationService) : base(navigationService)
        {
            _uuidManager = uuidManager;
            _dialogService = dialogService;
            _moveService = moveService;
            _manifestManager = manifestManager;
            _geolocationService = geolocationService;

            ScanCommand = new DelegateCommand(ScanCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SubmitCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();

                var location = await _geolocationService.GetLastLocationAsync();

                Loader.StopLoading();

                    var tags = ConstantManager.Tags;
                    var partnerModel = ConstantManager.Partner;

                    if (Barcodes.Count() == 0)
                    {
                        await _dialogService.DisplayAlertAsync("Error", "Error: Please add some scans.", "Ok");
                        return;
                    }

                    IEnumerable<BarcodeModel> empty = Barcodes.Where(x => x.Barcode.Count() == 0);
                    if (empty.ToList().Count > 0)
                    {
                        string result = await _dialogService.DisplayActionSheetAsync("Error? \n Some pallets have 0 scans. Do you want to edit them or remove the empty pallets.", null, null, "Remove empties", "Edit");
                        if (result == "Remove empties")
                        {
                            foreach (var item in empty.Reverse())
                            {
                                Barcodes.Remove(item);
                            }
                            if (Barcodes.Count == 0)
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
                    List<TItem> palletItems = new List<TItem>();
                    TItem palletItem = null;

                    foreach (var pallet in Barcodes)
                    {
                        palletItem = new TItem
                        {
                            Barcode = pallet.Barcode,
                            ScanDate = DateTimeOffset.UtcNow.Date,
                            TagsStr = pallet.TagsStr
                        };

                        if (pallet.Tags != null)
                        {
                            foreach (var tag in pallet.Tags)
                            {
                                palletItem.Tags.Add(tag);
                            }
                        }
                        palletItems.Add(palletItem);

                        newPallet = new NewPallet
                        {
                            Barcode = BatchId,
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

                    bool accept = await _dialogService.DisplayAlertAsync("Close batch", "Mark this batch as completed?", "Yes", "No");
                    if (accept)
                        closedBatches = Barcodes.Select(x => x.Barcode).ToList();

                    Loader.StartLoading();
                    ManifestModel model = model = GenerateManifest(location??new Xamarin.Essentials.Location(0,0), newPallets, closedBatches);
                    if (model != null)
                    {
                        try
                        {
                            var current = Connectivity.NetworkAccess;
                            if (current == NetworkAccess.Internet)
                            {
                                ManifestModelGet manifestResult = await _moveService.PostManifestAsync(model, AppSettings.SessionId, Configuration.NewManifest);
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
                            Loader.StopLoading();
                            model = null;
                            tags = null;
                            partnerModel = null;
                            closedBatches = null;
                            newPallets = null;
                            newPallet = null;
                            palletItems = null;
                            Cleanup();
                        }
                    }
                    else
                        await _dialogService.DisplayAlertAsync("Alert", "Something goes wrong please check again", "Ok");
                
            }
            catch (Exception)
            {
            }
            finally
            {
                Loader.StopLoading();
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
                manifest.TrackingNumber = TrackingNumber;
                manifest.ShipDate = DateTimeOffset.UtcNow.Date.ToShortDateString();
                manifest.CreatorCompany = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
                manifest.SenderPartner = new CreatorCompany { Address = ConstantManager.Partner.Address/*, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey */};
                manifest.ReceiverPartner = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
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

        public ManifestModel GenerateManifest(Xamarin.Essentials.Location location, List<NewPallet> newPallets, List<string> closedBatches)
        {
            return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.FILL_MANIFEST, manifestId: TrackingNumber,
                        Barcodes, (long)location.Latitude, (long)location.Longitude, tags:  new List<Tag>(), tagsStr: default,
                        partnerModel: ConstantManager.Partner, newPallets, batches: new List<NewBatch>(),
                        closedBatches, null, validationStatus: 4, contents: Contents);
        }

        private async Task ItemTappedCommandRecieverAsync(BarcodeModel model)
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

        public void Cleanup()
        {
            try
            {
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

        private async void ScanCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        internal void AssignInitialValue(INavigationParameters parameters)
        {
            try
            {
                Barcodes = parameters.GetValue<IList<BarcodeModel>>("BarcodeCollection");
                BatchId = parameters.GetValue<string>("BatchId");

                var partner = ConstantManager.Partner;
                var content = "";

                TrackingNumber = _uuidManager.GetUuId();
                ManifestTo = partner.FullName + "\n" + partner.PartnerTypeCode;
                ItemCount = Barcodes.Count;
                Contents = !string.IsNullOrEmpty(content) ? content : "No contents";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BatchId"))
            {
                AssignInitialValue(parameters);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ScanCommandRecieverAsync"))
            {
                ScanCommandRecieverAsync();
            }
        }

        #endregion
    }
}
