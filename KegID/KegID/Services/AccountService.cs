using KegID.Common;
using KegID.Response;
using System.Threading.Tasks;

namespace KegID.Services
{
    public enum HttpMethodType
    {
        Get,
        Post,
        Put,
        Delete
    }

    public class AccountService : IAccountService
    {
        public async Task<LoginModel> AuthenticateAsync(string username, string password)
        {
            string url = string.Format(Configuration.GetLoginUserUrl, username,password);
            return await Helper.ExecutePostCall<LoginModel>(url, HttpMethodType.Get, string.Empty);
        }
    }
}
