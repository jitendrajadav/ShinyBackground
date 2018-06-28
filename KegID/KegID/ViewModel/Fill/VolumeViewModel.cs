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
    public class VolumeViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        #region VolumeCollection

        /// <summary>
        /// The <see cref="VolumeCollection" /> property's name.
        /// </summary>
        public const string VolumeCollectionPropertyName = "VolumeCollection";

        private IList<string> _VolumeCollection = null;

        /// <summary>
        /// Sets and gets the VolumeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> VolumeCollection
        {
            get
            {
                return _VolumeCollection;
            }

            set
            {
                if (_VolumeCollection == value)
                {
                    return;
                }

                _VolumeCollection = value;
                RaisePropertyChanged(VolumeCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand<string> ItemTappedCommand { get; }
        
        #endregion

        #region Constructor

        public VolumeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            ItemTappedCommand = new DelegateCommand<string>((model)=>ItemTappedCommandRecieverAsync(model));
            LoadAssetVolumeAsync();
        }

        #endregion

        #region Methods
        private void LoadAssetVolumeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<AssetVolumeModel>().ToList();
                VolumeCollection = value.Select(x => x.AssetVolume).ToList();
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(string model)
        {
            try
            {
                //SimpleIoc.Default.GetInstance<AddBatchViewModel>().VolumeChar = model;
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
