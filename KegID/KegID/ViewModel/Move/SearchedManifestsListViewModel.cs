using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
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

        private readonly IMoveService _moveService;
        public IList<ManifestSearchResponseModel> SearchManifestsCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<ManifestSearchResponseModel> ItemTappedCommand { get; }
        public DelegateCommand SearchManifestsCommand { get; }

        #endregion

        #region Constructor

        public SearchedManifestsListViewModel(IMoveService moveService, INavigationService navigationService) : base(navigationService)
        {
            _moveService = moveService;
            ItemTappedCommand = new DelegateCommand<ManifestSearchResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
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

        private async void ItemTappedCommandRecieverAsync(ManifestSearchResponseModel model)
        {
            try
            {
                Loader.StartLoading();

                var manifest = await _moveService.GetManifestAsync(AppSettings.SessionId, model.ManifestId);
                if (manifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    Loader.StopLoading();
                    await _navigationService.NavigateAsync("ManifestDetailView", new NavigationParameters
                    {
                        { "manifest", manifest }
                    }, animated: false);
                }
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
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
