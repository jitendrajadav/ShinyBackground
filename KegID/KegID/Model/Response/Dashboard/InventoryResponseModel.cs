using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class InventoryResponseModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DoNotNotify]
        public string OwnerName { get; set; }
        [DoNotNotify]
        public string Status { get; set; }
        [DoNotNotify]
        public string Contents { get; set; }
        [DoNotNotify]
        public string Size { get; set; }
        [DoNotNotify]
        public long Quantity { get; set; }
    }

    public class InventoryDetailModel 
    {
        [DoNotNotify]
        public KegIDResponse Response { get; set; }
        [DoNotNotify]
        public IList<InventoryResponseModel> InventoryResponseModel { get; set; }
    }
}
