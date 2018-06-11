//using KegID.Droid.DependencyServices;
//using KegID.SQLiteClient;
//using SQLite.Net;
//using SQLite.Net.Async;
//using SQLite.Net.Platform.XamarinAndroid;
//using System;
//using System.IO;

//[assembly: Xamarin.Forms.Dependency(typeof(SQLiteAndroid))]
//namespace KegID.Droid.DependencyServices
//{
//    public class SQLiteAndroid : ISQLite
//    {
//        private SQLiteConnectionWithLock _conn;

//        public SQLiteAndroid()
//        {

//        }

//        private static string GetDatabasePath()
//        {
//            const string sqliteFilename = "KegID.db3";

//            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
//            var path = Path.Combine(documentsPath, sqliteFilename);

//            return path;
//        }

//        public SQLiteConnection GetConnection()
//        {
//            var dbPath = GetDatabasePath();

//            // Return the synchronous database connection 
//            return new SQLiteConnection(new SQLitePlatformAndroid(), dbPath);
//        }

//        public SQLiteAsyncConnection GetAsyncConnection()
//        {
//            var dbPath = GetDatabasePath();

//            var platForm = new SQLitePlatformAndroid();

//            var connectionFactory = new Func<SQLiteConnectionWithLock>(
//                () =>
//                {
//                    if (_conn == null)
//                    {
//                        _conn =
//                            new SQLiteConnectionWithLock(platForm,
//                                new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: true));
//                    }
//                    return _conn;
//                });

//            return new SQLiteAsyncConnection(connectionFactory);
//        }

//        public void DeleteDatabase()
//        {
//            var path = GetDatabasePath();

//            try
//            {
//                if (_conn != null)
//                {
//                    _conn.Close();
//                }
//            }
//            catch
//            {
//                // Best effort close. No need to worry if throws an exception
//            }

//            if (File.Exists(path))
//            {
//                File.Delete(path);
//            }
//            _conn = null;
//        }

//        public void CloseConnection()
//        {
//            if (_conn != null)
//            {
//                _conn.Close();
//                _conn.Dispose();
//                _conn = null;

//                // Must be called as the disposal of the connection is not released until the GC runs.
//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//            }
//        }
//    }
//}