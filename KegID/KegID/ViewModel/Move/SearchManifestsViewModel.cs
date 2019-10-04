using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Globalization;

namespace KegID.ViewModel
{
    public class SearchManifestsViewModel : BaseViewModel
    {
        #region Properties

        private readonly IMoveService _moveService;
        public bool IsManifestDestination { get; set; }
        public string TrackingNumber { get; set; }
        public string Barcode { get; set; }
        public string ManifestSender { get; set; }
        public string ManifestDestination { get; set; }
        public string Referencekey { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;

        #endregion

        #region Commands

        public DelegateCommand ManifestsCommand { get; }
        public DelegateCommand ManifestSenderCommand { get; }
        public DelegateCommand ManifestDestinationCommand { get; }
        public DelegateCommand SearchCommand { get; }

        #endregion

        #region Constructor

        public SearchManifestsViewModel(IMoveService moveService, INavigationService navigationService) : base(navigationService)
        {
            _moveService = moveService;

            ManifestsCommand = new DelegateCommand(ManifestsCommandRecieverAsync);
            ManifestSenderCommand = new DelegateCommand(ManifestSenderCommandRecieverAsync);
            ManifestDestinationCommand = new DelegateCommand(ManifestDestinationCommandRecieverAsync);
            SearchCommand = new DelegateCommand(SearchCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SearchCommandRecieverAsync()
        {
            try
            {
                var value = await _moveService.GetManifestSearchAsync(AppSettings.SessionId, TrackingNumber, Barcode, ManifestSender, ManifestDestination, Referencekey, FromDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")), ToDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")));
                await _navigationService.NavigateAsync("SearchedManifestsListView", new NavigationParameters
                    {
                        { "SearchManifestsCollection", value.ManifestSearchResponseModel }
                    }, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestDestinationCommandRecieverAsync()
        {
            try
            {
                IsManifestDestination = true;
                await _navigationService.NavigateAsync("PartnersView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ManifestSenderCommandRecieverAsync()
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

        private async void ManifestsCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        internal void AssignPartnerValue(PartnerModel model)
        {
            try
            {
                if (IsManifestDestination)
                {
                    IsManifestDestination = false;
                    ManifestDestination = model.FullName;
                }
                else
                    ManifestSender = model.FullName;
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
                AssignPartnerValue(parameters.GetValue<PartnerModel>("model"));
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ManifestsCommandRecieverAsync"))
            {
                ManifestsCommandRecieverAsync();
            }
        }

        #endregion
    }
}
