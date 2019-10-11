using System.Collections.ObjectModel;
using KegID.Services;
using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System;
using Microsoft.AppCenter.Crashes;
using Realms;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using KegID.Common;

namespace KegID.ViewModel
{
    public class PartnersViewModel : BaseViewModel
    {
        #region Properties

        private readonly IMoveService _moveService;
        public bool BrewerStockOn { get; set; }
        public bool IsWorking { get; set; }
        public string InternalBackgroundColor { get; set; } = "Transparent";
        public string InternalTextColor { get; set; } = "White";
        public string AlphabeticalBackgroundColor { get; set; } = "Transparent";
        public string AlphabeticalTextColor { get; set; } = "#4E6388";
        public ObservableCollection<PartnerModel> PartnerCollection { get; set; }
        public string PartnerName { get; set; }
        public string CommingFrom { get; set; }

        public IList<PartnerModel> AllPartners { get; set; }

        #endregion

        #region Commands

        public DelegateCommand InternalCommand { get; }
        public DelegateCommand AlphabeticalCommand { get; }
        public DelegateCommand<PartnerModel> ItemTappedCommand { get; }
        public DelegateCommand SearchPartnerCommand { get; }
        public DelegateCommand AddNewPartnerCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand TextChangedCommand { get; }

        #endregion

        #region Constructor

        public PartnersViewModel(IMoveService moveService, INavigationService navigationService) : base(navigationService)
        {
            _moveService = moveService;

            InternalCommand = new DelegateCommand(InternalCommandReciever);
            AlphabeticalCommand = new DelegateCommand(AlphabeticalCommandReciever);
            ItemTappedCommand = new DelegateCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
            SearchPartnerCommand = new DelegateCommand(SearchPartnerCommandRecieverAsync);
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            InternalBackgroundColor = "#4E6388";
            InternalTextColor = "White";
        }

        #endregion

        #region Methods

        private void TextChangedCommandRecieverAsync()
        {
            if (!string.IsNullOrEmpty(PartnerName))
            {
                try
                {
                    var notNullPartners = AllPartners.Where(x => x.FullName != null).ToList();
                    var result = notNullPartners.Where(x => x.FullName.IndexOf(PartnerName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                    PartnerCollection = new ObservableCollection<PartnerModel>(result);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                if (InternalTextColor.Contains("White"))
                    InternalCommandReciever();
                else
                    AlphabeticalCommandReciever();
            }
        }

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            try
            {
                if (model != null)
                {
                    ConstantManager.Partner = model;
                    await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "model", model },{ "CommingFrom", CommingFrom }
                    }, animated: false);

                    Cleanup();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async Task LoadPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());

            AllPartners = RealmDb.All<PartnerModel>().Where(x=>x.PartnerId != AppSettings.CompanyId).ToList();
            try
            {
                PartnerCollection = null;
                if (AllPartners.Count > 0)
                {
                    if (BrewerStockOn)
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                    else
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                }
                else
                {
                    DeletePartners();
                    await LoadMetaDataPartnersAsync();
                    await LoadPartnersAsync();
                }
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

        public async Task LoadMetaDataPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            try
            {
                var value = await _moveService.GetPartnersListAsync(AppSettings.SessionId);
                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    var Partners = value.PartnerModel.Where(x => x.FullName != string.Empty).ToList();

                    if (BrewerStockOn)
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                    else
                        PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                }
            }
            catch
            {
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

        private void AlphabeticalCommandReciever()
        {
            try
            {
                PartnerCollection = null;
                if (BrewerStockOn)
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName).Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                else
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName));

                AlphabeticalBackgroundColor = "#4E6388";
                AlphabeticalTextColor = "White";

                InternalBackgroundColor = "White";
                InternalTextColor = "#4E6388";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void InternalCommandReciever()
        {
            try
            {
                PartnerCollection = null;
                if (BrewerStockOn)
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                else
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);

                InternalBackgroundColor = "#4E6388";
                InternalTextColor = "White";

                AlphabeticalBackgroundColor = "White";
                AlphabeticalTextColor = "#4E6388";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BackCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
                Cleanup();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void AddNewPartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("AddPartnerView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void SearchPartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("SearchPartnersView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void Cleanup()
        {
            try
            {
                BrewerStockOn = false;
                PartnerCollection = null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BrewerStockOn"))
                BrewerStockOn = true;
            else
                BrewerStockOn = false;
            if (parameters.ContainsKey("GoingFrom"))
            {
                CommingFrom = parameters.GetValue<string>("GoingFrom");
            }
            await LoadPartnersAsync();

            //return base.InitializeAsync(parameters);
        }

        #endregion
    }
}
