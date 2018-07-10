using KegID.Model;
using LinkOS.Plugin.Abstractions;
using System.Collections.Generic;

namespace KegID.Services
{
    public interface IConstantManager
    {
        IDiscoveredPrinter PrinterSetting { get; set; }
        string IPAddr { get; set; }
        bool IsIPAddr { get; set; }
        string SelectedPrinter { get; set; }
        LocationInfo Position { get; set; }
        bool IsFromScanned { get; set; }
        List<BarcodeModel> VerifiedBarcodes { get; set; }
        IList<BarcodeModel> Barcodes { get; set; }
        string Barcode { get; set; }
        string ManifestId { get; set; }
        PartnerModel Partner { get; set; }
        List<Tag> Tags { get; set; }
        string TagsStr { get; set; }
        IList<MaintainTypeReponseModel> MaintainTypeCollection { get; set; }
        string Contents { get; set; }
        string DBPartnerId { get; set; }
        object ContentsCode { get; set; }
    }
}
