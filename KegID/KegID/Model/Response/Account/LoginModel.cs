using Realms;
//using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace KegID.Model
{
    public class LoginModel : RealmObject
    {
        //public int Id { get; set; }
        //public string CompanyNo { get; set; }
        //public string CompanyKey { get; set; }
        public string MasterCompanyId { get; set; }
        //public string MasterCompanyName { get; set; }
        //public List<string> Roles { get; set; }
        //public bool IsEnabled { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Phone { get; set; }
        //public string TimeZone { get; set; }
        //public string PalletBarcodePrefix { get; set; }
        //public bool IsNotify { get; set; }
        public string SessionExpires { get; set; }
        public IList<Preference> Preferences { get; }
        //public string DataInfo { get; set; }
        //public string UserHome { get; set; }
        //public string UserHomeName { get; set; }
        //public List<string> Operators { get; set; }
        public string UserId { get; set; }
        //public string FullName { get; set; }
        //public string Email { get; set; }
        public string CompanyId { get; set; }
        //public string CompanyName { get; set; }
        //public string ReferenceKey { get; set; }
        //public bool IsOperator { get; set; }
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

