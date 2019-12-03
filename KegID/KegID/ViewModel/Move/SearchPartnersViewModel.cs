using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace KegID.ViewModel
{
    public class SearchPartnersViewModel : BaseViewModel
    {
        #region Properties

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

        public SearchPartnersViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackPartners = "< Partners";
            BackPartnersCommand = new DelegateCommand(BackPartnersCommandRecieverAsync);
            PartnerSearchCommand = new DelegateCommand(async () => await RunSafe(PartnerSearchCommandRecieverAsync()));
            ItemTappedCommand = new DelegateCommand<PartnerModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async Task PartnerSearchCommandRecieverAsync()
        {
            if (!string.IsNullOrEmpty(PartnerSearch))
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Loading");
                    var response = await ApiManager.GetPartnerSearch(AppSettings.SessionId, PartnerSearch, false, true);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<PartnerModel>>(json, GetJsonSetting()));

                        PartnerSearchCollection = data;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                }
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
