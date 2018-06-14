﻿using System.Threading.Tasks;
using KegID.Common;
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

            palletResponseModel = DeserializeObject<PalletResponseModel>(value.Response, GetJsonSetting());
            palletResponseModel.Response.StatusCode = value.StatusCode;
            return palletResponseModel;
        }
    }
}
