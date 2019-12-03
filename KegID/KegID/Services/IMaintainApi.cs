using KegID.Model;
using Refit;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KegID.Services
{
    [Headers("Content-Type: application/json")]
    public interface IMaintainApi
    {
        [Get("/MaintenanceType/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetMaintainType(string sessionId, CancellationToken cancellationToken);

        [Post("/MaintenanceDone/?sessionId={sessionId}")]
        [Headers("Request-type : PostedMaintenanceDone")]
        Task<HttpResponseMessage> PostMaintenanceDone([Body(BodySerializationMethod.Serialized)] MaintenanceDoneRequestModel model, string sessionId, CancellationToken cancellationToken);
    }
}
