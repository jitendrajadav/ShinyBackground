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
    public class SearchedManifestsListViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;

        #region SearchManifestsCollection

        /// <summary>
        /// The <see cref="SearchManifestsCollection" /> property's name.
        /// </summary>
        public const string SearchManifestsCollectionPropertyName = "SearchManifestsCollection";

        private IList<ManifestSearchResponseModel> _SearchManifestsCollection = null;

        /// <summary>
        /// Sets and gets the SearchManifestsCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<ManifestSearchResponseModel> SearchManifestsCollection
        {
            get
            {
                return _SearchManifestsCollection;
            }

            set
            {
                if (_SearchManifestsCollection == value)
                {
                    return;
                }

                _SearchManifestsCollection = value;
                RaisePropertyChanged(SearchManifestsCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand<ManifestSearchResponseModel> ItemTappedCommand { get; }
        public DelegateCommand SearchManifestsCommand { get; }

        #endregion

        #region Constructor

        public SearchedManifestsListViewModel(IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

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
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
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
                    await _navigationService.NavigateAsync(new Uri("ManifestDetailView", UriKind.Relative), new NavigationParameters
                    {
                        { "manifest", manifest }
                    }, useModalNavigation: true, animated: false);
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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("SearchManifestsCollection"))
            {
                SearchManifestsCollection = parameters.GetValue<IList<ManifestSearchResponseModel>>("SearchManifestsCollection");
            }
        }

        #endregion
    }
}
