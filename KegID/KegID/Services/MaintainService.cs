using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class MaintainService : IMaintainService
    {
        public async Task<MaintainTypeModel> GetMaintainTypeAsync(string sessionId)
        {
            MaintainTypeModel maintainTypeModel = new MaintainTypeModel();

            string url = string.Format(Configuration.GetMaintenanceTypeUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            maintainTypeModel.MaintainTypeReponseModel = DeserializeObject<IList<MaintainTypeReponseModel>>(value.Response, GetJsonSetting());
            maintainTypeModel.Response.StatusCode = value.StatusCode;

            return maintainTypeModel;
        }

        public async Task<KegIDResponse> PostMaintenanceDoneAsync(MaintenanceDoneRequestModel model, string sessionId, string RequestType)
        {
            string url = string.Format(Configuration.PostMaintenanceDoneUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            return value;
        }
    }
}
