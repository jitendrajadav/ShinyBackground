using System.Collections.ObjectModel;
using KegID.Services;
using System.Linq;
using System.Collections.Generic;
using KegID.Model;
using System;
using Microsoft.AppCenter.Crashes;
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

        public bool IsWorking { get; set; }
        public string InternalBackgroundColor { get; set; }
        public string InternalTextColor { get; set; }
        public string AlphabeticalBackgroundColor { get; set; }
        public string AlphabeticalTextColor { get; set; }
        public string KegsHeldBackgroundColor { get; set; }
        public string KegsHeldTextColor { get; set; }
        public ObservableCollection<PossessorResponseModel> PartnerCollection { get; set; }
        public string PartnerName { get; set; }

        #endregion

        #region Commands

        public DelegateCommand InternalCommand { get;}
        public DelegateCommand AlphabeticalCommand { get; }
        public DelegateCommand<PossessorResponseModel> ItemTappedCommand { get;}
        public DelegateCommand AddNewPartnerCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand TextChangedCommand { get;}
        public DelegateCommand KegsHeldCommand { get;}
        
        #endregion

        #region Constructor

        public DashboardPartnersViewModel(INavigationService navigationService) : base(navigationService)
        {
            InternalCommand = new DelegateCommand(InternalCommandReciever);
            AlphabeticalCommand = new DelegateCommand(AlphabeticalCommandReciever);
            ItemTappedCommand = new DelegateCommand<PossessorResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            KegsHeldCommand = new DelegateCommand(KegsHeldCommandReciever);

            InitialSetting();
            LoadPartners();
        }

        #endregion

        #region Methods

        private void LoadPartners()
        {
            var RealmDb = Realm.GetInstance(RealmDbManager.GetRealmDbConfig());
            AllPartners = RealmDb.All<PossessorResponseModel>().ToList();
            try
            {
                if (AllPartners.Count > 0)
                    PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderByDescending(x => x.KegsHeld));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void KegsHeldCommandReciever()
        {
            try
            {
                InitialSetting();
                PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderByDescending(x => x.KegsHeld));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void InitialSetting()
        {
            try
            {
                KegsHeldBackgroundColor = "#4E6388";
                KegsHeldTextColor = "White";

                AlphabeticalBackgroundColor = "White";
                AlphabeticalTextColor = "#4E6388";

                InternalBackgroundColor = "White";
                InternalTextColor = "#4E6388";
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void TextChangedCommandRecieverAsync()
        {
            try
            {
                var result = AllPartners.Where(x => x.Location.FullName.ToLower().Contains(PartnerName.ToLower()));
                PartnerCollection = new ObservableCollection<PossessorResponseModel>(result);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(PossessorResponseModel model)
        {
            try
            {
                if (model != null)
                {
                    try
                    {
                        ConstantManager.DBPartnerId = model.Location.PartnerId;
                    }
                    catch (Exception)
                    {
                    }
                    await _navigationService.NavigateAsync("PartnerInfoView", new NavigationParameters
                    {
                        { "PartnerModel", model }
                    }, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void AlphabeticalCommandReciever()
        {
            try
            {
                AlphabeticalBackgroundColor = "#4E6388";
                AlphabeticalTextColor = "White";

                InternalBackgroundColor = "White";
                InternalTextColor = "#4E6388";

                KegsHeldBackgroundColor = "White";
                KegsHeldTextColor = "#4E6388";

                PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners.OrderBy(x => x.Location.FullName));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void InternalCommandReciever()
        {
            try
            {
                InternalBackgroundColor = "#4E6388";
                InternalTextColor = "White";

                AlphabeticalBackgroundColor = "White";
                AlphabeticalTextColor = "#4E6388";

                KegsHeldBackgroundColor = "White";
                KegsHeldTextColor = "#4E6388";

                PartnerCollection = new ObservableCollection<PossessorResponseModel>(AllPartners);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BackCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void AddNewPartnerCommandRecieverAsync()
        {
            try
            {
                await _navigationService.NavigateAsync("AddPartnerView", animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
