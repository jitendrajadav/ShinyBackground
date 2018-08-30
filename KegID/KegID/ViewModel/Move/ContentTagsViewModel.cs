using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace KegID.ViewModel
{
    public class ContentTagsViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

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

        public DelegateCommand ManifestCommand { get; }

        #endregion

        #region Constructor

        public ContentTagsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            ManifestCommand = new DelegateCommand(ManifestCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void ManifestCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Barcode"))
            {
                ContentCollection = parameters.GetValue<List<string>>("Barcode");
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ManifestCommandRecieverAsync"))
            {
                ManifestCommandRecieverAsync();
            }
        }

        #endregion
    }
}
