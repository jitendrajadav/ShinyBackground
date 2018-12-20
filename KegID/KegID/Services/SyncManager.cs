using KegID.Common;
using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Realms;
using System;
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
                var value = RealmDb.All<ManifestModel>().Where(x => x.IsQueue == true).ToList();

                if (value.Count > 0)
                {
                    IMoveService _moveService = new MoveService();

                    try
                    {
                        foreach (var item in value)
                        {
                            switch ((EventTypeEnum)item.EventTypeId)
                            {
                                case EventTypeEnum.MOVE_MANIFEST:
                                    var resultMoveManifest = await _moveService.PostManifestAsync(item, AppSettings.SessionId, Configuration.NewManifest);
                                    if (resultMoveManifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                                        AddorUpdateManifestOffline(item, false);
                                    break;
                                case EventTypeEnum.SHIP_MANIFEST:
                                    break;
                                case EventTypeEnum.RECEIVE_MANIFEST:
                                    break;
                                case EventTypeEnum.FILL_MANIFEST:
                                    var resultFillManifest = await _moveService.PostManifestAsync(item, AppSettings.SessionId, Configuration.NewManifest);
                                    if (resultFillManifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                                        AddorUpdateManifestOffline(item, false);
                                    break;
                                case EventTypeEnum.PALLETIZE_MANIFEST:

                                    break;
                                case EventTypeEnum.RETURN_MANIFEST:
                                    break;
                                case EventTypeEnum.REPAIR_MANIFEST:
                                    IMaintainService _maintainService = new MaintainService();
                                    KegIDResponse resultRepairManifest = await _maintainService.PostMaintenanceDoneAsync(item.MaintenanceModels.MaintenanceDoneRequestModel, AppSettings.SessionId, Configuration.PostedMaintenanceDone);
                                    if (resultRepairManifest.StatusCode == System.Net.HttpStatusCode.NoContent.ToString())
                                        AddorUpdateManifestOffline(item, false);
                                    break;
                                case EventTypeEnum.COLLECT_MANIFEST:
                                    break;
                                case EventTypeEnum.ARCHIVE_MANIFEST:
                                    break;
                                default:
                                    break;
                            }
                        }

                        CheckDraftmaniFests checkDraftmaniFests = new CheckDraftmaniFests
                        {
                            IsCheckDraft = true
                        };
                        MessagingCenter.Send(checkDraftmaniFests, "CheckDraftmaniFests");
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }

                var pallets = RealmDb.All<PalletRequestModel>().Where(x => x.IsQueue == true).ToList();

                if (pallets.Count > 0)
                {
                    IPalletizeService _palletizeService = new PalletizeService();

                    foreach (var pallet in pallets)
                    {
                        var result = await _palletizeService.PostPalletAsync(pallet, AppSettings.SessionId, Configuration.NewPallet);
                        AddorUpdatePalletsOffline(pallet,false);
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
                try
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        manifestPostModel.IsQueue = false;
                        manifestPostModel.IsDraft = false;
                        RealmDb.Add(manifestPostModel, update: true);
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                try
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
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private void AddorUpdatePalletsOffline(PalletRequestModel palletRequestModel, bool queue)
        {
            string palletId = palletRequestModel.PalletId;
            var isNew = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).Find<PalletRequestModel>(palletId);
            if (isNew != null)
            {
                try
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        palletRequestModel.IsQueue = false;
                        RealmDb.Add(palletRequestModel, update: true);
                    });
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

    }
}
