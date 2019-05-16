//using SQLite.Net.Attributes;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class TItem : RealmObject
    {
        public string Barcode { get; set; }
        public DateTimeOffset ScanDate { get; set; }
        public IList<Tag> Tags { get; }
        public string TagsStr { get; set; }
        public string Icon { get; set; }
    }

    public class NewBatch : RealmObject
    {
        public string BatchId { get; set; }
        public string CompanyId { get; set; }
        public string BatchCode { get; set; }
        public string RecipeId { get; set; }
        public string BrandName { get; set; }
        public DateTimeOffset BrewDate { get; set; }
        public DateTimeOffset PackageDate { get; set; }
        public DateTimeOffset BestBeforeDate { get; set; }
        public float? BrewedVolume { get; set; }
        public string BrewedVolumeUom { get; set; }
        public long PackagedVolume { get; set; }
        public string PackagedVolumeUom { get; set; }
        public DateTimeOffset CompletedDate { get; set; }
        public bool IsCompleted { get; set; }
        public float? Abv { get; set; }
        public string SourceKey { get; set; }
        public IList<Tag> Tags { get; }
    }

    public class NewPallet : RealmObject
    {
        public IList<TItem> PalletItems { get; }
        public string PalletId { get; set; }
        public string OwnerId { get; set; }
        public DateTimeOffset BuildDate { get; set; }
        public string Barcode { get; set; }
        public string StockLocation { get; set; }
        public string StockLocationId { get; set; }
        public string StockLocationName { get; set; }
        public string ReferenceKey { get; set; }
        public IList<Tag> Tags { get; }
        public bool IsPalletze { get; set; }
    }

    public class Tag : RealmObject
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string Name { get; set; }
    }

    public class GeneralTag 
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }
}
