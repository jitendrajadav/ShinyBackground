using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletResponseModel : KegIDResponse
    {
        public string PalletId { get; set; }
        public Owner Owner { get; set; }
        public DateTime BuildDate { get; set; }
        public string Barcode { get; set; }
        public PalletLocation StockLocation { get; set; }
        public PalletLocation Location { get; set; }
        public List<Tag> Tags { get; set; }
        public List<PalletItem> PalletItems { get; set; }
        public object TargetLocation { get; set; }
        public DateTime CreatedDate { get; set; }
        public object Container { get; set; }
        public object ReferenceKey { get; set; }
        public object DataInfo { get; set; }
    }

    public class PalletLocation
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
        public object MasterCompanyId { get; set; }
        public string ParentPartnerId { get; set; }
        public string ParentPartnerName { get; set; }
        public string SourceKey { get; set; }
        public string LocationStatus { get; set; }
        public object CompanyNo { get; set; }
    }

    public class Owner
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
        public object MasterCompanyId { get; set; }
        public object ParentPartnerId { get; set; }
        public object ParentPartnerName { get; set; }
        public string SourceKey { get; set; }
        public string LocationStatus { get; set; }
        public long CompanyNo { get; set; }
    }

    public class PalletItem
    {
        //[PrimaryKey]
        //public string PalletId { get; set; }
        public string Barcode { get; set; }
        public DateTime ScanDate { get; set; }
        public long ValidationStatus { get; set; }
        //public DateTime DateScanned { get; set; }
        //public string Contents { get; set; }
        //[Ignore]
        //public PalletKeg Keg { get; set; }
        //public bool IsActive { get; set; }
        //public string RemovedManifest { get; set; }
        [Ignore]
        public List<Tag> Tags { get; set; }
    }

    public class PalletKeg
    {
        public string KegId { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Barcode { get; set; }
        public string SizeName { get; set; }
        public object AssetSizeSourceKey { get; set; }
        public string TypeName { get; set; }
        public object AssetTypeSourceKey { get; set; }
        public string VolumeName { get; set; }
        public object AssetVolumeSourceKey { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string Contents { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string PalletId { get; set; }
        public object SkuId { get; set; }
        public object SkuCode { get; set; }
        public object SkuName { get; set; }
        public object SlgSkuId { get; set; }
        public object SlgSkuCode { get; set; }
        public object SlgSkuName { get; set; }
    }

}
