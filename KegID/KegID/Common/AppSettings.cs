using KegID.Converter;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using System;

namespace KegID.Common
{
    public static class Settings
    {
        // Endpoints
        private const string DefaultBookingEndpoint = "YOUR_BOOKING_ENDPOINT";
        private const string DefaultHotelsEndpoint = "YOUR_HOTELS_ENDPOINT";
        private const string DefaultSuggestionsEndpoint = "YOUR_SUGGESTIONS_ENDPOINT";
        private const string DefaultNotificationsEndpoint = "YOUR_NOTIFICATIONS_ENDPOINT";
        private const string DefaultSettingsFileUrl = "http://sh360services-public.eastus2.cloudapp.azure.com/configuration-api/cfg/public-http";
        private const string DefaultImagesBaseUri = "YOUR_IMAGE_BASE_URI";

        // Mobile Center
        private const string DefaultMobileCenterAnalyticsAndroid = "YOUR_APPCENTER_ANDROID_ANALYTICS_ID";


        private const string DefaultMobileCenterAnalyticsIos = "YOUR_APPCENTER_IOS_ANALYTICS_ID";
        private const string DefaultMobileCenterAnalyticsWindows = "YOUR_APPCENTER_WINDOWS_ANALYTICS_ID";

        // Maps
        private const string DefaultBingMapsApiKey = "Ao8rhjv5SFQSx1ghwMhoW-OboAS7PyENTdGpVjoH8AUM5Vix_3q5PiuXCHDD4Nfu";
        public const string DefaultFallbackMapsLocation = "19.125896 -72.0235896";

        // Bots
        private const string DefaultSkypeBotId = "YOUR_SKYPE_BOT_ID";
        private const string DefaultFacebookBotId = "YOUR_FACEBOOK_BOT_ID";

        // B2c
        public const string B2cAuthority = "https://login.microsoftonline.com/";
        public const string DefaultB2cPolicy = "B2C_1_SignUpInPolicy";
        public const string DefaultB2cClientId = "YOUR_B2C_CLIENT_ID";
        public const string DefaultB2cTenant = "YOUR_B2C_TENANT";


        // Booking
        private const bool DefaultHasBooking = false;

        // Fakes
        private const bool DefaultUseFakes = false;

        private static ISettings AppSettings => CrossSettings.Current;

        #region Scandit Settings

        private static readonly IBarcodePicker picker = ScanditService.BarcodePicker;
        private static readonly ScanSettings scanSettings = picker.GetDefaultScanSettings();

        // DPM Mode
        public const string DpmModeString = "Sym_DPM_Mode";
        public const string DataMatrixString = "Sym_DataMatrix";

        // Checksum
        public const string MsiPlesseyChecksumString = "MsiPlesseyChecksum";
        public const string MsiPlesseyChecksumString_None = "None";
        public const string MsiPlesseyChecksumString_Mod10 = "Mod 10";
        public const string MsiPlesseyChecksumString_Mod11 = "Mod 11";
        public const string MsiPlesseyChecksumString_Mod1010 = "Mod 1010";
        public const string MsiPlesseyChecksumString_Mod1110 = "Mod 1110";

        // Feedback
        public const string BeepString = "Overlay_BeepEnabled";
        public const string VibrateString = "Overlay_VibrateEnabled";

        // Torch button
        public const string TorchButtonString = "Overlay_TorchButtonVisible";
        public const string TorchButtonXString = "Overlay_TorchButtonX"; // Unused as not supported yet.
        public const string TorchButtonYString = "Overlay_TorchButtonY"; // Unused as not supported yet.

        // Camera button
        public const string CameraButtonString = "Overlay_CameraButton";
        public const string CameraButtonString_Always = "Overlay_CameraButton_Always";
        public const string CameraButtonString_Never = "Overlay_CameraButton_Never";
        public const string CameraButtonString_OnlyTablet = "Overlay_CameraButton_OnlyTablets";
        public const string CameraButtonXString = "Overlay_CameraButtonX"; // Unused as not supported yet.
        public const string CameraButtonYString = "Overlay_CameraButtonY"; // Unused as not supported yet.

        // Hotspot
        public const string RestrictedAreaString = "ScanSettings_RestrictedAreaScanningEnabled";
        public const string HotSpotHeightString = "ScanOverlay_HotSpotHeight";
        public const string HotSpotWidthString = "ScanOverlay_HotSpotWidth";
        public const string HotSpotYString = "ScanOverlay_HotSpotY";

