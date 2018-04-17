namespace KegID.Services
{
    public static class Configuration
    {

        /// <summary>
        /// Constant flag
        /// </summary>
        public const string NewManifest = "NewManifest";
        public const string NewPartner = "NewPartner";
        public const string NewBatch = "Batch";
        public const string NewPallet = "NewPallet";
        public const string PostedMaintenanceDone = "PostedMaintenanceDone";
        public const string NewKeg = "NewKeg";
        public const string PostedMaintenanceAlert = "PostedMaintenanceAlert";
        public const string DeleteTypeMaintenanceAlert = "DeleteType";
        public const string KegUploadModel = "KegUploadModel";
        public const string MassUpdateKegList = "MassUpdateKegList";
        

        public const string StageAPIUrl = "https://stageapi.kegid.com/api/";
        public const string TestAPIUrl = "https://testapi.kegid.com/api/";

        /// <summary>
        /// Base Url
        /// </summary>
        public const string ServiceUrl = StageAPIUrl;

        /// <summary>
        /// Login Service Url
        /// </summary>
        public const string GetLoginUserUrl = ServiceUrl + "login/get?username={0}&password={1}";
        public const string GetUserUrl = ServiceUrl + "user/get?username={0}&password={1}";

        /// <summary>
        /// Dashboard Service Url
        /// </summary>
        public const string GetDashboardUrl = ServiceUrl + "dashboard/get?sessionId={0}";
        public const string GetInventoryUrl = ServiceUrl + "Inventory/?sessionId={0}";
        public const string GetKegPossessionByPartnerIdUrl = ServiceUrl + "KegPossession/?sessionId={0}&partnerid={1}";
        public const string GetPartnerInfoByPartnerIdUrl = ServiceUrl + "Partner/?sessionId={0}&id={1}";
        public const string GetKegStatusByKegIdUrl = ServiceUrl + "Keg/{0}?sessionId={1}";
        public const string GetKegSearchByBarcodeUrl = ServiceUrl + "Keg/?sessionId={0}&barcode={1}&includePartials={2}";

        public const string GetKegMaintenanceHistoryByKegIdUrl = ServiceUrl + "KegMaintenanceHistory/{0}?sessionId={1}";
        public const string GetMaintenanceAlertByKegIdUrl = ServiceUrl + "MaintenanceAlert?kegId={0}&sessionId={1}";
        
        public const string GetDeleteMaintenanceAlertByKegIdUrl = ServiceUrl + "DeleteMaintenanceAlert?kegId={0}&sessionId={1}";
        public const string GetPalletSearchUrl = ServiceUrl + "PalletSearch?sessionId={0}&locationId={1}&fromDate={2}&toDate={3}&kegs={4}&kegOwnerId={5}";
        public const string GetPalletUrl = ServiceUrl + "Pallet/?sessionId={0}&barcode={1}";
        public const string GetAssetSize = ServiceUrl + "AssetSize/?sessionId={0}&assignableOnly={1}";
        public const string GetAssetType = ServiceUrl + "AssetType/?sessionId={0}&assignableOnly={1}";
        public const string GetAssetVolume = ServiceUrl + "AssetVolume/?sessionId={0}&assignableOnly={1}";

        public const string PostKegsUrl = ServiceUrl + "Kegs/?sessionId={0}";
        public const string PostKegUrl = ServiceUrl + "Keg/?sessionId={0}";

        public const string PostMaintenanceAlertUrl = ServiceUrl + "MaintenanceAlert/?sessionId={0}";
        public const string PostMaintenanceDeleteAlertUrl = ServiceUrl + "MaintenanceAlert/DeleteAlert/?sessionId={0}";
        
        /// <summary>
        /// Move Service Url
        /// </summary>
        public const string GetPartnerBySesssionIdUrl = ServiceUrl + "Partner/?sessionId={0}";
        public const string GetValidateBarcodeUrl = ServiceUrl + "validation/?barcode={0}&sessionId={1}";
        public const string GetBrandUrl = ServiceUrl + "brand/?sessionId={0}";
        public const string GetManifestUrl = ServiceUrl + "Manifest/{0}?sessionId={1}";
        public const string GetPartnerTypeUrl = ServiceUrl + "PartnerType/?sessionId={0}";
        public const string GetPartnerSearchUrl = ServiceUrl + "PartnerSearch/?sessionId={0}&search={1}&internalonly={2}&includepublic={3}";
        public const string GetManifestSearchUrl = ServiceUrl + "ManifestSearch/?sessionId={0}&trackingNumber={1}&kegs={2}&senderId={3}&destinationId={4}&referenceKey={5}&fromDate={6}&toDate={7}";
        
        public const string PostManifestUrl = ServiceUrl + "Manifest/?sessionId={0}";
        public const string PostNewPartnerUrl = ServiceUrl + "Partner/?sessionId={0}";


        /// <summary>
        /// Fill Service Url
        /// </summary>
        public const string GetBatchUrl = ServiceUrl + "batch/?sessionId={0}";

        public const string PostBatchUrl = ServiceUrl + "Batch/?sessionId={0}";
        public const string PostPalletUrl = ServiceUrl + "Pallet/?sessionId={0}";


        /// <summary>
        /// Maintainenance Service Url
        /// </summary>
        public const string GetMaintenanceTypeUrl = ServiceUrl + "MaintenanceType/?sessionId={0}";

        public const string PostMaintenanceDoneUrl = ServiceUrl + "MaintenanceDone/?sessionId={0}";

    }
}
