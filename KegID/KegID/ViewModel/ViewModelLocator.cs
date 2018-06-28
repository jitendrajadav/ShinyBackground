//

//namespace KegID.ViewModel
//{
//    public class ViewModelLocator
//    {
//        #region Constant

//        //public const string BulkUpdateScanView = "BulkUpdateScanView";
//        //public const string DashboardPartnersView = "DashboardPartnersView";
//        //public const string DashboardView = "DashboardView";
//        //public const string EditKegView = "EditKegView";
//        //public const string InventoryView = "InventoryView";
//        //public const string KegSearchedListView = "KegSearchedListView";
//        //public const string KegSearchView = "KegSearchView";
//        //public const string KegStatusView = "KegStatusView";
//        //public const string KegsView = "KegsView";
//        //public const string PartnerInfoMapView = "PartnerInfoMapView";
//        //public const string PartnerInfoView = "PartnerInfoView";
//        //public const string AddBatchView = "AddBatchView";
//        //public const string AddPalletsView = "AddPalletsView";
//        //public const string BatchView = "BatchView";
//        //public const string BrandView = "BrandView";
//        //public const string FillScanReviewView = "FillScanReviewView";
//        //public const string FillScanView = "FillScanView";
//        //public const string FillView = "FillView";
//        //public const string SizeView = "SizeView";
//        //public const string VolumeView = "VolumeView";
//        //public const string MaintainDetailView = "MaintainDetailView";
//        //public const string MaintainScanView = "MaintainScanView";
//        //public const string MaintainView = "MaintainView";
//        //public const string MenuView = "MenuView";
//        //public const string AddPartnerView = "AddPartnerView";
//        //public const string AddTagsView = "AddTagsView";
//        //public const string AssignSizesView = "AssignSizesView";
//        //public const string ContentTagsView = "ContentTagsView";
//        //public const string EditAddressView = "EditAddressView";
//        //public const string ManifestDetailView = "ManifestDetailView";
//        //public const string ManifestsView = "ManifestsView";
//        //public const string MoveView = "MoveView";
//        //public const string PartnersView = "PartnersView";
//        //public const string ScanInfoView = "ScanInfoView";
//        //public const string ScanKegsView = "ScanKegsView";
//        //public const string SearchedManifestsListView = "SearchedManifestsListView";
//        //public const string SearchManifestsView = "SearchManifestsView";
//        //public const string SearchPartnersView = "SearchPartnersView";
//        //public const string ValidateBarcodeView = "ValidateBarcodeView";
//        //public const string PalletizeDetailView = "PalletizeDetailView";
//        //public const string PalletizeView = "PalletizeView";
//        //public const string PalletSearchedListView = "PalletSearchedListView";
//        //public const string SearchPalletView = "SearchPalletView";
//        //public const string BarcodePage = "BarcodePage";
//        //public const string CognexScanView = "CognexScanView";
//        //public const string CustomScanPage = "CustomScanPage";
//        //public const string ScannerView = "ScannerView";
//        //public const string PrinterSettingView = "PrinterSettingView";
//        //public const string SettingView = "SettingView";
//        //public const string WhatIsNewView = "WhatIsNewView";
//        //public const string LoginView = "LoginView";
//        //public const string MainPage = "MainPage"; 

//        #endregion

//        #region Constructor
//        /// <summary>
//        /// Initializes a new instance of the ViewModelLocator class.
//        /// </summary>
//        public ViewModelLocator()
//        {
//            SimpleIoc.Default.Register(() => SimpleIoc.Default);

//            #region Move
//            SimpleIoc.Default.Register<MoveViewModel>();
//            SimpleIoc.Default.Register<ScanInfoViewModel>();
//            SimpleIoc.Default.Register<ManifestsViewModel>();
//            SimpleIoc.Default.Register<SearchManifestsViewModel>();
//            SimpleIoc.Default.Register<SearchedManifestsListViewModel>();
//            SimpleIoc.Default.Register<ManifestDetailViewModel>();
//            SimpleIoc.Default.Register<BatchViewModel>();
//            SimpleIoc.Default.Register<SizeViewModel>();
//            SimpleIoc.Default.Register<ContentTagsViewModel>();
//            SimpleIoc.Default.Register<ValidateBarcodeViewModel>();
//            SimpleIoc.Default.Register<PartnersViewModel>();
//            SimpleIoc.Default.Register<AddPartnerViewModel>();
//            SimpleIoc.Default.Register<EditAddressViewModel>();
//            SimpleIoc.Default.Register<SearchPartnersViewModel>();
//            SimpleIoc.Default.Register<AddTagsViewModel>();
//            SimpleIoc.Default.Register<ScanKegsViewModel>();
//            SimpleIoc.Default.Register<AssignSizesViewModel>();
//            #endregion

