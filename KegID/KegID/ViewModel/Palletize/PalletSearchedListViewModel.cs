using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using KegID.Common;
using KegID.Model;
using KegID.Services;
using KegID.Views;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PalletSearchedListViewModel : BaseViewModel
    {
        #region Properties

        public IDashboardService _dashboardService { get; set; }

        #region PalletSearchCollection

        /// <summary>
        /// The <see cref="PalletSearchCollection" /> property's name.
        /// </summary>
        public const string PalletSearchCollectionPropertyName = "PalletSearchCollection";

        private IList<SearchPalletResponseModel> _PalletSearchCollection = null;

        /// <summary>
        /// Sets and gets the PalletSearchCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IList<SearchPalletResponseModel> PalletSearchCollection
        {
            get
            {
                return _PalletSearchCollection;
            }

            set
            {
                if (_PalletSearchCollection == value)
                {
                    return;
                }

                _PalletSearchCollection = value;
                RaisePropertyChanged(PalletSearchCollectionPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        public RelayCommand BackCommand { get; }
        public RelayCommand<SearchPalletResponseModel> ItemTappedCommand { get;}

        #endregion

        #region Contructor

        public PalletSearchedListViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

            BackCommand = new RelayCommand(BackCommandRecieverAsync);
            ItemTappedCommand = new RelayCommand<SearchPalletResponseModel>((model) => ItemTappedCommandRecieverAsync(model));
        }

        #endregion

        #region Methods

        private async void BackCommandRecieverAsync()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async void GetPalletSearchAsync(string partnerId,string fromDate, string toDate,string kegs,string kegOwnerId)
        {
            try
            {
                //needs to assing partnerId??string.Empty once backend is ready...
                var value = await _dashboardService.GetPalletSearchAsync(AppSettings.User.SessionId, string.Empty, fromDate, toDate, kegs, kegOwnerId);
                PalletSearchCollection = value.SearchPalletResponseModel;
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void ItemTappedCommandRecieverAsync(SearchPalletResponseModel model)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new PalletizeDetailView(), animated: false);
                SimpleIoc.Default.GetInstance<PalletizeDetailViewModel>().AssingIntialValueAsync(model, true);
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
