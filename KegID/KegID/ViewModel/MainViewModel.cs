﻿using KegID.Common;
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
using System.Collections.Generic;
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
        private readonly IDashboardService _dashboardService;
        private readonly IUuidManager _uuidManager;
        private ConnectionType connetionType;

        public string Stock { get; set; } = "0";
        public string Empty { get; set; } = "0";
        public string  InUse { get; set; } = "0";
        public string Total { get; set; } = "0";
        public string AverageCycle { get; set; } = "0 day";
        public string Atriskegs { get; set; } = "0";
        public string DraftmaniFests { get; set; }
        public bool IsVisibleDraftmaniFestsLabel { get; set; }
        public string APIBase { get; set; }
        public double KegRefresh { get; set; }

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

        public MainViewModel(INavigationService navigationService, IDeviceCheckInMngr deviceCheckInMngr, IInitializeMetaData initializeMetaData, IDashboardService dashboardService, IUuidManager uuidManager) : base(navigationService)
        {
            _deviceCheckInMngr = deviceCheckInMngr;
            _initializeMetaData = initializeMetaData;
            _dashboardService = dashboardService;
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

            PreferenceSetting();
            LoadMetadData();
            HandleUnsubscribeMessages();
            HandleReceivedMessages();

            RefreshDashboardRecieverAsync();
            if (Device.RuntimePlatform != Device.UWP)
            {
                StartPrinterSearch();
            }

            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            APIBase = ConstantManager.BaseUrl.Contains("Prod") ? string.Empty : ConstantManager.BaseUrl;
            DeviceCheckIn();
        }

        #endregion

        #region Methods

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            KegRefresh = Convert.ToDouble(preferences.Find(x => x.PreferenceName == "KEG_REFRESH")?.PreferenceValue);
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckDraftmaniFests();
        }

        private async void LoadMetadData()
        {
            if (VersionTracking.IsFirstLaunchForCurrentVersion)
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
                    //AppSettings.IsMetaDataLoaded = true;
                    Loader.StopLoading();
                }
            }
        }

        private void StartPrinterSearch()
        {
            new Task(new Action(() => StartBluetoothDiscovery())).Start();
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
            MessagingCenter.Subscribe<InvalidServiceCall>(this, "InvalidServiceCall", _ =>
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

            MessagingCenter.Subscribe<SettingToDashboardMsg>(this, "SettingToDashboardMsg", _ => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    RefreshDashboardRecieverAsync(true);
                });
            });

            MessagingCenter.Subscribe<CheckDraftmaniFests>(this, "CheckDraftmaniFests", _ => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CheckDraftmaniFests();
                });
            });
        }

        private async void DeviceCheckIn()
        {
            Device.StartTimer(TimeSpan.FromDays(1), () =>
            {
                DeviceCheckIn();
                return true; // True = Repeat again, False = Stop the timer
            });

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await _deviceCheckInMngr.DeviceCheckInAync();
            }

            Device.StartTimer(TimeSpan.FromMilliseconds(KegRefresh), () =>
            {
                var model = new KegRequestModel
                {
                    KegId = string.Empty,
                    Barcode = string.Empty,
                    OwnerId = ConstantManager.Partner?.PartnerId,
                    AltBarcode = string.Empty,
                    Notes = "",
                    ReferenceKey = "",
                    ProfileId = "",
                    AssetType = string.Empty,
                    AssetSize = string.Empty,
                    AssetVolume = "",
                    AssetDescription = "",
                    OwnerSkuId = "",
                    FixedContents = "",
                    Tags = new List<Tag>(),
                    MaintenanceAlertIds = new List<string>(),
                    LessorId = "",
                    PurchaseDate = DateTimeOffset.Now,
                    PurchasePrice = 0,
                    PurchaseOrder = "",
                    ManufacturerName = "",
                    ManufacturerId = "",
                    ManufactureLocation = "",
                    ManufactureDate = DateTimeOffset.Now,
                    Material = "",
                    Markings = "",
                    Colors = ""
                };

                Task.Factory.StartNew(async () =>
                {
                    var value = await _dashboardService.PostKegAsync(model, string.Empty, AppSettings.SessionId, Configuration.Keg);
                });
                return false; // True = Repeat again, False = Stop the timer
            });
        }

        private async void MoveCommandRecieverAsync()
        {
            try
            {
                _ = await _navigationService.NavigateAsync("MoveView", new NavigationParameters
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
                await _navigationService.NavigateAsync("ManifestsView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal void CheckDraftmaniFests()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var manifests = RealmDb.All<ManifestModel>().Where(x => x.IsDraft || x.IsQueue).ToList();
                var pallets = RealmDb.All<PalletRequestModel>().Where(x => x.IsQueue).ToList();

                var draft = manifests.Where(x => x.IsDraft).ToList();
                var queue = manifests.Where(x => x.IsQueue).ToList();
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
            DashboardResponseModel Result;
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
                Loader.StopLoading();
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            CheckDraftmaniFests();
            base.OnNavigatedTo(parameters);
        }

        #endregion
    }
}
