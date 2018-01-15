using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SizeViewModel : ViewModelBase
    {
        #region Properties

        #endregion

        #region Commands

        public RelayCommand BBL2Command { get; set; }
        public RelayCommand BBL4Command { get; set; }
        public RelayCommand BBL6Command { get; set; }
        public RelayCommand L40Command { get; set; }
        public RelayCommand L50Command { get; set; }

        #endregion

        #region Constructor

        public SizeViewModel()
        {
            BBL2Command = new RelayCommand(BBL2CommandRecieverAsync);
            BBL4Command = new RelayCommand(BBL4CommandRecieverAsync);
            BBL6Command = new RelayCommand(BBL6CommandRecieverAsync);
            L40Command = new RelayCommand(L40CommandRecieverAsync);
            L50Command = new RelayCommand(L50CommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void L50CommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<FillViewModel>().SizeButtonTitle = "50 L";
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void L40CommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<FillViewModel>().SizeButtonTitle = "40 L";
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void BBL6CommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<FillViewModel>().SizeButtonTitle = "1/6 bbl";
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void BBL4CommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<FillViewModel>().SizeButtonTitle = "1/4 bbl";
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void BBL2CommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<FillViewModel>().SizeButtonTitle = "1/2 bbl";
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
