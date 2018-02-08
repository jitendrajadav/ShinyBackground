using System.Collections.Generic;

namespace KegID.Model
{
    public class InventoryResponseModel 
    {
        public string OwnerName { get; set; }
        public string Status { get; set; }
        public string Contents { get; set; }
        public string Size { get; set; }
        public long Quantity { get; set; }
    }

    public class InventoryDetailModel : KegIDResponse
    {
        public IList<InventoryResponseModel> InventoryResponseModel { get; set; }
    }
}
