﻿using System;
using System.Collections.Generic;
using System.Linq;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class KegsViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;
        public IDashboardService _dashboardService { get; set; }

        #region KegsTitle
        /// <summary>
        /// The <see cref="KegsTitle" /> property's name.
        /// </summary>
        public const string KegsTitlePropertyName = "KegsTitle";

        private string _KegsTitle = default(string);

        /// <summary>
        /// Sets and gets the KegsTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string KegsTitle
        {
            get
            {
                return _KegsTitle;
            }

            set
            {
                if (_KegsTitle == value)
                {
                    return;
                }

                _KegsTitle = value;
                RaisePropertyChanged(KegsTitlePropertyName);
            }
        }
        #endregion

        #region KegPossessionCollection

        /// <summary>
        /// The <see cref="KegPossessionCollection" /> property's name.
        /// </summary>
        public const string KegPossessionCollectionPropertyName = "KegPossessionCollection";

        private IList<KegPossessionResponseModel> _KegPossessionCollection = null;

        /// <summary>
        /// Sets and gets the KegPossessionCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<KegPossessionResponseModel> KegPossessionCollection
        {
            get
            {
                return _KegPossessionCollection;
            }

            set
            {
                if (_KegPossessionCollection == value)
                {
                    return;
                }

                _KegPossessionCollection = value;
                RaisePropertyChanged(KegPossessionCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public DelegateCommand PartnerInfoCommand { get; }
        public DelegateCommand<KegPossessionResponseModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public KegsViewModel(IDashboardService dashboardService, INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            _dashboardService = dashboardService;
            PartnerInfoCommand = new DelegateCommand(PartnerInfoCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<KegPossessionResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            LoadKegPossessionAsync();
        }

        #endregion

        #region Methods

        private async void PartnerInfoCommandRecieverAsync()
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PopModalAsync();
                await _navigationService.GoBackAsync(useModalNavigation: true, animated: false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(KegPossessionResponseModel model)
        {
            try
            {
                //await Application.Current.MainPage.Navigation.PushModalAsync(new KegStatusView(), animated: false);
                //await SimpleIoc.Default.GetInstance<KegStatusViewModel>().LoadMaintenanceHistoryAsync(model.KegId, model.Contents, model.HeldDays, model.PossessorName, model.Barcode, model.TypeName, model.SizeName);
                var param = new NavigationParameters
                    {
                        { "model", model }
                    };
                await _navigationService.NavigateAsync(new Uri("KegStatusView", UriKind.Relative), param, useModalNavigation: true, animated: false);


            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void LoadKegPossessionAsync()
        {
            try
            {
                var value = await _dashboardService.GetKegPossessionAsync(AppSettings.User.SessionId, AppSettings.User.DBPartnerId);
                KegPossessionCollection = value.KegPossessionResponseModel;
                KegsTitle = KegPossessionCollection.FirstOrDefault().PossessorName;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            
        }

        #endregion
    }
}
