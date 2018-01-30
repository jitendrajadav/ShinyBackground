using KegID.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IMoveService
    {
        Task<IList<PartnerModel>> GetPartnersListAsync(string sessionId);
        Task<ValidateBarcodeModel> GetValidateBarcodeAsync(string sessionId, string barcode);
        Task<IList<BrandModel>> GetBrandListAsync(string sessionId);
        Task<IList<ManifestModelGet>> GetManifestListAsync(string sessionId);
        Task<IList<PartnerTypeModel>> GetPartnerTypeAsync(string sessionId);


        Task<ManifestModelGet> PostManifestAsync(ManifestModel model, string sessionId,string RequestType);
        Task<NewPartnerResponseModel> PostNewPartnerAsync(NewPartnerRequestModel model, string sessionId, string RequestType);
    }
}
