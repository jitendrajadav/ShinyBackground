using System.Collections.Generic;

namespace KegID.Model
{
    public class KegBulkUpdateItemRequestModel 
    {
        public List<MassUpdateKeg> Kegs { get; set; }
    }
    public class MassUpdateKeg
    {
        public string KegId { get; set; }
        public string OwnerId { get; set; }
        public string Barcode { get; set; }
        public string ProfileId { get; set; }
        public string AssetSize { get; set; }
        public string AssetType { get; set; }
        public string AssetVolume { get; set; }
        public string OwnerSkuId { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class KegBulkUpdateItemKeg
    {
        public string OwnerName { get; set; }
        public string Barcode { get; set; }
        public string AssetProfile { get; set; }
        public string AssetType { get; set; }
        public string AssetSize { get; set; }
        public string Volume { get; set; }
        public string Measure { get; set; }
        public string FixedContents { get; set; }
        public string Material { get; set; }
        public string Coupling { get; set; }
        public string Manufacturer { get; set; }
        public string ManufactureDate { get; set; }
        public string ManufactureLocation { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchasePrice { get; set; }
        public string PurchaseOrder { get; set; }
        public string Colors { get; set; }
        public string Markings { get; set; }
        public string LeasingCompany { get; set; }
        public string Location { get; set; }
        public string LocationDate { get; set; }
        public string SkuCode { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
