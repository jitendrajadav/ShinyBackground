using System;
using System.Collections.Generic;
using Realms;

namespace KegID.Model
{
    public class ManifestItem : RealmObject
    {
        public string Barcode { get; set; }
        public DateTimeOffset ScanDate { get; set; }
        public long ValidationStatus { get; set; }
        public string KegId { get; set; }
        public IList<Tag> Tags { get; }
        //public List<KegStatus> KegStatus { get; set; }
    }

    public class KegStatus
    {
        public string KegId { get; set; }
        public string Barcode { get; set; }
        public string AltBarcode { get; set; }
        public string Contents { get; set; }
        public string Batch { get; set; }
        public string Size { get; set; }
        public string Alert { get; set; }
        public Location Location { get; set; }
        public string OwnerName { get; set; }
    }


    public class ManifestModelGet 
    {
        public KegIDResponse Response { get; set; }
        public string ManifestId { get; set; }
        public string TrackingNumber { get; set; }
        public DateTimeOffset ShipDate { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public long ItemCount { get; set; }
        public string Gs1Id { get; set; }
        public string Type { get; set; }
    }
}
