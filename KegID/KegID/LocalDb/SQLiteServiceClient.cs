//using KegID.Model;
//using Microsoft.AppCenter.Crashes;
//using SQLite.Net.Async;
//using System;
//using System.Threading;
//using Xamarin.Forms;

//namespace KegID.SQLiteClient
//{
//    public class SQLiteServiceClient : ISQLiteServiceClient
//    {
//        private static readonly Lazy<SQLiteServiceClient> Lazy = new Lazy<SQLiteServiceClient>(() => new SQLiteServiceClient());

//        public static ISQLiteServiceClient Instance => Lazy.Value;

//        private SQLiteServiceClient()
//        {
//        }

//        private static SQLiteAsyncConnection _dbConnection;
//        public static SQLiteAsyncConnection Db
//        {
//            get
//            {
//                if (_dbConnection == null)
//                {
//                    try
//                    {
//                        LazyInitializer.EnsureInitialized(ref _dbConnection, DependencyService.Get<ISQLite>().GetAsyncConnection);
//                    }
//                    catch (Exception ex)
//                    {
//                         Crashes.TrackError(ex);
//                    }
//                }

//                return _dbConnection;
//            }
//        }

//        public async void CreateDbIfNotExist()
//        {
//            try
//            {
//                await Db.CreateTablesAsync<DraftManifestModel, BarcodeModel, PartnerModel, Preference, AssetSizeModel>();
//                await Db.CreateTablesAsync<BrandModel, PartnerTypeModel, BatchModel, MaintainTypeReponseModel, AssetTypeModel>();
//                await Db.CreateTablesAsync<AssetVolumeModel, OwnerModel, InventoryResponseModel>();
//            }
//            catch (Exception ex)
//            {
//                 Crashes.TrackError(ex);
//            }
//        }
//    }
//}
