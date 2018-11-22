using KegID.Model;

namespace KegID.Messages
{
    public class PagesMessage
    {
        public bool AssingValue { get; set; }
        public string Barcode { get; set; }
    }

    public class PalletToScanKegPagesMsg
    {
        public bool BarcodeScan { get; set; }
    }

    public class MaintainDTToMaintMsg
    {
        public bool CleanUp { get; set; }
    }

    public class AddPalletToFillScanMsg
    {
        public bool CleanUp { get; set; }
    }

    public class BarcodeScannerToKegSearchMsg
    {
        public Barcode Barcodes { get; set; }
    }

    public class SettingToDashboardMsg
    {
        public bool IsRefresh { get; set; }
    }

    public class WhatsNewViewToModel
    {
        public bool IsBack { get; set; }
    }

    public class InvalidServiceCall
    {
        public bool IsInvalidCall { get; set; }
    }

    public class CheckDraftmaniFests
    {
        public bool IsCheckDraft { get; set; }
    }

    public class ScannerToPalletAssign
    {
        public string Barcode { get; set; }
    }
}
