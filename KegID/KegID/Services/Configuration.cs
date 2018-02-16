namespace KegID.Services
{
    public static class Configuration
    {
        /// <summary>
        /// Global SessionId
        /// </summary>
        public static string SessionId;
        public static string CompanyId;
        public const string NewManifest = "NewManifest";
        public const string NewPartner = "NewPartner";
        public const string NewBatch = "Batch";
        public const string NewPallet = "NewPallet";
        
        /// <summary>
        /// Base Url
        /// </summary>
        public const string ServiceUrl = "https://testapi.kegid.com/api/";

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

        /// <summary>
        /// Move Service Url
        /// </summary>
        public const string GetPartnerUrl = ServiceUrl + "Partner/?sessionId={0}";
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
        

    }
}
