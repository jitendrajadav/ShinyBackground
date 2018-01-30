using KegID.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IFillService
    {
        Task<IList<BatchModel>> GetBatchListAsync(string sessionId);
    }
}
