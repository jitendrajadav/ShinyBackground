using SQLite;

namespace KegID.Response
{
    public class BrandModel
    {
        [PrimaryKey, AutoIncrement]
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string StyleName { get; set; }
        public string Description { get; set; }
        public string BrandCode { get; set; }
        public string SourceKey { get; set; }
        public long? FreshDays { get; set; }
    }
}
