using System;
using SQLite.Net.Attributes;

namespace KegID.Response
{
    public class ManifestModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ManifestId { get; set; }
        public DateTime ShipDate { get; set; }
        public long ManifestItems { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string OriginId { get; set; }
        public string SenderId { get; set; }
        public string DestinationId { get; set; }
        public string ReceiverId { get; set; }
        public string Tags { get; set; }
        public string GS1GSIN { get; set; }
        public bool IsSendManifest { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int EventTypeId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long NewPallets { get; set; }
        public string NewBatch { get; set; }
        public long NewBatches { get; set; }
        public string KegOrderId { get; set; }
        public DateTime PostedDate { get; set; }
        public string SourceKey { get; set; }
        public long ClosedBatches { get; set; }
    }

    public class ManifestModelOldGet
    {
        public string ManifestId { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime ShipDate { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public long ItemCount { get; set; }
        public string Gs1Id { get; set; }
        public string Type { get; set; }
    }
}
