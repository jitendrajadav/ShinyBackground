using KegID.Common;
using Scandit.BarcodePicker.Unified;
using System.Collections.Generic;

namespace KegID.Converter
{
    public class ScanditConvert
    {
        public ScanditConvert()
        {

        }

        // Associates the key (string) for permanent storage with the list of symbologies in ScanSettings
        public static IReadOnlyDictionary<string, Symbology[]> settingToSymbologies = new Dictionary<string, Symbology[]> {
            { "Sym_Ean13Upca", new Symbology[] { Symbology.Ean13, Symbology.Upca } },
            { "Sym_Ean8", new Symbology[] { Symbology.Ean8 } },
            { "Sym_Upce", new Symbology[] { Symbology.Upce } },
            { "Sym_TwoDigitAddOn", new Symbology[] { Symbology.TwoDigitAddOn } },
            { "Sym_FiveDigitAddOn", new Symbology[] { Symbology.FiveDigitAddOn } },
            { "Sym_Code11", new Symbology[] { Symbology.Code11 } },
            { "Sym_Code25", new Symbology[] { Symbology.Code25 } },
            { "Sym_Code32", new Symbology[] { Symbology.Code32 } },
            { "Sym_Code39", new Symbology[] { Symbology.Code39 } },
            { "Sym_Code93", new Symbology[] { Symbology.Code93 } },
            { "Sym_Code128", new Symbology[] { Symbology.Code128 } },
            { "Sym_Interleaved2Of5", new Symbology[] { Symbology.Interleaved2Of5 } },
            { "Sym_MsiPlessey", new Symbology[] { Symbology.MsiPlessey } },
            { "Sym_Gs1Databar", new Symbology[] { Symbology.Gs1Databar } },
            { "Sym_Gs1DatabarExpanded", new Symbology[] { Symbology.Gs1DatabarExpanded } },
            { "Sym_Gs1DatabarLimited", new Symbology[] { Symbology.Gs1DatabarLimited } },
            { "Sym_Codabar", new Symbology[] { Symbology.Codabar } },
            { "Sym_Qr", new Symbology[] { Symbology.Qr } },
            { "Sym_DataMatrix", new Symbology[] { Symbology.DataMatrix } },
            { AppSettings.DpmModeString, null },
            { "Sym_Pdf417", new Symbology[] { Symbology.Pdf417 } },
            { "Sym_MicroPdf417", new Symbology[] { Symbology.MicroPdf417 } },
            { "Sym_Aztec", new Symbology[] { Symbology.Aztec } },
            { "Sym_MaxiCode", new Symbology[] { Symbology.MaxiCode } },
            { "Sym_Rm4scc", new Symbology[] { Symbology.Rm4scc } },
            { "Sym_Kix", new Symbology[] { Symbology.Kix } },
            { "Sym_DotCode", new Symbology[] { Symbology.DotCode } },
            { "Sym_MicroQR", new Symbology[] { Symbology.MicroQr } },
            { "Sym_Lapa4sc", new Symbology[] { Symbology.LAPA4SC } }
        };

        // Associates the key (string) for permanent storage with the text displayed in the GUI
        public static IReadOnlyDictionary<string, string> settingToDisplay = new Dictionary<string, string> {
            { "Sym_Ean13Upca", "EAN-13 & UPC-A" },
            { "Sym_Ean8", "EAN-8" },
            { "Sym_Upce", "UPC-E" },
            { "Sym_TwoDigitAddOn", "Two-Digit-Add-On" },
            { "Sym_FiveDigitAddOn", "Five-Digit-Add-On" },
            { "Sym_Code11", "Code 11" },
            { "Sym_Code25", "Code 25" },
            { "Sym_Code32", "Code 32" },
            { "Sym_Code39", "Code 39" },
            { "Sym_Code93", "Code 93" },
            { "Sym_Code128", "Code 128" },
            { "Sym_Interleaved2Of5", "Interleaved-Two-of-Five (ITF)" },
            { "Sym_MsiPlessey", "MSI Plessey" },
            { "Sym_Gs1Databar", "GS1 DataBar" },
            { "Sym_Gs1DatabarExpanded", "GS1 DataBar Expanded" },
            { "Sym_Gs1DatabarLimited", "GS1 DataBar Limited" },
            { "Sym_Codabar", "Codabar" },
            { "Sym_Qr", "QR Code" },
            { "Inv_Sym_Qr", "Color-inverted QR Code" },
            { "Sym_DataMatrix", "Data Matrix" },
            { "Inv_Sym_DataMatrix", "Color-Inverted Data Matrix" },
            { AppSettings.DpmModeString, "DPM Mode" },
            { "Sym_Pdf417", "PDF417" },
            { "Sym_MicroPdf417", "MicroPDF417" },
            { "Sym_Aztec", "Aztec Code" },
            { "Sym_MaxiCode", "MaxiCode" },
            { "Sym_Rm4scc", "RM4SCC" },
            { "Sym_Kix", "KIX" },
            { "Sym_DotCode", "DotCode" },
            { "Sym_MicroQR", "MicroQr" },
            { "Sym_Lapa4sc", "Lapa4sc" }
        };

