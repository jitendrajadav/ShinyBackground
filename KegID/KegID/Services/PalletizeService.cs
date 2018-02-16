using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Model;
using Newtonsoft.Json;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class PalletizeService : IPalletizeService
    {
        public async Task<PalletResponseModel> PostPalletAsync(PalletRequestModel model, string sessionId, string RequestType)
        {
            PalletResponseModel palletResponseModel = new PalletResponseModel();

            string url = string.Format(Configuration.PostPalletUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            palletResponseModel = DeserializeObject<PalletResponseModel>(value.Response, settings);
            palletResponseModel.StatusCode = value.StatusCode;
            return palletResponseModel;
        }
    }
}
