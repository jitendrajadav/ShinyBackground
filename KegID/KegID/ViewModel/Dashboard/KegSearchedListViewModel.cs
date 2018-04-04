using GalaSoft.MvvmLight.Command;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class KegSearchedListViewModel : BaseViewModel
    {
        #region Properties
        public IDashboardService _dashboardService { get; set; }

        #region KegSearchCollection

        /// <summary>
        /// The <see cref="KegSearchCollection" /> property's name.
        /// </summary>
        public const string KegSearchCollectionPropertyName = "KegSearchCollection";

        private IList<KegSearchResponseModel> _KegSearchCollection = null;

        /// <summary>
        /// Sets and gets the KegSearchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<KegSearchResponseModel> KegSearchCollection
        {
            get
            {
                return _KegSearchCollection;
            }

            set
            {
                if (_KegSearchCollection == value)
                {
                    return;
                }

                _KegSearchCollection = value;
                RaisePropertyChanged(KegSearchCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand<KegSearchResponseModel> ItemTappedCommand { get; set; }

        #endregion

        #region Contructor

        public KegSearchedListViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            ItemTappedCommand = new RelayCommand<KegSearchResponseModel>(execute: (model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void ItemTappedCommandRecieverAsync(KegSearchResponseModel model)
        {
           await Application.Current.MainPage.Navigation.PushModalAsync(new KegStatusView());
        }

        internal async void LoadKegSearchAsync(string barcode)
        {
            var value = await _dashboardService.GetKegSearchAsync(AppSettings.User.SessionId, barcode, false);
            KegSearchCollection = value.KegSearchResponseModel;
        }

        #endregion
    }
}
