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
    public class SizeViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        #region SizeCollection

        /// <summary>
        /// The <see cref="SizeCollection" /> property's name.
        /// </summary>
        public const string SizeCollectionPropertyName = "SizeCollection";

        private IList<string> _SizeCollection = null;

        /// <summary>
        /// Sets and gets the SizeCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> SizeCollection
        {
            get
            {
                return _SizeCollection;
            }

            set
            {
                if (_SizeCollection == value)
                {
                    return;
                }

                _SizeCollection = value;
                RaisePropertyChanged(SizeCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand<string> ItemTappedCommand { get;}

        #endregion

        #region Constructor

        public SizeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            ItemTappedCommand = new DelegateCommand<string>((model) => ItemTappedCommandRecieverAsync(model));
            LoadAssetSizeAsync();
        }

        #endregion

        #region Methods

        private void LoadAssetSizeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<AssetSizeModel>().ToList(); 
                SizeCollection = value.Select(x => x.AssetSize).ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(string model)
        {
            try
            {
                var param = new NavigationParameters
                    {
                        { "SizeModel", model }
                    };
                await _navigationService.GoBackAsync(param, useModalNavigation: true, animated: false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
