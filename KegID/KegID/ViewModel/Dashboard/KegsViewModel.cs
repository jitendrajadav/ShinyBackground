using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Model;
using KegID.Services;
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
        #endregion

        #region Constructor
        public KegsViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            PartnerInfoCommand = new RelayCommand(PartnerInfoCommandRecieverAsync);
            LoadKegPossessionAsync();
        }

        private async void LoadKegPossessionAsync()
        {
            var value = await _dashboardService.GetKegPossessionAsync(Configuration.SessionId, SimpleIoc.Default.GetInstance<DashboardPartnersViewModel>().PartnerId);
            KegPossessionCollection = value.KegPossessionResponseModel;
            KegsTitle = KegPossessionCollection.FirstOrDefault().PossessorName;
        }

        #endregion

        #region Methods
        private async void PartnerInfoCommandRecieverAsync()
        {
           await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #endregion
    }
}
