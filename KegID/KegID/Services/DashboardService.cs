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

        public async Task<MaintenanceAlertModel> GetKegMaintenanceAlertAsync(string kegId, string sessionId)
        {
            MaintenanceAlertModel maintenanceAlertModel = new MaintenanceAlertModel();

            string url = string.Format(Configuration.GetMaintenanceAlertByKegIdUrl, kegId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            maintenanceAlertModel.MaintenanceAlertResponseModel = DeserializeObject<IList<MaintenanceAlertResponseModel>>(value.Response, settings);
            maintenanceAlertModel.StatusCode = value.StatusCode;
            return maintenanceAlertModel;
        }

        public async Task<KegMaintenanceHistoryModel> GetKegMaintenanceHistoryAsync(string kegId, string sessionId)
        {
            KegMaintenanceHistoryModel kegMaintenanceHistoryModel = new KegMaintenanceHistoryModel();

            string url = string.Format(Configuration.GetKegStatusByKegIdUrl, kegId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            kegMaintenanceHistoryModel.KegMaintenanceHistoryResponseModel = DeserializeObject<IList<KegMaintenanceHistoryResponseModel>>(value.Response, settings);
            kegMaintenanceHistoryModel.StatusCode = value.StatusCode;
            return kegMaintenanceHistoryModel;
        }

        public async Task<KegPossessionModel> GetKegPossessionAsync(string sessionId, string partnerId)
        {
            KegPossessionModel kegPossessionModel = new KegPossessionModel();

            string url = string.Format(Configuration.GetKegPossessionByPartnerIdUrl, sessionId, partnerId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            kegPossessionModel.KegPossessionResponseModel = DeserializeObject<IList<KegPossessionResponseModel>>(value.Response, settings);
            kegPossessionModel.StatusCode = value.StatusCode;
            return kegPossessionModel;
        }

        public async Task<KegStatusResponseModel> GetKegStatusAsync(string kegId, string sessionId)
        {
            KegStatusResponseModel kegStatusResponseModel = new KegStatusResponseModel();

            string url = string.Format(Configuration.GetKegMaintenanceHistoryByKegIdUrl, kegId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            kegStatusResponseModel = DeserializeObject<KegStatusResponseModel>(value.Response, settings);
            kegStatusResponseModel.StatusCode = value.StatusCode;
            return kegStatusResponseModel;
        }

        public async Task<PartnerInfoResponseModel> GetPartnerInfoAsync(string sessionId, string partnerId)
        {
            PartnerInfoResponseModel partnerInfoResponseModel = new PartnerInfoResponseModel();

            string url = string.Format(Configuration.GetPartnerInfoByPartnerIdUrl, sessionId, partnerId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            partnerInfoResponseModel = DeserializeObject<PartnerInfoResponseModel>(value.Response, settings);
            partnerInfoResponseModel.StatusCode = value.StatusCode;
            return partnerInfoResponseModel;
        }
    }
}
