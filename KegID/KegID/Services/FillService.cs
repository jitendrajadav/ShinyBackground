using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;

namespace KegID.Services
{
    public class FillService : IFillService
    {
        public async Task<IList<BatchModel>> GetBatchListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetBatchUrl, sessionId);
            return await Helper.ExecuteServiceCall<IList<BatchModel>>(url, HttpMethodType.Get, string.Empty);
        }
    }
}
