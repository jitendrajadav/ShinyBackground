using Acr.UserDialogs;
using KegID.Common;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class KegSearchedListViewModel : BaseViewModel
    {
        #region Properties

        public IList<KegSearchResponseModel> KegSearchCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<KegSearchResponseModel> ItemTappedCommand { get; }
        public DelegateCommand KegSearchCommand { get; }

        #endregion

        #region Contructor

        public KegSearchedListViewModel(INavigationService navigationService) : base(navigationService)
        {
            ItemTappedCommand = new DelegateCommand<KegSearchResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            KegSearchCommand = new DelegateCommand(KegSearchCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void KegSearchCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void ItemTappedCommandRecieverAsync(KegSearchResponseModel model)
        {
            await _navigationService.NavigateAsync("KegStatusView", new NavigationParameters
                    {
                        { "KegSearchedKegStatusModel", model }
                    }, animated: false);
        }

        internal async Task LoadKegSearchAsync(string barcode)
        {
            UserDialogs.Instance.ShowLoading("Loading");
            var response = await ApiManager.GetKegSearch(Settings.SessionId, barcode, true);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<KegSearchResponseModel>>(json, GetJsonSetting()));

                KegSearchCollection = data;
            }
            UserDialogs.Instance.HideLoading();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("KegSearchCommandRecieverAsync"))
            {
                KegSearchCommandRecieverAsync();
            }
            if (parameters.ContainsKey("LoadKegSearchAsync"))
            {
                await RunSafe(LoadKegSearchAsync(parameters.GetValue<string>("LoadKegSearchAsync")));
            }
        }

        #endregion
    }
}
