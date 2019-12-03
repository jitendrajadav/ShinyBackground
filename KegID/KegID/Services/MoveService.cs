//using System.Collections.Generic;
//using System.Threading.Tasks;
//using KegID.Common;
//using KegID.Model;
//using Newtonsoft.Json;

//namespace KegID.Services
//{
//    public class MoveService : IMoveService
//    {
//        public async Task<OwnerResponseModel> GetOwnerAsync(string sessionId)
//        {
//            OwnerResponseModel model = new OwnerResponseModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetOwner, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.OwnerModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<OwnerModel>>(value.Response) : new List<OwnerModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<BrandResponseModel> GetBrandListAsync(string sessionId)
//        {
//            BrandResponseModel model = new BrandResponseModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetBrandUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.BrandModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<BrandModel>>(value.Response) : new List<BrandModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<ManifestResponseModel> GetManifestAsync(string sessionId, string manifestId)
//        {
//            ManifestResponseModel model = null;
//            try
//            {
//                string url = string.Format(Configuration.GetManifestUrl, manifestId, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<ManifestResponseModel>(value.Response) : new ManifestResponseModel();
//                if (model != null)
//                {
//                    model.Response = new KegIDResponse
//                    {
//                        StatusCode = value.StatusCode
//                    };
//                }
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<PartnerResponseModel> GetPartnersListAsync(string sessionId)
//        {
//            PartnerResponseModel model = new PartnerResponseModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetPartnerBySesssionIdUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.PartnerModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<PartnerModel>>(value.Response) : new List<PartnerModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<PartnerTypeResponseModel> GetPartnerTypeAsync(string sessionId)
//        {
//            PartnerTypeResponseModel model = new PartnerTypeResponseModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetPartnerTypeUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.PartnerTypeModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<PartnerTypeModel>>(value.Response) : new List<PartnerTypeModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<BarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode)
//        {
//            BarcodeModel model = null;
//            try
//            {
//                string url = string.Format(Configuration.GetValidateBarcodeUrl, barcode, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<BarcodeModel>(value.Response) : new BarcodeModel();
//                if (model != null)
//                {
//                    model.Response = new KegIDResponse
//                    {
//                        StatusCode = value.StatusCode
//                    };
//                }
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<PartnerResponseModel> GetPartnerSearchAsync(string sessionId, string search, bool internalonly, bool includepublic)
//        {
//            PartnerResponseModel model = new PartnerResponseModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetPartnerSearchUrl, sessionId, search, internalonly, includepublic);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.PartnerModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<PartnerModel>>(value.Response) : new List<PartnerModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<IList<string>> GetAssetSizeAsync(string sessionId, bool assignableOnly)
//        {
//            IList<string> model = null;
//            try
//            {
//                string url = string.Format(Configuration.GetAssetSize, sessionId, assignableOnly);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);
//                model= !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<string>>(value.Response) : new List<string>();
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<IList<string>> GetAssetTypeAsync(string sessionId, bool assignableOnly)
//        {
//            IList<string> model = null;
//            try
//            {
//                string url = string.Format(Configuration.GetAssetType, sessionId, assignableOnly);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);
//                model = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<string>>(value.Response) : new List<string>();
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<ManifestSearchModel> GetManifestSearchAsync(string sessionId, string trackingNumber, string barcode, string senderId, string destinationId, string referenceKey, string fromDate, string toDate)
//        {
//            ManifestSearchModel model = new ManifestSearchModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetManifestSearchUrl, sessionId, trackingNumber, barcode, senderId, destinationId, referenceKey, fromDate, toDate);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.ManifestSearchResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<ManifestSearchResponseModel>>(value.Response) : new List<ManifestSearchResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<ManifestModelGet> PostManifestAsync(ManifestModel inModel, string sessionId, string RequestType)
//        {
//            ManifestModelGet outModel = null;
//            try
//            {
//                string url = string.Format(Configuration.PostManifestUrl, sessionId);
//                string content = JsonConvert.SerializeObject(inModel);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

//                outModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<ManifestModelGet>(value.Response) : new ManifestModelGet();
//                if (outModel != null)
//                {
//                    outModel.Response = new KegIDResponse
//                    {
//                        StatusCode = value.StatusCode
//                    };
//                }
//            }
//            catch (System.Exception)
//            {

//            }
//            return outModel;
//        }

//        public async Task<NewPartnerResponseModel> PostNewPartnerAsync(NewPartnerRequestModel inModel, string sessionId, string RequestType)
//        {
//            NewPartnerResponseModel outModel = null;
//            try
//            {
//                string url = string.Format(Configuration.PostNewPartnerUrl, sessionId);
//                string content = JsonConvert.SerializeObject(inModel);

//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

//                outModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<NewPartnerResponseModel>(value.Response) : new NewPartnerResponseModel();
//                if (outModel != null)
//                {
//                    outModel.Response = new KegIDResponse
//                    {
//                        StatusCode = value.StatusCode
//                    };
//                }
//            }
//            catch (System.Exception)
//            {

//            }
//            return outModel;
//        }
//    }
//}
