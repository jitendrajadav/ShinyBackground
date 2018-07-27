using KegID.Services;
using System;
using System.Threading;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        private readonly ISyncManager _syncManager;
        private readonly IDeviceCheckInMngr _deviceCheckInMngr;
        Timer timer;
        #endregion

        #region Commands

        #endregion

        #region Constructor

        public MainViewModel(ISyncManager syncManager, IDeviceCheckInMngr deviceCheckInMngr)
        {
            _syncManager = syncManager;
            _deviceCheckInMngr = deviceCheckInMngr;
            Device.StartTimer(TimeSpan.FromDays(1), () =>
            {
                // Do something
                DeviceCheckIn();
                return true; // True = Repeat again, False = Stop the timer
            });
            _syncManager.NotifyConnectivityChanged();

            DeviceCheckIn();
        }

        private async void DeviceCheckIn()
        {
            await _deviceCheckInMngr.DeviceCheckInAync();
        }

        #endregion

        #region Methods


        #endregion
    }
}