        // Associates an index with the storage key (string)
        public static Dictionary<int, string> indexToMsiPlesseyChecksum = new Dictionary<int, string> {
            { 0, AppSettings.MsiPlesseyChecksumString_None },
            { 1, AppSettings.MsiPlesseyChecksumString_Mod10 },
            { 2, AppSettings.MsiPlesseyChecksumString_Mod11 },
            { 3, AppSettings.MsiPlesseyChecksumString_Mod1010 },
            { 4, AppSettings.MsiPlesseyChecksumString_Mod1110 }
        };

        // Associates the storage key (string) with an index
        public static Dictionary<string, int> msiPlesseyChecksumToIndex = new Dictionary<string, int> {
            { AppSettings.MsiPlesseyChecksumString_None, 0 },
            { AppSettings.MsiPlesseyChecksumString_Mod10, 1 },
            { AppSettings.MsiPlesseyChecksumString_Mod11, 2 },
            { AppSettings.MsiPlesseyChecksumString_Mod1010, 3 },
            { AppSettings.MsiPlesseyChecksumString_Mod1110, 4 }
        };

        // Associates an index with the enum in the picker
        public static Dictionary<string, Checksum> msiPlesseyChecksumToScanSetting = new Dictionary<string, Checksum> {
            { AppSettings.MsiPlesseyChecksumString_None, Checksum.None },
            { AppSettings.MsiPlesseyChecksumString_Mod10, Checksum.Mod10 },
            { AppSettings.MsiPlesseyChecksumString_Mod11, Checksum.Mod11 },
            { AppSettings.MsiPlesseyChecksumString_Mod1010, Checksum.Mod1010 },
            { AppSettings.MsiPlesseyChecksumString_Mod1110, Checksum.Mod1110 }
        };

        // Associates the index in the picker with the storage key (string)
        public static Dictionary<int, string> indexToCameraButton = new Dictionary<int, string> {
            { 0, AppSettings.CameraButtonString_Always },
            { 1, AppSettings.CameraButtonString_Never },
            { 2, AppSettings.CameraButtonString_OnlyTablet }
        };

        // Associates the storage key (string) with the index in the picker
        public static Dictionary<string, int> cameraButtonToIndex = new Dictionary<string, int>
        {
            { AppSettings.CameraButtonString_Always, 0 },
            { AppSettings.CameraButtonString_Never, 1 },
            { AppSettings.CameraButtonString_OnlyTablet, 2 }
        };

        // Associates the storage key (string) with ScanSettings enum
        public static Dictionary<string, CameraSwitchVisibility> cameraToScanSetting = new Dictionary<string, CameraSwitchVisibility>
        {
            { AppSettings.CameraButtonString_Always, CameraSwitchVisibility.Always },
            { AppSettings.CameraButtonString_Never, CameraSwitchVisibility.Never },
            { AppSettings.CameraButtonString_OnlyTablet, CameraSwitchVisibility.OnTablets }
        };

        // Associates the index in the picker with the storage key (string)
        public static Dictionary<int, string> indexToGuiStyle = new Dictionary<int, string> {
            { 0, AppSettings.GuiStyleString_Frame },
            { 1, AppSettings.GuiStyleString_Laser },
            { 2, AppSettings.GuiStyleString_None },
            { 3, AppSettings.GuiStyleString_LocationsOnly }
        };

        // Associates the storage key (string) with the index in the picker
        public static Dictionary<string, int> guiStyleToIndex = new Dictionary<string, int>
        {
            { AppSettings.GuiStyleString_Frame, 0 },
            { AppSettings.GuiStyleString_Laser, 1 },
            { AppSettings.GuiStyleString_None, 2 },
            { AppSettings.GuiStyleString_LocationsOnly, 3 }
        };

        // Associates the storage key (string) with ScanSettings enum
        public static Dictionary<string, GuiStyle> guiStyleToScanSetting = new Dictionary<string, GuiStyle>
        {
            { AppSettings.GuiStyleString_Frame, GuiStyle.Default },
            { AppSettings.GuiStyleString_Laser, GuiStyle.Laser },
            { AppSettings.GuiStyleString_None, GuiStyle.None },
            { AppSettings.GuiStyleString_LocationsOnly, GuiStyle.LocationsOnly }
        };

        // Associates the index in the picker with the storage key (string)
        public static Dictionary<int, string> indexToResolution = new Dictionary<int, string> {
            { 0, AppSettings.ResolutionString_HD },
            { 1, AppSettings.ResolutionString_FullHD }
        };

        // Associates the storage key (string) with the index in the picker
        public static Dictionary<string, int> resolutionToIndex = new Dictionary<string, int>
        {
            { AppSettings.ResolutionString_HD, 0 },
            { AppSettings.ResolutionString_FullHD, 1 }
        };

        // Associates the storage key (string) with ScanSettings enum
        public static Dictionary<string, VideoResolution> resolutionToScanSetting = new Dictionary<string, VideoResolution>
        {
            { AppSettings.ResolutionString_HD, VideoResolution.Medium},
            { AppSettings.ResolutionString_FullHD, VideoResolution.High }
        };

        // List of Settings that are enabled by default
        public static readonly string[] EnabledSettings = {
            "Sym_Ean13Upca", "Sym_Ean13Upca", "Sym_Ean8", "Sym_Upce", "Sym_Code39", "Sym_Code128",
            "Sym_Interleaved2Of5", "Sym_Qr", "Sym_DataMatrix", AppSettings.BeepString,
            AppSettings.TorchButtonString
        };
    }
}
