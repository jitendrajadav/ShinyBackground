using KegID.Model;
using LinkOS.Plugin.Abstractions;

namespace KegID.Messages
{
    public class PagesMessage
    {
        public bool AssingValue { get; set; }
    }

    public class SelectPrinterMsg
    {
        public IDiscoveredPrinter IDiscoveredPrinter { get; set; }
        public string friendlyLbl { get; set; }
    }

    public class PalletToScanKegPagesMsg
    {
        public bool BarcodeScan { get; set; }
    }

    public class FillToFillScanPagesMsg
    {
        public BatchModel BatchModel { get; set; }
        public string SizeButtonTitle { get; set; }
        public PartnerModel PartnerModel { get; set; }
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
}
