using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class ManifestModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string ManifestId { get; set; }
        [DoNotNotify]
        public long EventTypeId { get; set; }
        [DoNotNotify]
        public long Latitude { get; set; }
        [DoNotNotify]
        public long Longitude { get; set; }
        [DoNotNotify]
        public DateTimeOffset SubmittedDate { get; set; }
        [DoNotNotify]
        public DateTimeOffset ShipDate { get; set; }
        [DoNotNotify]
        public string OriginId { get; set; }
        [DoNotNotify]
        public string SenderId { get; set; }
        [DoNotNotify]
        public string DestinationId { get; set; }
        [DoNotNotify]
        public string ReceiverId { get; set; }
        [DoNotNotify]
        public string Gs1Gsin { get; set; }
        [DoNotNotify]
        public bool IsSendManifest { get; set; }
        [DoNotNotify]
        public DateTimeOffset EffectiveDate { get; set; }
        [DoNotNotify]
        public string OwnerName { get; set; }
        [DoNotNotify]
        public int ManifestItemsCount { get; set; }
        [DoNotNotify]
        public IList<BarcodeModel> BarcodeModels { get;}
        [DoNotNotify]
        public IList<ManifestTItem> ManifestItems { get; }
        [DoNotNotify]
        public IList<NewPallet> NewPallets { get; }
        [DoNotNotify]
        public MaintenanceModel MaintenanceModels { get; set; }
        [DoNotNotify]
        public IList<Tag> Tags { get; }
        [DoNotNotify]
        public string TagsStr { get; set; }
        [DoNotNotify]
        public string KegOrderId { get; set; }
        [DoNotNotify]
        public DateTimeOffset PostedDate { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public IList<string> ClosedBatches { get; }
        [DoNotNotify]
        public NewBatch NewBatch { get; set; }
        [DoNotNotify]
        public IList<NewBatch> NewBatches { get; }
        [DoNotNotify]
        public bool IsDraft { get; set; }
        [DoNotNotify]
        public bool IsQueue { get; set; }
        [DoNotNotify]
        public string Size { get; set; }
    }

    public class ManifestTItem : RealmObject
    {
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public DateTimeOffset ScanDate { get; set; }
        [DoNotNotify]
        public IList<Tag> Tags { get; }
        [DoNotNotify]
        public string Contents { get; set; }
        [DoNotNotify]
        public string KegId { get; set; }
        [DoNotNotify]
        public string PalletId { get; set; }
        [DoNotNotify]
        public string HeldOnPalletId { get; set; }
        [DoNotNotify]
        public string SkuId { get; set; }
        [DoNotNotify]
        public string BatchId { get; set; }
        [DoNotNotify]
        public string AssetProfileId { get; set; }
        [DoNotNotify]
        public string Icon { get; set; }
        [DoNotNotify]
        public string TagsStr { get; set; }

    }
    public class BarcodeModel : RealmObject
    {
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public string Icon { get; set; }
        [DoNotNotify]
        public string TagsStr { get; set; }
        [DoNotNotify]
        public IList<Tag> Tags { get; }
        [DoNotNotify]
        public string Contents { get; set; }
        [DoNotNotify]
        public string Page { get; set; }
        [DoNotNotify]
        public bool HasMaintenaceVerified { get; set; }
        [DoNotNotify]
        public bool IsScanned { get; set; }
        [DoNotNotify]
        public Kegs Kegs { get; set; }
        [DoNotNotify]
        public BPallets Pallets { get; set; }
        [DoNotNotify]
        public KegIDResponse Response { get; set; }

    }
    public class Kegs : RealmObject
    {
        [DoNotNotify]
        public IList<string> Contents { get; }
        [DoNotNotify]
        public IList<string> Sizes { get; }
        [DoNotNotify]
        public IList<string> Batches { get; }
        [DoNotNotify]
        public IList<Partner> Partners { get; }
        [DoNotNotify]
        public IList<Location> Locations { get; }
        [DoNotNotify]
        public IList<MaintenanceItem> MaintenanceItems { get; }
        [DoNotNotify]
        public IList<SkUs> SkUs { get; }
        [DoNotNotify]
        public IList<Profile> Profiles { get; }
    }

    public class Location : RealmObject
    {
        [DoNotNotify]
        public string Name { get; set; }
        [DoNotNotify]
        public string TypeCode { get; set; }
        [DoNotNotify]
        public string EntityId { get; set; }
    }

    public class MaintenanceItem : RealmObject
    {
        [DoNotNotify]
        public long Id { get; set; }
        [DoNotNotify]
        public string Name { get; set; }
        [DoNotNotify]
        public string Description { get; set; }
        [DoNotNotify]
        public bool IsAlert { get; set; }
        [DoNotNotify]
        public bool IsAction { get; set; }
        [DoNotNotify]
        public string DefectType { get; set; }
        [DoNotNotify]
        public string ActivationMethod { get; set; }
        [DoNotNotify]
        public DateTimeOffset? DeletedDate { get; set; }
        [DoNotNotify]
        public bool InUse { get; set; }
        [DoNotNotify]
        public IList<string> ActivationPartnerTypes { get;}
    }

    public class Partner : RealmObject
    {
        [DoNotNotify]
        public string PartnerId { get; set; }
        [DoNotNotify]
        public IList<Keg> Kegs { get; }
        [DoNotNotify]
        public string FullName { get; set; }
        [DoNotNotify]
        public string Address { get; set; }
        [DoNotNotify]
        public string Address1 { get; set; }
        [DoNotNotify]
        public string City { get; set; }
        [DoNotNotify]
        public string State { get; set; }
        [DoNotNotify]
        public string PostalCode { get; set; }
        [DoNotNotify]
        public string Country { get; set; }
        [DoNotNotify]
        public string PhoneNumber { get; set; }
        [DoNotNotify]
        public string PartnerTypeName { get; set; }
        [DoNotNotify]
        public string PartnerTypeCode { get; set; }
        [DoNotNotify]
        public string LocationCode { get; set; }
        [DoNotNotify]
        public double Lat { get; set; }
        [DoNotNotify]
        public double Lon { get; set; }
        [DoNotNotify]
        public bool IsInternal { get; set; }
        [DoNotNotify]
        public bool IsShared { get; set; }
        [DoNotNotify]
        public bool IsActive { get; set; }
        [DoNotNotify]
        public bool PartnershipIsActive { get; set; }
        [DoNotNotify]
        public string MasterCompanyId { get; set; }
        [DoNotNotify]
        public string ParentPartnerId { get; set; }
        [DoNotNotify]
        public string ParentPartnerName { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public string LocationStatus { get; set; }
        [DoNotNotify]
        public int CompanyNo { get; set; }
    }

    public class Keg : RealmObject
    {
        [DoNotNotify]
        public string KegId { get; set; }
        [DoNotNotify]
        public string PartnerId { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public string AltBarcode { get; set; }
        [DoNotNotify]
        public long Contents { get; set; }
        [DoNotNotify]
        public long Batch { get; set; }
        [DoNotNotify]
        public long Size { get; set; }
        [DoNotNotify]
        public string Alert { get; set; }
        [DoNotNotify]
        public long Location { get; set; }
        [DoNotNotify]
        public IList<long> MaintenanceItems { get; }
        [DoNotNotify]
        public IList<long> PendingMaintenanceItems { get;}
        [DoNotNotify]
        public long? Sku { get; set; }
        [DoNotNotify]
        public long? ContentsSku { get; set; }
    }

    public class SkUs : RealmObject
    {
        [DoNotNotify]
        public string AssetOwnerName { get; set; }
        [DoNotNotify]
        public string AssetOwnersSkuId { get; set; }
        [DoNotNotify]
        public string SupplierName { get; set; }
        [DoNotNotify]
        public string SkuId { get; set; }
        [DoNotNotify]
        public string CompanyId { get; set; }
        [DoNotNotify]
        public string SkuCode { get; set; }
        [DoNotNotify]
        public string SkuName { get; set; }
        [DoNotNotify]
        public string SkuClass { get; set; }
        [DoNotNotify]
        public string AssetOwnerId { get; set; }
        [DoNotNotify]
        public string SupplierId { get; set; }
        [DoNotNotify]
        public string AssetType { get; set; }
        [DoNotNotify]
        public string AssetSize { get; set; }
        [DoNotNotify]
        public string BrandName { get; set; }
        [DoNotNotify]
        public bool IsScanRequired { get; set; }
        [DoNotNotify]
        public bool IsActive { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public string ShortName { get; set; }
        [DoNotNotify]
        public string AssetOwnerSkuId { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
    }

    public class BPallets : RealmObject
    {
        [DoNotNotify]
        public IList<string> Contents { get; }
        [DoNotNotify]
        public IList<string> Pallets { get; }
        [DoNotNotify]
        public IList<string> MaintenanceItems { get; }
        [DoNotNotify]
        public IList<string> Companies { get; }
        [DoNotNotify]
        public IList<string> Skus { get; }
        [DoNotNotify]
        public IList<string> Profiles { get; }
    }
}
