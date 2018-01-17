using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using KegID.View;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ManifestsViewModel : ViewModelBase
    {
        #region Properties

        #endregion

        #region Commands

        public RelayCommand HomeCommand { get; set; }
        public RelayCommand ActionSearchCommand { get; set; }
        public RelayCommand QueuedCommand { get; set; }
        public RelayCommand DraftCommand { get; set; }
        public RelayCommand RecentCommand { get; set; }

        #endregion

        #region Constructor

        public ManifestsViewModel()
        {
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
            ActionSearchCommand = new RelayCommand(ActionSearchCommandRecieverAsync);
            QueuedCommand = new RelayCommand(QueuedCommandReciever);
            DraftCommand = new RelayCommand(DraftCommandReciever);
            RecentCommand = new RelayCommand(RecentCommandReciever);
        }

        #endregion

        #region Methods

        private async void HomeCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void ActionSearchCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new SearchManifestsView());
        }

        private void QueuedCommandReciever()
        {
            
        }

        private void DraftCommandReciever()
        {
            
        }

        private void RecentCommandReciever()
        {
            
        }

        #endregion

    }
}
