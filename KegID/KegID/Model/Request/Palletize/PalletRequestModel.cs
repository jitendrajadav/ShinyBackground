using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletRequestModel
    {
        public string PalletId { get; set; }
        public string OwnerId { get; set; }
        public string Barcode { get; set; }
        public DateTimeOffset BuildDate { get; set; }
        public string StockLocation { get; set; }
        public string StockLocationId { get; set; }
        public string StockLocationName { get; set; }
        public string ReferenceKey { get; set; }
        public List<PalletItem> PalletItems { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
