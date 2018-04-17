﻿using KegID.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IDashboardService
    {
        Task<DashboardResponseModel> GetDeshboardDetailAsync(string sessionId);
        Task<InventoryDetailModel> GetInventoryAsync(string sessionId);
        Task<KegPossessionModel> GetKegPossessionAsync(string sessionId, string partnerId);
        Task<PartnerInfoResponseModel> GetPartnerInfoAsync(string sessionId, string partnerId);
        Task<KegStatusResponseModel> GetKegStatusAsync(string kegId, string sessionId);
        Task<KegMaintenanceHistoryModel> GetKegMaintenanceHistoryAsync(string kegId, string sessionId);
        Task<MaintenanceAlertModel> GetKegMaintenanceAlertAsync(string kegId, string sessionId);
        Task<DeleteMaintenanceAlertResponseModel> GetDeleteMaintenanceAlertAsync(string kegId, string sessionId);
        Task<object> PostKegAsync(KegRequestModel model, string sessionId, string RequestType);
        Task<AddMaintenanceAlertModel> PostMaintenanceAlertAsync(AddMaintenanceAlertRequestModel model, string sessionId, string RequestType);
        Task<AddMaintenanceAlertModel> PostMaintenanceDeleteAlertUrlAsync(DeleteMaintenanceAlertRequestModel model, string sessionId, string RequestType);
        Task<SearchPalletModel> GetPalletSearchAsync(string sessionId, string locationId, string fromDate, string toDate, string kegs, string kegOwnerId);
        Task<KegSearchModel> GetKegSearchAsync(string sessionId, string barcode, bool includePartials);
        Task<IList<string>> GetAssetVolumeAsync(string sessionId, bool assignableOnly);

        Task<KegMassUpdateKegModel> PostKegUploadAsync(KegBulkUpdateItemRequestModel model, string sessionId, string RequestType);
    }
}
