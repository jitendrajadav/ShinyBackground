using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using KegID.DependencyServices;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using Zebra.Sdk.Printer.Discovery;
using DependencyService = Xamarin.Forms.DependencyService;

namespace KegID.ViewModel
{
    public class SelectPrinterViewModel : BaseViewModel
    {
        #region Propreties

        private readonly IPageDialogService _dialogService;
        public string LabelMsg { get; set; } = "Discovering Printers...";
        public ObservableCollection<DiscoveredPrinter> discoveredPrinters { get; set; } = new ObservableCollection<DiscoveredPrinter>();
        public string FriendlyLbl { get; set; }

        #endregion

        #region Commands

        public DelegateCommand BackCommand { get; }
        public DelegateCommand<DiscoveredPrinter> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public SelectPrinterViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            BackCommand = new DelegateCommand(BackCommandReceiver);
            ItemTappedCommand = new DelegateCommand<DiscoveredPrinter>((model) => ItemTappedCommandReciever(model));
        }

        #endregion

        #region Methods

        private async void ItemTappedCommandReciever(DiscoveredPrinter discoveredPrinter)
        {
            ClearDiscoveredPrinters();

            ConstantManager.PrinterSetting = discoveredPrinter;
            await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter", ConstantManager.PrinterSetting}
                        }, animated: false);

        }

        private async void BackCommandReceiver()
        {
            ClearDiscoveredPrinters();
            await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter", ConstantManager.PrinterSetting},{"friendlyLbl", FriendlyLbl }
                        }, animated: false);
        }

        private void ClearDiscoveredPrinters()
        {
            try
            {
                discoveredPrinters.Clear();
            }
            catch (NotImplementedException)
            {
                discoveredPrinters.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
            }
        }

        private async void StartSearch()
        {
            try
            {
                ClearDiscoveredPrinters();

                DiscoveryHandlerImplementation discoveryHandler = new DiscoveryHandlerImplementation(this);

                await Task.Factory.StartNew(() =>
                {
                    try
                    {
                        DependencyService.Get<IConnectionManager>().FindBluetoothPrinters(discoveryHandler);
                    }
                    catch (NotImplementedException)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _dialogService.DisplayAlertAsync("Error", "Bluetooth discovery not supported on this platform", "OK");
                        });
                    }
                });
            }
            catch (Exception e)
            {
                await _dialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
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

        private class DiscoveryHandlerImplementation : DiscoveryHandler
        {
            private SelectPrinterViewModel selectPrinterViewModel;

            public DiscoveryHandlerImplementation(SelectPrinterViewModel selectPrinterViewModel)
            {
                this.selectPrinterViewModel = selectPrinterViewModel;
            }

            public void DiscoveryError(string message)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("Discovery Error", message, "OK");
                });
            }

            public void DiscoveryFinished()
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //discoveryDemoPage.SetInputEnabled(true);
                });
            }

            public void FoundPrinter(DiscoveredPrinter printer)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    selectPrinterViewModel.discoveredPrinters.Add(printer);
                });
            }
        }

        #endregion
    }
}
