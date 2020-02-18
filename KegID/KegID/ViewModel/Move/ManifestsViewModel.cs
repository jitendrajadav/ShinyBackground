using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KegID.LocalDb;
using KegID.Model;
using Prism.Commands;
using Prism.Navigation;
using Realms;

namespace KegID.ViewModel
{
    public class ManifestsViewModel : BaseViewModel
    {
        #region Properties

        public ObservableCollection<ManifestModel> ManifestCollection { get; set; } = new ObservableCollection<ManifestModel>();

        #endregion

        #region Commands

        public DelegateCommand HomeCommand { get; }
        public DelegateCommand ActionSearchCommand { get; }
        public DelegateCommand QueuedCommand { get; }
        public DelegateCommand DraftCommand { get; }
        public DelegateCommand RecentCommand { get; }
        public DelegateCommand<ManifestModel> ItemTappedCommand { get; }
        public DelegateCommand<object> SelectedSegmentCommand { get; }
        public int SelectedSegment { get; private set; }

        #endregion

        #region Constructor

        public ManifestsViewModel(INavigationService navigationService) : base(navigationService)
        {
            HomeCommand = new DelegateCommand(HomeCommandRecieverAsync);
            ActionSearchCommand = new DelegateCommand(ActionSearchCommandRecieverAsync);
            SelectedSegmentCommand = new DelegateCommand<object>((seg) => SelectedSegmentCommandReciever(seg));
            ItemTappedCommand = new DelegateCommand<ManifestModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

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

        private async void HomeCommandRecieverAsync()
        {
                await _navigationService.GoBackToRootAsync();
        }

        private async void ActionSearchCommandRecieverAsync()
        {
            await _navigationService.NavigateAsync("SearchManifestsView", animated: false);
        }

        private void SelectedSegmentCommandReciever(object seg)
        {
            ManifestCollection.Clear();
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var manifest = RealmDb.All<ManifestModel>().ToList();
            SelectedSegment = (int)seg;
            if (manifest.Count > 0)
            {
                switch (seg)
                {
                    case 0:
                        var result = manifest.Where(x => !x.IsDraft && x.IsQueue).ToList();
                        if (result.Count > 0)
                        {
                            AssignColletionToManifest(result);
                        }
                        break;
                    case 1:
                        result = RealmDb.All<ManifestModel>().ToList().Where(x => x.IsDraft && !x.IsQueue).ToList();
                        if (result.Count > 0)
                        {
                            AssignColletionToManifest(result);
                        }
                        break;
                    case 2:
                        result = RealmDb.All<ManifestModel>().ToList().Where(x => x.SubmittedDate == DateTimeOffset.UtcNow.Date && !x.IsDraft && !x.IsQueue).ToList();
                        if (result.Count > 0)
                        {
                            AssignColletionToManifest(result);
                        }
                        break;
                }
            }
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
