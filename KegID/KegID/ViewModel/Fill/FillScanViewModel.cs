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

        private readonly IZebraPrinterManager _zebraPrinterManager;
        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly ICalcCheckDigitMngr _calcCheckDigitMngr;
        private readonly IPageDialogService _dialogService;

        public string BatchId { get; set; }
        public NewBatch BatchModel { get; private set; }
        public string SizeButtonTitle { get; private set; }
        public PartnerModel PartnerModel { get; private set; }
        public bool HasPrint { get; private set; }
        public string Title { get; set; }
        public string ManifestId { get; set; }
        public string ManaulBarcode { get; set; }
        public string TagsStr { get; set; }
        public bool IsPalletVisible { get; set; }
        public bool IsPalletze { get; set; } = true;
        public ObservableCollection<BarcodeModel> BarcodeCollection { get; set; } = new ObservableCollection<BarcodeModel>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public IList<string> FillFromLocations { get; set; }
        public bool AllowMaintenanceFill { get; set; }
        public bool Lastbadscan { get; set; }

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

        public FillScanViewModel(INavigationService navigationService, IZebraPrinterManager zebraPrinterManager, IGetIconByPlatform getIconByPlatform, ICalcCheckDigitMngr calcCheckDigitMngr, IPageDialogService dialogService) : base(navigationService)
        {
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
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandReciever);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>(LabelItemTappedCommandRecieverAsync);
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>(IconItemTappedCommandRecieverAsync);
            DeleteItemCommand = new DelegateCommand<BarcodeModel>(DeleteItemCommandReciever);

            HandleUnSubscirbeMessages();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void DeleteItemCommandReciever(BarcodeModel model)
        {
            BarcodeCollection.Remove(model);
            Lastbadscan = false;
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
                        }

                        if (FillFromLocations != null)
                        {
                            foreach (var item in FillFromLocations)
                            {
                                if (item != message.Barcodes.Kegs.Locations.FirstOrDefault()?.EntityId)
                                {
                                    // needs to remove scan item and show alert message.
                                    BarcodeCollection.Remove(message.Barcodes);
                                    _ = _dialogService.DisplayAlertAsync("Alert", "Fill from location is not in any of default location", "Ok");
                                }
                            }
                        }

                        if (AllowMaintenanceFill && message.Barcodes.Kegs.MaintenanceItems.Count > 0)
                        {
                            //needs to check if any bed scan is there needs to stay here not to go away until user remove this scan manually.
                            Lastbadscan = true;
                            _ = _dialogService.DisplayAlertAsync("Alert", "There is a kegs with maintenance pleaser remove scan", "Ok");
                        }
                    }
                });
            });

            MessagingCenter.Subscribe<AddPalletToFillScanMsg>(this, "AddPalletToFillScanMsg", _ => Device.BeginInvokeOnMainThread(Cleanup));

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", _ =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    const string value = "Cancelled";
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
                FillFromLocations = parameters.GetValue<IList<string>>("FillFromLocations");
                AllowMaintenanceFill = parameters.GetValue<bool>("AllowMaintenanceFill");

                foreach (var item in parameters.GetValue<IList<BarcodeModel>>("Barcodes"))
                {
                    BarcodeCollection.Add(item);
                    TagsStr = item.TagsStr;
                }

                GenerateManifestId(null);
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
                    var page = formsNav.Navigation.NavigationStack[formsNav.Navigation.NavigationStack.Count-2];
                    (page?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "Barcodes", BarcodeCollection },{ "BatchId", BatchId }
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                await _navigationService.ClearPopupStackAsync(animated: false);
                if (IsPalletze)
                {
                    await _navigationService.GoBackAsync(animated: false);
                }
                else
                {
                    await _navigationService.NavigateAsync("FillScanReviewView", new NavigationParameters
                    {
                        { "BatchId", BatchId },{ "Count", BarcodeCollection.Count }
                    }, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void GenerateManifestId(PalletModel palletModel)
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
            foreach (var item in RealmDb.All<Preference>().Where(x => x.PreferenceName == "DashboardPreferences").ToList())
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
            int hours = 24 - now.Hour - 1;
            int minutes = 60 - now.Minute - 1;
            int seconds = 60 - now.Second - 1;
            return _ = seconds + (minutes * 60) + (hours * 3600);
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
                    await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.FillScanView },
                        {"AddTagsViewInitialValue",model }
                    }, animated: false);
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
                            await _navigationService.NavigateAsync("ScanInfoView", new NavigationParameters
                            {
                                { "model", model }
                            }, animated: false);
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
                await _navigationService.NavigateAsync("ValidateBarcodeView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void BarcodeManualCommandReciever()
        {
            try
            {
                if (!Lastbadscan)
                {
                    bool isNew = BarcodeCollection.ToList().Any(x => x.Barcode == ManaulBarcode);

                    BarcodeModel model = null;
                    if (!isNew)
                    {
                        try
                        {
                            model = new BarcodeModel
                            {
                                Barcode = ManaulBarcode,
                                TagsStr = TagsStr,
                                Icon = Cloud,
                                Page = nameof(ViewTypeEnum.FillScanView),
                                Contents = Tags.Count > 2 ? Tags?[2]?.Name ?? string.Empty : string.Empty
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
                                PageName = nameof(ViewTypeEnum.FillScanView)
                            };
                            MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                        }

                        ManaulBarcode = string.Empty;
                    }
                }
                else
                {
                    _ = _dialogService.DisplayAlertAsync("Alert", "There is a kegs with maintenance pleaser remove scan", "Ok");
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
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var result = BarcodeCollection.Where(x => x.Kegs.Partners.Count > 1).ToList();
                    if (result.Count > 0)
                        await NavigateToValidatePartner(result.ToList());
                    else
                    {
                        await NavigateToFillScanReview();
                    }
                }
                else
                {
                    await NavigateToFillScanReview();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task NavigateToFillScanReview()
        {
            await _navigationService.NavigateAsync("FillScanReviewView",
                                        new NavigationParameters
                                        {
                                { "BatchId", BatchId },{ "BarcodeCollection", BarcodeCollection }
                                        }, animated: false);
        }

        private void IsPalletVisibleCommandReciever()
        {
            IsPalletVisible = !IsPalletVisible;
        }

        private async void PrintCommandRecieverAsync()
        {
            if (!Lastbadscan)
            {
                await ValidateBarcode();
            }
            else
            {
                _ = _dialogService.DisplayAlertAsync("Alert", "There is a kegs with maintenance pleaser remove scan", "Ok");
            }
        }

        private async Task ValidateBarcode()
        {
            HasPrint = true;
            List<BarcodeModel> alert = BarcodeCollection.Where(x => x.Icon == "maintenace.png").ToList();
            if (alert.Count > 0 && alert.FirstOrDefault()?.HasMaintenaceVerified == false)
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
                            })).Start();

                            await _navigationService.GoBackAsync(new NavigationParameters
                                    {
                                        { "AssignValueToAddPalletAsync", BatchId }, { "BarcodesCollection", BarcodeCollection },
                                    }, animated: false);
                            break;
                        case "Assign sizes":
                            var param = new NavigationParameters
                            {
                                { "alert", alert }
                            };
                            await _navigationService.NavigateAsync("AssignSizesView", param, animated: false);

                            break;
                        case "Continue with current scans":
                            await NavigateNextPage();
                            break;

                        case "Stay here":
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
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    await _navigationService.GoBackAsync(new NavigationParameters
                                    {
                                        { "AssignValueToAddPalletAsync", BatchId }, { "BarcodesCollection", BarcodeCollection },
                                    }, animated: false);
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
                if (!Lastbadscan)
                {
                    await _navigationService.GoBackAsync(new NavigationParameters
                {
                    { "BarcodeCollection", BarcodeCollection },
                    { "BatchId", BatchId },
                    { "ManifestId", ManifestId },

                }, animated: false);
                }
                else
                {
                    _ = _dialogService.DisplayAlertAsync("Alert", "There is a kegs with maintenance pleaser remove scan", "Ok");
                }
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
                await _navigationService.NavigateAsync("ScanditScanView", new NavigationParameters
                    {
                        { "Tags", Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.FillScanView }
                    }, animated: false);
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
                if (!Lastbadscan)
                {
                    await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.FillScanView }
                    }, animated: false);
                }
                else
                {
                    _ = _dialogService.DisplayAlertAsync("Alert", "There is a kegs with maintenance pleaser remove scan", "Ok");
                }
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
                IList<Partner> selecetdPertner = BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault()?.Barcode).Select(x => x.Kegs.Partners).FirstOrDefault();
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
                    if (model.Kegs.FirstOrDefault()?.MaintenanceItems?.Count > 0)
                    {
                        Realm RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        RealmDb.Write(() => BarcodeCollection.FirstOrDefault(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).Icon = _getIconByPlatform.GetIcon(Maintenace));
                    }
                    else
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        try
                        {
                            RealmDb.Write(() => BarcodeCollection.FirstOrDefault(x => x.Barcode == model
                                .Kegs
                                .FirstOrDefault()
                                .Barcode).Icon = _getIconByPlatform.GetIcon(ValidationOK));
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

                    try
                    {
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        RealmDb.Write(() => BarcodeCollection.FirstOrDefault(x => x.Barcode == model.Kegs.FirstOrDefault()?.Barcode)?.Kegs.Partners.Remove(unusedPartner));
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    if (!BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                        await _navigationService.GoBackAsync(animated: false);
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }

            switch (parameters.Keys.FirstOrDefault())
            {
                case "Partner":
                    await AssignValidatedValueAsync(parameters.GetValue<Partner>("Partner"));
                    break;
                case "IsPalletze":
                    AssignInitValue(parameters);
                    break;
                case "model":
                    GenerateManifestId(parameters.GetValue<PalletModel>("model"));
                    break;
                case "GenerateManifestIdAsync":
                    GenerateManifestId(null);
                    try
                    {
                        BatchModel = parameters.GetValue<NewBatch>("NewBatchModel");
                        SizeButtonTitle = parameters.GetValue<string>("SizeButtonTitle");
                        PartnerModel = parameters.GetValue<PartnerModel>("PartnerModel");
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                    break;
                case "models":
                    AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
                    break;
                case "AddTags":
                    AssignAddTagsValue(parameters);
                    break;
            }

        }

        #endregion
    }
}
