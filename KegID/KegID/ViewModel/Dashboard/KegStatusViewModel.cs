using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;

namespace KegID.ViewModel
{
    public class KegStatusViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private readonly IUuidManager _uuidManager;

        public LocationInfo Posision { get; set; }
        public List<MaintenanceAlert> Alerts { get; set; }
        public string AltBarcode { get; set; }
        public string Owner { get; set; }
        public string SizeName { get; set; }
        public string TypeName { get; set; }
        public string Barcode { get; set; }
        public string KegId { get; set; }
        public string PossessorName { get; set; }
        public long HeldDays { get; set; }
        public string Contents { get; set; }
        public string TagsStr { get; set; }
        public string CurrentLocation { get; set; }
        public string Batch { get; set; }
        public string MoveKeg { get; set; }
        public ObservableCollection<MaintenanceAlert> MaintenanceCollection { get; set; } = new ObservableCollection<MaintenanceAlert>();
        public MaintenanceAlert SelectedMaintenance { get; set; }
        public ObservableCollection<MaintenanceAlert> RemoveMaintenanceCollection { get; set; } = new ObservableCollection<MaintenanceAlert>();
        public MaintenanceAlert RemoveSelecetedMaintenance { get; set; }
        public IList<KegMaintenanceHistoryResponseModel> MaintenancePerformedCollection { get; set; }
        public bool IsVisibleListView { get; set; }
        public bool KegHasAlert { get; set; }
        public string ContainerType { get; set; }

        #endregion

        #region Commands

        public DelegateCommand KegsCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand InvalidToolsCommand { get; }
        public DelegateCommand CurrentLocationCommand { get; }
        public DelegateCommand MoveKegCommand { get; }
        public DelegateCommand AddAlertCommand { get; }
        public DelegateCommand RemoveAlertCommand { get; }

        #endregion

        #region Constructor

        public KegStatusViewModel(INavigationService navigationService, IPageDialogService dialogService, IUuidManager uuidManager) : base(navigationService)
        {
            _dialogService = dialogService;
            _uuidManager = uuidManager;

            KegsCommand = new DelegateCommand(KegsCommandRecieverAsync);
            EditCommand = new DelegateCommand(EditCommandRecieverAsync);
            InvalidToolsCommand = new DelegateCommand(InvalidToolsCommandRecieverAsync);
            CurrentLocationCommand = new DelegateCommand(CurrentLocationCommandRecieverAsync);
            MoveKegCommand = new DelegateCommand(MoveKegCommandRecieverAsync);
            AddAlertCommand = new DelegateCommand(async() => await RunSafe(AddAlertCommandRecieverAsync()));
            RemoveAlertCommand = new DelegateCommand(async () => await RunSafe(RemoveAlertCommandRecieverAsync()));

            PreferenceSetting();
            MoveKeg = "Move "+ ContainerType;
        }

        #endregion

