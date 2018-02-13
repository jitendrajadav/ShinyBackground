using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;

namespace KegID.Services
{
    public class PalletizeService : IPalletizeService
    {
        public async Task<object> PostPalletAsync(PalletRequestModel model, string sessionId, string RequestType)
        {
            object obj = new object();

            string url = string.Format(Configuration.NewPallet, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            obj = Helper.DeserializeObject<object>(value.Response);
            //obj.StatusCode = value.StatusCode;
            return obj;
        }
    }
}
