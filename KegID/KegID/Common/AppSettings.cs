using KegID.Converter;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using System;

namespace KegID.Common
{
    public static class AppSettings
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

        private static ISettings Settings => CrossSettings.Current;

        #region Scandit Settings

        private static IBarcodePicker picker = ScanditService.BarcodePicker;
        private static ScanSettings scanSettings = picker.GetDefaultScanSettings();

        public const string SymbologyPrefix = "Sym_";
        public const string InvSymbologyPrefix = "Inv_Sym_";

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

        public static bool hasInvertedSymbology(string symbology)
        {
            return (symbology == "Sym_Qr" || symbology == "Sym_DataMatrix");
        }

        public static string getInvertedSymboloby(string symbology)
        {
            if (hasInvertedSymbology(symbology))
            {
                return ("Inv_" + symbology);
            }
            else
            {
                throw new Exception("has no inversion");
            }
        }

        public static bool getBoolSetting(string setting)
        {
            return Settings.GetValueOrDefault(setting, defaultBool(setting));
        }

        public static void setBoolSetting(string setting, bool value)
        {
            Settings.AddOrUpdateValue(setting, value);
        }

        private static bool defaultBool(string setting)
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

        public static int getIntSetting(string setting)
        {
            return Settings.GetValueOrDefault(setting, defaultInt(setting));
        }

        public static void setIntSetting(string setting, int value)
        {
            Settings.AddOrUpdateValue(setting, value);
        }

        private static int defaultInt(string setting)
        {
            return 15;
        }

        public static Double getDoubleSetting(string setting)
        {
            return Settings.GetValueOrDefault(setting, defaultDouble(setting));
        }

        public static void setDoubleSetting(string setting, Double value)
        {
            Settings.AddOrUpdateValue(setting, value);
        }

        private static double defaultDouble(string setting)
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
                    return picker.ScanOverlay.ViewFinderSizePortrait.Height;

