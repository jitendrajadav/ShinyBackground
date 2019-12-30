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
            try
            {
                var response = await ApiManager.GetSkuList(AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<Sku>>(json, GetJsonSetting()));

                    await RealmDb.WriteAsync((realmDb) =>
                    {
                        foreach (var item in data)
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
                //service = null;
            }
        }

        private async Task LoadOperators()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<OperatorModel> operators = null;
            try
            {
                var result = await ApiManager.GetOperators(AppSettings.SessionId);
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
                //service = null;
            }
        }

        private async Task LoadMaintainTypeAsync()
        {
            try
            {
                var response = await ApiManager.GetMaintainType(AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<MaintainTypeReponseModel>>(json, GetJsonSetting()));

                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                    await RealmDb.WriteAsync((realmDb) =>
                    {
                        foreach (var item in data)
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

        public async Task LoadBatchAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

                var response = await ApiManager.GetBatchList(AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<NewBatch>>(json, GetJsonSetting()));

                    var batches = data.Where(p => !string.IsNullOrEmpty(p.BrandName)).OrderBy(x => x.BrandName).ToList();
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
        }

        private async Task LoadPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var response = await ApiManager.GetPartnersList(AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<PartnerModel>>(json,GetJsonSetting()));

                    var Partners = data.Where(x => !string.IsNullOrEmpty(x.FullName)).ToList();

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
                var response = await ApiManager.GetBrandList(AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                     var data = JsonConvert.DeserializeObject<IList<BrandModel>>(json, GetJsonSetting());
                         //data = await Task.Run(() => JsonConvert.DeserializeObject<IList<BrandModel>>(json, GetJsonSetting()));
                    data.Insert(0, new BrandModel { BrandName = "Add", BrandCode = "Add", BrandId = Guid.NewGuid().ToString() });
                    data.Insert(1, new BrandModel { BrandName = "'\"'", BrandCode = "'\"'", BrandId = Guid.NewGuid().ToString() });
                    data.Move(data.FirstOrDefault(x => x.BrandName == "Empty"), 2);

                    await RealmDb.WriteAsync((realmDb) =>
                     {
                         foreach (var item in data)
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
                var result = await ApiManager.GetDashboardPartnersList(AppSettings.CompanyId, AppSettings.SessionId);
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadAsStringAsync();
                    var model = await Task.Run(() => JsonConvert.DeserializeObject<IList<PossessorResponseModel>>(response, GetJsonSetting()));
                    var partners = model.Where(x => !string.IsNullOrEmpty(x.Location.FullName)).ToList();
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
                var response = await ApiManager.GetAssetSize(AppSettings.SessionId, false);
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

        private async Task LoadAssetTypeAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetTypeModel> assetTypeModels = null;
            try
            {
                var response = await ApiManager.GetAssetType(AppSettings.SessionId, false);
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
                //service = null;
            }
        }

        private async Task LoadAssetVolumeAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            List<AssetVolumeModel> assetVolumeModel = null;
            try
            {
                var result = await ApiManager.GetAssetVolume(AppSettings.SessionId, false);
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
                //service = null;
            }
        }

        private async Task LoadOwnerAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var response = await ApiManager.GetOwner(AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<OwnerModel>>(json, GetJsonSetting()));

                    await RealmDb.WriteAsync((realmDb) =>
                 {
                     foreach (var item in data)
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

        public async Task LoadPartnerTypeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var response = await ApiManager.GetPartnerType(AppSettings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<PartnerTypeModel>>(json, GetJsonSetting()));
                    await RealmDb.WriteAsync((realmDb) =>
                     {
                         foreach (var item in data)
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
            try
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }
}
