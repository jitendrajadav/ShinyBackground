using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IFillService
    {
        Task<BatchResponseModel> GetBatchListAsync(string sessionId);
    }
}
