using System;

namespace KegID.Services
{
    public static class Configuration
    {
        /// <summary>
        /// Global SessionId
        /// </summary>
        public static string SessionId;

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


        /// <summary>
        /// Move Service Url
        /// </summary>
        public const string GetPartnerUrl = ServiceUrl + "Partner/?sessionId={0}";
        public const string GetValidateBarcodeUrl = ServiceUrl + "validation/?barcode={0}&sessionId={1}";
        public const string GetBrandUrl = ServiceUrl + "brand/?sessionId={0}";
        public const string GetManifestUrl = ServiceUrl + "Manifest/?sessionId={0}";

        public const string PostManifestUrl = ServiceUrl + "Manifest/?sessionId={0}";


        /// <summary>
        /// Fill Service Url
        /// </summary>
        public const string GetBatchUrl = ServiceUrl + "batch/?sessionId={0}";
    }
}
