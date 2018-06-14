using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchedManifestsListViewModel : BaseViewModel
    {
        #region Properties

        public IMoveService _moveService { get; set; }

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

        public RelayCommand<ManifestSearchResponseModel> ItemTappedCommand { get; }
        public RelayCommand SearchManifestsCommand { get; }

        #endregion

        #region Constructor

        public SearchedManifestsListViewModel(IMoveService moveService)
        {
            _moveService = moveService;

            ItemTappedCommand = new RelayCommand<ManifestSearchResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            SearchManifestsCommand = new RelayCommand(SearchManifestsCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void SearchManifestsCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
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

                var manifest = await _moveService.GetManifestAsync(AppSettings.User.SessionId, model.ManifestId);
                if (manifest.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().AssignInitialValue(manifest,string.Empty);

                    Loader.StopLoading();
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView(), animated: false);
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

        #endregion
    }
}
