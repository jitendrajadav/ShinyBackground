using KegID.Model;
using System.Collections.Generic;
using System.Linq;
using Realms;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace KegID.ViewModel
{
    public class BatchViewModel : BaseViewModel
    {
        #region Properties

        private readonly IPageDialogService _dialogService;
        public IList<NewBatch> BatchCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<NewBatch> ItemTappedCommand { get; }
        public DelegateCommand AddBatchCommand { get; }

        #endregion

        #region Constructor

        public BatchViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            ItemTappedCommand = new DelegateCommand<NewBatch>((model) => ItemTappedCommandRecieverAsync(model));
            AddBatchCommand = new DelegateCommand(AddBatchCommandRecieverAsync);

            LoadBatchAsync();
        }

        #endregion

        #region Methods

        private async void AddBatchCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("AddBatchView", animated: false);
        }

        private async void ItemTappedCommandRecieverAsync(NewBatch model)
        {
            if (model != null)
            {
                await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "BatchModel", model }
                    }, animated: false);
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Error", "Error: Please select batch.", "Ok");
            }
        }

        public void LoadBatchAsync()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            BatchCollection = RealmDb.All<NewBatch>().ToList();
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
