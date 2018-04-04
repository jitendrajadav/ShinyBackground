using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegSearchViewModel : BaseViewModel
    {
        #region Properties

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

        #endregion

        #region Commands

        public RelayCommand HomeCommand { get; }
        public RelayCommand BarcodeScanCommand { get; }
        public RelayCommand BulkUpdateCommand { get; }
        public RelayCommand SearchCommand { get; }
        
        #endregion

        #region Constructor

        public KegSearchViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
            BarcodeScanCommand = new RelayCommand(BarcodeScanCommandReciever);
            BulkUpdateCommand = new RelayCommand(BulkUpdateCommandRecieverAsync);
            SearchCommand = new RelayCommand(SearchCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SearchCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<KegSearchedListViewModel>().LoadKegSearchAsync(Barcode);
            await Application.Current.MainPage.Navigation.PushModalAsync(new KegSearchedListView());
        }

        private async void HomeCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void BarcodeScanCommandReciever()
        {
           var value = Barcode;
        }
        private async void BulkUpdateCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new BulkUpdateScanView());
        }

        #endregion
    }
}
