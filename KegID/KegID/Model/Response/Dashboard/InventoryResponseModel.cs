using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class InventoryResponseModel : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string OwnerName { get; set; }
        public string Status { get; set; }
        public string Contents { get; set; }
        public string Size { get; set; }
        public long Quantity { get; set; }
    }

    public class InventoryDetailModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<InventoryResponseModel> InventoryResponseModel { get; set; }
    }
}
