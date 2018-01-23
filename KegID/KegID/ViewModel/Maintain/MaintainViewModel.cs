using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainViewModel : ViewModelBase
    {
        #region Properties

        #endregion

        #region Commands
        public RelayCommand HomeCommand { get; set; }

        #endregion

        #region Constructor
        public MaintainViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void HomeCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
