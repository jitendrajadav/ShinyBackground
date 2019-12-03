//using System.Collections.Generic;
//using System.Threading.Tasks;
//using KegID.Common;
//using KegID.Model;
//using Newtonsoft.Json;

//namespace KegID.Services
//{
//    public class MaintainService : IMaintainService
//    {
//        public async Task<MaintainTypeModel> GetMaintainTypeAsync(string sessionId)
//        {
//            MaintainTypeModel model = new MaintainTypeModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetMaintenanceTypeUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.MaintainTypeReponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<MaintainTypeReponseModel>>(value.Response) : new List<MaintainTypeReponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<KegIDResponse> PostMaintenanceDoneAsync(MaintenanceDoneRequestModel model, string sessionId, string RequestType)
//        {
//            KegIDResponse outModel = null;
//            try
//            {
//                string url = string.Format(Configuration.PostMaintenanceDoneUrl, sessionId);
//                string content = JsonConvert.SerializeObject(model);
//                outModel = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);
//            }
//            catch (System.Exception)
//            {

//            }
//            return outModel;
//        }
//    }
//}
