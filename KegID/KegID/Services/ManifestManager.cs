using KegID.Common;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KegID.Services
{
    public class ManifestManager : IManifestManager
    {
        public ManifestModel GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId, IList<BarcodeModel> barcodeCollection, long Latitude,long Longitude,
            List<Tag> tags, string tagsStr, PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches, List<string> closedBatches, MaintenanceModel maintenanceModel, long validationStatus, DateTimeOffset? EffectiveDateAllowed, string contents = "", string size = "")
        {
            List<ManifestItem> manifestItemlst = new List<ManifestItem>();
            try
            {
                foreach (var item in barcodeCollection)
                {
                    string barcodeId = item.Barcode;
                    ManifestItem manifestItem = new ManifestItem()
                    {
                        Barcode = item.Barcode,
                        ScanDate = DateTimeOffset.UtcNow.Date,
                        ValidationStatus = validationStatus,
                        KegId = item.Kegs?.Partners?.FirstOrDefault()?.Kegs?.FirstOrDefault().KegId ?? default,
                        Icon = item.Icon,
                        TagsStr = item.TagsStr ?? default,
                    };
                    if (item.Tags != null)
                    {
                        foreach (var tag in item.Tags)
                        {
                            manifestItem.Tags.Add(tag);
                        }
                    }
                    manifestItemlst.Add(manifestItem);
                    barcodeId = string.Empty;
                }

                ManifestModel manifestModel = new ManifestModel()
                {
                    ManifestId = manifestId,
                    EventTypeId = (long)eventTypeEnum,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    SubmittedDate = EffectiveDateAllowed ?? DateTimeOffset.UtcNow.Date,
                    ShipDate = DateTimeOffset.UtcNow.Date,

                    SenderId = AppSettings.CompanyId,
                    ReceiverId = partnerModel?.PartnerId,
                    //DestinationName = partnerModel.FullName,
                    //DestinationTypeCode = partnerModel.LocationCode,
                    OwnerName = partnerModel?.FullName,
                    //ManifestItems= manifestItemlst,
                    //NewPallets = newPallets,
                    //NewBatches = batches,
                    //Tags = tags.ToList(),
                    //ClosedBatches = closedBatches
                    ManifestItemsCount = manifestItemlst.Count,
                    TagsStr = tagsStr,
                    Size = size
                };

                foreach (var item in barcodeCollection)
                    manifestModel.BarcodeModels.Add(item);

                foreach (var item in manifestItemlst)
                    manifestModel.ManifestItems.Add(item);

                foreach (var item in newPallets)
                    manifestModel.NewPallets.Add(item);

                foreach (var item in batches)
                    manifestModel.NewBatches.Add(item);

                if (maintenanceModel != null)
                {
                    manifestModel.MaintenanceModels = maintenanceModel;
                }

                if (tags != null)
                {
                    foreach (var item in tags)
                        manifestModel.Tags.Add(item);
                }

                foreach (var item in closedBatches)
                    manifestModel.ClosedBatches.Add(item);

                return manifestModel;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
        }
    }
}
