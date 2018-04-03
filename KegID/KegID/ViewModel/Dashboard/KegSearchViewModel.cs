using System;
using GalaSoft.MvvmLight.Command;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegSearchViewModel : BaseViewModel
    {
        #region Properties

        #region Borcode

        /// <summary>
        /// The <see cref="Borcode" /> property's name.
        /// </summary>
        public const string BorcodePropertyName = "Borcode";

        private string _Borcode = string.Empty;

        /// <summary>
        /// Sets and gets the Borcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Borcode
        {
            get
            {
                return _Borcode;
            }

            set
            {
                if (_Borcode == value)
                {
                    return;
                }

                _Borcode = value;
                RaisePropertyChanged(BorcodePropertyName);
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
            SearchCommand = new RelayCommand(SearchCommandReciever);
        }

        #endregion

        #region Methods

        private void SearchCommandReciever()
        {
            
        }

        private async void HomeCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void BarcodeScanCommandReciever()
        {
           var value = Borcode;
        }
        private async void BulkUpdateCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new BulkUpdateScanView());
        }

        #endregion
    }
}
