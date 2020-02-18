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
    public class SizeViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        public IList<string> SizeCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<string> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public SizeViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            ItemTappedCommand = new DelegateCommand<string>((model) => ItemTappedCommandRecieverAsync(model));
            LoadAssetSizeAsync();
        }

        #endregion

        #region Methods

        private void LoadAssetSizeAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var value = RealmDb.All<AssetSizeModel>().ToList();
            SizeCollection = value.Select(x => x.AssetSize).ToList();
        }

        private async void ItemTappedCommandRecieverAsync(string model)
        {
            if (!string.IsNullOrEmpty(model))
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "SizeModel", model }
                    });
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Error: Please select size.", "Ok");
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ItemTappedCommandRecieverAsync"))
            {
                ItemTappedCommandRecieverAsync(default);
            }
        }

        #endregion
    }
}
