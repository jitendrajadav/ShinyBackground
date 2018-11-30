using KegID.Common;
using KegID.Views;
using KegID.ViewModel;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.Unity;
using Prism.Ioc;
using Prism;
using KegID.Services;
using Xamarin.Forms;
using Prism.Plugin.Popups;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Scandit.BarcodePicker.Unified;
using Microsoft.AppCenter.Distribute;
using System;
using System.Threading.Tasks;
using KegID.DependencyServices;

namespace KegID
{
    public partial class App : PrismApplication
    {
        public const string appAndroidKey = "AQv7w5KBBqf8AtyImS1bVZclZ7c8B2+dMUFvZdxwZiBtT+jDD3rYlmBxC5EqKzdBWGlPxPFpBSZ4OGzKP1u/AldmFlQJQ17YaDnlcqtFbFandXpkbWryUsF3/Soeeb/yGS+eTsBKU/cSDEKHKzikBBVQYuZdfYxou8Nj4nMnBH1AWqHnq96P3EhKEL82Gplglc6DoVWHWO/pJtc52dTRLriz2gjmikEZ2t3ktsqcigBeA4M7AwkteR1iBCY795oqgnacZtm+3+yQO1Z1XSWqDkMWv/gGPa62Gu/mpZ46EK4oXkaFdxxmAg8IG7uDvLOc4EyYETa/tMZS8eBTLjbIikZTBNuWIHLpcyjyrsb4t2XlPc9W1UFKR8dxXYtU2HWXlBL4+ECS+Ma3o+cZCZlUf25TnZ+JXX0vIY8ETDRMjcYMbp5oBzHta+0RIHDcTrfSzl3Ye/7W5RWawMd0CNIQT0zg+6twEeOXAfR3kVPgVbHFNh8+2Gw+mir1c2jJKKq6beLRaK/y3PyYO6OFRqppKyxjG7Q7nY48Khy7zMkTG647CwYlKlT8Ipuj+eTBuwaq9IM7OiL1BT397tm8WGkeyMI7TlPvRRNPJLuiiLOC+w8U9auU8lUdqaGOrLegDzqvKoY/oH3jD/9I7uY850XbBK/UODW+SgMZdar6H1zs/a8lyQlRBWciEmR6aujG/9w83aVmbIjVn7s1POoGGA6aJwZTgml7Pj4bZOaPuAH9Jd8naQnt2V7jNJ9vP8y/Mot1N/24trAaxsfOHWIbyd8bmEwGCTInNE311JsYOQXPH6yDrX0iP9Fz8MvX7VA=";
        public const string appiOSKey= "AfZLcFjmRttIKSgzrTsvZw0OHeoICxs+IzO9Pg1aN8CUY8E1Z0MAn45SJfUGK87Wh0BYqftD6k1gSltf02cobChLzolPYriTmguX/JExthMvWtJ0yETXKQpzBuPofaSpInozs9oUmwzANmdGCgPpvCz2IURgWo3zmR+2sfZJ+/r4mT3BN84LmBNpecZkv2yXhv9qXf69wkqAoNqXEOa8q8BWFqT6h982b8ZVLbW+NhBFv8atAVLgLFYZbI17CFzCAoBhJCIIM1be0R/EWiHHJkVuU53vSNtkSLrVgglb5NMOKjTegI0hqEca3g+VTPms6j3/DsdwOGXZfg4fvRGNky1Pkh3FOcP250pLl+ew6BJwt15+9u6arSB0CN+c4ZgzLvkT7V+Xkn8rN6VAgLvLEPS+A+SONdjlJboQbXE8mWEKBJ1tmc8/62cRhSB0u0FdisIefUmTyx44kml8rDIrLKgwCf/bv6sWMuMo8gvyN72JWFtHm3jkTQ69M5F4hcTHvtvF6e15NM++1jmncBHuuqtRAwe8UokSiodH1nDMiVwLDWO3pgsLNH5H+U7Gi3OunUkUyQVFgvsxUTTSheH3RSkfndvisePsV1UKtJWTxM8qWUrZtB+hPyOLEWt7dUJOsTNSySb2BeLGaWwD8f3AS51tuBAbMLjFY3iFfqfH6I3mnSRxQM3J9w2cIpeISyrzsdtrSOrj/2QUr2EyrmzjZNhgUEt4I4XqesfFGgxLnUwNww8wDHGAWJs+rGi8vrytJWjrHfINYlPbIv0jSQYX0bMBB/FOIfohuTnMwZI4myACRG8gB2KhBiUE";

