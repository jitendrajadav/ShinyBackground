using KegID.Model;
using KegID.SQLiteClient;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KegID.Common
{
    public static class ManifestManager
    {
        public static async Task<ManifestModel> GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId, IList<Barcode> barcodeCollection,
            List<Tag> tags, PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches, List<string> closedBatches, long validationStatus, string contents = "")
        {
            ManifestModel manifestModel = null;
            ValidateBarcodeModel validateBarcodeModel = null;
            List<ManifestItem> manifestItemlst = new List<ManifestItem>();
            ManifestItem manifestItem = null;

            try
            {
                foreach (var item in barcodeCollection)
                {
                    string barcodeId = item.Id;
                    var barcodeResult = await SQLiteServiceClient.Db.Table<BarcodeModel>().Where(x => x.Barcode == barcodeId).FirstOrDefaultAsync();
                    validateBarcodeModel = JsonConvert.DeserializeObject<ValidateBarcodeModel>(barcodeResult.BarcodeJson);

                    manifestItem = new ManifestItem()
                    {
                        Barcode = barcodeResult.Barcode,
                        ScanDate = DateTime.Today,
                        ValidationStatus = validationStatus,
                        KegId = validateBarcodeModel.Kegs?.Partners?.FirstOrDefault()?.Kegs?.FirstOrDefault().KegId,
                        Tags = tags,
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
                    manifestItemlst.Add(manifestItem);
                    barcodeId = string.Empty;
                }

                manifestModel = new ManifestModel()
                {
                    ManifestId = manifestId,
                    EventTypeId = (long)eventTypeEnum,
                    Latitude = (long)Geolocation.savedPosition.Latitude,
                    Longitude = (long)Geolocation.savedPosition.Longitude,
                    SubmittedDate = DateTime.Today,
                    ShipDate = DateTime.Today,

                    SenderId = AppSettings.User.CompanyId,
                    ReceiverId = partnerModel.PartnerId,
                    //DestinationName = partnerModel.FullName,
                    //DestinationTypeCode = partnerModel.LocationCode,
                    OwnerName = partnerModel.FullName,
                    ManifestItems = manifestItemlst,
                    NewPallets = newPallets,
                    NewBatches = batches,
                    Tags = tags.ToList(),
                    ClosedBatches = closedBatches
                };

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
