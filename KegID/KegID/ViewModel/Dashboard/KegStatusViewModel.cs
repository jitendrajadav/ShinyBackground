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
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        public LocationInfo Posision { get; set; }
        public List<MaintenanceAlert> Alerts { get; set; }

        #region Owner

        /// <summary>
        /// The <see cref="Owner" /> property's name.
        /// </summary>
        public const string OwnerPropertyName = "Owner";

        private string _Owner = string.Empty;

        /// <summary>
        /// Sets and gets the Owner property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Owner
        {
            get
            {
                return _Owner;
            }

            set
            {
                if (_Owner == value)
                {
                    return;
                }

                _Owner = value;
                RaisePropertyChanged(OwnerPropertyName);
            }
        }

        #endregion

        #region SizeName

        /// <summary>
        /// The <see cref="SizeName" /> property's name.
        /// </summary>
        public const string SizeNamePropertyName = "SizeName";

        private string _SizeName = string.Empty;

        /// <summary>
        /// Sets and gets the SizeName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string SizeName
        {
            get
            {
                return _SizeName;
            }

            set
            {
                if (_SizeName == value)
                {
                    return;
                }

                _SizeName = value;
                RaisePropertyChanged(SizeNamePropertyName);
            }
        }

        #endregion

        #region TypeName

        /// <summary>
        /// The <see cref="TypeName" /> property's name.
        /// </summary>
        public const string TypeNamePropertyName = "TypeName";

        private string _TypeName = string.Empty;

        /// <summary>
        /// Sets and gets the TypeName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TypeName
        {
            get
            {
                return _TypeName;
            }

            set
            {
                if (_TypeName == value)
                {
                    return;
                }

                _TypeName = value;
                RaisePropertyChanged(TypeNamePropertyName);
            }
        }

        #endregion

        #region Barcode

        /// <summary>
        /// The <see cref="Barcode" /> property's name.
        /// </summary>
        public const string BarcodePropertyName = "Barcode";

        private string _Barcode = string.Empty;

        /// <summary>
        /// Sets and gets the Barcode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Barcode
        {
            get
            {
                return _Barcode;
            }

            set
            {
                if (_Barcode == value)
                {
                    return;
                }

                _Barcode = value;
                RaisePropertyChanged(BarcodePropertyName);
            }
        }

        #endregion

        #region KegId

        /// <summary>
        /// The <see cref="KegId" /> property's name.
        /// </summary>
        public const string KegIdPropertyName = "KegId";

        private string _KegId = string.Empty;

        /// <summary>
        /// Sets and gets the KegId property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string KegId
        {
            get
            {
                return _KegId;
            }

            set
            {
                if (_KegId == value)
                {
                    return;
                }

                _KegId = value;
                RaisePropertyChanged(KegIdPropertyName);
            }
        }

        #endregion

        #region PossessorName

        /// <summary>
        /// The <see cref="PossessorName" /> property's name.
        /// </summary>
        public const string PossessorNamePropertyName = "PossessorName";

        private string _PossessorName = string.Empty;

        /// <summary>
        /// Sets and gets the PossessorName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PossessorName
        {
            get
            {
                return _PossessorName;
            }

            set
            {
                if (_PossessorName == value)
                {
                    return;
                }

                _PossessorName = value;
                RaisePropertyChanged(PossessorNamePropertyName);
            }
        }

        #endregion

        #region HeldDays

        /// <summary>
        /// The <see cref="HeldDays" /> property's name.
        /// </summary>
        public const string HeldDaysPropertyName = "HeldDays";

        private long _HeldDays = 0;

        /// <summary>
        /// Sets and gets the HeldDays property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public long HeldDays
        {
            get
            {
                return _HeldDays;
            }

            set
            {
                if (_HeldDays == value)
                {
                    return;
                }

                _HeldDays = value;
                RaisePropertyChanged(HeldDaysPropertyName);
            }
        }

        #endregion

        #region Contents

        /// <summary>
        /// The <see cref="Contents" /> property's name.
        /// </summary>
        public const string ContentsPropertyName = "Contents";

        private string _Contents = string.Empty;

        /// <summary>
        /// Sets and gets the Contents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Contents
        {
            get
            {
                return _Contents;
            }

            set
            {
                if (_Contents == value)
                {
                    return;
                }

                _Contents = value;
                RaisePropertyChanged(ContentsPropertyName);
            }
        }

        #endregion

        #region TagsStr

        /// <summary>
        /// The <see cref="TagsStr" /> property's name.
        /// </summary>
        public const string TagsStrPropertyName = "TagsStr";

        private string _TagsStr = "--";

        /// <summary>
        /// Sets and gets the TagsStr property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TagsStr
        {
            get
            {
                return _TagsStr;
            }

            set
            {
                if (_TagsStr == value)
                {
                    return;
                }

                _TagsStr = value;
                RaisePropertyChanged(TagsStrPropertyName);
            }
        }

        #endregion

        #region CurrentLocation

        /// <summary>
        /// The <see cref="CurrentLocation" /> property's name.
        /// </summary>
        public const string CurrentLocationPropertyName = "CurrentLocation";

        private string _CurrentLocation = string.Empty;

        /// <summary>
        /// Sets and gets the CurrentLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CurrentLocation
        {
            get
            {
                return _CurrentLocation;
            }

            set
            {
                if (_CurrentLocation == value)
                {
                    return;
                }

                _CurrentLocation = value;
                RaisePropertyChanged(CurrentLocationPropertyName);
            }
        }

        #endregion

        #region Batch

        /// <summary>
        /// The <see cref="Batch" /> property's name.
        /// </summary>
        public const string BatchPropertyName = "Batch";

        private string _Batch = string.Empty;

        /// <summary>
        /// Sets and gets the Batch property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Batch
        {
            get
            {
                return _Batch;
            }

            set
            {
                if (_Batch == value)
                {
                    return;
                }

                _Batch = value;
                RaisePropertyChanged(BatchPropertyName);
            }
        }

        #endregion

        #region MoveKeg

        /// <summary>
        /// The <see cref="MoveKeg" /> property's name.
        /// </summary>
        public const string MoveKegPropertyName = "MoveKeg";

        private string _MoveKeg = "Move keg";

        /// <summary>
        /// Sets and gets the MoveKeg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string MoveKeg
        {
            get
            {
                return _MoveKeg;
            }

            set
            {
                if (_MoveKeg == value)
                {
                    return;
                }

                _MoveKeg = value;
                RaisePropertyChanged(MoveKegPropertyName);
            }
        }

        #endregion

        #region MaintenanceCollection

        /// <summary>
        /// The <see cref="MaintenanceCollection" /> property's name.
        /// </summary>
        public const string MaintenanceCollectionPropertyName = "MaintenanceCollection";

        private ObservableCollection<MaintenanceAlert> _MaintenanceCollection = new ObservableCollection<MaintenanceAlert>();

        /// <summary>
        /// Sets and gets the MaintenanceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<MaintenanceAlert> MaintenanceCollection
        {
            get
            {
                return _MaintenanceCollection;
            }

            set
            {
                if (_MaintenanceCollection == value)
                {
                    return;
                }

                _MaintenanceCollection = value;
                RaisePropertyChanged(MaintenanceCollectionPropertyName);
            }
        }

        #endregion

        #region SelectedMaintenance

        /// <summary>
        /// The <see cref="SelectedMaintenance" /> property's name.
        /// </summary>
        public const string SelectedMaintenancePropertyName = "SelectedMaintenance";

        private MaintenanceAlert _SelectedMaintenance = null;

        /// <summary>
        /// Sets and gets the SelectedMaintenance property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MaintenanceAlert SelectedMaintenance
        {
            get
            {
                return _SelectedMaintenance;
            }

            set
            {
                if (_SelectedMaintenance == value)
                {
                    return;
                }

                _SelectedMaintenance = value;
                if (_SelectedMaintenance != null)
                    //AddAlertPerticularKegAsync(_SelectedMaintenance);
                RaisePropertyChanged(SelectedMaintenancePropertyName);
            }
        }


        #endregion

        #region RemoveMaintenanceCollection

        /// <summary>
        /// The <see cref="RemoveMaintenanceCollection" /> property's name.
        /// </summary>
        public const string RemoveMaintenanceCollectionPropertyName = "RemoveMaintenanceCollection";

        private ObservableCollection<MaintenanceAlert> _RemoveMaintenanceCollection = new ObservableCollection<MaintenanceAlert>();

        /// <summary>
        /// Sets and gets the RemoveMaintenanceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<MaintenanceAlert> RemoveMaintenanceCollection
        {
            get
            {
                return _RemoveMaintenanceCollection;
            }

            set
            {
                if (_RemoveMaintenanceCollection == value)
                {
                    return;
                }

                _RemoveMaintenanceCollection = value;
                RaisePropertyChanged(RemoveMaintenanceCollectionPropertyName);
            }
        }

        #endregion

        #region RemoveSelecetedMaintenance

        /// <summary>
        /// The <see cref="RemoveSelecetedMaintenance" /> property's name.
        /// </summary>
        public const string RemoveSelecetedMaintenancePropertyName = "RemoveSelecetedMaintenance";

        private MaintenanceAlert _RemoveSelecetedMaintenance = null;

        /// <summary>
        /// Sets and gets the RemoveSelecetedMaintenance property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MaintenanceAlert RemoveSelecetedMaintenance
        {
            get
            {
                return _RemoveSelecetedMaintenance;
            }

            set
            {
                if (_RemoveSelecetedMaintenance == value)
                {
                    return;
                }

                _RemoveSelecetedMaintenance = value;
                if (_RemoveSelecetedMaintenance != null)
                    //RemoveAlertPerticularKegAsync(_RemoveSelecetedMaintenance);
                RaisePropertyChanged(RemoveSelecetedMaintenancePropertyName);
            }
        }

        #endregion

        #region MaintenancePerformedCollection

        /// <summary>
        /// The <see cref="MaintenancePerformedCollection" /> property's name.
        /// </summary>
        public const string MaintenancePerformedCollectionPropertyName = "MaintenancePerformedCollection";

        private IList<KegMaintenanceHistoryResponseModel> _MaintenancePerformedCollection = null;

        /// <summary>
        /// Sets and gets the MaintenancePerformedCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<KegMaintenanceHistoryResponseModel> MaintenancePerformedCollection
        {
            get
            {
                return _MaintenancePerformedCollection;
            }

            set
            {
                if (_MaintenancePerformedCollection == value)
                {
                    return;
                }

                _MaintenancePerformedCollection = value;
                RaisePropertyChanged(MaintenancePerformedCollectionPropertyName);
            }
        }

        #endregion

        #region IsVisibleListView

        /// <summary>
        /// The <see cref="IsVisibleListView" /> property's name.
        /// </summary>
        public const string IsVisibleListViewPropertyName = "IsVisibleListView";

        private bool _IsVisibleListView = false;

        /// <summary>
        /// Sets and gets the IsVisibleListView property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsVisibleListView
        {
            get
            {
                return _IsVisibleListView;
            }

            set
            {
                if (_IsVisibleListView == value)
                {
                    return;
                }

                _IsVisibleListView = value;
                RaisePropertyChanged(IsVisibleListViewPropertyName);
            }
        }

        #endregion

        #region KegHasAlert

        /// <summary>
        /// The <see cref="KegHasAlert" /> property's name.
        /// </summary>
        public const string KegHasAlertPropertyName = "KegHasAlert";

        private bool _KegHasAlert = false;

        /// <summary>
        /// Sets and gets the KegHasAlert property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool KegHasAlert
        {
            get
            {
                return _KegHasAlert;
            }

            set
            {
                if (_KegHasAlert == value)
                {
                    return;
                }

                _KegHasAlert = value;
                RaisePropertyChanged(KegHasAlertPropertyName);
            }
        }

        #endregion

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

        public KegStatusViewModel(IDashboardService dashboardService, INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _dashboardService = dashboardService;
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
                //var param = new NavigationParameters
                //    {
                //        { "AssignInitialValueFromKegStatus", Barcode }, {"KegId",KegId }
                //    };
                await _navigationService.NavigateAsync(new Uri("MoveView", UriKind.Relative), new NavigationParameters
                    {
                        { "AssignInitialValueFromKegStatus", Barcode }, {"KegId",KegId }
                    }, useModalNavigation: true, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task LoadMaintenanceHistoryAsync(string _kegId,string _contents,long _heldDays,string _possessorName, string _barcode,string _typeName,string _sizeName)
        {
            try
            {
                Loader.StartLoading();
                KegId = _kegId;
                Contents = _contents == string.Empty ? "--" : _contents;
                HeldDays = _heldDays;
                PossessorName = _possessorName;
                Barcode = _barcode;
                TypeName = _typeName;
                SizeName = _sizeName;

                var kegStatus = await _dashboardService.GetKegStatusAsync(KegId, AppSettings.User.SessionId);
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var addMaintenanceCollection = RealmDb.All<MaintainTypeReponseModel>().ToList();
                KegHasAlert = kegStatus.MaintenanceAlerts.Count > 0 ? true : false;
                Alerts = kegStatus.MaintenanceAlerts;
                try
                {
                    foreach (var item in addMaintenanceCollection)
                    {
                        var flag = kegStatus.MaintenanceAlerts.Where(x => x.Id == item.Id).FirstOrDefault();
                        if (flag == null)
                            MaintenanceCollection.Add(new MaintenanceAlert { Id = (int)item.Id, ActivationMethod = item.ActivationMethod, AssetSize = "", AssetType = "", Barcode = Barcode, DefectType = item.DefectType.ToString(), DueDate = DateTimeOffset.Now, IsActivated = false, KegId = _kegId, LocationId = kegStatus.Location.PartnerId, LocationName = kegStatus.Owner.FullName, Message = "", Name = item.Name, OwnerId = kegStatus.Pallet?.OwnerId, OwnerName = kegStatus.Pallet?.OwnerName, PalletBarcode = kegStatus.Pallet?.Barcode, PalletId = kegStatus.Pallet?.PalletId, TypeId = kegStatus.TypeId, TypeName = kegStatus.TypeName });
                    }
                }
                catch (Exception ex)
                {
                     Crashes.TrackError(ex);
                }

                RemoveMaintenanceCollection = new ObservableCollection<MaintenanceAlert>( kegStatus.MaintenanceAlerts);
                Owner = kegStatus.Owner.FullName;
                Batch = kegStatus.Batch == string.Empty ? "--" : kegStatus.Batch;
                Posision = new LocationInfo
                {
                    Address = kegStatus.Location.Address,
                    Label = kegStatus.Location.City,
                    Lat = kegStatus.Location.Lat,
                    Lon = kegStatus.Location.Lon
                };

                var value = await _dashboardService.GetKegMaintenanceHistoryAsync(KegId, AppSettings.User.SessionId);
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
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
                //var param = new NavigationParameters
                //    {
                //        { "AssignInitialValue", Barcode }, {"KegId",KegId },{"Owner",Owner },{"TypeName",TypeName },{"SizeName",SizeName }
                //    };
                await _navigationService.NavigateAsync(new Uri("EditKegView", UriKind.Relative), new NavigationParameters
                    {
                        { "AssignInitialValue", Barcode }, {"KegId",KegId },{"Owner",Owner },{"TypeName",TypeName },{"SizeName",SizeName }
                    }, useModalNavigation: true, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void InvalidToolsCommandRecieverAsync()
        {
            string maintenanceStr = string.Empty;

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
                maintenanceStr = default(string);
            }
        }

        private async void CurrentLocationCommandRecieverAsync()
        {
            try
            {
                //var param = new NavigationParameters
                //    {
                //        { "Posision", Posision }
                //    };
                await _navigationService.NavigateAsync(new Uri("PartnerInfoMapView", UriKind.Relative), new NavigationParameters
                    {
                        { "Posision", Posision }
                    }, useModalNavigation: true, animated: false);

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

                Loader.StartLoading();
                var result = await _dashboardService.PostMaintenanceAlertAsync(model, AppSettings.User.SessionId, Configuration.PostedMaintenanceAlert);

                if (result.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    Loader.StopLoading();
                    await _dialogService.DisplayAlertAsync("Alert", "Alert adedd successfuly", "Ok");
                    try
                    {
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
                    var result = await _dashboardService.PostMaintenanceDeleteAlertUrlAsync(model, AppSettings.User.SessionId, Configuration.DeleteTypeMaintenanceAlert);
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

        public async override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("KegStatusModel"))
            {
                var model = parameters.GetValue<KegPossessionResponseModel>("KegStatusModel");
                await LoadMaintenanceHistoryAsync(model.KegId, model.Contents, model.HeldDays, model.PossessorName, model.Barcode, model.TypeName, model.SizeName);
            }
        }

        #endregion

    }
}
