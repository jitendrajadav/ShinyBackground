using PropertyChanged;
using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class BrandModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string BrandId { get; set; }
        [DoNotNotify]
        public string BrandName { get; set; }
        [DoNotNotify]
        public string StyleName { get; set; }
        [DoNotNotify]
        public string Description { get; set; }
        [DoNotNotify]
        public string BrandCode { get; set; }
        [DoNotNotify]
        public string SourceKey { get; set; }
        [DoNotNotify]
        public long? FreshDays { get; set; }
    }

    public class BrandResponseModel
    {
        public IList<BrandModel> BrandModel { get; set; }
        public KegIDResponse Response { get; set; }
    }
}
