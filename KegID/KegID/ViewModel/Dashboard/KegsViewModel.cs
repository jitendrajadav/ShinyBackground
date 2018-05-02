using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegsViewModel : BaseViewModel
    {
        #region Properties

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

        public RelayCommand PartnerInfoCommand { get; }
        public RelayCommand<KegPossessionResponseModel> ItemTappedCommand { get; }

        #endregion

        #region Constructor

        public KegsViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            PartnerInfoCommand = new RelayCommand(PartnerInfoCommandRecieverAsync);
            ItemTappedCommand = new RelayCommand<KegPossessionResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
            LoadKegPossessionAsync();
        }

        #endregion

        #region Methods

        private async void PartnerInfoCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void ItemTappedCommandRecieverAsync(KegPossessionResponseModel model)
        {
            //SimpleIoc.Default.GetInstance<KegStatusViewModel>().KegStatusModel = model;
            await Application.Current.MainPage.Navigation.PushModalAsync(new KegStatusView(), animated: false);
            await SimpleIoc.Default.GetInstance<KegStatusViewModel>().LoadMaintenanceHistoryAsync(model.KegId,model.Contents,model.HeldDays,model.PossessorName,model.Barcode,model.TypeName,model.SizeName);
        }

        private async void LoadKegPossessionAsync()
        {
            var value = await _dashboardService.GetKegPossessionAsync(AppSettings.User.SessionId, SimpleIoc.Default.GetInstance<DashboardPartnersViewModel>().PartnerId);
            KegPossessionCollection = value.KegPossessionResponseModel;
            KegsTitle = KegPossessionCollection.FirstOrDefault().PossessorName;
        }

        #endregion
    }
}
