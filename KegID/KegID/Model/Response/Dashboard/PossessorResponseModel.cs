using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PossessorResponseModel : RealmObject
    {
        public PossessorLocation Location { get; set; }
        public long KegsHeld { get; set; }
        public long Oldest { get; set; }
        public long AvgDays { get; set; }
        public bool HasOverdueKegs { get; set; }
        public string KegAgeStatus { get; set; }
        public string Icon { get; set; }
    }

    public class PossessorLocation : RealmObject
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
        public long CompanyNo { get; set; }
    }

    public class PossessorModel : KegIDResponse
    {
        public IList<PossessorResponseModel> PossessorResponseModel { get; set; }
    }
}
