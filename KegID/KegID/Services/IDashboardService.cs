using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponseModel> GetDeshboardDetailAsync(string sessionId);
        Task<InventoryDetailModel> GetInventoryAsync(string sessionId);
    }
}
