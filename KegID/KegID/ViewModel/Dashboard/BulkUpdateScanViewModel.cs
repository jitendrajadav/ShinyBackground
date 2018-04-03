using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Rg.Plugins.Popup.Extensions;
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

        public BulkUpdateScanViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandRecieverAsync);
            SaveCommand = new RelayCommand(SaveCommandReciever);
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            SizeCollection = new List<string>() { "1/2 bbl", "1/4 bbl", "1/6 bbl", "30 L", "40 L", "50 L" };
            LabelItemTappedCommand = new RelayCommand<Barcode>(execute: (model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>(execute: (model) => IconItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods
        private async void IconItemTappedCommandRecieverAsync(Barcode model)
        {
            if (model.PartnerCount > 1)
            {
                List<Barcode> modelList = new List<Barcode>
                    {
                        model
                    };
                await NavigateToValidatePartner(modelList);
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView());
                var value = await SQLiteServiceClient.Db.Table<ValidatePartnerModel>().Where(x => x.Barcode == model.Id).ToListAsync();

                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Barcode = string.Format(" Barcode {0} ", model.Id);
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Ownername = value.FirstOrDefault().FullName;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Size = value.FirstOrDefault().Size;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Contents = value.FirstOrDefault().Contents;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Batch = value.FirstOrDefault().Batch;
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().Location = value.FirstOrDefault().Location;
            }
        }

        private async void LabelItemTappedCommandRecieverAsync(Barcode model)
        {
            if (model.PartnerCount > 1)
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
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
            }
        }
        private static async Task NavigateToValidatePartner(List<Barcode> model)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
            await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBardeValue(model);
        }

        private async void AddTagsCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        private async void BarcodeManualCommandRecieverAsync()
        {
            await BarcodeScanner.ValidateBarcodeInsertIntoLocalDB(ManaulBarcode, _moveService);
            ManaulBarcode = string.Empty;
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
           var value = await BarcodeScanner.BarcodeScanAsync(_moveService);
        }

        private void SaveCommandReciever()
        {
            
        }

        private async void CancelCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
