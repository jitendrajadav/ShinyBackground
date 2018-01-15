using KegID.Model;
using KegID.Response;
using SQLite.Net.Async;
using System;
using System.Diagnostics;
using System.Threading;
using Xamarin.Forms;

namespace KegID.SQLiteClient
{
    public class SQLiteServiceClient : ISQLiteServiceClient
    {
        private static readonly Lazy<SQLiteServiceClient> Lazy = new Lazy<SQLiteServiceClient>(() => new SQLiteServiceClient());

        public static ISQLiteServiceClient Instance => Lazy.Value;

        private SQLiteServiceClient()
        {
        }

        private static SQLiteAsyncConnection _dbConnection;
        public static SQLiteAsyncConnection Db
        {
            get
            {
                if (_dbConnection == null)
                {
                    try
                    {
                        LazyInitializer.EnsureInitialized(ref _dbConnection, DependencyService.Get<ISQLite>().GetAsyncConnection);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                return _dbConnection;
            }
        }

        public async void CreateDbIfNotExist()
        {
            try
            {
                await Db.CreateTablesAsync<GlobalModel, PartnerModel, BrandModel, PartnerTable>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
