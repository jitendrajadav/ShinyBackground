using KegID.Common;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace KegID.Services
{
    public class AccountService : IAccountService
    {
        public async Task<LoginResponseModel> AuthenticateAsync(string username, string password)
        {
            LoginResponseModel model = new LoginResponseModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetLoginUserUrl, username, password);
            var value = await App.kegIDClient.
                ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.LoginModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<LoginModel>(value.Response) : new LoginModel();
            try
            {
                model.Response.StatusCode = value.StatusCode;
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
            return model;
        }

        public async Task<object> DeviceCheckinAsync(DeviceCheckinRequestModel inModel, string sessionId, string RequestType)
        {
            DeviceCheckinResponseModel model = new DeviceCheckinResponseModel
            {
                Response = new KegIDResponse()
            };

            string url = string.Format(Configuration.DeviceCheckinUrl, sessionId);
            string content = JsonConvert.SerializeObject(inModel);
            var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            model.DeviceCheckinModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<LoginModel>(value.Response) : new LoginModel();
            try
            {
                model.Response.StatusCode = value.StatusCode;
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
            return model;
        }
    }
}
