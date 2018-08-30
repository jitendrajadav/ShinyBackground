using KegID.Messages;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SelectPrinterViewModel : BaseViewModel
    {
        #region Propreties

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        #endregion

        #region Propreties


        #endregion

        #region Constructor

        public SelectPrinterViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
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
                    await GoBackMethod(value);
                });
            });
        }

        private async System.Threading.Tasks.Task GoBackMethod(SelectPrinterMsg value)
        {
            if (value != null)
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            {"IDiscoveredPrinter",value.IDiscoveredPrinter },{"friendlyLbl",value.friendlyLbl }
                        }, useModalNavigation: true, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Error: Please select printer.", "Ok");
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<SelectPrinterMsg>(this, "SelectPrinterMsg");
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GoBackMethod"))
            {
               await GoBackMethod(null);
            }
        }

        #endregion

    }
}
