using System;
using System.Collections.Generic;
using System.Linq;
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
    public class KegsViewModel : BaseViewModel
    {
        #region Properties

        public string KegsTitle { get; set; }
        public IList<KegPossessionResponseModel> KegPossessionCollection { get; set; }

        #endregion

        #region Commands

        public DelegateCommand PartnerInfoCommand { get; }
        public DelegateCommand<KegPossessionResponseModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public KegsViewModel(INavigationService navigationService) : base(navigationService)
        {
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
                UserDialogs.Instance.ShowLoading("Loading");
                var response = await ApiManager.GetKegPossession(Settings.SessionId, ConstantManager.DBPartnerId);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<IList<KegPossessionResponseModel>>(json, GetJsonSetting()));

                    KegPossessionCollection = data;
                    KegsTitle = KegPossessionCollection.FirstOrDefault()?.PossessorName;
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
