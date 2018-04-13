using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanViewModel : BaseViewModel
    {
        #region Properties

        public IMoveService _moveService { get; set; }

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

        public RelayCommand CancelCommand { get; }
        public RelayCommand BarcodeScanCommand { get; }
        public RelayCommand AddTagsCommand { get; }
        public RelayCommand PrintCommand { get; }
        public RelayCommand IsPalletVisibleCommand { get;}
        public RelayCommand SubmitCommand { get;}
        public RelayCommand BarcodeManualCommand { get; }
        public RelayCommand<Barcode> IconItemTappedCommand { get;}
        public RelayCommand<Barcode> LabelItemTappedCommand { get; }

        #endregion

        #region Constructor

        public FillScanViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            PrintCommand = new RelayCommand(PrintCommandRecieverAsync);
            IsPalletVisibleCommand = new RelayCommand(IsPalletVisibleCommandReciever);
            SubmitCommand = new RelayCommand(SubmitCommandReciever);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new RelayCommand<Barcode>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>((model) => IconItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        public async void GenerateManifestIdAsync(PalletModel palletModel)
        {
            DateTime now = DateTime.Now;
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
                    BarcodeCollection = new ObservableCollection<Barcode>(palletModel.Barcode);
                }
                else
                {
                    BarcodeCollection.Clear();
                    var preference = await SQLiteServiceClient.Db.Table<Preference>().Where(x => x.PreferenceName == "DashboardPreferences").ToListAsync();

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
                    var checksumDigit = Utils.CalculateCheckDigit(barCode);
                    ManifestId = barCode + checksumDigit;
                }
            }
        }

        private static int SecondsInDayTillNow()
        {
            DateTime now = DateTime.Now;
            int hours = 0, minutes = 0, seconds = 0, totalSeconds = 0;
            hours = (24 - now.Hour) - 1;
            minutes = (60 - now.Minute) - 1;
            seconds = (60 - now.Second - 1);

            return totalSeconds = seconds + (minutes * 60) + (hours * 3600);
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
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
            }
        }

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
                SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model.Id);
            }
        }

        private async Task NavigateToValidatePartner(List<Barcode> model)
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
            await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
        }

        private async void BarcodeManualCommandRecieverAsync()
        {
            var value = await BarcodeScanner.ValidateBarcodeInsertIntoLocalDB(ManaulBarcode, _moveService);
            ManaulBarcode = string.Empty;
            BarcodeCollection = new ObservableCollection<Barcode>(value);
        }

        private void SubmitCommandReciever()
        {
            
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
            var result = BarcodeCollection.Where(x => x.PartnerCount > 1).ToList();
            if (result.Count > 0)
                await NavigateToValidatePartner(result.ToList());
            else
                await SimpleIoc.Default.GetInstance<AddPalletsViewModel>().AssignValueToAddPalletAsync(ManifestId, BarcodeCollection);
        }

        private async void CancelCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            var value = await BarcodeScanner.BarcodeScanAsync(_moveService);
            BarcodeCollection = new ObservableCollection<Barcode>(value);
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        internal void AssignValidatedValue(Partner model)
        {
            BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
            BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().PartnerCount = 1;
        }

        #endregion
    }
}
