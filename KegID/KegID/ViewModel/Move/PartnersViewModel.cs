﻿using System.Collections.ObjectModel;
using KegID.Services;
using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System;
using Realms;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using KegID.Common;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Acr.UserDialogs;
using KegID.Delegates;
using Shiny.Locations;
using Shiny;

namespace KegID.ViewModel
{
    public class PartnersViewModel : BaseViewModel, IDestructible
    {
        #region Properties

        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;

        public bool BrewerStockOn { get; set; }
        public bool IsWorking { get; set; }
        public ObservableCollection<PartnerModel> PartnerCollection { get; set; }
        public string PartnerName { get; set; }
        public string CommingFrom { get; set; }
        public int? SelectedSegment { get; private set; }

        public IList<PartnerModel> AllPartners { get; set; }
        public Position LocationMessage { get; set; }
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

        public PartnersViewModel(INavigationService navigationService, IGpsManager gpsManager, IGpsListener gpsListener) : base(navigationService)
        {
            _gpsManager = gpsManager;
            _gpsListener = gpsListener;
            _gpsListener.OnReadingReceived += OnReadingReceived;

            ItemTappedCommand = new DelegateCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
            SearchPartnerCommand = new DelegateCommand(SearchPartnerCommandRecieverAsync);
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            SelectedSegmentCommand = new DelegateCommand<object>((seg) => SelectedSegmentCommandReciever(seg));
        }

        #endregion

        #region Methods

        void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            LocationMessage = e.Reading.Position;
        }

        private void TextChangedCommandRecieverAsync()
        {
            if (!string.IsNullOrEmpty(PartnerName))
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
            else
            {
                SelectedSegmentCommandReciever(SelectedSegment);
            }
        }

        private void SelectedSegmentCommandReciever(object seg)
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
                            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                            using (var trans = RealmDb.BeginWrite())
                            {
                                foreach (var item in AllPartners)
                                {
                                    item.Distance = Xamarin.Essentials.Location.CalculateDistance(new Xamarin.Essentials.Location(LocationMessage.Latitude, LocationMessage.Latitude), item.Lat, item.Lon, DistanceUnits.Miles);
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
            if (model != null)
            {
                ConstantManager.Partner = model;
                await NavigationService.GoBackAsync(new NavigationParameters
                    {
                        { "model", model },{ "CommingFrom", CommingFrom }
                    }, animated: false);

                Cleanup();
            }
        }

        public async Task LoadPartnersAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            AllPartners = RealmDb.All<PartnerModel>().Where(x => x.PartnerId != Settings.CompanyId).ToList();

            if (AllPartners.Count <= 0)
            {
                DeletePartners();
                await RunSafe(LoadMetaDataPartnersAsync());
                await LoadPartnersAsync();
            }

            UserDialogs.Instance.HideLoading();
        }

        public async Task LoadMetaDataPartnersAsync()
        {
            Realm RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var response = await ApiManager.GetPartnersList(Settings.SessionId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                IList<PartnerModel> data = await Task.Run(() => JsonConvert.DeserializeObject<IList<PartnerModel>>(json, GetJsonSetting()));
                if (BrewerStockOn)
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners.Where(x => x.PartnerTypeName == "Brewer - Stock").ToList());
                else
                    PartnerCollection = new ObservableCollection<PartnerModel>(AllPartners);
            }
        }

        private void DeletePartners()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            using (var trans = RealmDb.BeginWrite())
            {
                RealmDb.RemoveAll<PartnerModel>();
                trans.Commit();
            }
        }

        private async void BackCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
            Cleanup();
        }

        private async void AddNewPartnerCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("AddPartnerView", animated: false);
        }

        private async void SearchPartnerCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("SearchPartnersView", animated: false);
        }

        public void Cleanup()
        {
            BrewerStockOn = false;
            PartnerCollection = null;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
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

            if (_gpsManager.IsListening)
            {
                await _gpsManager.StopListener();
            }

            await _gpsManager.StartListener(new GpsRequest
            {
                UseBackground = true,
                Priority = GpsPriority.Highest,
                Interval = TimeSpan.FromSeconds(5),
                ThrottledInterval = TimeSpan.FromSeconds(3) //Should be lower than Interval
            });
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

        public void Destroy()
        {
            _gpsListener.OnReadingReceived -= OnReadingReceived;
        }

        #endregion
    }
}