        public static string CurrentLanguage = "EN";
        public static ServiceProvider serviceProvider;
        public static KegIDClient kegIDClient;

        public App(IPlatformInitializer initializer = null) : base(initializer) { }
        protected override async void OnInitialized()
        {
            // Initialize Live Reload.
            //#if DEBUG
            //            LiveReload.Init();
            //#endif
            InitializeComponent();

            serviceProvider = new ServiceCollection()
                .AddSingleton<HttpClient>()
                .AddTransient<KegIDClient>()
                .BuildServiceProvider();

            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.Android:
                    ScanditService.ScanditLicense.AppKey = appAndroidKey;
                    break;
                case Xamarin.Forms.Device.iOS:
                    ScanditService.ScanditLicense.AppKey = appiOSKey;
                    break;
            }

            kegIDClient = serviceProvider.GetService<KegIDClient>();

            if (!string.IsNullOrEmpty(AppSettings.SessionId))
            {
                await NavigationService.NavigateAsync("NavigationPage/MainPage");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/LoginView");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Main Navigation for MasterPage.
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<ScanditScanView, ScanditScanViewModel>();

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
            containerRegistry.RegisterForNavigation<AboutAppView, AboutAppViewModel>();
            
            containerRegistry.RegisterForNavigation<WhatIsNewView, WhatIsNewViewModel>();
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            
            //Popup Navigation Register
            containerRegistry.RegisterPopupNavigationService();

            //Services Register
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
            containerRegistry.Register<IGeolocationService, GeolocationService>();
        }

        protected async override void OnStart()
        {
            await Distribute.SetEnabledAsync(true);
            // In this example OnReleaseAvailable is a method name in same class
            Distribute.ReleaseAvailable = OnReleaseAvailable;

            //AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("uwp=0404c586-124c-4b55-8848-910689b6881b;" +
                   "android=31ceef42-fd24-49d3-8e7e-21f144355dde;" +
                   "ios=b80b8476-04cf-4fc3-b7f7-be06ba7f2213",
                   typeof(Analytics), typeof(Crashes), typeof(Distribute));

            var _syncManager = Container.Resolve<SyncManager>();
            _syncManager.NotifyConnectivityChanged();

            try
            {
                var _geolocationService = Container.Resolve<GeolocationService>();
                await _geolocationService.InitCurrentLocationAsync();
            }
            catch { }

            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.Android:
                    var permission = await DependencyService.Get<IPermission>().VerifyStoragePermissions();
                    break;
            }
        }

        protected override void OnSleep ()
		{
            serviceProvider.Dispose();
        }

        protected override void OnResume()
        {

        }

        private bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            // Look at releaseDetails public properties to get version information, release notes text or release notes URL
            string versionName = releaseDetails.ShortVersion;
            string versionCodeOrBuildNumber = releaseDetails.Version;
            string releaseNotes = releaseDetails.ReleaseNotes;
            Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

            // custom dialog
            var title = "Version " + versionName + " available!";
            Task answer;

            // On mandatory update, user cannot postpone
            if (releaseDetails.MandatoryUpdate)
            {
                answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install");
            }
            else
            {
                answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install", "Maybe tomorrow...");
            }
            answer.ContinueWith((task) =>
            {
                // If mandatory or if answer was positive
                if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                {
                    // Notify SDK that user selected update
                    Distribute.NotifyUpdateAction(UpdateAction.Update);
                }
                else
                {
                    // Notify SDK that user selected postpone (for 1 day)
                    // Note that this method call is ignored by the SDK if the update is mandatory
                    Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                }
            });

            // Return true if you are using your own dialog, false otherwise
            return true;
        }
    }
}
