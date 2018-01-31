using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;

namespace KegID.Services
{
    public class MoveService : IMoveService
    {
        public async Task<BrandResponseModel> GetBrandListAsync(string sessionId)
        {
            BrandResponseModel brandResponseModel = new BrandResponseModel();

            string url = string.Format(Configuration.GetBrandUrl, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            brandResponseModel.BrandModel = Helper.DeserializeObject<IList<BrandModel>>(value.Response);
            brandResponseModel.StatusCode = value.StatusCode;

            return brandResponseModel;
        }

        public async Task<ManifestResponseModel> GetManifestAsync(string sessionId,string manifestId)
        {
            ManifestResponseModel manifestResponseModel = new ManifestResponseModel();

            string url = string.Format(Configuration.GetManifestUrl, manifestId, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            manifestResponseModel = !string.IsNullOrEmpty(value.Response) ? Helper.DeserializeObject<ManifestResponseModel>(value.Response) : manifestResponseModel;
            manifestResponseModel.StatusCode = value.StatusCode;

            return manifestResponseModel;
        }

        public async Task<PartnerResponseModel> GetPartnersListAsync(string sessionId)
        {
            PartnerResponseModel partnerResponseModel = new PartnerResponseModel();

            string url = string.Format(Configuration.GetPartnerUrl, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            partnerResponseModel.PartnerModel = Helper.DeserializeObject<IList<PartnerModel>>(value.Response);
            partnerResponseModel.StatusCode = value.StatusCode;

            return partnerResponseModel;
        }

        public async Task<PartnerTypeResponseModel> GetPartnerTypeAsync(string sessionId)
        {
            PartnerTypeResponseModel partnerTypeResponseModel = new PartnerTypeResponseModel();

            string url = string.Format(Configuration.GetPartnerTypeUrl, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            partnerTypeResponseModel.PartnerTypeModel = Helper.DeserializeObject<IList<PartnerTypeModel>>(value.Response);
            partnerTypeResponseModel.StatusCode = value.StatusCode;

            return partnerTypeResponseModel;
        }

        public async Task<ValidateBarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode)
        {
            ValidateBarcodeModel validateBarcodeModel = new ValidateBarcodeModel();

            string url = string.Format(Configuration.GetValidateBarcodeUrl, barcode, sessionId);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            validateBarcodeModel = Helper.DeserializeObject<ValidateBarcodeModel>(value.Response);
            validateBarcodeModel.StatusCode = value.StatusCode;

            return validateBarcodeModel;
        }

        public async Task<PartnerResponseModel> GetPartnerSearchAsync(string sessionId, string search, bool internalonly, bool includepublic)
        {
            PartnerResponseModel partnerResponseModel = new PartnerResponseModel();

            string url = string.Format(Configuration.GetPartnerSearchUrl, sessionId, search, internalonly, includepublic);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            partnerResponseModel.PartnerModel = Helper.DeserializeObject<IList<PartnerModel>>(value.Response);
            partnerResponseModel.StatusCode = value.StatusCode;

            return partnerResponseModel;
        }

        public async Task<ManifestModelGet> PostManifestAsync(ManifestModel model, string sessionId, string RequestType)
        {
            ManifestModelGet manifestModelGet = new ManifestModelGet();

            string url = string.Format(Configuration.PostManifestUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            manifestModelGet = Helper.DeserializeObject<ManifestModelGet>(value.Response);
            manifestModelGet.StatusCode = value.StatusCode;
            return manifestModelGet;
        }

        public async Task<NewPartnerResponseModel> PostNewPartnerAsync(NewPartnerRequestModel model, string sessionId, string RequestType)
        {
            NewPartnerResponseModel partnerResponseModel = new NewPartnerResponseModel();

            string url = string.Format(Configuration.PostNewPartnerUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);

            var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);
            partnerResponseModel = Helper.DeserializeObject<NewPartnerResponseModel>(value.Response);
            partnerResponseModel.StatusCode = value.StatusCode;
            return partnerResponseModel;
        }
    }
}
