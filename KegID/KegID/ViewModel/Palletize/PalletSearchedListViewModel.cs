using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace KegID.ViewModel
{
    public class PalletSearchedListViewModel : BaseViewModel
    {
        #region Properties

        private readonly IDashboardService _dashboardService;
        public IList<SearchPalletResponseModel> PalletSearchCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand BackCommand { get; }
        public DelegateCommand<SearchPalletResponseModel> ItemTappedCommand { get;}

        #endregion

        #region Contructor

        public PalletSearchedListViewModel(IDashboardService dashboardService, INavigationService navigationService) : base(navigationService)
        {
            _dashboardService = dashboardService;

            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<SearchPalletResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void BackCommandRecieverAsync()
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

        internal async void GetPalletSearchAsync(string partnerId,string fromDate, string toDate,string kegs,string kegOwnerId)
        {
            try
            {
                Loader.StartLoading();
                //needs to assing partnerId??string.Empty once backend is ready...
                var value = await _dashboardService.GetPalletSearchAsync(AppSettings.SessionId, string.Empty, fromDate, toDate, kegs, kegOwnerId);
                PalletSearchCollection = value.SearchPalletResponseModel;
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

        private async void ItemTappedCommandRecieverAsync(SearchPalletResponseModel model)
        {
            try
            {
                await _navigationService.NavigateAsync("PalletizeDetailView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GetPalletSearchAsync"))
            {
               GetPalletSearchAsync(parameters.GetValue<string>("GetPalletSearchAsync"), parameters.GetValue<string>("FromDate"), parameters.GetValue<string>("ToDate"),string.Empty,string.Empty);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }
        }

        #endregion
    }
}
