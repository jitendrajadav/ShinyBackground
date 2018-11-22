using KegID.Model.PrintPDF;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletResponseModel 
    {
        public KegIDResponse Response { get; set; }
        public string PalletId { get; set; }
        public Owner Owner { get; set; }
        public DateTimeOffset BuildDate { get; set; }
        public string Barcode { get; set; }
        public Owner StockLocation { get; set; }
        public Owner Location { get; set; }
        public List<Tag> Tags { get; set; }
        public List<PalletItem> PalletItems { get; set; }
        public TargetLocation TargetLocation { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
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

    public class PalletItem : RealmObject
    {
        public string PalletId { get; set; }
        public string Barcode { get; set; }
        public DateTimeOffset DateScanned { get; set; }
        public string Contents { get; set; }
        public string ContentsSkuId { get; set; }
        public string BuildBatchId { get; set; }
        public string BuildBatchCode { get; set; }
        public string BuildSkuId { get; set; }
        public PalletKeg Keg { get; set; }
        public bool IsActive { get; set; }
        public string RemovedManifest { get; set; }
        public IList<Tag> Tags { get;}
        public DateTimeOffset ScanDate { get; set; }
    }

    public class PalletKeg : RealmObject
    {
        public string KegId { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Barcode { get; set; }
        public string SizeName { get; set; }
        public string AssetSizeSourceKey { get; set; }
        public string TypeName { get; set; }
        public string AssetTypeSourceKey { get; set; }
        public string VolumeName { get; set; }
        public string AssetVolumeSourceKey { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string Contents { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
        public string PalletId { get; set; }
        public string PalletName { get; set; }
    }

}
