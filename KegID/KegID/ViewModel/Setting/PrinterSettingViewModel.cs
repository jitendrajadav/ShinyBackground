using GalaSoft.MvvmLight.Command;
using KegID.PrintTemplates;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PrinterSettingViewModel : BaseViewModel
    {
        #region Properties

        #endregion

        #region Commands

        public RelayCommand CancelCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand SelectPrinterCommand { get; }
        #endregion

        #region Constructor

        public PrinterSettingViewModel()
        {
            CancelCommand = new RelayCommand(CancelCommandRecieverAsync);
            SaveCommand = new RelayCommand(SaveCommandRecieverAsync);
            SelectPrinterCommand = new RelayCommand(SelectPrinterCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void SelectPrinterCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SelectPrinterView());
        }

        private async void CancelCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        private async void SaveCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
