using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PrinterSettingViewModel : ViewModelBase
    {
        #region Properties

        #endregion

        #region Commands

        public RelayCommand CancelCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }

        #endregion

        #region Constructor

        public PrinterSettingViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            SaveCommand = new RelayCommand(SaveCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void CancelCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        private async void SaveCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion

    }
}
