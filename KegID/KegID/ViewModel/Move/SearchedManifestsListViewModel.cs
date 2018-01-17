using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private IList<string> _SearchManifestsCollection = null;

        /// <summary>
        /// Sets and gets the SearchManifestsCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> SearchManifestsCollection
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

        public RelayCommand<string> ItemTappedCommand { get; set; }

        #endregion

        #region Constructor

        public SearchedManifestsListViewModel()
        {
            ItemTappedCommand = new RelayCommand<string>((model) => ItemTappedCommandRecieverAsync(model));
        }

        private async void ItemTappedCommandRecieverAsync(string model)
        {
           await Application.Current.MainPage.Navigation.PushModalAsync(new ManifestDetailView());
        }

        #endregion

        #region Methods

        #endregion

    }
}
