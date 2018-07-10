using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class BulkUpdateScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";
        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;
        private readonly IDashboardService _dashboardService;
        private readonly IGetIconByPlatform _getIconByPlatform;

        #region ManaulBarcode

        /// <summary>
        /// The <see cref="ManaulBarcode" /> property's name.
        /// </summary>
        public const string ManaulBarcodePropertyName = "ManaulBarcode";

        private string _ManaulBarcode = string.Empty;

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

        #region SelectedItemType

        /// <summary>
        /// The <see cref="SelectedItemType" /> property's name.
        /// </summary>
        public const string SelectedItemTypePropertyName = "SelectedItemType";

        private string _SelectedItemType = string.Empty;

        /// <summary>
        /// Sets and gets the SelectedItemType property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedItemType
        {
            get
            {
                return _SelectedItemType;
            }

            set
            {
                if (_SelectedItemType == value)
                {
                    return;
                }

                _SelectedItemType = value;
                RaisePropertyChanged(SelectedItemTypePropertyName);
            }
        }

        #endregion

        #region AssetTypeCollection

        /// <summary>
        /// The <see cref="AssetTypeCollection" /> property's name.
        /// </summary>
        public const string AssetTypeCollectionPropertyName = "AssetTypeCollection";

        private IList<string> _AssetTypeCollection = null;

        /// <summary>
        /// Sets and gets the AssetTypeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> AssetTypeCollection
        {
            get
            {
                return _AssetTypeCollection;
            }

            set
            {
                if (_AssetTypeCollection == value)
                {
                    return;
                }

                _AssetTypeCollection = value;
                RaisePropertyChanged(AssetTypeCollectionPropertyName);
            }
        }

        #endregion

        #region SizeCollection

        /// <summary>
        /// The <see cref="SizeCollection" /> property's name.
        /// </summary>
        public const string SizeCollectionPropertyName = "SizeCollection";

        private IList<string> _SizeCollection = null;

        /// <summary>
        /// Sets and gets the SizeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> SizeCollection
        {
            get
            {
                return _SizeCollection;
            }

            set
            {
                if (_SizeCollection == value)
                {
                    return;
                }

                _SizeCollection = value;
                RaisePropertyChanged(SizeCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedItemSize

        /// <summary>
        /// The <see cref="SelectedItemSize" /> property's name.
        /// </summary>
        public const string SelectedItemSizePropertyName = "SelectedItemSize";

        private string _SelectedItemSize = string.Empty;

        /// <summary>
        /// Sets and gets the SelectedItemSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedItemSize
        {
            get
            {
                return _SelectedItemSize;
            }

            set
            {
                if (_SelectedItemSize == value)
                {
                    return;
                }

                _SelectedItemSize = value;
                RaisePropertyChanged(SelectedItemSizePropertyName);
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

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _tagsStr = default(string);

        /// <summary>
        /// Sets and gets the TagsStr property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TagsStr
        {
            get
            {
                return _tagsStr;
            }

            set
            {
                if (_tagsStr == value)
                {
                    return;
                }

                _tagsStr = value;
                RaisePropertyChanged(TagsStrPropertyName);
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

        public BulkUpdateScanViewModel(IMoveService moveService, IDashboardService dashboardService, INavigationService navigationService, IGetIconByPlatform getIconByPlatform)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _moveService = moveService;
            _dashboardService = dashboardService;
            _getIconByPlatform = getIconByPlatform;

            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            SaveCommand = new DelegateCommand(SaveCommandRecieverAsync);
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));

            LoadAssetSizeAsync();
            LoadAssetTypeAsync();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<BulkUpdateScanMessage>(this, "BulkUpdateScanMessage", message => {
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

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message => {
                Device.BeginInvokeOnMainThread(() => {
                    var value = "Cancelled";
                    if (value == "Cancelled")
                    {

                    }
                });
            });
        }

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
                    var param = new NavigationParameters
                    {
                        { "model", model }
                    };
                    await _navigationService.NavigateAsync(new Uri("ScanInfoView", UriKind.Relative), param, useModalNavigation: true, animated: false);

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
                    var param = new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.BulkUpdateScanView }
                    };
                    await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), param,useModalNavigation: true, animated: false);
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
                var param = new NavigationParameters
                    {
                        { "model", model }
                    };
                await _navigationService.NavigateAsync(new Uri("ValidateBarcodeView", UriKind.Relative), param,useModalNavigation:true, animated: false);

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
                var param = new NavigationParameters
                    {
                        {"viewTypeEnum",ViewTypeEnum.BulkUpdateScanView }
                    };
                await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), param,useModalNavigation: true, animated: false);
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
                        //Tags = null,
                        TagsStr = string.Empty,
                        Icon = Cloud
                    };
                    BarcodeCollection.Add(model);
                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = new List<string>() { ManaulBarcode },
                        PageName = ViewTypeEnum.BulkUpdateScanView.ToString()
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

        private async void BarcodeScanCommandRecieverAsync()
        {
            try
            {
                var param = new NavigationParameters
                    {
                        { "Tags", Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.BulkUpdateScanView }
                    };
                await _navigationService.NavigateAsync(new Uri("CognexScanView", UriKind.Relative), param, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SaveCommandRecieverAsync()
        {
            try
            {
                if (BarcodeCollection.Count > 0)
                {
                    var model = new KegBulkUpdateItemRequestModel();
                    var MassUpdateKegKegs = new List<MassUpdateKeg>();
                    MassUpdateKeg MassUpdateKeg = null;
                    var val = await _dashboardService.GetAssetVolumeAsync(AppSettings.User.SessionId, false);
                    foreach (var item in BarcodeCollection)
                    {
                        MassUpdateKeg = new MassUpdateKeg
                        {
                            //AssetProfile = "",
                            //Colors = "",
                            //Coupling = "",
                            //FixedContents = "",
                            //LeasingCompany = "",
                            //Location = "",
                            //LocationDate = DateTimeOffset.Now.ToShortDateString(),
                            //ManufactureDate = DateTimeOffset.Now.ToShortDateString(),
                            //ManufactureLocation = "",
                            //Manufacturer = "",
                            //Markings = "",
                            //Material = "",
                            //Measure = "",
                            //OwnerName = "",
                            //PurchaseDate = DateTimeOffset.Now.ToShortDateString(),
                            //PurchaseOrder = "",
                            //PurchasePrice = "",
                            //SkuCode = "",
                            //Volume = item.Tags?.FirstOrDefault().Value
                            AssetSize = SelectedItemSize,
                            AssetType = SelectedItemType,
                            Barcode = item.Barcode,
                            //Tags = item.Tags,
                            AssetVolume = item.TagsStr,
                            KegId = Uuid.GetUuId(),
                            OwnerId = AppSettings.User.CompanyId,
                            OwnerSkuId = "",
                            ProfileId = "",
                        };

                        MassUpdateKegKegs.Add(MassUpdateKeg);
                    }
                    model.Kegs = MassUpdateKegKegs;

                    var value = await _dashboardService.PostKegUploadAsync(model, AppSettings.User.SessionId, Configuration.MassUpdateKegList);
                    if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                    {
                        var param = new NavigationParameters
                        {
                            { "AssingSuccessMsgAsync", "AssingSuccessMsgAsync" }
                        };
                        await _navigationService.GoBackAsync(param, useModalNavigation: true, animated: false);
                    }
                }
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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        #endregion
    }
}
