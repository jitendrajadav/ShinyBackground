﻿using System.Collections.Generic;

namespace KegID.Model
{
    public partial class PartnerInfoResponseModel : KegIDResponse
    {
        public string PartnerId { get; set; }
        public string CompanyNo { get; set; }
        public string ParentPartnerId { get; set; }
        public string ParentPartnerName { get; set; }
        public string MasterCompanyId { get; set; }
        public string MasterCompanyName { get; set; }
        public string FullName { get; set; }
        public string PartnerName { get; set; }
        public string ShortName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PartnerTypeId { get; set; }
        public string PartnerTypeCode { get; set; }
        public string PartnerTypeName { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public Address ShipAddress { get; set; }
        public Address BillAddress { get; set; }
        public bool IsShared { get; set; }
        public bool IsInternal { get; set; }
        public string LocationCode { get; set; }
        public string TimeZone { get; set; }
        public string ReferenceKey { get; set; }
        public string ContactEmail { get; set; }
        public bool IsNotify { get; set; }
        public string Phone { get; set; }
        public string SmsAddress { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public bool IsMaster { get; set; }
        public bool IsActive { get; set; }
        public bool PartnershipIsActive { get; set; }
        public bool IsPartner { get; set; }
        public string ContactName { get; set; }
        public string RouteName { get; set; }
        public string AccountNumber { get; set; }
        public string Notes { get; set; }
        public string PrivateKey { get; set; }
        public long KegsHeld { get; set; }
        public long OldestKeg { get; set; }
        public long AvgDays { get; set; }
        public bool HasOverdueKegs { get; set; }
        public object Tags { get; set; }
        public Dictionary<string, object> BusinessHours { get; set; }
        public Dictionary<string, object> ReceivingHours { get; set; }
        public string OwnerCompanyId { get; set; }
        public string LocationStatus { get; set; }
        public string KegAgeStatus { get; set; }
        public string CreatedByCompanyId { get; set; }
    }

}
