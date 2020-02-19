using KegID.Model;
using KegID.Services;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Messages;
using Realms;
using Xamarin.Essentials;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Acr.UserDialogs;
using KegID.Common;
using Newtonsoft.Json;

namespace KegID.ViewModel
{
    public class ScanKegsViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IGetIconByPlatform _getIconByPlatform;

        private const string Maintenace = "maintenace.png";
        private const string ValidationOK = "validationok.png";
        private const string Cloud = "collectionscloud.png";
        public bool HasDone { get; set; }
        public ObservableCollection<BarcodeModel> BarcodeCollection { get; set; } = new ObservableCollection<BarcodeModel>();
        public IList<BrandModel> BrandCollection { get; set; }
        public string SelectedBrand { get; set; }
        public string ManaulBarcode { get; set; }
        public string TagsStr { get; set; }
        public string Batch { get; set; }
        public bool IsExpand { get; set; }
        public int Warning { get; set; }

        #endregion

        #region Commands

        public DelegateCommand DoneCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand BarcodeManualCommand { get; }
        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> LabelItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> DeleteItemCommand { get; set; }
        public DelegateCommand ExpandCommand { get; }

        #endregion

        #region Constructor

        public ScanKegsViewModel(INavigationService navigationService, IPageDialogService dialogService, IGetIconByPlatform getIconByPlatform) : base(navigationService)
        {
            _dialogService = dialogService;
            _getIconByPlatform = getIconByPlatform;

            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandReciever);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));
            DeleteItemCommand = new DelegateCommand<BarcodeModel>((model) => DeleteItemCommandReciever(model));
            ExpandCommand = new DelegateCommand(ExpandCommandReciever);
            //LoadBrand();

            //HandleUnsubscribeMessages();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void ExpandCommandReciever()
        {
            IsExpand = true;
        }

        //private void HandleUnsubscribeMessages()
        //{
        //    MessagingCenter.Unsubscribe<MoveScanKegsMessage>(this, "MoveScanKegsMessage");
        //    MessagingCenter.Unsubscribe<PalletToScanKegPagesMsg>(this, "PalletToScanKegPagesMsg");
        //}

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<MoveScanKegsMessage>(this, "MoveScanKegsMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        ManaulBarcode = message.Barcode;
                        BarcodeManualCommandRecieverAsync();
                    }
                });
            });

            MessagingCenter.Subscribe<PalletToScanKegPagesMsg>(this, "PalletToScanKegPagesMsg", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        BarcodeScanCommandReciever();
                    }
                });
            });
        }

        public void LoadBrand()
        {
            BrandCollection = new ObservableCollection<BrandModel>(LoadBrandAsync());
        }

        public IList<BrandModel> LoadBrandAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var all = RealmDb.All<BrandModel>().ToList();
            IList<BrandModel> model = all;

            if (model.Count > 0)
                return model;

            UserDialogs.Instance.HideLoading();
            return model;
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
                ConstantManager.IsFromScanned = true;
                await NavigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.ScanKegsView },
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
                        bool accept = await _dialogService.DisplayAlertAsync("Warning", "This scan could not be verified", "Keep", "Delete");
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

        private async void AddTagsCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum", ViewTypeEnum.ScanKegsView },
                    }, animated: false);
        }

        private async void DoneCommandRecieverAsync()
        {
            HasDone = true;
            List<BarcodeModel> alert = BarcodeCollection.Where(x => x.Icon == "maintenace.png").ToList();
            if (alert.Count > 0 && !alert.FirstOrDefault().HasMaintenaceVerified)
            {
                string strBarcode = alert.FirstOrDefault().Barcode;
                var option = await _dialogService.DisplayActionSheetAsync("No keg with a barcode of \n" + strBarcode + " could be found",
                    null, null, "Remove unverified scans", "Assign sizes", "Countinue with current scans", "Stay here");
                switch (option)
                {
                    case "Remove unverified scans":
                        BarcodeCollection.Remove(alert.FirstOrDefault());
                        await NavigationService.GoBackAsync(animated: false);
                        Cleanup();

                        break;
                    case "Assign sizes":
                        var param = new NavigationParameters
                            {
                                { "alert", alert }
                            };
                        await NavigationService.NavigateAsync("AssignSizesView", param, animated: false);
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
            else
            {
                await NavigateNextPage();
            }
        }

        private async Task NavigateNextPage()
        {
            ConstantManager.Barcodes = BarcodeCollection.ToList();
            ConstantManager.Contents = SelectedBrand;
            if (BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                await NavigateToValidatePartner(BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList());
            else
            {
                await NavigationService.GoBackAsync(animated: false);
                Cleanup();
            }
        }

        internal void AssignSizesValue(List<BarcodeModel> verifiedBarcodes)
        {
            using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
            {
                foreach (var item in verifiedBarcodes)
                {
                    BarcodeCollection.Where(x => x.Barcode == item.Barcode).FirstOrDefault().HasMaintenaceVerified = item.HasMaintenaceVerified;
                }
                db.Commit();
            }
        }

        internal void AssignInitialValue()
        {
            if (!string.IsNullOrEmpty(ConstantManager.Barcode))
                BarcodeCollection.Add(new BarcodeModel { Barcode = ConstantManager.Barcode });
        }

        private void DeleteItemCommandReciever(BarcodeModel model)
        {
            BarcodeCollection.Remove(model);
        }

        private void BarcodeManualCommandRecieverAsync()
        {
            var isNew = BarcodeCollection.FirstOrDefault(x => x.Barcode == ManaulBarcode);
            UpdateTagsStr();
            BarcodeModel model = new BarcodeModel()
            {
                Barcode = ManaulBarcode,
                TagsStr = TagsStr,
                Icon = _getIconByPlatform.GetIcon(Cloud),
                Page = nameof(ViewTypeEnum.ScanKegsView),
                Contents = SelectedBrand
            };

            if (ConstantManager.Tags != null)
            {
                foreach (var item in ConstantManager.Tags)
                    model.Tags.Add(item);
            }

            if (ConstantManager.Tags != null)
            {
                foreach (var item in ConstantManager.Tags)
                    model.Tags.Add(item);
            }
            if (isNew == null)
            {
                BarcodeCollection.Add(model);
            }
            else
            {
                using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                {
                    var oldBarcode = BarcodeCollection.FirstOrDefault(x => x.Barcode == ManaulBarcode);
                    oldBarcode.TagsStr = model.TagsStr;
                    oldBarcode.Icon = model.Icon;
                    oldBarcode.Contents = SelectedBrand;
                    db.Commit();
                }
            }
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                //var KegModel = new KegRequestModel
                //{
                //    KegId = KegId,
                //    Barcode = ManaulBarcode,
                //    OwnerId =  PartnerModel.PartnerId,
                //    AltBarcode = Barcode,
                //    Notes = "",
                //    ReferenceKey = "",
                //    ProfileId = "",
                //    AssetType = SelectedItemType,
                //    AssetSize = Size,
                //    AssetVolume = "",
                //    AssetDescription = "",
                //    OwnerSkuId = "",
                //    FixedContents = "",
                //    Tags = Tags,
                //    MaintenanceAlertIds = new List<string>(),
                //    LessorId = "",
                //    PurchaseDate = DateTimeOffset.Now,
                //    PurchasePrice = 0,
                //    PurchaseOrder = "",
                //    ManufacturerName = "",
                //    ManufacturerId = "",
                //    ManufactureLocation = "",
                //    ManufactureDate = DateTimeOffset.Now,
                //    Material = "",
                //    Markings = "",
                //    Colors = ""
                //};
                //var kegStatusResponse = await _dashboardService.PostKegAsync(KegModel, KegId, Settings.SessionId, Configuration.Keg);

                //var message = new StartLongRunningTaskMessage
                //{
                //    Barcode = new List<string>() { ManaulBarcode },
                //    PageName = nameof(ViewTypeEnum.ScanKegsView)
                //};
                //MessagingCenter.Send(message, "StartLongRunningTaskMessage");

                // IJobManager can and should be injected into your viewmodel code
                Shiny.ShinyHost.Resolve<Shiny.Jobs.IJobManager>().RunTask("MoveJob" + ManaulBarcode, async _ =>
                  {
                        // your code goes here - async stuff is welcome (and necessary)
                        var response = await ApiManager.GetValidateBarcode(ManaulBarcode, Settings.SessionId);
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

        private void UpdateTagsStr()
        {
            string tags = string.Empty;
            if (ConstantManager.Tags != null)
            {
                if (!string.IsNullOrEmpty(SelectedBrand))
                {
                    if (!ConstantManager.Tags.Any(x => x.Property == "Contents"))
                    {
                        ConstantManager.Tags.Add(new Tag { Property = "Contents", Value = SelectedBrand });
                    }
                    else
                    {
                        ConstantManager.Tags.Find(x => x.Property == "Contents").Value = SelectedBrand;
                    }
                }
                if (!string.IsNullOrEmpty(Batch))
                {
                    if (!ConstantManager.Tags.Any(x => x.Property == "Batch"))
                    {
                        ConstantManager.Tags.Add(new Tag { Property = "Batch", Value = Batch });
                    }
                    else
                    {
                        ConstantManager.Tags.Find(x => x.Property == "Batch").Value = Batch;
                    }
                }

                foreach (Tag item in ConstantManager.Tags)
                {
                    tags = tags + item?.Property + " : " + item?.Value + " ; ";
                }
                TagsStr = tags;
            }
        }

        internal async void BarcodeScanCommandReciever()
        {
            UpdateTagsStr();
            await NavigationService.NavigateAsync("ScanditScanView", new NavigationParameters
                    {
                        { "Tags", ConstantManager.Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.ScanKegsView }
                    }, animated: false);
        }

        internal void AssignBarcodeScannerValue(IList<BarcodeModel> models)
        {
            if (models.Count > 0)
            {
                if (ConstantManager.Tags != null)
                    ConstantManager.Tags.Clear();
                else
                    ConstantManager.Tags = new List<Tag>();

                foreach (var item in models)
                {
                    BarcodeCollection.Add(item);
                    TagsStr = item.TagsStr;
                }
                foreach (var tags in BarcodeCollection.LastOrDefault().Tags)
                {
                    ConstantManager.Tags.Add(tags);
                }
            }
        }

        internal async Task AssignValidatedValueAsync(Partner model)
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

            if (model.Kegs.FirstOrDefault().MaintenanceItems?.Count > 0)
            {
                BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(Maintenace);
            }
            else
            {
                BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(ValidationOK);
            }

            BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Kegs.Partners.Remove(unusedPartner);

            if (HasDone && model.Kegs?.FirstOrDefault()?.MaintenanceItems?.Count < 0)
            {
                await NavigationService.NavigateAsync("../../", animated: false);
                Cleanup();
            }
            else
            {
                var check = BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList();
                if (check.Count == 0)
                {
                    await NavigationService.GoBackAsync(animated: false);
                }
            }

            unusedPartner = null;
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

            ConstantManager.IsFromScanned = false;
            TagsStr = ConstantManager.TagsStr;
        }

        public void Cleanup()
        {
            BarcodeCollection.Clear();
            TagsStr = default;
            SelectedBrand = null;
            HasDone = false;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("models"))
            {
                AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
            }
            return base.InitializeAsync(parameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "Partner":
                    await AssignValidatedValueAsync(parameters.GetValue<Partner>("Partner"));
                    break;
                case "AddTags":
                    AssignAddTagsValue(parameters);
                    break;

                case "AssignSizesValue":
                    AssignSizesValue(ConstantManager.VerifiedBarcodes);
                    break;
                case "Barcode":
                    BarcodeCollection.Add(new BarcodeModel { Barcode = parameters.GetValue<string>("Barcode"), Icon = ValidationOK });
                    break;
            }
            if (parameters.ContainsKey("DoneCommandRecieverAsync"))
            {
                DoneCommandRecieverAsync();
            }
        }

        #endregion
    }
}