        // ViewFinder
        public const string ViewFinderPortraitWidthString = "Overlay_ViewFinderSizePortrait_Width";
        public const string ViewFinderPortraitHeightString = "Overlay_ViewFinderSizePortrait_Height";
        public const string ViewFinderLandscapeWidthString = "Overlay_ViewFinderSizeLandscape_Width";
        public const string ViewFinderLandscapeHeightString = "Overlay_ViewFinderSizeLandscape_Height";

        // Camera
        public const string ResolutionString = "ScanSettings_Resolution";
        public const string ResolutionString_HD = "ScanSettings_Resolution_HD";
        public const string ResolutionString_FullHD = "ScanSettings_Resolution_FullHD";

        public static readonly string[] SliderStrings = {
            ViewFinderPortraitWidthString,
            ViewFinderPortraitHeightString,
            ViewFinderLandscapeWidthString,
            ViewFinderLandscapeHeightString
        };

        public const string GuiStyleString = "Overlay_GuiStyle";
        public const string GuiStyleString_Frame = "Overlay_GuiStyle_Frame";
        public const string GuiStyleString_Laser = "Overlay_GuiStyle_Laser";
        public const string GuiStyleString_None = "Overlay_GuiStyle_None";
        public const string GuiStyleString_LocationsOnly = "Overlay_GuiStyle_LocationsOnly";

        public static bool IsDpmMode(string symbology)
        {
            return symbology == DpmModeString;
        }

        public static bool IsDataMatrix(string symbology)
        {
            return symbology == DataMatrixString;
        }

        public static bool HasInvertedSymbology(string symbology)
        {
            return (symbology == "Sym_Qr" || symbology == "Sym_DataMatrix");
        }

        public static string GetInvertedSymbology(string symbology)
        {
            if (HasInvertedSymbology(symbology))
            {
                return ("Inv_" + symbology);
            }

            throw new Exception("has no inversion");
        }

        public static bool GetBoolSetting(string setting)
        {
            return AppSettings.GetValueOrDefault(setting, DefaultBool(setting));
        }

        public static void SetBoolSetting(string setting, bool value)
        {
            AppSettings.AddOrUpdateValue(setting, value);
        }

