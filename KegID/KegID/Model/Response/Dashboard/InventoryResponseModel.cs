using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace KegID.Model
{
    public class InventoryResponseModel 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
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
