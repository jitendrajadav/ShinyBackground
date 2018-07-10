using LinkOS.Plugin.Abstractions;
using System;

namespace KegID.Services
{
    public interface IZebraPrinterManager
    {
        IDiscoveredPrinter MyPrinter { get; set; }
        String PalletHeader { get; set; }
        String TestPrint { get; set; }
        void SendZplPallet(string header, bool IsIpAddr, string ipAddr);
        bool CheckPrinterLanguage(IConnection connection);
        bool PreCheckPrinterStatus(IZebraPrinter printer);
    }
}
