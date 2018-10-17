using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Realms;
using System;
using System.Linq;
using Xamarin.Essentials;

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
            var profiles = e.Profiles;
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<ManifestModel>().Where(x => x.IsQueue == true).ToList();

                if (value.Count > 0)
                {
                    #region Old Code
                    //foreach (var item in value)
                    //{
                    //    var message = new StartLongRunningTaskMessage
                    //    {

                    //        Barcode = item.BarcodeModels.Select(x=>x.Barcode).ToList(),
                    //        PageName = ViewTypeEnum.ScanKegsView.ToString()
                    //    };
                    //    MessagingCenter.Send(message, "StartLongRunningTaskMessage");
                    //} 
                    #endregion

                    IMoveService _moveService = new MoveService();

                    try
                    {
                        foreach (var item in value)
                        {
                            switch ((EventTypeEnum)item.EventTypeId)
                            {
                                case EventTypeEnum.MOVE_MANIFEST:
                                    var result = await _moveService.PostManifestAsync(item, AppSettings.SessionId, Configuration.NewManifest);
                                    AddorUpdateManifestOffline(item,false);
                                    break;
                                case EventTypeEnum.SHIP_MANIFEST:
                                    break;
                                case EventTypeEnum.RECEIVE_MANIFEST:
                                    break;
                                case EventTypeEnum.FILL_MANIFEST:
                                    var manifestPostModel = await _moveService.PostManifestAsync(item, AppSettings.SessionId, Configuration.NewManifest);
                                    AddorUpdateManifestOffline(item, false);
                                    break;
                                case EventTypeEnum.PALLETIZE_MANIFEST:
                                    break;
                                case EventTypeEnum.RETURN_MANIFEST:
                                    break;
                                case EventTypeEnum.REPAIR_MANIFEST:
                                    IMaintainService _maintainService = new MaintainService();
                                    KegIDResponse kegIDResponse = await _maintainService.PostMaintenanceDoneAsync(item.MaintenanceModels.MaintenanceDoneRequestModel, AppSettings.SessionId, Configuration.PostedMaintenanceDone);
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
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
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
                    manifestPostModel.IsQueue = false;
                    manifestPostModel.IsDraft = false;
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
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
                    if (queue)
                    {
                        manifestPostModel.IsQueue = true;
                    }
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    RealmDb.Write(() =>
                    {
                        RealmDb.Add(manifestPostModel);
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
