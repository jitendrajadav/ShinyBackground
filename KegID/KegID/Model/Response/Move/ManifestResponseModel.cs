using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class ManifestResponseModel 
    {
        public KegIDResponse Response { get; set; }
        public string ReceivedDate { get; set; }
        public CreatorCompany CreatorCompany { get; set; }
        public object CreatorUser { get; set; }
        public object SenderUser { get; set; }
        public object SenderUserLocation { get; set; }
        public object ReceiverUser { get; set; }
        public object ReceiverUserLocation { get; set; }
        public string SenderCreatedDate { get; set; }
        public string SenderSubmittedDate { get; set; }
        public string ReceiverCreatedDate { get; set; }
        public string ReceiverSubmittedDate { get; set; }
        public string ManifestId { get; set; }
        public string TrackingNumber { get; set; }
        public string EventType { get; set; }
        public string ShipDate { get; set; }
        public string ManifestDate { get; set; }
        public List<CreatedManifestItem> ManifestItems { get; set; }
        public List<object> ManualCountQuantities { get; set; }
        public ManifestUser ManifestUser { get; set; }
        public ManifestUserLocation ManifestUserLocation { get; set; }
        public string SubmittedDate { get; set; }
        public CreatorCompany SenderPartner { get; set; }
        public Address SenderShipAddress { get; set; }
        public Address SenderBillAddress { get; set; }
        public object SenderContactName { get; set; }
        public object SenderContactEmail { get; set; }
        public object SenderContactPhone { get; set; }
        public object SenderReferenceKey { get; set; }
        public object SenderNotes { get; set; }
        public CreatorCompany ReceiverPartner { get; set; }
        public Address ReceiverShipAddress { get; set; }
        public Address ReceiverBillAddress { get; set; }
        public object ReceiverContactName { get; set; }
        public object ReceiverContactEmail { get; set; }
        public object ReceiverContactPhone { get; set; }
        public object ReceiverReferenceKey { get; set; }
        public object ReceiverNotes { get; set; }
        public List<Tag> Tags { get; set; }
        public object Gs1Id { get; set; }
        public object EffectiveDate { get; set; }
        public object KegOrder { get; set; }
    }

    public class CreatorCompany
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
        public string CompanyNo { get; set; }
    }

    public class CreatedManifestItem
    {
        public object DataInfo { get; set; }
        public string ManifestItemId { get; set; }
        public string Contents { get; set; }
        public object ContentsKey { get; set; }
        public string Barcode { get; set; }
        public object SkuId { get; set; }
        public object BatchId { get; set; }
        public long Quantity { get; set; }
        public object HandlingCondition { get; set; }
        public CreatedManifestKeg Keg { get; set; }
        public Pallet Pallet { get; set; }
        public DateTimeOffset ScanDate { get; set; }
        public object Notes { get; set; }
        public object ReferenceKey { get; set; }
        public List<GeneralTag> Tags { get; set; }
        public string FullStatus { get; set; }
        public DateTimeOffset SenderScanDate { get; set; }
        public object SenderNotes { get; set; }
        public object SenderReferenceKey { get; set; }
        public bool IsReceived { get; set; }
        public object ReceiverScanDate { get; set; }
        public object ReceiverNotes { get; set; }
        public object ReceiverReferenceKey { get; set; }
    }

    public class CreatedManifestKeg
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
        public object PalletId { get; set; }
        public object SkuId { get; set; }
        public object SkuCode { get; set; }
        public object SkuName { get; set; }
        public object SlgSkuId { get; set; }
        public object SlgSkuCode { get; set; }
        public object SlgSkuName { get; set; }
    }

    public class ManifestUser
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ReferenceKey { get; set; }
        public bool IsOperator { get; set; }
        public object SessionId { get; set; }
    }

    public class ManifestUserLocation
    {
        public long Lat { get; set; }
        public long Lon { get; set; }
    }

}
