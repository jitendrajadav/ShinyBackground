using KegID.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AppCenter.Crashes;
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

        public DelegateCommand<Sku> ItemTappedCommand { get;}
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
            try
            {
                await _navigationService.NavigateAsync("AddBatchView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(Sku model)
        {
            try
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                SKUCollection = RealmDb.All<Sku>().ToList();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
