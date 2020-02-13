using KegID.Model;

namespace KegID.Messages
{
    public class MoveScanKegsMessage
    {
        public string Barcode { get; set; }
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
