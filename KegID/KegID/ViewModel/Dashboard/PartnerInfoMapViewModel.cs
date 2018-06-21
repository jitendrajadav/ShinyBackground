using GalaSoft.MvvmLight.Command;
using KegID.Model;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;

namespace KegID.ViewModel
{
    public class PartnerInfoMapViewModel : BaseViewModel
    {
        #region Properties
        public LocationInfo Position { get; set; }

        #endregion

        #region Commands

        public RelayCommand PartnerInfoCommand { get; }

        #endregion

        #region Constructor

        public PartnerInfoMapViewModel()
        {
            try
            {
                PartnerInfoCommand = new RelayCommand(PartnerInfoCommandRecieverAsync);
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
                await Application.Current.MainPage.Navigation.PopModalAsync();
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
                Position = new LocationInfo
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

        #endregion
    }
}
