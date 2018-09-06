using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using KegID.LocalDb;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Realms;

namespace KegID.ViewModel
{
    public class ManifestsViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        private readonly IMoveService _moveService;

        #region ManifestCollection

        /// <summary>
        /// The <see cref="ManifestCollection" /> property's name.
        /// </summary>
        public const string ManifestCollectionPropertyName = "ManifestCollection";

        private ObservableCollection<ManifestModel> _ManifestCollection = new ObservableCollection<ManifestModel>();

        /// <summary>
        /// Sets and gets the ManifestCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ManifestModel> ManifestCollection
        {
            get
            {
                return _ManifestCollection;
            }

            set
            {
                if (_ManifestCollection == value)
                {
                    return;
                }

                _ManifestCollection = value;
                RaisePropertyChanged(ManifestCollectionPropertyName);
            }
        }

        #endregion

        #region QueuedTextColor

        /// <summary>
        /// The <see cref="QueuedTextColor" /> property's name.
        /// </summary>
        public const string QueuedTextColorPropertyName = "QueuedTextColor";

        private string _QueuedTextColor = "#4E6388";

        /// <summary>
        /// Sets and gets the QueuedTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string QueuedTextColor
        {
            get
            {
                return _QueuedTextColor;
            }

            set
            {
                if (_QueuedTextColor == value)
                {
                    return;
                }

                _QueuedTextColor = value;
                RaisePropertyChanged(QueuedTextColorPropertyName);
            }
        }

        #endregion

        #region QueuedBackgroundColor

        /// <summary>
        /// The <see cref="QueuedBackgroundColor" /> property's name.
        /// </summary>
        public const string QueuedBackgroundColorPropertyName = "QueuedBackgroundColor";

        private string _QueuedBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the QueuedBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string QueuedBackgroundColor
        {
            get
            {
                return _QueuedBackgroundColor;
            }

            set
            {
                if (_QueuedBackgroundColor == value)
                {
                    return;
                }

                _QueuedBackgroundColor = value;
                RaisePropertyChanged(QueuedBackgroundColorPropertyName);
            }
        }

        #endregion

        #region DraftTextColor

        /// <summary>
        /// The <see cref="DraftTextColor" /> property's name.
        /// </summary>
        public const string DraftTextColorPropertyName = "DraftTextColor";

        private string _DraftTextColor = "White";

        /// <summary>
        /// Sets and gets the DraftTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DraftTextColor
        {
            get
            {
                return _DraftTextColor;
            }

            set
            {
                if (_DraftTextColor == value)
                {
                    return;
                }

                _DraftTextColor = value;
                RaisePropertyChanged(DraftTextColorPropertyName);
            }
        }

        #endregion

        #region DraftBackgroundColor

        /// <summary>
        /// The <see cref="DraftBackgroundColor" /> property's name.
        /// </summary>
        public const string DraftBackgroundColorPropertyName = "DraftBackgroundColor";

        private string _DraftBackgroundColor = "#4E6388";

        /// <summary>
        /// Sets and gets the DraftBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string DraftBackgroundColor
        {
            get
            {
                return _DraftBackgroundColor;
            }

            set
            {
                if (_DraftBackgroundColor == value)
                {
                    return;
                }

                _DraftBackgroundColor = value;
                RaisePropertyChanged(DraftBackgroundColorPropertyName);
            }
        }

        #endregion

        #region RecentTextColor

        /// <summary>
        /// The <see cref="RecentTextColor" /> property's name.
        /// </summary>
        public const string RecentTextColorPropertyName = "RecentTextColor";

        private string _RecentTextColor = "#4E6388";

        /// <summary>
        /// Sets and gets the RecentTextColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RecentTextColor
        {
            get
            {
                return _RecentTextColor;
            }

            set
            {
                if (_RecentTextColor == value)
                {
                    return;
                }

                _RecentTextColor = value;
                RaisePropertyChanged(RecentTextColorPropertyName);
            }
        }

        #endregion

        #region RecentBackgroundColor

        /// <summary>
        /// The <see cref="RecentBackgroundColor" /> property's name.
        /// </summary>
        public const string RecentBackgroundColorPropertyName = "RecentBackgroundColor";

        private string _RecentBackgroundColor = "Transparent";

