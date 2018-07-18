﻿using KegID.Model;
using LinkOS.Plugin.Abstractions;
using System.Collections.Generic;

namespace KegID.Services
{
    public static class ConstantManager /*: IConstantManager*/
    {
        public static IDiscoveredPrinter PrinterSetting { get; set; }
        public static string IPAddr { get; set; }
        public static bool IsIPAddr { get; set; }
        //public static string SelectedPrinter { get; set; } = "No printers found";
        public static LocationInfo Position { get; set; }
        public static bool IsFromScanned { get; set; }
        public static List<BarcodeModel> VerifiedBarcodes { get; set; }
        public static IList<BarcodeModel> Barcodes { get; set; }
        public static string Barcode { get; set; }
        public static string ManifestId { get; set; }
        public static PartnerModel Partner { get; set; }
        public static List<Tag> Tags { get; set; }
        public static string TagsStr { get; set; }
        public static IList<MaintainTypeReponseModel> MaintainTypeCollection { get; set; }
        public static string Contents { get; set; }
        public static string DBPartnerId { get; set; }
        public static object ContentsCode { get; set; }
    }
}