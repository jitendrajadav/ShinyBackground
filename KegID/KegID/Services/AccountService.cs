using KegID.Common;
using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public enum HttpMethodType
    {
        Get,
        Post,
        Send,
        Put,
        Delete
    }

    public class AccountService : IAccountService
    {
        public async Task<LoginResponseModel> AuthenticateAsync(string username, string password)
        {
            LoginResponseModel loginResponseModel = new LoginResponseModel();

            string url = string.Format(Configuration.GetLoginUserUrl, username,password);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            loginResponseModel.LoginModel = Helper.DeserializeObject<LoginModel>(value.Response);
            loginResponseModel.StatusCode = value.StatusCode;
            return loginResponseModel;
        }
    }
}
