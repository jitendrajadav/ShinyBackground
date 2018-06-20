using GalaSoft.MvvmLight.Ioc;
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
        public static async Task LoadInitializeMetaData()
        {
            await LoadAssetSizeAsync();
            await LoadAssetTypeAsync();
            await LoadAssetVolumeAsync();
            await LoadOwnerAsync();
            await LoadDashboardPartnersAsync();
            await LoadPartnersAsync();
            await LoadBrandAsync();
        }

        private static async Task LoadPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var service = SimpleIoc.Default.GetInstance<IMoveService>();

            try
            {
                var value = await service.GetPartnersListAsync(AppSettings.User.SessionId);
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

        private static async Task LoadBrandAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var service = SimpleIoc.Default.GetInstance<IMoveService>();
                var value = await service.GetBrandListAsync(AppSettings.User.SessionId);

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

        private static async Task LoadDashboardPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var service = SimpleIoc.Default.GetInstance<IDashboardService>();

                var value = await service.GetDashboardPartnersListAsync(AppSettings.User.CompanyId, AppSettings.User.SessionId);
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

        private static async Task LoadAssetSizeAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetSizeModel> assetSizeModel = null;
            var service = SimpleIoc.Default.GetInstance<IMoveService>();
            try
            {
                var model = await service.GetAssetSizeAsync(AppSettings.User.SessionId, false);
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
                service = null;
            }
        }

        private static async Task LoadAssetTypeAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetTypeModel> assetTypeModels = null;
            var service = SimpleIoc.Default.GetInstance<IMoveService>();
            try
            {
                var model = await service.GetAssetTypeAsync(AppSettings.User.SessionId, false);
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
                service = null;
            }
        }

        private static async Task LoadAssetVolumeAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetVolumeModel> assetVolumeModel = null;
            var service = SimpleIoc.Default.GetInstance<IDashboardService>();
            try
            {
                var model = await service.GetAssetVolumeAsync(AppSettings.User.SessionId, false);

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
                service = null;
            }
        }

        private static async Task LoadOwnerAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = await SimpleIoc.Default.GetInstance<IMoveService>().GetOwnerAsync(AppSettings.User.SessionId);
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
