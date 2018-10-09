using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class LoginModel : RealmObject
    {
        public string MasterCompanyId { get; set; }
        public string SessionExpires { get; set; }
        public IList<Preference> Preferences { get; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string SessionId { get; set; }
    }

    public class Preference : RealmObject
    {
        public int Id { get; set; }
        public string PreferenceName { get; set; }
        public string PreferenceValue { get; set; }
    }

    public class LoginResponseModel 
    {
        public LoginModel LoginModel { get; set; }
        public KegIDResponse Response { get; set; }
    }
}

