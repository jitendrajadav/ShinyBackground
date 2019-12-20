﻿using System;

namespace KegID.Services
{
    public interface IZebraPrinterManager
    {
        String PalletHeader { get; set; }
        String TestPrint { get; set; }
        void SendZplPalletAsync(string header, string ipAddr);
    }
}
