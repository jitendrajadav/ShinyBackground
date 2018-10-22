using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";
        private const string Maintenace = "maintenace.png";
        private const string ValidationOK = "validationok.png";

        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;
        private readonly IZebraPrinterManager _zebraPrinterManager;
        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly ICalcCheckDigitMngr _calcCheckDigitMngr;
        private readonly IPageDialogService _dialogService;

        public string BatchId { get; set; }
        public NewBatch BatchModel { get; private set; }
        public string SizeButtonTitle { get; private set; }
        public PartnerModel PartnerModel { get; private set; }
        public bool HasPrint { get; private set; }

        #region Title

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _Title = default;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }

            set
            {
                if (_Title == value)
                {
                    return;
                }

                _Title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        #endregion

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
                RaisePropertyChanged(ManifestIdPropertyName);
            }
        }

        #endregion

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

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _TagsStr = default;


        /// <summary>
        /// Sets and gets the TagsStr property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TagsStr
        {
            get
            {
                return _TagsStr;
            }

            set
            {
                if (_TagsStr == value)
                {
                    return;
                }

                _TagsStr = value;
                RaisePropertyChanged(TagsStrPropertyName);
            }
        }

        #endregion

        #region IsPalletVisible

        /// <summary>
        /// The <see cref="IsPalletVisible" /> property's name.
        /// </summary>
        public const string IsPalletVisiblePropertyName = "IsPalletVisible";

        private bool _IsPalletVisible = true;

        /// <summary>
        /// Sets and gets the IsPalletVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPalletVisible
        {
            get
            {
                return _IsPalletVisible;
            }

            set
            {
                if (_IsPalletVisible == value)
                {
                    return;
                }

                _IsPalletVisible = value;
                RaisePropertyChanged(IsPalletVisiblePropertyName);
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
      
        #region Tags
        /// <summary>
        /// The <see cref="Tags" /> property's name.
        /// </summary>
        public const string TagsPropertyName = "Tags";

        private List<Tag> _tags = new List<Tag>();

        /// <summary>
        /// Sets and gets the Tags property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<Tag> Tags
        {
            get
            {
                return _tags;
            }

            set
            {
                if (_tags == value)
                {
                    return;
                }

                _tags = value;
                RaisePropertyChanged(TagsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand PrintCommand { get; }
        public DelegateCommand IsPalletVisibleCommand { get;}
        public DelegateCommand SubmitCommand { get;}
        public DelegateCommand BarcodeManualCommand { get; }
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get;}
        public DelegateCommand<BarcodeModel> LabelItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> DeleteItemCommand { get; }
        
        #endregion

        #region Constructor

        public FillScanViewModel(IMoveService moveService, INavigationService navigationService, IZebraPrinterManager zebraPrinterManager, IGetIconByPlatform getIconByPlatform, ICalcCheckDigitMngr calcCheckDigitMngr, IPageDialogService dialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;
            _getIconByPlatform = getIconByPlatform;
            _zebraPrinterManager = zebraPrinterManager;
            _calcCheckDigitMngr = calcCheckDigitMngr;
            _dialogService = dialogService;

            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            PrintCommand = new DelegateCommand(PrintCommandRecieverAsync);
            IsPalletVisibleCommand = new DelegateCommand(IsPalletVisibleCommandReciever);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));
            DeleteItemCommand = new DelegateCommand<BarcodeModel>((model) => DeleteItemCommandReciever(model));

            HandleUnSubscirbeMessages();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void DeleteItemCommandReciever(BarcodeModel model)
        {
            BarcodeCollection.Remove(model);
        }

        private void HandleUnSubscirbeMessages()
        {
            MessagingCenter.Unsubscribe<FillScanMessage>(this, "FillScanMessage");
            MessagingCenter.Unsubscribe<AddPalletToFillScanMsg>(this, "AddPalletToFillScanMsg");
            MessagingCenter.Unsubscribe<CancelledMessage>(this, "CancelledMessage");
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<FillScanMessage>(this, "FillScanMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (message != null)
                    {
                        using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                        {
                            try
                            {
                                var oldBarcode = BarcodeCollection.Where(x => x.Barcode == message.Barcodes.Barcode).FirstOrDefault();
                                oldBarcode.Pallets = message?.Barcodes?.Pallets;
                                oldBarcode.Kegs = message?.Barcodes?.Kegs;
                                oldBarcode.Icon = message?.Barcodes?.Kegs?.Partners?.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : message?.Barcodes?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                                oldBarcode.IsScanned = true;
                                db.Commit();
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
                        };
                    }
                });
            });

            MessagingCenter.Subscribe<AddPalletToFillScanMsg>(this, "AddPalletToFillScanMsg", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Cleanup();
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

        internal void AssignInitValue(INavigationParameters parameters)
        {
            try
            {
                BatchModel = parameters.GetValue<NewBatch>("NewBatchModel");
                SizeButtonTitle = parameters.GetValue<string>("SizeButtonTitle");
                PartnerModel = parameters.GetValue<PartnerModel>("PartnerModel");
                IsPalletze = parameters.GetValue<bool>("IsPalletze");
                Title = parameters.GetValue<string>("Title");
                ManifestId = parameters.GetValue<string>("ManifestId");
                IList<BarcodeModel> collection = parameters.GetValue<IList<BarcodeModel>>("Barcodes");
                foreach (var item in collection)
                {
                    BarcodeCollection.Add(item);
                    TagsStr = item.TagsStr;
                }

                GenerateManifestIdAsync(null);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async Task AssignValidateBarcodeValueAsync()
        {
            try
            {
                try
                {
                    ConstantManager.Barcodes = BarcodeCollection;
                    ConstantManager.Tags = Tags;
                    var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
                    var page = formsNav.Navigation.ModalStack[formsNav.Navigation.ModalStack.Count-2];
                    (page?.BindingContext as INavigationAware)?.OnNavigatingTo(new NavigationParameters
                    {
                        { "Barcodes", BarcodeCollection },{ "BatchId", BatchId }
                    });
                }
                catch (Exception)
                {

                }
                await _navigationService.ClearPopupStackAsync(animated: false);
                if (IsPalletze)
                {
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                }
                else
                {
                    await _navigationService.NavigateAsync(new Uri("FillScanReviewView", UriKind.Relative), new NavigationParameters
                    {
                        { "BatchId", BatchId },{ "Count", BarcodeCollection.Count }
                    }, useModalNavigation: true, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void GenerateManifestIdAsync(PalletModel palletModel)
        {
            try
            {
                DateTimeOffset now = DateTimeOffset.Now;
                string barCode;
                long prefix = 0;
                var lastCharOfYear = now.Year.ToString().ToCharArray().LastOrDefault().ToString();
                var dayOfYear = now.DayOfYear;
                var secondsInDayTillNow = SecondsInDayTillNow();
                var millisecond = now.Millisecond;

                if (IsPalletze)
                {
                    if (palletModel != null)
                    {
                        BatchId = palletModel.BatchId;
                        Title = "Pallet #" + BatchId;
                        foreach (var item in palletModel.Barcode)
                        {
                            BarcodeCollection.Add(item);
                            TagsStr = item.TagsStr ?? string.Empty;
                        }
                    }
                    else
                    {
                        BarcodeCollection.Clear();

                        barCode = GenerateBatchId(ref prefix, lastCharOfYear, dayOfYear, secondsInDayTillNow, millisecond);
                    }
                }
                else
                {
                    barCode = GenerateBatchId(ref prefix, lastCharOfYear, dayOfYear, secondsInDayTillNow, millisecond);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private string GenerateBatchId(ref long prefix, string lastCharOfYear, int dayOfYear, int secondsInDayTillNow, int millisecond)
        {
            string barCode;
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preference = RealmDb.All<Preference>().Where(x => x.PreferenceName == "DashboardPreferences").ToList();

            foreach (var item in preference)
            {
                if (item.PreferenceValue.Contains("OldestKegs"))
                {
                    var preferenceValue = JsonConvert.DeserializeObject<PreferenceValueResponseModel>(item.PreferenceValue);
                    var value = preferenceValue.SelectedWidgets.Where(x => x.Id == "OldestKegs").FirstOrDefault();
                    prefix = value.Pos.Y;
                }
            }
            barCode = prefix.ToString().PadLeft(9, '0') + lastCharOfYear + dayOfYear + secondsInDayTillNow + (millisecond / 100);
            var checksumDigit = _calcCheckDigitMngr.CalculateCheckDigit(barCode);
            BatchId = barCode + checksumDigit;
            Title = "Pallet #" + BatchId;
            return barCode;
        }

        private static int SecondsInDayTillNow()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            int hours = 0, minutes = 0, seconds = 0, totalSeconds = 0;
            hours = (24 - now.Hour) - 1;
            minutes = (60 - now.Minute) - 1;
            seconds = (60 - now.Second - 1);

            return totalSeconds = seconds + (minutes * 60) + (hours * 3600);
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
                        {"viewTypeEnum",ViewTypeEnum.FillScanView },
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
                    if (model.Kegs.Partners?.FirstOrDefault()?.Kegs.FirstOrDefault().MaintenanceItems?.Count > 0)
                    {
                        string strAlert = string.Empty;
                        for (int i = 0; i < model.Kegs.MaintenanceItems.Count; i++)
                        {
                            strAlert += "-" + model.Kegs.MaintenanceItems[i].Name + "\n";
                            if (model.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().MaintenanceItems.Count == i)
                            {
                                break;
                            }
                        }
                        await _dialogService.DisplayAlertAsync("Warning", "This keg needs the following maintenance performed:\n" + strAlert, "Ok");
                    }
                    else
                    {
                        if (model.Icon == "validationerror.png")
                        {
                            bool accept = await _dialogService.DisplayAlertAsync("Warning", "This scan could not be verified", "Keep", "Delete");
                        }
                        else
                        {
                            await _navigationService.NavigateAsync(new Uri("ScanInfoView", UriKind.Relative), new NavigationParameters
                            {
                                { "model", model }
                            }, useModalNavigation: true, animated: false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task NavigateToValidatePartner(List<BarcodeModel> model)
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("ValidateBarcodeView", UriKind.Relative), new NavigationParameters
                    {
                        { "model", model }
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
                bool isNew = BarcodeCollection.ToList().Any(x => x.Barcode == ManaulBarcode);

                BarcodeModel model = null;
                    if(!isNew)
                {
                    try
                    {
                        model = new BarcodeModel
                        {
                            Barcode = ManaulBarcode,
                            TagsStr = TagsStr,
                            Icon = Cloud,
                            Page = ViewTypeEnum.FillScanView.ToString(),
                            Contents = Tags.Count > 2 ? Tags?[2]?.Name??string.Empty : string.Empty
                        };
                        if (ConstantManager.Tags != null)
                        {
                            foreach (var item in ConstantManager.Tags)
                                model.Tags.Add(item);
                        }
                        BarcodeCollection.Add(model);
                    }
                   
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        var message = new StartLongRunningTaskMessage
                        {
                            Barcode = new List<string>() { ManaulBarcode },
                            PageName = ViewTypeEnum.FillScanView.ToString()
                        };
                        MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                    }

                    #region Old Code
                    //else
                    //{
                    //    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    //    var manifestModel = RealmDb.Find<ManifestModel>(ConstantManager.ManifestId);
                    //    try
                    //    {
                    //        if (manifestModel != null)
                    //        {
                    //            await RealmDb.WriteAsync((realmDb) =>
                    //            {
                    //                manifestModel.BarcodeModels.Add(model);
                    //                realmDb.Add(manifestModel, true);
                    //            });
                    //        }
                    //        else
                    //        {
                    //            //ManifestModel manifestModel = _manifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST,
                    //            //                                manifestId: ConstantManager.ManifestId, barcodeCollection: BarcodeCollection.Where(t => t.IsScanned == false).ToList(), tags: ConstantManager.Tags,
                    //            //                                TagsStr, partnerModel: ConstantManager.Partner, newPallets: new List<NewPallet>(),
                    //            //                                batches: new List<NewBatch>(), closedBatches: new List<string>(), validationStatus: 2, contents: SelectedBrand?.BrandName);

                    //            manifestModel.IsQueue = true;
                    //            RealmDb.Write(() =>
                    //            {
                    //                RealmDb.Add(manifestModel, true);
                    //            });
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Crashes.TrackError(ex);
                    //    }
                    //} 
                    #endregion

                    ManaulBarcode = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SubmitCommandRecieverAsync()
        {
            try
            {
                var result = BarcodeCollection.Where(x => x.Kegs.Partners.Count > 1).ToList();
                if (result.Count > 0)
                    await NavigateToValidatePartner(result.ToList());
                else
                {
                    await _navigationService.NavigateAsync(new Uri("FillScanReviewView", UriKind.Relative), new NavigationParameters
                    {
                        { "BatchId", BatchId },{ "BarcodeCollection", BarcodeCollection }
                    }, useModalNavigation: true, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void IsPalletVisibleCommandReciever()
        {
            IsPalletVisible = !IsPalletVisible;
        }

        private async void PrintCommandRecieverAsync()
        {
            await ValidateBarcode();
        }

        private async Task ValidateBarcode()
        {
            HasPrint = true;
            List<BarcodeModel> alert = BarcodeCollection.Where(x => x.Icon == "maintenace.png").ToList();
            if (alert.Count > 0 && !alert.FirstOrDefault().HasMaintenaceVerified)
            {
                try
                {
                    string strBarcode = alert.FirstOrDefault().Barcode;
                    var option = await _dialogService.DisplayActionSheetAsync("No keg with a barcode of \n" + strBarcode + " could be found",
                        null, null, "Remove unverified scans", "Assign sizes", "Countinue with current scans", "Stay here");
                    switch (option)
                    {
                        case "Remove unverified scans":
                            BarcodeCollection.Remove(alert.FirstOrDefault());

                            new Task(new Action(() =>
                            {
                                PrintPallet();
                            }
                                )).Start();

                            await _navigationService.GoBackAsync(new NavigationParameters
                                    {
                                        { "AssignValueToAddPalletAsync", BatchId }, { "BarcodesCollection", BarcodeCollection },
                                    }, useModalNavigation: true, animated: false);
                            break;
                        case "Assign sizes":
                            var param = new NavigationParameters
                            {
                                { "alert", alert }
                            };
                            await _navigationService.NavigateAsync(new Uri("AssignSizesView", UriKind.Relative), param, useModalNavigation: true, animated: false);

                            break;
                        case "Countinue with current scans":
                            await NavigateNextPage();
                            break;

                        case "Stay here":
                            break;
                        default:
                            break;
                    }
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
                    await NavigateNextPage();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private async Task NavigateNextPage()
        {
            try
            {
                ConstantManager.Barcodes = BarcodeCollection.ToList();
                if (BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                    await NavigateToValidatePartner(BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList());
                else
                {
                    try
                    {
                        PrintPallet();
                    }
                    catch (Exception)
                    {
                    }

                    await _navigationService.GoBackAsync(new NavigationParameters
                                    {
                                        { "AssignValueToAddPalletAsync", BatchId }, { "BarcodesCollection", BarcodeCollection },
                                    }, useModalNavigation: true, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void PrintPallet()
        {
            try
            {
                string header = string.Format(_zebraPrinterManager.PalletHeader, BatchId, PartnerModel.ParentPartnerName, PartnerModel.Address1, PartnerModel.City + "," + PartnerModel.State + " " + PartnerModel.PostalCode, "", PartnerModel.ParentPartnerName, BatchModel.BatchCode, DateTimeOffset.UtcNow.Date.ToShortDateString(), "1", "", BatchModel.BrandName,
                                    "1", "", "", "", "", "", "", "", "", "",
                                    "", "", "", "", "", "", "", "", "", "",
                                    "", BatchId, BatchId);
                new Thread(() =>
                {
                    _zebraPrinterManager.SendZplPalletAsync(header, ConstantManager.IPAddr);
                }).Start();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                {
                    { "BarcodeCollection", BarcodeCollection },
                    { "BatchId", BatchId },
                    { "ManifestId", ManifestId },

                },useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("ScanditScanView", UriKind.Relative), new NavigationParameters
                    {
                        { "Tags", Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.FillScanView }
                    }, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignBarcodeScannerValue(IList<BarcodeModel> barcodes)
        {
            try
            {
                foreach (var item in barcodes)
                    BarcodeCollection.Add(item);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            try
            {
                Tags = _tags;
                TagsStr = _tagsStr;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void AddTagsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.FillScanView }
                    }, useModalNavigation: true, animated: false);
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
                IList<Partner> selecetdPertner = BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).Select(x => x.Kegs.Partners).FirstOrDefault();
                Partner unusedPartner = null;
                foreach (var item in selecetdPertner)
                {
                    if (item.FullName == model.FullName)
                    {
                    }
                    else
                    {
                        unusedPartner = item;
                        break;
                    }
                }
                try
                {
                    if (model.Kegs.FirstOrDefault().MaintenanceItems?.Count > 0)
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        RealmDb.Write(() =>
                        {
                            BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(Maintenace);
                        });
                    }
                    else
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        try
                        {
                            RealmDb.Write(() =>
                            {
                                BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(ValidationOK);
                            });
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    try
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        RealmDb.Write(() =>
                       {
                           BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Kegs.Partners.Remove(unusedPartner);
                       });
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    if (!BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                        await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                finally
                {
                    unusedPartner = null;
                }
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
                BarcodeCollection.Clear();
                Tags = null;
                TagsStr = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignAddTagsValue(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey("Barcode"))
                {
                    try
                    {
                        using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                        {
                            string barcode = parameters.GetValue<string>("Barcode");
                            var oldBarcode = BarcodeCollection.Where(x => x.Barcode == barcode).FirstOrDefault();

                            for (int i = oldBarcode.Tags.Count - 1; i >= 0; i--)
                            {
                                oldBarcode.Tags.RemoveAt(i);
                            }
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

                Tags = ConstantManager.Tags;
                TagsStr = ConstantManager.TagsStr;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "Partner":
                    await AssignValidatedValueAsync(parameters.GetValue<Partner>("Partner"));
                    break;
                case "IsPalletze":
                    AssignInitValue(parameters);
                    break;
                case "model":
                    GenerateManifestIdAsync(parameters.GetValue<PalletModel>("model"));
                    break;
                case "GenerateManifestIdAsync":
                    GenerateManifestIdAsync(null);
                    try
                    {
                        BatchModel = parameters.GetValue<NewBatch>("NewBatchModel");
                        SizeButtonTitle = parameters.GetValue<string>("SizeButtonTitle");
                        PartnerModel = parameters.GetValue<PartnerModel>("PartnerModel");
                    }
                    catch (Exception)
                    {
                    }
                    break;
                case "models":
                    AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
                    break;
                case "AddTags":
                    AssignAddTagsValue(parameters);
                    break;
                default:
                    break;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }
        }

        #endregion
    }
}
