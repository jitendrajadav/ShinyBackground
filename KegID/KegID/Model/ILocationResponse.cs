using KegID.Common;
using System;
using System.Globalization;

namespace KegID.Model
{
    public interface ILocationResponse
    {
    }

    public class GeoLocation : ILocationResponse
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public static GeoLocation Parse(string location)
        {
            GeoLocation result = new GeoLocation();

            var locationSetting = Settings.DefaultFallbackMapsLocation;
            var locationParts = locationSetting.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            result.Latitude = double.Parse(locationParts[0], CultureInfo.InvariantCulture);
            result.Longitude = double.Parse(locationParts[1], CultureInfo.InvariantCulture);

            return result;
        }
    }
}
