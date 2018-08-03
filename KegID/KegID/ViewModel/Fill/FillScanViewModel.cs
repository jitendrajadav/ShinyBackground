using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";

        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;
        private readonly IZebraPrinterManager _zebraPrinterManager;
        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly ICalcCheckDigitMngr _calcCheckDigitMngr;

        public string BatchId { get; set; }
        public BatchModel BatchModel { get; private set; }
        public string SizeButtonTitle { get; private set; }
        public PartnerModel PartnerModel { get; private set; }

        #region ManifestId

        /// <summary>
        /// The <see cref="ManifestId" /> property's name.
        /// </summary>
        public const string ManifestIdPropertyName = "ManifestId";

        private string _ManifestId = default(string);

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

        private string _ManaulBarcode = default(string);

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

        private string _TagsStr = default(string);


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

        #endregion

        #region Constructor

        public FillScanViewModel(IMoveService moveService, INavigationService navigationService, IZebraPrinterManager zebraPrinterManager, IGetIconByPlatform getIconByPlatform, ICalcCheckDigitMngr calcCheckDigitMngr)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;
            _getIconByPlatform = getIconByPlatform;
            _zebraPrinterManager = zebraPrinterManager;
            _calcCheckDigitMngr = calcCheckDigitMngr;

            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            PrintCommand = new DelegateCommand(PrintCommandRecieverAsync);
            IsPalletVisibleCommand = new DelegateCommand(IsPalletVisibleCommandReciever);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));

            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<FillScanMessage>(this, "FillScanMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        RealmDb.Write(() =>
                        {
                            var oldBarcode = BarcodeCollection.Where(x => x.Barcode == value.Barcodes.Barcode).FirstOrDefault();
                            oldBarcode.Pallets = value.Barcodes.Pallets;
                            oldBarcode.Kegs = value.Barcodes.Kegs;
                            oldBarcode.Icon = value?.Barcodes?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : value?.Barcodes?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                            oldBarcode.IsScanned = true;
                        });
                    }
                });
            });

            MessagingCenter.Subscribe<FillToFillScanPagesMsg>(this, "FillToFillScanPagesMsg", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        AssignInitValue(value.BatchModel, value.SizeButtonTitle, value.PartnerModel);
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

        internal void AssignInitValue(BatchModel batchModel  , string sizeButtonTitle, PartnerModel partnerModel)
        {
            BatchModel = batchModel;
            SizeButtonTitle = sizeButtonTitle;
            PartnerModel = partnerModel;
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
                        { "Barcodes", BarcodeCollection },{ "ManifestId", BatchId }
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
                        ManifestId = palletModel.ManifestId;
                        BarcodeCollection = new ObservableCollection<BarcodeModel>(palletModel.Barcode);
                    }
                    else
                    {
                        BarcodeCollection.Clear();
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
                        ManifestId = "Pallet #" + BatchId;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
                        {"viewTypeEnum",ViewTypeEnum.FillScanView }
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
                    await _navigationService.NavigateAsync(new Uri("ScanInfoView", UriKind.Relative), new NavigationParameters
                    {
                        { "model", model }
                    }, useModalNavigation: true, animated: false);
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
                var isNew = BarcodeCollection.ToList().Any(x => x.Barcode == ManaulBarcode);
                if (!isNew)
                {
                    BarcodeModel model = new BarcodeModel
                    {
                        Barcode = ManaulBarcode,
                        //Tags = Tags,
                        TagsStr = TagsStr,
                        Icon = Cloud
                    };
                    BarcodeCollection.Add(model);
                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = new List<string>() { ManaulBarcode },
                        PageName = ViewTypeEnum.FillScanView.ToString()
                    };
                    MessagingCenter.Send(message, "StartLongRunningTaskMessage");
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
                        { "BatchId", BatchId },{ "Count", BarcodeCollection.Count }
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
            try
            {
                var result = BarcodeCollection.Where(x => x.Kegs.Partners.Count > 1).ToList();
                if (result.Count > 0)
                    await NavigateToValidatePartner(result.ToList());
                else
                {
                    new Task(new Action(() => 
                    {
                        PrintPallet();
                    }
                    )).Start();

                    try
                    {
                        ConstantManager.Barcodes = BarcodeCollection;
                        ConstantManager.Tags = Tags;

                        var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
                        var page = formsNav.Navigation.ModalStack[formsNav.Navigation.ModalStack.Count-2];
                        (page?.BindingContext as INavigationAware)?.OnNavigatingTo(new NavigationParameters
                        {
                            { "AssignValueToAddPalletAsync", BatchId }, { "BarcodesCollection", BarcodeCollection },
                        });
                    }
                    catch (Exception)
                    {

                    }
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

                _zebraPrinterManager.SendZplPalletAsync(header,ConstantManager.IsIPAddr,ConstantManager.IPAddr);
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
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
                await _navigationService.NavigateAsync(new Uri("CognexScanView", UriKind.Relative), new NavigationParameters
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
                var unusedPerner = BarcodeCollection.Where(x => x.Kegs.Partners != model).Select(x => x.Kegs.Partners.FirstOrDefault()).FirstOrDefault();
                BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon("validationquestion.png");

               await AssignValidateBarcodeValueAsync();

                foreach (var item in BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode))
                {
                    item.Kegs.Partners.Remove(unusedPerner);
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

        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "Partner":
                    await AssignValidatedValueAsync(parameters.GetValue<Partner>("Partner"));
                    break;
                case "IsPalletze":
                    IsPalletze = parameters.GetValue<bool>("IsPalletze");
                    ManifestId = parameters.GetValue<string>("ManifestId");
                    break;
                case "model":
                    GenerateManifestIdAsync(parameters.GetValue<PalletModel>("model"));
                    break;
                case "GenerateManifestIdAsync":
                    GenerateManifestIdAsync(null);
                    break;
                case "models":
                    AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
                    break;
                case "AddTags":
                    Tags = ConstantManager.Tags;
                    TagsStr = ConstantManager.TagsStr;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
