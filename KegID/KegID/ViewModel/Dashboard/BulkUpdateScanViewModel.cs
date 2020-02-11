using Acr.UserDialogs;
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class BulkUpdateScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";
        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly IUuidManager _uuidManager;
        private readonly IPageDialogService _dialogService;

        public string ManaulBarcode { get; set; }
        public string SelectedItemType { get; set; }
        public IList<string> AssetTypeCollection { get; set; }
        public IList<string> SizeCollection { get; set; }
        public string SelectedItemSize { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public string TagsStr { get; set; }
        public ObservableCollection<BarcodeModel> BarcodeCollection { get; set; } = new ObservableCollection<BarcodeModel>();

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand BarcodeManualCommand { get; }
        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand<BarcodeModel> LabelItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get; }

        #endregion

        #region Contructor

        public BulkUpdateScanViewModel(INavigationService navigationService, IGetIconByPlatform getIconByPlatform, IUuidManager uuidManager, IPageDialogService dialogService) : base(navigationService)
        {
            _getIconByPlatform = getIconByPlatform;
            _uuidManager = uuidManager;
            _dialogService = dialogService;

            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            SaveCommand = new DelegateCommand(async () => await RunSafe(SaveCommandRecieverAsync()));
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));

            LoadAssetSizeAsync();
            LoadAssetTypeAsync();

            //HandleUnsubscribeMessages();
            //HandleReceivedMessages();
        }

        #endregion

        #region Methods

        //private void HandleUnsubscribeMessages()
        //{
        //    //MessagingCenter.Unsubscribe<BulkUpdateScanMessage>(this, "BulkUpdateScanMessage");
        //    //MessagingCenter.Unsubscribe<CancelledMessage>(this, "CancelledMessage");
        //}

        //private void HandleReceivedMessages()
        //{
        //    //MessagingCenter.Subscribe<BulkUpdateScanMessage>(this, "BulkUpdateScanMessage", message => {
        //    //    Device.BeginInvokeOnMainThread(() =>
        //    //    {
        //    //        var value = message;
        //    //        if (value != null)
        //    //        {
        //    //            try
        //    //            {
        //    //                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
        //    //                RealmDb.Write(() =>
        //    //                {
        //    //                    var oldBarcode = BarcodeCollection.FirstOrDefault(x => x.Barcode == value.Barcodes.Barcode);
        //    //                    oldBarcode.Pallets = value.Barcodes.Pallets;
        //    //                    oldBarcode.Kegs = value.Barcodes.Kegs;
        //    //                    oldBarcode.Icon = value?.Barcodes?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : value?.Barcodes?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
        //    //                    oldBarcode.IsScanned = true;
        //    //                });
        //    //            }
        //    //            catch (Exception ex)
        //    //            {
        //    //                Crashes.TrackError(ex);
        //    //            }
        //    //        }
        //    //    });
        //    //});

        //    //MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message => {
        //    //    Device.BeginInvokeOnMainThread(() => {
        //    //        var value = "Cancelled";
        //    //        if (value == "Cancelled")
        //    //        {

        //    //        }
        //    //    });
        //    //});
        //}

        private void LoadAssetSizeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<AssetSizeModel>().ToList();
                SizeCollection = value.Select(x => x.AssetSize).ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void LoadAssetTypeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<AssetTypeModel>().ToList();
                AssetTypeCollection = value.Select(x => x.AssetType).ToList();
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
                    await _navigationService.NavigateAsync("ScanInfoView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
                        {"viewTypeEnum",ViewTypeEnum.BulkUpdateScanView }
                    }, animated: false);
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
                        {"viewTypeEnum",ViewTypeEnum.BulkUpdateScanView }
                    }, animated: false);
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
                var isNew = BarcodeCollection.ToList().Any(x => x?.Kegs?.Partners?.FirstOrDefault()?.Kegs?.FirstOrDefault()?.Barcode == ManaulBarcode);
                if (!isNew)
                {
                    BarcodeModel model = new BarcodeModel()
                    {
                        Barcode = ManaulBarcode,
                        TagsStr = TagsStr,
                        Icon = _getIconByPlatform.GetIcon(Cloud),
                        Page = nameof(ViewTypeEnum.BulkUpdateScanView),
                        Contents = SelectedItemType
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
                        //var message = new StartLongRunningTaskMessage
                        //{
                        //    Barcode = new List<string>() { ManaulBarcode },
                        //    PageName = nameof(ViewTypeEnum.BulkUpdateScanView)
                        //};
                        //MessagingCenter.Send(message, "StartLongRunningTaskMessage");

                        // IJobManager can and should be injected into your viewmodel code
                        Shiny.ShinyHost.Resolve<Shiny.Jobs.IJobManager>().RunTask("BulkUpdateJob" + ManaulBarcode, async _ =>
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
                                        try
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
                                        catch (Exception EX)
                                        {
                                        }
                                    }
                                }
                            }
                        });

                    }

                    ManaulBarcode = string.Empty;
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
                await _navigationService.NavigateAsync("CognexScanView", new NavigationParameters
                    {
                        { "Tags", Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.BulkUpdateScanView }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task SaveCommandRecieverAsync()
        {
            try
            {
                if (BarcodeCollection.Count > 0)
                {
                    UserDialogs.Instance.ShowLoading("Loading");
                    var model = new KegBulkUpdateItemRequestModel();
                    var MassUpdateKegKegs = new List<MassUpdateKeg>();
                    MassUpdateKeg MassUpdateKeg = null;
                    var val = await ApiManager.GetAssetVolume(Settings.SessionId, false);
                    foreach (var item in BarcodeCollection)
                    {
                        MassUpdateKeg = new MassUpdateKeg
                        {
                            AssetSize = SelectedItemSize,
                            AssetType = SelectedItemType,
                            Barcode = item.Barcode,
                            AssetVolume = item.TagsStr,
                            KegId = _uuidManager.GetUuId(),
                            OwnerId = Settings.CompanyId,
                            OwnerSkuId = "",
                            ProfileId = "",
                        };

                        MassUpdateKegKegs.Add(MassUpdateKeg);
                    }
                    model.Kegs = MassUpdateKegKegs;

                    var value = await ApiManager.PostKegUpload(model, Settings.SessionId);
                    if (value.IsSuccessStatusCode)
                    {
                        await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            { "AssingSuccessMsgAsync", "AssingSuccessMsgAsync" }
                        }, animated: false);
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Alert", "Please add scan item", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void CancelCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
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
