﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchedManifestsListViewModel : ViewModelBase
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

        public RelayCommand<ManifestSearchResponseModel> ItemTappedCommand { get; set; }
        public RelayCommand SearchManifestsCommand { get; set; }

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
          await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void ItemTappedCommandRecieverAsync(ManifestSearchResponseModel model)
        {
            try
            {
                Loader.StartLoading();
                var manifest = await _moveService.GetManifestAsync(Configuration.SessionId, model.ManifestId);
                if (manifest.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().TrackingNumber = manifest.TrackingNumber;

                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ManifestTo = manifest.CreatorCompany.FullName + "\n" + manifest.CreatorCompany.PartnerTypeName;

                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ShippingDate = manifest.ShipDate;
                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ItemCount = manifest.ManifestItems.Count;
                    SimpleIoc.Default.GetInstance<ContentTagsViewModel>().ContentCollection = manifest.ManifestItems.Select(x=>x.Barcode).ToList();

                    SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().Contents = !string.IsNullOrEmpty(manifest.ManifestItems.FirstOrDefault().Contents) ? manifest.ManifestItems.FirstOrDefault().Contents : "No contens";

                    Loader.StopLoading();
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
            }
        }
        #endregion

    }
}
