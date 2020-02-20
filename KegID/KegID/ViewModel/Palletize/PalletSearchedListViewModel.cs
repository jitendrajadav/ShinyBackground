using Acr.UserDialogs;
using KegID.Common;
using KegID.Model;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class PalletSearchedListViewModel : BaseViewModel
    {
        #region Properties

        public IList<SearchPalletResponseModel> PalletSearchCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand BackCommand { get; }
        public DelegateCommand<SearchPalletResponseModel> ItemTappedCommand { get; }

        #endregion

        #region Contructor

        public PalletSearchedListViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<SearchPalletResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void BackCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
        }

        internal async Task GetPalletSearchAsync(string fromDate, string toDate, string kegs, string kegOwnerId)
        {
            UserDialogs.Instance.ShowLoading("Loading");
            //needs to assing partnerId??string.Empty once backend is ready...
            var response = await ApiManager.GetPalletSearch(Settings.SessionId, string.Empty, fromDate, toDate, kegs, kegOwnerId);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<SearchPalletResponseModel>>(json, GetJsonSetting()));

                PalletSearchCollection = data;
            }

            UserDialogs.Instance.HideLoading();
        }

        private async void ItemTappedCommandRecieverAsync(SearchPalletResponseModel model)
        {
            await NavigationService.NavigateAsync("PalletizeDetailView", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GetPalletSearchAsync"))
            {
                await RunSafe(GetPalletSearchAsync(parameters.GetValue<string>("FromDate"), parameters.GetValue<string>("ToDate"), string.Empty, string.Empty));
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
