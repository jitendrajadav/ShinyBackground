using KegID.Model;
using KegID.SQLiteClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KegID.Common
{
    public static class  ManifestManager
    {
        public static async Task<ManifestModel> GetManifestDraft(EventTypeEnum eventTypeEnum, string manifestId, IList<Barcode> barcodeCollection, 
            List<Tag> tags, PartnerModel partnerModel, List<NewPallet> newPallets, List<NewBatch> batches, List<string> closedBatches, long validationStatus, string contents = "")
        {
            try
            {
                ManifestModel manifestModel = null;
                ValidateBarcodeModel validateBarcodeModel = null;
                List<ManifestItem> manifestItemlst = new List<ManifestItem>();
                ManifestItem manifestItem = null;

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
                        KegId = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().KegId,
                        Tags = tags,
                        KegStatus = new List<KegStatus>()
                        {
                            new KegStatus()
                            {
                                KegId= validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().KegId,
                                Barcode=barcodeResult.Barcode,
                                AltBarcode=validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().AltBarcode,
                                Contents = contents,
                                Batch =validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Batch.ToString(),
                                Size = tags.Any(x=>x.Property == "Size") ? tags.Where(x=>x.Property == "Size").Select(x=>x.Value).FirstOrDefault():string.Empty,
                                Alert = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Alert,
                                Location = validateBarcodeModel.Kegs.Locations.FirstOrDefault(),
                                OwnerName = partnerModel.FullName,
                            }
                        },
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
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        //public static async Task<ManifestRequestModel> GetManifestDraftNew(EventTypeEnum eventTypeEnum, string manifestId,
        //    IList<Barcode> barcodeCollection, List<Tag> tags, PartnerModel partnerModel, string contents = "")
        //{
        //    try
        //    {
        //        ManifestRequestModel manifestModel = null;
        //        ValidateBarcodeModel validateBarcodeModel = null;
        //        List<TItem> manifestItemlst = new List<TItem>();
        //        TItem manifestItem = null;

        //        foreach (var item in barcodeCollection)
        //        {
        //            string barcodeId = item.Id;
        //            var barcodeResult = await SQLiteServiceClient.Db.Table<BarcodeModel>().Where(x => x.Barcode == barcodeId).FirstOrDefaultAsync();
        //            validateBarcodeModel = JsonConvert.DeserializeObject<ValidateBarcodeModel>(barcodeResult.BarcodeJson);

        //            manifestItem = new TItem()
        //            {
        //                Barcode = barcodeResult.Barcode,
        //                ScanDate = DateTime.Today,
        //                //ValidationStatus = 2,
        //                KegId = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().KegId,
        //                Tags = tags,
        //                BatchId = string.Empty,
        //                Contents = contents,
        //                HeldOnPalletId = string.Empty,
        //                PalletId = manifestId,
        //                SkuId = string.Empty
        //                //KegStatus = new List<KegStatus>()
        //                //{
        //                //    new KegStatus()
        //                //    {
        //                //        KegId= validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().KegId,
        //                //        Barcode=barcodeResult.Barcode,
        //                //        AltBarcode=validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().AltBarcode,
        //                //        Contents = contents,
        //                //        Batch =validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Batch.ToString(),
        //                //        Size = tags.Any(x=>x.Property == "Size") ? tags.Where(x=>x.Property == "Size").Select(x=>x.Value).FirstOrDefault():string.Empty,
        //                //        Alert = validateBarcodeModel.Kegs.Partners.FirstOrDefault().Kegs.FirstOrDefault().Alert,
        //                //        Location = validateBarcodeModel.Kegs.Locations.FirstOrDefault(),
        //                //        OwnerName = partnerModel.FullName,
        //                //    }
        //                //},
        //            };
        //            manifestItemlst.Add(manifestItem);
        //            barcodeId = string.Empty;
        //        }

        //        manifestModel = new ManifestRequestModel()
        //        {
        //            ManifestId = manifestId,
        //            ClosedBatches = new List<string>(),
        //            DestinationId = partnerModel.PartnerId,
        //            EffectiveDate = DateTime.Today,
        //            Gs1Gsin = string.Empty,
        //            IsSendManifest = false,
        //            KegOrderId = string.Empty,
        //            NewBatch = new NewBatch(),
        //            NewBatches = new List<NewBatch>(),
        //            OriginId = string.Empty,
        //            PostedDate = DateTime.Today,
        //            SourceKey = string.Empty,

        //            EventTypeId = (long)eventTypeEnum,
        //            Latitude = (long)Geolocation.savedPosition.Latitude,
        //            Longitude = (long)Geolocation.savedPosition.Longitude,
        //            SubmittedDate = DateTime.Today,
        //            ShipDate = DateTime.Today,

        //            SenderId = Configuration.CompanyId,
        //            ReceiverId = partnerModel.PartnerId,

        //            ManifestItems = manifestItemlst,
        //            NewPallets = new List<NewPallet>(),
        //            Tags = tags.ToList()
        //        };

        //        return manifestModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return null;
        //    }
        //}

    }
}
