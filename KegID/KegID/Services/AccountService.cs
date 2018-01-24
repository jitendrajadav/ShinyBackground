using KegID.Common;
using KegID.Response;
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
        public async Task<LoginModel> AuthenticateAsync(string username, string password)
        {
            string url = string.Format(Configuration.GetLoginUserUrl, username,password);
            return await Helper.ExecuteServiceCall<LoginModel>(url, HttpMethodType.Get, string.Empty);
        }
    }
}
