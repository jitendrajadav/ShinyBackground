using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegSearchViewModel : BaseViewModel
    {
        #region Properties

        public IMoveService _moveService { get; set; }

        #region Barcode

        /// <summary>
        /// The <see cref="Barcode" /> property's name.
        /// </summary>
        public const string BarcodePropertyName = "Barcode";

        private string _Barcode = string.Empty;

        /// <summary>
        /// Sets and gets the Barcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Barcode
        {
            get
            {
                return _Barcode;
            }

            set
            {
                if (_Barcode == value)
                {
                    return;
                }

                _Barcode = value;
                RaisePropertyChanged(BarcodePropertyName);
            }
        }

        #endregion

        #region KegsSuccessMsg

        /// <summary>
            /// The <see cref="KegsSuccessMsg" /> property's name.
            /// </summary>
        public const string KegsSuccessMsgPropertyName = "KegsSuccessMsg";

        private string _kegsSuccessMsg = string.Empty;

        /// <summary>
        /// Sets and gets the KegsSuccessMsg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string KegsSuccessMsg
        {
            get
            {
                return _kegsSuccessMsg;
            }

            set
            {
                if (_kegsSuccessMsg == value)
                {
                    return;
                }

                _kegsSuccessMsg = value;
                RaisePropertyChanged(KegsSuccessMsgPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand HomeCommand { get; }
        public RelayCommand BarcodeScanCommand { get; }
        public RelayCommand BulkUpdateCommand { get; }
        public RelayCommand SearchCommand { get; }
        
        #endregion

        #region Constructor

        public KegSearchViewModel(IMoveService moveService)
        {
            _moveService = moveService;
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandRecieverAsync);
            BulkUpdateCommand = new RelayCommand(BulkUpdateCommandRecieverAsync);
            SearchCommand = new RelayCommand(SearchCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SearchCommandRecieverAsync()
        {
            try
            {
                SimpleIoc.Default.GetInstance<KegSearchedListViewModel>().LoadKegSearchAsync(Barcode);
                await Application.Current.MainPage.Navigation.PushModalAsync(new KegSearchedListView(), animated: false);
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
                await Application.Current.MainPage.Navigation.PopModalAsync();
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
                await BarcodeScanner.BarcodeScanSingleAsync(_moveService, null, string.Empty);
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
                await Application.Current.MainPage.Navigation.PushModalAsync(new BulkUpdateScanView(), animated: false);
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
                SimpleIoc.Default.GetInstance<KegSearchedListViewModel>().LoadKegSearchAsync(barcodes.Id);
                await Application.Current.MainPage.Navigation.PushModalAsync(new KegSearchedListView(), animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
