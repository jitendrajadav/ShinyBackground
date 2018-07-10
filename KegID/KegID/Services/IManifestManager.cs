using KegID.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IManifestManager
    {
        Task<ManifestModel> GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId, IList<BarcodeModel> barcodeCollection,
    List<Tag> tags, PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches, List<string> closedBatches, long validationStatus, string contents = "");

    }
}
