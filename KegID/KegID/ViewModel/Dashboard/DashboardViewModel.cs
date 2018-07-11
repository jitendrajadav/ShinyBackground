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
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IDashboardService _dashboardService;
        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly IUuidManager _uuidManager;

        #region Stock

        private string _Stock = "0";

        public string Stock
        {
            get
            {
                return _Stock;
            }

            set
            {
                SetProperty(ref _Stock,value);
            }
        }

        #endregion

        #region Empty

        /// <summary>
        /// The <see cref="Empty" /> property's name.
        /// </summary>
        public const string EmptyPropertyName = "Empty";

        private string _Empty = "0";

        /// <summary>
        /// Sets and gets the Empty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Empty
        {
            get
            {
                return _Empty;
            }

            set
            {
                if (_Empty == value)
                {
                    return;
                }

                _Empty = value;
                RaisePropertyChanged(EmptyPropertyName);
            }
        }

        #endregion

        #region InUse

        /// <summary>
        /// The <see cref="InUse" /> property's name.
        /// </summary>
        public const string InUsePropertyName = "InUse";

        private string _InUse = "0";

        /// <summary>
        /// Sets and gets the InUse property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string InUse
        {
            get
            {
                return _InUse;
            }

            set
            {
                if (_InUse == value)
                {
                    return;
                }

                _InUse = value;
                RaisePropertyChanged(InUsePropertyName);
            }
        }

        #endregion

        #region Total

        /// <summary>
        /// The <see cref="Total" /> property's name.
        /// </summary>
        public const string TotalPropertyName = "Total";

        private string _Total = "0";

        /// <summary>
        /// Sets and gets the Total property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged(TotalPropertyName);
            }
        }

        #endregion

        #region AverageCycle

        /// <summary>
        /// The <see cref="AverageCycle" /> property's name.
        /// </summary>
        public const string AverageCyclePropertyName = "AverageCycle";

        private string _AverageCycle = "0 day";

        /// <summary>
        /// Sets and gets the AverageCycle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AverageCycle
        {
            get
            {
                return _AverageCycle;
            }

            set
            {
                if (_AverageCycle == value)
                {
                    return;
                }

                _AverageCycle = value;
                RaisePropertyChanged(AverageCyclePropertyName);
            }
        }

        #endregion

        #region Atriskegs

        /// <summary>
        /// The <see cref="Atriskegs" /> property's name.
        /// </summary>
        public const string AtriskegsPropertyName = "Atriskegs";

        private string _Atriskegs = "0";

        /// <summary>
        /// Sets and gets the Atriskegs property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Atriskegs
        {
            get
            {
                return _Atriskegs;
            }

            set
            {
                if (_Atriskegs == value)
                {
                    return;
                }

                _Atriskegs = value;
                RaisePropertyChanged(AtriskegsPropertyName);
            }
        }

        #endregion

        #region DraftmaniFests

        /// <summary>
        /// The <see cref="DraftmaniFests" /> property's name.
        /// </summary>
        public const string DraftmaniFestsPropertyName = "DraftmaniFests";

        private int _DraftmaniFests = default(int);

        /// <summary>
        /// Sets and gets the DraftmaniFests property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int DraftmaniFests
        {
            get
            {
                return _DraftmaniFests;
            }

            set
            {
                if (_DraftmaniFests == value)
                {
                    return;
                }

                _DraftmaniFests = value;
                RaisePropertyChanged(DraftmaniFestsPropertyName);
            }
        }

        #endregion

        #region BgImage

        /// <summary>
        /// The <see cref="BgImage" /> property's name.
        /// </summary>
        public const string BgImagePropertyName = "BgImage";

        private string _BgImage = "Assets/kegbg.png";

        /// <summary>
        /// Sets and gets the BgImage property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BgImage
        {
            get
            {
                return _BgImage;
            }

            set
            {
                if (_BgImage == value)
                {
                    return;
                }

                _BgImage = value;
                RaisePropertyChanged(BgImagePropertyName);
            }
        }

        #endregion

        #region IsVisibleDraftmaniFestsLabel

        /// <summary>
        /// The <see cref="IsVisibleDraftmaniFestsLabel" /> property's name.
        /// </summary>
        public const string IsVisibleDraftmaniFestsLabelPropertyName = "IsVisibleDraftmaniFestsLabel";

        private bool _IsVisibleDraftmaniFestsLabel = false;

        /// <summary>
        /// Sets and gets the IsVisibleDraftmaniFestsLabel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsVisibleDraftmaniFestsLabel
        {
            get
            {
                return _IsVisibleDraftmaniFestsLabel;
            }

            set
            {
                if (_IsVisibleDraftmaniFestsLabel == value)
                {
                    return;
                }

                _IsVisibleDraftmaniFestsLabel = value;
                RaisePropertyChanged(IsVisibleDraftmaniFestsLabelPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand MoreCommand { get;}
        public DelegateCommand MaintainCommand { get; }
        public DelegateCommand PalletizeCommand { get; }
        public DelegateCommand PalletsCommand { get; }
        public DelegateCommand FillCommand { get; }
        public DelegateCommand ManifestCommand { get; }
        public DelegateCommand StockCommand { get; }
        public DelegateCommand EmptyCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand KegsCommand { get; }
        public DelegateCommand InUsePartnerCommand { get; }
        public DelegateCommand MoveCommand { get; }
        
        #endregion

        #region Constructor

        public DashboardViewModel(IDashboardService dashboardService, INavigationService navigationService, IGetIconByPlatform getIconByPlatform, IUuidManager uuidManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _dashboardService = dashboardService;
            _getIconByPlatform = getIconByPlatform;
            _uuidManager = uuidManager;

            BgImage = _getIconByPlatform.GetIcon("kegbg.png");

            MoveCommand = new DelegateCommand(MoveCommandRecieverAsync);
            MoreCommand = new DelegateCommand(MoreCommandRecieverAsync);
            MaintainCommand = new DelegateCommand(MaintainCommandRecieverAsync);
            PalletizeCommand = new DelegateCommand(PalletizeCommandRecieverAsync);
            PalletsCommand = new DelegateCommand(PalletsCommandRecieverAsync);
            FillCommand = new DelegateCommand(FillCommandRecieverAsync);
            ManifestCommand = new DelegateCommand(ManifestCommandRecieverAsync);
            StockCommand = new DelegateCommand(StockCommandRecieverAsync);
            EmptyCommand = new DelegateCommand(EmptyCommandRecieverAsync);
            PartnerCommand = new DelegateCommand(PartnerCommandRecieverAsync);
            KegsCommand = new DelegateCommand(KegsCommandRecieverAsync);
            InUsePartnerCommand = new DelegateCommand(InUsePartnerCommandRecieverAsync);
            RefreshDashboardRecieverAsync();
            CheckDraftmaniFestsAsync();
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<SettingToDashboardMsg>(this, "SettingToDashboardMsg", message => {
                Device.BeginInvokeOnMainThread(() => 
                {
                    RefreshDashboardRecieverAsync(true);
                });
            });
        }

        private async void MoveCommandRecieverAsync()
        {
            try
            {
                var param = new NavigationParameters
                {
                    { "ManifestId", _uuidManager.GetUuId() }
                };
                await _navigationService.NavigateAsync(new Uri("MoveView", UriKind.Relative), param, useModalNavigation: true, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void InUsePartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("DashboardPartnersView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void KegsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("KegSearchView", UriKind.Relative), useModalNavigation: true, animated: false);
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
                await _navigationService.NavigateAsync(new Uri("DashboardPartnersView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void StockCommandRecieverAsync()
        {
            try
            {
                var param = new NavigationParameters
                    {
                        { "currentPage", 0 }
                    };
                await _navigationService.NavigateAsync(new Uri("InventoryView", UriKind.Relative), param, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void EmptyCommandRecieverAsync()
        {
            try
            {
                var param = new NavigationParameters
                    {
                        { "currentPage", 1 }
                    };
                await _navigationService.NavigateAsync(new Uri("InventoryView", UriKind.Relative), param, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestCommandRecieverAsync()
        {
            try
            {
                var param = new NavigationParameters
                    {
                        { "LoadDraftManifestAsync", "LoadDraftManifestAsync" }
                    };
                await _navigationService.NavigateAsync(new Uri("ManifestsView", UriKind.Relative), param, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void CheckDraftmaniFestsAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var collection = RealmDb.All<ManifestModel>().Where(x => x.IsDraft == true).ToList();
                if (collection.Count > 0)
                {
                    DraftmaniFests = collection.Count;
                    IsVisibleDraftmaniFestsLabel = true;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void FillCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync(new Uri("FillView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        private async void PalletizeCommandRecieverAsync()
        {
            try
            {
                var param = new NavigationParameters
                    {
                        { "GenerateManifestIdAsync", "GenerateManifestIdAsync" }
                    };
                await _navigationService.NavigateAsync(new Uri("PalletizeView", UriKind.Relative), param, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void PalletsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("SearchPalletView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void MaintainCommandRecieverAsync()
        {
            try
            {
                var param = new NavigationParameters
                    {
                        { "LoadMaintenanceTypeAsync", "LoadMaintenanceTypeAsync" }
                    };
                await _navigationService.NavigateAsync(new Uri("MaintainView", UriKind.Relative), param, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void RefreshDashboardRecieverAsync(bool refresh = false)
        {
            DashboardResponseModel Result = null;
            try
            {
                if (refresh)
                    await _navigationService.ClearPopupStackAsync(animated:false);
                Result = await _dashboardService.GetDeshboardDetailAsync(AppSettings.User.SessionId);
                Stock = Result.Stock.ToString("0,0", CultureInfo.InvariantCulture);
                Empty = Result.Empty.ToString("0,0", CultureInfo.InvariantCulture);
                InUse = Result.InUse.ToString("0,0", CultureInfo.InvariantCulture);
                var total  = Result.Stock+ Result.Empty+ Result.InUse;
                Total = total.ToString("0,0", CultureInfo.InvariantCulture);
                AverageCycle = Result.AverageCycle.ToString() + " days";
                Atriskegs = Result.InactiveKegs.ToString();
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                Result = null;
            }
        }

        private async void MoreCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("SettingView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<SettingToDashboardMsg>(this, "SettingToDashboardMsg");
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ManifestId"))
            {
                var Manifestd = parameters.GetValue<string>("ManifestId");
            }
        }

        #endregion
    }
}
