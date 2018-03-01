using System.Collections.Generic;

namespace KegID.Model
{
    public class KegStatusResponseModel : KegIDResponse
    {
        public string KegId { get; set; }
        public Location Owner { get; set; }
        public string Barcode { get; set; }
        public string KegName { get; set; }
        public string TypeName { get; set; }
        public string AssetTypeSourceKey { get; set; }
        public string TypeId { get; set; }
        public string SizeName { get; set; }
        public string AssetSizeSourceKey { get; set; }
        public string SizeId { get; set; }
        public string VolumeName { get; set; }
        public string AssetVolumeSourceKey { get; set; }
        public string AssetDesc { get; set; }
        public string AltBarcode { get; set; }
        public string Notes { get; set; }
        public string ReferenceKey { get; set; }
        public string FixedContents { get; set; }
        public List<string> Tags { get; set; }
        public string SkuId { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public string SlgSkuId { get; set; }
        public string SlgSkuCode { get; set; }
        public string SlgSkuName { get; set; }
        public KegStatusLocation Location { get; set; }
        public string ReceivedDate { get; set; }
        public string Pallet { get; set; }
        public string Contents { get; set; }
        public string Batch { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public List<MaintenanceAlert> MaintenanceAlerts { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchasePrice { get; set; }
        public string PurchaseOrder { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufactureLocation { get; set; }
        public string ManufactureDate { get; set; }
        public string Material { get; set; }
        public string Markings { get; set; }
        public string Colors { get; set; }
    }

    public class KegStatusLocation
    {
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

    public class MaintenanceAlert
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public long Id { get; set; }
        public long TypeId { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string DefectType { get; set; }
        public string DueDate { get; set; }
        public bool IsActivated { get; set; }
        public long ActivationMethod { get; set; }
        public string Message { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string AssetSize { get; set; }
        public string AssetType { get; set; }
        public string PalletId { get; set; }
        public string PalletBarcode { get; set; }
    }

}
