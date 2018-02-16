using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Model;
using Newtonsoft.Json;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class DashboardService : IDashboardService
    {
        public async Task<DashboardResponseModel> GetDeshboardDetailAsync(string sessionId)
        {
            DashboardResponseModel dashboardModel = new DashboardResponseModel();

            string url = string.Format(Configuration.GetDashboardUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            dashboardModel = DeserializeObject<DashboardResponseModel>(value.Response, settings);
            dashboardModel.StatusCode = value.StatusCode;
            return dashboardModel;
        }

        public async Task<InventoryDetailModel> GetInventoryAsync(string sessionId)
        {
            InventoryDetailModel inventoryDetailModel = new InventoryDetailModel();

            string url = string.Format(Configuration.GetInventoryUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            inventoryDetailModel.InventoryResponseModel = DeserializeObject<IList<InventoryResponseModel>>(value.Response, settings);
            inventoryDetailModel.StatusCode = value.StatusCode;
            return inventoryDetailModel;
        }
    }
}
