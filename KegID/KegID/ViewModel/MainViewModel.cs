using KegID.Common;
using KegID.Messages;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        private readonly ISyncManager _syncManager;
        private readonly IDeviceCheckInMngr _deviceCheckInMngr;
        private readonly IInitializeMetaData _initializeMetaData;
        private readonly INavigationService _navigationService;

        #endregion

        #region Commands

        #endregion

        #region Constructor

        public MainViewModel(INavigationService navigationService, ISyncManager syncManager, IDeviceCheckInMngr deviceCheckInMngr, IInitializeMetaData initializeMetaData)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _syncManager = syncManager;
            _deviceCheckInMngr = deviceCheckInMngr;
            _initializeMetaData = initializeMetaData;

            Device.StartTimer(TimeSpan.FromDays(1), () =>
            {
                // Do something
                DeviceCheckIn();
                return true; // True = Repeat again, False = Stop the timer
            });

            _syncManager.NotifyConnectivityChanged();
            DeviceCheckIn();

            HandleReceivedMessages();
        }

        #endregion

        #region Methods
        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<InvalidServiceCall>(this, "InvalidServiceCall", message =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var param = new NavigationParameters
                    {
                        { "IsLogOut",true}
                    };
                    await _navigationService.NavigateAsync("/NavigationPage/LoginView", param, useModalNavigation: true);
                });
            });
        }

        private async void DeviceCheckIn()
        {
            await _deviceCheckInMngr.DeviceCheckInAync();

            if (!AppSettings.IsMetaDataLoaded)
            {
                try
                {
                    Loader.StartLoading();
                    await _initializeMetaData.LoadInitializeMetaData();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                finally
                {
                    AppSettings.IsMetaDataLoaded = true;
                    Loader.StopLoading();
                }
            }
        }

        #endregion
    }
}
