using KegID.Model;
using System.Collections.Generic;

namespace KegID.Messages
{
    public class ScanKegsMessage
    {
        public Barcode Barcodes { get; set; }
    }
    public class FillScanMessage
    {
        public Barcode Barcodes { get; set; }
    }

    public class MaintainScanMessage
    {
        public Barcode Barcodes { get; set; }
    }

    public class BulkUpdateScanMessage
    {
        public Barcode Barcodes { get; set; }
    }

}
