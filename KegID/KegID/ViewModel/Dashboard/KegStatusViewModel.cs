using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;

namespace KegID.ViewModel
{
    public class KegStatusViewModel : BaseViewModel
    {
        #region Properties

        private readonly IDashboardService _dashboardService;
        private readonly IPageDialogService _dialogService;
        private readonly IUuidManager _uuidManager;

        public LocationInfo Posision { get; set; }
        public List<MaintenanceAlert> Alerts { get; set; }
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
        public string MoveKeg { get; set; } = "Move keg";
        public ObservableCollection<MaintenanceAlert> MaintenanceCollection { get; set; } = new ObservableCollection<MaintenanceAlert>();
        public MaintenanceAlert SelectedMaintenance { get; set; }
        public ObservableCollection<MaintenanceAlert> RemoveMaintenanceCollection { get; set; } = new ObservableCollection<MaintenanceAlert>();
        public MaintenanceAlert RemoveSelecetedMaintenance { get; set; }
        public IList<KegMaintenanceHistoryResponseModel> MaintenancePerformedCollection { get; set; }
        public bool IsVisibleListView { get; set; }
        public bool KegHasAlert { get; set; }

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

        public KegStatusViewModel(IDashboardService dashboardService, INavigationService navigationService, IPageDialogService dialogService, IUuidManager uuidManager) : base(navigationService)
        {
            //_navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _dashboardService = dashboardService;
            _uuidManager = uuidManager;

            KegsCommand = new DelegateCommand(KegsCommandRecieverAsync);
            EditCommand = new DelegateCommand(EditCommandRecieverAsync);
            InvalidToolsCommand = new DelegateCommand(InvalidToolsCommandRecieverAsync);
            CurrentLocationCommand = new DelegateCommand(CurrentLocationCommandRecieverAsync);
            MoveKegCommand = new DelegateCommand(MoveKegCommandRecieverAsync);
            AddAlertCommand = new DelegateCommand(AddAlertCommandRecieverAsync);
            RemoveAlertCommand = new DelegateCommand(RemoveAlertCommandRecieverAsync);
        }

        #endregion

        #region Methods

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
                Loader.StartLoading();
                KegId = _kegId;
                Contents = string.IsNullOrEmpty(_contents) ? "--" : _contents;
                HeldDays = _heldDays;
                PossessorName = _possessorName;
                Barcode = _barcode;
                TypeName = _typeName;
                SizeName = _sizeName;

                KegStatusResponseModel kegStatus = await _dashboardService.GetKegStatusAsync(KegId, AppSettings.SessionId);
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var addMaintenanceCollection = RealmDb.All<MaintainTypeReponseModel>().ToList();
                KegHasAlert = kegStatus.MaintenanceAlerts.Count > 0 ? true : false;
                Alerts = kegStatus.MaintenanceAlerts;
                try
                {
                    foreach (var item in addMaintenanceCollection)
                    {
                        var flag = kegStatus.MaintenanceAlerts.Find(x => x.Id == item.Id);
                        if (flag == null)
                            MaintenanceCollection.Add(new MaintenanceAlert { Id = (int)item.Id, ActivationMethod = item.ActivationMethod, AssetSize = "", AssetType = "", Barcode = Barcode, DefectType = item.DefectType.ToString(), DueDate = DateTimeOffset.Now, IsActivated = false, KegId = _kegId, LocationId = kegStatus.Location.PartnerId, LocationName = kegStatus.Owner.FullName, Message = "", Name = item.Name, OwnerId = kegStatus.Pallet?.OwnerId, OwnerName = kegStatus.Pallet?.OwnerName, PalletBarcode = kegStatus.Pallet?.Barcode, PalletId = kegStatus.Pallet?.PalletId, TypeId = kegStatus.TypeId, TypeName = kegStatus.TypeName });
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

                RemoveMaintenanceCollection = new ObservableCollection<MaintenanceAlert>(kegStatus.MaintenanceAlerts);
                Owner = kegStatus.Owner.FullName;
                Batch = kegStatus.Batch == string.Empty ? "--" : kegStatus.Batch;
                Posision = new LocationInfo
                {
                    Address = kegStatus.Location.Address,
                    Label = kegStatus.Location.City,
                    Lat = kegStatus.Location.Lat,
                    Lon = kegStatus.Location.Lon
                };

                var value = await _dashboardService.GetKegMaintenanceHistoryAsync(KegId, AppSettings.SessionId);
                MaintenancePerformedCollection = value.KegMaintenanceHistoryResponseModel;

                if (value.KegMaintenanceHistoryResponseModel.Count > 0)
                    IsVisibleListView = true;
                else
                    IsVisibleListView = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
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

        private async void AddAlertCommandRecieverAsync()
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
                    Loader.StartLoading();
                    var result = await _dashboardService.PostMaintenanceAlertAsync(model, AppSettings.SessionId, Configuration.PostedMaintenanceAlert);

                    if (result.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                    {
                        await _dialogService.DisplayAlertAsync("Alert", "Alert adedd successfuly", "Ok");

                        foreach (var item in result.AddMaintenanceAlertResponseModel)
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
                    Loader.StopLoading();
                    SelectedMaintenance = null;
                }
            }
        }

        private async void RemoveAlertCommandRecieverAsync()
        {
            if (RemoveSelecetedMaintenance != null)
            {
                var neededTypes = RemoveMaintenanceCollection.Where(x => x.Id == RemoveSelecetedMaintenance.Id).Select(x => x.Id).FirstOrDefault();
                Loader.StartLoading();
                var model = new DeleteMaintenanceAlertRequestModel
                {
                    KegId = KegId,
                    TypeId = neededTypes,
                };
                try
                {
                    var result = await _dashboardService.PostMaintenanceDeleteAlertUrlAsync(model, AppSettings.SessionId, Configuration.DeleteTypeMaintenanceAlert);
                    if (result.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                    {
                        Loader.StopLoading();
                        await _dialogService.DisplayAlertAsync("Alert", "Alert removed successfuly", "Ok");

                        foreach (var item in result.AddMaintenanceAlertResponseModel)
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
                    Loader.StopLoading();
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
        }

        #endregion

    }
}
