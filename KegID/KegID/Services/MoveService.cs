using System.Collections.Generic;
using System.Threading.Tasks;
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

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            brandResponseModel.BrandModel = DeserializeObject<IList<BrandModel>>(value.Response, settings);
            brandResponseModel.StatusCode = value.StatusCode;

            return brandResponseModel;
        }

        public async Task<ManifestResponseModel> GetManifestAsync(string sessionId,string manifestId)
        {
            ManifestResponseModel manifestResponseModel = new ManifestResponseModel();

            string url = string.Format(Configuration.GetManifestUrl, manifestId, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            manifestResponseModel = !string.IsNullOrEmpty(value.Response) ? DeserializeObject<ManifestResponseModel>(value.Response, settings) : manifestResponseModel;
            manifestResponseModel.StatusCode = value.StatusCode;

            return manifestResponseModel;
        }

        public async Task<PartnerResponseModel> GetPartnersListAsync(string sessionId)
        {
            PartnerResponseModel partnerResponseModel = new PartnerResponseModel();

            string url = string.Format(Configuration.GetPartnerBySesssionIdUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            partnerResponseModel.PartnerModel = DeserializeObject<IList<PartnerModel>>(value.Response, settings);
            partnerResponseModel.StatusCode = value.StatusCode;

            return partnerResponseModel;
        }

        public async Task<PartnerTypeResponseModel> GetPartnerTypeAsync(string sessionId)
        {
            PartnerTypeResponseModel partnerTypeResponseModel = new PartnerTypeResponseModel();

            string url = string.Format(Configuration.GetPartnerTypeUrl, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            partnerTypeResponseModel.PartnerTypeModel = DeserializeObject<IList<PartnerTypeModel>>(value.Response, settings);
            partnerTypeResponseModel.StatusCode = value.StatusCode;

            return partnerTypeResponseModel;
        }

        public async Task<ValidateBarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode)
        {
            ValidateBarcodeModel validateBarcodeModel = new ValidateBarcodeModel();

            string url = string.Format(Configuration.GetValidateBarcodeUrl, barcode, sessionId);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            validateBarcodeModel = value.Response!= null? DeserializeObject<ValidateBarcodeModel>(value.Response,settings) : new ValidateBarcodeModel();
            validateBarcodeModel.StatusCode = value.StatusCode;

            return validateBarcodeModel;
        }

        public async Task<PartnerResponseModel> GetPartnerSearchAsync(string sessionId, string search, bool internalonly, bool includepublic)
        {
            PartnerResponseModel partnerResponseModel = new PartnerResponseModel();

            string url = string.Format(Configuration.GetPartnerSearchUrl, sessionId, search, internalonly, includepublic);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            partnerResponseModel.PartnerModel = DeserializeObject<IList<PartnerModel>>(value.Response, settings);
            partnerResponseModel.StatusCode = value.StatusCode;

            return partnerResponseModel;
        }

        public async Task<ManifestSearchModel> GetManifestSearchAsync(string sessionId, string trackingNumber,string barcode, string senderId, string destinationId, string referenceKey,string fromDate, string toDate)
        {
            ManifestSearchModel manifestSearchModel = new ManifestSearchModel();

            string url = string.Format(Configuration.GetManifestSearchUrl, sessionId, trackingNumber, barcode, senderId, destinationId, referenceKey, fromDate, toDate);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            manifestSearchModel.ManifestSearchResponseModel = DeserializeObject<IList<ManifestSearchResponseModel>>(value.Response, settings);
            manifestSearchModel.StatusCode = value.StatusCode;

            return manifestSearchModel;
        }

        public async Task<ManifestModelGet> PostManifestAsync(ManifestModel model, string sessionId, string RequestType)
        {
            ManifestModelGet manifestModelGet = new ManifestModelGet();

            string url = string.Format(Configuration.PostManifestUrl, sessionId);
            string content = JsonConvert.SerializeObject(model);
            var value = await ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            manifestModelGet = value.Response != null? DeserializeObject<ManifestModelGet>(value.Response, settings) : new ManifestModelGet();
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

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                Converters = new List<JsonConverter> { new CustomIntConverter() }
            };

            partnerResponseModel = DeserializeObject<NewPartnerResponseModel>(value.Response, settings);
            partnerResponseModel.StatusCode = value.StatusCode;
            return partnerResponseModel;
        }

    }
}
