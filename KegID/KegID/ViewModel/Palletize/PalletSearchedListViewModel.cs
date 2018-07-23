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

        private readonly INavigationService _navigationService;
        private readonly IDashboardService _dashboardService;

        #region PalletSearchCollection

        /// <summary>
        /// The <see cref="PalletSearchCollection" /> property's name.
        /// </summary>
        public const string PalletSearchCollectionPropertyName = "PalletSearchCollection";

        private IList<SearchPalletResponseModel> _PalletSearchCollection = null;

        /// <summary>
        /// Sets and gets the PalletSearchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<SearchPalletResponseModel> PalletSearchCollection
        {
            get
            {
                return _PalletSearchCollection;
            }

            set
            {
                if (_PalletSearchCollection == value)
                {
                    return;
                }

                _PalletSearchCollection = value;
                RaisePropertyChanged(PalletSearchCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand BackCommand { get; }
        public DelegateCommand<SearchPalletResponseModel> ItemTappedCommand { get;}

        #endregion

        #region Contructor

        public PalletSearchedListViewModel(IDashboardService dashboardService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

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
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
                //needs to assing partnerId??string.Empty once backend is ready...
                var value = await _dashboardService.GetPalletSearchAsync(AppSettings.User.SessionId, string.Empty, fromDate, toDate, kegs, kegOwnerId);
                PalletSearchCollection = value.SearchPalletResponseModel;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(SearchPalletResponseModel model)
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("PalletizeDetailView", UriKind.Relative), new NavigationParameters
                    {
                        { "model", model }
                    }, useModalNavigation: true, animated: false);
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

        #endregion
    }
}
