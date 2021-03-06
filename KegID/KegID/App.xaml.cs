﻿using KegID.Views;
using KegID.ViewModel;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism.DryIoc;
using Prism.Ioc;
using KegID.Services;
using Xamarin.Forms;
using Prism.Plugin.Popups;
using Microsoft.AppCenter.Distribute;
using System;
using System.Threading.Tasks;
using KegID.DependencyServices;
using KegID.Common;
using Xamarin.Essentials;
using Prism.Logging;
using Prism;

namespace KegID
{
    public partial class App : PrismApplication
    {
        public static string CurrentLanguage = "EN";
        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;

        protected override async void OnInitialized()
        {
            InitializeComponent();

            VersionTracking.Track();
            Xamarin.Forms.Device.SetFlags(new[] { "CarouselView_Experimental", "IndicatorView_Experimental", "SwipeView_Experimental" });


            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Logger.Log(e.Exception.ToString(), Category.Exception, Priority.High);
            };
#if DEBUG
            HotReloader.Current.Run(this);
            ConstantManager.BaseUrl = ConstantManager.TestApiUrl;
#elif RELEASE
            ConstantManager.BaseUrl = ConstantManager.TestApiUrl;
#endif

            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case Xamarin.Forms.Device.Android:
                    var permission = await DependencyService.Get<IPermission>().VerifyStoragePermissions();
                    break;
            }

            var versionUpdated = VersionTracking.CurrentVersion.CompareTo(VersionTracking.PreviousVersion);
            if (string.IsNullOrEmpty(Settings.UserId))
            {
                await NavigationService.NavigateAsync("NavigationPage/LoginView");
            }
            else if (versionUpdated > 0 && VersionTracking.IsFirstLaunchForCurrentVersion && VersionTracking.PreviousVersion != null)
            {
                await NavigationService.NavigateAsync("NavigationPage/WhatIsNewView");
            }
            else
            {
                if (TargetIdiom.Tablet == Xamarin.Forms.Device.Idiom)
                    await NavigationService.NavigateAsync("NavigationPage/MainPageTablet");
                else
                    await NavigationService.NavigateAsync("NavigationPage/MainPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Main Navigation for MasterPage.
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            containerRegistry.RegisterForNavigation<MainPageTablet, MainViewModel>();

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
            containerRegistry.RegisterForNavigation<SKUView, SKUViewModel>();

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
            containerRegistry.RegisterForNavigation<ScanKegsTabView, ScanKegsViewModel>();

            containerRegistry.RegisterForNavigation<AssetProfileView, AssetProfileViewModel>();

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

            //Popup Navigation Register
            containerRegistry.RegisterPopupNavigationService();

            //Services Register
            containerRegistry.Register<IInitializeMetaData, InitializeMetaData>();
            containerRegistry.Register<IZebraPrinterManager, ZebraPrinterManager>();
            containerRegistry.Register<IManifestManager, ManifestManager>();
            containerRegistry.Register<IGetIconByPlatform, GetIconByPlatform>();
            containerRegistry.Register<ISyncManager, SyncManager>();
            containerRegistry.Register<IUuidManager, UuidManager>();
            containerRegistry.Register<ICalcCheckDigitMngr, CalcCheckDigitMngr>();
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
        }

        protected override void OnSleep()
        {
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
                if (releaseDetails.MandatoryUpdate || (task as Task<bool>)?.Result == true)
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
