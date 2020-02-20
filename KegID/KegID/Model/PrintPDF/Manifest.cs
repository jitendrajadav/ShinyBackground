using System.Collections.Generic;
using System.Xml.Serialization;

namespace KegID.Model.PrintPDF
{
    public class ShipDate
    {
        [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string OffsetMinutes { get; set; }
        [XmlAttribute(AttributeName = "d2p1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D2p1 { get; set; }
    }

    [XmlRoot(ElementName = "SubmittedDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SubmittedDate
    {
        [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string OffsetMinutes { get; set; }
        [XmlAttribute(AttributeName = "d2p1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D2p1 { get; set; }
    }

    [XmlRoot(ElementName = "ManifestUser", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ManifestUser
    {
        [XmlElement(ElementName = "UserId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string UserId { get; set; }
        [XmlElement(ElementName = "FullName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string FullName { get; set; }
        [XmlElement(ElementName = "CompanyId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string CompanyId { get; set; }
        [XmlElement(ElementName = "Email", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Email { get; set; }
        [XmlElement(ElementName = "CompanyName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string CompanyName { get; set; }
    }

    [XmlRoot(ElementName = "MasterCompanyId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class MasterCompanyId
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ParentPartnerId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ParentPartnerId
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ParentPartnerName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ParentPartnerName
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "SenderPartner", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderPartner
    {
        [XmlElement(ElementName = "Address", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Address { get; set; }
        [XmlElement(ElementName = "FullName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string FullName { get; set; }
        [XmlElement(ElementName = "LocationCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string LocationCode { get; set; }
        [XmlElement(ElementName = "MasterCompanyId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public MasterCompanyId MasterCompanyId { get; set; }
        [XmlElement(ElementName = "ParentPartnerId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ParentPartnerId ParentPartnerId { get; set; }
        [XmlElement(ElementName = "ParentPartnerName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ParentPartnerName ParentPartnerName { get; set; }
        [XmlElement(ElementName = "PartnerId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PartnerId { get; set; }
        [XmlElement(ElementName = "PartnerTypeCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PartnerTypeCode { get; set; }
        [XmlElement(ElementName = "PartnerTypeName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PartnerTypeName { get; set; }
        [XmlElement(ElementName = "Lat", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Lat { get; set; }
        [XmlElement(ElementName = "Lon", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Lon { get; set; }
        [XmlElement(ElementName = "isActive", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string IsActive { get; set; }
        [XmlElement(ElementName = "isInternal", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string IsInternal { get; set; }
        [XmlElement(ElementName = "isShared", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string IsShared { get; set; }
        [XmlElement(ElementName = "Address1", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Address1 { get; set; }
        [XmlElement(ElementName = "State", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string State { get; set; }
        [XmlElement(ElementName = "City", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string City { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PostalCode { get; set; }
    }

    [XmlRoot(ElementName = "Line2", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Line2
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "Line3", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Line3
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "Line4", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Line4
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "Line5", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Line5
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "SenderShipAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderShipAddress
    {
        [XmlElement(ElementName = "Line1", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Line1 { get; set; }
        [XmlElement(ElementName = "Line2", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line2 Line2 { get; set; }
        [XmlElement(ElementName = "Line3", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line3 Line3 { get; set; }
        [XmlElement(ElementName = "Line4", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line4 Line4 { get; set; }
        [XmlElement(ElementName = "Line5", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line5 Line5 { get; set; }
        [XmlElement(ElementName = "City", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string City { get; set; }
        [XmlElement(ElementName = "State", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string State { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Country", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Country { get; set; }
    }

    [XmlRoot(ElementName = "SenderBillAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderBillAddress
    {
        [XmlElement(ElementName = "Line1", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Line1 { get; set; }
        [XmlElement(ElementName = "Line2", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line2 Line2 { get; set; }
        [XmlElement(ElementName = "Line3", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line3 Line3 { get; set; }
        [XmlElement(ElementName = "Line4", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line4 Line4 { get; set; }
        [XmlElement(ElementName = "Line5", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line5 Line5 { get; set; }
        [XmlElement(ElementName = "City", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string City { get; set; }
        [XmlElement(ElementName = "State", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string State { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Country", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Country { get; set; }
    }

    [XmlRoot(ElementName = "SenderContactName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderContactName
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "SenderContactEmail", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderContactEmail
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "SenderContactPhone", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderContactPhone
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "SenderReferenceKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderReferenceKey
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "SenderNotes", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SenderNotes
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverContactName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverContactName
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverContactEmail", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverContactEmail
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverContactPhone", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverContactPhone
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverReferenceKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverReferenceKey
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverNotes", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverNotes
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverPartner", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverPartner
    {
        [XmlElement(ElementName = "Address", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Address { get; set; }
        [XmlElement(ElementName = "FullName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string FullName { get; set; }
        [XmlElement(ElementName = "LocationCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string LocationCode { get; set; }
        [XmlElement(ElementName = "MasterCompanyId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public MasterCompanyId MasterCompanyId { get; set; }
        [XmlElement(ElementName = "ParentPartnerId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string ParentPartnerId { get; set; }
        [XmlElement(ElementName = "ParentPartnerName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string ParentPartnerName { get; set; }
        [XmlElement(ElementName = "PartnerId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PartnerId { get; set; }
        [XmlElement(ElementName = "PartnerTypeCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PartnerTypeCode { get; set; }
        [XmlElement(ElementName = "PartnerTypeName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PartnerTypeName { get; set; }
        [XmlElement(ElementName = "Lat", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Lat { get; set; }
        [XmlElement(ElementName = "Lon", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Lon { get; set; }
        [XmlElement(ElementName = "isActive", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string IsActive { get; set; }
        [XmlElement(ElementName = "isInternal", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string IsInternal { get; set; }
        [XmlElement(ElementName = "isShared", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string IsShared { get; set; }
        [XmlElement(ElementName = "Address1", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Address1 { get; set; }
        [XmlElement(ElementName = "State", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string State { get; set; }
        [XmlElement(ElementName = "City", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string City { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PostalCode { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverShipAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverShipAddress
    {
        [XmlElement(ElementName = "Line1", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Line1 { get; set; }
        [XmlElement(ElementName = "Line2", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line2 Line2 { get; set; }
        [XmlElement(ElementName = "Line3", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line3 Line3 { get; set; }
        [XmlElement(ElementName = "Line4", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line4 Line4 { get; set; }
        [XmlElement(ElementName = "Line5", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line5 Line5 { get; set; }
        [XmlElement(ElementName = "City", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string City { get; set; }
        [XmlElement(ElementName = "State", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string State { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Country", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Country { get; set; }
    }

    [XmlRoot(ElementName = "ReceiverBillAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceiverBillAddress
    {
        [XmlElement(ElementName = "Line1", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Line1 { get; set; }
        [XmlElement(ElementName = "Line2", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line2 Line2 { get; set; }
        [XmlElement(ElementName = "Line3", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line3 Line3 { get; set; }
        [XmlElement(ElementName = "Line4", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line4 Line4 { get; set; }
        [XmlElement(ElementName = "Line5", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Line5 Line5 { get; set; }
        [XmlElement(ElementName = "City", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string City { get; set; }
        [XmlElement(ElementName = "State", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string State { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Country", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Country { get; set; }
    }

    [XmlRoot(ElementName = "ReceivedDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReceivedDate
    {
        [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string OffsetMinutes { get; set; }
        [XmlAttribute(AttributeName = "d2p1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D2p1 { get; set; }
    }

    [XmlRoot(ElementName = "PalletName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class PalletName
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "Keg", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Keg
    {
        [XmlElement(ElementName = "KegId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string KegId { get; set; }
        [XmlElement(ElementName = "Barcode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Barcode { get; set; }
        [XmlElement(ElementName = "OwnerId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string OwnerId { get; set; }
        [XmlElement(ElementName = "OwnerName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string OwnerName { get; set; }
        [XmlElement(ElementName = "SizeName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string SizeName { get; set; }
        [XmlElement(ElementName = "TypeName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string TypeName { get; set; }
        [XmlElement(ElementName = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Contents { get; set; }
        [XmlElement(ElementName = "LocationId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string LocationId { get; set; }
        [XmlElement(ElementName = "LocationName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string LocationName { get; set; }
        [XmlElement(ElementName = "ReceivedDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string ReceivedDate { get; set; }
        [XmlElement(ElementName = "PalletId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PalletId { get; set; }
        [XmlElement(ElementName = "PalletName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PalletName { get; set; }
    }

    [XmlRoot(ElementName = "LocationId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class LocationId
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "LocationName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class LocationName
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "CreateDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class CreateDate
    {
        [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string OffsetMinutes { get; set; }
        [XmlAttribute(AttributeName = "d2p1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D2p1 { get; set; }
    }

    [XmlRoot(ElementName = "ScanDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ScanDate
    {
        [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string OffsetMinutes { get; set; }
        [XmlAttribute(AttributeName = "d2p1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D2p1 { get; set; }
    }

    [XmlRoot(ElementName = "Notes", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Notes
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "Tags", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Tags
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
        [XmlElement(ElementName = "Tag", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Tag Tag { get; set; }
    }

    [XmlRoot(ElementName = "ManifestItem", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ManifestItem
    {
        [XmlElement(ElementName = "ManifestItemId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string ManifestItemId { get; set; }
        [XmlElement(ElementName = "Keg", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Keg Keg { get; set; }
        [XmlElement(ElementName = "Pallet", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Pallet Pallet { get; set; }
        [XmlElement(ElementName = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Contents { get; set; }
        [XmlElement(ElementName = "ScanDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ScanDate ScanDate { get; set; }
        [XmlElement(ElementName = "Notes", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Notes Notes { get; set; }
        [XmlElement(ElementName = "Tags", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Tags Tags { get; set; }
    }

    [XmlRoot(ElementName = "ManifestItems", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ManifestItems
    {
        [XmlElement(ElementName = "ManifestItem", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public List<CreatedManifestItem> ManifestItem { get; set; }
    }

    [XmlRoot(ElementName = "Tag", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Tag
    {
        [XmlElement(ElementName = "Property", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Property { get; set; }
        [XmlElement(ElementName = "Value", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Manifest", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Manifest
    {
        [XmlElement(ElementName = "ManifestId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string ManifestId { get; set; }
        [XmlElement(ElementName = "ShipDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string ShipDate { get; set; }
        [XmlElement(ElementName = "SubmittedDate", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public SubmittedDate SubmittedDate { get; set; }
        [XmlElement(ElementName = "TrackingNumber", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string TrackingNumber { get; set; }
        [XmlElement(ElementName = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Contents { get; set; }
        [XmlElement(ElementName = "ManifestUser", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ManifestUser ManifestUser { get; set; }
        [XmlElement(ElementName = "SenderPartner", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public CreatorCompany SenderPartner { get; set; }
        [XmlElement(ElementName = "SenderShipAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Address SenderShipAddress { get; set; }
        [XmlElement(ElementName = "SenderBillAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public SenderBillAddress SenderBillAddress { get; set; }
        [XmlElement(ElementName = "SenderContactName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public SenderContactName SenderContactName { get; set; }
        [XmlElement(ElementName = "SenderContactEmail", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public SenderContactEmail SenderContactEmail { get; set; }
        [XmlElement(ElementName = "SenderContactPhone", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public SenderContactPhone SenderContactPhone { get; set; }
        [XmlElement(ElementName = "SenderReferenceKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public SenderReferenceKey SenderReferenceKey { get; set; }
        [XmlElement(ElementName = "SenderNotes", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public SenderNotes SenderNotes { get; set; }
        [XmlElement(ElementName = "ReceiverContactName", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ReceiverContactName ReceiverContactName { get; set; }
        [XmlElement(ElementName = "ReceiverContactEmail", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ReceiverContactEmail ReceiverContactEmail { get; set; }
        [XmlElement(ElementName = "ReceiverContactPhone", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ReceiverContactPhone ReceiverContactPhone { get; set; }
        [XmlElement(ElementName = "ReceiverReferenceKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ReceiverReferenceKey ReceiverReferenceKey { get; set; }
        [XmlElement(ElementName = "ReceiverNotes", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ReceiverNotes ReceiverNotes { get; set; }
        [XmlElement(ElementName = "ReceiverPartner", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public CreatorCompany ReceiverPartner { get; set; }
        [XmlElement(ElementName = "ReceiverShipAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Address ReceiverShipAddress { get; set; }
        [XmlElement(ElementName = "ReceiverBillAddress", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ReceiverBillAddress ReceiverBillAddress { get; set; }
        [XmlElement(ElementName = "ManifestItems", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ManifestItems ManifestItems { get; set; }
        [XmlElement(ElementName = "Tags", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Tags Tags { get; set; }
        [XmlAttribute(AttributeName = "i", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string I { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
