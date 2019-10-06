using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;

namespace KegID.ViewModel
{
    public class ContentTagsViewModel : BaseViewModel
    {
        #region Properties

        public IList<string> ContentCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand ManifestCommand { get; }

        #endregion

        #region Constructor

        public ContentTagsViewModel(INavigationService navigationService) : base(navigationService)
        {
            ManifestCommand = new DelegateCommand(ManifestCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void ManifestCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
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
