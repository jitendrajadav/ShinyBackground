using KegID.Model;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponseModel> GetDeshboardDetailAsync(string sessionId);
        Task<InventoryDetailModel> GetInventoryAsync(string sessionId);

        Task<KegPossessionModel> GetKegPossessionAsync(string sessionId,string partnerId);

        Task<PartnerInfoResponseModel> GetPartnerInfoAsync(string sessionId, string partnerId);

        Task<KegStatusResponseModel> GetKegStatusAsync(string kegId, string sessionId);

        Task<KegMaintenanceHistoryModel> GetKegMaintenanceHistoryAsync(string kegId, string sessionId);

    }
}
