using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class ManifestModel : RealmObject
    {
        [PrimaryKey]
        public string ManifestId { get; set; }
        public long EventTypeId { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public DateTimeOffset ShipDate { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string OwnerName { get; set; }
        public int ManifestItemsCount { get; set; }
        public IList<BarcodeModel> BarcodeModels { get;}
        public IList<ManifestItem> ManifestItems { get; }
        public IList<NewPallet> NewPallets { get; }
        public IList<Tag> Tags { get; }
        public IList<string> ClosedBatches { get; }
        public IList<NewBatch> NewBatches { get; }
        public bool IsDraft { get; set; }
        public bool IsQueue { get; set; }
    }

    public class BarcodeModel : RealmObject
    {
        public string Barcode { get; set; }
        public string Icon { get; set; }
        public string TagsStr { get; set; }
        public IList<Tag> Tags { get; }
        public string Contents { get; set; }
        public string Page { get; set; }
        public bool HasMaintenaceVerified { get; set; }
        public bool IsScanned { get; set; }
        public Kegs Kegs { get; set; }
        public Pallets Pallets { get; set; }
        public KegIDResponse Response { get; set; }

    }
    public class Kegs : RealmObject
    {
        public IList<string> Contents { get; }
        public IList<string> Sizes { get; }
        public IList<string> Batches { get; }
        public IList<Partner> Partners { get; }
        public IList<Location> Locations { get; }
        public IList<MaintenanceItem> MaintenanceItems { get; }
        public IList<SkUs> SkUs { get; }
    }

    public class Location : RealmObject
    {
        public string Name { get; set; }
        public string TypeCode { get; set; }
        public string EntityId { get; set; }
    }

    public class MaintenanceItem : RealmObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public bool IsAction { get; set; }
        public string DefectType { get; set; }
        public string ActivationMethod { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public bool InUse { get; set; }
        public IList<string> ActivationPartnerTypes { get;}
    }

    public class Partner : RealmObject
    {
        public string PartnerId { get; set; }
        public IList<Keg> Kegs { get; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PartnerTypeName { get; set; }
        public string PartnerTypeCode { get; set; }
        public string LocationCode { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public bool IsInternal { get; set; }
        public bool IsShared { get; set; }
        public bool IsActive { get; set; }
        public bool PartnershipIsActive { get; set; }
        public string MasterCompanyId { get; set; }
        public string ParentPartnerId { get; set; }
        public string ParentPartnerName { get; set; }
        public string SourceKey { get; set; }
        public string LocationStatus { get; set; }
        public long? CompanyNo { get; set; }
    }

    public class Keg : RealmObject
    {
        public string KegId { get; set; }
        public string PartnerId { get; set; }
        public string Barcode { get; set; }
        public string AltBarcode { get; set; }
        public long Contents { get; set; }
        public long Batch { get; set; }
        public long Size { get; set; }
        public string Alert { get; set; }
        public long Location { get; set; }
        public IList<long> MaintenanceItems { get; }
        public IList<long> PendingMaintenanceItems { get;}
        public long? Sku { get; set; }
        public long? ContentsSku { get; set; }
    }

    public class SkUs : RealmObject
    {
        public string AssetOwnerName { get; set; }
        public string AssetOwnersSkuId { get; set; }
        public string SupplierName { get; set; }
        public string SkuId { get; set; }
        public string CompanyId { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string SkuClass { get; set; }
        public string AssetOwnerId { get; set; }
        public string SupplierId { get; set; }
        public string AssetType { get; set; }
        public string AssetSize { get; set; }
        public string BrandName { get; set; }
        public bool IsScanRequired { get; set; }
        public bool IsActive { get; set; }
        public string SourceKey { get; set; }
        public string ShortName { get; set; }
        public string AssetOwnerSkuId { get; set; }
        public string Barcode { get; set; }
    }

    public class Pallets : RealmObject
    {
        public IList<string> Contents { get; }
        public IList<string> PurplePallets { get; }
        public IList<string> Companies { get; }
        public IList<string> Skus { get; }
    }
}
