using KegID.Model;
using Refit;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KegID.Services
{
    [Headers("Content-Type: application/json")]
    public interface IDashboardApi
    {
        [Get("/Possessor?ownerId={ownerId}&sessionId={sessionId}")]
        Task<HttpResponseMessage> GetDashboardPartnersList(string ownerId, string sessionId, CancellationToken cancellationToken);

        [Get("/dashboard/get?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetDeshboardDetail(string sessionId, CancellationToken cancellationToken);

        [Get("/Inventory/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetInventory(string sessionId, CancellationToken cancellationToken);

        [Get("/KegPossession/?sessionId={sessionId}&partnerid={partnerId}")]
        Task<HttpResponseMessage> GetKegPossession(string sessionId, string partnerId, CancellationToken cancellationToken);

        [Get("/Partner/?sessionId={sessionId}&id={partnerId}")]
        Task<HttpResponseMessage> GetPartnerInfo(string sessionId, string partnerId, CancellationToken cancellationToken);

        [Get("/Keg/{kegId}?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetKegStatus(string kegId, string sessionId, CancellationToken cancellationToken);

        [Get("/KegMaintenanceHistory/{kegId}?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetKegMaintenanceHistory(string kegId, string sessionId, CancellationToken cancellationToken);

        [Get("/MaintenanceAlert?kegId={kegId}&sessionId={sessionId}")]
        Task<HttpResponseMessage> GetKegMaintenanceAlert(string kegId, string sessionId, CancellationToken cancellationToken);

        [Get("/MaintenanceAlert?kegId={kegId}&sessionId={sessionId}")]
        Task<HttpResponseMessage> GetDeletedMaintenanceAlert(string kegId, string sessionId, CancellationToken cancellationToken);

        [Get("/PalletSearch?sessionId={sessionId}&locationId={locationId}&fromDate={fromDate}&toDate={toDate}&kegs={kegs}&kegOwnerId={kegOwnerId}")]
        Task<HttpResponseMessage> GetPalletSearch(string sessionId, string locationId, string fromDate, string toDate, string kegs, string kegOwnerId, CancellationToken cancellationToken);

        [Get("/Keg/?sessionId={sessionId}&barcode={barcode}&includePartials={includePartials}")]
        Task<HttpResponseMessage> GetKegSearch(string sessionId, string barcode, bool includePartials, CancellationToken cancellationToken);

        [Get("/AssetVolume/?sessionId={sessionId}&assignableOnly={assignableOnly}")]
        Task <HttpResponseMessage> GetAssetVolume(string sessionId, bool assignableOnly, CancellationToken cancellationToken);

        [Get("/Operators/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetOperators(string sessionId, CancellationToken cancellationToken);


        [Post("/Keg/{kegId}?sessionId={sessionId}")]
        [Headers("Request-type : Keg")]
        Task<HttpResponseMessage> PostKegStatus([Body(BodySerializationMethod.Serialized)] KegRequestModel model, string kegId, string sessionId, CancellationToken cancellationToken);

        [Post("/Keg/?sessionId={sessionId}")]
        [Headers("Request-type : Keg")]
        Task<HttpResponseMessage> PostKeg([Body(BodySerializationMethod.Serialized)] KegRequestModel model, string kegId, string sessionId, CancellationToken cancellationToken);

        [Post("/MaintenanceAlert/?sessionId={sessionId}")]
        [Headers("Request-type : PostedMaintenanceAlert")]
        Task<HttpResponseMessage> PostMaintenanceAlert([Body(BodySerializationMethod.Serialized)] AddMaintenanceAlertRequestModel model, string sessionId, CancellationToken cancellationToken);

        [Post("/MaintenanceAlert/DeleteAlert/?sessionId={sessionId}")]
        [Headers("Request-type : DeleteType")]
        Task<HttpResponseMessage> PostMaintenanceDeleteAlertUrl([Body(BodySerializationMethod.Serialized)] DeleteMaintenanceAlertRequestModel model, string sessionId, CancellationToken cancellationToken);

        [Post("/Kegs/?sessionId={sessionId}")]
        [Headers("Request-type : MassUpdateKegList")]
        Task<HttpResponseMessage> PostKegUpload([Body(BodySerializationMethod.Serialized)] KegBulkUpdateItemRequestModel model, string sessionId, CancellationToken cancellationToken);
    }
}
