﻿using Realms;

namespace KegID.LocalDb
{
    public static class RealmDbManager
    {
        public static RealmConfiguration GetRealmDbConfig()
        {
            var config = new RealmConfiguration
            {
                SchemaVersion = 2,
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
