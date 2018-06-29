using KegID.Model;
using KegID.Services;
using System.Linq;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using KegID.Common;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using KegID.Messages;
using Realms;
using Xamarin.Essentials;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class ScanKegsViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        private const string Maintenace = "maintenace.png";
        private const string ValidationOK = "validationok.png";
        private const string Cloud = "collectionscloud.png";

        public bool HasDone { get; set; }
        //public bool IsFromScanned { get; set; }
        public IMoveService _moveService { get; set; }

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

        //#region Tags
        ///// <summary>
        ///// The <see cref="Tags" /> property's name.
        ///// </summary>
        //public const string TagsPropertyName = "Tags";

        //private List<Tag> _tags = new List<Tag>();

        ///// <summary>
        ///// Sets and gets the Tags property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// </summary>
        //public List<Tag> Tags
        //{
        //    get
        //    {
        //        return _tags;
        //    }

        //    set
        //    {
        //        if (_tags == value)
        //        {
        //            return;
        //        }

        //        _tags = value;
        //        RaisePropertyChanged(TagsPropertyName);
        //    }
        //}

        //#endregion

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

        public DelegateCommand DoneCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand BarcodeManualCommand { get; }
        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand<BarcodeModel> IconItemTappedCommand { get; }
        public DelegateCommand<BarcodeModel> LabelItemTappedCommand { get; }

        #endregion

        #region Constructor

        public ScanKegsViewModel(IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;

            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandReciever);
            BarcodeManualCommand = new DelegateCommand(BarcodeManualCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            LabelItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => LabelItemTappedCommandRecieverAsync(model));
            IconItemTappedCommand = new DelegateCommand<BarcodeModel>((model) => IconItemTappedCommandRecieverAsync(model));
            LoadBrand();

            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
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
                            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                            RealmDb.Write(() =>
                            {
                                var oldBarcode = BarcodeCollection.Where(x => x.Barcode == value.Barcodes.Barcode).FirstOrDefault();
                                oldBarcode.Pallets = value.Barcodes.Pallets;
                                oldBarcode.Kegs = value.Barcodes.Kegs;
                                oldBarcode.Icon = value?.Barcodes?.Kegs?.Partners.Count > 1 ? GetIconByPlatform.GetIcon("validationquestion.png") : value?.Barcodes?.Kegs?.Partners?.Count == 0 ? GetIconByPlatform.GetIcon("validationerror.png") : GetIconByPlatform.GetIcon("validationok.png");
                                oldBarcode.IsScanned = true;
                            });
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

        public void LoadBrand() => BrandCollection = LoadBrandAsync();

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
                    await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), useModalNavigation: true, animated: false);
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
                            //await Application.Current.MainPage.Navigation.PushModalAsync(new ScanInfoView(), animated: false);
                            //SimpleIoc.Default.GetInstance<ScanInfoViewModel>().AssignInitialValue(model);
                            var param = new NavigationParameters
                            {
                                { "model", model }
                            };
                            await _navigationService.NavigateAsync(new Uri("ScanInfoView", UriKind.Relative), param, useModalNavigation: true, animated: false);
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
                //await Application.Current.MainPage.Navigation.PushPopupAsync(new ValidateBarcodeView());
                //SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>().LoadBarcodeValue(model);
                var param = new NavigationParameters
                            {
                                { "model", model }
                            };
                await _navigationService.NavigateAsync(new Uri("ValidateBarcodeView", UriKind.Relative), param, useModalNavigation: true, animated: false);
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
                await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), useModalNavigation: true, animated: false);
                //await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView(), animated: false);
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
                    string strBarcode = alert.FirstOrDefault().Barcode;
                    var option = await Application.Current.MainPage.DisplayActionSheet("No keg with a barcode of \n" + strBarcode + " could be found",
                        null, null, "Remove unverified scans", "Assign sizes", "Countinue with current scans", "Stay here");
                    switch (option)
                    {
                        case "Remove unverified scans":
                            BarcodeCollection.Remove(alert.FirstOrDefault());
                            //await Application.Current.MainPage.Navigation.PopModalAsync();
                            await _navigationService.GoBackAsync(useModalNavigation:true, animated: false);
                            Cleanup();

                            break;
                        case "Assign sizes":
                            //SimpleIoc.Default.GetInstance<AssignSizesViewModel>().AssignInitialValueAsync(alert);
                            //await Application.Current.MainPage.Navigation.PushModalAsync(new AssignSizesView(), animated: false);
                            var param = new NavigationParameters
                            {
                                { "alert", alert }
                            };
                            await _navigationService.NavigateAsync(new Uri("AssignSizesView", UriKind.Relative), param, useModalNavigation: true, animated: false);

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
                //var formsNav = ((Prism.Common.IPageAware)_navigationService).Page;
                //switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), formsNav.Navigation.ModalStack.LastOrDefault().GetType().Name))
                //{
                //    case ViewTypeEnum.MoveView:
                //        if (!BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                //        {
                //            //SimpleIoc.Default.GetInstance<MoveViewModel>().AssingScanKegsValue(BarcodeCollection.ToList(), Tags, SelectedBrand?.BrandName);
                //            ScanKegToMovePagesMsg pageMsg = new ScanKegToMovePagesMsg
                //            {
                //                Barcodes = BarcodeCollection.ToList(),
                //                Contents = SelectedBrand?.BrandName,
                //                Tags = ConstantManager.Tags
                //            };
                //            MessagingCenter.Send(pageMsg, "ScanKegToMovePagesMsg");
                //        }
                //        break;

                //    case ViewTypeEnum.PalletizeView:
                //        //SimpleIoc.Default.GetInstance<PalletizeViewModel>().AssingScanKegsValue(BarcodeCollection);

                //        ScanKegToPalletPagesMsg palletMsg = new ScanKegToPalletPagesMsg
                //        {
                //            Barcodes = BarcodeCollection.ToList(),
                //        };
                //        MessagingCenter.Send(palletMsg, "ScanKegToPalletPagesMsg");
                //        break;

                //    default:
                //        break;
                //}
                ConstantManager.Barcodes = BarcodeCollection.ToList();
                ConstantManager.Contents = SelectedBrand?.BrandName;
                if (BarcodeCollection.Any(x => x?.Kegs?.Partners?.Count > 1))
                    await NavigateToValidatePartner(BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList());
                else
                {
                    //await Application.Current.MainPage.Navigation.PopModalAsync();
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
                foreach (var item in verifiedBarcodes)
                {
                    BarcodeCollection.Where(x => x.Barcode == item.Barcode).FirstOrDefault().HasMaintenaceVerified = item.HasMaintenaceVerified;
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
                if (!string.IsNullOrEmpty(ConstantManager.Barcode))
                    BarcodeCollection.Add(new BarcodeModel { Barcode = ConstantManager.Barcode });

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
                var isNew = BarcodeCollection.ToList().Any(x => x?.Kegs?.Partners?.FirstOrDefault()?.Kegs?.FirstOrDefault()?.Barcode == ManaulBarcode);
                if (!isNew)
                {
                    BarcodeModel model = new BarcodeModel()
                    {
                        Barcode = ManaulBarcode,
                        TagsStr = TagsStr,
                        Icon = Cloud,
                        Page = ViewTypeEnum.ScanKegsView.ToString()
                    };

                    BarcodeCollection.Add(model);

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
                    else
                    {
                        //SimpleIoc @default = SimpleIoc.Default;
                        //string ManifestId = @default.GetInstance<MoveViewModel>().ManifestId;

                        var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                        var IsManifestExist = RealmDb.Find<ManifestModel>(ConstantManager.ManifestId);
                        try
                        {
                            if (IsManifestExist != null)
                            {
                                RealmDb.Write(() =>
                                {
                                    IsManifestExist.BarcodeModels.Add(model);
                                    RealmDb.Add(IsManifestExist, true);
                                });
                            }
                            else
                            {
                                ManifestModel manifestModel = await ManifestManager.GetManifestDraft(eventTypeEnum: EventTypeEnum.MOVE_MANIFEST,
                                                                manifestId: ConstantManager.ManifestId, barcodeCollection: BarcodeCollection.Where(t=>t.IsScanned == false).ToList(), tags: ConstantManager.Tags, 
                                                                partnerModel: ConstantManager.Partner, newPallets: new List<NewPallet>(),
                                                                batches: new List<NewBatch>(), closedBatches: new List<string>(), validationStatus: 2, contents: SelectedBrand?.BrandName);

                                manifestModel.IsQueue = true;

                                RealmDb.Write(() =>
                                {
                                    RealmDb.Add(manifestModel, true);
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
                    }

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
                var param = new NavigationParameters
                    {
                        { "Tags", ConstantManager.Tags },{ "TagsStr", TagsStr },{ "ViewTypeEnum", ViewTypeEnum.ScanKegsView }
                    };
                await _navigationService.NavigateAsync(new Uri("CognexScanView", UriKind.Relative), param, useModalNavigation: true, animated: false);

                //await BarcodeScanner.BarcodeScanAsync(_moveService, ConstantManager.Tags, TagsStr, ViewTypeEnum.ScanKegsView.ToString(),_navigationService);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void AssignBarcodeScannerValue(IList<BarcodeModel> models)
        {
            try
            {
                foreach (var item in models)
                    BarcodeCollection.Add(item);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async Task AssignValidatedValueAsync(Partner model)
        {
            var selecetdPertner = BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).Select(x=>x.Kegs.Partners).FirstOrDefault();
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
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    await RealmDb.WriteAsync((realmdb) =>
                     {
                         BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon(Maintenace);
                     });
                }
                else
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    await RealmDb.WriteAsync((realmdb) =>
                    {
                        BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Icon = GetIconByPlatform.GetIcon(ValidationOK);
                    });
                }

                //SimpleIoc.Default.GetInstance<MoveViewModel>().AssingScanKegsValue(_barcodes: BarcodeCollection.ToList(), _tags: Tags, _contents: SelectedBrand?.BrandName);

                try
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    await RealmDb.WriteAsync((realmdb) =>
                     {
                         BarcodeCollection.Where(x => x.Barcode == model.Kegs.FirstOrDefault().Barcode).FirstOrDefault().Kegs.Partners.Remove(unusedPartner);
                     });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                if (HasDone && model.Kegs?.FirstOrDefault()?.MaintenanceItems?.Count < 0)
                {
                    //await Application.Current.MainPage.Navigation.PopPopupAsync();
                    //await Application.Current.MainPage.Navigation.PopModalAsync();
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                    await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
                    Cleanup();
                }
                else
                {
                    var check = BarcodeCollection.Where(x => x?.Kegs?.Partners?.Count > 1).ToList();
                    if (check.Count == 0)
                    {    //await Application.Current.MainPage.Navigation.PopPopupAsync();
                        await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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

        internal void AssignAddTagsValue(List<Tag> _tags, string _tagsStr)
        {
            try
            {
                ConstantManager.IsFromScanned = false;
                //IsFromScanned = false;
                //Tags = _tags;
                //Tags = _tags;
                TagsStr = _tagsStr;
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
                //base.Cleanup();
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

        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Partner"))
            {
                await AssignValidatedValueAsync(parameters.GetValue<Partner>("Partner"));
            }
            if (parameters.ContainsKey("AddTags"))
            {
                AssignAddTagsValue(ConstantManager.Tags, ConstantManager.TagsStr);
            }
            if (parameters.ContainsKey("models"))
            {
                AssignBarcodeScannerValue(parameters.GetValue<IList<BarcodeModel>>("models"));
            }
            if (parameters.ContainsKey("AssignSizesValue"))
            {
                AssignSizesValue(ConstantManager.VerifiedBarcodes);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            //MessagingCenter.Unsubscribe<ValidToScanKegPagesMsg>(this, "ValidToScanKegPagesMsg");
            //MessagingCenter.Unsubscribe<ScanKegsMessage>(this, "ScanKegsMessage");
            //MessagingCenter.Unsubscribe<CancelledMessage>(this, "CancelledMessage");
            //MessagingCenter.Unsubscribe<PalletToScanKegPagesMsg>(this, "PalletToScanKegPagesMsg");
        }


        #endregion
    }
}
