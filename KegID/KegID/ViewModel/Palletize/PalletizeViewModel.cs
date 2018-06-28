using System;
using System.Collections.Generic;
using System.Linq;
using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
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

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand AddTagsCommand { get; }
        public DelegateCommand TargetLocationPartnerCommand { get; }
        public DelegateCommand AddKegsCommand { get; }
        public DelegateCommand IsPalletVisibleCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand SubmitCommand { get; }
        
        #endregion

        #region Constructor

        public PalletizeViewModel(IPalletizeService palletizeService, IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;
            _palletizeService = palletizeService;
            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            PartnerCommand = new DelegateCommand(PartnerCommandRecieverAsync);
            AddTagsCommand = new DelegateCommand(AddTagsCommandRecieverAsync);
            TargetLocationPartnerCommand = new DelegateCommand(TargetLocationPartnerCommandRecieverAsync);
            AddKegsCommand = new DelegateCommand(AddKegsCommandRecieverAsync);
            IsPalletVisibleCommand = new DelegateCommand(IsPalletVisibleCommandReciever);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandReciever);
            SubmitCommand = new DelegateCommand(SubmitCommandRecieverAsync);

            StockLocation.FullName = "Barcode Brewing";
            TargetLocation.FullName = "None";

            //HandleReceivedMessages();
        }

        #endregion

        #region Methods

        //void HandleReceivedMessages()
        //{
        //    MessagingCenter.Subscribe<ScanKegToPalletPagesMsg>(this, "ScanKegToPalletPagesMsg", message =>
        //    {
        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            var value = message;
        //            if (value != null)
        //            {
        //                AssingScanKegsValue(value.Barcodes);
        //            }
        //        });
        //    });
        //}

        public void GenerateManifestIdAsync(PalletModel palletModel)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            DateTimeOffset now = DateTimeOffset.Now;
            string barCode;
            long prefix = 0;
            var lastCharOfYear = now.Year.ToString().ToCharArray().LastOrDefault().ToString();
            var dayOfYear = now.DayOfYear;
            var secondsInDayTillNow = SecondsInDayTillNow();
            var millisecond = now.Millisecond;

            var preference = RealmDb.All<Preference>().Where(x => x.PreferenceName == "DashboardPreferences").ToList();
            try
            {
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
                ManifestId = barCode + checksumDigit;
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                preference = null;
            }
        }

        internal void AssignPartnerValue(PartnerModel model)
        {
            try
            {
                if (TargetLocationPartner)
                {
                    TargetLocationPartner = false;
                    TargetLocation = model;
                }
                else
                    StockLocation = model;
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

        private async void SubmitCommandRecieverAsync()
        {
            List<PalletItem> palletItems = new List<PalletItem>();
            PalletItem pallet = null;
            PalletRequestModel palletRequestModel = null;
            var barCodeCollection = ConstantManager.Barcodes;

            try
            {
                Loader.StartLoading();

                foreach (var item in barCodeCollection)
                {
                    pallet = new PalletItem
                    {
                        Barcode = item.Barcode,
                        ScanDate = DateTimeOffset.Now,
                        Tags = ConstantManager.Tags,
                        ValidationStatus = 4
                    };

                    palletItems.Add(pallet);
                }

                palletRequestModel = new PalletRequestModel
                {
                    Barcode = ManifestId.Split('-').LastOrDefault(),
                    BuildDate = DateTimeOffset.Now,
                    OwnerId = AppSettings.User.CompanyId,
                    PalletId = Uuid.GetUuId(),
                    PalletItems = palletItems,
                    ReferenceKey = "",
                    StockLocation = StockLocation.PartnerId,
                    StockLocationId = StockLocation.PartnerId,
                    StockLocationName = StockLocation.FullName,
                    Tags = ConstantManager.Tags
                };

                var value = await _palletizeService.PostPalletAsync(palletRequestModel, AppSettings.User.SessionId, Configuration.NewPallet);

                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    //SimpleIoc.Default.GetInstance<PalletizeDetailViewModel>().LoadInfo(value);

                    Loader.StopLoading();
                    //await Application.Current.MainPage.Navigation.PushModalAsync(new PalletizeDetailView(), animated: false);
                    var param = new NavigationParameters
                    {
                        { "LoadInfo", value }
                    };
                    await _navigationService.NavigateAsync(new Uri("PalletizeDetailView", UriKind.Relative), param, useModalNavigation: true, animated: false);

                }
                else
                {
                    Loader.StopLoading();
                    //SimpleIoc.Default.GetInstance<LoginViewModel>().InvalideServiceCallAsync();
                }
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
                palletItems = null;
                pallet = null;
                barCodeCollection = null;
                palletRequestModel = null;
                Cleanup();
            }
        }

        internal void AssingScanKegsValue(IList<BarcodeModel> _barcodes)
        {
            if (_barcodes.Count > 1)
                AddKegs = string.Format("{0} Items", _barcodes.Count);
            else if (_barcodes.Count == 1)
                AddKegs = string.Format("{0} Item", _barcodes.Count);
            if (!IsSubmitVisible)
                IsSubmitVisible = true;
        }

        private void BarcodeScanCommandReciever()
        {
            try
            {
                PalletToScanKegPagesMsg msg = new PalletToScanKegPagesMsg
                {
                    BarcodeScan = true
                };
                MessagingCenter.Send(msg, "PalletToScanKegPagesMsg");

                //SimpleIoc.Default.GetInstance<ScanKegsViewModel>().BarcodeScanCommandReciever();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void IsPalletVisibleCommandReciever()
        {
            IsCameraVisible = true;
        }

        private async void AddKegsCommandRecieverAsync()
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PushModalAsync(new ScanKegsView(), animated: false);
                await _navigationService.NavigateAsync(new Uri("ScanKegsView", UriKind.Relative), useModalNavigation: true, animated: false);
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
                //await Application.Current.MainPage.Navigation.PushModalAsync(new AddTagsView(), animated: false);
                await _navigationService.NavigateAsync(new Uri("AddTagsView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void PartnerCommandRecieverAsync()
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView(), animated: false);
                await _navigationService.NavigateAsync(new Uri("PartnersView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void TargetLocationPartnerCommandRecieverAsync()
        {
            try
            {
                TargetLocationPartner = true;
                //await Application.Current.MainPage.Navigation.PushModalAsync(new PartnersView(), animated: false);
                await _navigationService.NavigateAsync(new Uri("PartnersView", UriKind.Relative), useModalNavigation: true, animated: false);
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
                //await Application.Current.MainPage.Navigation.PopModalAsync();
                IsCameraVisible = false;
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
                StockLocation = null;
                TargetLocation = null;
                AddInfoTitle = "Add info";
                AddKegs = "Add Kegs";
                IsSubmitVisible = false;
                Tags = null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            AssingScanKegsValue(ConstantManager.Barcodes);

            if (parameters.ContainsKey("model"))
            {
                AssignPartnerValue(parameters.GetValue<PartnerModel>("model"));
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            //MessagingCenter.Unsubscribe<ScanKegToPalletPagesMsg>(this, "ScanKegToPalletPagesMsg");
        }

        #endregion
    }
}
