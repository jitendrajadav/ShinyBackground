using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            await NavigationService.GoBackAsync(animated: false);
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Barcode"))
            {
                ContentCollection = parameters.GetValue<List<string>>("Barcode");
            }

            return base.InitializeAsync(parameters);
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
