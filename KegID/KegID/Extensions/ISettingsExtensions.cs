using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Plugin.Settings.Abstractions;

namespace KegID.Extensions
{
    public static class ISettingsExtensions
    {
        public static T GetValueOrDefault<T>(this ISettings settings, string key) where T : class
        {
            string serialized = settings.GetValueOrDefault(key, string.Empty);
            T result = JsonConvert.DeserializeObject<T>(serialized);
            return result;
        }


        public static bool AddOrUpdateValue<T>(this ISettings settings, string key, T obj) where T : class
        {
            JsonSerializerSettings serializeSettings = GetSerializerSettings();
            string serialized = JsonConvert.SerializeObject(obj, serializeSettings);

            return settings.AddOrUpdateValue(key, serialized);
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
