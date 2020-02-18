using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using KegID.ViewModel;
using Realms;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KegID.Services
{
    public class SyncManager : BaseViewModel, ISyncManager
    {
        public SyncManager() : base(null)
        {

        }

        public void NotifyConnectivityChanged()
        {
            // Register for connectivity changes, be sure to unsubscribe when finished
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<ManifestModel>().Where(x => x.IsQueue).ToList();

                if (value.Count > 0)
                {
                    foreach (var item in value)
                    {
                        switch ((EventTypeEnum)item.EventTypeId)
                        {
                            case EventTypeEnum.MOVE_MANIFEST:
                                var response = await ApiManager.PostManifest(item, Settings.SessionId);
                                if (response.IsSuccessStatusCode)
                                    AddorUpdateManifestOffline(item, false);
                                break;
                            case EventTypeEnum.SHIP_MANIFEST:
                                break;
                            case EventTypeEnum.RECEIVE_MANIFEST:
                                break;
                            case EventTypeEnum.FILL_MANIFEST:
                                response = await ApiManager.PostManifest(item, Settings.SessionId);
                                if (response.IsSuccessStatusCode)
                                    AddorUpdateManifestOffline(item, false);
                                break;
                            case EventTypeEnum.PALLETIZE_MANIFEST:

                                break;
                            case EventTypeEnum.RETURN_MANIFEST:
                                break;
                            case EventTypeEnum.REPAIR_MANIFEST:
                                response = await ApiManager.PostMaintenanceDone(item.MaintenanceModels.MaintenanceDoneRequestModel, Settings.SessionId);
                                if (response.IsSuccessStatusCode)
                                    AddorUpdateManifestOffline(item, false);
                                break;
                            case EventTypeEnum.COLLECT_MANIFEST:
                                break;
                            case EventTypeEnum.ARCHIVE_MANIFEST:
                                break;
                        }
                    }

                    CheckDraftmaniFests checkDraftmaniFests = new CheckDraftmaniFests
                    {
                        IsCheckDraft = true
                    };
                    MessagingCenter.Send(checkDraftmaniFests, "CheckDraftmaniFests");
                }

                var pallets = RealmDb.All<PalletRequestModel>().Where(x => x.IsQueue).ToList();

                if (pallets.Count > 0)
                {
                    foreach (var pallet in pallets)
                    {
                        var response = await ApiManager.PostPallet(pallet, Settings.SessionId);
                        AddorUpdatePalletsOffline(pallet);
                    }

                    CheckDraftmaniFests checkDraftmaniFests = new CheckDraftmaniFests
                    {
                        IsCheckDraft = true
                    };
                    MessagingCenter.Send(checkDraftmaniFests, "CheckDraftmaniFests");
                }
            }
        }

        private void AddorUpdateManifestOffline(ManifestModel manifestPostModel, bool queue)
        {
            string manifestId = manifestPostModel.ManifestId;
            var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<ManifestModel>(manifestId);
            if (isNew != null)
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() =>
                {
                    manifestPostModel.IsQueue = false;
                    manifestPostModel.IsDraft = false;
                    RealmDb.Add(manifestPostModel, update: true);
                });
            }
            else
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() =>
                {
                    if (queue)
                    {
                        manifestPostModel.IsQueue = true;
                    }
                    RealmDb.Add(manifestPostModel);
                });
            }
        }

        private void AddorUpdatePalletsOffline(PalletRequestModel palletRequestModel)
        {
            string palletId = palletRequestModel.PalletId;
            var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<PalletRequestModel>(palletId);
            if (isNew != null)
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() =>
                {
                    palletRequestModel.IsQueue = false;
                    RealmDb.Add(palletRequestModel, update: true);
                });
            }
        }
    }
}
