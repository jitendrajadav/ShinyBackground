using KegID.Common;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PartnerInfoMapView : ContentPage
    {
        Map map;

        public PartnerInfoMapView ()
		{
			InitializeComponent ();
            map = new Map(MapSpan.FromCenterAndRadius(
                    new Position((long)Geolocation.savedPosition.Latitude, (long)Geolocation.savedPosition.Longitude), Distance.FromMiles(0.3)))
            {  
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            //MapHelper.CenterMapInDefaultLocation(MapControl);

    //        map.MoveToRegion(MapSpan.FromCenterAndRadius(
    //new Position((long)Geolocation.savedPosition.Latitude, (long)Geolocation.savedPosition.Longitude), Distance.FromMiles(0))); // Santa Cruz golf course

            var position = new Position((long)Geolocation.savedPosition.Latitude, (long)Geolocation.savedPosition.Longitude); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "Powai",
                Address = "custom detail info"
            };
            map.Pins.Add(pin);


            //MapControl.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));

            // for debugging output only
            map.PropertyChanged += (sender, e) => {
                Debug.WriteLine(e.PropertyName + " just changed!");
                if (e.PropertyName == "VisibleRegion" && map.VisibleRegion != null)
                    CalculateBoundingCoordinates(map.VisibleRegion);
            };

            MapControl.Children.Add(map);

        }

        /// <summary>
        /// In response to this forum question http://forums.xamarin.com/discussion/22493/maps-visibleregion-bounds
        /// Useful if you need to send the bounds to a web service or otherwise calculate what
        /// pins might need to be drawn inside the currently visible viewport.
        /// </summary>
        static void CalculateBoundingCoordinates(MapSpan region)
        {
            // WARNING: I haven't tested the correctness of this exhaustively!
            var center = region.Center;
            var halfheightDegrees = region.LatitudeDegrees / 2;
            var halfwidthDegrees = region.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            if (left < -180) left = 180 + (180 + left);
            if (right > 180) right = (right - 180) - 180;
            // I don't wrap around north or south; I don't think the map control allows this anyway

            Debug.WriteLine("Bounding box:");
            Debug.WriteLine("                    " + top);
            Debug.WriteLine("  " + left + "                " + right);
            Debug.WriteLine("                    " + bottom);
        }

    }
}