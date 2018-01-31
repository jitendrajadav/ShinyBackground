using System.Collections.Generic;

namespace KegID.Model
{
    public class NewPartnerRequestModel
    {
        public string PartnerId { get; set; }
        public string ParentPartnerId { get; set; }
        public string PartnerName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PartnerTypeCode { get; set; }
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
        public string ContactName { get; set; }
        public string RouteName { get; set; }
        public string AccountNumber { get; set; }
        public string Notes { get; set; }
        public string PrivateKey { get; set; }
        public List<Tag> Tags { get; set; }
        public string LocationStatus { get; set; }
    }

    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public bool Geocoded { get; set; }
    }
}
