using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IAccountService
    {
        Task<LoginResponseModel> AuthenticateAsync(string username, string password);
    }
}
