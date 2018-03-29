using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class FillService : IFillService
    {
        public async Task<BatchResponseModel> GetBatchListAsync(string sessionId)
        {
            BatchResponseModel batchResponseModel = new BatchResponseModel();

            string url = string.Format(Configuration.GetBatchUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            batchResponseModel.BatchModel = DeserializeObject<IList<BatchModel>>(value.Response, GetJsonSetting());
            batchResponseModel.StatusCode = value.StatusCode;
            return batchResponseModel;
        }

        //public async Task<object> PostBatchAsync(BatchRequestModel model, string sessionId, string RequestType)
        //{
        //    object obj = new object();

        //    string url = string.Format(Configuration.PostBatchUrl, sessionId);
        //    string content = JsonConvert.SerializeObject(model);
        //    var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

        //    obj = Helper.DeserializeObject<object>(value.Response);
        //    //obj.StatusCode = value.StatusCode;
        //    return obj;
        //}
    }
}
