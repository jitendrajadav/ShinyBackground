using GalaSoft.MvvmLight.Ioc;

namespace KegID.ViewModel
{
    public class ViewModelLocator
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<KegIDMasterPageMasterViewModel>();
            SimpleIoc.Default.Register<MoveViewModel>();
            SimpleIoc.Default.Register<FillViewModel>();
            SimpleIoc.Default.Register<PalletizeViewModel>();
            SimpleIoc.Default.Register<MaintainViewModel>();
            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<WhatIsNewViewModel>();
            SimpleIoc.Default.Register<PartnersViewModel>();
            SimpleIoc.Default.Register<AddPartnerViewModel>();
            SimpleIoc.Default.Register<EditAddressViewModel>();
            SimpleIoc.Default.Register<SearchPartnersViewModel>();
            SimpleIoc.Default.Register<AddTagsViewModel>();
            SimpleIoc.Default.Register<ScanKegsViewModel>();
            SimpleIoc.Default.Register<BatchViewModel>();
            SimpleIoc.Default.Register<SizeViewModel>();
            SimpleIoc.Default.Register<ValidateBarcodeViewModel>();
            SimpleIoc.Default.Register<PrinterSettingViewModel>();
            SimpleIoc.Default.Register<ScanInfoViewModel>();
            SimpleIoc.Default.Register<ManifestsViewModel>();
            SimpleIoc.Default.Register<SearchManifestsViewModel>();
            SimpleIoc.Default.Register<SearchedManifestsListViewModel>();
            SimpleIoc.Default.Register<ManifestDetailViewModel>();
            SimpleIoc.Default.Register<ContentTagsViewModel>();
            SimpleIoc.Default.Register<AddPalletsViewModel>();
            SimpleIoc.Default.Register<InventoryViewModel>();
            SimpleIoc.Default.Register<FillScanViewModel>();
            SimpleIoc.Default.Register<AddBatchViewModel>();
            SimpleIoc.Default.Register<MaintainScanViewModel>();
            SimpleIoc.Default.Register<DashboardPartnersViewModel>();
            SimpleIoc.Default.Register<PartnerInfoViewModel>();
            SimpleIoc.Default.Register<BrandViewModel>();
            SimpleIoc.Default.Register<VolumeViewModel>();
            SimpleIoc.Default.Register<PalletizeDetailViewModel>();

        }
        #endregion

        #region Properties

        #region Move

        #region Move

        /// <summary>
        /// Gets the Move property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MoveViewModel Move
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MoveViewModel>();
            }
        }

        #endregion

        #region Partners

        /// <summary>
        /// Gets the Partners property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PartnersViewModel Partners
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PartnersViewModel>();
            }
        }

        #endregion

        #region AddPartner

        /// <summary>
        /// Gets the AddPartner property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddPartnerViewModel AddPartner
        {
            get
            {
                return SimpleIoc.Default.GetInstance<AddPartnerViewModel>();
            }
        }

        #endregion

        #region EditAddress

        /// <summary>
        /// Gets the EditAddress property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public EditAddressViewModel EditAddress
        {
            get
            {
                return SimpleIoc.Default.GetInstance<EditAddressViewModel>();
            }
        }

        #endregion

        #region SearchPartners

        /// <summary>
        /// Gets the SearchPartners property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SearchPartnersViewModel SearchPartners
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SearchPartnersViewModel>();
            }
        }

        #endregion

        #region AddTags

        /// <summary>
        /// Gets the AddTags property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddTagsViewModel AddTags
        {
            get
            {
                return SimpleIoc.Default.GetInstance<AddTagsViewModel>();
            }
        }

        #endregion

        #region ScanKegs

        /// <summary>
        /// Gets the ScanKegs property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ScanKegsViewModel ScanKegs
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ScanKegsViewModel>();
            }
        }

        #endregion

        #region Batch

        /// <summary>
        /// Gets the Batch property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public BatchViewModel Batch
        {
            get
            {
                return SimpleIoc.Default.GetInstance<BatchViewModel>();
            }
        }

        #endregion

        #region Size

        /// <summary>
        /// Gets the Size property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SizeViewModel Size
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SizeViewModel>();
            }
        }

        #endregion

        #region ValidateBarcode

        /// <summary>
        /// Gets the ValidateBarcode property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ValidateBarcodeViewModel ValidateBarcode
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ValidateBarcodeViewModel>();
            }
        }


        #endregion

        #region ScanInfo

        /// <summary>
        /// Gets the ScanInfo property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ScanInfoViewModel ScanInfo
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ScanInfoViewModel>();
            }
        }

        #endregion

        #region Manifests

        /// <summary>
        /// Gets the Manifests property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ManifestsViewModel Manifests
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ManifestsViewModel>();
            }
        }

        #endregion

        #region SearchManifests

        /// <summary>
        /// Gets the SearchManifests property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SearchManifestsViewModel SearchManifests
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SearchManifestsViewModel>();
            }
        }

        #endregion

        #region SearchedManifestsList

        /// <summary>
        /// Gets the SearchedManifestsList property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SearchedManifestsListViewModel SearchedManifestsList
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SearchedManifestsListViewModel>();
            }
        }

        #endregion

        #region ManifestDetail

        /// <summary>
        /// Gets the ManifestDetail property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ManifestDetailViewModel ManifestDetail
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ManifestDetailViewModel>();
            }
        }

        #endregion

        #region ContentTags

        /// <summary>
        /// Gets the ContentView property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ContentTagsViewModel ContentTags
        {
            get
            {
                return SimpleIoc.Default.GetInstance<ContentTagsViewModel>();
            }
        }

        #endregion

        #endregion

        #region Account

        #region Main

        public MainViewModel Main
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainViewModel>();
            }
        }
        #endregion

        #region Login

        /// <summary>
        /// Gets the Login property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public LoginViewModel Login
        {
            get
            {
                return SimpleIoc.Default.GetInstance<LoginViewModel>();
            }
        }

        #endregion

        #region KegIDMasterPage

        /// <summary>
        /// Gets the KegIDMasterPage property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public KegIDMasterPageMasterViewModel KegIDMasterPage
        {
            get
            {
                return SimpleIoc.Default.GetInstance<KegIDMasterPageMasterViewModel>();
            }
        }

        #endregion

        #endregion

        #region Fill
        
        #region Fill

        /// <summary>
        /// Gets the Fill property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public FillViewModel Fill
        {
            get
            {
                return SimpleIoc.Default.GetInstance<FillViewModel>();
            }
        }

        #endregion

        #region AddPallets

        /// <summary>
        /// Gets the AddPallets property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddPalletsViewModel AddPallets
        {
            get
            {
                return SimpleIoc.Default.GetInstance<AddPalletsViewModel>();
            }
        }

        #endregion

        #region FillScan

        /// <summary>
        /// Gets the FillScan property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public FillScanViewModel FillScan
        {
            get
            {
                return SimpleIoc.Default.GetInstance<FillScanViewModel>();
            }
        }

        #endregion

        #region AddBatch

        /// <summary>
        /// Gets the AddBatch property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddBatchViewModel AddBatch
        {
            get
            {
                return SimpleIoc.Default.GetInstance<AddBatchViewModel>();
            }
        }

        #endregion

        #region Brand

        /// <summary>
        /// Gets the Brand property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public BrandViewModel Brand
        {
            get
            {
                return SimpleIoc.Default.GetInstance<BrandViewModel>();
            }
        }

        #endregion

        #region Volume

        /// <summary>
        /// Gets the Volume property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public VolumeViewModel Volume
        {
            get
            {
                return SimpleIoc.Default.GetInstance<VolumeViewModel>();
            }
        }

        #endregion

        #endregion

        #region Palletize

        #region Palletize

        /// <summary>
        /// Gets the Palletize property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PalletizeViewModel Palletize
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PalletizeViewModel>();
            }
        }

        #endregion

        #region PalletizeDetail

        /// <summary>
        /// Gets the PalletizeDetail property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PalletizeDetailViewModel PalletizeDetail
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PalletizeDetailViewModel>();
            }
        }

        #endregion

        #endregion

        #region Maintain

        #region Maintain

        /// <summary>
        /// Gets the Maintain property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MaintainViewModel Maintain
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MaintainViewModel>();
            }
        }

        #endregion

        #region MaintainScan

        /// <summary>
        /// Gets the MaintainScan property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MaintainScanViewModel MaintainScan
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MaintainScanViewModel>();
            }
        }

        #endregion

        #endregion

        #region Dashboard

        #region Dashboard

        /// <summary>
        /// Gets the Dashboard property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public DashboardViewModel Dashboard
        {
            get
            {
                return SimpleIoc.Default.GetInstance<DashboardViewModel>();
            }
        }

        #endregion

        #region Inventory

        /// <summary>
        /// Gets the Inventory property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public InventoryViewModel Inventory
        {
            get
            {
                return SimpleIoc.Default.GetInstance<InventoryViewModel>();
            }
        }

        #endregion

        #region DashboardPartners

        /// <summary>
        /// Gets the DashboardPartners property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public DashboardPartnersViewModel DashboardPartners
        {
            get
            {
                return SimpleIoc.Default.GetInstance<DashboardPartnersViewModel>();
            }
        }

        #endregion

        #region PartnerInfo

        /// <summary>
        /// Gets the PartnerInfo property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PartnerInfoViewModel PartnerInfo
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PartnerInfoViewModel>();
            }
        }

        #endregion

        #endregion

        #region Setting

        #region Setting

        /// <summary>
        /// Gets the Setting property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SettingViewModel Setting
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SettingViewModel>();
            }
        }

        #endregion

        #region WhatIsNew

        /// <summary>
        /// Gets the WhatIsNew property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public WhatIsNewViewModel WhatIsNew
        {
            get
            {
                return SimpleIoc.Default.GetInstance<WhatIsNewViewModel>();
            }
        }

        #endregion

        #region PrinterSetting

        /// <summary>
        /// Gets the PrinterSetting property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PrinterSettingViewModel PrinterSetting
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PrinterSettingViewModel>();
            }
        }

        #endregion

        #endregion

        #endregion

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }

}
