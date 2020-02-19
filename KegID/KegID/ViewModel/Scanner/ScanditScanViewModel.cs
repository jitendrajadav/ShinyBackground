using Acr.UserDialogs;
using KegID.Common;
using KegID.Converter;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ScanditScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";
        private readonly IGetIconByPlatform _getIconByPlatform;

        public IList<Tag> Tags { get; set; }
        public string TagsStr { get; set; }
        public string PageName { get; set; }
        private readonly IList<BarcodeModel> models = new List<BarcodeModel>();
        public ObservableCollection<BarcodeModel> BarcodeCollection { get; set; } = new ObservableCollection<BarcodeModel>();

        private ScanSettings scanSettings;
        readonly IBarcodePicker picker = ScanditService.BarcodePicker;

        public bool IsAnalyzing { get; set; } = true;
        public bool IsScanning { get; set; } = true;
        public string BottonText { get; set; }

        #endregion

        #region Commands

        public DelegateCommand DoneCommand { get; }

        #endregion

        #region Constructor

        public ScanditScanViewModel(INavigationService navigationService, IGetIconByPlatform getIconByPlatform) : base(navigationService)
        {
            _getIconByPlatform = getIconByPlatform;

            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
            _ = InitSettings();
        }

        #endregion

        #region Methods

        private async void OnDidStopAsync(DidStopReason reason)
        {
            await NavigationService.GoBackAsync(new NavigationParameters
                    {
                        { "models", models }
                    }, animated: false);

            await NavigationService.GoBackAsync(animated: false);
        }

        private void OnDidScan(ScanSession session)
        {
            var firstCode = session.NewlyRecognizedCodes.First();
            var message = "";

            if (session.NewlyRecognizedCodes.Count() == 1)
                message = firstCode.Data;
            else if (session.NewlyRecognizedCodes.Count() > 1)
            {
                var secondCode = session.NewlyRecognizedCodes.ElementAt(1);
                message = secondCode.Data;
            }

            // Because this event handler is called from an scanner-internal thread,
            // you must make sure to dispatch to the main thread for any UI work.
            Device.BeginInvokeOnMainThread(() =>
            {
                var check = models.Any(x => x.Barcode == message);
                if (!check)
                {
                    BottonText = "Last scan: " + message;
                    BarcodeModel model = new BarcodeModel()
                    {
                        Barcode = message,
                        TagsStr = TagsStr,
                        Icon = Cloud
                    };

                    if (Tags != null)
                    {
                        foreach (var item in Tags)
                            model.Tags.Add(item);
                    }
                    models.Add(model);
                    if (PageName == nameof(ViewTypeEnum.PalletizeView))
                    {
                        ScannerToPalletAssign scannerToPalletAssign = new ScannerToPalletAssign
                        {
                            Barcode = models.LastOrDefault()?.Barcode
                        };
                        MessagingCenter.Send(scannerToPalletAssign, "ScannerToPalletAssign");
                    }
                    var moveScan = new MoveScanKegsMessage
                    {
                        Barcode = message
                    };
                    MessagingCenter.Send(moveScan, "MoveScanKegsMessage");

                    var toastConfig = new ToastConfig("Last scan: " + message);
                    toastConfig.SetDuration(3000);
                    toastConfig.SetBackgroundColor(System.Drawing.Color.White);
                    UserDialogs.Instance.Toast(toastConfig);
                }
            });

            if (!Settings.BatchScan)
                session.StopScanning();
        }

        private async Task InitSettings()
        {
            scanSettings = picker.GetDefaultScanSettings();
            UpdateScanOverlay();

            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a very generous
            // set of symbologies. In your own apps, only enable the symbologies you
            // actually need.
            var symbologiesToEnable = new Symbology[] {
                Symbology.Qr,
                Symbology.Ean13,
                Symbology.Upce,
                Symbology.Ean8,
                Symbology.Upca,
                Symbology.Qr,
                Symbology.DataMatrix
            };
            foreach (var sym in symbologiesToEnable)
                scanSettings.EnableSymbology(sym, true);
            await picker.ApplySettingsAsync(scanSettings);
            // This will open the scanner in full-screen mode.

            // This will open the scanner in full-screen mode.
            picker.CancelButtonText = "Done";
            picker.DidScan += OnDidScan;
            picker.DidStop += OnDidStopAsync;
            picker.AlwaysShowModally = true;

            //await UpdateScanSettings();
            await picker.StartScanningAsync();
        }

        // reads the values needed for ScanSettings from the Settings class
        // and applies them to the Picker
#pragma warning disable IDE0051 // Remove unused private members
        private async Task UpdateScanSettings()
