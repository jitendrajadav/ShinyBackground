using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;

namespace KegID.Services
{
    public class DashboardService : IDashboardService
    {
        public async Task<DashboardResponseModel> GetDeshboardDetailAsync(string sessionId)
        {
            DashboardResponseModel dashboardModel = new DashboardResponseModel();

            string url = string.Format(Configuration.GetDashboardUrl, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            dashboardModel = Helper.DeserializeObject<DashboardResponseModel>(value.Response);
            dashboardModel.StatusCode = value.StatusCode;
            return dashboardModel;
        }

        public async Task<InventoryDetailModel> GetInventoryAsync(string sessionId)
        {
            InventoryDetailModel inventoryDetailModel = new InventoryDetailModel();

            string url = string.Format(Configuration.GetInventoryUrl, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            inventoryDetailModel.InventoryResponseModel = Helper.DeserializeObject<IList<InventoryResponseModel>>(value.Response);
            inventoryDetailModel.StatusCode = value.StatusCode;
            return inventoryDetailModel;
        }
    }
}
