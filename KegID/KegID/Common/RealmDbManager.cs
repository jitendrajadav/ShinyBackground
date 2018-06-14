using Realms;

namespace KegID.Common
{
    public static class RealmDbManager
    {
        public static RealmConfiguration GetRealmDbConfig()
        {
            var config = new RealmConfiguration
            {
                SchemaVersion = 1,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    // potentially lengthy data migration
                }
            };

            //try
            //{
            //    var realm = await Realm.GetInstanceAsync(config);
            //    // Realm successfully opened, with migration applied on background thread
            //}
            //catch (Exception ex)
            //{
            //    // Handle exception that occurred while opening the Realm
            //}

            return config;
        }
    }
}
