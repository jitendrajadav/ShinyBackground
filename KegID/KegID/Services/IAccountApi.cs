using KegID.Model;
using Refit;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KegID.Services
{
    [Headers("Content-Type: application/json")]
    public interface IAccountApi
    {
        [Get("/login/get?username={username}&password={password}")]
        Task<HttpResponseMessage> GetAuthenticate(string username, string password, CancellationToken cancellationToken);

        [Post("/DeviceCheckin/?sessionId={sessionId}")]
        [Headers("Request-type : DeviceCheckin")]
        Task<HttpResponseMessage> PostDeviceCheckin([Body(BodySerializationMethod.Serialized)] DeviceCheckinRequestModel model, string sessionId, CancellationToken cancellationToken);

    }
}
