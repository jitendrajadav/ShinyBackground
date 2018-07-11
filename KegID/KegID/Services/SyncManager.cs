using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using Realms;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.Services
{
    public class SyncManager : ISyncManager
    {
        public void NotifyConnectivityChanged()
        {
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.Profiles;
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<ManifestModel>().Where(x=>x.IsQueue == true).ToList();

                if (value.Count > 0)
                {
                    foreach (var item in value)
                    {
                        var message = new StartLongRunningTaskMessage
                        {

                            Barcode = item.BarcodeModels.Select(x=>x.Barcode).ToList(),
                            PageName = ViewTypeEnum.ScanKegsView.ToString()
                        };
                        MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                    }
                }
            }
        }
    }
}
