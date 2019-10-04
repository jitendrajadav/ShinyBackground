using PropertyChanged;
using Realms;
using System;

namespace KegID.Model
{
    public class ValidatePartnerModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DoNotNotify]
        public string Barcode { get; set; }
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
        public long? CompanyNo { get; set; }
        [DoNotNotify]
        public string Contents { get; internal set; }
        [DoNotNotify]
        public string Size { get; internal set; }
        [DoNotNotify]
        public string Batch { get; internal set; }
        [DoNotNotify]
        public string Location { get; internal set; }
    }
}
