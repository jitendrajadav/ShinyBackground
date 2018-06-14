using Realms;
using System;
//using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PartnerTypeModel : RealmObject
    {
        [PrimaryKey]
        public string PartnerId { get; set; } = Guid.NewGuid().ToString();
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class PartnerTypeResponseModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<PartnerTypeModel> PartnerTypeModel { get; set; }
    }
}
