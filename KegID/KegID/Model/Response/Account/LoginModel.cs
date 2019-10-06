using PropertyChanged;
using Realms;
using System.Collections.Generic;

namespace KegID.Model
{
    public class LoginModel : RealmObject
    {
        [DoNotNotify]
        public string MasterCompanyId { get; set; }
        [DoNotNotify]
        public string SessionExpires { get; set; }
        [DoNotNotify]
        public IList<Preference> Preferences { get; }
        [DoNotNotify]
        public string UserId { get; set; }
        [DoNotNotify]
        public string CompanyId { get; set; }
        [DoNotNotify]
        public string SessionId { get; set; }
    }

    public class Preference : RealmObject
    {
        [DoNotNotify]
        public int Id { get; set; }
        [DoNotNotify]
        public string PreferenceName { get; set; }
        [DoNotNotify]
        public string PreferenceValue { get; set; }
    }

    public class LoginResponseModel 
    {
        public LoginModel LoginModel { get; set; }
        public KegIDResponse Response { get; set; }
    }
}

