using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeViewModel : BaseViewModel
    {
        #region Properties
        public IPalletizeService _palletizeService { get; set; }
        public IMoveService _moveService { get; set; }
        public bool TargetLocationPartner { get; set; }

        #region StockLocation

        /// <summary>
        /// The <see cref="StockLocation" /> property's name.
        /// </summary>
        public const string StockLocationPropertyName = "StockLocation";

        private PartnerModel _stockLocation = new PartnerModel();

        /// <summary>
        /// Sets and gets the StockLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel StockLocation
        {
            get
            {
                return _stockLocation;
            }

            set
            {
                if (_stockLocation == value)
                {
                    return;
                }

                _stockLocation = value;
                RaisePropertyChanged(StockLocationPropertyName);
            }
        }

        #endregion

        #region TargetLocation

        /// <summary>
        /// The <see cref="TargetLocation" /> property's name.
        /// </summary>
        public const string TargetLocationPropertyName = "TargetLocation";

        private PartnerModel _targetLocation = new PartnerModel();

        /// <summary>
        /// Sets and gets the TargetLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public PartnerModel TargetLocation
        {
            get
            {
                return _targetLocation;
            }

            set
            {
                if (_targetLocation == value)
                {
                    return;
                }

                _targetLocation = value;
                RaisePropertyChanged(TargetLocationPropertyName);
            }
        }

        #endregion

        #region AddInfoTitle

        /// <summary>
        /// The <see cref="AddInfoTitle" /> property's name.
        /// </summary>
        public const string AddInfoTitlePropertyName = "AddInfoTitle";

        private string _AddInfoTitle = "Add info";

        /// <summary>
        /// Sets and gets the AddInfoTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddInfoTitle
        {
            get
            {
                return _AddInfoTitle;
            }

            set
            {
                if (_AddInfoTitle == value)
                {
                    return;
                }

                _AddInfoTitle = value;
                RaisePropertyChanged(AddInfoTitlePropertyName);
            }
        }

        #endregion

        #region IsCameraVisible

        /// <summary>
        /// The <see cref="IsCameraVisible" /> property's name.
        /// </summary>
        public const string IsCameraVisiblePropertyName = "IsCameraVisible";

        private bool _IsCameraVisible = false;

        /// <summary>
        /// Sets and gets the IsCameraVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsCameraVisible
        {
            get
            {
                return _IsCameraVisible;
            }

            set
            {
                if (_IsCameraVisible == value)
                {
                    return;
                }

                _IsCameraVisible = value;
                RaisePropertyChanged(IsCameraVisiblePropertyName);
            }
        }

        #endregion

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

        #region AddKegs

        /// <summary>
        /// The <see cref="AddKegs" /> property's name.
        /// </summary>
        public const string AddKegsPropertyName = "AddKegs";

        private string _AddKegs = "Add Kegs";

        /// <summary>
        /// Sets and gets the AddKegs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AddKegs
        {
            get
            {
                return _AddKegs;
            }

            set
            {
                if (_AddKegs == value)
                {
                    return;
                }

                _AddKegs = value;
                RaisePropertyChanged(AddKegsPropertyName);
            }
        }

        #endregion

        #region IsSubmitVisible

        /// <summary>
        /// The <see cref="IsSubmitVisible" /> property's name.
        /// </summary>
        public const string IsSubmitVisiblePropertyName = "IsSubmitVisible";

        private bool _IsSubmitVisible = false;

        /// <summary>
        /// Sets and gets the IsSubmitVisible property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSubmitVisible
        {
            get
            {
                return _IsSubmitVisible;
            }

            set
            {
                if (_IsSubmitVisible == value)
                {
                    return;
                }

                _IsSubmitVisible = value;
                RaisePropertyChanged(IsSubmitVisiblePropertyName);
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
        public RelayCommand PartnerCommand { get; }
        public RelayCommand AddTagsCommand { get; }
        public RelayCommand TargetLocationPartnerCommand { get; }
        public RelayCommand AddKegsCommand { get; }
        public RelayCommand IsPalletVisibleCommand { get; }
        public RelayCommand BarcodeScanCommand { get; }
        public RelayCommand SubmitCommand { get; }
        #endregion

        #region Constructor
        public PalletizeViewModel(IPalletizeService palletizeService, IMoveService moveService)
        {
            _moveService = moveService;
            _palletizeService = palletizeService;
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            PartnerCommand = new RelayCommand(PartnerCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            TargetLocationPartnerCommand = new RelayCommand(TargetLocationPartnerCommandRecieverAsync);
            AddKegsCommand = new RelayCommand(AddKegsCommandRecieverAsync);
            IsPalletVisibleCommand = new RelayCommand(IsPalletVisibleCommandReciever);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandReciever);
            SubmitCommand = new RelayCommand(SubmitCommandRecieverAsync);

            StockLocation.FullName = "Barcode Brewing";
            TargetLocation.FullName = "None";
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

        private static int SecondsInDayTillNow()
        {
            DateTime now = DateTime.Now;
            int hours = 0, minutes = 0, seconds = 0, totalSeconds = 0;
            hours = (24 - now.Hour) - 1;
            minutes = (60 - now.Minute) - 1;
            seconds = (60 - now.Second - 1);

            return totalSeconds = seconds + (minutes * 60) + (hours * 3600);
        }

        private async void SubmitCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();

                List<PalletItem> palletItems = new List<PalletItem>();
                PalletItem pallet = null;
                var barCodeCollection = SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeCollection;

                foreach (var item in barCodeCollection)
                {
                    pallet = new PalletItem
                    {
                        Barcode = item.Id,
                        ScanDate = DateTime.Now,
                        Tags = SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags,
                        ValidationStatus = 4
                    };

                    palletItems.Add(pallet);
                }

                PalletRequestModel palletRequestModel = new PalletRequestModel
                {
                    Barcode = ManifestId.Split('-').LastOrDefault(),
                    BuildDate = DateTime.Now,
                    OwnerId = AppSettings.User.CompanyId,
                    PalletId = Uuid.GetUuId(),
                    PalletItems = palletItems,
                    ReferenceKey = "",
                    StockLocation = StockLocation.PartnerId,
                    StockLocationId = StockLocation.PartnerId,
                    StockLocationName = StockLocation.FullName,
                    Tags = SimpleIoc.Default.GetInstance<ScanKegsViewModel>().Tags
                };

                var value = await _palletizeService.PostPalletAsync(palletRequestModel, AppSettings.User.SessionId, Configuration.NewPallet);

                if (value.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SimpleIoc.Default.GetInstance<PalletizeDetailViewModel>().LoadInfo(value);

                    Loader.StopLoading();
                    await Application.Current.MainPage.Navigation.PushModalAsync(new PalletizeDetailView());
                }
                else
                {
                    Loader.StopLoading();
                    SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        private void BarcodeScanCommandReciever()
        {
            SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeScanCommandReciever();
        }

        private void IsPalletVisibleCommandReciever()
        {
            IsCameraVisible = true;
        }

        private async void AddKegsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView());
        }

        private async void AddTagsCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView());
        }

        private async void PartnerCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }
        private async void TargetLocationPartnerCommandRecieverAsync()
        {
            TargetLocationPartner = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView());
        }

        private async void CancelCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
