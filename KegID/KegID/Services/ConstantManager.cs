using KegID.Model;
using LinkOS.Plugin.Abstractions;
using System.Collections.Generic;

namespace KegID.Services
{
    public static class ConstantManager 
    {
        //public static Xamarin.Essentials.Location Location { get; set; }
        public static IDiscoveredPrinter PrinterSetting { get; set; }
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
        public static IList<MaintainTypeReponseModel> MaintainTypeCollection { get; set; }
        public static string Contents { get; set; }
        public static string DBPartnerId { get; set; }
        public static object ContentsCode { get; set; }
    }
}
