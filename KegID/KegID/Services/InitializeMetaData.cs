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
            await LoadPartnersAsync();
            await LoadOperators();
            await LoadMaintainTypeAsync();
            await LoadAssetSizeAsync();
            await LoadAssetTypeAsync();
            await LoadAssetVolumeAsync();
            await LoadOwnerAsync();
            await LoadDashboardPartnersAsync();
            await LoadBrandAsync();
            await LoadBatchAsync();
            await LoadPartnerTypeAsync();
            await LoadGetSkuListAsync();
        }

        private async Task LoadGetSkuListAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var model = await _fillService.GetSkuListAsync(AppSettings.SessionId);

                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in model.Sku)
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

        private async Task LoadOperators()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<OperatorModel> operators = null;
            try
            {
                var model = await _dashboardService.GetOperatorsAsync(AppSettings.SessionId);

                operators = new List<OperatorModel>();
                foreach (var item in model)
                {
                    operators.Add(new OperatorModel { Operator = item });
                }
                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in operators)
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

        private async Task LoadMaintainTypeAsync()
        {
            try
            {
                var maintenance = await _maintainService.GetMaintainTypeAsync(AppSettings.SessionId);
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in maintenance.MaintainTypeReponseModel)
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
        }

        public async Task LoadBatchAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                var value = await _fillService.GetBatchListAsync(AppSettings.SessionId);
                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    var batches = value.BatchModel.Where(p => p.BrandName != string.Empty).OrderBy(x => x.BrandName).ToList();
                    await RealmDb.WriteAsync((realmDb) =>
                    {
                        foreach (var item in batches)
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
            finally
            {
            }
        }

        private async Task LoadPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var value = await _moveService.GetPartnersListAsync(AppSettings.SessionId);
                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    var Partners = value.PartnerModel.Where(x => x.FullName != string.Empty).ToList();

                    RealmDb.Write(() =>
                     {
                         foreach (var item in Partners)
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
        }

        private async Task LoadBrandAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var value = await _moveService.GetBrandListAsync(AppSettings.SessionId);

                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    value.BrandModel.Insert(0, new BrandModel { BrandName = "Add", BrandCode = "Add", BrandId = Guid.NewGuid().ToString() });
                    value.BrandModel.Insert(1, new BrandModel { BrandName = "'\"'", BrandCode = "'\"'", BrandId = Guid.NewGuid().ToString() });
                    value.BrandModel.Move(value.BrandModel.Where(x => x.BrandName == "Empty").FirstOrDefault(), 2);

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
                var value = await _dashboardService.GetDashboardPartnersListAsync(AppSettings.CompanyId, AppSettings.SessionId);
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
                var model = await _moveService.GetAssetSizeAsync(AppSettings.SessionId, false);
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
                var model = await _moveService.GetAssetTypeAsync(AppSettings.SessionId, false);
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
                var model = await _dashboardService.GetAssetVolumeAsync(AppSettings.SessionId, false);

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
                var value = await _moveService.GetOwnerAsync(AppSettings.SessionId);
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
                var value = await _moveService.GetPartnerTypeAsync(AppSettings.SessionId);
                if (value.Response.StatusCode == nameof(System.Net.HttpStatusCode.OK))
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

        public void DeleteInitializeMetaData()
        {
            DeleteMaintainType();
            DeleteBatch();
            DeletePartners();
            DeleteBrand();
            DeleteDashboardPartners();
            DeleteAssetSize();
            DeleteAssetType();
            DeleteAssetVolume();
            DeleteOwner();
            DeletePartnerType();
        }

        private void DeleteMaintainType()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<MaintainTypeReponseModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteBatch()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<NewBatch>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeletePartners()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<PartnerModel>();
                    trans.Commit();
                }
                var AllPartners = RealmDb.All<PartnerModel>().ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteBrand()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<BrandModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteDashboardPartners()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<PossessorResponseModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteAssetSize()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<AssetSizeModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteAssetType()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<AssetTypeModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteAssetVolume()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<AssetVolumeModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeleteOwner()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<OwnerModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void DeletePartnerType()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                using (var trans = RealmDb.BeginWrite())
                {
                    RealmDb.RemoveAll<PartnerTypeModel>();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
