﻿using KegID.Common;
using KegID.Delegates;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Newtonsoft.Json;
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
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillViewModel : BaseViewModel, IDestructible
    {
        #region Properties

        public IList<PalletModel> PalletCollection { get; private set; }

        private readonly IPageDialogService _dialogService;
        private readonly IManifestManager _manifestManager;
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;
        public Position LocationMessage { get; set; }
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
        public string SizeButtonTitle { get; set; } = "Select size";
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
        public IList<string> FillFromLocations { get; set; }
        public bool AllowMaintenanceFill { get; set; }

        #endregion

        #region Commands

        public DelegateCommand BatchCommand { get; }
        public DelegateCommand SizeCommand { get; }
        public DelegateCommand DestinationCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructor

        public FillViewModel(INavigationService navigationService, IPageDialogService dialogService, IManifestManager manifestManager, IGpsManager gpsManager, IGpsListener gpsListener) : base(navigationService)
        {
            _dialogService = dialogService;
            _manifestManager = manifestManager;
            _gpsManager = gpsManager;
            _gpsListener = gpsListener;
            _gpsListener.OnReadingReceived += OnReadingReceived;

            BatchCommand = new DelegateCommand(BatchCommandRecieverAsync);
            SizeCommand = new DelegateCommand(SizeCommandRecieverAsync);
            DestinationCommand = new DelegateCommand(DestinationCommandRecieverAsync);
            NextCommand = new DelegateCommand(NextCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);

            PreferenceSetting();
        }

        #endregion

        #region Methods

        void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            LocationMessage = e.Reading.Position;
        }

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            var preferenceUSER_HOME = preferences.Find(x => x.PreferenceName == "USER_HOME");
            DestinationTitle = preferenceUSER_HOME?.PreferenceValue ?? DestinationTitle;

            var preferenceUsesSkus = preferences.Find(x => x.PreferenceName == "UsesSkus");
            UsesSkus = preferenceUsesSkus != null && bool.Parse(preferenceUsesSkus.PreferenceValue);

            //In Fill--> Scan where we have to check while scanning where we can get default location?...
            var preferenceFillFromLocations = preferences.Find(x => x.PreferenceName == "FillFromLocations");
            FillFromLocations = preferenceFillFromLocations != null ? JsonConvert.DeserializeObject<IList<string>>(preferenceFillFromLocations.PreferenceValue) : null;

            var preferenceAllowMaintenanceFill = preferences.Find(x => x.PreferenceName == "AllowMaintenanceFill");
            AllowMaintenanceFill = preferenceAllowMaintenanceFill != null && bool.Parse(preferenceAllowMaintenanceFill.PreferenceValue);

            var preferenceOperator = preferences.Find(x => x.PreferenceName == "Operator");
            Operator = preferenceOperator != null && bool.Parse(preferenceOperator.PreferenceValue);
        }

        private async void CancelCommandRecieverAsync()
        {
            var result = await _dialogService.DisplayActionSheetAsync("Cancel? \n You have like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
            if (result == "Delete manifest")
            {
                // Delete an object with a transaction
                DeleteManifest(ManifestId);
                _ = await NavigationService.GoBackAsync(animated: false);
            }
            else
            {
                //Save Draft Manifest logic here...
                SaveDraftCommandRecieverAsync();
            }
        }

        private async void SaveDraftCommandRecieverAsync()
        {
            ManifestModel manifestModel = GenerateManifest(PalletCollection ?? new List<PalletModel>(), LocationMessage ?? new Position(0, 0));
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

                await NavigationService.NavigateAsync("ManifestsView",
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
            PalletCollection.Clear();

            AddPalletToFillScanMsg msg = new AddPalletToFillScanMsg
            {
                CleanUp = true
            };
            MessagingCenter.Send(msg, "AddPalletToFillScanMsg");
        }

        public ManifestModel GenerateManifest(IList<PalletModel> palletCollection, Position location)
        {
            _ = new List<string>();
            List<NewPallet> newPallets = new List<NewPallet>();
            List<ManifestTItem> palletItems = new List<ManifestTItem>();
            foreach (var pallet in palletCollection)
            {
                foreach (var item in pallet.Barcode)
                {
                    ManifestTItem palletItem = new ManifestTItem
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

                NewPallet newPallet = new NewPallet
                {
                    Barcode = pallet.ManifestId,
                    BuildDate = DateTimeOffset.UtcNow.Date,
                    StockLocation = ConstantManager.Partner.PartnerId,
                    StockLocationId = ConstantManager.Partner.PartnerId,
                    StockLocationName = ConstantManager.Partner.FullName,
                    OwnerId = Settings.CompanyId,
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
                        barcodeCollection: ConstantManager.Barcodes ?? new List<BarcodeModel>(), (long)location.Latitude, (long)location.Longitude, string.Empty, string.Empty, tags: new List<Tag>(), tagsStr: "",
                        partnerModel: ConstantManager.Partner, newPallets: newPallets ?? new List<NewPallet>(), batches: newBatches,
                        closedBatches: new List<string>(), null, validationStatus: 4, null, contents: SizeButtonTitle, size: SizeButtonTitle);
        }

        private void DeleteManifest(string manifestId)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var manifest = RealmDb.Find<ManifestModel>(manifestId);
            if (manifest != null)
            {
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.Remove(manifest);
                    trans.Commit();
                } 
            }
            ConstantManager.Barcodes?.Clear();
            PalletCollection?.Clear();
        }

        private async void NextCommandRecieverAsync()
        {
            if (!BatchButtonTitle.Contains("Select batch") && !DestinationTitle.Contains("Select destination"))
            {
                if (IsPalletze)
                {
                    await NavigationService.NavigateAsync("AddPalletsView", new NavigationParameters
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
                    await NavigationService.NavigateAsync("FillScanView", new NavigationParameters
                        {
                            { "IsPalletze",IsPalletze},
                            { "Title","Filling " + SizeButtonTitle + " kegs with " + BatchButtonTitle + " " + DestinationTitle},
                            { "NewBatchModel",NewBatchModel },
                            { "PartnerModel",PartnerModel },
                            { "SizeButtonTitle",SizeButtonTitle },
                            { "ManifestId",ManifestId },
                            { "Barcodes",ConstantManager.Barcodes},
                            { "FillFromLocations",FillFromLocations},
                            { "AllowMaintenanceFill",AllowMaintenanceFill }
                        }, animated: false);
                }
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Batch and destination is required please select it.", "Ok");
            }
        }

        private async void DestinationCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("PartnersView", new NavigationParameters
                    {
                        { "BrewerStockOn", true }
                    }, animated: false);
        }

        private async void BatchCommandRecieverAsync()
        {
            if (UsesSkus)
            {
                await NavigationService.NavigateAsync("SKUView", animated: false);
            }
            else
            {
                await NavigationService.NavigateAsync("BatchView", animated: false);
            }
        }

        private async void SizeCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("SizeView", animated: false);
        }

        private void AssignInitialValue(ManifestModel manifestModel)
        {
            NewBatchModel = manifestModel?.NewBatches.FirstOrDefault();
            SizeButtonTitle = manifestModel?.Size;
            DestinationTitle = manifestModel?.OwnerName;
            ManifestId = manifestModel?.ManifestId;
            ConstantManager.Barcodes = manifestModel?.BarcodeModels;

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

        public override async void OnNavigatedTo(INavigationParameters parameters)
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

        public void Destroy()
        {
            _gpsListener.OnReadingReceived -= OnReadingReceived;
        }

        #endregion
    }
}
