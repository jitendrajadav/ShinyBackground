using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KegID.Common;
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
    public class PalletizeViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPalletizeService _palletizeService;
        private readonly IPageDialogService _dialogService;
        private readonly IZebraPrinterManager _zebraPrinterManager;
        private readonly IUuidManager _uuidManager;
        private readonly ICalcCheckDigitMngr _calcCheckDigitMngr;

        public bool TargetLocationPartner { get; set; }
        public PartnerModel StockLocation { get; set; } = new PartnerModel();
        public PartnerModel TargetLocation { get; set; } = new PartnerModel();
        public string AddInfoTitle { get; set; } = "Add info";
        public bool IsCameraVisible { get; set; }
        public string ManifestId { get; set; }
        public string AddKegs { get; set; }
        public void OnAddKegsChanged()
        {
            IsSubmitVisible = AddKegs.Contains("Item") ? true : false;
        }
        public bool IsSubmitVisible { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public bool Operator { get; set; }
        public string ContainerTypes { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand TargetLocationPartnerCommand { get; }
        public DelegateCommand AddKegsCommand { get; }
        public DelegateCommand IsPalletVisibleCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand SubmitCommand { get; }

        #endregion

        #region Constructor

        public PalletizeViewModel(IPalletizeService palletizeService, INavigationService navigationService, IZebraPrinterManager zebraPrinterManager, IUuidManager uuidManager, ICalcCheckDigitMngr calcCheckDigitMngr, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _palletizeService = palletizeService;
            _zebraPrinterManager = zebraPrinterManager;
            _uuidManager = uuidManager;
            _calcCheckDigitMngr = calcCheckDigitMngr;

            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            PartnerCommand = new DelegateCommand(PartnerCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            TargetLocationPartnerCommand = new DelegateCommand(TargetLocationPartnerCommandRecieverAsync);
            AddKegsCommand = new DelegateCommand(AddKegsCommandRecieverAsync);
            IsPalletVisibleCommand = new DelegateCommand(IsPalletVisibleCommandReciever);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandReciever);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);

            StockLocation.FullName = "Barcode Brewing";
            TargetLocation.FullName = "None";
            HandleUnsubscribeMessages();
            HandleReceivedMessages();

            PreferenceSetting();
            AddKegs = string.Format("Add {0}", ContainerTypes);
        }

        #endregion

        #region Methods

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            var preferenceUSER_HOME = preferences.Find(x => x.PreferenceName == "USER_HOME");
            StockLocation.FullName = preferenceUSER_HOME?.PreferenceValue ?? StockLocation.FullName;

            var preferenceOperator = preferences.Find(x => x.PreferenceName == "Operator");
            Operator = preferenceOperator != null && bool.Parse(preferenceOperator.PreferenceValue);

            ContainerTypes = preferences.Find(x => x.PreferenceName == "CONTAINER_TYPES").PreferenceValue;
        }

        private void HandleUnsubscribeMessages()
        {
            MessagingCenter.Unsubscribe<ScannerToPalletAssign>(this, "ScannerToPalletAssign");
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<ScannerToPalletAssign>(this, "ScannerToPalletAssign", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.Barcode))
                        {
                            ManifestId = value.Barcode;
                        }
                    }
                });
            });
        }

        public void GenerateManifestIdAsync(PalletModel palletModel)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            DateTimeOffset now = DateTimeOffset.Now;
            string barCode;
            long prefix = 0;
            var lastCharOfYear = now.Year.ToString().ToCharArray().LastOrDefault().ToString();
            var dayOfYear = now.DayOfYear;
            var secondsInDayTillNow = SecondsInDayTillNow();
            var millisecond = now.Millisecond;

            var preference = RealmDb.All<Preference>().Where(x => x.PreferenceName == "DashboardPreferences").ToList();
            try
            {
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
                ManifestId = barCode + checksumDigit;
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                preference = null;
            }
        }

        internal void AssignPartnerValue(PartnerModel model)
        {
            try
            {
                if (TargetLocationPartner)
                {
                    TargetLocationPartner = false;
                    TargetLocation = model;
                }
                else
                    StockLocation = model;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static int SecondsInDayTillNow()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            int hours = 24 - now.Hour - 1;
            int minutes = 60 - now.Minute - 1;
            int seconds = 60 - now.Second - 1;
            return _ = seconds + (minutes * 60) + (hours * 3600);
        }

        private async void SubmitCommandRecieverAsync()
        {
            List<PalletItem> palletItems = new List<PalletItem>();
            PalletItem pallet = null;
            PalletRequestModel palletRequestModel = null;
            var barCodeCollection = ConstantManager.Barcodes;
            PalletResponseModel palletResponseModel = null;

            try
            {
                Loader.StartLoading();

                foreach (var item in barCodeCollection)
                {
                    pallet = new PalletItem
                    {
                        Barcode = item.Barcode,
                        ScanDate = DateTimeOffset.Now,
                    };

                    foreach (var tag in ConstantManager.Tags)
                    {
                        pallet.Tags.Add(tag);
                    }
                    palletItems.Add(pallet);
                }

                palletRequestModel = new PalletRequestModel
                {
                    Barcode = ManifestId.Split('-').LastOrDefault(),
                    BuildDate = DateTimeOffset.Now,
                    OwnerId = AppSettings.CompanyId,
                    PalletId = _uuidManager.GetUuId(),
                    ReferenceKey = "",
                    StockLocation = StockLocation.PartnerId,
                    StockLocationId = StockLocation.PartnerId,
                    StockLocationName = StockLocation.FullName,
                };

                foreach (var item in palletItems)
                {
                    palletRequestModel.PalletItems.Add(item);
                }
                foreach (var item in ConstantManager.Tags)
                {
                    palletRequestModel.Tags.Add(item);
                }

                palletResponseModel = new PalletResponseModel
                {
                    Barcode = ManifestId.Split('-').LastOrDefault(),
                    BuildDate = DateTimeOffset.Now,
                    Container = null,
                    CreatedDate = DateTimeOffset.Now,
                    DataInfo = new Model.PrintPDF.DateInfo { },
                    Location = new Owner { },
                    Owner = new Owner { },
                    PalletId = palletRequestModel.PalletId,
                    PalletItems = palletItems,
                    ReferenceKey = string.Empty,
                    StockLocation = new Owner { FullName = StockLocation.FullName , PartnerTypeName = StockLocation.PartnerTypeName },
                    TargetLocation = new Model.PrintPDF.TargetLocation { },
                    Tags = ConstantManager.Tags,
                };

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var value = await _palletizeService.PostPalletAsync(palletRequestModel, AppSettings.SessionId, Configuration.NewPallet);
                }
                else
                {
                    try
                    {
                        palletRequestModel.IsQueue = true;
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        RealmDb.Write(() =>
                        {
                            RealmDb.Add(palletRequestModel, update: true);
                        });
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }

                PrintPallet();

                await _navigationService.NavigateAsync("PalletizeDetailView", new NavigationParameters
                    {
                        { "LoadInfo", palletResponseModel },{ "Contents", ConstantManager.Contents }
                    }, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
                palletItems = null;
                pallet = null;
                barCodeCollection = null;
                palletRequestModel = null;
                Cleanup();
            }
        }

        public void PrintPallet()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var brandCode = RealmDb.All<BrandModel>().Where(x=>x.BrandName == ConstantManager.Contents).FirstOrDefault();
            try
            {
                var addresss = StockLocation;
                string header = string.Format(_zebraPrinterManager.PalletHeader, ManifestId, addresss.ParentPartnerName, addresss.Address1, addresss.City + "," + addresss.State + " " + addresss.PostalCode, "", addresss.ParentPartnerName, brandCode, DateTimeOffset.UtcNow.Date.ToShortDateString(), "1", "", ConstantManager.Contents,
                                    "1", "", "", "", "", "", "", "", "", "",
                                    "", "", "", "", "", "", "", "", "", "",
                                    "", ManifestId, ManifestId);
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

        internal void AssingScanKegsValue(IList<BarcodeModel> _barcodes)
        {
            if (_barcodes.Count > 1)
                AddKegs = string.Format("{0} Items", _barcodes.Count);
            else if (_barcodes.Count == 1)
                AddKegs = string.Format("{0} Item", _barcodes.Count);
            else
                AddKegs = string.Format("Add {0}", ContainerTypes);
        }

        private async void BarcodeScanCommandReciever()
        {
            try
            {
                await _navigationService.NavigateAsync("ScanditScanView", new NavigationParameters
                    {
                        { "ViewTypeEnum", ViewTypeEnum.PalletizeView }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void IsPalletVisibleCommandReciever()
        {
            IsCameraVisible = true;
        }

        private async void AddKegsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("ScanKegsView", new NavigationParameters { { "models", ConstantManager.Barcodes } }, animated: false);
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
                await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.PalletizeView }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void PartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PartnersView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void TargetLocationPartnerCommandRecieverAsync()
        {
            try
            {
                TargetLocationPartner = true;
                await _navigationService.NavigateAsync("PartnersView", animated: false);
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
                if (AddInfoTitle == "Add info" && AddKegs == string.Format("Add {0}", ContainerTypes))
                {
                    await _navigationService.GoBackAsync(animated: false);
                    IsCameraVisible = false;
                }
                else
                {
                    bool result = await _dialogService.DisplayAlertAsync("Cancel?", "Are you sure you want to cancel?", "Leave", "Stay here");
                    if (result)
                    {
                        await _navigationService.GoBackAsync(animated: false);
                        IsCameraVisible = false;
                        Cleanup();
                    }
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
                AddInfoTitle = "Add info";
                AddKegs = string.Format("Add {0}", ContainerTypes);
                IsSubmitVisible = false;
                // Update an object with a transaction
                //using (var trans = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                //{
                //    StockLocation.FullName = "Barcode Brewing";
                //    TargetLocation.FullName = "None";
                //    trans.Commit();
                //}
                Tags = null;
                ConstantManager.Barcodes.Clear();
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

            if (ConstantManager.Barcodes != null)
                AssingScanKegsValue(ConstantManager.Barcodes);

            switch (parameters.Keys.FirstOrDefault())
            {
                case "model":
                    AssignPartnerValue(parameters.GetValue<PartnerModel>("model"));
                    break;
                case "AddTags":
                    AddInfoTitle = ConstantManager.TagsStr;
                    Tags = ConstantManager.Tags;
                    break;
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GenerateManifestIdAsync"))
            {
                GenerateManifestIdAsync(null);
            }
            return base.InitializeAsync(parameters);
        }

        #endregion
    }
}
