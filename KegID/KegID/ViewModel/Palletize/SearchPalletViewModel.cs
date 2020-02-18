using System;
using KegID.Model;
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
            await _navigationService.NavigateAsync("PartnersView", animated: false);
        }

        private async void HomeCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void SearchCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("PalletSearchedListView", new NavigationParameters
                    {
                        { "GetPalletSearchAsync", PartnerModel?.PartnerId },{ "FromDate", FromDate.Date.ToShortDateString() },{ "ToDate", ToDate.Date.ToShortDateString() }
                    }, animated: false);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("model"))
            {
                PartnerModel = parameters.GetValue<PartnerModel>("model");
            }
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        #endregion
    }
}
