using SQLite.Net.Attributes;
using System.Collections.Generic;

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

    public class PartnerTypeResponseModel : KegIDResponse
    {
        public IList<PartnerTypeModel> PartnerTypeModel { get; set; }
    }
}
