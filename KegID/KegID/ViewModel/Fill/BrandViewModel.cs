using KegID.LocalDb;
using KegID.Model;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System.Collections.Generic;
using System.Linq;

namespace KegID.ViewModel
{
    public class BrandViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        public IList<BrandModel> BrandCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<BrandModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public BrandViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            ItemTappedCommand = new DelegateCommand<BrandModel>((model) => ItemTappedCommandRecieverAsync(model));
            LoadBrand();
        }

        #endregion

        #region Methods

        private void LoadBrand()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            BrandCollection = RealmDb.All<BrandModel>().ToList();
        }

        private async void ItemTappedCommandRecieverAsync(BrandModel model)
        {
            if (model != null)
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "BrandModel", model }
                    }, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Warning!", "Warning! Please select brand.", "Ok");
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ItemTappedCommandRecieverAsync"))
            {
                ItemTappedCommandRecieverAsync(null);
            }
        }

        #endregion
    }
}
