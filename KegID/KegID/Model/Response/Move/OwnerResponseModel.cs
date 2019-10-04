using PropertyChanged;
using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class OwnerModel : RealmObject
    {
        [DoNotNotify]
        public string PartnerId { get; set; }
        [DoNotNotify]
        public string FullName { get; set; }
        [DoNotNotify]
        public bool HasInitial { get; set; }
    }

    public class OwnerResponseModel 
    {
        [DoNotNotify]
        public KegIDResponse Response { get; set; }
        [DoNotNotify]
        public IList<OwnerModel> OwnerModel { get; set; }
    }
}
