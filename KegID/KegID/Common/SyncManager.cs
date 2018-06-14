using KegID.Messages;
using KegID.Model;
using Realms;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.Common
{
    public static class SyncManager
    {
        public static void NotifyConnectivityChanged()
        {
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private static void Connectivity_ConnectivityChanged(ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.Profiles;
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<Barcode>().ToList();

                if (value.Count > 0)
                {
                    var message = new StartLongRunningTaskMessage
                    {
                        Barcode = value.Select(p => p.Id).ToList(), //new List<string>() { ManaulBarcode },
                        Page = ViewTypeEnum.ScanKegsView
                    };
                    MessagingCenter.Send(message, "StartLongRunningTaskMessage"); 
                }
            }
        }
    }
}
