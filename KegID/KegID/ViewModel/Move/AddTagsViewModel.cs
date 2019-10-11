using KegID.Messages;
using KegID.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class AddTagsViewModel : BaseViewModel
    {
        #region Properties

        public DateTime ProductionDate { get; set; } = DateTime.Now.Date;
        public DateTime BestByDataDate { get; set; } = DateTime.Now;

        #endregion

        #region Commands

        public DelegateCommand SaveCommand { get; }

        #endregion

        #region Contructor

        public AddTagsViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        #endregion

        #region Methods

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<PagesMessage>(this, "PagesMessage", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var value = message;
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.Barcode))
                        {
                            await _navigationService.GoBackAsync(new NavigationParameters
                                {
                                    { "AddTags", ConstantManager.Tags },
                                    { "Barcode", value.Barcode }
                            }, animated: false);
                        }
                        else
                        {
                            await _navigationService.GoBackAsync(new NavigationParameters
                                {
                                    { "AddTags", ConstantManager.Tags },
                            }, animated: false);
                        }
                    }
                });
            });
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            HandleReceivedMessages();
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<PagesMessage>(this, "PagesMessage");
        }

        #endregion
    }
}
