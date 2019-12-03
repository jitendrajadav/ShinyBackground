using KegID.Model;
using Refit;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KegID.Services
{
    [Headers("Content-Type: application/json")]
    public interface IPalletApi
    {
        [Post("/Pallet/?sessionId={sessionId}")]
        [Headers("Request-type : NewPallet")]
        Task<HttpResponseMessage> PostPallet([Body(BodySerializationMethod.Serialized)] PalletRequestModel model, string sessionId, CancellationToken cancellationToken);
    }
}
