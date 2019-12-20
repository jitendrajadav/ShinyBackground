﻿using System;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Zebra.Sdk.Printer.Discovery;

namespace KegID.ViewModel
{
    public class PrinterSettingViewModel : BaseViewModel
    {
        #region Properties

        private readonly IZebraPrinterManager _zebraPrinterManager;
        public string SelectedPrinter { get; set; } = "Select printer";
        public bool IsBluetoothOn { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public bool PrintEveryManifest { get; set; }
        public bool PrintEveryPallet { get; set; }
        public int PalletLabelCopies { get; set; } = 1;
        public bool BeepOnValidScans { get; set; }
        public bool BatchScan { get; set; }
        public string Version { get; set; }
        public bool Ean13 { get; set; }
        public bool Upce { get; set; }
        public bool DataMatrix { get; set; }
        public bool Qr { get; set; }
        public bool Code39 { get; set; }
        public bool Code128 { get; set; }

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand SelectPrinterCommand { get; }
        public DelegateCommand PrinterTestCommand { get; }

        #endregion

        #region Constructor

        public PrinterSettingViewModel(INavigationService navigationService, IZebraPrinterManager zebraPrinterManager) : base(navigationService)
        {
            _zebraPrinterManager = zebraPrinterManager;

            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            SaveCommand = new DelegateCommand(SaveCommandRecieverAsync);
            SelectPrinterCommand = new DelegateCommand(SelectPrinterCommandRecieverAsync);
            PrinterTestCommand = new DelegateCommand(PrinterTestCommandReciever);

            Version = "Version " + AppInfo.VersionString;
        }

        #endregion

        #region Methods

        private async void SelectPrinterCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("SelectPrinterView", animated: false);
        }

        private void PrinterTestCommandReciever()
        {
            new Task(new Action(() =>
            {
                _zebraPrinterManager.SendZplPalletAsync(_zebraPrinterManager.TestPrint, IpAddress);
            })).Start();
        }

        private async void CancelCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void SaveCommandRecieverAsync()
        {
            UpdatePrintingSetting();
            await _navigationService.GoBackAsync(animated: false);
        }

        private void UpdatePrintingSetting()
        {
            AppSettings.PrintEveryManifest = PrintEveryManifest;
            AppSettings.PrintEveryPallet = PrintEveryPallet;
            AppSettings.PalletLabelCopies = PalletLabelCopies;
            AppSettings.BeepOnValidScans = BeepOnValidScans;
            AppSettings.Ean13 = Ean13;
            AppSettings.Upce = Upce;
            AppSettings.DataMatrix = DataMatrix;
            AppSettings.Qr = Qr;
            AppSettings.Code39 = Code39;
            AppSettings.Code128 = Code128;
            AppSettings.BatchScan = BatchScan;
            AppSettings.IpAddress = IpAddress;
            AppSettings.Port = Port;
            AppSettings.FriendlyLbl = SelectedPrinter;
        }

        private void AssignPrintingSetting()
        {
            PrintEveryManifest = AppSettings.PrintEveryManifest;
            PrintEveryPallet = AppSettings.PrintEveryPallet;
            PalletLabelCopies = AppSettings.PalletLabelCopies;
            BeepOnValidScans = AppSettings.BeepOnValidScans;
            BatchScan = AppSettings.BatchScan;
            IsBluetoothOn = AppSettings.IsBluetoothOn;
            IpAddress = AppSettings.IpAddress;
            Port = AppSettings.Port;
            SelectedPrinter = AppSettings.FriendlyLbl;
            Ean13 = AppSettings.Ean13;
            Upce = AppSettings.Upce;
            DataMatrix = AppSettings.DataMatrix;
            Qr = AppSettings.Qr;
            Code39 = AppSettings.Code39;
            Code128 = AppSettings.Code128;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("IDiscoveredPrinter"))
            {
                var printer = parameters.GetValue<DiscoveredPrinter>("IDiscoveredPrinter");
                SelectedPrinter = printer.Address;
                AppSettings.FriendlyLbl = SelectedPrinter;
                AppSettings.PrinterAddress = SelectedPrinter;
            }
            else
            {
                SelectedPrinter = "No printers found";
            }

            AssignPrintingSetting();

            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CancelCommandRecieverAsync"))
            {
                CancelCommandRecieverAsync();
            }
        }

        #endregion
    }
}
