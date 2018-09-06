using KegID.Model;
using KegID.Services;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AppCenter.Crashes;
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

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IFillService _fillService;

        #region BatchCollection

        /// <summary>
        /// The <see cref="BatchCollection" /> property's name.
        /// </summary>
        public const string BatchCollectionPropertyName = "BatchCollection";

        private IList<NewBatch> _BatchCollection = null;

        /// <summary>
        /// Sets and gets the BatchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<NewBatch> BatchCollection
        {
            get
            {
                return _BatchCollection;
            }

            set
            {
                if (_BatchCollection == value)
                {
                    return;
                }

                _BatchCollection = value;
                RaisePropertyChanged(BatchCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand<NewBatch> ItemTappedCommand { get;}
        public DelegateCommand AddBatchCommand { get; }

        #endregion

        #region Constructor

        public BatchViewModel(IFillService fillService, INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");
            _dialogService = dialogService;
            _fillService = fillService;
            ItemTappedCommand = new DelegateCommand<NewBatch>((model) => ItemTappedCommandRecieverAsync(model));
            AddBatchCommand = new DelegateCommand(AddBatchCommandRecieverAsync);

            LoadBatchAsync();
        }

        #endregion

        #region Methods

        private async void AddBatchCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("AddBatchView", UriKind.Relative), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(NewBatch model)
        {
            try
            {
                if (model != null)
                {
                    await _navigationService.GoBackAsync(new NavigationParameters
                    {
                        { "BatchModel", model }
                    }, useModalNavigation: true, animated: false);
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error", "Error: Please select batch.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public void LoadBatchAsync()
        {
            try
            {
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                BatchCollection = RealmDb.All<NewBatch>().ToList();
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("NewBatchModel"))
            {
                await _navigationService.GoBackAsync(parameters, useModalNavigation: true, animated: false);
            }
            if (parameters.ContainsKey("ItemTappedCommandRecieverAsync"))
            {
                ItemTappedCommandRecieverAsync(null);
            }
        }

        #endregion
    }
}
