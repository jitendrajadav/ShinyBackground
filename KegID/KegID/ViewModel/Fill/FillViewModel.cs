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
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillViewModel : BaseViewModel
    {
        #region Properties

        public IList<PalletModel> PalletCollection { get; private set; }

        private readonly IPageDialogService _dialogService;
        private readonly IManifestManager _manifestManager;
        private readonly IGeolocationService _geolocationService;

        public string ManifestId { get; set; }

        public void OnManifestIdChanged()
        {
            ConstantManager.ManifestId = ManifestId;
        }

        public NewBatch NewBatchModel { get; set; } = new NewBatch();

        public void OnNewBatchModelChanged()
        {
            BatchButtonTitle = NewBatchModel.BrandName + "-" + NewBatchModel.BatchCode;
            IsRequiredVisible = false;
        }
        public Sku Sku { get; set; }
        public void OnSkuChanged()
        {
            BatchButtonTitle = Sku.AssetProfileName;
            IsRequiredVisible = false;
        }
        public string BatchButtonTitle { get; set; } = "Select batch";
        public string SizeButtonTitle { get; set; } =  "Select size";
        public string DestinationTitle { get; set; } = "Select destination";
        public void OnDestinationTitleChanged()
        {
            IsDestinationRequiredVisible = false;
        }

        public PartnerModel PartnerModel { get; set; } = new PartnerModel();
        public void OnPartnerModelChanged()
        {
            DestinationTitle = PartnerModel.FullName;
            ConstantManager.Partner = PartnerModel;
        }

        public bool IsPalletze { get; set; } = true;
        public bool IsRequiredVisible { get; set; } = true;
        public bool IsDestinationRequiredVisible { get; set; } = true;
        public bool Operator { get; set; }
        public bool UsesSkus { get; set; }

        #endregion

        #region Commands

        public DelegateCommand BatchCommand { get; }
        public DelegateCommand SizeCommand { get;}
        public DelegateCommand DestinationCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructor

        public FillViewModel(INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager, IGeolocationService geolocationService) : base(navigationService)
        {
            _dialogService = dialogService;
            _manifestManager = manifestManager;
            _geolocationService = geolocationService;

            BatchCommand = new DelegateCommand(BatchCommandRecieverAsync);
            SizeCommand = new DelegateCommand(SizeCommandRecieverAsync);
            DestinationCommand = new DelegateCommand(DestinationCommandRecieverAsync);
            NextCommand = new DelegateCommand(NextCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);

            PreferenceSetting();
        }

        #endregion

        #region Methods

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            var preferenceUSER_HOME = preferences.Find(x => x.PreferenceName == "USER_HOME");
            DestinationTitle = preferenceUSER_HOME?.PreferenceValue ?? DestinationTitle;

            var preferenceUsesSkus = preferences.Find(x => x.PreferenceName == "UsesSkus");
            UsesSkus = preferenceUsesSkus != null && bool.Parse(preferenceUsesSkus.PreferenceValue);

            var preferenceFillFromLocations = preferences.Find(x => x.PreferenceName == "FillFromLocations");
            var FillFromLocations = preferenceFillFromLocations != null && bool.Parse(preferenceFillFromLocations.PreferenceValue);

            var preferenceAllowMaintenanceFill = preferences.Find(x => x.PreferenceName == "AllowMaintenanceFill");
            var AllowMaintenanceFill = preferenceAllowMaintenanceFill != null && bool.Parse(preferenceAllowMaintenanceFill.PreferenceValue);

            var preferenceOperator = preferences.Find(x => x.PreferenceName == "Operator");
            Operator = preferenceOperator != null && bool.Parse(preferenceOperator.PreferenceValue);
        }

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                var result = await _dialogService.DisplayActionSheetAsync("Cancel? \n You have like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
                if (result == "Delete manifest")
                {
                    // Delete an object with a transaction
                    DeleteManifest(ManifestId);
                    _ = await _navigationService.GoBackAsync(animated: false);
                }
                else
                {
                    //Save Draft Manifest logic here...
                    SaveDraftCommandRecieverAsync();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SaveDraftCommandRecieverAsync()
        {
            var location = await _geolocationService.GetLastLocationAsync();

            ManifestModel manifestModel = GenerateManifest(PalletCollection ?? new List<PalletModel>(), location ?? new Xamarin.Essentials.Location(0, 0));
            if (manifestModel != null)
            {

                manifestModel.IsDraft = true;
                var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestModel.ManifestId);
                if (isNew != null)
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() => RealmDb.Add(manifestModel, update: true));
                }
                else
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() => RealmDb.Add(manifestModel));
                }

                await _navigationService.NavigateAsync("ManifestsView",
                    new NavigationParameters { { "LoadDraftManifestAsync", "LoadDraftManifestAsync" } }, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Could not save manifest.", "Ok");
            }

            manifestModel = null;
            Cleanup();
        }


        private void Cleanup()
        {
            try
            {
                PalletCollection.Clear();

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

        public ManifestModel GenerateManifest(IList<PalletModel> palletCollection, Xamarin.Essentials.Location location)
        {
            _ = new List<string>();
            List<NewPallet> newPallets = new List<NewPallet>();
            NewPallet newPallet = null;
            List<TItem> palletItems = new List<TItem>();
            TItem palletItem = null;

            foreach (var pallet in palletCollection)
            {
                foreach (var item in pallet.Barcode)
                {
                    palletItem = new TItem
                    {
                        Barcode = item.Barcode,
                        ScanDate = DateTimeOffset.UtcNow.Date,
                        Icon = item.Icon,
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
                    Barcode = pallet.ManifestId,
                    BuildDate = DateTimeOffset.UtcNow.Date,
                    StockLocation = ConstantManager.Partner.PartnerId,
                    StockLocationId = ConstantManager.Partner.PartnerId,
                    StockLocationName = ConstantManager.Partner.FullName,
                    OwnerId = AppSettings.CompanyId,
                    PalletId = pallet.BatchId,
                    ReferenceKey = "",
                    IsPalletze = IsPalletze
                };
                foreach (var item in palletItems)
                    newPallet.PalletItems.Add(item);
                newPallets.Add(newPallet);
            }

            List<NewBatch> newBatches = new List<NewBatch>
            {
                NewBatchModel,
            };

            return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.FILL_MANIFEST, manifestId: ManifestId,
                        barcodeCollection: ConstantManager.Barcodes ?? new List<BarcodeModel>(), (long)location.Latitude, (long)location.Longitude, tags: new List<Tag>(), tagsStr: "",
                        partnerModel: ConstantManager.Partner, newPallets: newPallets ?? new List<NewPallet>(), batches: newBatches,
                        closedBatches: new List<string>(), null, validationStatus: 4, null, contents: SizeButtonTitle, size: SizeButtonTitle);
        }

        private void DeleteManifest(string manifestId)
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var manifest = RealmDb.All<ManifestModel>().First(b => b.ManifestId == manifestId);
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.Remove(manifest);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            ConstantManager.Barcodes?.Clear();
            PalletCollection?.Clear();
        }

        private async void NextCommandRecieverAsync()
        {
            try
            {
                if (!BatchButtonTitle.Contains("Select batch") && !DestinationTitle.Contains("Select destination"))
                {
                    if (IsPalletze)
                    {
                        await _navigationService.NavigateAsync("AddPalletsView", new NavigationParameters
                        {
                            { "AddPalletsTitle", "Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + "\n" + DestinationTitle },
                            {"PalletCollection",PalletCollection },
                            {"ManifestId",ManifestId },
                            {"PartnerModel",PartnerModel },
                            {"NewBatchModel",NewBatchModel },
                            {"SizeButtonTitle",SizeButtonTitle }
                        }, animated: false);
                    }
                    else
                    {
                        await _navigationService.NavigateAsync("FillScanView", new NavigationParameters
                        {
                            { "IsPalletze",IsPalletze},
                            { "Title","Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + " " + DestinationTitle},
                            {"NewBatchModel",NewBatchModel },
                            {"PartnerModel",PartnerModel },
                            {"SizeButtonTitle",SizeButtonTitle },
                            {"ManifestId",ManifestId },
                            { "Barcodes",ConstantManager.Barcodes}
                        }, animated: false);
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error", "Batch and destination is required please select it.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void DestinationCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PartnersView", new NavigationParameters
                    {
                        { "BrewerStockOn", true }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BatchCommandRecieverAsync()
        {
            if (!UsesSkus)
            {
                await _navigationService.NavigateAsync("SKUView", animated: false);
            }
            else
            {
                await _navigationService.NavigateAsync("BatchView", animated: false);
            }
        }

        private async void SizeCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("SizeView", animated: false);
        }

        private void AssignInitialValue(ManifestModel manifestModel)
        {
            try
            {
                NewBatchModel = manifestModel?.NewBatches.FirstOrDefault();
                SizeButtonTitle = manifestModel?.Size;
                DestinationTitle = manifestModel?.OwnerName;
                ManifestId = manifestModel?.ManifestId;
                ConstantManager.Barcodes = manifestModel?.BarcodeModels;

                try
                {
                    PartnerModel partner = new PartnerModel()
                    {
                        PartnerId = manifestModel?.ReceiverId,
                        FullName = manifestModel?.OwnerName
                    };
                    ConstantManager.Partner = partner;
                    if (manifestModel.Tags != null)
                    {
                        ConstantManager.Tags = new List<Tag>();
                        foreach (var item in manifestModel.Tags)
                        {
                            ConstantManager.Tags.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                foreach (var item in manifestModel.NewPallets)
                {
                    PalletModel palletModel = new PalletModel
                    {
                        Count = item.PalletItems.Count,
                        ManifestId = ManifestId,
                        BatchId = item.PalletId
                    };
                    palletModel.Barcode = ConstantManager.Barcodes;
                    if (PalletCollection == null)
                    {
                        PalletCollection = new List<PalletModel>();
                    }
                    PalletCollection.Add(palletModel);
                    IsPalletze = item.IsPalletze;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "CancelCommandRecieverAsync":
                    CancelCommandRecieverAsync();
                    break;
                case "SKUModel":
                    Sku = parameters.GetValue<Sku>("SKUModel");
                    break;
                case "model":
                    PartnerModel = parameters.GetValue<PartnerModel>("model");
                    ConstantManager.Partner = PartnerModel;
                    break;
                case "BatchModel":
                    NewBatchModel = parameters.GetValue<NewBatch>("BatchModel");
                    break;
                case "SizeModel":
                    SizeButtonTitle = parameters.GetValue<string>("SizeModel");
                    break;
                case "NewBatchModel":
                    NewBatchModel = parameters.GetValue<NewBatch>("NewBatchModel");
                    break;
                case "PalletCollection":
                    PalletCollection = parameters.GetValue<IList<PalletModel>>("PalletCollection");
                    break;
                case "AssignInitialValue":
                    AssignInitialValue(parameters.GetValue<ManifestModel>("AssignInitialValue"));
                    break;
                case "BarcodeCollection":
                    AssingScanToFillView(parameters);
                    break;
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("UuId"))
            {
                ManifestId = parameters.GetValue<string>("UuId");
            }
            return base.InitializeAsync(parameters);
        }

        private void AssingScanToFillView(INavigationParameters parameters)
        {
            try
            {
                ConstantManager.Barcodes = parameters.GetValue<IList<BarcodeModel>>("BarcodeCollection");
                string BatchId = parameters.GetValue<string>("BatchId");
                string ManifestId = parameters.GetValue<string>("ManifestId");

                PalletCollection = new List<PalletModel>
                    {
                    new PalletModel
                        {
                              Barcode = ConstantManager.Barcodes,
                              BatchId =BatchId,
                              ManifestId = ManifestId,
                              Count = ConstantManager.Barcodes.Count
                        }
                    };
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
