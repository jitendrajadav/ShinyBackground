using KegID.Common;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
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
    }
}
