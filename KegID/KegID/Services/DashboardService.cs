//using System.Collections.Generic;
//using System.Threading.Tasks;
//using KegID.Common;
//using KegID.Model;
//using Newtonsoft.Json;

//namespace KegID.Services
//{
//    public class DashboardService : IDashboardService
//    {
//        public async Task<PossessorModel> GetDashboardPartnersListAsync(string ownerId, string sessionId)
//        {
//            PossessorModel model = new PossessorModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetPossessorByownerId, ownerId, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.PossessorResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<PossessorResponseModel>>(value.Response) : new List<PossessorResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {
//            }
//            return model;
//        }

//        public async Task<IList<string>> GetAssetVolumeAsync(string sessionId, bool assignableOnly)
//        {
//            IList<string> response = null;
//            try
//            {
//                string url = string.Format(Configuration.GetAssetVolume, sessionId, assignableOnly);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);
//                response = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<string>>(value.Response) : new List<string>();
//            }
//            catch (System.Exception)
//            {
//            }
//            return response;
//        }

//        public async Task<IList<string>> GetOperatorsAsync(string sessionId)
//        {
//            IList<string> response = null;
//            try
//            {
//                string url = string.Format(Configuration.GetOperatorsUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);
//                response = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<string>>(value.Response) : new List<string>();
//            }
//            catch (System.Exception)
//            {
//            }
//            return response;
//        }

