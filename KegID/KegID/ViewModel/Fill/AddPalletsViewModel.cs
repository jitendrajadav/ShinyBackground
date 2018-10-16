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

namespace KegID.ViewModel
{
    public class AddPalletsViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IPalletizeService _palletizeService;
        private readonly IMoveService _moveService;
        private readonly IManifestManager _manifestManager;
        private readonly IUuidManager _uuidManager;
        public string ManifestId { get; set; }
        public NewBatch BatchModel { get; private set; }
        public string SizeButtonTitle { get; private set; }
        public PartnerModel PartnerModel { get; private set; }

        #region AddPalletsTitle

        /// <summary>
        /// The <see cref="AddPalletsTitle" /> property's name.
        /// </summary>
        public const string AddPalletsTitlePropertyName = "AddPalletsTitle";

        private string _AddPalletsTitle = default;

        /// <summary>
        /// Sets and gets the AddPalletsTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddPalletsTitle
        {
            get
            {
                return _AddPalletsTitle;
            }

            set
            {
                if (_AddPalletsTitle == value)
                {
                    return;
                }

                _AddPalletsTitle = value;
                RaisePropertyChanged(AddPalletsTitlePropertyName);
            }
        }

        #endregion

        #region Pallets

        /// <summary>
        /// The <see cref="Pallets" /> property's name.
        /// </summary>
        public const string PalletsPropertyName = "Pallets";

        private string _Pallets = default;

        /// <summary>
        /// Sets and gets the Pallets property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Pallets
        {
            get
            {
                return _Pallets;
            }

            set
            {
                if (_Pallets == value)
                {
                    return;
                }

                _Pallets = value;
                RaisePropertyChanged(PalletsPropertyName);
            }
        }

        #endregion

        #region PalletCollection

        /// <summary>
        /// The <see cref="PalletCollection" /> property's name.
        /// </summary>
        public const string PalletCollectionPropertyName = "PalletCollection";

        private ObservableCollection<PalletModel> _PalletCollection = new ObservableCollection<PalletModel>();

        /// <summary>
        /// Sets and gets the PalletCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<PalletModel> PalletCollection
        {
            get
            {
                return _PalletCollection;
            }

            set
            {
                if (_PalletCollection == value)
                {
                    return;
                }

                _PalletCollection = value;
                RaisePropertyChanged(PalletCollectionPropertyName);
            }
        }

        #endregion

        #region Kegs

        /// <summary>
        /// The <see cref="Kegs" /> property's name.
        /// </summary>
        public const string KegsPropertyName = "Kegs";

        private string _Kegs = default;

