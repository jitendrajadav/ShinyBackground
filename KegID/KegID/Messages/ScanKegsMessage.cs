using KegID.Model;

namespace KegID.Messages
{
    public class ScanKegsMessage 
    {
        public BarcodeModel Barcodes { get; set; }
    }
    public class FillScanMessage
    {
        public BarcodeModel Barcodes { get; set; }
    }

    public class MaintainScanMessage
    {
        public BarcodeModel Barcodes { get; set; }
    }

    public class BulkUpdateScanMessage
    {
        public BarcodeModel Barcodes { get; set; }
    }

}
