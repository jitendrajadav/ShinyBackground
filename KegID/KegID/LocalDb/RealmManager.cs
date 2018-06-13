using Microsoft.AppCenter.Crashes;
using Realms;
using System;
using System.Threading.Tasks;

namespace KegID.LocalDb
{
    public static class RealmManager
    {
        public static async Task<Realm> RealmDbAsync()
        {
            var config = new RealmConfiguration
            {
                SchemaVersion = 1,
                //MigrationCallback = (migration, oldSchemaVersion) =>
                //{
                //    // potentially lengthy data migration
                //}
            };

            try
            {
                var realm = await Realm.GetInstanceAsync(config);
                // Realm successfully opened, with migration applied on background thread

                return realm;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                // Handle exception that occurred while opening the Realm
                return null;
            }
        }
    }
}