        /// <summary>
        /// Sets and gets the Kegs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Kegs
        {
            get
            {
                return _Kegs;
            }

            set
            {
                if (_Kegs == value)
                {
                    return;
                }

                _Kegs = value;
                RaisePropertyChanged(KegsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand FillScanCommand { get; }
        public DelegateCommand FillKegsCommand { get; }
        public DelegateCommand<PalletModel> ItemTappedCommand { get; }
        public DelegateCommand<PalletModel> DeleteItemCommand { get; }

        #endregion

        #region Constructor

        public AddPalletsViewModel(IPalletizeService palletizeService, IMoveService moveService, INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager, IUuidManager uuidManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _palletizeService = palletizeService;
            _moveService = moveService;
            _manifestManager = manifestManager;
            _uuidManager = uuidManager;

            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
            FillScanCommand = new DelegateCommand(FillScanCommandRecieverAsync);
            FillKegsCommand = new DelegateCommand(FillKegsCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<PalletModel>(async (model) => await ItemTappedCommandRecieverAsync(model));
            DeleteItemCommand = new DelegateCommand<PalletModel>((model) => DeleteItemCommandReciever(model));
        }

        #endregion

        #region Methods

        private void DeleteItemCommandReciever(PalletModel model)
        {
            PalletCollection.Remove(model);
            CountKegs();
        }

        private async Task ItemTappedCommandRecieverAsync(PalletModel model)
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("FillScanView", UriKind.Relative), new NavigationParameters
                    {
                        { "model", model }
                    }, useModalNavigation: true, animated: false);
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
                await _navigationService.GoBackAsync(new NavigationParameters { { "PalletCollection", PalletCollection } },useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void SubmitCommandRecieverAsync()
        {
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
            List<TItem> palletItems = new List<TItem>();
            TItem palletItem = null;

            foreach (var pallet in PalletCollection)
            {
                foreach (var item in pallet.Barcode)
                {
                    palletItem = new TItem
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
            bool accept = await _dialogService.DisplayAlertAsync("Close batch", "Mark this batch as completed?", "Yes", "No");
            if (accept)
                closedBatches = PalletCollection.Select(x => x.ManifestId).ToList();

            Loader.StartLoading();

            ManifestModel model = null;
            try
            {
                model = _manifestManager.GetManifestDraft(EventTypeEnum.FILL_MANIFEST, ManifestId ?? PalletCollection.FirstOrDefault().ManifestId,
                barcodes, tags, string.Empty, partnerModel, newPallets, new List<NewBatch>(), closedBatches, 4);

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
                        var manifestPostModel = await _moveService.PostManifestAsync(model, AppSettings.SessionId, Configuration.NewManifest);

                        #region Old Code

                        //Making call for getting detail of Posted Manifest now Offline we dont need it.
                        //if (manifestResult.ManifestId != null)
                        //{
                        //    try
                        //    {
                        //        model.IsDraft = false;
                        //        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        //        RealmDb.Write(() =>
                        //        {
                        //            RealmDb.Add(model, update: true);
                        //        });
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Crashes.TrackError(ex);
                        //    }

                        //    var manifest = await _moveService.GetManifestAsync(AppSettings.SessionId, manifestResult.ManifestId);

                        #endregion

                        try
                        {
                            await AddorUpdateManifestOffline(model, false);
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
                            await AddorUpdateManifestOffline(model, true);
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

            await _navigationService.NavigateAsync(new Uri("ManifestDetailView", UriKind.Relative), new NavigationParameters
                                {
                                    { "manifest", manifest },{ "Contents", Contents }
                                }, useModalNavigation: true, animated: false);
        }

        private static async Task AddorUpdateManifestOffline(ManifestModel manifestPostModel, bool queue)
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
                await RealmDb.WriteAsync((realmDb) =>
                {
                    realmDb.Add(manifestPostModel);
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
                await _navigationService.NavigateAsync(new Uri("FillScanView", UriKind.Relative), new NavigationParameters
                    {
                        { "GenerateManifestIdAsync", "GenerateManifestIdAsync" },
                        {"PartnerModel",PartnerModel },
                            {"NewBatchModel",BatchModel },
                            {"SizeButtonTitle",SizeButtonTitle }
                    }, useModalNavigation: true, animated: false);
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
                if (!PalletCollection.Any(x => x.BatchId == _batchId))
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
                Kegs = string.Format("({0} Kegs)", PalletCollection.Sum(x => x.Count));
            else
                Kegs = string.Format("({0} Keg)", PalletCollection.Sum(x => x.Count));
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
                    Kegs = string.Format("({0} Kegs)", PalletCollection.Sum(x => x.Count));
                else
                    Kegs = string.Format("({0} Keg)", PalletCollection.Sum(x => x.Count));
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("FillKegsCommandRecieverAsync"))
            {
                FillKegsCommandRecieverAsync();
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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "Barcodes":
                    AssignFillScanValue(parameters.GetValue<IList<BarcodeModel>>("Barcodes"), parameters.GetValue<string>("BatchId"));
                    break;
                case "AddPalletsTitle":
                    AssignInitValue(parameters);
                    break;
                case "SubmitCommandRecieverAsync":
                    SubmitCommandRecieverAsync();
                    break;
                case "AssignValueToAddPalletAsync":
                    AssignValueToAddPalletAsync(parameters.GetValue<string>("AssignValueToAddPalletAsync"), parameters.GetValue<IList<BarcodeModel>>("BarcodesCollection"));
                    break;
                default:
                    break;
            }
        }

        #endregion
    }

}
