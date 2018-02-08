using System.Net;

namespace KegID.Model
{
    public class KegIDResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }

    }
}
