using KegID.Services;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PartnerInfoMapView : ContentPage
    {
        public PartnerInfoMapView ()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var map = new Map
            {
                MapType = MapType.Street,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            NavigeatToMapAsync(map);
        }

        private void NavigeatToMapAsync(Map map)
        {
            var model = ConstantManager.Position;
            var position = new Position(model.Lat, model.Lon);
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = model.Label,
                Address = model.Address
            };

            map.Pins.Add(pin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1.0)));
            MapControl.Children.Add(map);
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as INavigationAware)?.OnNavigatedTo(new NavigationParameters
                    {
                        { "PartnerInfoCommandRecieverAsync", "PartnerInfoCommandRecieverAsync" }
                    });
            return true;
        }
    }
}