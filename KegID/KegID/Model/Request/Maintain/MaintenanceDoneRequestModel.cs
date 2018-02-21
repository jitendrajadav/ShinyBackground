using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class MaintenanceDoneRequestModel : KegIDResponse
    {
        public string MaintenancePostingId { get; set; }
        public List<long> ActionsPerformed { get; set; }
        public DateTimeOffset DatePerformed { get; set; }
        public string Operator { get; set; }
        public string LocationId { get; set; }
        public List<MaintainKeg> Kegs { get; set; }
        public List<MaintenanceDoneRequestModelTag> Tags { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public string SourceKey { get; set; }
    }

    public class MaintainKeg
    {
        public string Message { get; set; }
        public string Barcode { get; set; }
        public DateTimeOffset ScanDate { get; set; }
        public List<KegTag> Tags { get; set; }
        public string Contents { get; set; }
        public string KegId { get; set; }
        public string PalletId { get; set; }
        public string HeldOnPalletId { get; set; }
        public string SkuId { get; set; }
        public string BatchId { get; set; }
    }

    public class KegTag
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }

    public class MaintenanceDoneRequestModelTag
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
