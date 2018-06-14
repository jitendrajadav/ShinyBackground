﻿using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class DashboardService : IDashboardService
    {
        public async Task<PossessorModel> GetDashboardPartnersListAsync(string ownerId, string sessionId)
        {
            PossessorModel model = new PossessorModel();

            string url = string.Format(Configuration.GetPossessorByownerId, ownerId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.PossessorResponseModel = DeserializeObject<IList<PossessorResponseModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;

            return model;
        }

        public async Task<IList<string>> GetAssetVolumeAsync(string sessionId, bool assignableOnly)
        {
            string url = string.Format(Configuration.GetAssetVolume, sessionId,assignableOnly);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);
            var test = DeserializeObject<IList<string>>(value.Response, GetJsonSetting());
            return test;
        }

        public async Task<DeleteMaintenanceAlertResponseModel> GetDeleteMaintenanceAlertAsync(string kegId, string sessionId)
        {
            DeleteMaintenanceAlertResponseModel model = new DeleteMaintenanceAlertResponseModel();

            string url = string.Format(Configuration.GetMaintenanceAlertByKegIdUrl, kegId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.MyProperty = DeserializeObject<List<string>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;
            return model;
        }

        public async Task<DashboardResponseModel> GetDeshboardDetailAsync(string sessionId)
        {
            DashboardResponseModel dashboardModel = new DashboardResponseModel();

            string url = string.Format(Configuration.GetDashboardUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            dashboardModel = DeserializeObject<DashboardResponseModel>(value.Response, GetJsonSetting());
            dashboardModel.Response.StatusCode = value.StatusCode;
            return dashboardModel;
        }

        public async Task<InventoryDetailModel> GetInventoryAsync(string sessionId)
        {
            InventoryDetailModel inventoryDetailModel = new InventoryDetailModel();

            string url = string.Format(Configuration.GetInventoryUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            inventoryDetailModel.InventoryResponseModel = DeserializeObject<IList<InventoryResponseModel>>(value.Response, GetJsonSetting());
            inventoryDetailModel.Response.StatusCode = value.StatusCode;
            return inventoryDetailModel;
        }

        public async Task<MaintenanceAlertModel> GetKegMaintenanceAlertAsync(string kegId, string sessionId)
        {
            MaintenanceAlertModel maintenanceAlertModel = new MaintenanceAlertModel();

            string url = string.Format(Configuration.GetMaintenanceAlertByKegIdUrl, kegId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            maintenanceAlertModel.MaintenanceAlertResponseModel = DeserializeObject<IList<MaintenanceAlertResponseModel>>(value.Response, GetJsonSetting());
            maintenanceAlertModel.Response.StatusCode = value.StatusCode;
            return maintenanceAlertModel;
        }

        public async Task<KegMaintenanceHistoryModel> GetKegMaintenanceHistoryAsync(string kegId, string sessionId)
        {
            KegMaintenanceHistoryModel kegMaintenanceHistoryModel = new KegMaintenanceHistoryModel();

            string url = string.Format(Configuration.GetKegMaintenanceHistoryByKegIdUrl, kegId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            kegMaintenanceHistoryModel.KegMaintenanceHistoryResponseModel = DeserializeObject<IList<KegMaintenanceHistoryResponseModel>>(value.Response, GetJsonSetting());
            kegMaintenanceHistoryModel.Response.StatusCode = value.StatusCode;
            return kegMaintenanceHistoryModel;
        }

        public async Task<KegPossessionModel> GetKegPossessionAsync(string sessionId, string partnerId)
        {
            KegPossessionModel kegPossessionModel = new KegPossessionModel();

            string url = string.Format(Configuration.GetKegPossessionByPartnerIdUrl, sessionId, partnerId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            kegPossessionModel.KegPossessionResponseModel = DeserializeObject<IList<KegPossessionResponseModel>>(value.Response, GetJsonSetting());
            kegPossessionModel.Response.StatusCode = value.StatusCode;
            return kegPossessionModel;
        }

        public async Task<KegSearchModel> GetKegSearchAsync(string sessionId, string barcode, bool includePartials)
        {
            KegSearchModel model = new KegSearchModel();

            string url = string.Format(Configuration.GetKegSearchByBarcodeUrl, sessionId, barcode, includePartials);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.KegSearchResponseModel = DeserializeObject<IList<KegSearchResponseModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;
            return model;
        }

        public async Task<KegStatusResponseModel> GetKegStatusAsync(string kegId, string sessionId)
        {
            KegStatusResponseModel kegStatusResponseModel = new KegStatusResponseModel();

            string url = string.Format(Configuration.GetKegStatusByKegIdUrl, kegId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            kegStatusResponseModel = DeserializeObject<KegStatusResponseModel>(value.Response, GetJsonSetting());
            kegStatusResponseModel.Response.StatusCode = value.StatusCode;
            return kegStatusResponseModel;
        }

        public async Task<SearchPalletModel> GetPalletSearchAsync(string sessionId, string locationId, string fromDate, string toDate, string kegs, string kegOwnerId)
        {
            SearchPalletModel model = new SearchPalletModel();

            string url = string.Format(Configuration.GetPalletSearchUrl, sessionId, locationId, fromDate, toDate, kegs, kegOwnerId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.SearchPalletResponseModel = DeserializeObject<IList<SearchPalletResponseModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;
            return model;
        }

        public async Task<PartnerInfoResponseModel> GetPartnerInfoAsync(string sessionId, string partnerId)
        {
            PartnerInfoResponseModel partnerInfoResponseModel = new PartnerInfoResponseModel();

            string url = string.Format(Configuration.GetPartnerInfoByPartnerIdUrl, sessionId, partnerId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            partnerInfoResponseModel = DeserializeObject<PartnerInfoResponseModel>(value.Response, GetJsonSetting());
            partnerInfoResponseModel.Response.StatusCode = value.StatusCode;
            return partnerInfoResponseModel;
        }

        public async Task<object> PostKegAsync(KegRequestModel model, string sessionId, string RequestType)
        {
            //ManifestModelGet manifestModelGet = new ManifestModelGet();

            string url = string.Format(Configuration.PostKegUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            //manifestModelGet = value.Response != null ? DeserializeObject<ManifestModelGet>(value.Response, GetJsonSetting()) : new ManifestModelGet();
            //manifestModelGet.StatusCode = value.StatusCode;
            //return manifestModelGet;
            return null;
        }

        public async Task<KegMassUpdateKegModel> PostKegUploadAsync(KegBulkUpdateItemRequestModel model, string sessionId, string RequestType)
        {
            KegMassUpdateKegModel resultModel = new KegMassUpdateKegModel();

            string url = string.Format(Configuration.PostKegsUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            resultModel.Model = value.Response != null ? DeserializeObject<IList<KegMassUpdateKegResponseModel>>(value.Response, GetJsonSetting()) : new List<KegMassUpdateKegResponseModel>();
            resultModel.Response.StatusCode = value.StatusCode;
            return resultModel;
        }

        public async Task<AddMaintenanceAlertModel> PostMaintenanceAlertAsync(AddMaintenanceAlertRequestModel model, string sessionId, string RequestType)
        {
            AddMaintenanceAlertModel responseModel = new AddMaintenanceAlertModel();

            string url = string.Format(Configuration.PostMaintenanceAlertUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);
            var result  = value.Response != null ? DeserializeObject<IList<AddMaintenanceAlertResponseModel>>(value.Response, GetJsonSetting()) : new List<AddMaintenanceAlertResponseModel>();

            foreach (var item in result)
                responseModel.AddMaintenanceAlertResponseModel.Add(item);

            responseModel.Response.StatusCode = value.StatusCode;
            return responseModel;
        }

        public async Task<AddMaintenanceAlertModel> PostMaintenanceDeleteAlertUrlAsync(DeleteMaintenanceAlertRequestModel model, string sessionId, string RequestType)
        {
            AddMaintenanceAlertModel responseModel = new AddMaintenanceAlertModel();

            string url = string.Format(Configuration.PostMaintenanceDeleteAlertUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);
            var result = value.Response != null ? DeserializeObject<IList<AddMaintenanceAlertResponseModel>>(value.Response, GetJsonSetting()) : new List<AddMaintenanceAlertResponseModel>();
            foreach (var item in result)
                responseModel.AddMaintenanceAlertResponseModel.Add(item);

            responseModel.Response.StatusCode = value.StatusCode;
            return responseModel;
        }
    }
}
