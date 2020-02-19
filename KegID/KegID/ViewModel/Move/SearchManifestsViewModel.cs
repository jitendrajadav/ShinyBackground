using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class SearchManifestsViewModel : BaseViewModel
    {
        #region Properties

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

        public SearchManifestsViewModel(INavigationService navigationService) : base(navigationService)
        {
            ManifestsCommand = new DelegateCommand(ManifestsCommandRecieverAsync);
            ManifestSenderCommand = new DelegateCommand(ManifestSenderCommandRecieverAsync);
            ManifestDestinationCommand = new DelegateCommand(ManifestDestinationCommandRecieverAsync);
            SearchCommand = new DelegateCommand(async () => await RunSafe(SearchCommandRecieverAsync()));
        }

        #endregion

        #region Methods

        private async Task SearchCommandRecieverAsync()
        {
            var response = await ApiManager.GetManifestSearch(Settings.SessionId, TrackingNumber, Barcode, ManifestSender, ManifestDestination, Referencekey, FromDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")), ToDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US")));
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<ManifestSearchResponseModel>>(json, GetJsonSetting()));

                await NavigationService.NavigateAsync("SearchedManifestsListView", new NavigationParameters
                    {
                        { "SearchManifestsCollection", data }
                    }, animated: false);
            }
        }

        private async void ManifestDestinationCommandRecieverAsync()
        {
            IsManifestDestination = true;
            await NavigationService.NavigateAsync("PartnersView", animated: false);
        }

        private async void ManifestSenderCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("PartnersView", animated: false);
        }

        private async void ManifestsCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
        }

        internal void AssignPartnerValue(PartnerModel model)
        {
            if (IsManifestDestination)
            {
                IsManifestDestination = false;
                ManifestDestination = model.FullName;
            }
            else
                ManifestSender = model.FullName;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                AssignPartnerValue(parameters.GetValue<PartnerModel>("model"));
            }
            return base.InitializeAsync(parameters);
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
