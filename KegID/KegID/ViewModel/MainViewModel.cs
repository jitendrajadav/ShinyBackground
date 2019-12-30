using Acr.UserDialogs;
using KegID.Common;
using KegID.DependencyServices;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using Scandit.BarcodePicker.Unified;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;

namespace KegID.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        private readonly IInitializeMetaData _initializeMetaData;
        private readonly IUuidManager _uuidManager;

        public string DraftmaniFests { get; set; }
        public bool IsVisibleDraftmaniFestsLabel { get; set; }
        public string APIBase { get; set; }

        public ObservableCollection<Dashboard> Dashboards { get; set; } = new ObservableCollection<Dashboard>();

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

        public MainViewModel(INavigationService navigationService, IInitializeMetaData initializeMetaData, IUuidManager uuidManager) : base(navigationService)
        {
            _initializeMetaData = initializeMetaData;
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
        }

        #endregion

        #region Methods

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckDraftmaniFests();
        }

        private async Task LoadMetadData()
        {
            if (VersionTracking.IsFirstLaunchForCurrentVersion && (Application.Current.Properties.ContainsKey("OnSleep") && (!(bool)Application.Current.Properties["OnSleep"])))
            {
                UserDialogs.Instance.ShowLoading("Wait while downloading meta-data...");

                _initializeMetaData.DeleteInitializeMetaData();
                await RunSafe(_initializeMetaData.LoadInitializeMetaData());
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task StartPrinterSearch()
        {
            DiscoveryHandlerImplementation discoveryHandler = new DiscoveryHandlerImplementation();
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    DependencyService.Get<IConnectionManager>().FindBluetoothPrinters(discoveryHandler);
                }
                catch (NotImplementedException)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Bluetooth discovery not supported on this platform", "OK");
                    });
                }
            });
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

            MessagingCenter.Subscribe<SettingToDashboardMsg>(this, "SettingToDashboardMsg", _ => Device.BeginInvokeOnMainThread(()=>RefreshDashboardRecieverAsync(true)));

            MessagingCenter.Subscribe<CheckDraftmaniFests>(this, "CheckDraftmaniFests", _ => Device.BeginInvokeOnMainThread(() => CheckDraftmaniFests()));
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
                                DraftmaniFests = pallets.Count > 0
                                    ? string.Format("{0} " + draftMsg + ", {1} " + queueMsg + ", {2} " + palletMsg, draft.Count, queue.Count, pallets.Count)
                                    : string.Format("{0} " + draftMsg + ", {1} " + queueMsg, draft.Count, queue.Count);
                            }
                            else
                            {
                                DraftmaniFests = pallets.Count > 0
                                    ? string.Format("{0} " + draftMsg + ", {1} " + palletMsg, draft.Count, pallets.Count)
                                    : string.Format("{0} " + draftMsg, draft.Count);
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
                                DraftmaniFests = pallets.Count > 0
                                    ? string.Format("Lost communication with KegID.com, {0} " + draftMsg + ", {1} " + queueMsg + ", {2} " + palletMsg, draft.Count, queue.Count, pallets.Count)
                                    : string.Format("Lost communication with KegID.com, {0} " + draftMsg + ", {1} " + queueMsg, draft.Count, queue.Count);
                            }
                            else
                            {
                                DraftmaniFests = pallets.Count > 0
                                    ? string.Format("Lost communication with KegID.com, {0} " + draftMsg + ", {1} " + palletMsg, draft.Count, pallets.Count)
                                    : string.Format("Lost communication with KegID.com, {0} " + draftMsg, draft.Count);
                            }
                        }
                        else
                        {
                            if (queue.Count > 0)
                            {
                                DraftmaniFests = pallets.Count > 0
                                    ? string.Format("Lost communication with KegID.com, {0} " + queueMsg + ", {1} " + palletMsg, queue.Count, pallets.Count)
                                    : string.Format("Lost communication with KegID.com, {0} " + queueMsg , queue.Count);
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
            try
            {
                if (refresh)
                    await _navigationService.ClearPopupStackAsync(animated: false);
                var result = await ApiManager.GetDeshboardDetail(AppSettings.SessionId);
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadAsStringAsync();
                    var model = await Task.Run(() => JsonConvert.DeserializeObject<DashboardResponseModel>(response, GetJsonSetting()));

                    Dashboards.LastOrDefault().Stock = model.Stock.ToString("0,0", CultureInfo.InvariantCulture);
                    Dashboards.LastOrDefault().Empty = model.Empty.ToString("0,0", CultureInfo.InvariantCulture);
                    Dashboards.LastOrDefault().InUse = model.InUse.ToString("0,0", CultureInfo.InvariantCulture);
                    var total = model.Stock + model.Empty + model.InUse;
                    Dashboards.LastOrDefault().Total = total.ToString("0,0", CultureInfo.InvariantCulture);
                    Dashboards.LastOrDefault().AverageCycle = model.AverageCycle.ToString() + " days";
                    Dashboards.LastOrDefault().Atriskegs = model.InactiveKegs.ToString();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
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
            if (Dashboards.Count == 0)
            {
                Dashboards.Add(new Dashboard());
                Dashboards.Add(new Dashboard()
                {
                    Stock = "0",
                    Empty = "0",
                    InUse = "0",
                    Total = "0",
                    AverageCycle = "0 day",
                    Atriskegs = "0"
                });
            }
            CheckDraftmaniFests();
            base.OnNavigatedTo(parameters);
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            HandleUnsubscribeMessages();
            HandleReceivedMessages();

            RefreshDashboardRecieverAsync();
            if (Device.RuntimePlatform != Device.UWP)
            {
                await StartPrinterSearch();
            }

            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            APIBase = ConstantManager.BaseUrl.Contains("Prod") ? string.Empty : ConstantManager.BaseUrl;

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    ScanditService.ScanditLicense.AppKey = Resources["scanditAndroidKey"];
                    break;
                case Device.iOS:
                    ScanditService.ScanditLicense.AppKey = Resources["scanditiOSKey"];
                    break;
            }
            await LoadMetadData();
        }

        #endregion

        private class DiscoveryHandlerImplementation : DiscoveryHandler
        {
            public DiscoveryHandlerImplementation()
            {
            }

            public void DiscoveryError(string message)
            {
                //Device.BeginInvokeOnMainThread(async () => {
                //    await Application.Current.MainPage.DisplayAlert("Discovery Error", message, "OK");
                //});
            }

            public void DiscoveryFinished()
            {
                Device.BeginInvokeOnMainThread(() => {
                    //discoveryDemoPage.SetInputEnabled(true);
                });
            }

            public void FoundPrinter(DiscoveredPrinter printer)
            {
                Device.BeginInvokeOnMainThread(() => {
                    if (printer.Address == AppSettings.PrinterAddress)
                    {
                        ConstantManager.PrinterSetting = printer;
                    }
                });
            }
        }
    }
}
