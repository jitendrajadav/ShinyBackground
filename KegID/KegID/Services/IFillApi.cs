using Refit;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace KegID.Services
{
    [Headers("Content-Type: application/json")]
    public interface IFillApi
    {
        [Get("/batch/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetBatchList(string sessionId, CancellationToken cancellationToken);

        [Get("/sku/?sessionId={sessionId}")]
        Task<HttpResponseMessage> GetSkuList(string sessionId, CancellationToken cancellationToken);
    }
}
