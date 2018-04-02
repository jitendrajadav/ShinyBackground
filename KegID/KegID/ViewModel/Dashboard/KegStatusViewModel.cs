using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegStatusViewModel : BaseViewModel
    {
        #region Properties
        public IDashboardService _dashboardService { get; set; }

        #region KegStatuModel

        /// <summary>
        /// The <see cref="KegStatusModel" /> property's name.
        /// </summary>
        public const string KegStatusModelPropertyName = "KegStatusModel";

        private KegPossessionResponseModel _kegStatusModel = null;

        /// <summary>
        /// Sets and gets the KegStatusModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public KegPossessionResponseModel KegStatusModel
        {
            get
            {
                return _kegStatusModel;
            }

            set
            {
                if (_kegStatusModel == value)
                {
                    return;
                }

                _kegStatusModel = value;
                RaisePropertyChanged(KegStatusModelPropertyName);
            }
        }

        #endregion

        #region Owner

        /// <summary>
        /// The <see cref="Owner" /> property's name.
        /// </summary>
        public const string OwnerPropertyName = "Owner";

        private string _Owner = "Barcode Brewing";

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

        private string _Batch = "45";

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

        private IList<MaintainTypeReponseModel> _MaintenanceCollection = null;

        /// <summary>
        /// Sets and gets the MaintenanceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<MaintainTypeReponseModel> MaintenanceCollection
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

        private MaintainTypeReponseModel _SelectedMaintenance = null;

        /// <summary>
        /// Sets and gets the SelectedMaintenance property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MaintainTypeReponseModel SelectedMaintenance
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
                RaisePropertyChanged(SelectedMaintenancePropertyName);
            }
        }

        #endregion

        #region RemoveMaintenanceCollection

        /// <summary>
        /// The <see cref="RemoveMaintenanceCollection" /> property's name.
        /// </summary>
        public const string RemoveMaintenanceCollectionPropertyName = "RemoveMaintenanceCollection";

        private IList<MaintainTypeReponseModel> _RemoveMaintenanceCollection = null;

        /// <summary>
        /// Sets and gets the RemoveMaintenanceCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<MaintainTypeReponseModel> RemoveMaintenanceCollection
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

        private MaintainTypeReponseModel _RemoveSelecetedMaintenance = null;

        /// <summary>
        /// Sets and gets the RemoveSelecetedMaintenance property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MaintainTypeReponseModel RemoveSelecetedMaintenance
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
                RaisePropertyChanged(RemoveSelecetedMaintenancePropertyName);
            }
        }

        #endregion

        #region MaintenancePerformedCollection

        /// <summary>
        /// The <see cref="MaintenancePerformedCollection" /> property's name.
        /// </summary>
        public const string MaintenancePerformedCollectionPropertyName = "MaintenancePerformedCollection";

        private IList<MaintainTypeReponseModel> _MaintenancePerformedCollection = null;

        /// <summary>
        /// Sets and gets the MaintenancePerformedCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<MaintainTypeReponseModel> MaintenancePerformedCollection
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
        public KegStatusViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
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
        }

        public async Task LoadMaintenanceHistoryAsync()
        {
            var value = await _dashboardService.GetKegMaintenanceHistoryAsync(KegStatusModel.KegId, AppSettings.User.SessionId);

            if (value.KegMaintenanceHistoryResponseModel != null)
                IsVisibleListView = true;
            else
                IsVisibleListView = false;
        }

        private async void KegsCommandRecieverAsync()
        {
          await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void EditCommandRecieverAsync()
        {
            SimpleIoc.Default.GetInstance<EditKegViewModel>().LoadData(KegStatusModel);
            await Application.Current.MainPage.Navigation.PushModalAsync(new EditKegView());
        }

        private async void InvalidToolsCommandRecieverAsync()
        {
            MaintenanceAlertModel model = null;
            string maintenanceStr = string.Empty;

            try
            {
                model = await _dashboardService.GetKegMaintenanceAlertAsync(KegStatusModel.KegId, AppSettings.User.SessionId);
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

        #endregion

    }
}
