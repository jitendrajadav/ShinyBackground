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
    public class SearchedManifestsListViewModel : BaseViewModel
    {
        #region Properties

        public IList<ManifestSearchResponseModel> SearchManifestsCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<ManifestSearchResponseModel> ItemTappedCommand { get; }
        public DelegateCommand SearchManifestsCommand { get; }

        #endregion

        #region Constructor

        public SearchedManifestsListViewModel(INavigationService navigationService) : base(navigationService)
        {
            ItemTappedCommand = new DelegateCommand<ManifestSearchResponseModel>(async (model) => await RunSafe(ItemTappedCommandRecieverAsync(model)));
            SearchManifestsCommand = new DelegateCommand(SearchManifestsCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SearchManifestsCommandRecieverAsync()
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

        private async Task ItemTappedCommandRecieverAsync(ManifestSearchResponseModel model)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading");

                var response = await ApiManager.GetManifest(model.ManifestId, Settings.SessionId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<ManifestResponseModel>(json, GetJsonSetting()));

                    UserDialogs.Instance.HideLoading();
                    await _navigationService.NavigateAsync("ManifestDetailView", new NavigationParameters
                    {
                        { "manifest", data }
                    }, animated: false);
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

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("SearchManifestsCollection"))
            {
                SearchManifestsCollection = parameters.GetValue<IList<ManifestSearchResponseModel>>("SearchManifestsCollection");
            }
            return base.InitializeAsync(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("SearchManifestsCommandRecieverAsync"))
            {
                SearchManifestsCommandRecieverAsync();
            }
        }

        #endregion
    }
}
