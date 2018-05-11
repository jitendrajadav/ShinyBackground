using System.Collections.Generic;

namespace KegID.Dtos
{
    public class LoginDto
    {
        public string MasterCompanyId { get; set; }
        public string SessionExpires { get; set; }
        public List<PreferenceDto> Preferences { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string SessionId { get; set; }
    }
}
