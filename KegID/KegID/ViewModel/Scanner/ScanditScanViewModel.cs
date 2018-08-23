﻿using KegID.Common;
using KegID.Converter;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ScanditScanViewModel : BaseViewModel
    {
        #region Properties

        private const string Cloud = "collectionscloud.png";
        private readonly IMoveService _moveService;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        public IList<Tag> Tags { get; set; }
        public string TagsStr { get; set; }
        public string Page { get; set; }
        IList<BarcodeModel> models = new List<BarcodeModel>();
        private ScanSettings _scanSettings;

        #region IsAnalyzing
        private bool isAnalyzing = true;
        public bool IsAnalyzing
        {
            get { return isAnalyzing; }
            set
            {
                if (!Equals(isAnalyzing, value))
                {
                    isAnalyzing = value;
                    RaisePropertyChanged(nameof(IsAnalyzing));
                }
            }
        }
        #endregion

        #region IsScanning

        private bool isScanning = true;
        public bool IsScanning
        {
            get { return isScanning; }
            set
            {
                if (!Equals(isScanning, value))
                {
                    isScanning = value;
                    RaisePropertyChanged(nameof(IsScanning));
                }
            }
        }
        #endregion

        #region BottonText

        private string bottonText = default(string);
        public string BottonText
        {
            get { return bottonText; }
            set
            {
                if (!Equals(bottonText, value))
                {
                    bottonText = value;
                    RaisePropertyChanged(nameof(BottonText));
                }
            }
        }
        #endregion

        #endregion

        #region Commands

        public DelegateCommand DoneCommand { get; }
        
        #endregion

        #region Constructor

        public ScanditScanViewModel(IMoveService moveService, INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _moveService = moveService;
            _dialogService = dialogService;

            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);

            InitSettings();
            ScanditService.BarcodePicker.DidScan += OnDidScan;
            ScanditService.BarcodePicker.DidStop += OnDidStopAsync;
            UpdateScanSettings();
            UpdateScanOverlay();
            ScanditService.BarcodePicker.CancelButtonText = "Done";
            ScanditService.BarcodePicker.StartScanningAsync(false);
        }

        #endregion

        #region Methods

        private async void OnDidStopAsync(DidStopReason reason)
        {
            var message = new StartLongRunningTaskMessage
            {
                Barcode = models.Select(x => x.Barcode).ToList(),
                PageName = Page
            };
            MessagingCenter.Send(message, "StartLongRunningTaskMessage");

            await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "models", models }
                    }, useModalNavigation: true, animated: false);

            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
                }
            });
        }

        private async void InitSettings()
        {
            IBarcodePicker _picker = ScanditService.BarcodePicker;

            // The scanning behavior of the barcode picker is configured through scan
            // settings. We start with empty scan settings and enable a very generous
            // set of symbologies. In your own apps, only enable the symbologies you
            // actually need.
            _scanSettings = _picker.GetDefaultScanSettings();
            var symbologiesToEnable = new Symbology[] {
                Symbology.Qr,
                Symbology.Ean13,
                Symbology.Upce,
                Symbology.Code128,
                Symbology.Ean8,
                Symbology.Upca,
                Symbology.DataMatrix
            };
            foreach (var sym in symbologiesToEnable)
                _scanSettings.EnableSymbology(sym, true);
            await _picker.ApplySettingsAsync(_scanSettings);
            // This will open the scanner in full-screen mode. 
        }

        // reads the values needed for ScanSettings from the Settings class
        // and applies them to the Picker
        private void UpdateScanSettings()
        {
            bool addOnEnabled = false;
            foreach (string setting in ScanditConvert.settingToSymbologies.Keys)
            {
                bool enabled = AppSettings.getBoolSetting(setting);
                foreach (Symbology sym in ScanditConvert.settingToSymbologies[setting])
                {
                    _scanSettings.EnableSymbology(sym, enabled);
                    if (AppSettings.hasInvertedSymbology(setting))
                        _scanSettings.Symbologies[sym].ColorInvertedEnabled = AppSettings.getBoolSetting(AppSettings.getInvertedSymboloby(setting));
                    if (enabled && (sym == Symbology.TwoDigitAddOn || sym == Symbology.FiveDigitAddOn))
                        addOnEnabled = true;
                }
            }
            if (addOnEnabled)
                _scanSettings.MaxNumberOfCodesPerFrame = 2;

            _scanSettings.Symbologies[Symbology.MsiPlessey].Checksums = ScanditConvert.msiPlesseyChecksumToScanSetting[AppSettings.getStringSetting(AppSettings.MsiPlesseyChecksumString)];
            _scanSettings.RestrictedAreaScanningEnabled = AppSettings.getBoolSetting(AppSettings.RestrictedAreaString);

            if (_scanSettings.RestrictedAreaScanningEnabled)
            {
                Double HotSpotHeight = AppSettings.getDoubleSetting(AppSettings.HotSpotHeightString);
                Double HotSpotWidth = AppSettings.getDoubleSetting(AppSettings.HotSpotWidthString);
                Double HotSpotY = AppSettings.getDoubleSetting(AppSettings.HotSpotYString);

                Rect restricted = new Rect(0.5f - HotSpotWidth * 0.5f, HotSpotY - 0.5f * HotSpotHeight, HotSpotWidth, HotSpotHeight);

                _scanSettings.ScanningHotSpot = new Scandit.BarcodePicker.Unified.Point(0.5, AppSettings.getDoubleSetting(AppSettings.HotSpotYString));
                _scanSettings.ActiveScanningAreaPortrait = restricted;
                _scanSettings.ActiveScanningAreaLandscape = restricted;
            }

            _scanSettings.ResolutionPreference = ScanditConvert.resolutionToScanSetting[AppSettings.getStringSetting(AppSettings.ResolutionString)];
            _scanSettings.CodeDuplicateFilter = 0;
            ScanditService.BarcodePicker.ApplySettingsAsync(_scanSettings);
        }

        private void UpdateScanOverlay()
        {
            ScanditService.BarcodePicker.ScanOverlay.BeepEnabled = AppSettings.BeepOnValidScans;
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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ViewTypeEnum"))
            {
                switch (parameters.GetValue<ViewTypeEnum>("ViewTypeEnum"))
                {
                    case ViewTypeEnum.BulkUpdateScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.BulkUpdateScanView.ToString();
                        break;
                    case ViewTypeEnum.KegSearchView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.KegSearchView.ToString();
                        break;
                    case ViewTypeEnum.FillScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.FillScanView.ToString();
                        break;
                    case ViewTypeEnum.MaintainScanView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.MaintainScanView.ToString();
                        break;
                    case ViewTypeEnum.ScanKegsView:
                        Tags = parameters.GetValue<List<Tag>>("Tags");
                        TagsStr = parameters.GetValue<string>("TagsStr");
                        Page = ViewTypeEnum.ScanKegsView.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}