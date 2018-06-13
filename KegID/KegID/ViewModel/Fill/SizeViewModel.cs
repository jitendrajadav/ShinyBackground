using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SizeViewModel : BaseViewModel
    {
        #region Properties

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

        public RelayCommand<string> ItemTappedCommand { get;}

        #endregion

        #region Constructor

        public SizeViewModel()
        {
            //SizeCollection = new List<string>() { "1/2 bbl", "1/4 bbl", "1/6 bbl", "30 L", "40 L", "50 L" };
            ItemTappedCommand = new RelayCommand<string>((model) => ItemTappedCommandRecieverAsync(model));
            LoadAssetSizeAsync();
        }

        #endregion

        #region Methods

        private void LoadAssetSizeAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance();
                var value = RealmDb.All<AssetSizeModel>().ToList(); ;//await SQLiteServiceClient.Db.Table<AssetSizeModel>().ToListAsync();
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
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack[Application.Current.MainPage.Navigation.ModalStack.Count - 2].GetType().Name))
                {
                    case ViewTypeEnum.FillView:
                        SimpleIoc.Default.GetInstance<FillViewModel>().SizeButtonTitle = model;
                        break;
                    case ViewTypeEnum.EditKegView:
                        SimpleIoc.Default.GetInstance<EditKegViewModel>().Size = model;
                        break;
                    default:
                        break;
                }
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