        private static bool DefaultBool(string setting)
        {
            if (Array.IndexOf(ScanditConvert.EnabledSettings, setting) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetIntSetting(string setting)
        {
            return AppSettings.GetValueOrDefault(setting, DefaultInt(setting));
        }

        public static void SetIntSetting(string setting, int value)
        {
            AppSettings.AddOrUpdateValue(setting, value);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private static int DefaultInt(string setting)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            return 15;
        }

        public static Double GetDoubleSetting(string setting)
        {
            return AppSettings.GetValueOrDefault(setting, DefaultDouble(setting));
        }

        public static void SetDoubleSetting(string setting, Double value)
        {
            AppSettings.AddOrUpdateValue(setting, value);
        }

        private static Double DefaultDouble(string setting)
        {
            switch (setting)
            {
                case HotSpotHeightString:
                    return 0.25;
                case HotSpotWidthString:
                    return 1.0;
                case HotSpotYString:
                    return scanSettings.ScanningHotSpot.Y;

                case ViewFinderPortraitWidthString:
                    return picker.ScanOverlay.ViewFinderSizePortrait.Width;
                case ViewFinderPortraitHeightString:
                    return picker.ScanOverlay.ViewFinderSizePortrait.Height;
                case ViewFinderLandscapeWidthString:
                    return picker.ScanOverlay.ViewFinderSizeLandscape.Width;
                case ViewFinderLandscapeHeightString:
                    return picker.ScanOverlay.ViewFinderSizeLandscape.Height;

                default:
                    throw (new Exception("No such Double setting: " + setting));
            }
        }

        public static string GetStringSetting(string setting)
        {
            return AppSettings.GetValueOrDefault(setting, DefaultString(setting));
        }

        public static void SetStringSetting(string setting, string value)
        {
            AppSettings.AddOrUpdateValue(setting, value);
        }

        private static string DefaultString(string setting)
        {
            switch (setting)
            {
                case MsiPlesseyChecksumString:
                    return MsiPlesseyChecksumString_Mod10;
                case CameraButtonString:
                    return CameraButtonString_Always;
                case GuiStyleString:
                    return GuiStyleString_Frame;
                case ResolutionString:
                    return ResolutionString_HD;
                default:
                    throw new Exception("No default setting for " + setting);
            }
        }


        #endregion

        // Azure B2C settings
        public static string B2cClientId
        {
            get => AppSettings.GetValueOrDefault(nameof(B2cClientId), DefaultB2cClientId);
            set => AppSettings.AddOrUpdateValue(nameof(B2cClientId), value);
        }

        public static string B2cTenant
        {
            get => AppSettings.GetValueOrDefault(nameof(B2cTenant), DefaultB2cTenant);
            set => AppSettings.AddOrUpdateValue(nameof(B2cTenant), value);
        }

        public static string B2cPolicy
        {
            get => AppSettings.GetValueOrDefault(nameof(B2cPolicy), DefaultB2cPolicy);
            set => AppSettings.AddOrUpdateValue(nameof(B2cPolicy), value);
        }

        // API Endpoints
        public static string BookingEndpoint
        {
            get => AppSettings.GetValueOrDefault(nameof(BookingEndpoint), DefaultBookingEndpoint);
            set => AppSettings.AddOrUpdateValue(nameof(BookingEndpoint), value);
        }

        public static string HotelsEndpoint
        {
            get => AppSettings.GetValueOrDefault(nameof(HotelsEndpoint), DefaultHotelsEndpoint);
            set => AppSettings.AddOrUpdateValue(nameof(HotelsEndpoint), value);
        }

        public static string SuggestionsEndpoint
        {
            get => AppSettings.GetValueOrDefault(nameof(SuggestionsEndpoint), DefaultSuggestionsEndpoint);
            set => AppSettings.AddOrUpdateValue(nameof(SuggestionsEndpoint), value);
        }

        public static string NotificationsEndpoint
        {
            get => AppSettings.GetValueOrDefault(nameof(NotificationsEndpoint), DefaultNotificationsEndpoint);
            set => AppSettings.AddOrUpdateValue(nameof(NotificationsEndpoint), value);
        }

        public static string ImagesBaseUri
        {
            get => AppSettings.GetValueOrDefault(nameof(ImagesBaseUri), DefaultImagesBaseUri);
            set => AppSettings.AddOrUpdateValue(nameof(ImagesBaseUri), value);
        }

        public static string SkypeBotId
        {
            get => AppSettings.GetValueOrDefault(nameof(SkypeBotId), DefaultSkypeBotId);
            set => AppSettings.AddOrUpdateValue(nameof(SkypeBotId), value);
        }

        public static string FacebookBotId
        {
            get => AppSettings.GetValueOrDefault(nameof(FacebookBotId), DefaultFacebookBotId);
            set => AppSettings.AddOrUpdateValue(nameof(FacebookBotId), value);
        }

        // Other AppSettings
        public static string BingMapsApiKey
        {
            get => AppSettings.GetValueOrDefault(nameof(BingMapsApiKey), DefaultBingMapsApiKey);
            set => AppSettings.AddOrUpdateValue(nameof(BingMapsApiKey), value);
        }

        public static string SettingsFileUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(SettingsFileUrl), DefaultSettingsFileUrl);
            set => AppSettings.AddOrUpdateValue(nameof(SettingsFileUrl), value);
        }

        public static string FallbackMapsLocation
        {
            get => AppSettings.GetValueOrDefault(nameof(FallbackMapsLocation), DefaultFallbackMapsLocation);
            set => AppSettings.AddOrUpdateValue(nameof(FallbackMapsLocation), value);
        }

