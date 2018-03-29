using KegID.Common;
using KegID.Model;
using System.Threading.Tasks;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class AccountService : IAccountService
    {
        public async Task<LoginResponseModel> AuthenticateAsync(string username, string password)
        {
            LoginResponseModel loginResponseModel = new LoginResponseModel();

            string url = string.Format(Configuration.GetLoginUserUrl, username,password);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            loginResponseModel.LoginModel = DeserializeObject<LoginModel>(value.Response, GetJsonSetting());
            loginResponseModel.StatusCode = value.StatusCode;
            return loginResponseModel;
        }
    }
}
