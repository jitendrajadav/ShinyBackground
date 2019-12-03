namespace KegID.Services
{
    public static class Configuration
    {
        public static readonly string StageApiUrl = "https://stageapi.kegid.com/api/";
        public static readonly string TestApiUrl = "https://testapi.kegid.com/api/";
        public static readonly string ProdApiUrl = "https://api.kegid.com/api/";

        //public static string ApiHostName
        //{
        //    get
        //    {
        //        var apiHostName = Regex.Replace(TestApiUrl, @" ^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase)
        //                           .Replace("/", string.Empty);
        //        return apiHostName;
        //    }
        //}

        ///// <summary>
        ///// Constant flag
        ///// </summary>
        //public static readonly string NewManifest = "NewManifest";
        //public static readonly string NewPartner = "NewPartner";
        //public static readonly string NewBatch = "Batch";
        //public static readonly string NewPallet = "NewPallet";
        //public static readonly string PostedMaintenanceDone = "PostedMaintenanceDone";
        //public static readonly string NewKeg = "NewKeg";
        //public static readonly string Keg = "Keg";
        //public static readonly string PostedMaintenanceAlert = "PostedMaintenanceAlert";
        //public static readonly string DeleteTypeMaintenanceAlert = "DeleteType";
        //public static readonly string KegUploadModel = "KegUploadModel";
        //public static readonly string MassUpdateKegList = "MassUpdateKegList";
        //public static readonly string DeviceCheckin = "DeviceCheckin";

        ///// <summary>
        ///// Login Service Url
        ///// </summary>
        //public static readonly string GetLoginUserUrl =  "login/get?username={0}&password={1}";
        //public static readonly string GetUserUrl =  "user/get?username={0}&password={1}";
        //public static readonly string DeviceCheckinUrl =  "DeviceCheckin/?sessionId={0}";

        ///// <summary>
        ///// Dashboard Service Url
        ///// </summary>
        //public static readonly string GetDashboardUrl = "dashboard/get?sessionId={0}";
        //public static readonly string GetInventoryUrl = "Inventory/?sessionId={0}";
        //public static readonly string GetKegPossessionByPartnerIdUrl = "KegPossession/?sessionId={0}&partnerid={1}";
        //public static readonly string GetPartnerInfoByPartnerIdUrl = "Partner/?sessionId={0}&id={1}";
        //public static readonly string GetKegStatusByKegIdUrl = "Keg/{0}?sessionId={1}";
        //public static readonly string GetKegSearchByBarcodeUrl = "Keg/?sessionId={0}&barcode={1}&includePartials={2}";

        //public static readonly string GetKegMaintenanceHistoryByKegIdUrl = "KegMaintenanceHistory/{0}?sessionId={1}";
        //public static readonly string GetMaintenanceAlertByKegIdUrl = "MaintenanceAlert?kegId={0}&sessionId={1}";

        //public static readonly string GetDeleteMaintenanceAlertByKegIdUrl = "DeleteMaintenanceAlert?kegId={0}&sessionId={1}";
        //public static readonly string GetPalletSearchUrl = "PalletSearch?sessionId={0}&locationId={1}&fromDate={2}&toDate={3}&kegs={4}&kegOwnerId={5}";
        //public static readonly string GetPalletUrl = "Pallet/?sessionId={0}&barcode={1}";
        //public static readonly string GetAssetSize = "AssetSize/?sessionId={0}&assignableOnly={1}";
        //public static readonly string GetAssetType = "AssetType/?sessionId={0}&assignableOnly={1}";
        //public static readonly string GetAssetVolume = "AssetVolume/?sessionId={0}&assignableOnly={1}";
        //public static readonly string GetOwner = "Owner/?sessionId={0}";

        //public static readonly string PostKegsUrl = "Kegs/?sessionId={0}";
        //public static readonly string PostKegUrl = "Keg/?sessionId={0}";

        //public static readonly string PostMaintenanceAlertUrl = "MaintenanceAlert/?sessionId={0}";
        //public static readonly string PostMaintenanceDeleteAlertUrl = "MaintenanceAlert/DeleteAlert/?sessionId={0}";
        //public static readonly string GetOperatorsUrl = "Operators/?sessionId={0}";

        ///// <summary>
        ///// Move Service Url
        ///// </summary>
        //public static readonly string GetPartnerBySesssionIdUrl = "Partner/?sessionId={0}";
        //public static readonly string GetPossessorByownerId = "Possessor?ownerId={0}&sessionId={1}";

        //public static readonly string GetValidateBarcodeUrl = "validation/?barcode={0}&sessionId={1}";
        //public static readonly string GetBrandUrl = "brand/?sessionId={0}";
        //public static readonly string GetManifestUrl = "Manifest/{0}?sessionId={1}";
        //public static readonly string GetPartnerTypeUrl = "PartnerType/?sessionId={0}";
        //public static readonly string GetPartnerSearchUrl = "PartnerSearch/?sessionId={0}&search={1}&internalonly={2}&includepublic={3}";
        //public static readonly string GetManifestSearchUrl = "ManifestSearch/?sessionId={0}&trackingNumber={1}&kegs={2}&senderId={3}&destinationId={4}&referenceKey={5}&fromDate={6}&toDate={7}";

        //public static readonly string PostManifestUrl = "Manifest/?sessionId={0}";
        //public static readonly string PostNewPartnerUrl = "Partner/?sessionId={0}";


        ///// <summary>
        ///// Fill Service Url
        ///// </summary>
        //public static readonly string GetBatchUrl = "batch/?sessionId={0}";

        //public static readonly string PostBatchUrl = "Batch/?sessionId={0}";
        //public static readonly string PostPalletUrl = "Pallet/?sessionId={0}";
        //public static readonly string GetSkuUrl = "sku/?sessionId={0}";

        ///// <summary>
        ///// Maintainenance Service Url
        ///// </summary>
        //public static readonly string GetMaintenanceTypeUrl = "MaintenanceType/?sessionId={0}";
        //public static readonly string PostMaintenanceDoneUrl = "MaintenanceDone/?sessionId={0}";
    }
}
