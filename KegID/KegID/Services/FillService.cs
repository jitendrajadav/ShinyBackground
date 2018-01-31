using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;

namespace KegID.Services
{
    public class FillService : IFillService
    {
        public async Task<BatchResponseModel> GetBatchListAsync(string sessionId)
        {
            BatchResponseModel batchResponseModel = new BatchResponseModel();

            string url = string.Format(Configuration.GetBatchUrl, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            batchResponseModel.BatchModel = Helper.DeserializeObject<IList<BatchModel>>(value.Response);
            batchResponseModel.StatusCode = value.StatusCode;
            return batchResponseModel;
        }
    }
}
