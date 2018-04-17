using System.Collections.Generic;
using System.Threading.Tasks;
using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;
using static KegID.Common.Helper;

namespace KegID.Services
{
    public class MoveService : IMoveService
    {
        public async Task<BrandResponseModel> GetBrandListAsync(string sessionId)
        {
            BrandResponseModel brandResponseModel = new BrandResponseModel();

            string url = string.Format(Configuration.GetBrandUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            brandResponseModel.BrandModel = DeserializeObject<IList<BrandModel>>(value.Response, GetJsonSetting());
            brandResponseModel.StatusCode = value.StatusCode;

            return brandResponseModel;
        }

        public async Task<ManifestResponseModel> GetManifestAsync(string sessionId,string manifestId)
        {
            ManifestResponseModel manifestResponseModel = new ManifestResponseModel();

            string url = string.Format(Configuration.GetManifestUrl, manifestId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            manifestResponseModel = !string.IsNullOrEmpty(value.Response) ? DeserializeObject<ManifestResponseModel>(value.Response, GetJsonSetting()) : manifestResponseModel;
            manifestResponseModel.StatusCode = value.StatusCode;

            return manifestResponseModel;
        }

        public async Task<PartnerResponseModel> GetPartnersListAsync(string sessionId)
        {
            PartnerResponseModel partnerResponseModel = new PartnerResponseModel();

            string url = string.Format(Configuration.GetPartnerBySesssionIdUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            partnerResponseModel.PartnerModel = DeserializeObject<IList<PartnerModel>>(value.Response, GetJsonSetting());
            partnerResponseModel.StatusCode = value.StatusCode;

            return partnerResponseModel;
        }

        public async Task<PartnerTypeResponseModel> GetPartnerTypeAsync(string sessionId)
        {
            PartnerTypeResponseModel partnerTypeResponseModel = new PartnerTypeResponseModel();

            string url = string.Format(Configuration.GetPartnerTypeUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            partnerTypeResponseModel.PartnerTypeModel = DeserializeObject<IList<PartnerTypeModel>>(value.Response, GetJsonSetting());
            partnerTypeResponseModel.StatusCode = value.StatusCode;

            return partnerTypeResponseModel;
        }

        public async Task<ValidateBarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode)
        {
            ValidateBarcodeModel validateBarcodeModel = new ValidateBarcodeModel();

            string url = string.Format(Configuration.GetValidateBarcodeUrl, barcode, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            validateBarcodeModel = value.Response!= null? DeserializeObject<ValidateBarcodeModel>(value.Response, GetJsonSetting()) : new ValidateBarcodeModel();
            validateBarcodeModel.StatusCode = value.StatusCode;

            return validateBarcodeModel;
        }

        public async Task<PartnerResponseModel> GetPartnerSearchAsync(string sessionId, string search, bool internalonly, bool includepublic)
        {
            PartnerResponseModel partnerResponseModel = new PartnerResponseModel();

            string url = string.Format(Configuration.GetPartnerSearchUrl, sessionId, search, internalonly, includepublic);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            partnerResponseModel.PartnerModel = DeserializeObject<IList<PartnerModel>>(value.Response, GetJsonSetting());
            partnerResponseModel.StatusCode = value.StatusCode;

            return partnerResponseModel;
        }

        public async Task<IList<string>> GetAssetSizeAsync(string sessionId, bool assignableOnly)
        {
            string url = string.Format(Configuration.GetAssetSize, sessionId, assignableOnly);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);
            return DeserializeObject<IList<string>>(value.Response, GetJsonSetting());
        }

        public async Task<IList<string>> GetAssetTypeAsync(string sessionId, bool assignableOnly)
        {
            string url = string.Format(Configuration.GetAssetType, sessionId, assignableOnly);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);
            return DeserializeObject<IList<string>>(value.Response, GetJsonSetting());
        }

        public async Task<ManifestSearchModel> GetManifestSearchAsync(string sessionId, string trackingNumber,string barcode, string senderId, string destinationId, string referenceKey,string fromDate, string toDate)
        {
            ManifestSearchModel manifestSearchModel = new ManifestSearchModel();

            string url = string.Format(Configuration.GetManifestSearchUrl, sessionId, trackingNumber, barcode, senderId, destinationId, referenceKey, fromDate, toDate);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            manifestSearchModel.ManifestSearchResponseModel = DeserializeObject<IList<ManifestSearchResponseModel>>(value.Response, GetJsonSetting());
            manifestSearchModel.StatusCode = value.StatusCode;

            return manifestSearchModel;
        }

        public async Task<ManifestModelGet> PostManifestAsync(ManifestModel model, string sessionId, string RequestType)
        {
            ManifestModelGet manifestModelGet = new ManifestModelGet();

            string url = string.Format(Configuration.PostManifestUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            manifestModelGet = value.Response != null? DeserializeObject<ManifestModelGet>(value.Response, GetJsonSetting()) : new ManifestModelGet();
            manifestModelGet.StatusCode = value.StatusCode;
            return manifestModelGet;
        }

        //public async Task<ManifestModelGet> PostManifestAsync(ManifestRequestModel model, string sessionId, string RequestType)
        //{
        //    ManifestModelGet manifestModelGet = new ManifestModelGet();

        //    string url = string.Format(Configuration.PostManifestUrl, sessionId);
        //    string content = JsonConvert.SerializeObject(model);
        //    var value = await Helper.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

        //    manifestModelGet = Helper.DeserializeObject<ManifestModelGet>(value.Response);
        //    manifestModelGet.StatusCode = value.StatusCode;
        //    return manifestModelGet;
        //}

        public async Task<NewPartnerResponseModel> PostNewPartnerAsync(NewPartnerRequestModel model, string sessionId, string RequestType)
        {
            NewPartnerResponseModel partnerResponseModel = new NewPartnerResponseModel();

            string url = string.Format(Configuration.PostNewPartnerUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);

            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            partnerResponseModel = DeserializeObject<NewPartnerResponseModel>(value.Response, GetJsonSetting());
            partnerResponseModel.StatusCode = value.StatusCode;
            return partnerResponseModel;
        }

       
    }
}