        public static string SessionId
        {
            get => AppSettings.GetValueOrDefault(nameof(SessionId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(SessionId), value);
        }

        public static string CompanyId
        {
            get => AppSettings.GetValueOrDefault(nameof(CompanyId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(CompanyId), value);
        }

        public static string MasterCompanyId
        {
            get => AppSettings.GetValueOrDefault(nameof(MasterCompanyId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(MasterCompanyId), value);
        }

        public static string UserId
        {
            get => AppSettings.GetValueOrDefault(nameof(UserId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserId), value);
        }

        public static string SessionExpires
        {
            get => AppSettings.GetValueOrDefault(nameof(SessionExpires), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(SessionExpires), value);
        }

        public static long Overdue_days
        {
            get => AppSettings.GetValueOrDefault(nameof(Overdue_days), 0);
            set => AppSettings.AddOrUpdateValue(nameof(Overdue_days), value);
        }

        public static long At_risk_days
        {
            get => AppSettings.GetValueOrDefault(nameof(At_risk_days), 0);
            set => AppSettings.AddOrUpdateValue(nameof(At_risk_days), value);
        }

        public static string MobileCenterAnalyticsAndroid
        {
            get => AppSettings.GetValueOrDefault(nameof(MobileCenterAnalyticsAndroid), DefaultMobileCenterAnalyticsAndroid);
            set => AppSettings.AddOrUpdateValue(nameof(MobileCenterAnalyticsAndroid), value);
        }

        public static string MobileCenterAnalyticsIos
        {
            get => AppSettings.GetValueOrDefault(nameof(MobileCenterAnalyticsIos), DefaultMobileCenterAnalyticsIos);
            set => AppSettings.AddOrUpdateValue(nameof(MobileCenterAnalyticsIos), value);
        }

        public static string MobileCenterAnalyticsWindows
        {
            get => AppSettings.GetValueOrDefault(nameof(MobileCenterAnalyticsWindows), DefaultMobileCenterAnalyticsWindows);
            set => AppSettings.AddOrUpdateValue(nameof(MobileCenterAnalyticsWindows), value);
        }

        public static bool UseFakes
        {
            get => AppSettings.GetValueOrDefault(nameof(UseFakes), DefaultUseFakes);
            set => AppSettings.AddOrUpdateValue(nameof(UseFakes), value);
        }

        public static bool HasBooking
        {
            get => AppSettings.GetValueOrDefault(nameof(HasBooking), DefaultHasBooking);

            set => AppSettings.AddOrUpdateValue(nameof(HasBooking), value);
        }

        public static bool IsMetaDataLoaded
        {
            get => AppSettings.GetValueOrDefault(nameof(IsMetaDataLoaded), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsMetaDataLoaded), value);
        }

        public static bool PrintEveryManifest
        {
            get => AppSettings.GetValueOrDefault(nameof(PrintEveryManifest), false);
            set => AppSettings.AddOrUpdateValue(nameof(PrintEveryManifest), value);
        }

        public static bool PrintEveryPallet
        {
            get => AppSettings.GetValueOrDefault(nameof(PrintEveryPallet), false);
            set => AppSettings.AddOrUpdateValue(nameof(PrintEveryPallet), value);
        }

        public static int PalletLabelCopies
        {
            get => AppSettings.GetValueOrDefault(nameof(PalletLabelCopies), 1);
            set => AppSettings.AddOrUpdateValue(nameof(PalletLabelCopies), value);
        }

        public static bool BeepOnValidScans
        {
            get => AppSettings.GetValueOrDefault(nameof(BeepOnValidScans), false);
            set => AppSettings.AddOrUpdateValue(nameof(BeepOnValidScans), value);
        }

        public static bool Ean13
        {
            get => AppSettings.GetValueOrDefault(nameof(Ean13), false);
            set => AppSettings.AddOrUpdateValue(nameof(Ean13), value);
        }

        public static bool Upce
        {
            get => AppSettings.GetValueOrDefault(nameof(Upce), false);
            set => AppSettings.AddOrUpdateValue(nameof(Upce), value);
        }

        public static bool DataMatrix
        {
            get => AppSettings.GetValueOrDefault(nameof(DataMatrix), false);
            set => AppSettings.AddOrUpdateValue(nameof(DataMatrix), value);
        }

        public static bool Qr
        {
            get => AppSettings.GetValueOrDefault(nameof(Qr), false);
            set => AppSettings.AddOrUpdateValue(nameof(Qr), value);
        }

        public static bool Code39
        {
            get => AppSettings.GetValueOrDefault(nameof(Code39), false);
            set => AppSettings.AddOrUpdateValue(nameof(Code39), value);
        }
        public static bool Code128
        {
            get => AppSettings.GetValueOrDefault(nameof(Code128), false);
            set => AppSettings.AddOrUpdateValue(nameof(Code128), value);
        }

        public static bool BatchScan
        {
            get => AppSettings.GetValueOrDefault(nameof(BatchScan), false);
            set => AppSettings.AddOrUpdateValue(nameof(BatchScan), value);
        }

        public static bool IsBluetoothOn
        {
            get => AppSettings.GetValueOrDefault(nameof(IsBluetoothOn), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsBluetoothOn), value);
        }

        public static string IpAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(IpAddress), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(IpAddress), value);
        }

        public static string Port
        {
            get => AppSettings.GetValueOrDefault(nameof(Port), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Port), value);
        }

        public static string PrinterAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(PrinterAddress), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(PrinterAddress), value);
        }

        public static string FriendlyLbl
        {
            get => AppSettings.GetValueOrDefault(nameof(FriendlyLbl), "Select printer");
            set => AppSettings.AddOrUpdateValue(nameof(FriendlyLbl), value);
        }

        public static void RemoveUserData()
        {
            AppSettings.Remove(nameof(SessionId));
            AppSettings.AddOrUpdateValue(nameof(IsMetaDataLoaded), false);
            AppSettings.Clear();
        }
    }
}
