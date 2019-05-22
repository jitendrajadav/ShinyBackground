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
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillViewModel : BaseViewModel
    {
        #region Properties
        public IList<PalletModel> PalletCollection { get; private set; }

        //private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IUuidManager _uuidManager;
        private readonly IManifestManager _manifestManager;
        private readonly IGeolocationService _geolocationService;


        #region ManifestId

        /// <summary>
        /// The <see cref="ManifestId" /> property's name.
        /// </summary>
        public const string ManifestIdPropertyName = "ManifestId";

        private string _ManifestId = default;

        /// <summary>
        /// Sets and gets the ManifestId property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestId
        {
            get
            {
                return _ManifestId;
            }

            set
            {
                if (_ManifestId == value)
                {
                    return;
                }

                _ManifestId = value;
                ConstantManager.ManifestId = value;
                RaisePropertyChanged(ManifestIdPropertyName);
            }
        }

        #endregion

        #region NewBatchModel

        /// <summary>
        /// The <see cref="NewBatchModel" /> property's name.
        /// </summary>
        public const string NewBatchModelPropertyName = "NewBatchModel";

        private NewBatch _NewBatchModel = new NewBatch();

        /// <summary>
        /// Sets and gets the NewBatchModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NewBatch NewBatchModel
        {
            get
            {
                return _NewBatchModel;
            }

            set
            {
                if (_NewBatchModel == value)
                {
                    return;
                }

                _NewBatchModel = value;
                BatchButtonTitle = _NewBatchModel.BrandName + "-" + _NewBatchModel.BatchCode;
                IsRequiredVisible = false;
                RaisePropertyChanged(NewBatchModelPropertyName);
            }
        }

        #endregion

        #region BatchButtonTitle

        /// <summary>
        /// The <see cref="BatchButtonTitle" /> property's name.
        /// </summary>
        public const string BatchButtonTitlePropertyName = "BatchButtonTitle";

        private string _BatchButtonTitle = "Select batch";

        /// <summary>
        /// Sets and gets the BatchButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BatchButtonTitle
        {
            get
            {
                return _BatchButtonTitle;
            }

            set
            {
                if (_BatchButtonTitle == value)
                {
                    return;
                }

                _BatchButtonTitle = value;
                RaisePropertyChanged(BatchButtonTitlePropertyName);
            }
        }

        #endregion

        #region SizeButtonTitle

        /// <summary>
        /// The <see cref="SizeButtonTitle" /> property's name.
        /// </summary>
        public const string SizeButtonTitlePropertyName = "SizeButtonTitle";

        private string _SizeButtonTitle = "Select size";

        /// <summary>
        /// Sets and gets the SizeButtonTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SizeButtonTitle
        {
            get
            {
                return _SizeButtonTitle;
            }

            set
            {
                if (_SizeButtonTitle == value)
                {
                    return;
                }

                _SizeButtonTitle = value;
                RaisePropertyChanged(SizeButtonTitlePropertyName);
            }
        }

        #endregion

        #region DestinationTitle

        /// <summary>
        /// The <see cref="DestinationTitle" /> property's name.
        /// </summary>
        public const string DestinationTitlePropertyName = "DestinationTitle";

        private string _DestinationTitle = "Select destination";

        /// <summary>
        /// Sets and gets the DestinationTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DestinationTitle
        {
            get
            {
                return _DestinationTitle;
            }

            set
            {
                if (_DestinationTitle == value)
                {
                    return;
                }

                _DestinationTitle = value;
                IsDestinationRequiredVisible = false;
                RaisePropertyChanged(DestinationTitlePropertyName);
            }
        }

        #endregion

        #region PartnerModel

        /// <summary>
        /// The <see cref="PartnerModel" /> property's name.
        /// </summary>
        public const string PartnerModelPropertyName = "PartnerModel";

        private PartnerModel _PartnerModel = new PartnerModel();

        /// <summary>
        /// Sets and gets the PartnerModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel PartnerModel
        {
            get
            {
                return _PartnerModel;
            }

            set
            {
                if (_PartnerModel == value)
                {
                    return;
                }

                _PartnerModel = value;
                DestinationTitle = _PartnerModel.FullName;
                ConstantManager.Partner = _PartnerModel;
                RaisePropertyChanged(PartnerModelPropertyName);
            }
        }

        #endregion

        #region IsPalletze

        /// <summary>
        /// The <see cref="IsPalletze" /> property's name.
        /// </summary>
        public const string IsPalletzePropertyName = "IsPalletze";

        private bool _IsPalletze = true;

        /// <summary>
        /// Sets and gets the IsPalletze property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPalletze
        {
            get
            {
                return _IsPalletze;
            }

            set
            {
                if (_IsPalletze == value)
                {
                    return;
                }

                _IsPalletze = value;
                RaisePropertyChanged(IsPalletzePropertyName);
            }
        }

        #endregion

        #region IsRequiredVisible

        /// <summary>
        /// The <see cref="IsRequiredVisible" /> property's name.
        /// </summary>
        public const string IsRequiredVisiblePropertyName = "IsRequiredVisible";

        private bool _IsRequiredVisible = true;

        /// <summary>
        /// Sets and gets the IsRequiredVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsRequiredVisible
        {
            get
            {
                return _IsRequiredVisible;
            }

            set
            {
                if (_IsRequiredVisible == value)
                {
                    return;
                }

                _IsRequiredVisible = value;
                RaisePropertyChanged(IsRequiredVisiblePropertyName);
            }
        }

        #endregion

        #region IsDestinationRequiredVisible

        /// <summary>
        /// The <see cref="IsDestinationRequiredVisible" /> property's name.
        /// </summary>
        public const string IsDestinationRequiredVisiblePropertyName = "IsDestinationRequiredVisible";

        private bool _IsDestinationRequiredVisible = true;

        /// <summary>
        /// Sets and gets the IsDestinationRequiredVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsDestinationRequiredVisible
        {
            get
            {
                return _IsDestinationRequiredVisible;
            }

            set
            {
                if (_IsDestinationRequiredVisible == value)
                {
                    return;
                }

                _IsDestinationRequiredVisible = value;
                RaisePropertyChanged(IsDestinationRequiredVisiblePropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand BatchCommand { get; }
        public DelegateCommand SizeCommand { get;}
        public DelegateCommand DestinationCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand CancelCommand { get; }

        #endregion

        #region Constructor

        public FillViewModel(INavigationService navigationService, IPageDialogService dialogService, IUuidManager uuidManager, IManifestManager manifestManager, IGeolocationService geolocationService) : base(navigationService)
        {
            //_navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _uuidManager = uuidManager;
            _manifestManager = manifestManager;
            _geolocationService = geolocationService;

            BatchCommand = new DelegateCommand(BatchCommandRecieverAsync);
            SizeCommand = new DelegateCommand(SizeCommandRecieverAsync);
            DestinationCommand = new DelegateCommand(DestinationCommandRecieverAsync);
            NextCommand = new DelegateCommand(NextCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                var result = await _dialogService.DisplayActionSheetAsync("Cancel? \n You have like to save this manifest as a draft or delete?", null, null, "Delete manifest", "Save as draft");
                if (result == "Delete manifest")
                {
                    // Delete an object with a transaction
                    DeleteManifest(ManifestId);
                    await _navigationService.GoBackAsync(animated: false);
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

            ManifestModel manifestModel = null;


            manifestModel = GenerateManifest(PalletCollection ?? new List<PalletModel>(), location ?? new Xamarin.Essentials.Location(0, 0));
            if (manifestModel != null)
            {

                manifestModel.IsDraft = true;
                var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestModel.ManifestId);
                if (isNew != null)
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestModel, update: true);
                    });

                }
                else
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestModel);
                    });
                }

                await _navigationService.NavigateAsync("ManifestsView",
                    new NavigationParameters
                    {
                                    { "LoadDraftManifestAsync", "LoadDraftManifestAsync" }
                    }, animated: false);
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
            List<string> closedBatches = new List<string>();
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
                        closedBatches: new List<string>(), null, validationStatus: 4, contents: SizeButtonTitle, size: SizeButtonTitle);
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
            await _navigationService.NavigateAsync("BatchView", animated: false);
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
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "UuId":
                    ManifestId = parameters.GetValue<string>("UuId");
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
                default:
                    break;
            }
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
