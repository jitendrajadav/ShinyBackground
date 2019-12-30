using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class TItem : RealmObject
    {
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public DateTimeOffset ScanDate { get; set; }
        [DoNotNotify]
        public IList<Tag> Tags { get; }
        [DoNotNotify]
        public string TagsStr { get; set; }
        [DoNotNotify]
        public string Icon { get; set; }
    }

    public class NewBatch : RealmObject
    {
        [DoNotNotify]
        public string BatchId { get; set; }
        [DoNotNotify]
        public string CompanyId { get; set; }
        [DoNotNotify]
        public string BatchCode { get; set; }
        [DoNotNotify]
        public string RecipeId { get; set; }
        [DoNotNotify]
        public string BrandName { get; set; }
        [DoNotNotify]
        public DateTimeOffset? BrewDate { get; set; }
        [DoNotNotify]
        public DateTimeOffset? PackageDate { get; set; }
        [DoNotNotify]
        public DateTimeOffset? BestBeforeDate { get; set; }
        [DoNotNotify]
        public string BrewedVolume { get; set; }
        [DoNotNotify]
        public string BrewedVolumeUom { get; set; }
        [DoNotNotify]
        public long? PackagedVolume { get; set; }
        [DoNotNotify]
        public string PackagedVolumeUom { get; set; }
        [DoNotNotify]
        public DateTimeOffset? CompletedDate { get; set; }
        [DoNotNotify]
        public bool IsCompleted { get; set; }
        [DoNotNotify]
        public string Abv { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public IList<Tag> Tags { get; }
    }

    public class NewPallet : RealmObject
    {
        [DoNotNotify]
        public IList<ManifestTItem> PalletItems { get; }
        [DoNotNotify]
        public string PalletId { get; set; }
        [DoNotNotify]
        public string OwnerId { get; set; }
        [DoNotNotify]
        public DateTimeOffset BuildDate { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public string BarcodeFormat { get; set; }
        [DoNotNotify]
        public string StockLocation { get; set; }
        [DoNotNotify]
        public string StockLocationId { get; set; }
        [DoNotNotify]
        public string StockLocationName { get; set; }
        [DoNotNotify]
        public string ReferenceKey { get; set; }
        [DoNotNotify]
        public IList<Tag> Tags { get; }
        [DoNotNotify]
        public bool IsPalletze { get; set; }
    }

    public class Tag : RealmObject
    {
        [DoNotNotify]
        public string Property { get; set; }
        [DoNotNotify]
        public string Value { get; set; }
        [DoNotNotify]
        public string PropertyName { get; set; }
        [DoNotNotify]
        public string PropertyValue { get; set; }
        [DoNotNotify]
        public string Name { get; set; }
    }

    public class GeneralTag
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }
}
