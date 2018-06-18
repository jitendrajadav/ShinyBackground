using KegID.Model;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KegID.Common
{
    public static class ManifestManager
    {
        public static async Task<ManifestModel> GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId, IList<BarcodeModel> barcodeCollection,
            List<Tag> tags, PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches, List<string> closedBatches, long validationStatus, string contents = "")
        {
            ManifestModel manifestModel = null;
            List<ManifestItem> manifestItemlst = new List<ManifestItem>();
            ManifestItem manifestItem = null;

            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
                location = await Geolocation.GetLocationAsync(request);

            try
            {
                foreach (var item in barcodeCollection)
                {
                    string barcodeId = item.Barcode;
                    manifestItem = new ManifestItem()
                    {
                        Barcode = item.Barcode,
                        ScanDate = DateTimeOffset.UtcNow.Date,
                        ValidationStatus = validationStatus,
                        KegId = item.Kegs?.Partners?.FirstOrDefault()?.Kegs?.FirstOrDefault().KegId,
                        //Tags = tags,
                        //KegStatus = new List<KegStatus>()
                        //{
                        //    new KegStatus()
                        //    {
                        //        KegId = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().KegId,
                        //        Barcode = barcodeResult.Barcode,
                        //        AltBarcode = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().AltBarcode??string.Empty,
                        //        Contents = contents,
                        //        Batch = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Batch.ToString(),
                        //        Size = tags.Any(x=>x.Property == "Size") ? tags.Where(x=>x.Property == "Size").Select(x=>x.Value).FirstOrDefault():string.Empty,
                        //        Alert = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Alert,
                        //        Location = validateBarcodeModel.Kegs.Locations.FirstOrDefault(),
                        //        OwnerName = partnerModel.FullName,
                        //    }
                        //},
                    };

                    foreach (var tag in tags)
                    {
                        manifestItem.Tags.Add(tag);
                    }
                    manifestItemlst.Add(manifestItem);
                    barcodeId = string.Empty;
                }

                manifestModel = new ManifestModel()
                {
                    ManifestId = manifestId,
                    EventTypeId = (long)eventTypeEnum,
                    Latitude = (long)location.Latitude,
                    Longitude = (long)location.Longitude,
                    SubmittedDate = DateTimeOffset.UtcNow.Date,
                    ShipDate = DateTimeOffset.UtcNow.Date,

                    SenderId = AppSettings.User.CompanyId,
                    ReceiverId = partnerModel.PartnerId,
                    //DestinationName = partnerModel.FullName,
                    //DestinationTypeCode = partnerModel.LocationCode,
                    OwnerName = partnerModel.FullName,
                    //ManifestItems= manifestItemlst,
                    //NewPallets = newPallets,
                    //NewBatches = batches,
                    //Tags = tags.ToList(),
                    //ClosedBatches = closedBatches
                };

                foreach (var item in manifestItemlst)
                    manifestModel.ManifestItems.Add(item);

                foreach (var item in newPallets)
                    manifestModel.NewPallets.Add(item);

                foreach (var item in batches)
                    manifestModel.NewBatches.Add(item);

                foreach (var item in tags)
                    manifestModel.Tags.Add(item);

                foreach (var item in closedBatches)
                    manifestModel.ClosedBatches.Add(item);

                return manifestModel;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
            finally
            {
                //manifestItem = null;
                //manifestItemlst = null;
                //validateBarcodeModel = null;
                //manifestModel = null;
            }
        }
    }
}
