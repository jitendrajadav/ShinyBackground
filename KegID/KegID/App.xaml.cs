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

namespace KegID
{
    public partial class App : PrismApplication
    {
        public const string appKey = "AQv7w5KBBqf8AtyImS1bVZclZ7c8B2+dMUFvZdxwZiBtT+jDD3rYlmBxC5EqKzdBWGlPxPFpBSZ4OGzKP1u/AldmFlQJQ17YaDnlcqtFbFandXpkbWryUsF3/Soeeb/yGS+eTsBKU/cSDEKHKzikBBVQYuZdfYxou8Nj4nMnBH1AWqHnq96P3EhKEL82Gplglc6DoVWHWO/pJtc52dTRLriz2gjmikEZ2t3ktsqcigBeA4M7AwkteR1iBCY795oqgnacZtm+3+yQO1Z1XSWqDkMWv/gGPa62Gu/mpZ46EK4oXkaFdxxmAg8IG7uDvLOc4EyYETa/tMZS8eBTLjbIikZTBNuWIHLpcyjyrsb4t2XlPc9W1UFKR8dxXYtU2HWXlBL4+ECS+Ma3o+cZCZlUf25TnZ+JXX0vIY8ETDRMjcYMbp5oBzHta+0RIHDcTrfSzl3Ye/7W5RWawMd0CNIQT0zg+6twEeOXAfR3kVPgVbHFNh8+2Gw+mir1c2jJKKq6beLRaK/y3PyYO6OFRqppKyxjG7Q7nY48Khy7zMkTG647CwYlKlT8Ipuj+eTBuwaq9IM7OiL1BT397tm8WGkeyMI7TlPvRRNPJLuiiLOC+w8U9auU8lUdqaGOrLegDzqvKoY/oH3jD/9I7uY850XbBK/UODW+SgMZdar6H1zs/a8lyQlRBWciEmR6aujG/9w83aVmbIjVn7s1POoGGA6aJwZTgml7Pj4bZOaPuAH9Jd8naQnt2V7jNJ9vP8y/Mot1N/24trAaxsfOHWIbyd8bmEwGCTInNE311JsYOQXPH6yDrX0iP9Fz8MvX7VA=";
                                    //test for android by JJ //"AV87+gFdHcH/Bw8f6Du0RYgICOjoAmi12EpSKetnwi0KdwfsjToWLDNVEPezTTDLhXkfeTRCGOGiX6DeL0FSS2Mn4GhOYrnD6jL3LWpEjTfiYE/tqnKlELhaVNHyamOOV2iQ6OUslmz/Jp8uQKm2CXBzgbkbKCuOsipHEE+m3ghh4rdGPXvOzR9zPYtwirbjR7/WwkIJHdbPctn8YzhtJGk4kV2dJ2KsL0ttyaJBo6dY7+dn9AZ+aBgR34i8Upb43WK7cDL1cz5TVlRXxHWamdCyJSV9Z7VBd3Lr0fgAafPsE9bfufnX4YM7pp9d4ZZUiCosu3/0UYfT1++U/O0B6MidtpchlxCIsoXkPGJZnkWc/M1EETtCzryGSJN9B+cUlQYlUxf7hRq14D1sH44hNBUUf8k0QozlhK7a7vQEfMaR3d0sO77vV0lcBHCjs2uTkDRzDxQZPkmJzVDSgAP8BVYLuWgxgeZsfBb2uG8o7/+BFQZca8+2yaKB87X1RTMNzfqx/obsZn5elm9MZmeM6dMZErGF4IUHD4m9IyDuzFQbph3NkkI/N7zqS0MGNRc8XL5ujmvFPAmTbEFd63efpRAqHKbkE6uKPihuvpOi/NIiEZ96CeTqzvioOffIU3CMQrO/lN/u4/QsTQEYFkfpyGltrrofm8Gas9BS7V8ztfc39Ode/fRhjJ1I7i8l6qOaVhQNwEeR5wmnTlYuwLZtDhkJJhF65PuctpsrxEZ2/MwnAP8M7z9Cd24K4aWU3S5rT+g7Un4RGKwRZ6mShcK9e6vnjFa/opS7yhs+XohhYIwXe8F0gfm/4aebxaA=";
                                    //for Android from SLG   //"AQv7w5KBBqf8AtyImS1bVZclZ7c8B2+dMUFvZdxwZiBtT+jDD3rYlmBxC5EqKzdBWGlPxPFpBSZ4OGzKP1u/AldmFlQJQ17YaDnlcqtFbFandXpkbWryUsF3/Soeeb/yGS+eTsBKU/cSDEKHKzikBBVQYuZdfYxou8Nj4nMnBH1AWqHnq96P3EhKEL82Gplglc6DoVWHWO/pJtc52dTRLriz2gjmikEZ2t3ktsqcigBeA4M7AwkteR1iBCY795oqgnacZtm+3+yQO1Z1XSWqDkMWv/gGPa62Gu/mpZ46EK4oXkaFdxxmAg8IG7uDvLOc4EyYETa/tMZS8eBTLjbIikZTBNuWIHLpcyjyrsb4t2XlPc9W1UFKR8dxXYtU2HWXlBL4+ECS+Ma3o+cZCZlUf25TnZ+JXX0vIY8ETDRMjcYMbp5oBzHta+0RIHDcTrfSzl3Ye/7W5RWawMd0CNIQT0zg+6twEeOXAfR3kVPgVbHFNh8+2Gw+mir1c2jJKKq6beLRaK/y3PyYO6OFRqppKyxjG7Q7nY48Khy7zMkTG647CwYlKlT8Ipuj+eTBuwaq9IM7OiL1BT397tm8WGkeyMI7TlPvRRNPJLuiiLOC+w8U9auU8lUdqaGOrLegDzqvKoY/oH3jD/9I7uY850XbBK/UODW+SgMZdar6H1zs/a8lyQlRBWciEmR6aujG/9w83aVmbIjVn7s1POoGGA6aJwZTgml7Pj4bZOaPuAH9Jd8naQnt2V7jNJ9vP8y/Mot1N/24trAaxsfOHWIbyd8bmEwGCTInNE311JsYOQXPH6yDrX0iP9Fz8MvX7VA=";
                                    // for iOS Beta from SLG //AfZLcFjmRttIKSgzrTsvZw0OHeoICxs+IzO9Pg1aN8CUY8E1Z0MAn45SJfUGK87Wh0BYqftD6k1gSltf02cobChLzolPYriTmguX/JExthMvWtJ0yETXKQpzBuPofaSpInozs9oUmwzANmdGCgPpvCz2IURgWo3zmR+2sfZJ+/r4mT3BN84LmBNpecZkv2yXhv9qXf69wkqAoNqXEOa8q8BWFqT6h982b8ZVLbW+NhBFv8atAVLgLFYZbI17CFzCAoBhJCIIM1be0R/EWiHHJkVuU53vSNtkSLrVgglb5NMOKjTegI0hqEca3g+VTPms6j3/DsdwOGXZfg4fvRGNky1Pkh3FOcP250pLl+ew6BJwt15+9u6arSB0CN+c4ZgzLvkT7V+Xkn8rN6VAgLvLEPS+A+SONdjlJboQbXE8mWEKBJ1tmc8/62cRhSB0u0FdisIefUmTyx44kml8rDIrLKgwCf/bv6sWMuMo8gvyN72JWFtHm3jkTQ69M5F4hcTHvtvF6e15NM++1jmncBHuuqtRAwe8UokSiodH1nDMiVwLDWO3pgsLNH5H+U7Gi3OunUkUyQVFgvsxUTTSheH3RSkfndvisePsV1UKtJWTxM8qWUrZtB+hPyOLEWt7dUJOsTNSySb2BeLGaWwD8f3AS51tuBAbMLjFY3iFfqfH6I3mnSRxQM3J9w2cIpeISyrzsdtrSOrj/2QUr2EyrmzjZNhgUEt4I4XqesfFGgxLnUwNww8wDHGAWJs+rGi8vrytJWjrHfINYlPbIv0jSQYX0bMBB/FOIfohuTnMwZI4myACRG8gB2KhBiUE;
                                    // for trial android from JJ//"AVjr0QddG/BwDeDnkkC+ua8/Tzm5Q2nG33rlj0Zgv+LsW7FSAwv4p1N/0A/varbfP13F9rUiJFRvUOTh3jIu/o8OX/w1dBFoGWse5WVE1kw/I9/MqDUx2LBlg3kIo4Gtlwi1k1lHxLO+dA0JL9dfzPx+UM9hdCi2oOVZFI9Zu/GKeCrNxxPkfps8VuP/ZOcgKyjwR2g4ncamPj30l2BJilWKRLl72UbtsNYOQYFUpIL8QGLwapml8APcvZ6MqbBMXt1g4t6mbFakagYJG+9c1I2mFHt8DmYnsaHb+rLVDSGlFAjUVYcVcJWr4OEu85+abrnV6rYFSxd3Z+ngH+PbBYVMeNpD1IFnpi1eQIpor/1VpO6pEYoQ61RxJD/2IYK/1vEx1Ay5vtgWgBqRqIrm4v8O8UGpka4y4Lvyu4QLSNtsi6DoLVbiDdvtdZKw+JJ4XN1P6P384NxJVB7YwoBR6UDSP5OjGxKL1ztfHBGZ2lxH6PlfAopRv6xPf30er/QlbCcvBZc47TxaSD2kGqEhDVD3+rh4tFX8JeUp6qdeC40A1Z9xR24hkqIGKoB1WyX2fHfionCi+CD2WmgJINvLuJvHuXtM55M/FqGcmOuyKlHLhdUwvjt+Kfg9kB4c27p4U93yaNTit75spft0nOPF/x6dXXHgq9mt6r7we7e9wgZWYWQkG7F0XwyMCjWBjangYHv7O+K4kpeyPxo+iXPFslH9Ty9wmINH10bGpcrEl9yXZFKsqi+K9VN8miRa6HWR0UAAS42SHJWaMYyp++3witJbFT8=";
                                    //for windows from JJ  //"ARN73xRdCHXbEFujOB6EQ94sXhF6A9okp3GNbSdcnh1GZ4uuUz+IrPNHcKNjaz24n1IuVGANB/awQyG8O3bIjNRjfdBJDhQMf33anNdKMqIIMaiLQSv7scwYNc70kXprMU66ttMkHXNGcRdCa/4rKnMXfwOn689Ye+dr96FldGSyb5aPiKiBA8gQ8dc+E1mCOVcyFRB4qkW53YuTD6TuFlBOS8VQ9keMyLcu61COa7Iz2JEdCTUR88tRVmF3n+BAWgbZECGdz2tfN60seWnbsENfzycYZgEIzt2c5fkYbKZgZltrC6v/ZhJ3sEfWpIzInonjBoXhotQVZTp30YSM7T9CfJXGsL+79Rgo23I/9V2N1K+x+sxRquQxoKnrtxUUnyL5nzRV9lBkiN4ugPAaECGFNhPkU/cOiXCaSv4iiBlv+zMHOAfEVvvK/CJ2xHJThqOuXpizufR64CEKFh+mA5QIrSo9+ZJBEUuzwxpFTZJL4AkIKbiQqag1UdBbUjDS6NeVsmJ8fCtQjVFIRnmX/vMSFyVEr8HliVHl5/1Kz4rL6YBfAtdLDKwGtmkCcmVEaNgGpdajDApmuZ4t5V2Viapy9rMp4v471d78u4DSeqRTBlNBeKkSUYn0/BJNYvY/cJ1pa9vtRDnK2vtXv93wvip+hXb9BZo4jSFt3hujO7NxKtdQEbDQLNkNbn625K2QPwdbaCAdhcKPK4lodWnmiSY3sUGTwqQdQuXf+nDL9nTL8EuDsoBQnH1VZh+tIzSZmUgkLrIFmeLH9CIukyjGXm7Mhaw=";

        public static string CurrentLanguage = "EN";
        public static ServiceProvider serviceProvider;
        public static KegIDClient kegIDClient;

        public App(IPlatformInitializer initializer = null) : base(initializer) { }
        protected override void OnInitialized()
        {
            // Initialize Live Reload.
#if DEBUG
            LiveReload.Init();
#endif
            InitializeComponent();

            serviceProvider = new ServiceCollection()
                .AddSingleton<HttpClient>()
                .AddTransient<KegIDClient>()
                .BuildServiceProvider();

            ScanditService.ScanditLicense.AppKey = appKey;

            kegIDClient = serviceProvider.GetService<KegIDClient>();

            if (!string.IsNullOrEmpty(AppSettings.SessionId))
                NavigationService.NavigateAsync(nameof(KegID.MainPage));
            else
                NavigationService.NavigateAsync(nameof(LoginView));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Main Navigation for MasterPage.
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            //containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
            //containerRegistry.RegisterForNavigation<MenuView, DashboardViewModel>();
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
        }

        protected override void OnSleep ()
		{
            serviceProvider.Dispose();
            AppSettings.IsFreshInstall = false;
        }

        protected override void OnResume ()
		{
            // Handle when your app resumes
        }
    }
}
