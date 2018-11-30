using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using KegID.DependencyServices;
using KegID.Services;
using KegID.Views;
using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SelectPrinterViewModel : BaseViewModel
    {
        #region Propreties

        //private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        ConnectionType connetionType;

        #region LabelMsg

        /// <summary>
        /// The <see cref="LabelMsg" /> property's name.
        /// </summary>
        public const string LabelMsgPropertyName = "LabelMsg";

        private string _LabelMsg = "Discovering Printers...";

        /// <summary>
        /// Sets and gets the LabelMsg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LabelMsg
        {
            get
            {
                return _LabelMsg;
            }

            set
            {
                if (_LabelMsg == value)
                {
                    return;
                }

                _LabelMsg = value;
                RaisePropertyChanged(LabelMsgPropertyName);
            }
        }

        #endregion

        #region PrinterList

        /// <summary>
        /// The <see cref="PrinterList" /> property's name.
        /// </summary>
        public const string PrinterListPropertyName = "PrinterList";

        private ObservableCollection<IDiscoveredPrinter> _printerList = new ObservableCollection<IDiscoveredPrinter>();

        /// <summary>
        /// Sets and gets the PrinterList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<IDiscoveredPrinter> PrinterList
        {
            get
            {
                return _printerList;
            }

            set
            {
                if (_printerList == value)
                {
                    return;
                }

                _printerList = value;
                RaisePropertyChanged(PrinterListPropertyName);
            }
        }

        #endregion

        #region friendlyLbl

        /// <summary>
        /// The <see cref="friendlyLbl" /> property's name.
        /// </summary>
        public const string friendlyLblPropertyName = "friendlyLbl";

        private string _friendlyLbl = default;

        /// <summary>
        /// Sets and gets the friendlyLbl property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string friendlyLbl
        {
            get
            {
                return _friendlyLbl;
            }

            set
            {
                if (_friendlyLbl == value)
                {
                    return;
                }

                _friendlyLbl = value;
                RaisePropertyChanged(friendlyLblPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand BackCommand { get; }
        public DelegateCommand<IDiscoveredPrinter> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public SelectPrinterViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            //_navigationService = navigationService;
            _dialogService = dialogService;

            BackCommand = new DelegateCommand(BackCommandReceiver);
            ItemTappedCommand = new DelegateCommand<IDiscoveredPrinter>((model) => ItemTappedCommandReciever(model));
        }

        #endregion

        #region Methods

        private async void ItemTappedCommandReciever(IDiscoveredPrinter model)
        {
            Xamarin.Forms.DependencyService.Get<IPrinterDiscovery>().CancelDiscovery();
            if (model is IDiscoveredPrinterUsb)
            {
                if (!((IDiscoveredPrinterUsb)model).HasPermissionToCommunicate)
                {
                    Xamarin.Forms.DependencyService.Get<IPrinterDiscovery>().RequestUSBPermission((IDiscoveredPrinterUsb)model);
                }
            }
            try
            {
                ConstantManager.PrinterSetting = model;
                Xamarin.Forms.DependencyService.Get<IPrinterDiscovery>().CancelDiscovery();
                await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter", ConstantManager.PrinterSetting}
                        }, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BackCommandReceiver()
        {
            Xamarin.Forms.DependencyService.Get<IPrinterDiscovery>().CancelDiscovery();
            await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter", ConstantManager.PrinterSetting},{"friendlyLbl", friendlyLbl }
                        },animated: false);
        }

        private void StartSearch()
        {
            new Task(new Action(() =>
            {
                StartBluetoothDiscovery();
            })).Start();
        }

        private void StartUSBDiscovery()
        {
            OnStatusMessage("Discovering USB Printers");
            try
            {
                IDiscoveryEventHandler usbhandler = DiscoveryHandlerFactory.Current.GetInstance();
                usbhandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
                usbhandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
                usbhandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
                connetionType = ConnectionType.USB;
                System.Diagnostics.Debug.WriteLine("Starting USB Discovery");
                Xamarin.Forms.DependencyService.Get<IPrinterDiscovery>().FindUSBPrinters(usbhandler);
            }
            catch (NotImplementedException)
            {
                //  USB not availible on iOS, so handle the exeption and move on to Bluetooth discovery
                StartBluetoothDiscovery();
            }
        }

        private void StartNetworkDiscovery()
        {
            OnStatusMessage("Discovering Network Printers");
            try
            {
                IDiscoveryEventHandler nwhandler = DiscoveryHandlerFactory.Current.GetInstance();
                nwhandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
                nwhandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
                nwhandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
                connetionType = ConnectionType.Network;
                System.Diagnostics.Debug.WriteLine("Starting Network Discovery");
                NetworkDiscoverer.Current.LocalBroadcast(nwhandler);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Network Exception: " + e.Message);
            }
        }

        private void StartBluetoothDiscovery()
        {
            OnStatusMessage("Discovering Bluetooth Printers");
            IDiscoveryEventHandler bthandler = DiscoveryHandlerFactory.Current.GetInstance();
            bthandler.OnDiscoveryError += DiscoveryHandler_OnDiscoveryError;
            bthandler.OnDiscoveryFinished += DiscoveryHandler_OnDiscoveryFinished;
            bthandler.OnFoundPrinter += DiscoveryHandler_OnFoundPrinter;
            connetionType = ConnectionType.Bluetooth;
            System.Diagnostics.Debug.WriteLine("Starting Bluetooth Discovery");
            Xamarin.Forms.DependencyService.Get<IPrinterDiscovery>().FindBluetoothPrinters(bthandler);
        }

        private void DiscoveryHandler_OnFoundPrinter(object sender, IDiscoveredPrinter discoveredPrinter)
        {
            System.Diagnostics.Debug.WriteLine("Found Printer:" + discoveredPrinter.ToString());
            Device.BeginInvokeOnMainThread(() =>
            {
                if (!PrinterList.Contains(discoveredPrinter))
                {
                    PrinterList.Add(discoveredPrinter);
                }
            });
        }

        private void DiscoveryHandler_OnDiscoveryFinished(object sender)
        {

            if (connetionType == ConnectionType.USB)
            {
                StartBluetoothDiscovery();
            }
            else if (connetionType == ConnectionType.Bluetooth)
            {
                StartNetworkDiscovery();
            }
            else
                OnStatusMessage("Discovery Finished");
        }

        private void DiscoveryHandler_OnDiscoveryError(object sender, string message)
        {
            System.Diagnostics.Debug.WriteLine("On Discovery Error: " + connetionType.ToString());
            OnError(message);

            if (connetionType == ConnectionType.USB)
            {
                StartBluetoothDiscovery();
            }
            else if (connetionType == ConnectionType.Bluetooth)
            {
                StartNetworkDiscovery();
            }
            else
                OnStatusMessage("Discovery Finished");
        }

        private void OnError(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await _dialogService.DisplayAlertAsync("Error", message, "OK");
            });
        }

        private void OnStatusMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                LabelMsg = message;
            });
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            StartSearch();
        }

        #endregion
    }
}
