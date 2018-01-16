using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Response;

namespace KegID.Services
{
    public class MoveService : IMoveService
    {
        public async Task<IList<BrandModel>> GetBrandListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetBrandUrl, sessionId);
            return await Helper.ExecutePostCall<IList<BrandModel>>(url, HttpMethodType.Get, string.Empty);
        }

        public async Task<IList<ManifestModel>> GetManifestListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetManifestUrl, sessionId);
            return await Helper.ExecutePostCall<IList<ManifestModel>>(url, HttpMethodType.Get, string.Empty);
        }

        public async Task<IList<PartnerModel>> GetPartnersListAsync(string sessionId)
        {
            string url = string.Format(Configuration.GetPartnerUrl, sessionId);
            return await Helper.ExecutePostCall<IList<PartnerModel>>(url, HttpMethodType.Get, string.Empty);
        }

        public async Task<ValidateBarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode)
        {
            string url = string.Format(Configuration.GetValidateBarcodeUrl, barcode, sessionId);
            return await Helper.ExecutePostCall<ValidateBarcodeModel>(url, HttpMethodType.Get, string.Empty);
        }
    }
}
