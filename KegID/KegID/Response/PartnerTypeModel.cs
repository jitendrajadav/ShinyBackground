using SQLite.Net.Attributes;

namespace KegID.Model
{
    public class PartnerTypeModel
    {
        [PrimaryKey,AutoIncrement]
        public int PartnerId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
