﻿using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace KegID.Response
{
    public class DraftManifestModel
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

    public class ManifestModel
    {
        [PrimaryKey]
        public string ManifestId { get; set; }
        public long EventTypeId { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime ShipDate { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string DestinationName { get; set; }
        public string DestinationTypeCode { get; set; }
        [Ignore]
        public List<ManifestItem> ManifestItems { get; set; }
        [Ignore]
        public List<string> NewPallets { get; set; }
        [Ignore]
        public List<Tag> Tags { get; set; }
    }

    public class ManifestItem
    {
        public string Barcode { get; set; }
        public DateTime ScanDate { get; set; }
        public long ValidationStatus { get; set; }
        public string KegId { get; set; }
        public List<Tag> Tags { get; set; }
        public List<KegStatus> KegStatus { get; set; }
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

    public class Tag
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }

    public class ManifestModelGet
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
