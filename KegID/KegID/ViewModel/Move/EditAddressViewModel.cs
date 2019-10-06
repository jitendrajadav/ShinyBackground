using KegID.Model;
using KegID.Services;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class EditAddressViewModel : BaseViewModel
    {
        #region Properties

        public bool IsShipping { get; private set; }
        public string AddressTitle { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        #endregion

        #region Commands

        public DelegateCommand BackCommand { get; }
        public DelegateCommand DoneCommand { get; }
        public DelegateCommand GetCurrentLocationCommand { get; }
        private readonly IGeolocationService _geolocationService;
        private readonly IPageDialogService _dialogService;

        #endregion

        #region Constructor

        public EditAddressViewModel(INavigationService navigationService,IGeolocationService geolocationService, IPageDialogService dialogService) : base(navigationService)
        {
            _geolocationService = geolocationService;
            _dialogService = dialogService;

            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
            GetCurrentLocationCommand = new DelegateCommand(GetCurrentLocationCommandRecieverAsync);
        }

        #endregion

        #region Methods

        private async void GetCurrentLocationCommandRecieverAsync()
        {
            try
            {
                var location = await _geolocationService.GetLastLocationAsync();

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        var geocodeAddress =
                            $"AdminArea:       {placemark.AdminArea}\n" +
                            $"CountryCode:     {placemark.CountryCode}\n" +
                            $"CountryName:     {placemark.CountryName}\n" +
                            $"FeatureName:     {placemark.FeatureName}\n" +
                            $"Locality:        {placemark.Locality}\n" +
                            $"PostalCode:      {placemark.PostalCode}\n" +
                            $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                            $"SubLocality:     {placemark.SubLocality}\n" +
                            $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                            $"Thoroughfare:    {placemark.Thoroughfare}\n";


                        Line1 = placemark.SubLocality;
                        Line2 = placemark.Locality;
                        Line3 = placemark.SubAdminArea;
                        City = placemark.Locality;
                        State = placemark.AdminArea;
                        PostalCode = placemark.PostalCode;
                        Country = placemark.CountryCode;

                        Console.WriteLine(geocodeAddress);
                    }
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Warning"," location updates unavailable", "Ok");
                }
            }
            catch (FeatureNotSupportedException)
            {
                //Crashes.TrackError(fnsEx);
                // Feature not supported on device
            }
            catch (Exception)
            {
                await _dialogService.DisplayAlertAsync("Warning", " location updates unavailable", "Ok");
                //Crashes.TrackError(ex);
                // Handle exception that may have occurred in geocoding
            }
        }

        private async void DoneCommandRecieverAsync()
        {
            try
            {
                Address address = new Address()
                {
                    Line1 = Line1,
                    Line2 = Line2,
                    Line3 = Line3,
                    City = City,
                    State = State,
                    PostalCode = PostalCode,
                    Country = Country
                };

                if (AddressTitle.Contains("Shipping"))
                    IsShipping = true;
                else
                    IsShipping = false;
                await _navigationService.GoBackAsync(new NavigationParameters
                        {
                            { "EditAddress", address },{ "IsShipping", IsShipping }
                        }, animated: false);

                CleanupData();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void CleanupData()
        {
            try
            {
                Line1 = default;
                Line2 = default;
                Line3 = default;
                City = default;
                State = default;
                PostalCode = default;
                Country = default;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async void BackCommandRecieverAsync()
        {
            await _navigationService.GoBackAsync(animated: false);
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("AddressTitle"))
            {
                AddressTitle = parameters.GetValue<string>("AddressTitle");
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }
        }

        #endregion
    }
}
