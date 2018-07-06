using KegID.LocalDb;
using KegID.Model;
using LinkOS.Plugin.Abstractions;
using Microsoft.AppCenter.Crashes;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KegID.Common
{
    public static class ConstantManager
    {
        public static IDiscoveredPrinter PrinterSetting;
        internal static string IPAddr;
        internal static bool IsIPAddr;
        public static string SelectedPrinter { get; internal set; } = "No printers found";

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
        public static string Contents { get; internal set; }
        public static string DBPartnerId { get; internal set; }
        public static object ContentsCode { get; internal set; }

        internal static IList<ManifestModel> CheckDraftmaniFestsAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                return RealmDb.All<ManifestModel>().Where(x => x.IsDraft == true).ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
        }
    }
}
