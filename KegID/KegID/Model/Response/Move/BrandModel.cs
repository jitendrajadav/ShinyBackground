//using SQLite.Net.Attributes;
using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class BrandModel : RealmObject
    {
        [PrimaryKey]
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string StyleName { get; set; }
        public string Description { get; set; }
        public string BrandCode { get; set; }
        public string SourceKey { get; set; }
        public long? FreshDays { get; set; }
    }

    public class BrandResponseModel
    {
        public IList<BrandModel> BrandModel { get; set; }
        public KegIDResponse Response { get; set; }
    }
}
