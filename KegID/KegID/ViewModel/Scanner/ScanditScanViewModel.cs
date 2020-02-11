using Acr.UserDialogs;
using KegID.Common;
using KegID.Converter;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        private ScanSettings _scanSettings;
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
            InitSettings();
        }

        #endregion

        #region Methods

        private async void OnDidStopAsync(DidStopReason reason)
        {
            //var message = new StartLongRunningTaskMessage
            //{
            //    Barcode = models.Select(x => x.Barcode).ToList(),
            //    PageName = PageName
            //};
            //MessagingCenter.Send(message, "StartLongRunningTaskMessage");

            foreach (string Barcode in models.Select(x => x.Barcode).ToList())
            {
                // IJobManager can and should be injected into your viewmodel code
                Shiny.ShinyHost.Resolve<Shiny.Jobs.IJobManager>().RunTask("ScanJob" + Barcode, async _ =>
                {
                    // your code goes here - async stuff is welcome (and necessary)
                    var response = await ApiManager.GetValidateBarcode(Barcode, Settings.SessionId);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var data = await Task.Run(() => JsonConvert.DeserializeObject<BarcodeModel>(json, GetJsonSetting()));

                        if (data.Kegs != null)
                        {
                            using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
                            {
                                try
                                {
                                    var oldBarcode = BarcodeCollection.FirstOrDefault(x => x.Barcode == data?.Kegs?.Partners?.FirstOrDefault().Kegs?.FirstOrDefault()?.Barcode);
                                    oldBarcode.Pallets = data.Pallets;
                                    oldBarcode.Kegs = data.Kegs;
                                    oldBarcode.Icon = data?.Kegs?.Partners.Count > 1 ? _getIconByPlatform.GetIcon("validationquestion.png") : data?.Kegs?.Partners?.Count == 0 ? _getIconByPlatform.GetIcon("validationerror.png") : _getIconByPlatform.GetIcon("validationok.png");
                                    if (oldBarcode.Icon == "validationerror.png")
                                        Vibration.Vibrate();
                                    oldBarcode.IsScanned = true;
                                    db.Commit();
                                }
                                catch (Exception EX)
                                {
                                }
                            }
                        }
                    }
                });

            }
            await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "models", models }
                    }, animated: false);

            await _navigationService.GoBackAsync(animated: false);
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
                    try
                    {
                        var toastConfig = new ToastConfig("Last scan: " + message);
                        toastConfig.SetDuration(3000);
                        toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
                        UserDialogs.Instance.Toast(toastConfig);
                    }
                    catch { }
                }
            });

            if (!Settings.BatchScan)
                session.StopScanning();
        }

        async Task InitSettings()
        {
            try
            {
                IBarcodePicker picker = ScanditService.BarcodePicker;

                // The scanning behavior of the barcode picker is configured through scan
                // settings. We start with empty scan settings and enable a very generous
                // set of symbologies. In your own apps, only enable the symbologies you
                // actually need.
                var settings = picker.GetDefaultScanSettings();
                var symbologiesToEnable = new Symbology[] {
                Symbology.Qr,
                //Symbology.Ean13,
                //Symbology.Upce,
                //Symbology.Ean8,
                //Symbology.Upca,
                Symbology.Qr
                //Symbology.DataMatrix
            };
                foreach (var sym in symbologiesToEnable)
                    settings.EnableSymbology(sym, true);
                await picker.ApplySettingsAsync(settings);
                // This will open the scanner in full-screen mode. 

                // This will open the scanner in full-screen mode.
                ScanditService.BarcodePicker.CancelButtonText = "Done";
                ScanditService.BarcodePicker.DidScan += OnDidScan;
                ScanditService.BarcodePicker.DidStop += OnDidStopAsync;
                ScanditService.BarcodePicker.AlwaysShowModally = true;

                //await updateScanSettings();
                //UpdateScanOverlay();
                await ScanditService.BarcodePicker.StartScanningAsync(true);
            }
            catch (Exception ex)
            {

            }
        }

        // reads the values needed for ScanSettings from the Settings class
        // and applies them to the Picker
        async Task updateScanSettings()
        {
            IBarcodePicker picker = ScanditService.BarcodePicker;
            bool addOnEnabled = false;
            bool isScanningAreaOverriddenByDpmMode = false;

            foreach (string setting in ScanditConvert.settingToSymbologies.Keys)
            {
                bool enabled = Settings.getBoolSetting(setting);

                // DPM Mode 
                if (Settings.isDpmMode(setting) && enabled)
                {
                    Rect restricted = new Rect(0.33f, 0.33f, 0.33f, 0.33f);
                    _scanSettings.ActiveScanningAreaPortrait = restricted;
                    _scanSettings.ActiveScanningAreaLandscape = restricted;

                    isScanningAreaOverriddenByDpmMode = true;

                    // Enabling the direct_part_marking_mode extension comes at the cost of increased frame processing times.
                    // It is recommended to restrict the scanning area to a smaller part of the image for best performance.
                    _scanSettings.Symbologies[Symbology.DataMatrix].SetExtensionEnabled("direct_part_marking_mode", true);
                    continue;
                }

                if (ScanditConvert.settingToSymbologies[setting] == null) continue;
                foreach (Symbology sym in ScanditConvert.settingToSymbologies[setting])
                {
                    _scanSettings.EnableSymbology(sym, enabled);
                    if (Settings.hasInvertedSymbology(setting))
                    {
                        _scanSettings.Symbologies[sym].ColorInvertedEnabled = Settings.getBoolSetting(
                            Settings.getInvertedSymbology(setting));
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
                _scanSettings.MaxNumberOfCodesPerFrame = 2;
            }

            _scanSettings.Symbologies[Symbology.MsiPlessey].Checksums =
                ScanditConvert.msiPlesseyChecksumToScanSetting[Settings.getStringSetting(Settings.MsiPlesseyChecksumString)];

            _scanSettings.RestrictedAreaScanningEnabled = isScanningAreaOverriddenByDpmMode || Settings.getBoolSetting(Settings.RestrictedAreaString);
            if (Settings.getBoolSetting(Settings.RestrictedAreaString) && !isScanningAreaOverriddenByDpmMode)
            {
                Double HotSpotHeight = Settings.getDoubleSetting(Settings.HotSpotHeightString);
                Double HotSpotWidth = Settings.getDoubleSetting(Settings.HotSpotWidthString);
                Double HotSpotY = Settings.getDoubleSetting(Settings.HotSpotYString);

                Rect restricted = new Rect(0.5f - HotSpotWidth * 0.5f, HotSpotY - 0.5f * HotSpotHeight,
                                           HotSpotWidth, HotSpotHeight);

                _scanSettings.ScanningHotSpot = new Scandit.BarcodePicker.Unified.Point(
                        0.5, Settings.getDoubleSetting(Settings.HotSpotYString));
                _scanSettings.ActiveScanningAreaPortrait = restricted;
                _scanSettings.ActiveScanningAreaLandscape = restricted;
            }
            _scanSettings.ResolutionPreference =
                ScanditConvert.resolutionToScanSetting[Settings.getStringSetting(Settings.ResolutionString)];

          await picker.ApplySettingsAsync(_scanSettings);
        }

        private void UpdateScanOverlay()
        {
            ScanditService.BarcodePicker.ScanOverlay.BeepEnabled = Settings.BeepOnValidScans;
            ScanditService.BarcodePicker.ScanOverlay.VibrateEnabled = true;
            ScanditService.BarcodePicker.ScanOverlay.TorchButtonVisible = true;
            ScanditService.BarcodePicker.ScanOverlay.CameraSwitchVisibility = CameraSwitchVisibility.Never;
            ScanditService.BarcodePicker.ScanOverlay.GuiStyle = GuiStyle.Default;
        }

        private void DoneCommandRecieverAsync()
        {
            //var message = new StartLongRunningTaskMessage
            //{
            //    Barcode = models.Select(x => x.Barcode).ToList(),
            //    PageName = Page
            //};
            //MessagingCenter.Send(message, "StartLongRunningTaskMessage");

            //await _navigationService.GoBackAsync(new NavigationParameters
            //        {
            //            { "models", models }
            //        }, useModalNavigation: true, animated: false);
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
