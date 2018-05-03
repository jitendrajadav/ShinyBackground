using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Rg.Plugins.Popup.Extensions;
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
        public IMoveService _moveService { get; set; }
        public IDashboardService _dashboardService { get; set; }

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

        private ObservableCollection<Barcode> _BarcodeCollection = new ObservableCollection<Barcode>();

        /// <summary>
        /// Sets and gets the BarcodeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Barcode> BarcodeCollection
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
        public RelayCommand CancelCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand BarcodeScanCommand { get; }
        public RelayCommand BarcodeManualCommand { get; }
        public RelayCommand AddTagsCommand { get; }
        public RelayCommand<Barcode> LabelItemTappedCommand { get; }
        public RelayCommand<Barcode> IconItemTappedCommand { get; }
        
        #endregion

        #region Contructor

        public BulkUpdateScanViewModel(IMoveService moveService, IDashboardService dashboardService)
        {
            _moveService = moveService;
            _dashboardService = dashboardService;

            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandRecieverAsync);
            SaveCommand = new RelayCommand(SaveCommandRecieverAsync);
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            //SizeCollection = new List<string>() { "1/2 bbl", "1/4 bbl", "1/6 bbl", "30 L", "40 L", "50 L" };
            LabelItemTappedCommand = new RelayCommand<Barcode>(execute: (model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>(execute: (model) => IconItemTappedCommandRecieverAsync(model));
            LoadAssetSizeAsync();
            LoadAssetTypeAsync();
        }

        #endregion

        #region Methods

        private async void LoadAssetSizeAsync()
        {
            var value = await SQLiteServiceClient.Db.Table<AssetSizeModel>().ToListAsync();
            SizeCollection = value.Select(x => x.AssetSize).ToList();
        }

        private async void LoadAssetTypeAsync()
        {
            var value = await SQLiteServiceClient.Db.Table<AssetTypeModel>().ToListAsync();
            AssetTypeCollection = value.Select(x=>x.AssetType).ToList();
        }

        private async void IconItemTappedCommandRecieverAsync(Barcode model)
        {
            if (model.Partners.Count > 1)
            {
                List<Barcode> modelList = new List<Barcode>
                    {
                        model
                    };
                await NavigateToValidatePartner(modelList);
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView(), animated: false);
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model);
            }
        }

        private async void LabelItemTappedCommandRecieverAsync(Barcode model)
        {
            if (model.Partners.Count > 1)
            {
                List<Barcode> modelList = new List<Barcode>
                    {
                        model
                    };
                await NavigateToValidatePartner(modelList);
            }
            else
            {
                //IsFromScanned = true;
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView(), animated: false);
            }
        }

        private static async Task NavigateToValidatePartner(List<Barcode> model)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
            await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
        }

        private async void AddTagsCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView(), animated: false);
        }

        private async void BarcodeManualCommandRecieverAsync()
        {
            var isNew = BarcodeCollection.ToList().Any(x => x.Id == ManaulBarcode);
            if (!isNew)
            {
                var barcodes = await BarcodeScanner.ValidateBarcodeInsertIntoLocalDB(_moveService, ManaulBarcode, Tags, TagsStr);
                ManaulBarcode = string.Empty;
                BarcodeCollection.Add(barcodes);
            }
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
           await BarcodeScanner.BarcodeScanAsync(_moveService,Tags,TagsStr);
        }

        private async void SaveCommandRecieverAsync()
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
                        //LocationDate = DateTime.Now.ToShortDateString(),
                        //ManufactureDate = DateTime.Now.ToShortDateString(),
                        //ManufactureLocation = "",
                        //Manufacturer = "",
                        //Markings = "",
                        //Material = "",
                        //Measure = "",
                        //OwnerName = "",
                        //PurchaseDate = DateTime.Now.ToShortDateString(),
                        //PurchaseOrder = "",
                        //PurchasePrice = "",
                        //SkuCode = "",
                        //Volume = item.Tags?.FirstOrDefault().Value
                        AssetSize = SelectedItemSize,
                        AssetType = SelectedItemType,
                        Barcode = item.Id,
                        Tags = item.Tags,
                        AssetVolume = item.Tags?.FirstOrDefault().Value,
                        KegId = Uuid.GetUuId(),
                        OwnerId = AppSettings.User.CompanyId,
                        OwnerSkuId = "",
                        ProfileId = "",
                    };

                    MassUpdateKegKegs.Add(MassUpdateKeg);
                }
                model.Kegs = MassUpdateKegKegs;

                var value = await _dashboardService.PostKegUploadAsync(model, AppSettings.User.SessionId, Configuration.MassUpdateKegList);
                if (value.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    SimpleIoc.Default.GetInstance<KegSearchViewModel>().AssingSuccessMsgAsync();
                } 
            }
        }

        private async void CancelCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            Tags = _tags;
            TagsStr = _tagsStr;
        }

        #endregion
    }
}
