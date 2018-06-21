using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class ContentTagsViewModel : BaseViewModel
    {
        #region Properties

        #region ContentCollection

        /// <summary>
        /// The <see cref="ContentCollection" /> property's name.
        /// </summary>
        public const string ContentCollectionPropertyName = "ContentCollection";

        private IList<string> _ContentCollection = null;

        /// <summary>
        /// Sets and gets the ContentCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<string> ContentCollection
        {
            get
            {
                return _ContentCollection;
            }

            set
            {
                if (_ContentCollection == value)
                {
                    return;
                }

                _ContentCollection = value;
                RaisePropertyChanged(ContentCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand ManifestCommand { get; }

        #endregion

        #region Constructor

        public ContentTagsViewModel()
        {
            ManifestCommand = new RelayCommand(ManifestCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void ManifestCommandRecieverAsync() => await Application.Current.MainPage.Navigation.PopModalAsync();

        #endregion
    }
}