//            #region Account
//            SimpleIoc.Default.Register<MainViewModel>();
//            SimpleIoc.Default.Register<LoginViewModel>();
//            #endregion

//            #region Dashboard
//            SimpleIoc.Default.Register<DashboardPartnersViewModel>();
//            SimpleIoc.Default.Register<InventoryViewModel>();
//            SimpleIoc.Default.Register<DashboardViewModel>();
//            SimpleIoc.Default.Register<PartnerInfoViewModel>();
//            SimpleIoc.Default.Register<PartnerInfoMapViewModel>();
//            SimpleIoc.Default.Register<KegsViewModel>();
//            SimpleIoc.Default.Register<KegStatusViewModel>();
//            SimpleIoc.Default.Register<EditKegViewModel>();
//            SimpleIoc.Default.Register<KegSearchViewModel>();
//            SimpleIoc.Default.Register<BulkUpdateScanViewModel>();
//            SimpleIoc.Default.Register<KegSearchedListViewModel>();
//            #endregion

//            #region Fill
//            SimpleIoc.Default.Register<FillViewModel>();
//            SimpleIoc.Default.Register<FillScanViewModel>();
//            SimpleIoc.Default.Register<FillScanReviewViewModel>();
//            SimpleIoc.Default.Register<AddPalletsViewModel>();
//            SimpleIoc.Default.Register<BrandViewModel>();
//            SimpleIoc.Default.Register<VolumeViewModel>();
//            SimpleIoc.Default.Register<AddBatchViewModel>();
//            #endregion

//            #region Palletize

//            SimpleIoc.Default.Register<PalletizeViewModel>();
//            SimpleIoc.Default.Register<PalletizeDetailViewModel>();
//            SimpleIoc.Default.Register<SearchPalletViewModel>();
//            SimpleIoc.Default.Register<PalletSearchedListViewModel>();

//            #endregion

//            #region Maintain
//            SimpleIoc.Default.Register<MaintainViewModel>();
//            SimpleIoc.Default.Register<MaintainScanViewModel>();
//            SimpleIoc.Default.Register<MaintainDetailViewModel>();
//            #endregion

//            #region Setting
//            SimpleIoc.Default.Register<SettingViewModel>();
//            SimpleIoc.Default.Register<WhatIsNewViewModel>();
//            SimpleIoc.Default.Register<PrinterSettingViewModel>();
//            #endregion

//        }

//        #endregion

//        #region Properties

//        #region Move

//        #region Move

//        /// <summary>
//        /// Gets the Move property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public MoveViewModel Move
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<MoveViewModel>();
//            }
//        }

//        #endregion

//        #region Partners

//        /// <summary>
//        /// Gets the Partners property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public PartnersViewModel Partners
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<PartnersViewModel>();
//            }
//        }

//        #endregion

//        #region AddPartner

//        /// <summary>
//        /// Gets the AddPartner property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public AddPartnerViewModel AddPartner
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<AddPartnerViewModel>();
//            }
//        }

//        #endregion

//        #region EditAddress

//        /// <summary>
//        /// Gets the EditAddress property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public EditAddressViewModel EditAddress
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<EditAddressViewModel>();
//            }
//        }

//        #endregion

//        #region SearchPartners

//        /// <summary>
//        /// Gets the SearchPartners property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public SearchPartnersViewModel SearchPartners
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<SearchPartnersViewModel>();
//            }
//        }

//        #endregion

//        #region AddTags

//        /// <summary>
//        /// Gets the AddTags property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public AddTagsViewModel AddTags
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<AddTagsViewModel>();
//            }
//        }

//        #endregion

//        #region ScanKegs

//        /// <summary>
//        /// Gets the ScanKegs property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public ScanKegsViewModel ScanKegs
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<ScanKegsViewModel>();
//            }
//        }

//        #endregion

//        #region Batch

