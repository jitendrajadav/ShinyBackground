using KegID.Model;
using System.Collections.Generic;
using Zebra.Sdk.Printer.Discovery;

namespace KegID.Services
{
    public static class ConstantManager
    {
        public static readonly string StageApiUrl = "https://stageapi.kegid.com/api/";
        public static readonly string TestApiUrl = "https://testapi.kegid.com/api/";
        public static readonly string ProdApiUrl = "https://api.kegid.com/api/";

        public static DiscoveredPrinter PrinterSetting { get; set; }
        public static string IPAddr { get; set; }
        public static LocationInfo Position { get; set; }
        public static bool IsFromScanned { get; set; }
        public static List<BarcodeModel> VerifiedBarcodes { get; set; }
        public static IList<BarcodeModel> Barcodes { get; set; } = new List<BarcodeModel>();
        public static string Barcode { get; set; }
        public static string ManifestId { get; set; }
        public static PartnerModel Partner { get; set; }
        public static List<Tag> Tags { get; set; }
        public static string TagsStr { get; set; }
        public static string Contents { get; set; }
        public static string DBPartnerId { get; set; }
        public static string BaseUrl { get; set; }
    }
}
