//using System.Collections.Generic;
//using System.Threading.Tasks;
//using KegID.Common;
//using KegID.Model;

//namespace KegID.Services
//{
//    public class FillService : IFillService
//    {
//        public async Task<BatchResponseModel> GetBatchListAsync(string sessionId)
//        {
//            BatchResponseModel model = new BatchResponseModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetBatchUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.BatchModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<NewBatch>>(value.Response) : new List<NewBatch>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<SkuModel> GetSkuListAsync(string sessionId)
//        {
//            SkuModel model = new SkuModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetSkuUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.Sku = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<Sku>>(value.Response) : new List<Sku>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }
//    }
//}
