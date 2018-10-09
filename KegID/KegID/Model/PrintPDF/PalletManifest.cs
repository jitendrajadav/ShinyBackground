using System.Xml.Serialization;
using System.Collections.Generic;

namespace KegID.Model.PrintPDF
{
    [XmlRoot(ElementName = "Container", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Container
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "CreatedDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class CreatedDate
    {
        [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string OffsetMinutes { get; set; }
        [XmlAttribute(AttributeName = "d2p1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D2p1 { get; set; }
    }

    [XmlRoot(ElementName = "BuildDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class BuildDate
    {
        [XmlElement(ElementName = "DateTime", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string DateTime { get; set; }
        [XmlElement(ElementName = "OffsetMinutes", Namespace = "http://schemas.datacontract.org/2004/07/System")]
        public string OffsetMinutes { get; set; }
        [XmlAttribute(AttributeName = "d2p1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string D2p1 { get; set; }
    }

    [XmlRoot(ElementName = "DateInfo", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class DateInfo
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Contents
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "ContentsKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ContentsKey
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "RemovedManifest", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class RemovedManifest
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "DateRemoved", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class DateRemoved
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "PalletItem", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class PalletItem
    {
        [XmlElement(ElementName = "PalletId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PalletId { get; set; }
        [XmlElement(ElementName = "Barcode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Barcode { get; set; }
        [XmlElement(ElementName = "DateScanned", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string DateScanned { get; set; }
        [XmlElement(ElementName = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Contents { get; set; }
        [XmlElement(ElementName = "ContentsKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ContentsKey ContentsKey { get; set; }
        [XmlElement(ElementName = "Keg", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Keg Keg { get; set; }
        [XmlElement(ElementName = "isActive", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string IsActive { get; set; }
        [XmlElement(ElementName = "RemovedManifest", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public RemovedManifest RemovedManifest { get; set; }
        [XmlElement(ElementName = "DateRemoved", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public DateRemoved DateRemoved { get; set; }
        [XmlElement(ElementName = "Tags", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Tag Tags { get; set; }
    }

    [XmlRoot(ElementName = "PalletItems", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class PalletItems
    {
        [XmlElement(ElementName = "PalletItem", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public List<PalletItem> PalletItem { get; set; }
    }

    [XmlRoot(ElementName = "ReferenceKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class ReferenceKey
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "StockLocation", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class StockLocation
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

    [XmlRoot(ElementName = "TargetLocation", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class TargetLocation
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "SummaryItem", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SummaryItem
    {
        [XmlElement(ElementName = "Combination", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Combination { get; set; }
        [XmlElement(ElementName = "Contents", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Contents { get; set; }
        [XmlElement(ElementName = "Quantity", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "Size", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Size { get; set; }
    }

    [XmlRoot(ElementName = "SummaryItems", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class SummaryItems
    {
        [XmlElement(ElementName = "SummaryItem", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public List<SummaryItem> SummaryItem { get; set; }
    }

    [XmlRoot(ElementName = "Pallet", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
    public class Pallet
    {
        [XmlElement(ElementName = "Barcode", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string Barcode { get; set; }
        [XmlElement(ElementName = "Container", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Container Container { get; set; }
        [XmlElement(ElementName = "CreatedDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string CreatedDate { get; set; }
        [XmlElement(ElementName = "BuildDate", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string BuildDate { get; set; }
        [XmlElement(ElementName = "DateInfo", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public DateInfo DateInfo { get; set; }
        [XmlElement(ElementName = "Location", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Owner Location { get; set; }
        [XmlElement(ElementName = "Owner", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Owner Owner { get; set; }
        [XmlElement(ElementName = "PalletId", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public string PalletId { get; set; }
        [XmlElement(ElementName = "PalletItems", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public PalletItems PalletItems { get; set; }
        [XmlElement(ElementName = "ReferenceKey", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public ReferenceKey ReferenceKey { get; set; }
        [XmlElement(ElementName = "StockLocation", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public Owner StockLocation { get; set; }
        [XmlElement(ElementName = "TargetLocation", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public TargetLocation TargetLocation { get; set; }
        [XmlElement(ElementName = "SummaryItems", Namespace = "http://schemas.datacontract.org/2004/07/com.anotherroundapps.apps.kegid.objects")]
        public SummaryItems SummaryItems { get; set; }
        [XmlAttribute(AttributeName = "i", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string I { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
