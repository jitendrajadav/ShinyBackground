using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.SQLiteClient;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class InventoryViewModel : BaseViewModel
    {
        #region Properties
        public int CurrentPage { get; internal set; }

        public IDashboardService _dashboardService { get; set; }

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

        #region EnptyInventoryCollection

        /// <summary>
        /// The <see cref="EnptyInventoryCollection" /> property's name.
        /// </summary>
        public const string EnptyInventoryCollectionPropertyName = "EnptyInventoryCollection";

        private IList<InventoryResponseModel> _EnptyInventoryCollection = null;

        /// <summary>
        /// Sets and gets the EnptyInventoryCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<InventoryResponseModel> EnptyInventoryCollection
        {
            get
            {
                return _EnptyInventoryCollection;
            }

            set
            {
                if (_EnptyInventoryCollection == value)
                {
                    return;
                }

                _EnptyInventoryCollection = value;
                RaisePropertyChanged(EnptyInventoryCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands
        public RelayCommand HomeCommand { get; }

        #endregion

        #region Constructor

        public InventoryViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            HomeCommand = new RelayCommand(HomeCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void HomeCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
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
                model = await _dashboardService.GetInventoryAsync(AppSettings.User.SessionId);
                await SQLiteServiceClient.Db.InsertAllAsync(model.InventoryResponseModel);

                StockInventoryCollection = model.InventoryResponseModel.Where(x => x.Status != "Empty").ToList();
                EnptyInventoryCollection = model.InventoryResponseModel.Where(x => x.Status == "Empty").ToList();
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

        internal async void InitialAssignValueAsync(int currentPage)
        {
            try
            {
                CurrentPage = currentPage;
                var model = await SQLiteServiceClient.Db.Table<InventoryResponseModel>().ToListAsync();
                if (model.Count > 0)
                {
                    StockInventoryCollection = model.Where(x => x.Status != "Empty").ToList();
                    EnptyInventoryCollection = model.Where(x => x.Status == "Empty").ToList();
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
        #endregion
    }
}
