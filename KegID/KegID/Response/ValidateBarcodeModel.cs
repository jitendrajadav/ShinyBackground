using SQLite;
using System.Collections.Generic;

namespace KegID.Response
{
    public class ValidateBarcodeModel
    {
        public Kegs Kegs { get; set; }
        public Pallets Pallets { get; set; }
    }

    public class Kegs
    {
        public List<string> Contents { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> Batches { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Location> Locations { get; set; }
        public List<MaintenanceItem> MaintenanceItems { get; set; }
        public List<SkUs> SkUs { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public string TypeCode { get; set; }
        public string EntityId { get; set; }
    }

    public class MaintenanceItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAlert { get; set; }
        public bool IsAction { get; set; }
        public string DefectType { get; set; }
        public string ActivationMethod { get; set; }
        public object DeletedDate { get; set; }
        public bool InUse { get; set; }
        public List<string> ActivationPartnerTypes { get; set; }
    }

    public class Partner
    {
        public List<Keg> Kegs { get; set; }
        public string PartnerId { get; set; }
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

    public class Keg
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public string AltBarcode { get; set; }
        public long Contents { get; set; }
        public long Batch { get; set; }
        public long Size { get; set; }
        public string Alert { get; set; }
        public long Location { get; set; }
        public List<long> MaintenanceItems { get; set; }
        public List<long> PendingMaintenanceItems { get; set; }
        public long? Sku { get; set; }
        public long? ContentsSku { get; set; }
    }

    public class SkUs
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

    public class Pallets
    {
        public List<object> Contents { get; set; }
        public List<object> PurplePallets { get; set; }
        public List<object> Companies { get; set; }
        public List<object> Skus { get; set; }
    }

}
