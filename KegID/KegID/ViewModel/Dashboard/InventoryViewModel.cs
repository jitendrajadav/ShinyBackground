using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class InventoryViewModel : BaseViewModel
    {
        #region Properties
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
            InventoryCommandRecieverAsync();
        }

        #endregion

        #region Methods

        private async void HomeCommandRecieverAsync()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void InventoryCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();
                var value = await _dashboardService.GetInventoryAsync(AppSettings.User.SessionId);
                StockInventoryCollection = value.InventoryResponseModel.Where(x => x.Status != "Empty").ToList();
                EnptyInventoryCollection = value.InventoryResponseModel.Where(x => x.Status == "Empty").ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
            }
        }
        #endregion
    }
}
