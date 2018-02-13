using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IPalletizeService
    {
        Task<object> PostPalletAsync(PalletRequestModel model, string sessionId, string RequestType);
    }
}
