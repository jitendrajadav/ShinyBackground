using KegID.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IMoveService
    {
        Task<OwnerResponseModel> GetOwnerAsync(string sessionId);
        Task<PartnerResponseModel> GetPartnersListAsync(string sessionId);
        Task<BarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode);
        Task<BrandResponseModel> GetBrandListAsync(string sessionId);
        Task<ManifestResponseModel> GetManifestAsync(string sessionId, string manifestId);
        Task<PartnerTypeResponseModel> GetPartnerTypeAsync(string sessionId);
        Task<PartnerResponseModel> GetPartnerSearchAsync(string sessionId,string search,bool internalonly,bool includepublic);
        Task<ManifestSearchModel> GetManifestSearchAsync(string sessionId, string trackingNumber,string barcode, string senderId, string destinationId,string referenceKey,string fromDate, string toDate);
        Task<IList<string>> GetAssetSizeAsync(string sessionId, bool assignableOnly);
        Task<IList<string>> GetAssetTypeAsync(string sessionId, bool assignableOnly);

        Task<ManifestModelGet> PostManifestAsync(ManifestModel model, string sessionId,string RequestType);

        Task<NewPartnerResponseModel> PostNewPartnerAsync(NewPartnerRequestModel model, string sessionId, string RequestType);
    }
}
