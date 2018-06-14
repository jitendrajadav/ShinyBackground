//using SQLite.Net.Attributes;
using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class OwnerModel : RealmObject
    {
        public string PartnerId { get; set; }
        public string FullName { get; set; }
        public bool HasInitial { get; set; }
    }

    public class OwnerResponseModel 
    {
        public KegIDResponse Response { get; set; }
        public IList<OwnerModel> OwnerModel { get; set; }
    }
}
