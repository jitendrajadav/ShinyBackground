using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class PalletizeService : IPalletizeService
    {
        public async Task<PalletResponseModel> PostPalletAsync(PalletRequestModel inModel, string sessionId, string RequestType)
        {
            string url = string.Format(Configuration.PostPalletUrl, sessionId);
            string content = JsonConvert.SerializeObject(inModel);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            var outModel = DeserializeObject<PalletResponseModel>(value.Response, GetJsonSetting());
            if (outModel != null)
            {
                outModel.Response = new KegIDResponse
                {
                    StatusCode = value.StatusCode
                };
            }
            return outModel;
        }
    }
}
