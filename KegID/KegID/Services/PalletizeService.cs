//using System.Threading.Tasks;
//using KegID.Common;
//using KegID.Model;
//using Newtonsoft.Json;

//namespace KegID.Services
//{
//    public class PalletizeService : IPalletizeService
//    {
//        public async Task<PalletResponseModel> PostPalletAsync(PalletRequestModel inModel, string sessionId, string RequestType)
//        {
//            PalletResponseModel outModel = null;
//            try
//            {
//                string url = string.Format(Configuration.PostPalletUrl, sessionId);
//                string content = JsonConvert.SerializeObject(inModel);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

//                outModel = value.Response != null ? App.kegIDClient.DeserializeObject<PalletResponseModel>(value.Response) : new PalletResponseModel();
//                if (outModel != null)
//                {
//                    outModel.Response = new KegIDResponse
//                    {
//                        StatusCode = value.StatusCode
//                    };
//                }
//            }
//            catch (System.Exception)
//            {

//            }
//            return outModel;
//        }
//    }
//}
