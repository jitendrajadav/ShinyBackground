using System;
using System.Threading.Tasks;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
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
            try
            {
                await _navigationService.NavigateAsync("KegSearchedListView", new NavigationParameters
                    {
                        { "LoadKegSearchAsync", Barcode }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void HomeCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BarcodeScanCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("CognexScanView", new NavigationParameters
                    {
                        { "Tags", null },{ "TagsStr", string.Empty },{ "ViewTypeEnum", ViewTypeEnum.KegSearchView }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BulkUpdateCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("BulkUpdateScanView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async void AssingSuccessMsgAsync()
        {
            try
            {
                KegsSuccessMsg = "Kegs successfully updated";
                await Task.Delay(new TimeSpan(0, 0, 5));
                KegsSuccessMsg = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async void AssignBarcodeScannerValueAsync(Barcode barcodes)
        {
            try
            {
                await _navigationService.NavigateAsync("KegSearchedListView", new NavigationParameters
                    {
                        { "LoadKegSearchAsync", barcodes }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<BarcodeScannerToKegSearchMsg>(this, "BarcodeScannerToKegSearchMsg");
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("AssingSuccessMsgAsync"))
            {
                AssingSuccessMsgAsync();
            }
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
