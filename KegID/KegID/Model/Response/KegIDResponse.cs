using Realms;
using System.Net;

namespace KegID.Model
{
    public class KegIDResponse : RealmObject
    {
        public string StatusCode { get; set; }
        public string Response { get; set; }
    }
}
