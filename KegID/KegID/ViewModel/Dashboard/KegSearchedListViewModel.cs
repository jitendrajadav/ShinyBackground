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
    public class KegSearchedListViewModel : BaseViewModel
    {
        #region Properties

        private readonly IDashboardService _dashboardService;
        private readonly INavigationService _navigationService;

        #region KegSearchCollection

        /// <summary>
        /// The <see cref="KegSearchCollection" /> property's name.
        /// </summary>
        public const string KegSearchCollectionPropertyName = "KegSearchCollection";

        private IList<KegSearchResponseModel> _KegSearchCollection = null;

        /// <summary>
        /// Sets and gets the KegSearchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<KegSearchResponseModel> KegSearchCollection
        {
            get
            {
                return _KegSearchCollection;
            }

            set
            {
                if (_KegSearchCollection == value)
                {
                    return;
                }

                _KegSearchCollection = value;
                RaisePropertyChanged(KegSearchCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand<KegSearchResponseModel> ItemTappedCommand { get; }
        public DelegateCommand KegSearchCommand { get; }

        #endregion

        #region Contructor

        public KegSearchedListViewModel(IDashboardService dashboardService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _dashboardService = dashboardService;
            ItemTappedCommand = new DelegateCommand<KegSearchResponseModel>( (model) => ItemTappedCommandRecieverAsync(model));
            KegSearchCommand = new DelegateCommand(KegSearchCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void KegSearchCommandRecieverAsync()
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

        private async void ItemTappedCommandRecieverAsync(KegSearchResponseModel model)
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("KegStatusView", UriKind.Relative), new NavigationParameters
                    {
                        { "model", model }
                    }, useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async void LoadKegSearchAsync(string barcode)
        {
            try
            {
                var value = await _dashboardService.GetKegSearchAsync(AppSettings.User.SessionId, barcode, true);
                KegSearchCollection = value.KegSearchResponseModel;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        #endregion
    }
}