//        public async Task<DeleteMaintenanceAlertResponseModel> GetDeleteMaintenanceAlertAsync(string kegId, string sessionId)
//        {
//            DeleteMaintenanceAlertResponseModel model = new DeleteMaintenanceAlertResponseModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetMaintenanceAlertByKegIdUrl, kegId, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.MyProperty = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<List<string>>(value.Response) : new List<string>(); ;
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {
//            }
//            return model;
//        }

//        public async Task<DashboardResponseModel> GetDeshboardDetailAsync(string sessionId)
//        {
//            DashboardResponseModel model = null;
//            string url = string.Format(Configuration.GetDashboardUrl, sessionId);
//            try
//            {
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<DashboardResponseModel>(value.Response) : new DashboardResponseModel();
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

//        public async Task<InventoryDetailModel> GetInventoryAsync(string sessionId)
//        {
//            InventoryDetailModel model = new InventoryDetailModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetInventoryUrl, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.InventoryResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<InventoryResponseModel>>(value.Response) : new List<InventoryResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {
//            }
//            return model;
//        }

//        public async Task<MaintenanceAlertModel> GetKegMaintenanceAlertAsync(string kegId, string sessionId)
//        {
//            MaintenanceAlertModel model = new MaintenanceAlertModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetMaintenanceAlertByKegIdUrl, kegId, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.MaintenanceAlertResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<MaintenanceAlertResponseModel>>(value.Response) : new List<MaintenanceAlertResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<KegMaintenanceHistoryModel> GetKegMaintenanceHistoryAsync(string kegId, string sessionId)
//        {
//            KegMaintenanceHistoryModel model = new KegMaintenanceHistoryModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetKegMaintenanceHistoryByKegIdUrl, kegId, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.KegMaintenanceHistoryResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<KegMaintenanceHistoryResponseModel>>(value.Response) : new List<KegMaintenanceHistoryResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<KegPossessionModel> GetKegPossessionAsync(string sessionId, string partnerId)
//        {
//            KegPossessionModel model = new KegPossessionModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetKegPossessionByPartnerIdUrl, sessionId, partnerId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.KegPossessionResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<KegPossessionResponseModel>>(value.Response) : new List<KegPossessionResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<KegSearchModel> GetKegSearchAsync(string sessionId, string barcode, bool includePartials)
//        {
//            KegSearchModel model = new KegSearchModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.GetKegSearchByBarcodeUrl, sessionId, barcode, includePartials);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.KegSearchResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<KegSearchResponseModel>>(value.Response) : new List<KegSearchResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<KegStatusResponseModel> GetKegStatusAsync(string kegId, string sessionId)
//        {
//            KegStatusResponseModel model = null;
//            try
//            {
//                string url = string.Format(Configuration.GetKegStatusByKegIdUrl, kegId, sessionId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<KegStatusResponseModel>(value.Response) : new KegStatusResponseModel();
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

//        public async Task<SearchPalletModel> GetPalletSearchAsync(string sessionId, string locationId, string fromDate, string toDate, string kegs, string kegOwnerId)
//        {
//            SearchPalletModel model = new SearchPalletModel
//            {
//                Response = new KegIDResponse()
//            };

//            try
//            {
//                string url = string.Format(Configuration.GetPalletSearchUrl, sessionId, locationId, fromDate, toDate, kegs, kegOwnerId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model.SearchPalletResponseModel = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<SearchPalletResponseModel>>(value.Response) : new List<SearchPalletResponseModel>();
//                model.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return model;
//        }

//        public async Task<PartnerInfoResponseModel> GetPartnerInfoAsync(string sessionId, string partnerId)
//        {
//            PartnerInfoResponseModel model = null;
//            try
//            {
//                string url = string.Format(Configuration.GetPartnerInfoByPartnerIdUrl, sessionId, partnerId);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Get, string.Empty);

//                model = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<PartnerInfoResponseModel>(value.Response) : new PartnerInfoResponseModel();
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

//        public async Task<object> PostKegStatusAsync(KegRequestModel model, string kegId, string sessionId, string RequestType)
//        {
//            //ManifestModelGet manifestModelGet = new ManifestModelGet();

//            // It is earlier
//            //string url = string.Format(Configuration.PostKegUrl, sessionId);
//            string url = string.Format(Configuration.GetKegStatusByKegIdUrl, kegId, sessionId);

//            string content = JsonConvert.SerializeObject(model);
//            var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

//            //manifestModelGet = !string.IsNullOrEmpty(value.Response) ? DeserializeObject<ManifestModelGet>(value.Response, GetJsonSetting()) : new ManifestModelGet();
//            //manifestModelGet.StatusCode = value.StatusCode;
//            //return manifestModelGet;
//            return null;
//        }

//        public async Task<object> PostKegAsync(KegRequestModel model, string kegId, string sessionId, string RequestType)
//        {
//            //ManifestModelGet manifestModelGet = new ManifestModelGet();

//            // It is earlier
//            string url = string.Format(Configuration.PostKegUrl, sessionId);

//            string content = JsonConvert.SerializeObject(model);
//            var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

//            //manifestModelGet = !string.IsNullOrEmpty(value.Response) ? DeserializeObject<ManifestModelGet>(value.Response, GetJsonSetting()) : new ManifestModelGet();
//            //manifestModelGet.StatusCode = value.StatusCode;
//            //return manifestModelGet;
//            return value;
//        }

//        public async Task<KegMassUpdateKegModel> PostKegUploadAsync(KegBulkUpdateItemRequestModel inModel, string sessionId, string RequestType)
//        {
//            KegMassUpdateKegModel outModel = new KegMassUpdateKegModel
//            {
//                Response = new KegIDResponse()
//            };
//            try
//            {
//                string url = string.Format(Configuration.PostKegsUrl, sessionId);
//                string content = JsonConvert.SerializeObject(inModel);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);

//                outModel.Model = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<KegMassUpdateKegResponseModel>>(value.Response) : new List<KegMassUpdateKegResponseModel>();
//                outModel.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return outModel;
//        }

//        public async Task<AddMaintenanceAlertModel> PostMaintenanceAlertAsync(AddMaintenanceAlertRequestModel inModel, string sessionId, string RequestType)
//        {
//            AddMaintenanceAlertModel outModel = new AddMaintenanceAlertModel
//            {
//                Response = new KegIDResponse(),
//                AddMaintenanceAlertResponseModel = new List<AddMaintenanceAlertResponseModel>()
//            };
//            try
//            {
//                string url = string.Format(Configuration.PostMaintenanceAlertUrl, sessionId);
//                string content = JsonConvert.SerializeObject(inModel);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);
//                var result = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<AddMaintenanceAlertResponseModel>>(value.Response) : new List<AddMaintenanceAlertResponseModel>();

//                foreach (var item in result)
//                    outModel.AddMaintenanceAlertResponseModel.Add(item);

//                outModel.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return outModel;
//        }

//        public async Task<AddMaintenanceAlertModel> PostMaintenanceDeleteAlertUrlAsync(DeleteMaintenanceAlertRequestModel inModel, string sessionId, string RequestType)
//        {
//            AddMaintenanceAlertModel outModel = new AddMaintenanceAlertModel
//            {
//                Response = new KegIDResponse(),
//                AddMaintenanceAlertResponseModel = new List<AddMaintenanceAlertResponseModel>()
//            };
//            try
//            {
//                string url = string.Format(Configuration.PostMaintenanceDeleteAlertUrl, sessionId);
//                string content = JsonConvert.SerializeObject(inModel);
//                var value = await App.kegIDClient.ExecuteServiceCall<KegIDResponse>(url, HttpMethodType.Send, content, RequestType: RequestType);
//                var result = !string.IsNullOrEmpty(value.Response) ? App.kegIDClient.DeserializeObject<IList<AddMaintenanceAlertResponseModel>>(value.Response) : new List<AddMaintenanceAlertResponseModel>();
//                foreach (var item in result)
//                    outModel.AddMaintenanceAlertResponseModel.Add(item);

//                outModel.Response.StatusCode = value.StatusCode;
//            }
//            catch (System.Exception)
//            {

//            }
//            return outModel;
//        }
//    }
//}
