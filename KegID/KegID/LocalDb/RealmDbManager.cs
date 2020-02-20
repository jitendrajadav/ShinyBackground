using Realms;

namespace KegID.LocalDb
{
    public static class RealmDbManager
    {
        public static RealmConfiguration GetRealmDbConfig()
        {
            var config = new RealmConfiguration
            {
                SchemaVersion = 3,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    // potentially lengthy data migration
                }
            };

            return config;
        }
    }
}
