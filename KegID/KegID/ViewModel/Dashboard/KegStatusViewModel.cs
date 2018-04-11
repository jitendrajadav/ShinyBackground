using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegStatusViewModel : BaseViewModel
    {
        #region Properties
        public IDashboardService DashboardService { get; set; }

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

        private IList<MaintenanceAlert> _MaintenanceCollection = null;

        /// <summary>
        /// Sets and gets the MaintenanceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<MaintenanceAlert> MaintenanceCollection
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
                AddAlertPerticularKegAsync(_SelectedMaintenance);
                RaisePropertyChanged(SelectedMaintenancePropertyName);
            }
        }


        #endregion

        #region RemoveMaintenanceCollection

        /// <summary>
        /// The <see cref="RemoveMaintenanceCollection" /> property's name.
        /// </summary>
        public const string RemoveMaintenanceCollectionPropertyName = "RemoveMaintenanceCollection";

        private IList<MaintenanceAlert> _RemoveMaintenanceCollection = null;

        /// <summary>
        /// Sets and gets the RemoveMaintenanceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<MaintenanceAlert> RemoveMaintenanceCollection
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
                RemoveAlertPerticularKegAsync(_RemoveSelecetedMaintenance);
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

        #endregion

        #region Commands

        public RelayCommand KegsCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand InvalidToolsCommand { get; }
        public RelayCommand CurrentLocationCommand { get; }
        public RelayCommand MoveKegCommand { get; }

        #endregion

        #region Constructor
        public KegStatusViewModel(IDashboardService _dashboardService)
        {
            DashboardService = _dashboardService;
            KegsCommand = new RelayCommand(KegsCommandRecieverAsync);
            EditCommand = new RelayCommand(EditCommandRecieverAsync);
            InvalidToolsCommand = new RelayCommand(InvalidToolsCommandRecieverAsync);
            CurrentLocationCommand = new RelayCommand(CurrentLocationCommandRecieverAsync);
            MoveKegCommand = new RelayCommand(MoveKegCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void MoveKegCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new MoveView());
            SimpleIoc.Default.GetInstance<MoveViewModel>().AssignInitialValue(KegId,Barcode);
        }

        public async Task LoadMaintenanceHistoryAsync(string _kegId,string _contents,long _heldDays,string _possessorName, string _barcode,string _typeName,string _sizeName)
        {
            try
            {
                KegId = _kegId;
                Contents = _contents == string.Empty ? "--" : _contents;
                HeldDays = _heldDays;
                PossessorName = _possessorName;
                Barcode = _barcode;
                TypeName = _typeName;
                SizeName = _sizeName;

                var kegStatus = await DashboardService.GetKegStatusAsync(KegId, AppSettings.User.SessionId);

                var addMaintenanceCollection = await SQLiteServiceClient.Db.Table<MaintainTypeReponseModel>().ToListAsync();
                MaintenanceCollection = new List<MaintenanceAlert>();

                foreach (var item in addMaintenanceCollection)
                {
                    var flag = kegStatus.MaintenanceAlerts.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (flag == null)
                        MaintenanceCollection.Add(new MaintenanceAlert { Id = (int)item.Id, ActivationMethod = (int)item.ActivationMethod, AssetSize = "", AssetType = "", Barcode = "", DefectType = item.DefectType.ToString(), DueDate = DateTime.Now, IsActivated = false, KegId = "", LocationId = "", LocationName = "", Message = "", Name = item.Name, OwnerId = "", OwnerName = "", PalletBarcode = "", PalletId = "", TypeId = 0, TypeName = "" });
                }

                RemoveMaintenanceCollection = kegStatus.MaintenanceAlerts;
                Owner = kegStatus.Owner.FullName;
                Batch = kegStatus.Batch == string.Empty ? "--" : kegStatus.Batch;
                //KegId = "6762E448-B6AD-4CE1-BA31-865DF01F6334";

                var value = await DashboardService.GetKegMaintenanceHistoryAsync(KegId, AppSettings.User.SessionId);
                MaintenancePerformedCollection = value.KegMaintenanceHistoryResponseModel;

                if (value.KegMaintenanceHistoryResponseModel.Count > 0)
                    IsVisibleListView = true;
                else
                    IsVisibleListView = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void KegsCommandRecieverAsync()
        {
          await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void EditCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<EditKegViewModel>().AssingInitialValue(KegId,Barcode, Owner, TypeName,SizeName);
            await Application.Current.MainPage.Navigation.PushModalAsync(new EditKegView());
        }

        private async void InvalidToolsCommandRecieverAsync()
        {
            MaintenanceAlertModel model = null;
            string maintenanceStr = string.Empty;

            try
            {
                model = await DashboardService.GetKegMaintenanceAlertAsync(KegId, AppSettings.User.SessionId);
                if (model != null)
                {
                    foreach (var item in model.MaintenanceAlertResponseModel)
                    {
                        maintenanceStr += "-" + item.MaintenanceType.Name + "\n";
                    }

                    await Application.Current.MainPage.DisplayAlert("Warning", Resources["dialog_maintenance_performed_message"] + "\n" + maintenanceStr, "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                model = null;
                maintenanceStr = default(string);
            }
        }

        private async void CurrentLocationCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new PartnerInfoMapView());
        }

        private async void AddAlertPerticularKegAsync(MaintenanceAlert _model)
        {
            var model = new AddMaintenanceAlertRequestModel
            {
                AlertCc = "",
                DueDate = _model.DueDate,
                KegId = KegId,
                Message = _model.Message,
                NeededTypes = new List<long>(),
                ReminderDays = 5
            };
            await DashboardService.PostMaintenanceAlertAsync(model, AppSettings.User.SessionId, Configuration.PostedMaintenanceAlert);
        }

        private async void RemoveAlertPerticularKegAsync(MaintenanceAlert _model)
        {
            var model = new AddMaintenanceAlertRequestModel
            {
                AlertCc = "",
                DueDate = _model.DueDate,
                KegId = KegId,
                Message = _model.Message,
                NeededTypes = new List<long>(),
                ReminderDays = 5
            };
            await DashboardService.PostMaintenanceDeleteAlertUrlAsync(model, AppSettings.User.SessionId, Configuration.PostedMaintenanceAlert);
        }

        #endregion

    }
}