//        /// <summary>
//        /// Gets the Batch property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public BatchViewModel Batch
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<BatchViewModel>();
//            }
//        }

//        #endregion

//        #region Size

//        /// <summary>
//        /// Gets the Size property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public SizeViewModel Size
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<SizeViewModel>();
//            }
//        }

//        #endregion

//        #region ValidateBarcode

//        /// <summary>
//        /// Gets the ValidateBarcode property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public ValidateBarcodeViewModel ValidateBarcode
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>();
//            }
//        }


//        #endregion

//        #region ScanInfo

//        /// <summary>
//        /// Gets the ScanInfo property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public ScanInfoViewModel ScanInfo
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<ScanInfoViewModel>();
//            }
//        }

//        #endregion

//        #region Manifests

//        /// <summary>
//        /// Gets the Manifests property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public ManifestsViewModel Manifests
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<ManifestsViewModel>();
//            }
//        }

//        #endregion

//        #region SearchManifests

//        /// <summary>
//        /// Gets the SearchManifests property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public SearchManifestsViewModel SearchManifests
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<SearchManifestsViewModel>();
//            }
//        }

//        #endregion

//        #region SearchedManifestsList

//        /// <summary>
//        /// Gets the SearchedManifestsList property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public SearchedManifestsListViewModel SearchedManifestsList
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<SearchedManifestsListViewModel>();
//            }
//        }

//        #endregion

//        #region ManifestDetail

//        /// <summary>
//        /// Gets the ManifestDetail property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public ManifestDetailViewModel ManifestDetail
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<ManifestDetailViewModel>();
//            }
//        }

//        #endregion

//        #region ContentTags

//        /// <summary>
//        /// Gets the ContentView property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public ContentTagsViewModel ContentTags
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<ContentTagsViewModel>();
//            }
//        }

//        #endregion

//        #region AssignSizes

//        /// <summary>
//        /// Gets the AssignSizes property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public AssignSizesViewModel AssignSizes
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<AssignSizesViewModel>();
//            }
//        }

//        #endregion

//        #endregion

//        #region Account

//        #region Main

//        public MainViewModel Main
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<MainViewModel>();
//            }
//        }
//        #endregion

//        #region Login

//        /// <summary>
//        /// Gets the Login property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public LoginViewModel Login
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<LoginViewModel>();
//            }
//        }

//        #endregion

//        #endregion

//        #region Dashboard

//        #region Dashboard

//        /// <summary>
//        /// Gets the Dashboard property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public DashboardViewModel Dashboard
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<DashboardViewModel>();
//            }
//        }

//        #endregion

//        #region Inventory

//        /// <summary>
//        /// Gets the Inventory property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public InventoryViewModel Inventory
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<InventoryViewModel>();
//            }
//        }

//        #endregion

//        #region DashboardPartners

//        /// <summary>
//        /// Gets the DashboardPartners property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public DashboardPartnersViewModel DashboardPartners
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<DashboardPartnersViewModel>();
//            }
//        }

//        #endregion

//        #region PartnerInfo

//        /// <summary>
//        /// Gets the PartnerInfo property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public PartnerInfoViewModel PartnerInfo
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<PartnerInfoViewModel>();
//            }
//        }

//        #endregion

//        #region PartnerInfoMap

//        /// <summary>
//        /// Gets the PartnerInfoMap property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public PartnerInfoMapViewModel PartnerInfoMap
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<PartnerInfoMapViewModel>();
//            }
//        }

//        #endregion

//        #region Kegs

//        /// <summary>
//        /// Gets the Kegs property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public KegsViewModel Kegs
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<KegsViewModel>();
//            }
//        }

//        #endregion

//        #region KegStatus

//        /// <summary>
//        /// Gets the KegStatus property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public KegStatusViewModel KegStatus
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<KegStatusViewModel>();
//            }
//        }

//        #endregion

//        #region EditKeg

//        /// <summary>
//        /// Gets the EditKeg property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public EditKegViewModel EditKeg
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<EditKegViewModel>();
//            }
//        }

//        #endregion

//        #region KegSearch

//        /// <summary>
//        /// Gets the KegSearch property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public KegSearchViewModel KegSearch
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<KegSearchViewModel>();
//            }
//        }

//        #endregion

//        #region BulkUpdateScan

