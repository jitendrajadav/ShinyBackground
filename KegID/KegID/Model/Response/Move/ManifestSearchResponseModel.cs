using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class ManifestSearchResponseModel
    {
        public string ManifestId { get; set; }
        public string TrackingNumber { get; set; }
        public DateTimeOffset ShipDate { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public long ItemCount { get; set; }
        public string Gs1Id { get; set; }
        public string PurpleType { get; set; }
    }

    public class ManifestSearchModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<ManifestSearchResponseModel> ManifestSearchResponseModel { get; set; }
    }
}
