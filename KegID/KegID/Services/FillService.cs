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
            BatchResponseModel model = new BatchResponseModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetBatchUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.BatchModel = DeserializeObject<IList<BatchModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;
            return model;
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