//        /// <summary>
//        /// Gets the BulkUpdateScan property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public BulkUpdateScanViewModel BulkUpdateScan
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<BulkUpdateScanViewModel>();
//            }
//        }

//        #endregion

//        #region KegSearchedList

//        /// <summary>
//        /// Gets the KegSearchedList property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public KegSearchedListViewModel KegSearchedList
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<KegSearchedListViewModel>();
//            }
//        }

//        #endregion

//        #endregion

//        #region Fill

//        #region Fill

//        /// <summary>
//        /// Gets the Fill property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public FillViewModel Fill
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<FillViewModel>();
//            }
//        }

//        #endregion

//        #region AddPallets

//        /// <summary>
//        /// Gets the AddPallets property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public AddPalletsViewModel AddPallets
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<AddPalletsViewModel>();
//            }
//        }

//        #endregion

//        #region FillScan

//        /// <summary>
//        /// Gets the FillScan property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public FillScanViewModel FillScan
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<FillScanViewModel>();
//            }
//        }

//        #endregion

//        #region FillScanReview

//        /// <summary>
//        /// Gets the FillScanReview property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public FillScanReviewViewModel FillScanReview
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<FillScanReviewViewModel>();
//            }
//        }

//        #endregion

//        #region AddBatch

//        /// <summary>
//        /// Gets the AddBatch property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public AddBatchViewModel AddBatch
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<AddBatchViewModel>();
//            }
//        }

//        #endregion

//        #region Brand

//        /// <summary>
//        /// Gets the Brand property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public BrandViewModel Brand
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<BrandViewModel>();
//            }
//        }

//        #endregion

//        #region Volume

//        /// <summary>
//        /// Gets the Volume property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public VolumeViewModel Volume
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<VolumeViewModel>();
//            }
//        }

//        #endregion

//        #endregion

//        #region Palletize

//        #region Palletize

//        /// <summary>
//        /// Gets the Palletize property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public PalletizeViewModel Palletize
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<PalletizeViewModel>();
//            }
//        }

//        #endregion

//        #region PalletizeDetail

//        /// <summary>
//        /// Gets the PalletizeDetail property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public PalletizeDetailViewModel PalletizeDetail
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<PalletizeDetailViewModel>();
//            }
//        }

//        #endregion

//        #region SearchPallet

//        /// <summary>
//        /// Gets the SearchPallet property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public SearchPalletViewModel SearchPallet
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<SearchPalletViewModel>();
//            }
//        }

//        #endregion

//        #region PalletSearchedList

//        /// <summary>
//        /// Gets the PalletSearchedList property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public PalletSearchedListViewModel PalletSearchedList
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<PalletSearchedListViewModel>();
//            }
//        }

//        #endregion

//        #endregion

//        #region Maintain

//        #region Maintain

//        /// <summary>
//        /// Gets the Maintain property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public MaintainViewModel Maintain
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<MaintainViewModel>();
//            }
//        }

//        #endregion

//        #region MaintainScan

//        /// <summary>
//        /// Gets the MaintainScan property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public MaintainScanViewModel MaintainScan
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<MaintainScanViewModel>();
//            }
//        }

//        #endregion

//        #region MaintainDetail

//        /// <summary>
//        /// Gets the MaintainDetail property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public MaintainDetailViewModel MaintainDetail
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<MaintainDetailViewModel>();
//            }
//        }

//        #endregion

//        #endregion

//        #region Setting

//        #region Setting

//        /// <summary>
//        /// Gets the Setting property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public SettingViewModel Setting
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<SettingViewModel>();
//            }
//        }

//        #endregion

//        #region WhatIsNew

//        /// <summary>
//        /// Gets the WhatIsNew property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public WhatIsNewViewModel WhatIsNew
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<WhatIsNewViewModel>();
//            }
//        }

//        #endregion

//        #region PrinterSetting

//        /// <summary>
//        /// Gets the PrinterSetting property.
//        /// </summary>
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
//            "CA1822:MarkMembersAsStatic",
//            Justification = "This non-static member is needed for data binding purposes.")]
//        public PrinterSettingViewModel PrinterSetting
//        {
//            get
//            {
//                return SimpleIoc.Default.GetInstance<PrinterSettingViewModel>();
//            }
//        }

//        #endregion

//        #endregion

//        #endregion

//        public static void Cleanup()
//        {
//            // TODO Clear the ViewModels
//        }
//    }
//}
