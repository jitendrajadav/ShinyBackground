using KegID.Messages;
using Prism.Navigation;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SelectPrinterViewModel : BaseViewModel
    {
        #region Propreties

        private readonly INavigationService _navigationService;

        #endregion

        #region Propreties


        #endregion

        #region Constructor

        public SelectPrinterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<SelectPrinterMsg>(this, "SelectPrinterMsg", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var value = message;
                    if (value != null)
                    {
                        //var param = new NavigationParameters
                        //{
                        //    {"IDiscoveredPrinter",value.IDiscoveredPrinter },{"friendlyLbl",value.friendlyLbl }
                        //};
                        await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter",value.IDiscoveredPrinter },{"friendlyLbl",value.friendlyLbl }
                        }, useModalNavigation: true, animated: false);
                    }
                });
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<SelectPrinterMsg>(this, "SelectPrinterMsg");
        }

        #endregion

    }
}
