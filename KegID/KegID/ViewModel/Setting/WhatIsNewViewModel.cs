using KegID.Messages;
using Prism.Navigation;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class WhatIsNewViewModel : BaseViewModel
    {
        #region Properties

        public IList<ImageClass> ImageCollection { get; set; }

        #endregion

        #region Commands

        #endregion

        #region Constructor

        public WhatIsNewViewModel(INavigationService navigationService) : base(navigationService)
        {
            ImageCollection = new List<ImageClass>
            {
                new ImageClass{ ImageUri = "new0.png" },
                new ImageClass{ ImageUri = "new1.png" },
                new ImageClass{ ImageUri = "new2.png" },
                new ImageClass{ ImageUri = "new3.png" },
            };

            HandleReceivedMessages();
        }

        #endregion

        #region Methods

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<WhatsNewViewToModel>(this, "WhatsNewViewToModel", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                        // If it is Android or iOS
                        await _navigationService.NavigateAsync("../MainPage", animated: false);
                });
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<WhatsNewViewToModel>(this, "WhatsNewViewToModel");
        }

        #endregion
    }
}
