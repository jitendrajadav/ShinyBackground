using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Response;

namespace KegID.Services
{
    public class FillService : IFillService
    {
        public async Task<IList<BatchModel>> GetBatchListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetBatchUrl, sessionId);
            return await Helper.ExecutePostCall<IList<BatchModel>>(url, HttpMethodType.Get, string.Empty);
        }
    }
}
