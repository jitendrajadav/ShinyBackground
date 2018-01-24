using KegID.Response;
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
        Task<ManifestModelGet> PostManifestAsync(ManifestModel model, string sessionId);
    }
}
