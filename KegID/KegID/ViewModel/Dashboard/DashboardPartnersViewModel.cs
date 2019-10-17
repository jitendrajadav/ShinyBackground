﻿using System.Collections.ObjectModel;
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
        public ObservableCollection<PossessorResponseModel> PartnerCollection { get; set; }
        public string PartnerName { get; set; }
        public int SelectedSegment { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<PossessorResponseModel> ItemTappedCommand { get;}
        public DelegateCommand AddNewPartnerCommand { get; }
        public DelegateCommand BackCommand { get; }
        public DelegateCommand TextChangedCommand { get;}
        public DelegateCommand<object> SelectedSegmentCommand { get;}

        #endregion

        #region Constructor

        public DashboardPartnersViewModel(INavigationService navigationService) : base(navigationService)
        {
            ItemTappedCommand = new DelegateCommand<PossessorResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            AddNewPartnerCommand = new DelegateCommand(AddNewPartnerCommandRecieverAsync);
            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            TextChangedCommand = new DelegateCommand(TextChangedCommandRecieverAsync);
            SelectedSegmentCommand = new DelegateCommand<object>((seg) => SelectedSegmentCommandReciever(seg));

            LoadPartners();
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
        }


        private void TextChangedCommandRecieverAsync()
        {
            if (!string.IsNullOrEmpty(PartnerName))
            {
                try
                {
                    var result = AllPartners.Where(x => x.Location.FullName.IndexOf(PartnerName, StringComparison.OrdinalIgnoreCase) >= 0);
                    PartnerCollection = new ObservableCollection<PossessorResponseModel>(result);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
            else
            {
                SelectedSegmentCommandReciever(SelectedSegment);
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
