using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KegID.Services
{
    public class GeolocationService : IGeolocationService, INotifyPropertyChanged
    {
        string notAvailable = "not available";
        string lastLocation;
        string currentLocation;
        int accuracy = (int)GeolocationAccuracy.Medium;
        CancellationTokenSource cts;
        private readonly IPageDialogService _dialogService;
        public Location location = null;

        public GeolocationService(IPageDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public string LastLocation
        {
            get => lastLocation;
            set => SetProperty(ref lastLocation, value);
        }

        public string CurrentLocation
        {
            get => currentLocation;
            set => SetProperty(ref currentLocation, value);
        }

        public string[] Accuracies
            => Enum.GetNames(typeof(GeolocationAccuracy));

        public int Accuracy
        {
            get => accuracy;
            set => SetProperty(ref accuracy, value);
        }

        protected virtual bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "", Action onChanged = null, Func<T, T, bool> validateValue = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            if (validateValue != null && !validateValue(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<Location> OnGetCurrentLocationAsync()
        {
            Location location = null;
            try
            {
                var request = new GeolocationRequest((GeolocationAccuracy)Accuracy);
                cts = new CancellationTokenSource(10000);
                location = await Geolocation.GetLocationAsync(request, cts.Token);
                CurrentLocation = FormatLocation(location);
            }
            catch (FeatureNotSupportedException)
            {
            }
            catch (PermissionException)
            {
                location = new Location(0, 0);
            }
            catch (FeatureNotEnabledException fNEx)
            {
                location = null;
                throw fNEx;
            }
            catch (Exception ex)
            {
                CurrentLocation = FormatLocation(null, ex);
            }
            finally
            {
                cts.Dispose();
                cts = null;
            }
            return location;
        }

        public async Task<Location> OnGetLastLocationAsync()
        {
            Location location = null;
            try
            {
                location = await Geolocation.GetLastKnownLocationAsync();
                LastLocation = FormatLocation(location);
            }
            catch (Exception ex)
            {
                LastLocation = FormatLocation(null, ex);
            }
            return location;
        }

        public async Task<Location> GetLastLocationAsync()
        {
            if (location != null)
                return location;
            else
            {
                try
                {
                    location = await OnGetCurrentLocationAsync();
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Warning", ex.Message + " Please enable location Service and try again", "Ok");
                }
                return location;
            }
        }

        string FormatLocation(Location location, Exception ex = null)
        {
            if (location == null)
            {
                return $"Unable to detect location. Exception: {ex?.Message ?? string.Empty}";
            }

            return
                $"Latitude: {location.Latitude}\n" +
                $"Longitude: {location.Longitude}\n" +
                $"Accuracy: {location.Accuracy}\n" +
                $"Altitude: {(location.Altitude.HasValue ? location.Altitude.Value.ToString() : notAvailable)}\n" +
                $"Heading: {(location.Course.HasValue ? location.Course.Value.ToString() : notAvailable)}\n" +
                $"Speed: {(location.Speed.HasValue ? location.Speed.Value.ToString() : notAvailable)}\n" +
                $"Date (UTC): {location.Timestamp:d}\n" +
                $"Time (UTC): {location.Timestamp:T}";
        }

        public async Task InitCurrentLocationAsync()
        {
            try
            {
                location = await OnGetCurrentLocationAsync();
            }
            catch { }
        }
    }
}
