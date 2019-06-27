using PropertyChanged;
using Realms;

namespace KegID.Model
{
    public class KegIDResponse : RealmObject
    {
        [DoNotNotify]
        public string StatusCode { get; set; }
        [DoNotNotify]
        public string Response { get; set; }
    }
}
