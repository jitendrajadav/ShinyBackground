using KegID.Common;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace KegID.ViewModel
{
    public class PartnerInfoMapViewModel : BaseViewModel
    {
        #region Properties

        private readonly INavigationService _navigationService;

        #endregion

        #region Commands

        public DelegateCommand PartnerInfoCommand { get; }

        #endregion

        #region Constructor

        public PartnerInfoMapViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException("navigationService");

            try
            {
                PartnerInfoCommand = new DelegateCommand(PartnerInfoCommandRecieverAsync);
            }
            catch (Exception ex)
            {
                 Crashes.TrackError(ex);
            }            
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

        internal void AssignInitialValue(double _lat, double _lon, string _lable, string _address)
        {
            try
            {
                //Position = new LocationInfo
                //{
                //    Lat = _lat,
                //    Lon = _lon,
                //    Label = _lable,
                //    Address = _address
                //};
                ConstantManager.Position = new LocationInfo
                {
                    Lat = _lat,
                    Lon = _lon,
                    Label = _lable,
                    Address = _address
                };
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
            //if (parameters.ContainsKey("PartnerInfoModel"))
            //{
            //    var value = parameters.GetValue<PartnerInfoResponseModel>("PartnerInfoModel");
            //    AssignInitialValue(value.Lat, value.Lat, value.BillAddress.City, value.BillAddress.Line1);
            //}
        }

        #endregion
    }
}
