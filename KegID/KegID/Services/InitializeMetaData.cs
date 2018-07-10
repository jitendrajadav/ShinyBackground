using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KegID.Services
{
    public class InitializeMetaData : IInitializeMetaData
    {
        private IMoveService _moveService { get; set; }
        private IMaintainService _maintainService { get; set; }
        private IDashboardService _dashboardService { get; set; }
        private IFillService _fillService { get; set; }

        public InitializeMetaData(IMoveService moveService, IDashboardService dashboardService, IMaintainService maintainService, IFillService fillService)
        {
            _moveService = moveService;
            _maintainService = maintainService;
            _dashboardService = dashboardService;
            _fillService = fillService;
        }

        public async Task LoadInitializeMetaData()
        {
            await LoadAssetSizeAsync();
            await LoadAssetTypeAsync();
            await LoadAssetVolumeAsync();
            await LoadOwnerAsync();
            await LoadDashboardPartnersAsync();
            await LoadPartnersAsync();
            await LoadBrandAsync();
            await LoadMaintenanceTypeAsync();
            await LoadBatchAsync();
            await LoadPartnerTypeAsync();
        }

        public async Task LoadBatchAsync()
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
                            try
                            {
                                RealmDb.Add(item);
                            }
                            catch (Exception ex)
                            {
                                Crashes.TrackError(ex);
                            }
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

        public async Task<IList<MaintainTypeReponseModel>> LoadMaintenanceTypeAsync()
        {
            var model = await _maintainService.GetMaintainTypeAsync(AppSettings.User.SessionId);
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(() =>
                {
                    foreach (var item in model.MaintainTypeReponseModel)
                    {
                        try
                        {
                            RealmDb.Add(item);
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                        }
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

        private async Task LoadPartnersAsync()
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
                             try
                             {
                                 realmDb.Add(item);
                             }
                             catch (Exception ex)
                             {
                                 Crashes.TrackError(ex);
                             }
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task LoadBrandAsync()
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
                             try
                             {
                                 realmDb.Add(item);
                             }
                             catch (Exception ex)
                             {
                                 Crashes.TrackError(ex);
                             }
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task LoadDashboardPartnersAsync()
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
                         {
                             try
                             {
                                 realmDb.Add(item);
                             }
                             catch (Exception ex)
                             {
                                 Crashes.TrackError(ex);
                             }
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task LoadAssetSizeAsync()
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
                         try
                         {
                             realmDb.Add(item);
                         }
                         catch (Exception ex)
                         {
                             Crashes.TrackError(ex);
                         }
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

        private async Task LoadAssetTypeAsync()
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
                         try
                         {
                             realmDb.Add(item);
                         }
                         catch (Exception ex)
                         {
                             Crashes.TrackError(ex);
                         }
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

        private async Task LoadAssetVolumeAsync()
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
                         try
                         {
                             realmDb.Add(item);
                         }
                         catch (Exception ex)
                         {
                             Crashes.TrackError(ex);
                         }
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

        private async Task LoadOwnerAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = await _moveService.GetOwnerAsync(AppSettings.User.SessionId);
                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in value.OwnerModel)
                     {
                         try
                         {
                             realmDb.Add(item);
                         }
                         catch (Exception ex)
                         {
                             Crashes.TrackError(ex);
                         }
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

        public async Task LoadPartnerTypeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = await _moveService.GetPartnerTypeAsync(AppSettings.User.SessionId);
                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    await RealmDb.WriteAsync((realmDb) =>
                     {
                         foreach (var item in value.PartnerTypeModel)
                         {
                             try
                             {
                                 realmDb.Add(item);
                             }
                             catch (Exception ex)
                             {
                                 Crashes.TrackError(ex);
                             }
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
