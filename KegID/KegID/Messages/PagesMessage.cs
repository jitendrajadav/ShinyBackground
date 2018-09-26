using KegID.Model;
using LinkOS.Plugin.Abstractions;

namespace KegID.Messages
{
    public class PagesMessage
    {
        public bool AssingValue { get; set; }
        public string Barcode { get; set; }
    }

    //public class SelectPrinterMsg
    //{
    //    public IDiscoveredPrinter IDiscoveredPrinter { get; set; }
    //    public string FriendlyLbl { get; set; }
    //}

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

}
