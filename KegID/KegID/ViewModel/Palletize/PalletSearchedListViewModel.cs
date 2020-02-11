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
            try
            {
                await _navigationService.GoBackAsync(animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async Task GetPalletSearchAsync(string partnerId, string fromDate, string toDate, string kegs, string kegOwnerId)
        {
            try
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
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
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

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("GetPalletSearchAsync"))
            {
                await RunSafe(GetPalletSearchAsync(parameters.GetValue<string>("GetPalletSearchAsync"), parameters.GetValue<string>("FromDate"), parameters.GetValue<string>("ToDate"), string.Empty, string.Empty));
            }

            //return base.InitializeAsync(parameters);
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
