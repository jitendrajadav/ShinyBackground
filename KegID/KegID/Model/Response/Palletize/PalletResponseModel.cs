using KegID.Model.PrintPDF;
using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletResponseModel 
    {
        [DoNotNotify]
        public KegIDResponse Response { get; set; }
        [DoNotNotify]
        public string PalletId { get; set; }
        [DoNotNotify]
        public Owner Owner { get; set; }
        [DoNotNotify]
        public DateTimeOffset BuildDate { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public Owner StockLocation { get; set; }
        [DoNotNotify]
        public Owner Location { get; set; }
        [DoNotNotify]
        public List<Tag> Tags { get; set; }
        [DoNotNotify]
        public List<PalletItem> PalletItems { get; set; }
        [DoNotNotify]
        public TargetLocation TargetLocation { get; set; }
        [DoNotNotify]
        public DateTimeOffset CreatedDate { get; set; }
        [DoNotNotify]
        public object Container { get; set; }
        [DoNotNotify]
        public object ReferenceKey { get; set; }
        [DoNotNotify]
        public object DataInfo { get; set; }
    }

    public class PalletLocation
    {
        [DoNotNotify]
        public string PartnerId { get; set; }
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
        public long Lat { get; set; }
        [DoNotNotify]
        public long Lon { get; set; }
        [DoNotNotify]
        public bool IsInternal { get; set; }
        [DoNotNotify]
        public bool IsShared { get; set; }
        [DoNotNotify]
        public bool IsActive { get; set; }
        [DoNotNotify]
        public bool PartnershipIsActive { get; set; }
        [DoNotNotify]
        public object MasterCompanyId { get; set; }
        [DoNotNotify]
        public string ParentPartnerId { get; set; }
        [DoNotNotify]
        public string ParentPartnerName { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public string LocationStatus { get; set; }
        [DoNotNotify]
        public object CompanyNo { get; set; }
    }

    public class Owner
    {
        [DoNotNotify]
        public string PartnerId { get; set; }
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
        public bool isInternal { get; set; }
        [DoNotNotify]
        public bool isShared { get; set; }
        [DoNotNotify]
        public bool isActive { get; set; }
        [DoNotNotify]
        public bool PartnershipIsActive { get; set; }
        [DoNotNotify]
        public object MasterCompanyId { get; set; }
        [DoNotNotify]
        public object ParentPartnerId { get; set; }
        [DoNotNotify]
        public object ParentPartnerName { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public string LocationStatus { get; set; }
        [DoNotNotify]
        public int CompanyNo { get; set; }
    }

    public class PalletItem : RealmObject
    {
        [DoNotNotify]
        public string PalletId { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public DateTimeOffset DateScanned { get; set; }
        [DoNotNotify]
        public string Contents { get; set; }
        [DoNotNotify]
        public string ContentsSkuId { get; set; }
        [DoNotNotify]
        public string BuildBatchId { get; set; }
        [DoNotNotify]
        public string BuildBatchCode { get; set; }
        [DoNotNotify]
        public string BuildSkuId { get; set; }
        [DoNotNotify]
        public PalletKeg Keg { get; set; }
        [DoNotNotify]
        public bool IsActive { get; set; }
        [DoNotNotify]
        public string RemovedManifest { get; set; }
        [DoNotNotify]
        public IList<Tag> Tags { get;}
        [DoNotNotify]
        public DateTimeOffset ScanDate { get; set; }
    }

    public class PalletKeg : RealmObject
    {
        [DoNotNotify]
        public string KegId { get; set; }
        [DoNotNotify]
        public string OwnerId { get; set; }
        [DoNotNotify]
        public string OwnerName { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public string SizeName { get; set; }
        [DoNotNotify]
        public string AssetSizeSourceKey { get; set; }
        [DoNotNotify]
        public string TypeName { get; set; }
        [DoNotNotify]
        public string AssetTypeSourceKey { get; set; }
        [DoNotNotify]
        public string VolumeName { get; set; }
        [DoNotNotify]
        public string AssetVolumeSourceKey { get; set; }
        [DoNotNotify]
        public string LocationId { get; set; }
        [DoNotNotify]
        public string LocationName { get; set; }
        [DoNotNotify]
        public string Contents { get; set; }
        [DoNotNotify]
        public DateTimeOffset ReceivedDate { get; set; }
        [DoNotNotify]
        public string PalletId { get; set; }
        [DoNotNotify]
        public string PalletName { get; set; }
    }

}
