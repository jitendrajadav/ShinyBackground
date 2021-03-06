﻿using Acr.UserDialogs;
using KegID.Common;
using KegID.Delegates;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using Shiny;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanReviewViewModel : BaseViewModel, IDestructible
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IUuidManager _uuidManager;
        private readonly IManifestManager _manifestManager;
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;

        public IList<BarcodeModel> Barcodes { get; set; }
        public string BatchId { get; set; }
        public string TrackingNumber { get; set; }
        public string ManifestTo { get; set; }
        public int ItemCount { get; set; }
        public string Contents { get; set; }
        public Position LocationMessage { get; set; }

        #endregion

        #region Commands

        public DelegateCommand ScanCommand { get; }
        public DelegateCommand SubmitCommand { get; }

        #endregion

        #region Constructor

        public FillScanReviewViewModel(INavigationService navigationService, IUuidManager uuidManager, IPageDialogService dialogService, IManifestManager manifestManager, IGpsManager gpsManager, IGpsListener gpsListener) : base(navigationService)
        {
            _uuidManager = uuidManager;
            _dialogService = dialogService;
            _manifestManager = manifestManager;
            _gpsManager = gpsManager;
            _gpsListener = gpsListener;
            _gpsListener.OnReadingReceived += OnReadingReceived;

            ScanCommand = new DelegateCommand(ScanCommandRecieverAsync);
            SubmitCommand = new DelegateCommand(async () => await RunSafe(SubmitCommandRecieverAsync()));
        }

        #endregion

        #region Methods

        void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            LocationMessage = e.Reading.Position;
        }

        private async Task SubmitCommandRecieverAsync()
        {
            UserDialogs.Instance.ShowLoading("Loading");

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
            List<ManifestTItem> palletItems = new List<ManifestTItem>();
            ManifestTItem palletItem = null;

            foreach (var pallet in Barcodes)
            {
                palletItem = new ManifestTItem
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
                    OwnerId = Settings.CompanyId,
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

            UserDialogs.Instance.ShowLoading("Loading");
            ManifestModel model = model = GenerateManifest(LocationMessage ?? new Shiny.Position(0, 0), newPallets, closedBatches);
            if (model != null)
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    System.Net.Http.HttpResponseMessage response = await ApiManager.PostManifest(model, Settings.SessionId);
                    AddorUpdateManifestOffline(model, false);
                    await GetPostedManifestDetail();
                }
                else
                {
                    AddorUpdateManifestOffline(model, true);
                    await GetPostedManifestDetail();
                }

                UserDialogs.Instance.HideLoading();
                model = null;
                tags = null;
                partnerModel = null;
                closedBatches = null;
                newPallets = null;
                newPallet = null;
                palletItems = null;
                Cleanup();
            }
            else
                await _dialogService.DisplayAlertAsync("Alert", "Something goes wrong please check again", "Ok");//TODO: clean up phrasing

            UserDialogs.Instance.HideLoading();
        }

        private async Task GetPostedManifestDetail()
        {
            ManifestResponseModel manifest = new ManifestResponseModel();
            string Contents = string.Empty;

            Contents = ConstantManager.Tags.Count > 2 ? ConstantManager.Tags?[2]?.Value ?? string.Empty : string.Empty;

            if (string.IsNullOrEmpty(Contents))
            {
                Contents = ConstantManager.Tags.Count > 3 ? ConstantManager.Tags?[3]?.Value ?? string.Empty : string.Empty;
            }

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
            manifest.CreatorCompany = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.ToString(), Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
            manifest.SenderPartner = new CreatorCompany { Address = ConstantManager.Partner.Address/*, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.HasValue ? ConstantManager.Partner.CompanyNo.Value.ToString() : string.Empty, Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey */};
            manifest.ReceiverPartner = new CreatorCompany { Address = ConstantManager.Partner.Address, State = ConstantManager.Partner.State, PostalCode = ConstantManager.Partner.PostalCode, Lon = ConstantManager.Partner.Lon, Address1 = ConstantManager.Partner.Address1, City = ConstantManager.Partner.City, CompanyNo = ConstantManager.Partner.CompanyNo.ToString(), Country = ConstantManager.Partner.Country, FullName = ConstantManager.Partner.FullName, IsActive = ConstantManager.Partner.IsActive, IsInternal = ConstantManager.Partner.IsInternal, IsShared = ConstantManager.Partner.IsShared, Lat = ConstantManager.Partner.Lat, LocationCode = ConstantManager.Partner.LocationCode, LocationStatus = ConstantManager.Partner.LocationStatus, MasterCompanyId = ConstantManager.Partner.MasterCompanyId, ParentPartnerId = ConstantManager.Partner.ParentPartnerId, ParentPartnerName = ConstantManager.Partner.ParentPartnerName, PartnerId = ConstantManager.Partner.PartnerId, PartnershipIsActive = ConstantManager.Partner.PartnershipIsActive, PartnerTypeCode = ConstantManager.Partner.PartnerTypeCode, PartnerTypeName = ConstantManager.Partner.PartnerTypeName, PhoneNumber = ConstantManager.Partner.PhoneNumber, SourceKey = ConstantManager.Partner.SourceKey };
            manifest.SenderShipAddress = new Address { City = ConstantManager.Partner.City, Country = ConstantManager.Partner.Country, Geocoded = false, Latitude = (long)ConstantManager.Partner.Lat, Line1 = ConstantManager.Partner.Address, Line2 = ConstantManager.Partner.Address1, Longitude = (long)ConstantManager.Partner.Lon, PostalCode = ConstantManager.Partner.PostalCode, State = ConstantManager.Partner.State };
            manifest.ReceiverShipAddress = new Address { City = ConstantManager.Partner.City, Country = ConstantManager.Partner.Country, Geocoded = false, Latitude = (long)ConstantManager.Partner.Lat, Line1 = ConstantManager.Partner.Address, Line2 = ConstantManager.Partner.Address1, Longitude = (long)ConstantManager.Partner.Lon, PostalCode = ConstantManager.Partner.PostalCode, State = ConstantManager.Partner.State };

            await NavigationService.NavigateAsync("ManifestDetailView", new NavigationParameters
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
                manifestPostModel.IsDraft = false;
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() => RealmDb.Add(manifestPostModel, update: true));

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

        public ManifestModel GenerateManifest(Shiny.Position location, List<NewPallet> newPallets, List<string> closedBatches)
        {
            return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.FILL_MANIFEST, manifestId: TrackingNumber,
                        Barcodes, (long)location.Latitude, (long)location.Longitude, string.Empty, string.Empty, tags: new List<Tag>(), tagsStr: default,
                        partnerModel: ConstantManager.Partner, newPallets, batches: new List<NewBatch>(),
                        closedBatches, null, validationStatus: 4, null, contents: Contents);
        }

        private async Task ItemTappedCommandRecieverAsync(BarcodeModel model)
        {
            await NavigationService.NavigateAsync("FillScanView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
        }

        public void Cleanup()
        {
            AddPalletToFillScanMsg msg = new AddPalletToFillScanMsg
            {
                CleanUp = true
            };
            MessagingCenter.Send(msg, "AddPalletToFillScanMsg");
        }

        private async void ScanCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
        }

        internal void AssignInitialValue(INavigationParameters parameters)
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

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BatchId"))
            {
                AssignInitialValue(parameters);
            }
            return base.InitializeAsync(parameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ScanCommandRecieverAsync"))
            {
                ScanCommandRecieverAsync();
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

        public void Destroy()
        {
            _gpsListener.OnReadingReceived -= OnReadingReceived;
        }

        #endregion
    }
}