        /// <summary>
        /// Sets and gets the RecentBackgroundColor property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string RecentBackgroundColor
        {
            get
            {
                return _RecentBackgroundColor;
            }

            set
            {
                if (_RecentBackgroundColor == value)
                {
                    return;
                }

                _RecentBackgroundColor = value;
                RaisePropertyChanged(RecentBackgroundColorPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand ActionSearchCommand { get; }
        public DelegateCommand QueuedCommand { get; }
        public DelegateCommand DraftCommand { get; }
        public DelegateCommand RecentCommand { get; }
        public DelegateCommand<ManifestModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public ManifestsViewModel(IMoveService moveService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _moveService = moveService;

            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            ActionSearchCommand = new DelegateCommand(ActionSearchCommandRecieverAsync);
            QueuedCommand = new DelegateCommand(QueuedCommandReciever);
            DraftCommand = new DelegateCommand(DraftCommandReciever);
            RecentCommand = new DelegateCommand(RecentCommandReciever);
            ItemTappedCommand = new DelegateCommand<ManifestModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        internal void LoadDraftManifestAsync()
        {
            List<ManifestModel> collection;
            try
            {
                ManifestCollection.Clear();
                Loader.StartLoading();
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                collection = RealmDb.All<ManifestModel>().Where(x => x.IsDraft == true && x.IsQueue == false).ToList();
                AssignColletionToManifest(collection);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
                collection = null;
            }
        }

        private void AssignColletionToManifest(List<ManifestModel> collection)
        {
            using (var db = Realm.GetInstance(RealmDbManager.GetRealmDbConfig()).BeginWrite())
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    ManifestModel item = collection[i];
                    item.SenderId = item.ManifestItemsCount > 1 ? item.ManifestItemsCount + " Items" : item.ManifestItemsCount + " Item";
                    ManifestCollection.Add(item);
                }
                db.Commit();
            }
        }

        private async void ItemTappedCommandRecieverAsync(ManifestModel model)
        {
            try
            {
                switch ((EventTypeEnum)model.EventTypeId)
                {
                    case EventTypeEnum.MOVE_MANIFEST:
                        await _navigationService.NavigateAsync(new Uri("MoveView", UriKind.Relative), new NavigationParameters
                                {
                                    { "AssignInitialValue", model }
                                }, useModalNavigation: true, animated: false);
                        break;
                    case EventTypeEnum.SHIP_MANIFEST:
                        break;
                    case EventTypeEnum.RECEIVE_MANIFEST:
                        break;
                    case EventTypeEnum.FILL_MANIFEST:
                        await _navigationService.NavigateAsync(new Uri("FillView", UriKind.Relative), new NavigationParameters
                                {
                                    { "AssignInitialValue", model }
                                }, useModalNavigation: true, animated: false);
                        break;
                    case EventTypeEnum.PALLETIZE_MANIFEST:
                        break;
                    case EventTypeEnum.RETURN_MANIFEST:
                        break;
                    case EventTypeEnum.REPAIR_MANIFEST:
                        break;
                    case EventTypeEnum.COLLECT_MANIFEST:
                        break;
                    case EventTypeEnum.ARCHIVE_MANIFEST:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void HomeCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync(new Uri("/NavigationPage/MainPage", UriKind.Absolute), useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ActionSearchCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync(new Uri("SearchManifestsView", UriKind.Relative), useModalNavigation: true, animated: false);
        }

        private void QueuedCommandReciever()
        {
            List<ManifestModel> collection;
            try
            {
                ManifestCollection.Clear();
                Loader.StartLoading();
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                collection = RealmDb.All<ManifestModel>().Where(x => x.IsDraft == false && x.IsQueue == true).ToList();
                AssignColletionToManifest(collection);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            finally
            {
                Loader.StopLoading();
                collection = null;
            }

            QueuedTextColor = "White";
            QueuedBackgroundColor = "#4E6388";
            DraftTextColor = "#4E6388";
            DraftBackgroundColor = "Transparent";
            RecentTextColor = "#4E6388";
            RecentBackgroundColor = "Transparent";
        }

        private void DraftCommandReciever()
        {
            try
            {
                LoadDraftManifestAsync();

                DraftTextColor = "White";
                DraftBackgroundColor = "#4E6388";
                QueuedTextColor = "#4E6388";
                QueuedBackgroundColor = "Transparent";
                RecentTextColor = "#4E6388";
                RecentBackgroundColor = "Transparent";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void RecentCommandReciever()
        {
            List<ManifestModel> collection;
            try
            {
                ManifestCollection.Clear();
                Loader.StartLoading();
                var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
                collection = RealmDb.All<ManifestModel>().Where(x => x.SubmittedDate == DateTimeOffset.UtcNow.Date && x.IsDraft == false && x.IsQueue == false).ToList();
                AssignColletionToManifest(collection);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
                collection = null;
            }

            RecentTextColor = "White";
            RecentBackgroundColor = "#4E6388";
            QueuedTextColor = "#4E6388";
            QueuedBackgroundColor = "Transparent";
            DraftTextColor = "#4E6388";
            DraftBackgroundColor = "Transparent";
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            LoadDraftManifestAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("HomeCommandRecieverAsync"))
            {
                HomeCommandRecieverAsync();
            }
        }

        #endregion
    }
}
