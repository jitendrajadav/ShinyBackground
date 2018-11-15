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
    public class FillScanReviewViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IUuidManager _uuidManager;
        private readonly IMoveService _moveService;
        private readonly IManifestManager _manifestManager;
        private readonly IGeolocationService _geolocationService;

        public IList<BarcodeModel> Barcodes { get; set; }
        public string BatchId { get; set; }


        #region TrackingNumber

        /// <summary>
        /// The <see cref="TrackingNumber" /> property's name.
        /// </summary>
        public const string TrackingNumberPropertyName = "TrackingNumber";

        private string _TrackingNumber = default;

        /// <summary>
        /// Sets and gets the TrackingNumber property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TrackingNumber
        {
            get
            {
                return _TrackingNumber;
            }

            set
            {
                if (_TrackingNumber == value)
                {
                    return;
                }

                _TrackingNumber = value;
                RaisePropertyChanged(TrackingNumberPropertyName);
            }
        }

        #endregion

        #region ManifestTo

        /// <summary>
        /// The <see cref="ManifestTo" /> property's name.
        /// </summary>
        public const string ManifestToPropertyName = "ManifestTo";

        private string _ManifestTo = default;

        /// <summary>
        /// Sets and gets the ManifestTo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ManifestTo
        {
            get
            {
                return _ManifestTo;
            }

            set
            {
                if (_ManifestTo == value)
                {
                    return;
                }

                _ManifestTo = value;
                RaisePropertyChanged(ManifestToPropertyName);
            }
        }

        #endregion

        #region ItemCount

        /// <summary>
        /// The <see cref="ItemCount" /> property's name.
        /// </summary>
        public const string ItemCountPropertyName = "ItemCount";

        private int _ItemCount = 0;

        /// <summary>
        /// Sets and gets the ItemCount property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ItemCount
        {
            get
            {
                return _ItemCount;
            }

            set
            {
                if (_ItemCount == value)
                {
                    return;
                }

                _ItemCount = value;
                RaisePropertyChanged(ItemCountPropertyName);
            }
        }

        #endregion

        #region Contents

        /// <summary>
        /// The <see cref="Contents" /> property's name.
        /// </summary>
        public const string ContentsPropertyName = "Contents";

        private string _Contents = "No contents";

        /// <summary>
        /// Sets and gets the Contents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Contents
        {
            get
            {
                return _Contents;
            }

            set
            {
                if (_Contents == value)
                {
                    return;
                }

                _Contents = value;
                RaisePropertyChanged(ContentsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand ScanCommand { get;}
        public DelegateCommand SubmitCommand { get; }

        #endregion

        #region Constructor

        public FillScanReviewViewModel(INavigationService navigationService, IUuidManager uuidManager, IPageDialogService dialogService, IMoveService moveService, IManifestManager manifestManager, IGeolocationService geolocationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
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
            var location = await _geolocationService.OnGetCurrentLocationAsync();
            if (location != null)
            {
                var barcodes = ConstantManager.Barcodes;
                var tags = ConstantManager.Tags;
                var partnerModel = ConstantManager.Partner;

                if (Barcodes.Count() == 0)
                {
                    await _dialogService.DisplayAlertAsync("Error", "Error: Please add some scans.", "Ok");
                    return;
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

                Loader.StartLoading();

                ManifestModel model = model = GenerateManifest(location);
                if (model != null)
                {
                    try
                    {
                        ManifestModelGet manifestResult = await _moveService.PostManifestAsync(model, AppSettings.SessionId, Configuration.NewManifest);
                        if (manifestResult.ManifestId != null)
                        {
                            try
                            {
                                model.IsDraft = false;
                                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                                RealmDb.Write(() =>
                                {
                                    RealmDb.Add(model, update: true);
                                });
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }

                            var manifest = await _moveService.GetManifestAsync(AppSettings.SessionId, manifestResult.ManifestId);
                            string contents = string.Empty;
                            try
                            {
                                contents = ConstantManager.Tags.Count > 2 ? ConstantManager.Tags?[2]?.Value ?? string.Empty : string.Empty;
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
                            if (string.IsNullOrEmpty(contents))
                            {
                                try
                                {
                                    contents = ConstantManager.Tags.Count > 3 ? ConstantManager.Tags?[3]?.Value ?? string.Empty : string.Empty;
                                }
                                catch (Exception ex)
                                {
                                    Crashes.TrackError(ex);
                                }
                            }
                            if (manifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                            {
                                Loader.StopLoading();
                                await _navigationService.NavigateAsync(new Uri("ManifestDetailView", UriKind.Relative), new NavigationParameters
                            {
                                { "manifest", manifest },{ "Contents", contents }
                            }, useModalNavigation: true, animated: false);
                            }
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
                        Cleanup();
                    }
                }
                else
                    await _dialogService.DisplayAlertAsync("Alert", "Something goes wrong please check again", "Ok");
            }
            //else
            //{
            //    await _dialogService.DisplayAlertAsync("Alert", "Something goes wrong please check again", "Ok");
            //}
        }

        public ManifestModel GenerateManifest(Xamarin.Essentials.Location location)
        {
            return _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.FILL_MANIFEST, manifestId: TrackingNumber,
                        barcodeCollection: ConstantManager.Barcodes ?? new List<BarcodeModel>(), (long)location.Latitude, (long)location.Longitude, tags:  new List<Tag>(), tagsStr: default,
                        partnerModel: ConstantManager.Partner, newPallets: new List<NewPallet>(), batches: new List<NewBatch>(),
                        closedBatches: new List<string>(),null, validationStatus: 4, contents: Contents);
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
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
                Contents = !string.IsNullOrEmpty(content) ? content : "No contens";
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
