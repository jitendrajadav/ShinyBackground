//using SQLite.Net.Attributes;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    //public class ManifestRequestModel
    //{
    //    [PrimaryKey]
    //    public string ManifestId { get; set; }
    //    public DateTimeOffset ShipDate { get; set; }
    //    [Ignore]
    //    public List<TItem> ManifestItems { get; set; }
    //    public DateTimeOffset SubmittedDate { get; set; }
    //    public string OriginId { get; set; }
    //    public string SenderId { get; set; }
    //    public string DestinationId { get; set; }
    //    public string ReceiverId { get; set; }
    //    public List<Tag> Tags { get; set; }
    //    public string Gs1Gsin { get; set; }
    //    public bool IsSendManifest { get; set; }
    //    public DateTimeOffset EffectiveDate { get; set; }
    //    public long EventTypeId { get; set; }
    //    public long Latitude { get; set; }
    //    public long Longitude { get; set; }
    //    [Ignore]
    //    public List<NewPallet> NewPallets { get; set; }
    //    [Ignore]
    //    public NewBatch NewBatch { get; set; }
    //    [Ignore]
    //    public List<NewBatch> NewBatches { get; set; }
    //    public string KegOrderId { get; set; }
    //    public DateTimeOffset PostedDate { get; set; }
    //    public string SourceKey { get; set; }
    //    [Ignore]
    //    public List<string> ClosedBatches { get; set; }
    //}

    public class TItem
    {
        public string Barcode { get; set; }
        public DateTimeOffset ScanDate { get; set; }
        //public long ValidationStatus { get; set; }
        public List<Tag> Tags { get; set; }
        //public string Contents { get; set; }
        //public string KegId { get; set; }
        //public string PalletId { get; set; }
        //public string HeldOnPalletId { get; set; }
        //public string SkuId { get; set; }
        //public string BatchId { get; set; }
    }

    public class NewBatch
    {
        public string BatchId { get; set; }
        public string CompanyId { get; set; }
        public string BatchCode { get; set; }
        public string RecipeId { get; set; }
        public string BrandName { get; set; }
        public DateTimeOffset BrewDate { get; set; }
        public DateTimeOffset PackageDate { get; set; }
        public DateTimeOffset BestBeforeDate { get; set; }
        public long BrewedVolume { get; set; }
        public string BrewedVolumeUom { get; set; }
        public long PackagedVolume { get; set; }
        public string PackagedVolumeUom { get; set; }
        public DateTimeOffset CompletedDate { get; set; }
        public bool IsCompleted { get; set; }
        public long Abv { get; set; }
        public string SourceKey { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class NewPallet
    {
        public List<TItem> PalletItems { get; set; }
        public string PalletId { get; set; }
        public string OwnerId { get; set; }
        public DateTimeOffset BuildDate { get; set; }
        public string Barcode { get; set; }
        //public string BarcodeFormat { get; set; }
        //public long ManifestTypeId { get; set; }
        public string StockLocation { get; set; }
        public string StockLocationId { get; set; }
        public string StockLocationName { get; set; }
        //public string TargetLocation { get; set; }
        public string ReferenceKey { get; set; }
        public List<Tag> Tags { get; set; }
    }

    public class Tag : RealmObject
    {
        public string Property { get; set; }
        public string Value { get; set; }
        //public string PropertyName { get; set; }
        //public string PropertyValue { get; set; }
        //public string Name { get; set; }
    }

    public class GeneralTag 
    {
        public string Property { get; set; }
        public string Value { get; set; }
        //public string PropertyName { get; set; }
        //public string PropertyValue { get; set; }
        //public string Name { get; set; }
    }


}
