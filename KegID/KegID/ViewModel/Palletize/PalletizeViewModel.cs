using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletizeViewModel : ViewModelBase
    {
        #region Properties

        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public PalletizeViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void CancelCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
