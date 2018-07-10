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

namespace KegID.ViewModel
{
    public class BatchViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IFillService _fillService;

        #region BatchCollection

        /// <summary>
        /// The <see cref="BatchCollection" /> property's name.
        /// </summary>
        public const string BatchCollectionPropertyName = "BatchCollection";

        private IList<BatchModel> _BatchCollection = null;

        /// <summary>
        /// Sets and gets the BatchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<BatchModel> BatchCollection
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

        public DelegateCommand<BatchModel> ItemTappedCommand { get;}
        public DelegateCommand AddBatchCommand { get; }

        #endregion

        #region Constructor

        public BatchViewModel(IFillService fillService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _fillService = fillService;
            ItemTappedCommand = new DelegateCommand<BatchModel>((model) => ItemTappedCommandRecieverAsync(model));
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

        private async void ItemTappedCommandRecieverAsync(BatchModel model)
        {
            try
            {
                if (model != null)
                {
                    var param = new NavigationParameters
                    {
                        { "BatchModel", model }
                    };
                    await _navigationService.GoBackAsync(param, useModalNavigation: true, animated: false);

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
                BatchCollection = RealmDb.All<BatchModel>().ToList();
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
