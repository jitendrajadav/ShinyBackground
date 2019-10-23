using KegID.Common;
using KegID.Model;
using Prism.Navigation;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KegID.Services
{
    public class DeviceCheckInMngr : IDeviceCheckInMngr
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;

        public DeviceCheckInMngr(IAccountService accountService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _accountService = accountService;
        }

        public async Task DeviceCheckInAync()
        {
            DeviceCheckinRequestModel deviceModel = new DeviceCheckinRequestModel
            {
                AppVersion = AppInfo.VersionString,
                DeviceId = DeviceInfo.Manufacturer,
                DeviceModel = DeviceInfo.Model,
                OS = DeviceInfo.VersionString,
                PushToken = "",
                UserId = AppSettings.UserId
            };

            KegIDResponse value = await _accountService.DeviceCheckinAsync(deviceModel, AppSettings.SessionId, Configuration.DeviceCheckin);
            if (value.StatusCode != nameof(HttpStatusCode.NoContent))
            {
                var param = new NavigationParameters
                    {
                        { "IsLogOut",true}
                    };
                await _navigationService.NavigateAsync("/NavigationPage/LoginView", param);
            }
        }
    }
}
