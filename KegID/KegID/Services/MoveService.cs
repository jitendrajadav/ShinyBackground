using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Response;
using Newtonsoft.Json;

namespace KegID.Services
{
    public class MoveService : IMoveService
    {
        public async Task<IList<BrandModel>> GetBrandListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetBrandUrl, sessionId);
            return await Helper.ExecuteServiceCall<IList<BrandModel>>(url, HttpMethodType.Get, string.Empty);
        }

        public async Task<IList<ManifestModelGet>> GetManifestListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetManifestUrl, sessionId);
            return await Helper.ExecuteServiceCall<IList<ManifestModelGet>>(url, HttpMethodType.Get, string.Empty);
        }

        public async Task<IList<PartnerModel>> GetPartnersListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetPartnerUrl, sessionId);
            return await Helper.ExecuteServiceCall<IList<PartnerModel>>(url, HttpMethodType.Get, string.Empty);
        }

        public async Task<ValidateBarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode)
        {
            string url = string.Format(Configuration.GetValidateBarcodeUrl, barcode, sessionId);
            return await Helper.ExecuteServiceCall<ValidateBarcodeModel>(url, HttpMethodType.Get, string.Empty);
        }

        public async Task<ManifestModelGet> PostManifestAsync(ManifestModel model, string sessionId)
        {
            string url = string.Format(Configuration.PostManifestUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);

            return await Helper.ExecuteServiceCall<ManifestModelGet>(url, HttpMethodType.Send, content);
        }
    }
}
