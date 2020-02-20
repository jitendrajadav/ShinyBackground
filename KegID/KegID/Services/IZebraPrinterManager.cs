using System;

namespace KegID.Services
{
    public interface IZebraPrinterManager
    {
        string PalletHeader { get; set; }
        string TestPrint { get; set; }
        void SendZplPalletAsync(string header, string ipAddr);
    }
}
