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

        public ObservableCollection<ManifestModel> ManifestCollection { get; set; } = new ObservableCollection<ManifestModel>();
        public string QueuedTextColor { get; set; } = "#4E6388";
        public string QueuedBackgroundColor { get; set; } = "Transparent";
        public string DraftTextColor { get; set; } = "White";
        public string DraftBackgroundColor { get; set; } = "#4E6388";
        public string RecentTextColor { get; set; } = "#4E6388";
        public string RecentBackgroundColor { get; set; } = "Transparent";

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

        public ManifestsViewModel(INavigationService navigationService) : base(navigationService)
        {
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
                if (model.IsDraft)
                {
                    switch ((EventTypeEnum)model.EventTypeId)
                    {
                        case EventTypeEnum.MOVE_MANIFEST:
                            await _navigationService.NavigateAsync("MoveView", new NavigationParameters
                                {
                                    { "AssignInitialValue", model }
                                }, animated: false);
                            break;
                        case EventTypeEnum.SHIP_MANIFEST:
                            break;
                        case EventTypeEnum.RECEIVE_MANIFEST:
                            break;
                        case EventTypeEnum.FILL_MANIFEST:
                            await _navigationService.NavigateAsync("FillView", new NavigationParameters
                                {
                                    { "AssignInitialValue", model }
                                }, animated: false);
                            break;
                        case EventTypeEnum.PALLETIZE_MANIFEST:
                            break;
                        case EventTypeEnum.RETURN_MANIFEST:
                            break;
                        case EventTypeEnum.REPAIR_MANIFEST:
                            await _navigationService.NavigateAsync("MaintainView", new NavigationParameters
                                {
                                    { "AssignInitialValue", model }
                                }, animated: false);
                            break;
                        case EventTypeEnum.COLLECT_MANIFEST:
                            break;
                        case EventTypeEnum.ARCHIVE_MANIFEST:
                            break;
                        default:
                            break;
                    } 
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
                await _navigationService.GoBackToRootAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ActionSearchCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("SearchManifestsView", animated: false);
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
