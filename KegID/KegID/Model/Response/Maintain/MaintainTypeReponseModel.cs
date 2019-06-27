using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;

namespace KegID.Model
{
    public class MaintainTypeReponseModel : RealmObject
    {
        [DoNotNotify]
        public long Id { get; set; }
        [DoNotNotify]
        public string Name { get; set; }
        [DoNotNotify]
        public string Description { get; set; }
        [DoNotNotify]
        public bool IsAlert { get; set; }
        [DoNotNotify]
        public bool IsAction { get; set; }
        [DoNotNotify]
        public string DefectType { get; set; }
        [DoNotNotify]
        public string ActivationMethod { get; set; }
        [DoNotNotify]
        public DateTimeOffset DeletedDate { get; set; }
        [DoNotNotify]
        public bool InUse { get; set; }
        
        public bool IsToggled { get; set; }
        [DoNotNotify]
        public List<string> ActivationPartnerTypes { get; }
    }

    public class MaintainTypeModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<MaintainTypeReponseModel> MaintainTypeReponseModel { get; set; }
    }
}
