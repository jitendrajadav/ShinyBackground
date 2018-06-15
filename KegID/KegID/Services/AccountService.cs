using KegID.Common;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using System.Threading.Tasks;
using static KegID.Common.Helper;

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
            string url = string.Format(Configuration.GetLoginUserUrl, username,password);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.LoginModel = DeserializeObject<LoginModel>(value.Response, GetJsonSetting());
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
