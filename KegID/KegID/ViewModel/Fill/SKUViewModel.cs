using KegID.Model;
using System.Collections.Generic;
using System.Linq;
using Realms;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace KegID.ViewModel
{
    public class SKUViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        public IList<Sku> SKUCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<Sku> ItemTappedCommand { get; }
        public DelegateCommand AddBatchCommand { get; }

        #endregion

        #region Constructor

        public SKUViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            ItemTappedCommand = new DelegateCommand<Sku>((model) => ItemTappedCommandRecieverAsync(model));
            AddBatchCommand = new DelegateCommand(AddBatchCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void AddBatchCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("AddBatchView", animated: false);
        }

        private async void ItemTappedCommandRecieverAsync(Sku model)
        {
            if (model != null)
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "SKUModel", model }
                    }, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Error: Please select sku.", "Ok");
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            SKUCollection = RealmDb.All<Sku>().ToList();
            return base.InitializeAsync(parameters);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("NewBatchModel"))
            {
                await _navigationService.GoBackAsync(parameters, animated: false);
            }
            if (parameters.ContainsKey("ItemTappedCommandRecieverAsync"))
            {
                ItemTappedCommandRecieverAsync(null);
            }
        }

        #endregion
    }
}
