using GalaSoft.MvvmLight.Command;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using KegID.Common;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Microsoft.AppCenter.Crashes;
using KegID.Messages;
using Realms;
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class ScanKegsViewModel : BaseViewModel
    {
        #region Properties

        private const string Maintenace = "maintenace.png";
        private const string ValidationOK = "validationok.png";
        private const string Cloud = "collectionscloud.png";

        public bool HasDone { get; set; }
        public bool IsFromScanned { get; set; }
        public IMoveService _moveService { get; set; }

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

        #region BrandCollection

        /// <summary>
        /// The <see cref="BrandCollection" /> property's name.
        /// </summary>
        public const string BrandCollectionPropertyName = "BrandCollection";

        private IList<BrandModel> _BrandCollection = null;

        /// <summary>
        /// Sets and gets the BrandCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<BrandModel> BrandCollection
        {
            get
            {
                return _BrandCollection;
            }

            set
            {
                if (_BrandCollection == value)
                {
                    return;
                }

                _BrandCollection = value;
                RaisePropertyChanged(BrandCollectionPropertyName);
            }
        }


        #endregion

        #region SelectedBrand

        /// <summary>
        /// The <see cref="SelectedBrand" /> property's name.
        /// </summary>
        public const string SelectedBrandPropertyName = "SelectedBrand";

        private BrandModel _SelectedBrand = null;

        /// <summary>
        /// Sets and gets the SelectedBrand property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public BrandModel SelectedBrand
        {
            get
            {
                return _SelectedBrand;
            }

            set
            {
                if (_SelectedBrand == value)
                {
                    return;
                }

                _SelectedBrand = value;
                RaisePropertyChanged(SelectedBrandPropertyName);
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

        #region Batch

        /// <summary>
        /// The <see cref="Batch" /> property's name.
        /// </summary>
        public const string BatchPropertyName = "Batch";

        private string _Batch = default(string);

        /// <summary>
        /// Sets and gets the Batch property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Batch
        {
            get
            {
                return _Batch;
            }

            set
            {
                if (_Batch == value)
                {
                    return;
                }

                _Batch = value;
                TagsStr = "Batch : " +_Batch;
                RaisePropertyChanged(BatchPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand DoneCommand { get; }
        public RelayCommand BarcodeScanCommand { get; }
        public RelayCommand BarcodeManualCommand { get; }
        public RelayCommand AddTagsCommand { get; }
        public RelayCommand<Barcode> IconItemTappedCommand { get; }
        public RelayCommand<Barcode> LabelItemTappedCommand { get; }

        #endregion

        #region Constructor

        public ScanKegsViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            DoneCommand = new RelayCommand(DoneCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandReciever);
            BarcodeManualCommand = new RelayCommand(BarcodeManualCommandRecieverAsync);
            AddTagsCommand = new RelayCommand(AddTagsCommandRecieverAsync);
            LabelItemTappedCommand = new RelayCommand<Barcode>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new RelayCommand<Barcode>((model) => IconItemTappedCommandRecieverAsync(model));
            LoadBrand();

            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<ScanKegsMessage>(this, "ScanKegsMessage", message => {
                Device.BeginInvokeOnMainThread(() => {
                   var value = message;
                        if (value != null)
                        {
                            var barode = BarcodeCollection.Where(x => x.Id == value.Barcodes.Id).FirstOrDefault();
                            barode.Icon = value.Barcodes.Icon;
                            //barode.Partners = value.Barcodes.Partners;
                            //barode.MaintenanceItems = value.Barcodes.MaintenanceItems;
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

        public async void LoadBrand() => BrandCollection = await LoadBrandAsync();

        public async Task<IList<BrandModel>> LoadBrandAsync()
        {
            var RealmDb = Realm.GetInstance();
            var all = RealmDb.All<BrandModel>().ToList();
            IList<BrandModel> model = all;// await SQLiteServiceClient.Db.Table<BrandModel>().ToListAsync();
            try
            {
                if (model.Count > 0)
                    return model;
                else
                {
                    Loader.StartLoading();
                    var value = await _moveService.GetBrandListAsync(AppSettings.User.SessionId);

                    if (value.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        model = value.BrandModel;
                        await RealmDb.WriteAsync((realmDb) =>
                        {
                            foreach (var item in value.BrandModel)
                            {
                                realmDb.Add(item);
                            }
                        });

                        //await SQLiteServiceClient.Db.InsertAllAsync(model);
                    }
                }
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
                    IsFromScanned = true;
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
                    if (model.Partners?.FirstOrDefault()?.Kegs.FirstOrDefault().MaintenanceItems?.Count > 0)
                    {
                        string strAlert = string.Empty;
                        for (int i = 0; i < model.MaintenanceItems.Count; i++)
                        {
                            strAlert += "-" + model.MaintenanceItems[i].Name + "\n";
                            if (model.Partners.FirstOrDefault().Kegs.FirstOrDefault().MaintenanceItems.Count == i)
                            {
                                break;
                            }
                        }
                        await Application.Current.MainPage.DisplayAlert("Warning", "This keg needs the following maintenance performed:\n" + strAlert, "Ok");
                    }
                    else
                    {
                        if (model.Icon == "validationerror.png")
                        {
                            await Application.Current.MainPage.DisplayAlert("Warning", "This scan could not be verified", "Keep", "Delete");
                        }
                        else
                        {
                            await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView(), animated: false);
                            SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static async Task NavigateToValidatePartner(List<Barcode> model)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
                SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
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

        private async void DoneCommandRecieverAsync()
        {
            HasDone = true;
            var alert = BarcodeCollection.Where(x => x.Icon == "maintenace.png").ToList();

            if (alert.Count > 0 && !alert.FirstOrDefault().HasMaintenaceVerified)
            {
                try
                {
                    string strBarcode = alert.FirstOrDefault().Id;
                    var option = await Application.Current.MainPage.DisplayActionSheet("No keg with a barcode of \n" + strBarcode + " could be found",
                        null, null, "Remove unverified scans", "Assign sizes", "Countinue with current scans", "Stay here");
                    switch (option)
                    {
                        case "Remove unverified scans":
                            BarcodeCollection.Remove(alert.FirstOrDefault());
                            await Application.Current.MainPage.Navigation.PopModalAsync();
                            Cleanup();

                            break;
                        case "Assign sizes":
                            SimpleIoc.Default.GetInstance<AssignSizesViewModel>().AssignInitialValueAsync(alert);
                            await Application.Current.MainPage.Navigation.PushModalAsync(new AssignSizesView(), animated: false);
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
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
                {
                    case ViewTypeEnum.MoveView:
                        if (!BarcodeCollection.Any(x => x.Partners?.Count > 1))
                            SimpleIoc.Default.GetInstance<MoveViewModel>().AssingScanKegsValue(BarcodeCollection.ToList(), Tags, SelectedBrand?.BrandName);
                        break;

                    case ViewTypeEnum.PalletizeView:
                        SimpleIoc.Default.GetInstance<PalletizeViewModel>().AssingScanKegsValue(BarcodeCollection);
                        break;

                    default:
                        break;
                }

                if (BarcodeCollection.Any(x => x.Partners?.Count > 1))
                    await NavigateToValidatePartner(BarcodeCollection.Where(x => x.Partners?.Count > 1).ToList());
                else
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    Cleanup();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignSizesValue(List<Barcode> verifiedBarcodes)
        {
            try
            {
                foreach (var item in verifiedBarcodes)
                {
                    BarcodeCollection.Where(x => x.Id == item.Id).FirstOrDefault().HasMaintenaceVerified = item.HasMaintenaceVerified;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignInitialValue(string _barcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(_barcode))
                    BarcodeCollection.Add(new Barcode { Id = _barcode });

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BarcodeManualCommandRecieverAsync()
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

                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        var RealmDb = Realm.GetInstance();
                        await RealmDb.WriteAsync((realmDb) => 
                        {
                            var Result = realmDb.Add(barcode);
                        }); 
                    }

                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = new List<string>() { ManaulBarcode },
                        Page = ViewTypeEnum.ScanKegsView
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

        internal async void BarcodeScanCommandReciever()
        {
            try
            {
                await BarcodeScanner.BarcodeScanAsync(_moveService, Tags, TagsStr, ViewTypeEnum.ScanKegsView);
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

        internal async void AssignValidatedValueAsync(Partner model)
        {
            try
            {
                BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Partners.Clear();
                if (model.Kegs.FirstOrDefault().MaintenanceItems?.Count > 0)
                {
                    BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon(Maintenace);
                }
                else
                {
                    BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon(ValidationOK);
                }

                BarcodeCollection.Where(x => x.Id == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Partners.Add(model);
                SimpleIoc.Default.GetInstance<MoveViewModel>().AssingScanKegsValue(_barcodes: BarcodeCollection.ToList(), _tags: Tags, _contents: SelectedBrand?.BrandName);

                if (HasDone && model.Kegs?.FirstOrDefault()?.MaintenanceItems?.Count < 0)
                {
                    await Application.Current.MainPage.Navigation.PopPopupAsync();
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    Cleanup();
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PopPopupAsync();
                }
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
                IsFromScanned = false;
                Tags = _tags;
                TagsStr = _tagsStr;
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
                base.Cleanup();
                BarcodeCollection.Clear();
                TagsStr = default(string);
                SelectedBrand = null;
                HasDone = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
