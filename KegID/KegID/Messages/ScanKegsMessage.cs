using KegID.Model;
using Realms;
using System.Collections.Generic;

namespace KegID.Messages
{
    public class ScanKegsMessage : RealmObject
    {
        public ValidateBarcodeModel Barcodes { get; set; }
    }
    public class FillScanMessage
    {
        public ValidateBarcodeModel Barcodes { get; set; }
    }

    public class MaintainScanMessage
    {
        public ValidateBarcodeModel Barcodes { get; set; }
    }

    public class BulkUpdateScanMessage
    {
        public ValidateBarcodeModel Barcodes { get; set; }
    }

}
