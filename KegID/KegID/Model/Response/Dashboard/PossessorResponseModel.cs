using PropertyChanged;
using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PossessorResponseModel : RealmObject
    {
        [DoNotNotify]
        public PossessorLocation Location { get; set; }
        [DoNotNotify]
        public long KegsHeld { get; set; }
        [DoNotNotify]
        public long Oldest { get; set; }
        [DoNotNotify]
        public long AvgDays { get; set; }
        [DoNotNotify]
        public bool HasOverdueKegs { get; set; }
        [DoNotNotify]
        public string KegAgeStatus { get; set; }
        [DoNotNotify]
        public string Icon { get; set; }
        [DoNotNotify]
        public string ContainerTypes { get; set; }
    }

    public class PossessorLocation : RealmObject
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
        public long CompanyNo { get; set; }
    }

    public class PossessorModel
    {
        public KegIDResponse Response { get; set; }
        public IList<PossessorResponseModel> PossessorResponseModel { get; set; }
    }
}
