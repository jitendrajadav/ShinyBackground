using PropertyChanged;
using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class SkuModel
    {
        public IList<Sku> Sku { get; set; }
        public KegIDResponse Response { get; set; }
    }

    public class Feature : RealmObject
    {
        [DoNotNotify]
        public string Name { get; set; }
        [DoNotNotify]
        public string Value { get; set; }
    }

    public class Variation : RealmObject
    {
        [DoNotNotify]
        public string VariationId { get; set; }
        [DoNotNotify]
        public bool IsPrimaryVariation { get; set; }
        [DoNotNotify]
        public IList<string> PhotoIds { get; }
        [DoNotNotify]
        public IList<Feature> Features { get; }
    }

    public class Sku : RealmObject
    {
        [DoNotNotify]
        public string SkuId { get; set; }
        [DoNotNotify]
        public string SkuCode { get; set; }
        [DoNotNotify]
        public string SkuName { get; set; }
        [DoNotNotify]
        public string ShortName { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public string SkuClass { get; set; }
        [DoNotNotify]
        public string AssetOwnerId { get; set; }
        [DoNotNotify]
        public string AssetOwner { get; set; }
        [DoNotNotify]
        public string AssetOwnerSkuId { get; set; }
        [DoNotNotify]
        public string SupplierId { get; set; }
        [DoNotNotify]
        public string SupplierName { get; set; }
        [DoNotNotify]
        public string AssetType { get; set; }
        [DoNotNotify]
        public string AssetSize { get; set; }
        [DoNotNotify]
        public double UnitVolume { get; set; }
        [DoNotNotify]
        public string UnitOfMeasure { get; set; }
        [DoNotNotify]
        public int SkuMultiplier { get; set; }
        [DoNotNotify]
        public string LogMethod { get; set; }
        [DoNotNotify]
        public string Brand { get; set; }
        [DoNotNotify]
        public string GTIN { get; set; }
        [DoNotNotify]
        public string GRAI { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public double Cost { get; set; }
        [DoNotNotify]
        public double ListPrice { get; set; }
        [DoNotNotify]
        public bool IsScanRequired { get; set; }
        [DoNotNotify]
        public bool IsActive { get; set; }
        [DoNotNotify]
        public string AssetProfileId { get; set; }
        [DoNotNotify]
        public string AssetProfileName { get; set; }
        [DoNotNotify]
        public string OwnerSkuCode { get; set; }
        [DoNotNotify]
        public string OwnerSkuName { get; set; }
        [DoNotNotify]
        public IList<Variation> Variations { get;}
        [DoNotNotify]
        public int UnitsPerPallet { get; set; }
        [DoNotNotify]
        public int PalletsPerPosition { get; set; }
        [DoNotNotify]
        public string Material { get; set; }
    }
}
