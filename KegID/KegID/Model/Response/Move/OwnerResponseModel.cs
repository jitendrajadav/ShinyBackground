using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace KegID.Model
{
    public class OwnerModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PartnerId { get; set; }
        public string FullName { get; set; }
    }

    public class OwnerResponseModel : KegIDResponse
    {
        public IList<OwnerModel> OwnerModel { get; set; }
    }
}
