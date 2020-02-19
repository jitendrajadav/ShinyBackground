using System;
using System.Threading.Tasks;
using KegID.Messages;
using KegID.Model;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegSearchViewModel : BaseViewModel
    {
        #region Properties

        public string Barcode { get; set; }
        public string KegsSuccessMsg { get; set; }

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand BarcodeScanCommand { get; }
        public DelegateCommand BulkUpdateCommand { get; }
        public DelegateCommand SearchCommand { get; }

        #endregion

        #region Constructor

        public KegSearchViewModel(INavigationService navigationService) : base(navigationService)
        {
            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            BarcodeScanCommand = new DelegateCommand(BarcodeScanCommandRecieverAsync);
            BulkUpdateCommand = new DelegateCommand(BulkUpdateCommandRecieverAsync);
            SearchCommand = new DelegateCommand(SearchCommandRecieverAsync);
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<BarcodeScannerToKegSearchMsg>(this, "BarcodeScannerToKegSearchMsg", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var value = message;
                    if (value != null)
                    {
                        AssignBarcodeScannerValueAsync(value.Barcodes);
                    }
                });
            });
        }
        private async void SearchCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("KegSearchedListView", new NavigationParameters
                    {
                        { "LoadKegSearchAsync", Barcode }
                    }, animated: false);
        }

        private async void HomeCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("CognexScanView", new NavigationParameters
                    {
                        { "Tags", null },{ "TagsStr", string.Empty },{ "ViewTypeEnum", ViewTypeEnum.KegSearchView }
                    }, animated: false);
        }

        private async void BulkUpdateCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("BulkUpdateScanView", animated: false);
        }

        internal async void AssingSuccessMsgAsync()
        {
            KegsSuccessMsg = "Kegs successfully updated";
            await Task.Delay(new TimeSpan(0, 0, 5));
            KegsSuccessMsg = string.Empty;
        }

        internal async void AssignBarcodeScannerValueAsync(Barcode barcodes)
        {
            await NavigationService.NavigateAsync("KegSearchedListView", new NavigationParameters
                    {
                        { "LoadKegSearchAsync", barcodes }
                    }, animated: false);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<BarcodeScannerToKegSearchMsg>(this, "BarcodeScannerToKegSearchMsg");
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("AssingSuccessMsgAsync"))
            {
                AssingSuccessMsgAsync();
            }
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        #endregion
    }
}
