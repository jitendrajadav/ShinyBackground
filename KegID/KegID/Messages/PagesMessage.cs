using KegID.Model;
using System.Collections.Generic;

namespace KegID.Messages
{
    public class PagesMessage
    {
        public bool AssingValue { get; set; }
    }

    public class ValidToScanKegPagesMsg
    {
        public Partner Partner { get; set; }
    }
    //public class ValidToFillScanPagesMsg
    //{
    //    public Partner Partner { get; set; }
    //}
    //public class ValidToMaintainPagesMsg
    //{
    //    public Partner Partner { get; set; }
    //}
    //public class ScanKegToMovePagesMsg
    //{
    //    public List<BarcodeModel> Barcodes { get; set; }
    //    public List<Tag> Tags { get; set; }
    //    public string Contents { get; set; }
    //}
    //public class ScanKegToPalletPagesMsg
    //{
    //    public List<BarcodeModel> Barcodes { get; set; }
    //}
    public class PalletToScanKegPagesMsg
    {
        public bool BarcodeScan { get; set; }
    }

    //public class PalletToContentTagsPagesMsg
    //{
    //    public List<string> Barcode { get; set; }
    //}

    public class FillToFillScanPagesMsg
    {
        public BatchModel BatchModel { get; set; }
        public string SizeButtonTitle { get; set; }
        public PartnerModel PartnerModel { get; set; }
    }

    //public class FillScanToAddPalletPagesMsg
    //{
    //    public IList<BarcodeModel> BarcodeModels { get; set; }
    //    public string BatchId { get; set; }
    //}

    public class AddPalletToFillScanMsg
    {
        public bool CleanUp { get; set; }
    }
    public class BarcodeScannerToKegSearchMsg
    {
        public Barcode Barcodes { get; set; }
    }

    //public class FillScanToAddPalletSubmitMsg
    //{
    //    public bool IsSubmit { get; set; }
    //}

    public class SettingToDashboardMsg
    {
        public bool IsRefresh { get; set; }
    }
}
