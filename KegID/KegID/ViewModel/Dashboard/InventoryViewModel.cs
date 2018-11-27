using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KegID.ViewModel
{
    public class InventoryViewModel : BaseViewModel
    {
        #region Properties

        public int CurrentPage { get; internal set; }
        private readonly INavigationService _navigationService;
        private readonly IDashboardService _dashboardService;
        private bool isNavigated;

        #region StockInventoryCollection

        /// <summary>
        /// The <see cref="StockInventoryCollection" /> property's name.
        /// </summary>
        public const string StockInventoryCollectionPropertyName = "StockInventoryCollection";

        private IList<InventoryResponseModel> _StockInventoryCollection = null;

        /// <summary>
        /// Sets and gets the StockInventoryCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<InventoryResponseModel> StockInventoryCollection
        {
            get
            {
                return _StockInventoryCollection;
            }

            set
            {
                if (_StockInventoryCollection == value)
                {
                    return;
                }

                _StockInventoryCollection = value;
                RaisePropertyChanged(StockInventoryCollectionPropertyName);
            }
        }

        #endregion

        #region EmptyInventoryCollection

        /// <summary>
        /// The <see cref="EmptyInventoryCollection" /> property's name.
        /// </summary>
        public const string EmptyInventoryCollectionPropertyName = "EmptyInventoryCollection";

        private IList<InventoryResponseModel> _EmptyInventoryCollection = null;

        /// <summary>
        /// Sets and gets the EmptyInventoryCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<InventoryResponseModel> EmptyInventoryCollection
        {
            get
            {
                return _EmptyInventoryCollection;
            }

            set
            {
                if (_EmptyInventoryCollection == value)
                {
                    return;
                }

                _EmptyInventoryCollection = value;
                RaisePropertyChanged(EmptyInventoryCollectionPropertyName);
            }
        }

        #endregion

        #region StockTotals

        /// <summary>
        /// The <see cref="StockTotals" /> property's name.
        /// </summary>
        public const string StockTotalsPropertyName = "StockTotals";

        private long _StockTotals = 0;

        /// <summary>
        /// Sets and gets the StockTotals property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public long StockTotals
        {
            get
            {
                return _StockTotals;
            }

            set
            {
                if (_StockTotals == value)
                {
                    return;
                }

                _StockTotals = value;
                RaisePropertyChanged(StockTotalsPropertyName);
            }
        }

        #endregion

        #region EmptyTotals

        /// <summary>
        /// The <see cref="EmptyTotals" /> property's name.
        /// </summary>
        public const string EmptyTotalsPropertyName = "EmptyTotals";

        private long _EmptyTotals = 0;

        /// <summary>
        /// Sets and gets the EmptyTotals property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public long EmptyTotals
        {
            get
            {
                return _EmptyTotals;
            }

            set
            {
                if (_EmptyTotals == value)
                {
                    return;
                }

                _EmptyTotals = value;
                RaisePropertyChanged(EmptyTotalsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public DelegateCommand HomeCommand { get; }

        #endregion

        #region Constructor

        public InventoryViewModel(IDashboardService dashboardService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _dashboardService = dashboardService;
            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void HomeCommandRecieverAsync()
        {
            try
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public async void InventoryCommandRecieverAsync()
        {
            InventoryDetailModel model = null;
            try
            {
                Loader.StartLoading();
                model = await _dashboardService.GetInventoryAsync(AppSettings.SessionId);
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                RealmDb.Write(()=>
                {
                    foreach (var item in model.InventoryResponseModel)
                    {
                        RealmDb.Add(item);
                    }
                });

                StockInventoryCollection = model.InventoryResponseModel.Where(x => x.Status != "Empty").ToList();
                EmptyInventoryCollection = model.InventoryResponseModel.Where(x => x.Status == "Empty").ToList();

                StockTotals = StockInventoryCollection.Sum(x => x.Quantity);
                EmptyTotals = EmptyInventoryCollection.Sum(x => x.Quantity);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                model = null;
                Loader.StopLoading();
            }
        }

        internal void InitialAssignValueAsync(int currentPage)
        {
            try
            {
                CurrentPage = currentPage;
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var model = RealmDb.All<InventoryResponseModel>().ToList();
                if (model.Count > 0)
                {
                    StockInventoryCollection = model.Where(x => x.Status != "Empty").ToList();
                    EmptyInventoryCollection = model.Where(x => x.Status == "Empty").ToList();

                    StockTotals = StockInventoryCollection.Sum(x => x.Quantity);
                    EmptyTotals = EmptyInventoryCollection.Sum(x => x.Quantity);
                }
                else
                {
                    InventoryCommandRecieverAsync();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (!isNavigated)
            {
                if (parameters.ContainsKey("currentPage"))
                {
                    InitialAssignValueAsync(parameters.GetValue<int>("currentPage"));
                    isNavigated = true;
                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            isNavigated = false;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        #endregion
    }
}
