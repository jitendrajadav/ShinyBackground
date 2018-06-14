using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.LocalDb;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Realms;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class VolumeViewModel : BaseViewModel
    {
        #region Properties

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

        public RelayCommand<string> ItemTappedCommand { get; }
        
        #endregion

        #region Constructor

        public VolumeViewModel()
        {
            ItemTappedCommand = new RelayCommand<string>((model)=>ItemTappedCommandRecieverAsync(model));
            //VolumeCollection = new List<string>() { "bbl", "hl", "gal" };
            LoadAssetVolumeAsync();
        }

        #endregion

        #region Methods
        private void LoadAssetVolumeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                var value = RealmDb.All<AssetVolumeModel>().ToList();//await SQLiteServiceClient.Db.Table<AssetVolumeModel>().ToListAsync();
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
                SimpleIoc.Default.GetInstance<AddBatchViewModel>().VolumeChar = model;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
