using KegID.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static KegID.Common.Helper;

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
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            loginResponseModel.LoginModel = DeserializeObject<LoginModel>(value.Response, settings);
            loginResponseModel.StatusCode = value.StatusCode;
            return loginResponseModel;
        }
    }
}
