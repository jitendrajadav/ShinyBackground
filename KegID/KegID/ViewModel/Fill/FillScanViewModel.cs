using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Realms;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";
        public IMoveService _moveService { get; set; }
        public string BatchId { get; set; }
        public BatchModel BatchModel { get; private set; }
        public string SizeButtonTitle { get; private set; }
        public PartnerModel PartnerModel { get; private set; }

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
            SubmitCommand = new RelayCommand(SubmitCommandRecieverAsync);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            LabelItemTappedCommand = new RelayCommand<Barcode>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>((model) => IconItemTappedCommandRecieverAsync(model));

            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<FillScanMessage>(this, "FillScanMessage", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value.Barcodes != null)
                    {
                        var barode = BarcodeCollection.Where(x => x.Id == value.Barcodes.Id).FirstOrDefault();
                        barode.Icon = value.Barcodes.Icon;
                        foreach (var item in value.Barcodes.Partners)
                        {
                            barode.Partners.Add(item);
                        }
                        foreach (var item in value.Barcodes.MaintenanceItems)
                        {
                            barode.MaintenanceItems.Add(item);
                        }
                        foreach (var item in value.Barcodes.Tags)
                        {
                            barode.Tags.Add(item);
                        }
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

        internal void AssignInitValue(BatchModel batchModel  , string sizeButtonTitle, PartnerModel partnerModel)
        {
            BatchModel = batchModel;
            SizeButtonTitle = sizeButtonTitle;
            PartnerModel = partnerModel;
        }

        internal async void AssignValidateBarcodeValueAsync()
        {
            try
            {
                SimpleIoc.Default.GetInstance<AddPalletsViewModel>().AssignFillScanValue(BarcodeCollection, BatchId);

                await Application.Current.MainPage.Navigation.PopPopupAsync();

                if (IsPalletze)
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                else
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanReviewView(), animated: false);
                    SimpleIoc.Default.GetInstance<FillScanReviewViewModel>().AssignInitialValue(BatchId, BarcodeCollection.Count);
                }
                //Cleanup();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void GenerateManifestIdAsync(PalletModel palletModel)
        {
            try
            {
                DateTimeOffset now = DateTimeOffset.Now;
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
                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        var preference = RealmDb.All<Preference>().Where(x => x.PreferenceName == "DashboardPreferences").ToList();
                        //await SQLiteServiceClient.Db.Table<Preference>().Where(x => x.PreferenceName == "DashboardPreferences").ToListAsync();

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
                        var checksumDigit = PermissionsUtils.CalculateCheckDigit(barCode);
                        BatchId = barCode + checksumDigit;
                        ManifestId = "Pallet #" + BatchId;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static int SecondsInDayTillNow()
        {
            DateTimeOffset now = DateTimeOffset.Now;
            int hours = 0, minutes = 0, seconds = 0, totalSeconds = 0;
            hours = (24 - now.Hour) - 1;
            minutes = (60 - now.Minute) - 1;
            seconds = (60 - now.Second - 1);

            return totalSeconds = seconds + (minutes * 60) + (hours * 3600);
        }

        private async void LabelItemTappedCommandRecieverAsync(Barcode model)
        {
            try
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
                    await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView(), animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void IconItemTappedCommandRecieverAsync(Barcode model)
        {
            try
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
                    SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(_barcode: model);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task NavigateToValidatePartner(List<Barcode> model)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView(), animate: false);
                SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
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
                var isNew = BarcodeCollection.ToList().Any(x => x.Id == ManaulBarcode);
                if (!isNew)
                {
                    Barcode barcode = new Barcode
                    {
                        Id = ManaulBarcode,
                        //Tags = Tags,
                        TagsStr = TagsStr,
                        Icon = Cloud
                    };
                    BarcodeCollection.Add(barcode);
                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = new List<string>() { ManaulBarcode },
                        Page = ViewTypeEnum.FillScanView
                    };
                    MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                    ManaulBarcode = string.Empty;

                    //var value = await BarcodeScanner.ValidateBarcodeInsertIntoLocalDB(_moveService, ManaulBarcode, Tags, TagsStr,ViewTypeEnum.FillScanView);
                    //ManaulBarcode = string.Empty;
                    //BarcodeCollection.Add(value);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SubmitCommandRecieverAsync()
        {
            try
            {
                var result = BarcodeCollection.Where(x => x.Partners.Count > 1).ToList();
                if (result.Count > 0)
                    await NavigateToValidatePartner(result.ToList());
                else
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new FillScanReviewView(), animated: false);
                    SimpleIoc.Default.GetInstance<FillScanReviewViewModel>().AssignInitialValue(BatchId, BarcodeCollection.Count);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
            try
            {
                var result = BarcodeCollection.Where(x => x.Partners.Count > 1).ToList();
                if (result.Count > 0)
                    await NavigateToValidatePartner(result.ToList());
                else
                {
                    new Task(new Action(() => {
                        PrintPallet();
                    })).Start();
                    await SimpleIoc.Default.GetInstance<AddPalletsViewModel>().AssignValueToAddPalletAsync(BatchId, BarcodeCollection);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void PrintPallet()
        {
            try
            {
                string header = string.Format(ZebraPrinterManager.palletHeader, BatchId, PartnerModel.ParentPartnerName, PartnerModel.Address1, PartnerModel.City + "," + PartnerModel.State + " " + PartnerModel.PostalCode, "", PartnerModel.ParentPartnerName, BatchModel.BatchCode, DateTimeOffset.UtcNow.Date.ToShortDateString(), "1", "", BatchModel.BrandName,
                                    "1", "", "", "", "", "", "", "", "", "",
                                    "", "", "", "", "", "", "", "", "", "",
                                    "", BatchId, BatchId);

                ZebraPrinterManager.SendZplPallet(header);
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
                await Application.Current.MainPage.Navigation.PopModalAsync();
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
                await BarcodeScanner.BarcodeScanAsync(_moveService, Tags, TagsStr, ViewTypeEnum.FillScanView);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignBarcodeScannerValue(IList<Barcode> barcodes)
        {
            try
            {
                foreach (var item in barcodes)
                    BarcodeCollection.Add(item);
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

        private async void AddTagsCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignValidatedValue(Partner model)
        {
            try
            {
                BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Partners.Clear();
                BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon("validationquestion.png");
                BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Partners.Add(model);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void Cleanup()
        {
            try
            {
                BarcodeCollection.Clear();
                Tags = null;
                TagsStr = string.Empty;
                base.Cleanup();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
