﻿using System;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class SearchPalletViewModel : BaseViewModel
    {
        #region Properties
        public string PalletBarcode { get; set; }
        public string Barcode { get; set; }
        public string LocationCreated { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now;
        public PartnerModel PartnerModel { get; set; } = new PartnerModel();
        public void OnPartnerModelChanged()
        {
            LocationCreated = PartnerModel.FullName;
        }

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand LocationCreatedCommand { get; }

        #endregion

        #region Contructor

        public SearchPalletViewModel(INavigationService navigationService) : base(navigationService)
        {
            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            SearchCommand = new DelegateCommand(SearchCommandRecieverAsync);
            LocationCreatedCommand = new DelegateCommand(LocationCreatedCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void LocationCreatedCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PartnersView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

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
        private async void SearchCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("PalletSearchedListView", new NavigationParameters
                    {
                        { "GetPalletSearchAsync", PartnerModel?.PartnerId },{ "FromDate", FromDate.ToShortDateString() },{ "ToDate", ToDate.ToShortDateString() }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                PartnerModel = parameters.GetValue<PartnerModel>("model");
            }
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
