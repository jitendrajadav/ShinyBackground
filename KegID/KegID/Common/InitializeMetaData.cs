using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KegID.Common
{
    public static class InitializeMetaData
    {
        public static async Task LoadInitializeMetaData(IMoveService _moveService, IDashboardService _dashboardService,IMaintainService _maintainService, IFillService _fillService)
        {
            await LoadAssetSizeAsync(_moveService);
            await LoadAssetTypeAsync(_moveService);
            await LoadAssetVolumeAsync(_dashboardService);
            await LoadOwnerAsync(_moveService);
            await LoadDashboardPartnersAsync(_dashboardService);
            await LoadPartnersAsync(_moveService);
            await LoadBrandAsync(_moveService);
            await LoadMaintenanceTypeAsync(_maintainService);
            await LoadBatchAsync(_fillService);
        }

        public static async Task LoadBatchAsync(IFillService _fillService)
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                var value = await _fillService.GetBatchListAsync(AppSettings.User.SessionId);
                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    var batches = value.BatchModel.Where(p => p.BrandName != string.Empty).OrderBy(x => x.BrandName).ToList();
                    RealmDb.Write(() =>
                    {
                        foreach (var item in batches)
                        {
                            RealmDb.Add(item);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
            }
        }

        public static async Task<IList<MaintainTypeReponseModel>> LoadMaintenanceTypeAsync(IMaintainService _maintainService)
        {
            var model = await _maintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() =>
                {
                    foreach (var item in model.MaintainTypeReponseModel)
                    {
                        RealmDb.Add(item);
                    }
                });
              return  model.MaintainTypeReponseModel;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
        }

        private static async Task LoadPartnersAsync(IMoveService _moveService)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var value = await _moveService.GetPartnersListAsync(AppSettings.User.SessionId);
                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    var Partners = value.PartnerModel.Where(x => x.FullName != string.Empty).ToList();

                    await RealmDb.WriteAsync((realmDb) =>
                     {
                         foreach (var item in Partners)
                         {
                             realmDb.Add(item);
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static async Task LoadBrandAsync(IMoveService _moveService)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var value = await _moveService.GetBrandListAsync(AppSettings.User.SessionId);

                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    await RealmDb.WriteAsync((realmDb) =>
                     {
                         foreach (var item in value.BrandModel)
                         {
                             realmDb.Add(item);
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static async Task LoadDashboardPartnersAsync(IDashboardService _dashboardService)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var value = await _dashboardService.GetDashboardPartnersListAsync(AppSettings.User.CompanyId, AppSettings.User.SessionId);
                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    var partners = value.PossessorResponseModel.Where(x => x.Location.FullName != string.Empty).ToList();
                    await RealmDb.WriteAsync((realmDb) =>
                     {
                         foreach (var item in partners)
                             realmDb.Add(item);
                     });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static async Task LoadAssetSizeAsync(IMoveService _moveService)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetSizeModel> assetSizeModel = null;
            try
            {
                var model = await _moveService.GetAssetSizeAsync(AppSettings.User.SessionId, false);
                assetSizeModel = new List<AssetSizeModel>();
                foreach (var item in model)
                {
                    assetSizeModel.Add(new AssetSizeModel { AssetSize = item });
                }
                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in assetSizeModel)
                     {
                         realmDb.Add(item);
                     }
                 });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
            }
        }

        private static async Task LoadAssetTypeAsync(IMoveService _moveService)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetTypeModel> assetTypeModels = null;
            try
            {
                var model = await _moveService.GetAssetTypeAsync(AppSettings.User.SessionId, false);
                assetTypeModels = new List<AssetTypeModel>();
                foreach (var item in model)
                {
                    assetTypeModels.Add(new AssetTypeModel { AssetType = item });
                }

                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in assetTypeModels)
                     {
                         realmDb.Add(item);
                     }
                 });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                //service = null;
            }
        }

        private static async Task LoadAssetVolumeAsync(IDashboardService _dashboardService)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetVolumeModel> assetVolumeModel = null;
            try
            {
                var model = await _dashboardService.GetAssetVolumeAsync(AppSettings.User.SessionId, false);

                assetVolumeModel = new List<AssetVolumeModel>();
                foreach (var item in model)
                {
                    assetVolumeModel.Add(new AssetVolumeModel { AssetVolume = item });
                }
                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in assetVolumeModel)
                     {
                         realmDb.Add(item);
                     }
                 });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                //service = null;
            }
        }

        private static async Task LoadOwnerAsync(IMoveService _moveService)
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = await _moveService.GetOwnerAsync(AppSettings.User.SessionId);
                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in value.OwnerModel)
                     {
                         realmDb.Add(item);
                     }
                 });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
            }
        }
    }
}