        #region Methods

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            ContainerType = preferences.Find(x => x.PreferenceName == "CONTAINER_TYPE").PreferenceValue;
        }

        private async void MoveKegCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("MoveView", new NavigationParameters
                    {
                        { "AssignInitialValueFromKegStatus", Barcode },
                        { "KegId", KegId },
                        { "ManifestId", _uuidManager.GetUuId() }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task LoadMaintenanceHistoryAsync(string _kegId, string _contents, long _heldDays, string _possessorName, string _barcode, string _typeName, string _sizeName)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading");
                KegId = _kegId;
                Contents = string.IsNullOrEmpty(_contents) ? "--" : _contents;
                HeldDays = _heldDays;
                PossessorName = _possessorName;
                Barcode = _barcode;
                TypeName = _typeName;
                SizeName = _sizeName;

                var kegStatusResponse = await ApiManager.GetKegStatus(KegId, Settings.SessionId);
                if (kegStatusResponse.IsSuccessStatusCode)
                {
                    var response = await kegStatusResponse.Content.ReadAsStringAsync();
                    var model = await Task.Run(() => JsonConvert.DeserializeObject<KegStatusResponseModel>(response, GetJsonSetting()));

                    var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                    var addMaintenanceCollection = RealmDb.All<MaintainTypeReponseModel>().ToList();
                    KegHasAlert = model.MaintenanceAlerts.Count > 0 ? true : false;
                    Alerts = model.MaintenanceAlerts;
                    try
                    {
                        foreach (var item in addMaintenanceCollection)
                        {
                            var flag = model.MaintenanceAlerts.Find(x => x.Id == item.Id);
                            if (flag == null)
                                MaintenanceCollection.Add(new MaintenanceAlert { Id = (int)item.Id, ActivationMethod = item.ActivationMethod, AssetSize = "", AssetType = "", Barcode = Barcode, DefectType = item.DefectType.ToString(), DueDate = DateTimeOffset.Now, IsActivated = false, KegId = _kegId, LocationId = model.Location.PartnerId, LocationName = model.Owner.FullName, Message = "", Name = item.Name, OwnerId = model.Pallet?.OwnerId, OwnerName = model.Pallet?.OwnerName, PalletBarcode = model.Pallet?.Barcode, PalletId = model.Pallet?.PalletId, TypeId = model.TypeId, TypeName = model.TypeName });
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }

                    RemoveMaintenanceCollection = new ObservableCollection<MaintenanceAlert>(model.MaintenanceAlerts);
                    Owner = model.Owner.FullName;
                    Batch = model.Batch == string.Empty ? "--" : model.Batch;
                    Posision = new LocationInfo
                    {
                        Address = model.Location.Address,
                        Label = model.Location.City,
                        Lat = model.Location.Lat,
                        Lon = model.Location.Lon
                    };

                    var kegMaintenaceResponse = await ApiManager.GetKegMaintenanceHistory(KegId, Settings.SessionId);
                    if (kegMaintenaceResponse.IsSuccessStatusCode)
                    {
                        var kegResponse = await kegMaintenaceResponse.Content.ReadAsStringAsync();
                        var kegModel = await Task.Run(() => JsonConvert.DeserializeObject<IList<KegMaintenanceHistoryResponseModel>>(kegResponse, GetJsonSetting()));

                        MaintenancePerformedCollection = kegModel;

                        IsVisibleListView = kegModel.Count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void KegsCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
                KegHasAlert = false;
                Alerts = null;
                MaintenanceCollection.Clear();
                RemoveMaintenanceCollection.Clear();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void EditCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("EditKegView", new NavigationParameters
                    {
                        { "AssignInitialValue", Barcode }, {"KegId",KegId },{"Owner",Owner },{"TypeName",TypeName },{"SizeName",SizeName }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void InvalidToolsCommandRecieverAsync()
        {
            string maintenanceStr = "";
            try
            {
                if (Alerts != null)
                {
                    foreach (var item in Alerts)
                        maintenanceStr += "-" + item.Name + "\n";
                    await _dialogService.DisplayAlertAsync("Warning", Resources["dialog_maintenance_performed_message"] + "\n" + maintenanceStr, "Ok");
                }
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                Alerts = null;
                maintenanceStr = default;
            }
        }

        private async void CurrentLocationCommandRecieverAsync()
        {
            try
            {
                ConstantManager.Position = Posision;
                await _navigationService.NavigateAsync("PartnerInfoMapView", new NavigationParameters
                    {
                        { "Posision", Posision }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task AddAlertCommandRecieverAsync()
        {
            if (SelectedMaintenance != null)
            {
                List<long> neededTypes = MaintenanceCollection.Where(x => x.Id == SelectedMaintenance.Id).Select(x => x.Id).ToList();
                var model = new AddMaintenanceAlertRequestModel
                {
                    AlertCc = "",
                    DueDate = SelectedMaintenance.DueDate,
                    KegId = KegId,
                    Message = SelectedMaintenance.Message,
                    NeededTypes = neededTypes,
                    ReminderDays = 5
                };

                try
                {
                    UserDialogs.Instance.ShowLoading("Loading");
                    var postMaintenaceAlertResponse = await ApiManager.PostMaintenanceAlert(model, Settings.SessionId);

                    if (postMaintenaceAlertResponse.IsSuccessStatusCode)
                    {
                        var response = await postMaintenaceAlertResponse.Content.ReadAsStringAsync();
                        var alertModel = await Task.Run(() => JsonConvert.DeserializeObject<IList<AddMaintenanceAlertResponseModel>>(response, GetJsonSetting()));

                        await _dialogService.DisplayAlertAsync("Alert", "Alert adedd successfuly", "Ok");

                        foreach (var item in alertModel)
                        {
                            var removal = MaintenanceCollection.Where(x => x.Id == item.MaintenanceType.Id).FirstOrDefault();
                            if (removal != null)
                            {
                                MaintenanceCollection.Remove(removal);
                                RemoveMaintenanceCollection.Add(removal);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                    SelectedMaintenance = null;
                }
            }
        }

        private async Task RemoveAlertCommandRecieverAsync()
        {
            if (RemoveSelecetedMaintenance != null)
            {
                var neededTypes = RemoveMaintenanceCollection.Where(x => x.Id == RemoveSelecetedMaintenance.Id).Select(x => x.Id).FirstOrDefault();
                UserDialogs.Instance.ShowLoading("Loading");
                var model = new DeleteMaintenanceAlertRequestModel
                {
                    KegId = KegId,
                    TypeId = neededTypes,
                };
                try
                {
                    var result = await ApiManager.PostMaintenanceDeleteAlertUrl(model, Settings.SessionId);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        var deleteModel = await Task.Run(() => JsonConvert.DeserializeObject<IList<AddMaintenanceAlertResponseModel>>(response, GetJsonSetting()));

                        UserDialogs.Instance.HideLoading();
                        await _dialogService.DisplayAlertAsync("Alert", "Alert removed successfuly", "Ok");

                        foreach (var item in deleteModel)
                        {
                            var removedItem = RemoveMaintenanceCollection.Where(x => x.Id != item.MaintenanceType.Id).First();
                            if (removedItem != null)
                            {
                                RemoveMaintenanceCollection.Remove(removedItem);
                                MaintenanceCollection.Add(removedItem);
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                     Crashes.TrackError(ex);
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                    RemoveSelecetedMaintenance = null;
                }
            }
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("KegStatusModel"))
            {
                var model = parameters.GetValue<KegPossessionResponseModel>("KegStatusModel");
                await LoadMaintenanceHistoryAsync(model.KegId, model.Contents, model.HeldDays, model.PossessorName, model.Barcode, model.TypeName, model.SizeName);
            }
            if (parameters.ContainsKey("KegSearchedKegStatusModel"))
            {
                var model = parameters.GetValue<KegSearchResponseModel>("KegSearchedKegStatusModel");
                await LoadMaintenanceHistoryAsync(model.KegId, model.Contents, 0, model?.Location?.FullName, model.Barcode, model.TypeName, model.SizeName);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("KegsCommandRecieverAsync"))
            {
                KegsCommandRecieverAsync();
            }
            else if (parameters.ContainsKey("TagsStr"))
            {
                TagsStr = parameters.GetValue<string>("TagsStr");
                Owner = parameters.GetValue<string>("Owner");
                SizeName = parameters.GetValue<string>("Size");
                TypeName = parameters.GetValue<string>("Type");
                AltBarcode = parameters.GetValue<string>("AltBarcode");
            }
        }

        #endregion

    }
}
