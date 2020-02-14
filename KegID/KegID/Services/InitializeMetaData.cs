using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.ViewModel;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Navigation;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KegID.Services
{
    public class InitializeMetaData : BaseViewModel, IInitializeMetaData
    {
        public InitializeMetaData(INavigationService navigationService) : base(navigationService)
        {
        }

        public async Task LoadInitializeMetaData()
        {
            await LoadBatchAsync();
            await LoadPartnersAsync();
            await LoadOperators();
            await LoadMaintainTypeAsync();
            await LoadAssetSizeAsync();
            await LoadAssetTypeAsync();
            await LoadAssetVolumeAsync();
            await LoadOwnerAsync();
            await LoadDashboardPartnersAsync();
            await LoadBrandAsync();
            await LoadPartnerTypeAsync();
            await LoadGetSkuListAsync();
        }

        private async Task LoadGetSkuListAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var response = await ApiManager.GetSkuList(Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<Sku>>(json, GetJsonSetting()));

                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in data)
                    {
                        realmDb.Add(item);
                    }
                });
            }


        }

        private async Task LoadOperators()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<OperatorModel> operators = null;
            var result = await ApiManager.GetOperators(Settings.SessionId);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                var model = await Task.Run(() => JsonConvert.DeserializeObject<IList<string>>(response, GetJsonSetting()));

                operators = new List<OperatorModel>();
                foreach (var item in model)
                {
                    operators.Add(new OperatorModel { Operator = item });
                }
                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in operators)
                    {
                        realmDb.Add(item);
                    }
                });
            }
        }

        private async Task LoadMaintainTypeAsync()
        {
            var response = await ApiManager.GetMaintainType(Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<MaintainTypeReponseModel>>(json, GetJsonSetting()));

                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in data)
                    {
                        realmDb.Add(item);
                    }
                });
            }
        }

        public async Task LoadBatchAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

            var response = await ApiManager.GetBatchList(Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<NewBatch>>(json, GetJsonSetting()));

                var batches = data.Where(p => !string.IsNullOrEmpty(p.BrandName)).OrderBy(x => x.BrandName).ToList();
                await RealmDb.WriteAsync((realmDb) =>
                {
                    foreach (var item in batches)
                    {
                        realmDb.Add(item);

                    }
                });
            }

        }

        private async Task LoadPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var response = await ApiManager.GetPartnersList(Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<PartnerModel>>(json, GetJsonSetting()));

                var Partners = data.Where(x => !string.IsNullOrEmpty(x.FullName)).ToList();

                await RealmDb.WriteAsync((realmDb) =>
                  {
                      foreach (var item in Partners)
                      {
                          realmDb.Add(item);
                      }
                  });
            }
        }

        private async Task LoadBrandAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var response = await ApiManager.GetBrandList(Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<IList<BrandModel>>(json, GetJsonSetting());
                data.Insert(0, new BrandModel { BrandName = "Add", BrandCode = "Add", BrandId = Guid.NewGuid().ToString() });
                data.Insert(1, new BrandModel { BrandName = "'\"'", BrandCode = "'\"'", BrandId = Guid.NewGuid().ToString() });
                data.Move(data.FirstOrDefault(x => x.BrandName == "Empty"), 2);

                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in data)
                     {
                         realmDb.Add(item);

                     }
                 });
            }
        }

        private async Task LoadDashboardPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

            var result = await ApiManager.GetDashboardPartnersList(Settings.CompanyId, Settings.SessionId);
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();
                var model = await Task.Run(() => JsonConvert.DeserializeObject<IList<PossessorResponseModel>>(response, GetJsonSetting()));
                var partners = model.Where(x => !string.IsNullOrEmpty(x.Location.FullName)).ToList();
                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in partners)
                     {
                         realmDb.Add(item);
                     }
                 });
            }

        }
        private async Task LoadAssetSizeAsync()
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                List<AssetSizeModel> assetSizeModel = null;
                var response = await ApiManager.GetAssetSize(Settings.SessionId, false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<string>>(json, GetJsonSetting()));

                    assetSizeModel = new List<AssetSizeModel>();
                    foreach (var item in data)
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
            }

            private async Task LoadAssetTypeAsync()
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                List<AssetTypeModel> assetTypeModels = null;
                var response = await ApiManager.GetAssetType(Settings.SessionId, false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<string>>(json, GetJsonSetting()));

                    assetTypeModels = new List<AssetTypeModel>();
                    foreach (var item in data)
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
            }

            private async Task LoadAssetVolumeAsync()
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                List<AssetVolumeModel> assetVolumeModel = null;
                var result = await ApiManager.GetAssetVolume(Settings.SessionId, false);
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadAsStringAsync();
                    var model = await Task.Run(() => JsonConvert.DeserializeObject<IList<string>>(response, GetJsonSetting()));

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
            }

            private async Task LoadOwnerAsync()
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var response = await ApiManager.GetOwner(Settings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<OwnerModel>>(json, GetJsonSetting()));

                    await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in data)
                     {
                         realmDb.Add(item);
                     }
                 });
                }
            }

        public async Task LoadPartnerTypeAsync()
        {

            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var response = await ApiManager.GetPartnerType(Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<PartnerTypeModel>>(json, GetJsonSetting()));
                await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in data)
                     {
                         realmDb.Add(item);
                     }
                 });
            }

        }

        public void DeleteInitializeMetaData()
                {
                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    using (var trans = RealmDb.BeginWrite())
                    {
                        RealmDb.RemoveAll<MaintainTypeReponseModel>();
                        RealmDb.RemoveAll<PartnerTypeModel>();
                        RealmDb.RemoveAll<OwnerModel>();
                        RealmDb.RemoveAll<AssetVolumeModel>();
                        RealmDb.RemoveAll<AssetTypeModel>();
                        RealmDb.RemoveAll<AssetSizeModel>();
                        RealmDb.RemoveAll<PossessorResponseModel>();
                        RealmDb.RemoveAll<BrandModel>();
                        RealmDb.RemoveAll<PartnerModel>();
                        RealmDb.RemoveAll<NewBatch>();
                        trans.Commit();
                    }
                }
            }
        }
