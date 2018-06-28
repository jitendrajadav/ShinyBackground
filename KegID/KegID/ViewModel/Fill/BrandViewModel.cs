using KegID.LocalDb;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KegID.ViewModel
{
    public class BrandViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        #region BrandCollection

        /// <summary>
        /// The <see cref="BrandCollection" /> property's name.
        /// </summary>
        public const string BrandCollectionPropertyName = "BrandCollection";

        private IList<BrandModel> _BrandCollection = null;

        /// <summary>
        /// Sets and gets the BrandCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<BrandModel> BrandCollection
        {
            get
            {
                return _BrandCollection;
            }

            set
            {
                if (_BrandCollection == value)
                {
                    return;
                }

                _BrandCollection = value;
                RaisePropertyChanged(BrandCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand<BrandModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public BrandViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            ItemTappedCommand = new DelegateCommand<BrandModel>((model)=>ItemTappedCommandRecieverAsync(model));
            LoadBrand();
        }

        #endregion

        #region Methods

        private void LoadBrand()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var all = RealmDb.All<BrandModel>().ToList();

                BrandCollection = all;
                // SimpleIoc.Default.GetInstance<ScanKegsViewModel>().LoadBrandAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(BrandModel model)
        {
            try
            {
                //SimpleIoc.Default.GetInstance<AddBatchViewModel>().BrandModel = model;
                //await Application.Current.MainPage.Navigation.PopModalAsync();

                var param = new NavigationParameters
                    {
                        { "model", model }
                    };
                await _navigationService.GoBackAsync(param, useModalNavigation: true, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }

        #endregion
    }
}
