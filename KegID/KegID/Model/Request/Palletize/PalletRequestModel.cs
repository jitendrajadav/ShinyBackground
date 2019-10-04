using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PalletRequestModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string PalletId { get; set; }
        [DoNotNotify]
        public string OwnerId { get; set; }
        [DoNotNotify]
        public string Barcode { get; set; }
        [DoNotNotify]
        public DateTimeOffset BuildDate { get; set; }
        [DoNotNotify]
        public string StockLocation { get; set; }
        [DoNotNotify]
        public string StockLocationId { get; set; }
        [DoNotNotify]
        public string StockLocationName { get; set; }
        [DoNotNotify]
        public string ReferenceKey { get; set; }
        [DoNotNotify]
        public IList<PalletItem> PalletItems { get; }
        [DoNotNotify]
        public IList<Tag> Tags { get;}
        [DoNotNotify]
        public bool IsQueue { get; internal set; }
    }
}
