using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using KegID.Common;
using KegID.Model;
using KegID.Services;
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
            await NavigationService.GoBackAsync(animated: false);
        }

        private async void ItemTappedCommandRecieverAsync(KegPossessionResponseModel model)
        {
            await NavigationService.NavigateAsync("KegStatusView", new NavigationParameters { { "KegStatusModel", model } }, animated: false);
        }

        private async void LoadKegPossessionAsync()
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
            UserDialogs.Instance.HideLoading();
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
