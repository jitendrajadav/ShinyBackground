using KegID.Model;
using System;
using System.Collections.Generic;

namespace KegID.Services
{
    public interface IManifestManager
    {
        ManifestModel GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId,
            IList<BarcodeModel> barcodeCollection, long Latitude, long Longitude,string OriginId, string OrderId,  List<Tag> tags, string tagsStr,
            PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches,
            List<string> closedBatches, MaintenanceModel maintenanceModel, long validationStatus, DateTimeOffset? EffectiveDateAllowed, string contents = "",
            string size = "");
    }
}
