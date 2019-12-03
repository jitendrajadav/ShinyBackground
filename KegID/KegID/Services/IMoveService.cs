//using KegID.Model;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace KegID.Services
//{
//    public interface IMoveService
//    {
//        Task<OwnerResponseModel> GetOwner(string sessionId);
//        Task<PartnerResponseModel> GetPartnersList(string sessionId);
//        Task<BarcodeModel> GetValidateBarcode(string sessionId, string barcode);
//        Task<BrandResponseModel> GetBrandList(string sessionId);
//        Task<ManifestResponseModel> GetManifest(string sessionId, string manifestId);
//        Task<PartnerTypeResponseModel> GetPartnerType(string sessionId);
//        Task<PartnerResponseModel> GetPartnerSearch(string sessionId,string search,bool internalonly,bool includepublic);
//        Task<ManifestSearchModel> GetManifestSearch(string sessionId, string trackingNumber,string barcode, string senderId, string destinationId,string referenceKey,string fromDate, string toDate);
//        Task<IList<string>> GetAssetSize(string sessionId, bool assignableOnly);
//        Task<IList<string>> GetAssetType(string sessionId, bool assignableOnly);

//        Task<ManifestModelGet> PostManifest(ManifestModel model, string sessionId,string RequestType);
//        Task<NewPartnerResponseModel> PostNewPartner(NewPartnerRequestModel model, string sessionId, string RequestType);
//    }
//}
