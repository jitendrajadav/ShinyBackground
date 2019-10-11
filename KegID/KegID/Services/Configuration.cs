using KegID.Common;

namespace KegID.Services
{
    public class Configuration
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
        public const string Keg = "Keg";
        public const string PostedMaintenanceAlert = "PostedMaintenanceAlert";
        public const string DeleteTypeMaintenanceAlert = "DeleteType";
        public const string KegUploadModel = "KegUploadModel";
        public const string MassUpdateKegList = "MassUpdateKegList";
        public const string DeviceCheckin = "DeviceCheckin";


        public const string StageAPIUrl = "https://stageapi.kegid.com/api/";
        public const string TestAPIUrl = "https://testapi.kegid.com/api/";
        public const string ProdAPIUrl = "https://api.kegid.com/api/";
        public const string DemoAPIUrl = "https://demo.kegid.com/api/";

        ///// <summary>
        ///// Base Url
        ///// </summary>
        //public string BaseURL {get;set;} = ProdAPIUrl;

        /// <summary>
        /// Login Service Url
        /// </summary>
        public const string GetLoginUserUrl =  "login/get?username={0}&password={1}";
        public const string GetUserUrl =  "user/get?username={0}&password={1}";
        public const string DeviceCheckinUrl =  "DeviceCheckin/?sessionId={0}";

        /// <summary>
        /// Dashboard Service Url
        /// </summary>
        public const string GetDashboardUrl = "dashboard/get?sessionId={0}";
        public const string GetInventoryUrl = "Inventory/?sessionId={0}";
        public const string GetKegPossessionByPartnerIdUrl = "KegPossession/?sessionId={0}&partnerid={1}";
        public const string GetPartnerInfoByPartnerIdUrl = "Partner/?sessionId={0}&id={1}";
        public const string GetKegStatusByKegIdUrl = "Keg/{0}?sessionId={1}";
        public const string GetKegSearchByBarcodeUrl = "Keg/?sessionId={0}&barcode={1}&includePartials={2}";

        public const string GetKegMaintenanceHistoryByKegIdUrl = "KegMaintenanceHistory/{0}?sessionId={1}";
        public const string GetMaintenanceAlertByKegIdUrl = "MaintenanceAlert?kegId={0}&sessionId={1}";

        public const string GetDeleteMaintenanceAlertByKegIdUrl = "DeleteMaintenanceAlert?kegId={0}&sessionId={1}";
        public const string GetPalletSearchUrl = "PalletSearch?sessionId={0}&locationId={1}&fromDate={2}&toDate={3}&kegs={4}&kegOwnerId={5}";
        public const string GetPalletUrl = "Pallet/?sessionId={0}&barcode={1}";
        public const string GetAssetSize = "AssetSize/?sessionId={0}&assignableOnly={1}";
        public const string GetAssetType = "AssetType/?sessionId={0}&assignableOnly={1}";
        public const string GetAssetVolume = "AssetVolume/?sessionId={0}&assignableOnly={1}";
        public const string GetOwner = "Owner/?sessionId={0}";

        public const string PostKegsUrl = "Kegs/?sessionId={0}";
        public const string PostKegUrl = "Keg/?sessionId={0}";

        public const string PostMaintenanceAlertUrl = "MaintenanceAlert/?sessionId={0}";
        public const string PostMaintenanceDeleteAlertUrl = "MaintenanceAlert/DeleteAlert/?sessionId={0}";

        /// <summary>
        /// Move Service Url
        /// </summary>
        public const string GetPartnerBySesssionIdUrl = "Partner/?sessionId={0}";
        public const string GetPossessorByownerId = "Possessor?ownerId={0}&sessionId={1}";

        public const string GetValidateBarcodeUrl = "validation/?barcode={0}&sessionId={1}";
        public const string GetBrandUrl = "brand/?sessionId={0}";
        public const string GetManifestUrl = "Manifest/{0}?sessionId={1}";
        public const string GetPartnerTypeUrl = "PartnerType/?sessionId={0}";
        public const string GetPartnerSearchUrl = "PartnerSearch/?sessionId={0}&search={1}&internalonly={2}&includepublic={3}";
        public const string GetManifestSearchUrl = "ManifestSearch/?sessionId={0}&trackingNumber={1}&kegs={2}&senderId={3}&destinationId={4}&referenceKey={5}&fromDate={6}&toDate={7}";

        public const string PostManifestUrl = "Manifest/?sessionId={0}";
        public const string PostNewPartnerUrl = "Partner/?sessionId={0}";


        /// <summary>
        /// Fill Service Url
        /// </summary>
        public const string GetBatchUrl = "batch/?sessionId={0}";

        public const string PostBatchUrl = "Batch/?sessionId={0}";
        public const string PostPalletUrl = "Pallet/?sessionId={0}";


        /// <summary>
        /// Maintainenance Service Url
        /// </summary>
        public const string GetMaintenanceTypeUrl = "MaintenanceType/?sessionId={0}";
        public const string PostMaintenanceDoneUrl = "MaintenanceDone/?sessionId={0}";
    }
}
