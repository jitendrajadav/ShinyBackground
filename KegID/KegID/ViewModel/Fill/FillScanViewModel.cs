using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class FillScanViewModel : BaseViewModel
    {
        #region Properties

        protected IDiscoveredPrinter myPrinter;

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
            MessagingCenter.Subscribe<FillScanMessage>(this, "FillScanMessage", message => {
                Device.BeginInvokeOnMainThread(() => {
                    var value = message;
                        if (value.Barcodes != null)
                        {
                            var barode = BarcodeCollection.Where(x => x.Id == value.Barcodes.Id).FirstOrDefault();
                            barode.Icon = value.Barcodes.Icon;
                            barode.Partners = value.Barcodes.Partners;
                            barode.MaintenanceItems = value.Barcodes.MaintenanceItems;
                            //barode.Tags = value.Barcodes.Tags;
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

        public async void GenerateManifestIdAsync(PalletModel palletModel)
        {
            try
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
            DateTime now = DateTime.Now;
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
                await SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
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
                    BarcodeCollection.Add(new Barcode { Id = ManaulBarcode, Tags = Tags, TagsStr = TagsStr, Icon = Cloud });
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

        private void SendZplPallet()
        {
            IConnection connection = null;
            myPrinter = AppSettings.Printer;
            try
            {
                connection = myPrinter.Connection;
                connection.Open();
                IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);
                if ((!CheckPrinterLanguage(connection)) || (!PreCheckPrinterStatus(printer)))
                {
                    return;
                }

                #region ZPL Printer format
               
                /*
                        This routine is provided to you as an example of how to create a variable length label with user specified data.
                        The basic flow of the example is as follows

                           Header of the label with some variable data
                           Body of the label
                               Loops thru user content and creates small line items of printed material
                           Footer of the label

                        As you can see, there are some variables that the user provides in the header, body and footer, and this routine uses that to build up a proper ZPL string for printing.
                        Using this same concept, you can create one label for your receipt header, one for the body and one for the footer. The body receipt will be duplicated as many items as there are in your variable data

                        */

                String tmpHeader =
                                    /*
                                     Some basics of ZPL. Find more information here : http://www.zebra.com

                                     ^XA indicates the beginning of a label
                                     ^PW sets the width of the label (in dots)
                                     ^MNN sets the printer in continuous mode (variable length receipts only make sense with variably sized labels)
                                     ^LL sets the length of the label (we calculate this value at the end of the routine)
                                     ^LH sets the reference axis for printing. 
                                        You will notice we change this positioning of the 'Y' axis (length) as we build up the label. Once the positioning is changed, all new fields drawn on the label are rendered as if '0' is the new home position
                                     ^FO sets the origin of the field relative to Label Home ^LH
                                     ^A sets font information 
                                     ^FD is a field description
                                     ^GB is graphic boxes (or lines)
                                     ^B sets barcode information
                                     ^XZ indicates the end of a label
                                     */

                                    "^XA" +
                                    "^FX--the box--^FS" +
                                    "^FO25,25" +
                                    "^GB775,1175,4,B,0 ^FS" +
                                    "^FX--the kegid text top left-- ^FS" +
                                    "^FO50,50" +
                                    "^AVB,40,40" +
                                    "^FDKegID ^FS" +
                                    "^FX--the pallet num-- ^FS" +
                                    "^FO200,50" +
                                    "^A0N,56" +
                                    "^FD{0}^FS" + //@@PALLETBARCODE@@
                                    "^FX--ownername, top right--^FS" +
                                    "^FO775,110,1" +
                                    "^A0N,42" +
                                    "^FD{1}^FS" + //@@OWNERNAME@@
                                    "^FX--owner address--^FS" +
                                    "^FO775,160,1" +
                                    "^A0N,32" +
                                    "^FD{2}^FS" +   //@@OWNERADDRESS@@
                                    "^FO775,195,1" +
                                    "^A0N,32" +
                                    "^FD{3}^FS" +   //@@OWNERCSZ@@
                                    "^FO775,225,1" +
                                    "^A0N,32" +
                                    "^FD{4}^FS" +   //@@OWNERPHONE@@
                                    "^FX--a line under the owner-- ^FS" +
                                    "^FO25,275" +
                                    "^GB750,1,4,B,1 ^FS" +
                                    "^FX--build location^FS" +
                                    "^FO50,300,0" +
                                    "^ADN,16" +
                                    "^FDBUILD LOCATION ^FS" +
                                    "^FO75,325,0" +
                                    "^A0N,32" +
                                    "^FD{5}^FS" +   //@@STOCKNAME@@
                                    "^FX--batch ^FS" +
                                    "^FO50,375,0" +
                                    "^ADN,16" +
                                    "^FDBATCH ^FS" +
                                    "^FO75,400,0" +
                                    "^A0N,32" +
                                    "^FD{6}^FS" +   //@@BATCHNUM@@
                                    "^FX--build date^FS" +
                                    "^FO350,375,0" +
                                    "^ADN,16" +
                                    "^FDBUILD DATE ^FS" +
                                    "^FO375,400,0" +
                                    "^A0N,32" +
                                    "^FD{7}^FS" +   //@@BUILDDATE@@
                                    "^FX--the kegs box-- ^FS" +
                                    "^FO800,275,1" +
                                    "^GB200,200,3,B,0 ^FS" +
                                    "^FX--the num of kegs^FS" +
                                    "^FO775,325,1" +
                                    "^A0N,128" +
                                    "^FD{8}^FS" +   //@@TOTALKEGS@@
                                    "^FX--a line under the header-- ^FS" +
                                    "^FO25,475" +
                                    "^GB750,1,4,B,1 ^FS" +
                                    "^FX--summary line 1, box 1-- ^FS" +
                                    "^FO25,475" +
                                    "^GB250,40,2,B,0 ^FS" +
                                    "^FX--summary line 1, text 1-- ^FS" +
                                    "^FO40,485" +
                                    "^AFN,18,10" +
                                    "^FD{9}^FS" +   //@@SIZE1@@
                                    "^FX--summary line 1, box 2-- ^FS" +
                                    "^FO275,475" +
                                    "^GB425,40,2,B,0 ^FS" +
                                    "^FX--summary line 1, text 2-- ^FS" +
                                    "^FO290,485" +
                                    "^AFN,18,10" +
                                    "^FD{10}^FS" +  //@@CONTENTS1@@
                                    "^FX--summary line 1, box 3-- ^FS" +
                                    "^FO800,475,1" +
                                    "^GB100,40,2,B,0 ^FS" +
                                    "^FX--summary line 1, text 3-- ^FS" +
                                    "^FO785,485,1" +
                                    "^AFN,18,10" +
                                    "^FD{11}^FS" + "\r\n" +  //@@QTY1@@
                                    "^FX--summary line 2, box 1-- ^FS" +
                                    "^FO25,515" +
                                    "^GB250,40,2,B,0 ^FS" +
                                    "^FX--summary line 2, text 1-- ^FS" +
                                    "^FO40,525" +
                                    "^AFN,18,10" +
                                    "^FD{12}^FS" +  //@@SIZE2@@
                                    "^FX--summary line 2, box 2-- ^FS" +
                                    "^FO275,515" +
                                    "^GB425,40,2,B,0 ^FS" +
                                    "^FX--summary line 2, text 2-- ^FS" +
                                    "^FO290,525" +
                                    "^AFN,18,10" +
                                    "^FD{13}^FS" +  //@@CONTENTS2@@
                                    "^FX--summary line 2, box 3-- ^FS" +
                                    "^FO800,515,1" +
                                    "^GB100,40,2,B,0 ^FS" +
                                    "^FX--summary line 2, text 3-- ^FS" +
                                    "^FO785,525,1" +
                                    "^AFN,18,10" +
                                    "^FD{14}^FS" + "\r\n" + //@@QTY2@@
                                    "^FX--summary line 3, box 1-- ^FS" +
                                    "^FO25,555" +
                                    "^GB250,40,2,B,0 ^FS" +
                                    "^FX--summary line 3, text 1-- ^FS" +
                                    "^FO40,565" +
                                    "^AFN,18,10" +
                                    "^FD{15}^FS" +  //@@SIZE3@@
                                    "^FX--summary line 3, box 2-- ^FS" +
                                    "^FO275,555" +
                                    "^GB425,40,2,B,0 ^FS" +
                                    "^FX--summary line 3, text 2-- ^FS" +
                                    "^FO290,565" +
                                    "^AFN,18,10" +
                                    "^FD{16}^FS" +  //@@CONTENTS3@@
                                    "^FX--summary line 3, box 3-- ^FS" +
                                    "^FO800,555,1" +
                                    "^GB100,40,2,B,0 ^FS" +
                                    "^FX--summary line 3, text 3-- ^FS" +
                                    "^FO785,565,1" +
                                    "^AFN,18,10" +
                                    "^FD{17}^FS" + "\r\n" + //@@QTY3@@
                                    "^FX--summary line 4, box 1-- ^FS" +
                                    "^FO25,595" +
                                    "^GB250,40,2,B,0 ^FS" +
                                    "^FX--summary line 4, text 1-- ^FS" +
                                    "^FO40,605" +
                                    "^AFN,18,10" +
                                    "^FD{18}^FS" +  //@@SIZE4@@
                                    "^FX--summary line 4, box 2-- ^FS" +
                                    "^FO275,595" +
                                    "^GB425,40,2,B,0 ^FS" +
                                    "^FX--summary line 4, text 2-- ^FS" +
                                    "^FO290,605" +
                                    "^AFN,18,10" +
                                    "^FD{19}^FS" +  //@@CONTENTS4@@
                                    "^FX--summary line 4, box 3-- ^FS" +
                                    "^FO800,595,1" +
                                    "^GB100,40,2,B,0 ^FS" +
                                    "^FX--summary line 4, text 3-- ^FS" +
                                    "^FO785,605,1" +
                                    "^AFN,18,10" +
                                    "^FD{20}^FS" + "\r\n" + //@@QTY4@@
                                    "^FX--summary line 5, box 1-- ^FS" +
                                    "^FO25,635" +
                                    "^GB250,40,2,B,0 ^FS" +
                                    "^FX--summary line 5, text 1-- ^FS" +
                                    "^FO40,645" +
                                    "^AFN,18,10" +
                                    "^FD{21}^FS" +  //@@SIZE5@@
                                    "^FX--summary line 5, box 2-- ^FS" +
                                    "^FO275,635" +
                                    "^GB425,40,2,B,0 ^FS" +
                                    "^FX--summary line 5, text 2-- ^FS" +
                                    "^FO290,645" +
                                    "^AFN,18,10" +
                                    "^FD{22}^FS" +  //@@CONTENTS5@@
                                    "^FX--summary line 5, box 3-- ^FS" +
                                    "^FO800,635,1" +
                                    "^GB100,40,2,B,0 ^FS" +
                                    "^FX--summary line 5, text 3-- ^FS" +
                                    "^FO785,645,1" +
                                    "^AFN,18,10" +
                                    "^FD{23}^FS" + "\r\n" + //@@QTY5@@
                                    "^FX--summary line 6, box 1-- ^FS" +
                                    "^FO25,675" +
                                    "^GB250,40,2,B,0 ^FS" +
                                    "^FX--summary line 6, text 1-- ^FS" +
                                    "^FO40,685" +
                                    "^AFN,18,10" +
                                    "^FD{24}^FS" +  //@@SIZE6@@
                                    "^FX--summary line 6, box 2-- ^FS" +
                                    "^FO275,675" +
                                    "^GB425,40,2,B,0 ^FS" +
                                    "^FX--summary line 6, text 2-- ^FS" +
                                    "^FO290,685" +
                                    "^AFN,18,10" +
                                    "^FD{25}^FS" +  //@@CONTENTS6@@
                                    "^FX--summary line 6, box 3-- ^FS" +
                                    "^FO800,675,1" +
                                    "^GB100,40,2,B,0 ^FS" +
                                    "^FX--summary line 6, text 3-- ^FS" +
                                    "^FO785,685,1" +
                                    "^AFN,18,10" +
                                    "^FD{26}^FS" + "\r\n" + "\r\n" +    //@@QTY6@@
                                    "^FX--overflow ~line 6, box-- ^FS" +
                                    "^FO25,715" +
                                    "^GB775,60,2,B,0 ^FS" +
                                    "^FX--summary line 7, text 1-- ^FS" +
                                    "^FO40,725" +
                                    "^AFN,16" +
                                    "^FD{27}^FS" + "\r\n" +     //@@MORESUMMARY@@
                                    "^FX--a line under the summary-- ^FS" +
                                    "^FO25,275" +
                                    "^GB750,1,4,B,1 ^FS" + "\r\n" +
                                    "^FX--the SSCC label-- ^FS" +
                                    "^FO50,780,0" +
                                    "^ADN,16" +
                                    "^FDSSCC ^FS" + "\r\n" + "\r\n" +
                                    "^FX--prepared for--^FS" +
                                    "^FO775, 780, 1" +
                                    "^ADN, 16" +
                                    "^FDPREPARED FOR ^FS" +
                                    "^FX--a line under the prepared-- ^FS" +
                                    "^FO800, 805, 1" +
                                    "^GB200, 1, 2, B, 1 ^FS" +
                                    "^FX--target location-- ^FS" +
                                    "^FO775, 820, 1" +
                                    "^A0N, 42" +
                                    "^FD{28}^FS" +  //@@TARGETNAME@@
                                    "^FX--owner address-- ^FS" +
                                    "^FO775, 860, 1" +
                                    "^A0N, 32" +
                                    "^FD{29}^FS" +  //@@TARGETADDRESS@@
                                    "^FO775, 900, 1" +
                                    "^A0N, 32" +
                                    "^FD{30}^FS" +  //@@TARGETCSZ@@
                                    "^FO775, 940, 1" +
                                    "^A0N, 32" +
                                    "^FD{31}^FS" + "\r\n" + "\r\n" + "\r\n" + "\r\n" +  //@@TARGETPHONE@@
                                    "^FX--the QR barcode-- ^FS" +
                                    "^FO50, 800, 0" +
                                    "^BQN, 2, 9, Q" +
                                    "^FDQA,{32}^FS" + "\r\n" +  //@@PALLETBARCODE@@
                                    "^FX--the code128 barcode-- ^FS" +
                                    "^FO50, 1050, 0 ^BY2" +
                                    "^BCN, 100, Y, N, N" +
                                    "^FD{33}^FS" + "\r\n" + "\r\n" + //@@PALLETBARCODE@@
                                    "^FX--the kegid text bottom right-- ^FS" +
                                    "^FO675, 1000, 0" +
                                    "^AVB, 48, 40" +
                                    "^FDKegID ^FS" +
                                    "^XZ";

                string header = string.Format(tmpHeader, BatchId, PartnerModel.ParentPartnerName, PartnerModel.Address1, PartnerModel.City + "," + PartnerModel.State + " " + PartnerModel.PostalCode, "", PartnerModel.ParentPartnerName, BatchModel.BatchCode, DateTime.Now.ToShortDateString(), "1", "", BatchModel.BrandName,
                    "1", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "",
                    "", BatchId, BatchId); 

                #endregion

                connection.Write(GetBytes(header));
            }
            catch (Exception ex)
            {
                // Connection Exceptions and issues are caught here
                Crashes.TrackError(ex);
            }
            finally
            {
                if ((connection != null) && (connection.IsConnected))
                    connection.Close();
            }
        }

        protected static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length];
            bytes = Encoding.UTF8.GetBytes(str);
            return bytes;
        }

        private Dictionary<string, string> CreateListOfItems()
        {
            String[] names = { "Microwave Oven", "Sneakers (Size 7)", "XL T-Shirt", "Socks (3-pack)", "Blender", "DVD Movie" };
            String[] prices = { "79.99", "69.99", "39.99", "12.99", "34.99", "16.99" };
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            for (int ix = 0; ix < names.Length; ix++)
            {
                retVal.Add(names[ix], prices[ix]);
            }
            return retVal;
        }

        protected bool CheckPrinterLanguage(IConnection connection)
        {
            if (!connection.IsConnected)
                connection.Open();
            //  Check the current printer language
            byte[] response = connection.SendAndWaitForResponse(GetBytes("! U1 getvar \"device.languages\"\r\n"), 500, 100);
            string language = Encoding.UTF8.GetString(response, 0, response.Length);
            if (language.Contains("line_print"))
            {
                Application.Current.MainPage.DisplayAlert("Switching printer to ZPL Control Language.", "Notification","Ok");
            }
            // printer is already in zpl mode
            else if (language.Contains("zpl"))
            {
                return true;
            }

            //  Set the printer command languege
            connection.Write(GetBytes("! U1 setvar \"device.languages\" \"zpl\"\r\n"));
            response = connection.SendAndWaitForResponse(GetBytes("! U1 getvar \"device.languages\"\r\n"), 500, 100);
            language = Encoding.UTF8.GetString(response, 0, response.Length);
            if (!language.Contains("zpl"))
            {
                Application.Current.MainPage.DisplayAlert("Printer language not set. Not a ZPL printer.", "Ok", "Ok");
                return false;
            }
            return true;
        }

        protected bool PreCheckPrinterStatus(IZebraPrinter printer)
        {
            // Check the printer status
            IPrinterStatus status = printer.CurrentStatus;
            if (!status.IsReadyToPrint)
            {
                Application.Current.MainPage.DisplayAlert("Unable to print. Printer is " + status.Status,"test","test");
                return false;
            }
            return true;
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
                    SendZplPallet();
                    await SimpleIoc.Default.GetInstance<AddPalletsViewModel>().AssignValueToAddPalletAsync(BatchId, BarcodeCollection);
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
