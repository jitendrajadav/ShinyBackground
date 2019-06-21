using System;
using System.Collections.Generic;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class SearchPartnersViewModel : BaseViewModel
    {
        #region Properties

        private readonly IMoveService _moveService;
        public string BackPartners { get; set; }
        public string PartnerSearch { get; set; }
        public IList<PartnerModel> PartnerSearchCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand BackPartnersCommand { get; }
        public DelegateCommand PartnerSearchCommand { get; }
        public DelegateCommand<PartnerModel> ItemTappedCommand { get; }

        #endregion

        #region Contructor

        public SearchPartnersViewModel(IMoveService moveService, INavigationService navigationService) : base(navigationService)
        {
            _moveService = moveService;

            BackPartners = "< Partners";
            BackPartnersCommand = new DelegateCommand(BackPartnersCommandRecieverAsync);
            PartnerSearchCommand = new DelegateCommand(PartnerSearchCommandRecieverAsync);
            ItemTappedCommand = new DelegateCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void PartnerSearchCommandRecieverAsync()
        {
            try
            {
                Loader.StartLoading();
                var value = await _moveService.GetPartnerSearchAsync(AppSettings.SessionId, PartnerSearch, false, false);

                if (value.Response.StatusCode == System.Net.HttpStatusCode.OK.ToString())
                {
                    PartnerSearchCollection = value.PartnerModel;
                }
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

        private async void BackPartnersCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        private async void ItemTappedCommandRecieverAsync(PartnerModel model)
        {
            try
            {
                if (model != null)
                {
                    ConstantManager.Partner = model;
                    await _navigationService.NavigateAsync("../../", new NavigationParameters
                    {
                        { "model", model }
                    }, animated: false);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackPartnersCommandRecieverAsync"))
            {
                BackPartnersCommandRecieverAsync();
            }
        }

        #endregion
    }
}