#pragma warning restore IDE0051 // Remove unused private members
        {
            bool addOnEnabled = false;
            bool isScanningAreaOverriddenByDpmMode = false;

            foreach (string setting in ScanditConvert.settingToSymbologies.Keys)
            {
                bool enabled = Settings.GetBoolSetting(setting);

                // DPM Mode
                if (Settings.IsDpmMode(setting) && enabled)
                {
                    Rect restricted = new Rect(0.33f, 0.33f, 0.33f, 0.33f);
                    scanSettings.ActiveScanningAreaPortrait = restricted;
                    scanSettings.ActiveScanningAreaLandscape = restricted;

                    isScanningAreaOverriddenByDpmMode = true;

                    // Enabling the direct_part_marking_mode extension comes at the cost of increased frame processing times.
                    // It is recommended to restrict the scanning area to a smaller part of the image for best performance.
                    scanSettings.Symbologies[Symbology.DataMatrix].SetExtensionEnabled("direct_part_marking_mode", true);
                    continue;
                }

                if (ScanditConvert.settingToSymbologies[setting] == null) continue;
                foreach (Symbology sym in ScanditConvert.settingToSymbologies[setting])
                {
                    scanSettings.EnableSymbology(sym, enabled);
                    if (Settings.HasInvertedSymbology(setting))
                    {
                        scanSettings.Symbologies[sym].ColorInvertedEnabled = Settings.GetBoolSetting(
                            Settings.GetInvertedSymbology(setting));
                    }

                    if (enabled && (sym == Symbology.TwoDigitAddOn
                                    || sym == Symbology.FiveDigitAddOn))
                    {
                        addOnEnabled = true;
                    }
                }
            }

            if (addOnEnabled)
            {
                scanSettings.MaxNumberOfCodesPerFrame = 2;
            }

            scanSettings.Symbologies[Symbology.MsiPlessey].Checksums =
                ScanditConvert.msiPlesseyChecksumToScanSetting[Settings.GetStringSetting(Settings.MsiPlesseyChecksumString)];

            scanSettings.RestrictedAreaScanningEnabled = isScanningAreaOverriddenByDpmMode || Settings.GetBoolSetting(Settings.RestrictedAreaString);
            if (Settings.GetBoolSetting(Settings.RestrictedAreaString) && !isScanningAreaOverriddenByDpmMode)
            {
                double HotSpotHeight = Settings.GetDoubleSetting(Settings.HotSpotHeightString);
                double HotSpotWidth = Settings.GetDoubleSetting(Settings.HotSpotWidthString);
                double HotSpotY = Settings.GetDoubleSetting(Settings.HotSpotYString);

                Rect restricted = new Rect(0.5f - HotSpotWidth * 0.5f, HotSpotY - 0.5f * HotSpotHeight,
                                           HotSpotWidth, HotSpotHeight);

                scanSettings.ScanningHotSpot = new Scandit.BarcodePicker.Unified.Point(
                        0.5, Settings.GetDoubleSetting(Settings.HotSpotYString));
                scanSettings.ActiveScanningAreaPortrait = restricted;
                scanSettings.ActiveScanningAreaLandscape = restricted;
            }
            scanSettings.ResolutionPreference =
                ScanditConvert.resolutionToScanSetting[Settings.GetStringSetting(Settings.ResolutionString)];

            await picker.ApplySettingsAsync(scanSettings);
        }

        private void UpdateScanOverlay()
        {
            picker.ScanOverlay.BeepEnabled = Settings.BeepOnValidScans;
            picker.ScanOverlay.VibrateEnabled = true;
            picker.ScanOverlay.TorchButtonVisible = true;

            picker.ScanOverlay.ViewFinderSizePortrait = new Scandit.BarcodePicker.Unified.Size(
               (float)Settings.GetDoubleSetting(Settings.ViewFinderPortraitWidthString),
               (float)Settings.GetDoubleSetting(Settings.ViewFinderPortraitHeightString)
           );
            picker.ScanOverlay.ViewFinderSizeLandscape = new Scandit.BarcodePicker.Unified.Size(
                   (float)Settings.GetDoubleSetting(Settings.ViewFinderLandscapeWidthString),
                   (float)Settings.GetDoubleSetting(Settings.ViewFinderLandscapeHeightString)
            );
            picker.ScanOverlay.CameraSwitchVisibility = CameraSwitchVisibility.Always;
            picker.ScanOverlay.GuiStyle = GuiStyle.Default;
        }

        private async void DoneCommandRecieverAsync()
        {

            await NavigationService.GoBackAsync(new NavigationParameters
                    {
                        { "models", models }
                    }, useModalNavigation: true, animated: false);
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ViewTypeEnum"))
            {
                switch (parameters.GetValue<ViewTypeEnum>("ViewTypeEnum"))
                {
                    case ViewTypeEnum.BulkUpdateScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        PageName = nameof(ViewTypeEnum.BulkUpdateScanView);
                        break;
                    case ViewTypeEnum.KegSearchView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        PageName = nameof(ViewTypeEnum.KegSearchView);
                        break;
                    case ViewTypeEnum.FillScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        PageName = nameof(ViewTypeEnum.FillScanView);
                        break;
                    case ViewTypeEnum.MaintainScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        PageName = nameof(ViewTypeEnum.MaintainScanView);
                        break;
                    case ViewTypeEnum.ScanKegsView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        PageName = nameof(ViewTypeEnum.ScanKegsView);
                        break;
                    case ViewTypeEnum.PalletizeView:
                        PageName = nameof(ViewTypeEnum.PalletizeView);
                        break;
                    default:
                        PageName = string.Empty;
                        break;
                }
            }

            return base.InitializeAsync(parameters);
        }

        #endregion
    }
}
