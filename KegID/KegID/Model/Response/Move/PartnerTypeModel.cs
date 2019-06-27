using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class PartnerTypeModel : RealmObject
    {
        [PrimaryKey]
        [DoNotNotify]
        public string PartnerId { get; set; } = Guid.NewGuid().ToString();
        [DoNotNotify]
        public string Id { get; set; }
        [DoNotNotify]
        public string Name { get; set; }
        [DoNotNotify]
        public string Code { get; set; }
    }

    public class PartnerTypeResponseModel 
    {
        [DoNotNotify]
        public KegIDResponse Response { get; set; }
        [DoNotNotify]
        public IList<PartnerTypeModel> PartnerTypeModel { get; set; }
    }
}
