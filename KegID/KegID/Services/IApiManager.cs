using KegID.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IApiManager
    {
        #region Account Api

        Task<HttpResponseMessage> GetAuthenticate(string username, string password);
        Task<HttpResponseMessage> PostDeviceCheckin(DeviceCheckinRequestModel model, string sessionId);

        #endregion

        #region Dashboard Api

        Task<HttpResponseMessage> GetDashboardPartnersList(string ownerId, string sessionId);
        Task<HttpResponseMessage> GetDeshboardDetail(string sessionId);
        Task<HttpResponseMessage> GetInventory(string sessionId);
        Task<HttpResponseMessage> GetKegPossession(string sessionId, string partnerId);
        Task<HttpResponseMessage> GetPartnerInfo(string sessionId, string partnerId);
        Task<HttpResponseMessage> GetKegStatus(string kegId, string sessionId);
        Task<HttpResponseMessage> GetKegMaintenanceHistory(string kegId, string sessionId);
        Task<HttpResponseMessage> GetKegMaintenanceAlert(string kegId, string sessionId);
        Task<HttpResponseMessage> GetDeletedMaintenanceAlert(string kegId, string sessionId);
        Task<HttpResponseMessage> GetPalletSearch(string sessionId, string locationId, string fromDate, string toDate, string kegs, string kegOwnerId);
        Task<HttpResponseMessage> GetKegSearch(string sessionId, string barcode, bool includePartials);
        Task<HttpResponseMessage> GetAssetVolume(string sessionId, bool assignableOnly);
        Task<HttpResponseMessage> GetOperators(string sessionId);
        Task<HttpResponseMessage> PostKegStatus(KegRequestModel model, string kegId, string sessionId);
        Task<HttpResponseMessage> PostKeg(KegRequestModel model, string kegId, string sessionId);
        Task<HttpResponseMessage> PostMaintenanceAlert(AddMaintenanceAlertRequestModel model, string sessionId);
        Task<HttpResponseMessage> PostMaintenanceDeleteAlertUrl(DeleteMaintenanceAlertRequestModel model, string sessionId);
        Task<HttpResponseMessage> PostKegUpload(KegBulkUpdateItemRequestModel model, string sessionId);

        #endregion

        #region Fill Api

        Task<HttpResponseMessage> GetBatchList(string sessionId);
        Task<HttpResponseMessage> GetSkuList(string sessionId);

        #endregion

        #region Maintain Api

        Task<HttpResponseMessage> GetMaintainType(string sessionId);
        Task<HttpResponseMessage> PostMaintenanceDone(MaintenanceDoneRequestModel model, string sessionId);

        #endregion

        #region Move Api

        Task<HttpResponseMessage> GetOwner(string sessionId);
        Task<HttpResponseMessage> GetPartnersList(string sessionId);
        Task<HttpResponseMessage> GetValidateBarcode(string barcode, string sessionId);
        Task<HttpResponseMessage> GetBrandList(string sessionId);
        Task<HttpResponseMessage> GetManifest(string manifestId, string sessionId);
        Task<HttpResponseMessage> GetPartnerType(string sessionId);
        Task<HttpResponseMessage> GetPartnerSearch(string sessionId, string search, bool internalonly, bool includepublic);
        Task<HttpResponseMessage> GetManifestSearch(string sessionId, string trackingNumber, string barcode, string senderId, string destinationId, string referenceKey, string fromDate, string toDate);
        Task<HttpResponseMessage> GetAssetSize(string sessionId, bool assignableOnly);
        Task<HttpResponseMessage> GetAssetType(string sessionId, bool assignableOnly);
        Task<HttpResponseMessage> PostManifest(ManifestModel model, string sessionId);
        Task<HttpResponseMessage> PostNewPartner(NewPartnerRequestModel model, string sessionId);

        #endregion

        #region Pallet Api

        Task<HttpResponseMessage> PostPallet(PalletRequestModel model, string sessionId);

        #endregion
    }
}
