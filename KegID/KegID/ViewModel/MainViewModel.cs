using KegID.Common;
using KegID.DependencyServices;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        private readonly IDeviceCheckInMngr _deviceCheckInMngr;
        private readonly IInitializeMetaData _initializeMetaData;
        //private readonly INavigationService _navigationService;
        private readonly IDashboardService _dashboardService;
        private readonly IGetIconByPlatform _getIconByPlatform;
        private readonly IUuidManager _uuidManager;
        ConnectionType connetionType;

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
                SetProperty(ref _Stock, value);
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

        private string _DraftmaniFests = default;

        /// <summary>
        /// Sets and gets the DraftmaniFests property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DraftmaniFests
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
        public DelegateCommand MoreCommand { get; }
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

        public MainViewModel(INavigationService navigationService/*, ISyncManager syncManager*/, IDeviceCheckInMngr deviceCheckInMngr, IInitializeMetaData initializeMetaData, IDashboardService dashboardService, IGetIconByPlatform getIconByPlatform, IUuidManager uuidManager) : base(navigationService)
        {
            //_navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _deviceCheckInMngr = deviceCheckInMngr;
            _initializeMetaData = initializeMetaData;
            _dashboardService = dashboardService;
            _getIconByPlatform = getIconByPlatform;
            _uuidManager = uuidManager;

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

            DeviceCheckIn();
            LoadMetadData();
            HandleUnsubscribeMessages();
            HandleReceivedMessages();

            RefreshDashboardRecieverAsync();
            //StartPrinterSearch();

            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        #endregion

        #region Methods
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckDraftmaniFestsAsync();
        }

        private async void LoadMetadData()
        {
            if (!AppSettings.IsMetaDataLoaded)
            {
                try
                {
                    Loader.StartLoading("Wait while downloading metadata...");

                    _initializeMetaData.DeleteInitializeMetaData();
                    await _initializeMetaData.LoadInitializeMetaData();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                finally
                {
                    AppSettings.IsMetaDataLoaded = true;
                    Loader.StopLoading();
                }
            }
        }

        private void StartPrinterSearch()
        {
            new Task(new Action(() =>
            {
                StartBluetoothDiscovery();
            })).Start();
        }

        private void StartBluetoothDiscovery()
        {
            IDiscoveryEventHandler bthandler = DiscoveryHandlerFactory.Current.GetInstance();
            bthandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
            bthandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
            bthandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
            connetionType = ConnectionType.Bluetooth;
            System.Diagnostics.Debug.WriteLine("Starting Bluetooth Discovery");
            DependencyService.Get<IPrinterDiscovery>().FindBluetoothPrinters(bthandler);
        }

        private void DiscoveryHandler_OnFoundPrinter(object sender, IDiscoveredPrinter discoveredPrinter)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (discoveredPrinter.Address == AppSettings.PrinterAddress)
                {
                    ConstantManager.PrinterSetting = discoveredPrinter;
                }
            });
        }

        private void StartNetworkDiscovery()
        {
            try
            {
                IDiscoveryEventHandler nwhandler = DiscoveryHandlerFactory.Current.GetInstance();
                nwhandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
                nwhandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
                nwhandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
                connetionType = ConnectionType.Network;
                System.Diagnostics.Debug.WriteLine("Starting Network Discovery");
                NetworkDiscoverer.Current.LocalBroadcast(nwhandler);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Network Exception: " + e.Message);
            }
        }

        private void DiscoveryHandler_OnDiscoveryFinished(object sender)
        {
            if (connetionType == ConnectionType.USB)
            {
                StartBluetoothDiscovery();
            }
            else if (connetionType == ConnectionType.Bluetooth)
            {
                StartNetworkDiscovery();
            }
        }

        private void DiscoveryHandler_OnDiscoveryError(object sender, string message)
        {
            if (connetionType == ConnectionType.USB)
            {
                StartBluetoothDiscovery();
            }
            else if (connetionType == ConnectionType.Bluetooth)
            {
                StartNetworkDiscovery();
            }
        }

        private void HandleUnsubscribeMessages()
        {
            MessagingCenter.Unsubscribe<SettingToDashboardMsg>(this, "SettingToDashboardMsg");
            MessagingCenter.Unsubscribe<InvalidServiceCall>(this, "InvalidServiceCall");
            MessagingCenter.Unsubscribe<CheckDraftmaniFests>(this, "CheckDraftmaniFests");
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<InvalidServiceCall>(this, "InvalidServiceCall", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var param = new NavigationParameters
                    {
                        { "IsLogOut",true}
                    };
                    await _navigationService.NavigateAsync("/NavigationPage/LoginView", param,animated:false);
                });
            });

            MessagingCenter.Subscribe<SettingToDashboardMsg>(this, "SettingToDashboardMsg", message => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    RefreshDashboardRecieverAsync(true);
                });
            });

            MessagingCenter.Subscribe<CheckDraftmaniFests>(this, "CheckDraftmaniFests", message => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CheckDraftmaniFestsAsync();
                });
            });
        }

        private async void DeviceCheckIn()
        {
            Device.StartTimer(TimeSpan.FromDays(1), () =>
            {
                // Do something
                DeviceCheckIn();
                return true; // True = Repeat again, False = Stop the timer
            });

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await _deviceCheckInMngr.DeviceCheckInAync(); 
            }
        }

        private async void MoveCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("MoveView", new NavigationParameters
                {
                    { "ManifestId", _uuidManager.GetUuId() }
                }, animated: false);
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
                await _navigationService.NavigateAsync("DashboardPartnersView", animated: false);
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
                await _navigationService.NavigateAsync("KegSearchView", animated: false);
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
                await _navigationService.NavigateAsync("DashboardPartnersView", animated: false);
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
                await _navigationService.NavigateAsync("InventoryView", new NavigationParameters
                    {
                        { "currentPage", 0 }
                    }, animated: false);
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
                await _navigationService.NavigateAsync("InventoryView", new NavigationParameters
                    {
                        { "currentPage", 1 }
                    }, animated: false);
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
                await _navigationService.NavigateAsync("ManifestsView", new NavigationParameters
                    {
                        { "LoadDraftManifestAsync", "LoadDraftManifestAsync" }
                    }, animated: false);
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
                var manifests = RealmDb.All<ManifestModel>().Where(x => x.IsDraft == true || x.IsQueue == true).ToList();
                var pallets = RealmDb.All<PalletRequestModel>().Where(x => x.IsQueue == true).ToList();

                var draft = manifests.Where(x => x.IsDraft == true).ToList();
                var queue = manifests.Where(x => x.IsQueue == true).ToList();
                string queueMsg = queue.Count > 1 ? "queued manifests" : "queued manifest";
                string draftMsg = draft.Count > 1 ? "draft manifests" : "draft manifest";
                string palletMsg = pallets.Count > 1 ? "queued pallets" : "queued pallet";

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    if (manifests.Count > 0)
                    {
                        if (draft.Count > 0)
                        {
                            if (queue.Count > 0)
                            {
                                if (pallets.Count > 0)
                                {
                                    DraftmaniFests = string.Format("{0} " + draftMsg + ", {1} " + queueMsg + ", {2} " + palletMsg, draft.Count, queue.Count, pallets.Count);
                                }
                                else
                                    DraftmaniFests = string.Format("{0} " + draftMsg + ", {1} " + queueMsg, draft.Count, queue.Count);
                            }
                            else
                            {
                                if (pallets.Count > 0)
                                {
                                    DraftmaniFests = string.Format("{0} " + draftMsg + ", {1} " + palletMsg, draft.Count, pallets.Count);
                                }
                                else
                                    DraftmaniFests = string.Format("{0} " + draftMsg, draft.Count);
                            }
                        }
                        else if (queue.Count > 0)
                        {
                            if (pallets.Count > 0)
                            {
                                DraftmaniFests = string.Format("{0} " + queueMsg + ", {1} " + palletMsg, queue.Count, pallets.Count);
                            }
                            DraftmaniFests = string.Format("{0} " + queueMsg, queue.Count);
                        }

                        IsVisibleDraftmaniFestsLabel = true;
                    }
                    else if (pallets.Count > 0)
                    {
                        DraftmaniFests = string.Format("{0} " + palletMsg, pallets.Count);
                    }
                    else
                    {
                        DraftmaniFests = string.Empty;
                        IsVisibleDraftmaniFestsLabel = false;
                    }
                }
                else
                {
                    if (manifests.Count > 0)
                    {
                        if (draft.Count > 0)
                        {
                            if (queue.Count > 0)
                            {
                                if (pallets.Count > 0)
                                {
                                    DraftmaniFests = string.Format("Lost communication with KegID.com, {0} " + draftMsg + ", {1} " + queueMsg + ", {2} " + palletMsg, draft.Count, queue.Count, pallets.Count);
                                }
                                else
                                    DraftmaniFests = string.Format("Lost communication with KegID.com, {0} " + draftMsg + ", {1} " + queueMsg, draft.Count, queue.Count);
                            }
                            else
                            {
                                if (pallets.Count > 0)
                                {
                                    DraftmaniFests = string.Format("Lost communication with KegID.com, {0} " + draftMsg + ", {1} " + palletMsg, draft.Count, pallets.Count);
                                }
                                else
                                    DraftmaniFests = string.Format("Lost communication with KegID.com, {0} " + draftMsg, draft.Count);
                            }
                        }
                        else
                        {
                            if (queue.Count > 0)
                            {
                                if (pallets.Count > 0)
                                {
                                    DraftmaniFests = string.Format("Lost communication with KegID.com, {0} " + queueMsg + ", {1} " + palletMsg, queue.Count, pallets.Count);
                                }
                                else
                                    DraftmaniFests = string.Format("Lost communication with KegID.com, {0} " + queueMsg , queue.Count);
                            }
                        }

                        IsVisibleDraftmaniFestsLabel = true;
                    }
                    else if (pallets.Count > 0)
                    {
                        DraftmaniFests = string.Format("Lost communication with KegID.com, {0} " + palletMsg, pallets.Count);
                    }
                    else
                    {
                        DraftmaniFests = string.Format("Lost communication with KegID.com");
                        IsVisibleDraftmaniFestsLabel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void FillCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("FillView", new NavigationParameters { { "UuId", _uuidManager.GetUuId() } }, animated: false);
        }

        private async void PalletizeCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PalletizeView", new NavigationParameters
                    {
                        { "GenerateManifestIdAsync", "GenerateManifestIdAsync" }
                    }, animated: false);
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
                await _navigationService.NavigateAsync("SearchPalletView", animated: false);
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
                await _navigationService.NavigateAsync("MaintainView", new NavigationParameters
                    {
                        { "LoadMaintenanceTypeAsync", "LoadMaintenanceTypeAsync" }
                    }, animated: false);
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
                    await _navigationService.ClearPopupStackAsync(animated: false);
                Result = await _dashboardService.GetDeshboardDetailAsync(AppSettings.SessionId);
                Stock = Result.Stock.ToString("0,0", CultureInfo.InvariantCulture);
                Empty = Result.Empty.ToString("0,0", CultureInfo.InvariantCulture);
                InUse = Result.InUse.ToString("0,0", CultureInfo.InvariantCulture);
                var total = Result.Stock + Result.Empty + Result.InUse;
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
                await _navigationService.NavigateAsync("SettingView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ManifestId"))
            {
                var Manifestd = parameters.GetValue<string>("ManifestId");
            }
            CheckDraftmaniFestsAsync();
        }

        #endregion
    }
}
