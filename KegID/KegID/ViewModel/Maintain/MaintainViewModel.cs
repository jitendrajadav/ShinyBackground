using KegID.LocalDb;
using KegID.Messages;
using KegID.Model;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class MaintainViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        private ManifestModel ManifestModel;
        public PartnerModel PartnerModel { get; set; } = new PartnerModel();
        public string Notes { get; set; }
        public ObservableCollection<MaintenanceTypeModel> MaintainTypeCollection { get; set; } = new ObservableCollection<MaintenanceTypeModel>();
        public bool Operator { get; set; }

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand PartnerCommand { get; }
        public DelegateCommand<MaintenanceTypeModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public MaintainViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            PartnerCommand = new DelegateCommand(PartnerCommandRecieverAsync);
            NextCommand = new DelegateCommand(NextCommandRecieverAsync);
            PartnerModel.FullName = "Select a location";
            ItemTappedCommand = new DelegateCommand<MaintenanceTypeModel>((model) => ItemTappedCommandReciever(model));
            HandleUnsubscribeMessages();
            HandleReceivedMessages();

            PreferenceSetting();
        }


        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            var preferenceOperator = preferences.Find(x => x.PreferenceName == "Operator");
            Operator = preferenceOperator != null && bool.Parse(preferenceOperator.PreferenceValue);
        }

        private void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<MaintainDTToMaintMsg>(this, "MaintainDTToMaintMsg", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Cleanup();
                });
            });
        }

        private void HandleUnsubscribeMessages()
        {
            MessagingCenter.Unsubscribe<MaintainDTToMaintMsg>(this, "MaintainDTToMaintMsg");
        }

        #endregion

        #region Methods

        private void ItemTappedCommandReciever(MaintenanceTypeModel model)
        {
            MaintainTypeCollection
                .Where(x => x.Id == model.Id)
                .FirstOrDefault(x => x.IsToggled = !model.IsToggled);
        }

        public void LoadMaintenanceTypeAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var result = RealmDb.All<MaintainTypeReponseModel>();

            foreach (var item in result.Where(x => x.ActivationMethod == "ReverseOnly").OrderBy(x => x.Name).ToList())
            {
                MaintainTypeCollection.Add(new MaintenanceTypeModel { ActivationMethod = item.ActivationMethod, DefectType = item.DefectType, DeletedDate = item.DeletedDate, Description = item.Description, Id = item.Id, InUse = item.InUse, IsAction = item.IsAction, IsAlert = item.IsAlert, IsToggled = item.IsToggled, Name = item.Name });
            }
        }

        private async void HomeCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void PartnerCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("PartnersView", animated: false);
        }

        private async void NextCommandRecieverAsync()
        {
            var selectedMaintenance = MaintainTypeCollection.Where(x => x.IsToggled).ToList();
            if (selectedMaintenance.Count > 0)
            {
                await _navigationService.NavigateAsync("MaintainScanView",
                    new NavigationParameters
                    {
                            { "Notes", Notes },
                            { "PartnerModel", PartnerModel},
                            { "ManifestModel", ManifestModel },
                            { "selectedMaintenance", selectedMaintenance }
                    }, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Please select at least one maintenance item to perform.", "Ok");
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.Keys.FirstOrDefault())
            {
                case "model":
                    PartnerModel = parameters.GetValue<PartnerModel>("model");
                    break;
                case "Cleanup":
                    Cleanup();
                    break;
            }
            if (parameters.ContainsKey("MaintainHome"))
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("LoadMaintenanceTypeAsync"))
            {
                LoadMaintenanceTypeAsync();
            }

            if (parameters.ContainsKey("AssignInitialValue"))
            {
                AssignInitialValue(parameters.GetValue<ManifestModel>("AssignInitialValue"));
            }
            return base.InitializeAsync(parameters);
        }

        private void AssignInitialValue(ManifestModel manifestModel)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var maintenance = RealmDb.All<MaintainTypeReponseModel>().ToList();

            ManifestModel = manifestModel;
            foreach (var item in maintenance.Where(x => x.ActivationMethod == "ReverseOnly").OrderBy(x => x.Name).ToList())
            {
                MaintainTypeCollection.Add(new MaintenanceTypeModel { ActivationMethod = item.ActivationMethod, DefectType = item.DefectType, DeletedDate = item.DeletedDate, Description = item.Description, Id = item.Id, InUse = item.InUse, IsAction = item.IsAction, IsAlert = item.IsAlert, IsToggled = item.IsToggled, Name = item.Name });
            }

            foreach (var item in manifestModel.MaintenanceModels.MaintenanceDoneRequestModel.ActionsPerformed)
            {
                var result = maintenance.Find(x => x.Id == item);
                if (result != null)
                {
                    MaintainTypeCollection.FirstOrDefault(x => x.Id == result.Id).IsToggled = true;
                }
            }
            Notes = manifestModel?.MaintenanceModels?.MaintenanceDoneRequestModel?.Notes;
            PartnerModel = manifestModel?.MaintenanceModels?.MaintenanceDoneRequestModel?.PartnerModel;
        }

        private void Cleanup()
        {
            //using (var trans = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
            //{
            PartnerModel.FullName = "Select a location";
            //trans.Commit();
            //}
            LoadMaintenanceTypeAsync();
            Notes = string.Empty;
        }

        #endregion
    }
}
