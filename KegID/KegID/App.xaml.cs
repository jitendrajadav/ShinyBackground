using KegID.Common;
using KegID.Views;
using KegID.ViewModel;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Prism.Unity;
using Prism.Ioc;
using Prism;
using KegID.Services;
using Xamarin.Forms;
using Prism.Plugin.Popups;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace KegID
{
    public partial class App : PrismApplication
    {
        public static string CurrentLanguage = "EN";
        public static ServiceProvider serviceProvider;
        public static KegIDClient kegIDClient;

        public App(IPlatformInitializer initializer = null) : base(initializer) { }
        protected override void OnInitialized()
        {
            InitializeComponent();

            serviceProvider = new ServiceCollection()
                .AddSingleton<HttpClient>()
                .AddTransient<KegIDClient>()
                .BuildServiceProvider();

            kegIDClient = serviceProvider.GetService<KegIDClient>();

            if (AppSettings.User != null)
                NavigationService.NavigateAsync(nameof(KegID.MainPage));
            else
                NavigationService.NavigateAsync(nameof(LoginView));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Main Navigation for MasterPage.
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
            containerRegistry.RegisterForNavigation<MenuView, DashboardViewModel>();
            containerRegistry.RegisterForNavigation<CognexScanView, CognexScanViewModel>();

            containerRegistry.RegisterForNavigation<SelectPrinterView, SelectPrinterViewModel>();
            containerRegistry.RegisterForNavigation<BulkUpdateScanView, BulkUpdateScanViewModel>();
            containerRegistry.RegisterForNavigation<DashboardPartnersView, DashboardPartnersViewModel>();
            containerRegistry.RegisterForNavigation<EditKegView, EditKegViewModel>();

            containerRegistry.RegisterForNavigation<InventoryView, InventoryViewModel>();
            containerRegistry.RegisterForNavigation<KegSearchedListView, KegSearchedListViewModel>();
            containerRegistry.RegisterForNavigation<KegSearchView, KegSearchViewModel>();
            containerRegistry.RegisterForNavigation<KegStatusView, KegStatusViewModel>();
            containerRegistry.RegisterForNavigation<KegsView, KegsViewModel>();

            containerRegistry.RegisterForNavigation<PartnerInfoMapView, PartnerInfoMapViewModel>();
            containerRegistry.RegisterForNavigation<PartnerInfoView, PartnerInfoViewModel>();
            containerRegistry.RegisterForNavigation<AddBatchView, AddBatchViewModel>();
            containerRegistry.RegisterForNavigation<AddPalletsView, AddPalletsViewModel>();
            containerRegistry.RegisterForNavigation<BatchView, BatchViewModel>();

            containerRegistry.RegisterForNavigation<BrandView, BrandViewModel>();
            containerRegistry.RegisterForNavigation<FillScanReviewView, FillScanReviewViewModel>();
            containerRegistry.RegisterForNavigation<FillScanView, FillScanViewModel>();
            containerRegistry.RegisterForNavigation<FillView, FillViewModel>();
            containerRegistry.RegisterForNavigation<SizeView, SizeViewModel>();

            containerRegistry.RegisterForNavigation<VolumeView, VolumeViewModel>();
            containerRegistry.RegisterForNavigation<MaintainDetailView, MaintainDetailViewModel>();
            containerRegistry.RegisterForNavigation<MaintainScanView, MaintainScanViewModel>();
            containerRegistry.RegisterForNavigation<MaintainView, MaintainViewModel>();
            containerRegistry.RegisterForNavigation<AddPartnerView, AddPartnerViewModel>();

            containerRegistry.RegisterForNavigation<AddTagsView, AddTagsViewModel>();
            containerRegistry.RegisterForNavigation<AssignSizesView, AssignSizesViewModel>();
            containerRegistry.RegisterForNavigation<ContentTagsView, ContentTagsViewModel>();
            containerRegistry.RegisterForNavigation<EditAddressView, EditAddressViewModel>();
            containerRegistry.RegisterForNavigation<ManifestDetailView, ManifestDetailViewModel>();

            containerRegistry.RegisterForNavigation<ManifestsView, ManifestsViewModel>();
            containerRegistry.RegisterForNavigation<MoveView, MoveViewModel>();
            containerRegistry.RegisterForNavigation<PartnersView, PartnersViewModel>();
            containerRegistry.RegisterForNavigation<ScanInfoView, ScanInfoViewModel>();
            containerRegistry.RegisterForNavigation<ScanKegsView, ScanKegsViewModel>();

            containerRegistry.RegisterForNavigation<SearchedManifestsListView, SearchedManifestsListViewModel>();
            containerRegistry.RegisterForNavigation<SearchManifestsView, SearchManifestsViewModel>();
            containerRegistry.RegisterForNavigation<SearchPartnersView, SearchPartnersViewModel>();
            containerRegistry.RegisterForNavigation<ValidateBarcodeView, ValidateBarcodeViewModel>();
            containerRegistry.RegisterForNavigation<PalletizeDetailView, PalletizeDetailViewModel>();

            containerRegistry.RegisterForNavigation<PalletizeView, PalletizeViewModel>();
            containerRegistry.RegisterForNavigation<PalletSearchedListView, PalletSearchedListViewModel>();
            containerRegistry.RegisterForNavigation<SearchPalletView, SearchPalletViewModel>();
            containerRegistry.RegisterForNavigation<PrinterSettingView, PrinterSettingViewModel>();
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>();

            containerRegistry.RegisterForNavigation<WhatIsNewView, WhatIsNewViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();

            //Popup Navigation Register
            containerRegistry.RegisterPopupNavigationService();

            //   Services Register
            containerRegistry.Register<IAccountService, AccountService>();
            containerRegistry.Register<IDashboardService, DashboardService>();
            containerRegistry.Register<IMoveService, MoveService>();
            containerRegistry.Register<IFillService, FillService>();
            containerRegistry.Register<IPalletizeService, PalletizeService>();
            containerRegistry.Register<IMaintainService, MaintainService>();
            containerRegistry.Register<IInitializeMetaData, InitializeMetaData>();
            containerRegistry.Register<IZebraPrinterManager, ZebraPrinterManager>();
            containerRegistry.Register<ILoader, Loader>();
            containerRegistry.Register<IManifestManager, ManifestManager>();
            containerRegistry.Register<IGetIconByPlatform, GetIconByPlatform>();
            containerRegistry.Register<ISyncManager, SyncManager>();
            containerRegistry.Register<IUuidManager, UuidManager>();
            containerRegistry.Register<ICalcCheckDigitMngr, CalcCheckDigitMngr>();
            containerRegistry.Register<IDeviceCheckInMngr, DeviceCheckInMngr>();
        }


        protected override void OnStart()
        {
            //AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("uwp=0404c586-124c-4b55-8848-910689b6881b;" +
                   "android=31ceef42-fd24-49d3-8e7e-21f144355dde;" +
                   "ios=b80b8476-04cf-4fc3-b7f7-be06ba7f2213",
                   typeof(Analytics), typeof(Crashes));

            //LoadPersistedValues();
        }

        protected override void OnSleep ()
		{
            // Handle when your app sleeps
            //Current.Properties["SleepDate"] = DateTimeOffset.Now.ToString("O");
            //Current.Properties["FirstName"] = _backgroundPage.FirstName;
            serviceProvider.Dispose();
        }

        protected override void OnResume ()
		{
            // Handle when your app resumes
            //LoadPersistedValues();
        }

        private void LoadPersistedValues()
        {
            if (Current.Properties.ContainsKey("SleepDate"))
            {
                var value = (string)Current.Properties["SleepDate"];
                if (DateTimeOffset.TryParse(value, out DateTimeOffset sleepDate))
                {
                    //_backgroundPage.SleepDate = sleepDate;
                }
            }

            if (Current.Properties.ContainsKey("FirstName"))
            {
                var firstName = (string)Current.Properties["FirstName"];
                //_backgroundPage.FirstName = firstName;
            }
        }
    }
}
