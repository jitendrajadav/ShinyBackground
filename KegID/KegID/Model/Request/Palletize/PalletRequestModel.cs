using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletRequestModel
    {
        public List<PalletItem> PalletItems { get; set; }
        public string PalletId { get; set; }
        public string OwnerId { get; set; }
        public DateTime BuildDate { get; set; }
        public string Barcode { get; set; }
        public string BarcodeFormat { get; set; }
        public long ManifestTypeId { get; set; }
        public string StockLocation { get; set; }
        public string TargetLocation { get; set; }
        public string ReferenceKey { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class PalletItem
    {
        public string Barcode { get; set; }
        public DateTime ScanDate { get; set; }
        public List<Tag> Tags { get; set; }
        public string Contents { get; set; }
        public string KegId { get; set; }
        public string PalletId { get; set; }
        public string HeldOnPalletId { get; set; }
        public string SkuId { get; set; }
        public string BatchId { get; set; }
    }
}