                default:
                    throw (new Exception("No such Double setting: " + setting));
            }
        }

        public static string getStringSetting(string setting)
        {
            return Settings.GetValueOrDefault(setting, defaultString(setting));
        }

        public static void setStringSetting(string setting, string value)
        {
            Settings.AddOrUpdateValue(setting, value);
        }

        private static string defaultString(string setting)
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
            get => Settings.GetValueOrDefault(nameof(B2cClientId), DefaultB2cClientId);

            set => Settings.AddOrUpdateValue(nameof(B2cClientId), value);
        }

        public static string B2cTenant
        {
            get => Settings.GetValueOrDefault(nameof(B2cTenant), DefaultB2cTenant);

            set => Settings.AddOrUpdateValue(nameof(B2cTenant), value);
        }

        public static string B2cPolicy
        {
            get => Settings.GetValueOrDefault(nameof(B2cPolicy), DefaultB2cPolicy);

            set => Settings.AddOrUpdateValue(nameof(B2cPolicy), value);
        }

        // API Endpoints
        public static string BookingEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(BookingEndpoint), DefaultBookingEndpoint);

            set => Settings.AddOrUpdateValue(nameof(BookingEndpoint), value);
        }

        public static string HotelsEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(HotelsEndpoint), DefaultHotelsEndpoint);

            set => Settings.AddOrUpdateValue(nameof(HotelsEndpoint), value);
        }

        public static string SuggestionsEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(SuggestionsEndpoint), DefaultSuggestionsEndpoint);

            set => Settings.AddOrUpdateValue(nameof(SuggestionsEndpoint), value);
        }

        public static string NotificationsEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(NotificationsEndpoint), DefaultNotificationsEndpoint);

            set => Settings.AddOrUpdateValue(nameof(NotificationsEndpoint), value);
        }

        public static string ImagesBaseUri
        {
            get => Settings.GetValueOrDefault(nameof(ImagesBaseUri), DefaultImagesBaseUri);

            set => Settings.AddOrUpdateValue(nameof(ImagesBaseUri), value);
        }

        public static string SkypeBotId
        {
            get => Settings.GetValueOrDefault(nameof(SkypeBotId), DefaultSkypeBotId);

            set => Settings.AddOrUpdateValue(nameof(SkypeBotId), value);
        }

        public static string FacebookBotId
        {
            get => Settings.GetValueOrDefault(nameof(FacebookBotId), DefaultFacebookBotId);

            set => Settings.AddOrUpdateValue(nameof(FacebookBotId), value);
        }

        // Other settings
        public static string BingMapsApiKey
        {
            get => Settings.GetValueOrDefault(nameof(BingMapsApiKey), DefaultBingMapsApiKey);

            set => Settings.AddOrUpdateValue(nameof(BingMapsApiKey), value);
        }

        public static string SettingsFileUrl
        {
            get => Settings.GetValueOrDefault(nameof(SettingsFileUrl), DefaultSettingsFileUrl);

            set => Settings.AddOrUpdateValue(nameof(SettingsFileUrl), value);
        }

        public static string FallbackMapsLocation
        {
            get => Settings.GetValueOrDefault(nameof(FallbackMapsLocation), DefaultFallbackMapsLocation);

            set => Settings.AddOrUpdateValue(nameof(FallbackMapsLocation), value);
        }

        public static string SessionId
        {
            get => Settings.GetValueOrDefault(nameof(SessionId), string.Empty);

            set => Settings.AddOrUpdateValue(nameof(SessionId), value);
        }

        public static string CompanyId
        {
            get => Settings.GetValueOrDefault(nameof(CompanyId), string.Empty);

            set => Settings.AddOrUpdateValue(nameof(CompanyId), value);
        }

        public static string MasterCompanyId
        {
            get => Settings.GetValueOrDefault(nameof(MasterCompanyId), string.Empty);

            set => Settings.AddOrUpdateValue(nameof(MasterCompanyId), value);
        }
        public static string UserId
        {
            get => Settings.GetValueOrDefault(nameof(UserId), string.Empty);

            set => Settings.AddOrUpdateValue(nameof(UserId), value);
        }
        public static string SessionExpires
        {
            get => Settings.GetValueOrDefault(nameof(SessionExpires), string.Empty);

            set => Settings.AddOrUpdateValue(nameof(SessionExpires), value);
        }
        public static long Overdue_days
        {
            get => Settings.GetValueOrDefault(nameof(Overdue_days), 0);

            set => Settings.AddOrUpdateValue(nameof(Overdue_days), value);
        }

        public static long At_risk_days
        {
            get => Settings.GetValueOrDefault(nameof(At_risk_days), 0);

            set => Settings.AddOrUpdateValue(nameof(At_risk_days), value);
        }

        public static string MobileCenterAnalyticsAndroid
        {
            get => Settings.GetValueOrDefault(nameof(MobileCenterAnalyticsAndroid), DefaultMobileCenterAnalyticsAndroid);

            set => Settings.AddOrUpdateValue(nameof(MobileCenterAnalyticsAndroid), value);
        }

        public static string MobileCenterAnalyticsIos
        {
            get => Settings.GetValueOrDefault(nameof(MobileCenterAnalyticsIos), DefaultMobileCenterAnalyticsIos);

            set => Settings.AddOrUpdateValue(nameof(MobileCenterAnalyticsIos), value);
        }

        public static string MobileCenterAnalyticsWindows
        {
            get => Settings.GetValueOrDefault(nameof(MobileCenterAnalyticsWindows), DefaultMobileCenterAnalyticsWindows);

            set => Settings.AddOrUpdateValue(nameof(MobileCenterAnalyticsWindows), value);
        }

        public static bool UseFakes
        {
            get => Settings.GetValueOrDefault(nameof(UseFakes), DefaultUseFakes);

            set => Settings.AddOrUpdateValue(nameof(UseFakes), value);
        }

        public static bool HasBooking
        {
            get => Settings.GetValueOrDefault(nameof(HasBooking), DefaultHasBooking);

            set => Settings.AddOrUpdateValue(nameof(HasBooking), value);
        }

        //public static bool IsMetaDataLoaded
        //{
        //    get => Settings.GetValueOrDefault(nameof(IsMetaDataLoaded), false);
        //    set => Settings.AddOrUpdateValue(nameof(IsMetaDataLoaded), value);
        //}

        public static bool PrintEveryManifest
        {
            get => Settings.GetValueOrDefault(nameof(PrintEveryManifest), false);
            set => Settings.AddOrUpdateValue(nameof(PrintEveryManifest), value);
        }

        public static bool PrintEveryPallet
        {
            get => Settings.GetValueOrDefault(nameof(PrintEveryPallet), false);
            set => Settings.AddOrUpdateValue(nameof(PrintEveryPallet), value);
        }

        public static int PalletLabelCopies
        {
            get => Settings.GetValueOrDefault(nameof(PalletLabelCopies), 1);
            set => Settings.AddOrUpdateValue(nameof(PalletLabelCopies), value);
        }

        public static bool BeepOnValidScans
        {
            get => Settings.GetValueOrDefault(nameof(BeepOnValidScans), false);
            set => Settings.AddOrUpdateValue(nameof(BeepOnValidScans), value);
        }

        public static bool Ean13
        {
            get => Settings.GetValueOrDefault(nameof(Ean13), false);
            set => Settings.AddOrUpdateValue(nameof(Ean13), value);
        }

        public static bool Upce
        {
            get => Settings.GetValueOrDefault(nameof(Upce), false);
            set => Settings.AddOrUpdateValue(nameof(Upce), value);
        }

        public static bool DataMatrix
        {
            get => Settings.GetValueOrDefault(nameof(DataMatrix), false);
            set => Settings.AddOrUpdateValue(nameof(DataMatrix), value);
        }

        public static bool Qr
        {
            get => Settings.GetValueOrDefault(nameof(Qr), false);
            set => Settings.AddOrUpdateValue(nameof(Qr), value);
        }

        public static bool Code39
        {
            get => Settings.GetValueOrDefault(nameof(Code39), false);
            set => Settings.AddOrUpdateValue(nameof(Code39), value);
        }
        public static bool Code128
        {
            get => Settings.GetValueOrDefault(nameof(Code128), false);
            set => Settings.AddOrUpdateValue(nameof(Code128), value);
        }

        public static bool BatchScan
        {
            get => Settings.GetValueOrDefault(nameof(BatchScan), false);
            set => Settings.AddOrUpdateValue(nameof(BatchScan), value);
        }

        public static bool IsBluetoothOn
        {
            get => Settings.GetValueOrDefault(nameof(IsBluetoothOn), false);
            set => Settings.AddOrUpdateValue(nameof(IsBluetoothOn), value);
        }

        public static string IpAddress
        {
            get => Settings.GetValueOrDefault(nameof(IpAddress), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(IpAddress), value);
        }

        public static string Port
        {
            get => Settings.GetValueOrDefault(nameof(Port), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(Port), value);
        }

        public static string PrinterAddress
        {
            get => Settings.GetValueOrDefault(nameof(PrinterAddress), string.Empty);
            set => Settings.AddOrUpdateValue(nameof(PrinterAddress), value);
        }

        public static string FriendlyLbl
        {
            get => Settings.GetValueOrDefault(nameof(FriendlyLbl), "Select printer");
            set => Settings.AddOrUpdateValue(nameof(FriendlyLbl), value);
        }

        public static void RemoveUserData()
        {
            Settings.Remove(nameof(SessionId));
            Settings.Clear();
        }
    }
}
