using System;
using System.Threading.Tasks;
using KegID.Common;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class PrinterSettingViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

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

        #endregion

        #region Commands

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand SelectPrinterCommand { get; }
        public DelegateCommand PrinterTestCommand { get; }

        #endregion

        #region Constructor

        public PrinterSettingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            CancelCommand = new DelegateCommand(CancelCommandRecieverAsync);
            SaveCommand = new DelegateCommand(SaveCommandRecieverAsync);
            SelectPrinterCommand = new DelegateCommand(SelectPrinterCommandRecieverAsync);
            PrinterTestCommand = new DelegateCommand(PrinterTestCommandReciever);
        }

        #endregion

        #region Methods
        private async void SelectPrinterCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync(new Uri("SelectPrinterView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        private void PrinterTestCommandReciever()
        {
            new Task(new Action(() => {
                ZebraPrinterManager.SendZplPallet(ZebraPrinterManager.testPrint,IsBluetoothOn, IpAddress);
            })).Start();
        }

        private async void CancelCommandRecieverAsync()
        {
            //await Application.Current.MainPage.Navigation.PopModalAsync();
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        private async void SaveCommandRecieverAsync()
        {
            //await Application.Current.MainPage.Navigation.PopModalAsync();
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            SelectedPrinter = ConstantManager.SelectedPrinter;
        }

        #endregion
    }
}
