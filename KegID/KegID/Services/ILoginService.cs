using Fusillade;
using KegID.Dtos;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface ILoginService
    {
        Task<LoginDto> GetLogin(Priority priority, string username, string password);
    }
}
