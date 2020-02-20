using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
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
        public DelegateCommand IsPalletVisibleCommand { get; }
        public DelegateCommand SubmitCommand { get; }
        public DelegateCommand BarcodeManualCommand { get; }
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get; }
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
                            var oldBarcode = BarcodeCollection.Where(x => x.Barcode == message.Barcodes.Barcode).FirstOrDefault();
                            oldBarcode.Pallets = message?.Barcodes?.Pallets;
                            oldBarcode.Kegs = message?.Barcodes?.Kegs;
                            oldBarcode.Icon = message?.Barcodes?.Kegs?.Partners?.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : message?.Barcodes?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                            oldBarcode.IsScanned = true;
                            db.Commit();

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
        }

        internal void AssignInitValue(INavigationParameters parameters)
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

        internal async Task AssignValidateBarcodeValueAsync()
        {
            ConstantManager.Barcodes = BarcodeCollection;
            ConstantManager.Tags = Tags;
            var formsNav = ((Prism.Common.IPageAware)NavigationService).Page;
            var page = formsNav.Navigation.NavigationStack[formsNav.Navigation.NavigationStack.Count - 2];
            (page?.BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "Barcodes", BarcodeCollection },{ "BatchId", BatchId }
                    });

            await NavigationService.ClearPopupStackAsync(animated: false);
            if (IsPalletze)
            {
                await NavigationService.GoBackAsync(animated: false);
            }
            else
            {
                await NavigationService.NavigateAsync("FillScanReviewView", new NavigationParameters
                    {
                        { "BatchId", BatchId },{ "Count", BarcodeCollection.Count }
                    }, animated: false);
            }
        }

        public void GenerateManifestId(PalletModel palletModel)
        {
            DateTimeOffset now = DateTimeOffset.Now;
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

                    _ = GenerateBatchId(ref prefix, lastCharOfYear, dayOfYear, secondsInDayTillNow, millisecond);
                }
            }
            else
            {
                _ = GenerateBatchId(ref prefix, lastCharOfYear, dayOfYear, secondsInDayTillNow, millisecond);
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
                await NavigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.FillScanView },
                        {"AddTagsViewInitialValue",model }
                    }, animated: false);
            }
        }

        private async void IconItemTappedCommandRecieverAsync(BarcodeModel model)
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
                        _ = await _dialogService.DisplayAlertAsync("Warning", "This scan could not be verified", "Keep", "Delete");
                    }
                    else
                    {
                        await NavigationService.NavigateAsync("ScanInfoView", new NavigationParameters
                            {
                                { "model", model }
                            }, animated: false);
                    }
                }
            }
        }

        private async Task NavigateToValidatePartner(List<BarcodeModel> model)
        {
            await NavigationService.NavigateAsync("ValidateBarcodeView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);

        }

        private void BarcodeManualCommandReciever()
        {
            if (!Lastbadscan)
            {
                bool isNew = BarcodeCollection.ToList().Any(x => x.Barcode == ManaulBarcode);

                BarcodeModel model = null;
                if (!isNew)
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

                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        // IJobManager can and should be injected into your viewmodel code
                        Shiny.ShinyHost.Resolve<Shiny.Jobs.IJobManager>().RunTask("FillJob" + ManaulBarcode, async _ =>
                    {
                        // your code goes here - async stuff is welcome (and necessary)
                        System.Net.Http.HttpResponseMessage response = await ApiManager.GetValidateBarcode(ManaulBarcode, Settings.SessionId);
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var data = await Task.Run(() => JsonConvert.DeserializeObject<BarcodeModel>(json, GetJsonSetting()));

                            if (data.Kegs != null)
                            {
                                using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                                {
                                    var oldBarcode = BarcodeCollection.FirstOrDefault(x => x.Barcode == data?.Kegs?.Partners?.FirstOrDefault().Kegs?.FirstOrDefault()?.Barcode);
                                    oldBarcode.Pallets = data.Pallets;
                                    oldBarcode.Kegs = data.Kegs;
                                    oldBarcode.Icon = data?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : data?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                                    if (oldBarcode.Icon == "validationerror.png")
                                        Vibration.Vibrate();
                                    oldBarcode.IsScanned = true;
                                    db.Commit();
                                }
                            }
                        }
                    });
                    }

                    ManaulBarcode = string.Empty;
                }
            }
            else
            {
                _ = _dialogService.DisplayAlertAsync("Alert", "There is a kegs with maintenance pleaser remove scan", "Ok");
            }

        }

        private async void SubmitCommandRecieverAsync()
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

        private async Task NavigateToFillScanReview()
        {
            await NavigationService.NavigateAsync("FillScanReviewView",
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

                        await NavigationService.GoBackAsync(new NavigationParameters
                                    {
                                        { "AssignValueToAddPalletAsync", BatchId }, { "BarcodesCollection", BarcodeCollection },
                                    }, animated: false);
                        break;
                    case "Assign sizes":
                        var param = new NavigationParameters
                            {
                                { "alert", alert }
                            };
                        await NavigationService.NavigateAsync("AssignSizesView", param, animated: false);

                        break;
                    case "Continue with current scans":
                        await NavigateNextPage();
                        break;

                    case "Stay here":
                        break;
                }

            }
            else
            {
                await NavigateNextPage();
            }
        }

        private async Task NavigateNextPage()
        {
            ConstantManager.Barcodes = BarcodeCollection.ToList();
            if (BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                await NavigateToValidatePartner(BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList());
            else
            {
                PrintPallet();

                await NavigationService.GoBackAsync(new NavigationParameters
                                    {
                                        { "AssignValueToAddPalletAsync", BatchId }, { "BarcodesCollection", BarcodeCollection },
                                    }, animated: false);
            }
        }

        public void PrintPallet()
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

        private async void CancelCommandRecieverAsync()
        {
            if (!Lastbadscan)
            {
                await NavigationService.GoBackAsync(new NavigationParameters
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

        private async void BarcodeScanCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("ScanditScanView", new NavigationParameters
                    {
                        { "Tags", Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.FillScanView }
                    }, animated: false);
        }

        internal void AssignBarcodeScannerValue(IList<BarcodeModel> barcodes)
        {
            foreach (var item in barcodes)
                BarcodeCollection.Add(item);
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            Tags = _tags;
            TagsStr = _tagsStr;
        }

        private async void AddTagsCommandRecieverAsync()
        {
            if (!Lastbadscan)
            {
                await NavigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.FillScanView }
                    }, animated: false);
            }
            else
            {
                _ = _dialogService.DisplayAlertAsync("Alert", "There is a kegs with maintenance pleaser remove scan", "Ok");
            }
        }

        internal async Task AssignValidatedValueAsync(Partner model)
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

            Realm RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

            if (model.Kegs.FirstOrDefault()?.MaintenanceItems?.Count > 0)
            {
                RealmDb.Write(() => BarcodeCollection.FirstOrDefault(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).Icon = _getIconByPlatform.GetIcon(Maintenace));
            }
            else
            {

                RealmDb.Write(() => BarcodeCollection.FirstOrDefault(x => x.Barcode == model
                    .Kegs
                    .FirstOrDefault()
                    .Barcode).Icon = _getIconByPlatform.GetIcon(ValidationOK));
            }

            RealmDb.Write(() => BarcodeCollection.FirstOrDefault(x => x.Barcode == model.Kegs.FirstOrDefault()?.Barcode)?.Kegs.Partners.Remove(unusedPartner));


            if (!BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                await NavigationService.GoBackAsync(animated: false);
        }

        public void Cleanup()
        {
            BarcodeCollection.Clear();
            Tags = null;
            TagsStr = string.Empty;
        }

        internal void AssignAddTagsValue(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Barcode"))
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

            Tags = ConstantManager.Tags;
            TagsStr = ConstantManager.TagsStr;
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
                    BatchModel = parameters.GetValue<NewBatch>("NewBatchModel");
                    SizeButtonTitle = parameters.GetValue<string>("SizeButtonTitle");
                    PartnerModel = parameters.GetValue<PartnerModel>("PartnerModel");
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
