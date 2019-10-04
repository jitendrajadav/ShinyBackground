using KegID.Model;
using KegID.Services;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using KegID.Messages;
using Realms;
using Xamarin.Essentials;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Acr.UserDialogs;

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
        public BrandModel SelectedBrand { get; set; }
        public string ManaulBarcode { get; set; }
        public string TagsStr { get; set; }
        public string Batch { get; set; }

        #endregion

        #region Commands

        public DelegateCommand DoneCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand BarcodeManualCommand { get; }
        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> LabelItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> DeleteItemCommand { get; set; }

        #endregion

        #region Constructor

        public ScanKegsViewModel(INavigationService navigationService, IPageDialogService dialogService,IGetIconByPlatform getIconByPlatform) : base(navigationService)
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
            //LoadBrand();

            HandleUnsubscribeMessages();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void HandleUnsubscribeMessages()
        {
            MessagingCenter.Unsubscribe<ScanKegsMessage>(this, "ScanKegsMessage");
            MessagingCenter.Unsubscribe<PalletToScanKegPagesMsg>(this, "PalletToScanKegPagesMsg");
            MessagingCenter.Unsubscribe<CancelledMessage>(this, "CancelledMessage");
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<ScanKegsMessage>(this, "ScanKegsMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        try
                        {
                            using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                            {
                                var oldBarcode = BarcodeCollection.Where(x => x.Barcode == value.Barcodes.Barcode).FirstOrDefault();
                                oldBarcode.Pallets = value.Barcodes.Pallets;
                                oldBarcode.Kegs = value.Barcodes.Kegs;
                                oldBarcode.Icon = value?.Barcodes?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : value?.Barcodes?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                                oldBarcode.IsScanned = true;
                                db.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
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


        public void LoadBrand()
        {
            BrandCollection = new ObservableCollection<BrandModel>(LoadBrandAsync());
        }

        public IList<BrandModel> LoadBrandAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var all = RealmDb.All<BrandModel>().ToList();
            IList<BrandModel> model = all;
            try
            {
                if (model.Count > 0)
                    return model;
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
                return null;
            }
            finally
            {
                Loader.StopLoading();
            }
            return model;
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
                    ConstantManager.IsFromScanned = true;
                    await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.ScanKegsView },
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

        private async void AddTagsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("AddTagsView", new NavigationParameters
                    {
                        {"viewTypeEnum", ViewTypeEnum.ScanKegsView },
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void DoneCommandRecieverAsync()
        {
            HasDone = true;
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
                            await _navigationService.GoBackAsync(animated: false);
                            Cleanup();

                            break;
                        case "Assign sizes":
                            var param = new NavigationParameters
                            {
                                { "alert", alert }
                            };
                            await _navigationService.NavigateAsync("AssignSizesView", param, animated: false);
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
                ConstantManager.Contents = SelectedBrand;
                if (BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                    await NavigateToValidatePartner(BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList());
                else
                {
                    await _navigationService.GoBackAsync(animated: false);
                    Cleanup();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignSizesValue(List<BarcodeModel> verifiedBarcodes)
        {
            try
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValue()
        {
            try
            {
                if (!string.IsNullOrEmpty(ConstantManager.Barcode))
                    BarcodeCollection.Add(new BarcodeModel { Barcode = ConstantManager.Barcode });

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteItemCommandReciever(BarcodeModel model)
        {
            BarcodeCollection.Remove(model);
        }

        private void BarcodeManualCommandRecieverAsync()
        {
            try
            {
                var isNew = BarcodeCollection.Where(x => x.Barcode == ManaulBarcode).FirstOrDefault();
                UpdateTagsStr();
                BarcodeModel model = new BarcodeModel()
                {

                    UpdateTagsStr();
                    BarcodeModel model = new BarcodeModel()
                    {
                        Barcode = ManaulBarcode,
                        TagsStr = TagsStr,
                        Icon = _getIconByPlatform.GetIcon(Cloud),
                        Page = ViewTypeEnum.ScanKegsView.ToString(),
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
                if (isNew==null)
                {
                    BarcodeCollection.Add(model);
                }
                else
                {
                    using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                    {
                        var oldBarcode = BarcodeCollection.Where(x => x.Barcode == ManaulBarcode).FirstOrDefault();
                        oldBarcode.TagsStr = model.TagsStr;
                        oldBarcode.Icon = model.Icon;
                        oldBarcode.Contents = SelectedBrand?.BrandName;
                        db.Commit();
                    }
                }
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = new List<string>() { ManaulBarcode },
                        PageName = ViewTypeEnum.ScanKegsView.ToString()
                    };
                    MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                }

                ManaulBarcode = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
            try
            {
                UpdateTagsStr();
                await _navigationService.NavigateAsync("ScanditScanView", new NavigationParameters
                    {
                        { "Tags", ConstantManager.Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.ScanKegsView }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignBarcodeScannerValue(IList<BarcodeModel> models)
        {
            if (models.Count > 0)
            {
                try
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
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        internal async Task AssignValidatedValueAsync(Partner model)
        {
            IList<Partner> selecetdPertner = BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).Select(x=>x.Kegs.Partners).FirstOrDefault();
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
                    try
                    {
                        BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(Maintenace);
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
                        BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = _getIconByPlatform.GetIcon(ValidationOK);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }

                try
                {
                    BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Kegs.Partners.Remove(unusedPartner);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                if (HasDone && model.Kegs?.FirstOrDefault()?.MaintenanceItems?.Count < 0)
                {
                    await _navigationService.NavigateAsync("../../", animated: false);
                    Cleanup();
                }
                else
                {
                    var check = BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList();
                    if (check.Count == 0)
                    {
                        await _navigationService.GoBackAsync(animated: false);
                    }
                }
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

                ConstantManager.IsFromScanned = false;
                TagsStr = ConstantManager.TagsStr;
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
                TagsStr = default;
                SelectedBrand = null;
                HasDone = false;
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
                case "AddTags":
                    AssignAddTagsValue(parameters);
                    break;
                case "models":
                    AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
                    break;
                case "AssignSizesValue":
                    AssignSizesValue(ConstantManager.VerifiedBarcodes);
                    break;
                case "Barcode":
                    BarcodeCollection.Add(new BarcodeModel { Barcode = parameters.GetValue<string>("Barcode"), Icon = ValidationOK });
                    break;
                default:
                    break;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("DoneCommandRecieverAsync"))
            {
                DoneCommandRecieverAsync();
            }
        }

        #endregion
    }
}
