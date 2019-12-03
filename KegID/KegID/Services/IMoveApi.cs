using KegID.Model;
using Refit;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KegID.Services
{
    [Headers("Content-Type: application/json")]
    public interface IMoveApi
    {
        [Get("/Owner/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetOwner(string sessionId, CancellationToken cancellationToken);

        [Get("/Partner/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetPartnersList(string sessionId, CancellationToken cancellationToken);

        [Get("/validation/?barcode={barcode}&sessionId={sessionId}")]
        Task<HttpResponseMessage> GetValidateBarcode(string barcode, string sessionId, CancellationToken cancellationToken);

        [Get("/brand/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetBrandList(string sessionId, CancellationToken cancellationToken);

        [Get("/Manifest/{manifestId}?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetManifest(string manifestId, string sessionId, CancellationToken cancellationToken);

        [Get("/PartnerType/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetPartnerType(string sessionId, CancellationToken cancellationToken);

        [Get("/PartnerSearch/?sessionId={sessionId}&search={search}&internalonly={internalonly}&includepublic={includepublic}")]
        Task<HttpResponseMessage> GetPartnerSearch(string sessionId, string search, bool internalonly, bool includepublic, CancellationToken cancellationToken);

        [Get("/ManifestSearch/?sessionId={sessionId}&trackingNumber={trackingNumber}&kegs={barcode}&senderId={senderId}&destinationId={destinationId}&referenceKey={referenceKey}&fromDate={fromDate}&toDate={toDate}")]
        Task<HttpResponseMessage> GetManifestSearch(string sessionId, string trackingNumber, string barcode, string senderId, string destinationId, string referenceKey, string fromDate, string toDate, CancellationToken cancellationToken);

        [Get("/AssetSize/?sessionId={sessionId}&assignableOnly={assignableOnly}")]
        Task<HttpResponseMessage> GetAssetSize(string sessionId, bool assignableOnly, CancellationToken cancellationToken);

        [Get("/AssetType/?sessionId={sessionId}&assignableOnly={assignableOnly}")]
        Task<HttpResponseMessage> GetAssetType(string sessionId, bool assignableOnly, CancellationToken cancellationToken);

        [Post("/Manifest/?sessionId={sessionId}")]
        [Headers("Request-type : NewManifest")]
        Task<HttpResponseMessage> PostManifest([Body(BodySerializationMethod.Serialized)] ManifestModel model, string sessionId, CancellationToken cancellationToken);

        [Post("/Partner/?sessionId={sessionId}")]
        [Headers("Request-type : NewPartner")]
        Task<HttpResponseMessage> PostNewPartner([Body(BodySerializationMethod.Serialized)] NewPartnerRequestModel model, string sessionId, CancellationToken cancellationToken);
    }
}
