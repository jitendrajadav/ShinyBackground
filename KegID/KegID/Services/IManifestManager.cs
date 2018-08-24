using KegID.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.Services
{
    public interface IManifestManager
    {
        ManifestModel GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId, IList<BarcodeModel> barcodeCollection,
    List<Tag> tags,string tagsStr, PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches, List<string> closedBatches, long validationStatus, string contents = "");

    }
}
