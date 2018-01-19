using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;

namespace KegID.Response
{
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var data = UserResponse.FromJson(jsonString);


    //public partial class LoginModel
    //{
    //    [JsonProperty("CompanyNo")]
    //    public object CompanyNo { get; set; }

    //    [JsonProperty("CompanyKey")]
    //    public string CompanyKey { get; set; }

    //    [JsonProperty("MasterCompanyId")]
    //    public object MasterCompanyId { get; set; }

    //    [JsonProperty("MasterCompanyName")]
    //    public object MasterCompanyName { get; set; }

    //    [JsonProperty("Roles")]
    //    public string[] Roles { get; set; }

    //    [JsonProperty("isEnabled")]
    //    public bool IsEnabled { get; set; }

    //    [JsonProperty("FirstName")]
    //    public string FirstName { get; set; }

    //    [JsonProperty("LastName")]
    //    public string LastName { get; set; }

    //    [JsonProperty("Phone")]
    //    public string Phone { get; set; }

    //    [JsonProperty("TimeZone")]
    //    public string TimeZone { get; set; }

    //    [JsonProperty("PalletBarcodePrefix")]
    //    public string PalletBarcodePrefix { get; set; }

    //    [JsonProperty("isNotify")]
    //    public bool IsNotify { get; set; }

    //    [JsonProperty("SessionExpires")]
    //    public DateTime SessionExpires { get; set; }

    //    [JsonProperty("Preferences")]
    //    public Preference[] Preferences { get; set; }

    //    [JsonProperty("DataInfo")]
    //    public object DataInfo { get; set; }

    //    [JsonProperty("UserHome")]
    //    public object UserHome { get; set; }

    //    [JsonProperty("UserHomeName")]
    //    public object UserHomeName { get; set; }

    //    [JsonProperty("Operators")]
    //    public string[] Operators { get; set; }

    //    [JsonProperty("UserId")]
    //    public string UserId { get; set; }

    //    [JsonProperty("FullName")]
    //    public string FullName { get; set; }

    //    [JsonProperty("Email")]
    //    public string Email { get; set; }

    //    [JsonProperty("CompanyId")]
    //    public string CompanyId { get; set; }

    //    [JsonProperty("CompanyName")]
    //    public string CompanyName { get; set; }

    //    [JsonProperty("ReferenceKey")]
    //    public string ReferenceKey { get; set; }

    //    [JsonProperty("IsOperator")]
    //    public bool IsOperator { get; set; }

    //    [JsonProperty("SessionId")]
    //    public string SessionId { get; set; }
    //}

    //public partial class Preference
    //{
    //    [JsonProperty("PreferenceName")]
    //    public string PreferenceName { get; set; }

    //    [JsonProperty("PreferenceValue")]
    //    public string PreferenceValue { get; set; }
    //}

    //public partial class LoginModel
    //{
    //    public static LoginModel FromJson(string json) => JsonConvert.DeserializeObject<LoginModel>(json, Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this LoginModel self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //}

    //public class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //    };
    //}

    public class LoginModel
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string CompanyNo { get; set; }
        public string CompanyKey { get; set; }
        public string MasterCompanyId { get; set; }
        public string MasterCompanyName { get; set; }
        [Ignore]
        public List<string> Roles { get; set; }
        public bool IsEnabled { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string TimeZone { get; set; }
        public string PalletBarcodePrefix { get; set; }
        public bool IsNotify { get; set; }
        public DateTime SessionExpires { get; set; }
        [Ignore]
        public List<Preference> Preferences { get; set; }
        public string DataInfo { get; set; }
        public string UserHome { get; set; }
        public string UserHomeName { get; set; }
        [Ignore]
        public List<string> Operators { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ReferenceKey { get; set; }
        public bool IsOperator { get; set; }
        public string SessionId { get; set; }
    }

    public class Preference
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PreferenceName { get; set; }
        public string PreferenceValue { get; set; }
    }
}

