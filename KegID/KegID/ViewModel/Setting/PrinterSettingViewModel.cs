using System;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class PrinterSettingViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IZebraPrinterManager _zebraPrinterManager;

        #region SelectedPrinter

        /// <summary>
        /// The <see cref="SelectedPrinter" /> property's name.
        /// </summary>
        public const string SelectedPrinterPropertyName = "SelectedPrinter";

        private string _SelectedPrinter = "";

        /// <summary>
        /// Sets and gets the SelectedPrinter property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SelectedPrinter
        {
            get
            {
                return _SelectedPrinter;
            }

            set
            {
                if (_SelectedPrinter == value)
                {
                    return;
                }

                _SelectedPrinter = value;
                RaisePropertyChanged(SelectedPrinterPropertyName);
            }
        }

        #endregion

        #region IsBluetoothOn

        /// <summary>
        /// The <see cref="IsBluetoothOn" /> property's name.
        /// </summary>
        public const string IsBluetoothOnPropertyName = "IsBluetoothOn";

        private bool _IsBluetoothOn = false;

        /// <summary>
        /// Sets and gets the IsBluetoothOn property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsBluetoothOn
        {
            get
            {
                return _IsBluetoothOn;
            }

            set
            {
                if (_IsBluetoothOn == value)
                {
                    return;
                }

                _IsBluetoothOn = value;
                ConstantManager.IsIPAddr = !_IsBluetoothOn;
                RaisePropertyChanged(IsBluetoothOnPropertyName);
            }
        }

        #endregion

        #region IpAddress

        /// <summary>
        /// The <see cref="IpAddress" /> property's name.
        /// </summary>
        public const string IpAddressPropertyName = "IpAddress";

        private string _IpAddress = default(string);

        /// <summary>
        /// Sets and gets the IpAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string IpAddress
        {
            get
            {
                return _IpAddress;
            }

            set
            {
                if (_IpAddress == value)
                {
                    return;
                }

                _IpAddress = value;
                ConstantManager.IPAddr = _IpAddress;
                RaisePropertyChanged(IpAddressPropertyName);
            }
        }

        #endregion

        #region Port

        /// <summary>
        /// The <see cref="Port" /> property's name.
        /// </summary>
        public const string PortPropertyName = "Port";

        private string _Port = default(string);

        /// <summary>
        /// Sets and gets the Port property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Port
        {
            get
            {
                return _Port;
            }

            set
            {
                if (_Port == value)
                {
                    return;
                }

                _Port = value;
                RaisePropertyChanged(PortPropertyName);
            }
        }

        #endregion

        #region PrintEveryManifest

        /// <summary>
        /// The <see cref="PrintEveryManifest" /> property's name.
        /// </summary>
        public const string PrintEveryManifestPropertyName = "PrintEveryManifest";

        private bool _PrintEveryManifest = false;

        /// <summary>
        /// Sets and gets the PrintEveryManifest property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool PrintEveryManifest
        {
            get
            {
                return _PrintEveryManifest;
            }

            set
            {
                if (_PrintEveryManifest == value)
                {
                    return;
                }

                _PrintEveryManifest = value;
                RaisePropertyChanged(PrintEveryManifestPropertyName);
            }
        }

        #endregion

        #region PrintEveryPallet

        /// <summary>
        /// The <see cref="PrintEveryPallet" /> property's name.
        /// </summary>
        public const string PrintEveryPalletPropertyName = "PrintEveryPallet";

        private bool _PrintEveryPallet = false;

        /// <summary>
        /// Sets and gets the PrintEveryPallet property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool PrintEveryPallet
        {
            get
            {
                return _PrintEveryPallet;
            }

            set
            {
                if (_PrintEveryPallet == value)
                {
                    return;
                }

                _PrintEveryPallet = value;
                RaisePropertyChanged(PrintEveryPalletPropertyName);
            }
        }

        #endregion

        #region PalletLabelCopies

        /// <summary>
        /// The <see cref="PalletLabelCopies" /> property's name.
        /// </summary>
        public const string PalletLabelCopiesPropertyName = "PalletLabelCopies";

        private int _PalletLabelCopies = 1;

        /// <summary>
        /// Sets and gets the PalletLabelCopies property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int PalletLabelCopies
        {
            get
            {
                return _PalletLabelCopies;
            }

            set
            {
                if (_PalletLabelCopies == value)
                {
                    return;
                }

                _PalletLabelCopies = value;
                RaisePropertyChanged(PalletLabelCopiesPropertyName);
            }
        }

        #endregion

        #region BeepOnValidScans

        /// <summary>
        /// The <see cref="BeepOnValidScans" /> property's name.
        /// </summary>
        public const string BeepOnValidScansPropertyName = "BeepOnValidScans";

        private bool _BeepOnValidScans = false;

        /// <summary>
        /// Sets and gets the BeepOnValidScans property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool BeepOnValidScans
        {
            get
            {
                return _BeepOnValidScans;
            }

            set
            {
                if (_BeepOnValidScans == value)
                {
                    return;
                }

                _BeepOnValidScans = value;
                RaisePropertyChanged(BeepOnValidScansPropertyName);
            }
        }

        #endregion

        #region BatchScan

        /// <summary>
        /// The <see cref="BatchScan" /> property's name.
        /// </summary>
        public const string BatchScanPropertyName = "BatchScan";

        private bool _BatchScan = false;

        /// <summary>
        /// Sets and gets the BatchScan property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool BatchScan
        {
            get
            {
                return _BatchScan;
            }

            set
            {
                if (_BatchScan == value)
                {
                    return;
                }

                _BatchScan = value;
                RaisePropertyChanged(BatchScanPropertyName);
            }
        }

        #endregion

        #region Version

        /// <summary>
        /// The <see cref="Version" /> property's name.
        /// </summary>
        public const string VersionPropertyName = "Version";

        private string _Version = string.Empty;

        /// <summary>
        /// Sets and gets the Version property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Version
        {
            get
            {
                return _Version;
            }

            set
            {
                if (_Version == value)
                {
                    return;
                }

                _Version = value;
                RaisePropertyChanged(VersionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand SelectPrinterCommand { get; }
        public DelegateCommand PrinterTestCommand { get; }

        #endregion

        #region Constructor

        public PrinterSettingViewModel(INavigationService navigationService, IZebraPrinterManager zebraPrinterManager)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
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
            await _navigationService.NavigateAsync(new Uri("SelectPrinterView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        private void PrinterTestCommandReciever()
        {
            new Task(new Action(() =>
            {
                _zebraPrinterManager.SendZplPalletAsync(_zebraPrinterManager.TestPrint, IsBluetoothOn, IpAddress);
            })).Start();
        }

        private async void CancelCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        private async void SaveCommandRecieverAsync()
        {
            UpdatePrintingSetting();
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        private void UpdatePrintingSetting()
        {
            AppSettings.PrintEveryManifest = PrintEveryManifest;
            AppSettings.PrintEveryPallet = PrintEveryPallet;
            AppSettings.PalletLabelCopies = PalletLabelCopies;
            AppSettings.BeepOnValidScans = BeepOnValidScans;
            AppSettings.IsBluetoothOn = IsBluetoothOn;
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
            IsBluetoothOn = AppSettings.IsBluetoothOn;
            IpAddress = AppSettings.IpAddress;
            Port = AppSettings.Port;
            SelectedPrinter = AppSettings.FriendlyLbl;
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("friendlyLbl"))
                SelectedPrinter = parameters.GetValue<string>("friendlyLbl");
            else
                SelectedPrinter = "No printers found";

            AssignPrintingSetting();
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
