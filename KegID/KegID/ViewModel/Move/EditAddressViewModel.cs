using KegID.Delegates;
using KegID.Model;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Shiny;
using Shiny.Locations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KegID.ViewModel
{
    public class EditAddressViewModel : BaseViewModel, IDestructible
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
        private readonly IGpsListener _gpsListener;
        private readonly IGpsManager _gpsManager;
        public Position LocationMessage { get; set; }
        #endregion

        #region Commands

        public DelegateCommand BackCommand { get; }
        public DelegateCommand DoneCommand { get; }
        public DelegateCommand GetCurrentLocationCommand { get; }
        //private readonly IGeolocationService _geolocationService;
        private readonly IPageDialogService _dialogService;

        #endregion

        #region Constructor

        public EditAddressViewModel(INavigationService navigationService, IGpsManager gpsManager, IGpsListener gpsListener, IPageDialogService dialogService) : base(navigationService)
        {
            //_geolocationService = geolocationService;
            _gpsManager = gpsManager;
            _gpsListener = gpsListener;
            _gpsListener.OnReadingReceived += OnReadingReceived;

            _dialogService = dialogService;

            BackCommand = new DelegateCommand(BackCommandRecieverAsync);
            DoneCommand = new DelegateCommand(DoneCommandRecieverAsync);
            GetCurrentLocationCommand = new DelegateCommand(GetCurrentLocationCommandRecieverAsync);
        }

        #endregion

        #region Methods

        void OnReadingReceived(object sender, GpsReadingEventArgs e)
        {
            LocationMessage = e.Reading.Position;
            //= $"{e.Reading.Position.Latitude}, {e.Reading.Position.Longitude}";
        }

        private async void GetCurrentLocationCommandRecieverAsync()
        {
            //var location = await _geolocationService.GetLastLocationAsync();

            if (LocationMessage != null)
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(LocationMessage.Latitude, LocationMessage.Longitude);

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
                await _dialogService.DisplayAlertAsync("Warning", " location updates unavailable", "Ok");
            }
        }

        private async void DoneCommandRecieverAsync()
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

            IsShipping = AddressTitle.Contains("Shipping");
            await NavigationService.GoBackAsync(new NavigationParameters
                        {
                            { "EditAddress", address },{ "IsShipping", IsShipping }
                        }, animated: false);

            CleanupData();
        }

        private void CleanupData()
        {
            Line1 = default;
            Line2 = default;
            Line3 = default;
            City = default;
            State = default;
            PostalCode = default;
            Country = default;
        }

        private async void BackCommandRecieverAsync()
        {
            await NavigationService.GoBackAsync(animated: false);
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("AddressTitle"))
            {
                AddressTitle = parameters.GetValue<string>("AddressTitle");
            }

            return base.InitializeAsync(parameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("BackCommandRecieverAsync"))
            {
                BackCommandRecieverAsync();
            }

            if (_gpsManager.IsListening)
            {
                await _gpsManager.StopListener();
            }

            await _gpsManager.StartListener(new GpsRequest
            {
                UseBackground = true,
                Priority = GpsPriority.Highest,
                Interval = TimeSpan.FromSeconds(5),
                ThrottledInterval = TimeSpan.FromSeconds(3) //Should be lower than Interval
            });
        }

        public void Destroy()
        {
            _gpsListener.OnReadingReceived -= OnReadingReceived;
        }

        #endregion
    }
}
