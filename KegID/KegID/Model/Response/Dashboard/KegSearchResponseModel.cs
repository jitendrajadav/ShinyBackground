using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class KegSearchResponseModel
    {
        public string KegId { get; set; }
        public KegSearchLocation Owner { get; set; }
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
        public List<Tag> Tags { get; set; }
        public string ProfileId { get; set; }
        public string AssetProfileName { get; set; }
        public string SkuId { get; set; }
        public string SkuCode { get; set; }
        public string SkuName { get; set; }
        public KegSearchLocation Location { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
        public Pallet Pallet { get; set; }
        public string Contents { get; set; }
        public string Batch { get; set; }
        public string ContentsSkuId { get; set; }
        public string ContentsSkuCode { get; set; }
        public string ContentsSkuName { get; set; }
        public string BatchId { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public List<MaintenanceAlert> MaintenanceAlerts { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public long PurchasePrice { get; set; }
        public string PurchaseOrder { get; set; }
        public string ManufacturerName { get; set; }
        public string ManufacturerId { get; set; }
        public string ManufactureLocation { get; set; }
        public DateTimeOffset ManufactureDate { get; set; }
        public string Material { get; set; }
        public string Markings { get; set; }
        public string Colors { get; set; }
        public Profile Profile { get; set; }
    }

    public class KegSearchLocation
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
        public long Lat { get; set; }
        public long Lon { get; set; }
        public bool IsInternal { get; set; }
        public bool IsShared { get; set; }
        public bool IsActive { get; set; }
        public bool PartnershipIsActive { get; set; }
        public string MasterCompanyId { get; set; }
        public string ParentPartnerId { get; set; }
        public string ParentPartnerName { get; set; }
        public string SourceKey { get; set; }
        public string LocationStatus { get; set; }
        public long CompanyNo { get; set; }
    }

    public class Pallet
    {
        public string PalletId { get; set; }
        public string Barcode { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public int ActiveCount { get; set; }
        public int BuildCount { get; set; }
        public DateTimeOffset BuildDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }

    public class Profile : RealmObject
    {
        public string ProfileId { get; set; }
        public string OwnerId { get; set; }
        public string FleetName { get; set; }
        public string ProfileName { get; set; }
        public string ProfileCode { get; set; }
        public string ProfileType { get; set; }
        public string ProfileSize { get; set; }
        public string Variation { get; set; }
        public long VolumeUnits { get; set; }
        public string VolumeUom { get; set; }
        public string ProfileVolume { get; set; }
        public string SourceKey { get; set; }
        public DateTimeOffset SourceModified { get; set; }
        public string SourceSystem { get; set; }
    }

    public class KegSearchModel
    {
        public KegIDResponse Response { get; set; }
        public IList<KegSearchResponseModel> KegSearchResponseModel { get; set; }
    }
}
