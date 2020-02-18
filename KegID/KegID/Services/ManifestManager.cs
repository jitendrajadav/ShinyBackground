using KegID.Common;
using KegID.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KegID.Services
{
    public class ManifestManager : IManifestManager
    {
        public ManifestModel GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId, IList<BarcodeModel> barcodeCollection, long Latitude, long Longitude, string OriginId, string OrderId,
            List<Tag> tags, string tagsStr, PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches, List<string> closedBatches, MaintenanceModel maintenanceModel, long validationStatus, DateTimeOffset? EffectiveDateAllowed, string contents = "", string size = "")
        {
            List<ManifestTItem> manifestItems = new List<ManifestTItem>();
            foreach (var item in barcodeCollection)
            {
                string barcodeId = item.Barcode;
                ManifestTItem manifestItem = new ManifestTItem()
                {
                    Barcode = item.Barcode,
                    ScanDate = DateTimeOffset.UtcNow.Date,
                    KegId = item.Kegs?.Partners?.FirstOrDefault()?.Kegs?.FirstOrDefault().KegId ?? default,
                    Icon = item.Icon,
                    TagsStr = item.TagsStr ?? default,
                    Contents = contents,
                };
                if (item.Tags != null)
                {
                    foreach (var tag in item.Tags)
                    {
                        manifestItem.Tags.Add(tag);
                    }
                }
                manifestItems.Add(manifestItem);
                barcodeId = string.Empty;
            }

            ManifestModel manifestModel = new ManifestModel()
            {
                ManifestId = manifestId,
                EventTypeId = (long)eventTypeEnum,
                Latitude = Latitude,
                Longitude = Longitude,
                SubmittedDate = DateTimeOffset.UtcNow.Date,
                ShipDate = DateTimeOffset.UtcNow.Date,
                SenderId = Settings.CompanyId,
                ReceiverId = partnerModel?.PartnerId,
                DestinationId = partnerModel?.PartnerId,
                EffectiveDate = EffectiveDateAllowed ?? DateTimeOffset.UtcNow.Date,
                OriginId = OriginId,
                OwnerName = partnerModel?.FullName,
                ManifestItemsCount = manifestItems.Count,
                TagsStr = tagsStr,
                Size = size,
                KegOrderId = OrderId,
                PostedDate = DateTimeOffset.UtcNow.Date,
                SourceKey = partnerModel.SourceKey
            };

            foreach (var item in barcodeCollection)
                manifestModel.BarcodeModels.Add(item);

            foreach (var item in manifestItems)
                manifestModel.ManifestItems.Add(item);

            foreach (var item in newPallets)
                manifestModel.NewPallets.Add(item);

            foreach (var item in batches)
                manifestModel.NewBatches.Add(item);

            if (maintenanceModel != null)
                manifestModel.MaintenanceModels = maintenanceModel;

            if (tags != null)
            {
                foreach (var item in tags)
                    manifestModel.Tags.Add(item);
            }

            foreach (var item in closedBatches)
                manifestModel.ClosedBatches.Add(item);

            return manifestModel;
        }
    }
}
