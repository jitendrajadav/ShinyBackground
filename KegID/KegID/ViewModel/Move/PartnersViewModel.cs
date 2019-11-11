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
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class PartnersViewModel : BaseViewModel
    {
        #region Properties

        private readonly IMoveService _moveService;
        private readonly IGeolocationService _geolocationService;

        public bool BrewerStockOn { get; set; }
        public bool IsWorking { get; set; }
        public ObservableCollection<PartnerModel> PartnerCollection { get; set; }
        public string PartnerName { get; set; }
        public string CommingFrom { get; set; }
        public int? SelectedSegment { get; private set; }

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
        public DelegateCommand<object> SelectedSegmentCommand { get; }

        #endregion

        #region Constructor

        public PartnersViewModel(IMoveService moveService, INavigationService navigationService, IGeolocationService geolocationService) : base(navigationService)
        {
            _moveService = moveService;
            _geolocationService = geolocationService;

            ItemTappedCommand = new DelegateCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
            SearchPartnerCommand = new DelegateCommand(SearchPartnerCommandRecieverAsync);
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            SelectedSegmentCommand = new DelegateCommand<object>((seg) => SelectedSegmentCommandReciever(seg));
        }

        #endregion

        #region Methods

        private void TextChangedCommandRecieverAsync()
        {
            if (!string.IsNullOrEmpty(PartnerName))
            {
                try
                {
                    if (SelectedSegment == 2)
                    {
                        var notNullPartners = AllPartners.Where(x => x.SourceKey != null).ToList();
                        List<PartnerModel> result = notNullPartners.Where(x => x.SourceKey.IndexOf(PartnerName, StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                        PartnerCollection = new ObservableCollection<PartnerModel>(result);
                    }
                    else
                    {
                        var notNullPartners = AllPartners.Where(x => x.FullName != null).ToList();
                        var result = notNullPartners.Where(x => x.FullName.IndexOf(PartnerName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                        PartnerCollection = new ObservableCollection<PartnerModel>(result);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                SelectedSegmentCommandReciever(SelectedSegment);
            }
        }

        private async void SelectedSegmentCommandReciever(object seg)
        {
            if (SelectedSegment != (int)seg)
            {
                SelectedSegment = (int)seg;
                if (AllPartners.Count > 0)
                {
                    switch (seg)
                    {
                        case 0:
                            if (BrewerStockOn)
                                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                            else
                                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
                            break;
                        case 1:
                            if (BrewerStockOn)
                                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName).Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                            else
                                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.FullName));
                            break;
                        case 2:
                            var locationStart = await _geolocationService.GetLastLocationAsync();
                            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                            using (var trans = RealmDb.BeginWrite())
                            {
                                foreach (var item in AllPartners)
                                {
                                    item.Distance = Xamarin.Essentials.Location.CalculateDistance(locationStart, item.Lat, item.Lon, DistanceUnits.Miles);
                                }
                                trans.Commit();
                            }
                            if (BrewerStockOn)
                                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.Distance).ToList());
                            else
                                PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.OrderBy(x => x.Distance).ToList());
                            break;
                    }
                }
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
                if (AllPartners.Count <= 0)
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
            switch (parameters.Keys.FirstOrDefault())
            {
                case "partnerModel":
                    PartnerModel partner = parameters.GetValue<PartnerModel>("partnerModel");
                    PartnerCollection.Add(new PartnerModel
                    {
                        Address = partner.Address,
                        Address1 = partner.Address1,
                        City = partner.City,
                        CompanyNo = partner.CompanyNo,
                        Country = partner.Country,
                        Distance = partner.Distance,
                        FullName = partner.FullName,
                        IsActive = partner.IsActive,
                        IsInternal = partner.IsInternal,
                        IsShared = partner.IsShared,
                        Lat = partner.Lat,
                        LocationCode = partner.LocationCode,
                        LocationStatus = partner.LocationStatus,
                        Lon = partner.Lon,
                        MasterCompanyId = partner.MasterCompanyId,
                        ParentPartnerId = partner.ParentPartnerId,
                        ParentPartnerName = partner.ParentPartnerName,
                        PartnerId = partner.PartnerId,
                        PartnershipIsActive = partner.PartnershipIsActive,
                        PartnerTypeCode = partner.PartnerTypeCode,
                        PartnerTypeName = partner.PartnerTypeName,
                        PhoneNumber = partner.PhoneNumber,
                        PostalCode = partner.PostalCode,
                        SourceKey = partner.SourceKey,
                        State = partner.State
                    });
                    break;
                case "BackCommandRecieverAsync":
                    BackCommandRecieverAsync();
                    break;
            }
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            BrewerStockOn = parameters.ContainsKey("BrewerStockOn");
            if (parameters.ContainsKey("GoingFrom"))
            {
                CommingFrom = parameters.GetValue<string>("GoingFrom");
            }
            await LoadPartnersAsync();
        }

        #endregion
    }
}
