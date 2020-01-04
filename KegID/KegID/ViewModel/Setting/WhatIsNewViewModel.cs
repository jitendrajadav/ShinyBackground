using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class WhatIsNewViewModel : BaseViewModel
    {
        #region Properties

        public IList<ImageClass> ImageCollection { get; set; }
        public string Title { get; set; }
        public ImageClass CurrentItem { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<ImageClass> KegFleetTappedCommand { get; }
        public DelegateCommand NextCommand { get; }

        #endregion

        #region Constructor

        public WhatIsNewViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Next >";

            ImageCollection = new List<ImageClass>
            {
                new ImageClass{ ImageUri = "new0.png", Index=0 },
                new ImageClass{ ImageUri = "new1.png", Index=1 },
                new ImageClass{ ImageUri = "new2.png", Index=2},
                new ImageClass{ ImageUri = "new3.png", Index=3},
            };

            KegFleetTappedCommand = new DelegateCommand<ImageClass>(KegFleetTappedCommandReciever);
            NextCommand = new DelegateCommand(NextCommandReciever);
            CurrentItem = ImageCollection.FirstOrDefault();
        }

        #endregion

        #region Methods

        private async void NextCommandReciever()
        {
            if (CurrentItem.Index == 3)
            {
                await _navigationService.NavigateAsync("../MainPage", animated: false);
            }
            else
            {
                CurrentItem = ImageCollection[CurrentItem.Index + 1];
            }
            Title = CurrentItem.Index == 3 ? "Got It." : "Next >";
        }

        private void KegFleetTappedCommandReciever(ImageClass model)
        {
            if (model.Index == 3)
            {
                try
                {
                    // You can remove the switch to UI Thread if you are already in the UI Thread.
                    Device.BeginInvokeOnMainThread(() => Launcher.OpenAsync(new Uri("https://www.slg.com/kegfleet/")));
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        #endregion
    }
}
