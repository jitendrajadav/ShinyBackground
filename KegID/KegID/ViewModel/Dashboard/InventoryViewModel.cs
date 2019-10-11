﻿using KegID.Common;
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
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class InventoryViewModel : BaseViewModel
    {
        #region Properties

        public int CurrentPage { get; internal set; }
        private readonly IDashboardService _dashboardService;
        private bool isNavigated;
        public IList<InventoryResponseModel> StockInventoryCollection { get; set; }
        public IList<InventoryResponseModel> EmptyInventoryCollection { get; set; }
        public long StockTotals { get; set; }
        public long EmptyTotals { get; set; }

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }

        #endregion

        #region Constructor

        public InventoryViewModel(IDashboardService dashboardService, INavigationService navigationService) : base(navigationService)
        {
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

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (!isNavigated)
            {
                if (parameters.ContainsKey("currentPage"))
                {
                    InitialAssignValueAsync(parameters.GetValue<int>("currentPage"));
                    isNavigated = true;
                }
            }

            return base.InitializeAsync(parameters);
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
