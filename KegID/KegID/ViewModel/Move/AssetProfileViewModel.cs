using KegID.Services;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class AssetProfileViewModel : BaseViewModel
    {
        #region Properties

        public string SelectedOwner { get; set; }

        #endregion

        #region Commands

        public DelegateCommand ApplyToAllCommand { get; }
        public DelegateCommand DoneCommand { get; }

        #endregion

        #region Constructor

        public AssetProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void DoneCommandRecieverAsync()
        {
                await NavigationService.GoBackAsync(new NavigationParameters
                        {
                            { "AssignSizesValue", ConstantManager.VerifiedBarcodes }
                        }, animated: false);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        #endregion

    }
}
