using System;
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

        private readonly IDashboardService _dashboardService;
        public string KegsTitle { get; set; }
        public IList<KegPossessionResponseModel> KegPossessionCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand PartnerInfoCommand { get; }
        public DelegateCommand<KegPossessionResponseModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public KegsViewModel(IDashboardService dashboardService, INavigationService navigationService) : base(navigationService)
        {
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
                await _navigationService.GoBackAsync(animated: false);
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
                await _navigationService.NavigateAsync("KegStatusView", new NavigationParameters { { "KegStatusModel", model } }, animated: false);
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
                Loader.StartLoading();
                var value = await _dashboardService.GetKegPossessionAsync(AppSettings.SessionId, ConstantManager.DBPartnerId);
                KegPossessionCollection = value.KegPossessionResponseModel;
                KegsTitle = KegPossessionCollection.FirstOrDefault()?.PossessorName;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                Loader.StopLoading();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PartnerInfoCommandRecieverAsync"))
            {
                PartnerInfoCommandRecieverAsync();
            }
        }

        #endregion
    }
}
