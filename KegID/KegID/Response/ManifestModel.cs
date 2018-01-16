using System;
using System.Collections.Generic;
using System.Text;

namespace KegID.Response
{
    public class ManifestModel
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
