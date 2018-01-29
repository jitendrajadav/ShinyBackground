using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Response;
using KegID.SQLiteClient;
using KegID.View;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class SearchedManifestsListViewModel : ViewModelBase
    {
        #region Properties

        #region SearchManifestsCollection

        /// <summary>
        /// The <see cref="SearchManifestsCollection" /> property's name.
        /// </summary>
        public const string SearchManifestsCollectionPropertyName = "SearchManifestsCollection";

        private IList<ManifestModel> _SearchManifestsCollection = null;

        /// <summary>
        /// Sets and gets the SearchManifestsCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<ManifestModel> SearchManifestsCollection
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

        public RelayCommand<ManifestModel> ItemTappedCommand { get; set; }

        #endregion

        #region Constructor

        public SearchedManifestsListViewModel()
        {
            ItemTappedCommand = new RelayCommand<ManifestModel>((model) => ItemTappedCommandRecieverAsync(model));
            LoadDraftManifestAsync();
        }

        #endregion

        #region Methods
        private async void LoadDraftManifestAsync()
        {
            try
            {
                Loader.StartLoading();
                SearchManifestsCollection = await SQLiteServiceClient.Db.Table<ManifestModel>().ToListAsync();
            }
            catch (System.Exception)
            {
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        private async void ItemTappedCommandRecieverAsync(ManifestModel model)
        {
            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().TrackingNumber = model.ManifestId;
            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ManifestTo = model.DestinationName;
            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ShippingDate = model.ShipDate;
            SimpleIoc.Default.GetInstance<ManifestDetailViewModel>().ItemCount = model.Tags.Count;

            await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
        }
        #endregion

    }
}
