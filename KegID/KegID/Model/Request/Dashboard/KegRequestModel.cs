using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public partial class KegRequestModel
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public string OwnerId { get; set; }
        public string AltBarcode { get; set; }
        public string Notes { get; set; }
        public string ReferenceKey { get; set; }
        public string ProfileId { get; set; }
        public string AssetType { get; set; }
        public string AssetSize { get; set; }
        public string AssetVolume { get; set; }
        public string AssetDescription { get; set; }
        public string Grai { get; set; }
        public string OwnerSkuId { get; set; }
        public string FixedContents { get; set; }
        public List<Tag> Tags { get; set; }
        public List<string> MaintenanceAlertIds { get; set; }
        public string LessorId { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public long PurchasePrice { get; set; }
        public string PurchaseOrder { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufactureLocation { get; set; }
        public DateTimeOffset ManufactureDate { get; set; }
        public string Material { get; set; }
        public string Markings { get; set; }
        public string Colors { get; set; }
    }
}
