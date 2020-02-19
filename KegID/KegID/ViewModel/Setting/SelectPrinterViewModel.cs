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
        public ObservableCollection<DiscoveredPrinter> DiscoveredPrinters { get; set; } = new ObservableCollection<DiscoveredPrinter>();
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
            await NavigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter", ConstantManager.PrinterSetting}
                        }, animated: false);
        }

        private async void BackCommandReceiver()
        {
            ClearDiscoveredPrinters();
            await NavigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter", ConstantManager.PrinterSetting},{"friendlyLbl", FriendlyLbl }
                        }, animated: false);
        }

        private void ClearDiscoveredPrinters()
        {
            try
            {
                DiscoveredPrinters.Clear();
            }
            catch (NotImplementedException)
            {
                DiscoveredPrinters.Clear(); // Workaround for Xamarin.Forms.Platform.WPF issue: https://github.com/xamarin/Xamarin.Forms/issues/3648
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            StartSearch();
        }

        private class DiscoveryHandlerImplementation : DiscoveryHandler
        {
            private readonly SelectPrinterViewModel selectPrinterViewModel;

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
                    selectPrinterViewModel.DiscoveredPrinters.Add(printer);
                });
            }
        }

        #endregion
    }
}
