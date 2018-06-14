using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class KegStatusResponseModel
    {
        public KegIDResponse Response { get; set; }
        public string KegId { get; set; }
        public Owner Owner { get; set; }
        public string Barcode { get; set; }
        public object KegName { get; set; }
        public string TypeName { get; set; }
        public string AssetTypeSourceKey { get; set; }
        public int TypeId { get; set; }
        public string SizeName { get; set; }
        public string AssetSizeSourceKey { get; set; }
        public object SizeId { get; set; }
        public string VolumeName { get; set; }
        public string AssetVolumeSourceKey { get; set; }
        public string AssetDesc { get; set; }
        public object AltBarcode { get; set; }
        public object Notes { get; set; }
        public object ReferenceKey { get; set; }
        public string FixedContents { get; set; }
        public List<object> Tags { get; set; }
        public string ProfileId { get; set; }
        public string AssetProfileName { get; set; }
        public string SkuId { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public KegStatusLocation Location { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
        public Pallet Pallet { get; set; }
        public string Contents { get; set; }
        public string Batch { get; set; }
        public string ContentsSkuId { get; set; }
        public string ContentsSkuCode { get; set; }
        public string ContentsSkuName { get; set; }
        public string BatchId { get; set; }
        public string Status { get; set; }
        public bool isActive { get; set; }
        public List<MaintenanceAlert> MaintenanceAlerts { get; set; }
        public object PurchaseDate { get; set; }
        public object PurchasePrice { get; set; }
        public object PurchaseOrder { get; set; }
        public object ManufacturerName { get; set; }
        public object ManufacturerId { get; set; }
        public object ManufactureLocation { get; set; }
        public object ManufactureDate { get; set; }
        public object Material { get; set; }
        public object Markings { get; set; }
        public object Colors { get; set; }
        public Profile Profile { get; set; }

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
        public bool isInternal { get; set; }
        public bool isShared { get; set; }
        public bool isActive { get; set; }
        public bool PartnershipIsActive { get; set; }
        public object MasterCompanyId { get; set; }
        public object ParentPartnerId { get; set; }
        public object ParentPartnerName { get; set; }
        public string SourceKey { get; set; }
        public string LocationStatus { get; set; }
        public int CompanyNo { get; set; }

    }

    public class MaintenanceAlert
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public long Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string DefectType { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public bool IsActivated { get; set; }
        public string ActivationMethod { get; set; }
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
