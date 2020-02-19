using System.Collections.ObjectModel;
using KegID.Services;
using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System;
using Realms;
using KegID.LocalDb;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class DashboardPartnersViewModel : BaseViewModel
    {
        #region Properties

        public IList<PossessorResponseModel> AllPartners { get; set; }
        public ObservableCollection<PossessorResponseModel> PartnerCollection { get; set; }
        public string PartnerName { get; set; }
        public int SelectedSegment { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<PossessorResponseModel> ItemTappedCommand { get; }
        public DelegateCommand AddNewPartnerCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand TextChangedCommand { get; }
        public DelegateCommand<object> SelectedSegmentCommand { get; }
        public string ContainerTypes { get; set; }

        #endregion

        #region Constructor

        public DashboardPartnersViewModel(INavigationService navigationService) : base(navigationService)
        {
            ItemTappedCommand = new DelegateCommand<PossessorResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            SelectedSegmentCommand = new DelegateCommand<object>((seg) => SelectedSegmentCommandReciever(seg));

            PreferenceSetting();
            LoadPartners();
        }

        private void PreferenceSetting()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            var preferences = RealmDb.All<Preference>().ToList();

            ContainerTypes = preferences.Find(x => x.PreferenceName == "CONTAINER_TYPES").PreferenceValue;
        }

        private void SelectedSegmentCommandReciever(object seg)
        {
            SelectedSegment = (int)seg;
            if (AllPartners.Count > 0)
            {
                switch (seg)
                {
                    case 0:
                        PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners);
                        break;
                    case 1:
                        PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderBy(x => x.Location.FullName));
                        break;
                    case 2:
                        PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderByDescending(x => x.KegsHeld));
                        break;
                }
            }
        }

        #endregion

        #region Methods

        private void LoadPartners()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            AllPartners = RealmDb.All<PossessorResponseModel>().ToList();
            using (var trans = RealmDb.BeginWrite())
            {
                foreach (var item in AllPartners)
                {
                    item.ContainerTypes = ContainerTypes;
                }
                trans.Commit();
            }
        }

        private void TextChangedCommandRecieverAsync()
        {
            if (!string.IsNullOrEmpty(PartnerName))
            {
                var result = AllPartners.Where(x => x.Location.FullName.IndexOf(PartnerName, StringComparison.OrdinalIgnoreCase) >= 0);
                PartnerCollection = new ObservableCollection<PossessorResponseModel>(result);
            }
            else
            {
                SelectedSegmentCommandReciever(SelectedSegment);
            }
        }

        private async void ItemTappedCommandRecieverAsync(PossessorResponseModel model)
        {
            if (model != null)
            {
                ConstantManager.DBPartnerId = model.Location.PartnerId;

                await NavigationService.NavigateAsync("PartnerInfoView", new NavigationParameters
                    {
                        { "PartnerModel", model }
                    }, animated: false);
            }
        }

        private async void BackCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
        }

        private async void AddNewPartnerCommandRecieverAsync()
        {
            await NavigationService.NavigateAsync("AddPartnerView", animated: false);

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }
        }

        #endregion
    }
}
