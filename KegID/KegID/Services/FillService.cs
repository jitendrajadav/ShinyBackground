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
            BatchResponseModel model = new BatchResponseModel
            {
                Response = new KegIDResponse()
            };
            try
            {
                string url = string.Format(Configuration.GetBatchUrl, sessionId);
                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

                model.BatchModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<NewBatch>>(value.Response) : new List<NewBatch>();
                model.Response.StatusCode = value.StatusCode;
            }
            catch (System.Exception)
            {

            }
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
