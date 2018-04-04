using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Services;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletSearchedListViewModel : BaseViewModel
    {
        #region Properties
        public IDashboardService _dashboardService { get; set; }

        #endregion

        #region Commands
        public RelayCommand BackCommand { get; }

        #endregion

        #region Contructor

        public PalletSearchedListViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

            BackCommand = new RelayCommand(BackCommandRecieverAsync);
        }

        #endregion

        #region Methods
        private async void BackCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        internal async void GetPalletSearchAsync(string barcode,string partnerId,string fromDate, string toDate,string kegs,string kegOwnerId)
        {
            var value = await _dashboardService.GetPalletSearchAsync(AppSettings.User.SessionId, barcode, partnerId, fromDate, toDate, kegs, kegOwnerId);
        }

        #endregion
    }
}
