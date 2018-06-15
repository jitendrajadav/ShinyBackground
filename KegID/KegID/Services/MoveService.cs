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
        public async Task<OwnerResponseModel> GetOwnerAsync(string sessionId)
        {
            OwnerResponseModel model = new OwnerResponseModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetOwner, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.OwnerModel = DeserializeObject<IList<OwnerModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;

            return model;
        }

        public async Task<BrandResponseModel> GetBrandListAsync(string sessionId)
        {
            BrandResponseModel model = new BrandResponseModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetBrandUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.BrandModel = DeserializeObject<IList<BrandModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;

            return model;
        }

        public async Task<ManifestResponseModel> GetManifestAsync(string sessionId,string manifestId)
        {
            string url = string.Format(Configuration.GetManifestUrl, manifestId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var model = !string.IsNullOrEmpty(value.Response) ? DeserializeObject<ManifestResponseModel>(value.Response, GetJsonSetting()) : new ManifestResponseModel();
            if (model != null)
            {
                model.Response = new KegIDResponse
                {
                    StatusCode = value.StatusCode
                };
            }
            return model;
        }

        public async Task<PartnerResponseModel> GetPartnersListAsync(string sessionId)
        {
            PartnerResponseModel model = new PartnerResponseModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetPartnerBySesssionIdUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.PartnerModel = DeserializeObject<IList<PartnerModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;

            return model;
        }

        public async Task<PartnerTypeResponseModel> GetPartnerTypeAsync(string sessionId)
        {
            PartnerTypeResponseModel model = new PartnerTypeResponseModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetPartnerTypeUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.PartnerTypeModel = DeserializeObject<IList<PartnerTypeModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;

            return model;
        }

        public async Task<ValidateBarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode)
        {
            string url = string.Format(Configuration.GetValidateBarcodeUrl, barcode, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var model = value.Response!= null? DeserializeObject<ValidateBarcodeModel>(value.Response, GetJsonSetting()) : new ValidateBarcodeModel();
            if (model != null)
            {
                model.Response = new KegIDResponse
                {
                    StatusCode = value.StatusCode
                };
            }
            return model;
        }

        public async Task<PartnerResponseModel> GetPartnerSearchAsync(string sessionId, string search, bool internalonly, bool includepublic)
        {
            PartnerResponseModel model = new PartnerResponseModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetPartnerSearchUrl, sessionId, search, internalonly, includepublic);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.PartnerModel = DeserializeObject<IList<PartnerModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;

            return model;
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
            ManifestSearchModel model = new ManifestSearchModel
            {
                Response = new KegIDResponse()
            };
            string url = string.Format(Configuration.GetManifestSearchUrl, sessionId, trackingNumber, barcode, senderId, destinationId, referenceKey, fromDate, toDate);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            model.ManifestSearchResponseModel = DeserializeObject<IList<ManifestSearchResponseModel>>(value.Response, GetJsonSetting());
            model.Response.StatusCode = value.StatusCode;

            return model;
        }

        public async Task<ManifestModelGet> PostManifestAsync(ManifestModel inModel, string sessionId, string RequestType)
        {
            string url = string.Format(Configuration.PostManifestUrl, sessionId);
            string content = JsonConvert.SerializeObject(inModel);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            var outModel = value.Response != null ? DeserializeObject<ManifestModelGet>(value.Response, GetJsonSetting()) : new ManifestModelGet();
            if (outModel != null)
            {
                outModel.Response = new KegIDResponse
                {
                    StatusCode = value.StatusCode
                };
            }
            return outModel;
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

        public async Task<NewPartnerResponseModel> PostNewPartnerAsync(NewPartnerRequestModel inModel, string sessionId, string RequestType)
        {
            string url = string.Format(Configuration.PostNewPartnerUrl, sessionId);
            string content = JsonConvert.SerializeObject(inModel);

            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            var outModel = DeserializeObject<NewPartnerResponseModel>(value.Response, GetJsonSetting());
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
