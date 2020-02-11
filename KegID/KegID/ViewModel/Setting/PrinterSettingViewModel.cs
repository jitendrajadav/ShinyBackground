using System;
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
            Settings.PrintEveryManifest = PrintEveryManifest;
            Settings.PrintEveryPallet = PrintEveryPallet;
            Settings.PalletLabelCopies = PalletLabelCopies;
            Settings.BeepOnValidScans = BeepOnValidScans;
            Settings.Ean13 = Ean13;
            Settings.Upce = Upce;
            Settings.DataMatrix = DataMatrix;
            Settings.Qr = Qr;
            Settings.Code39 = Code39;
            Settings.Code128 = Code128;
            Settings.BatchScan = BatchScan;
            Settings.IpAddress = IpAddress;
            Settings.Port = Port;
            Settings.FriendlyLbl = SelectedPrinter;
        }

        private void AssignPrintingSetting()
        {
            PrintEveryManifest = Settings.PrintEveryManifest;
            PrintEveryPallet = Settings.PrintEveryPallet;
            PalletLabelCopies = Settings.PalletLabelCopies;
            BeepOnValidScans = Settings.BeepOnValidScans;
            BatchScan = Settings.BatchScan;
            IsBluetoothOn = Settings.IsBluetoothOn;
            IpAddress = Settings.IpAddress;
            Port = Settings.Port;
            SelectedPrinter = Settings.FriendlyLbl;
            Ean13 = Settings.Ean13;
            Upce = Settings.Upce;
            DataMatrix = Settings.DataMatrix;
            Qr = Settings.Qr;
            Code39 = Settings.Code39;
            Code128 = Settings.Code128;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("IDiscoveredPrinter"))
            {
                var printer = parameters.GetValue<DiscoveredPrinter>("IDiscoveredPrinter");
                SelectedPrinter = printer.Address;
                Settings.FriendlyLbl = SelectedPrinter;
                Settings.PrinterAddress = SelectedPrinter;
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
