
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CognexScanView : ContentPage
	{
        public static ToolbarItem _toolbarItemSelectDevice;
        public static Button _btnScan;
        public static Label _lblStatus;
        public static Label _lblResultType;
        public static Label _lblResult;

        private bool firstStart = true;

        public CognexScanView ()
		{
			InitializeComponent ();
            _toolbarItemSelectDevice = new ToolbarItem
            {
                Order = ToolbarItemOrder.Primary,
                Text = "Device",
                Priority = 0
            };

            ToolbarItems.Add(_toolbarItemSelectDevice);

            _btnScan = new Button
            {
                Text = "START SCANNING",
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.Yellow,
                Margin = new Thickness(5, 0, 5, 5)
            };

            gridMain.Children.Add(_btnScan, 0, 1);

            _lblStatus = new Label
            {
                Text = "Disconnected",
                TextColor = Color.White,
                FontSize = 11,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.FromHex("#ffff4444"),
                WidthRequest = 80,
                Margin = new Thickness(0, 5, 5, 0)
            };

            gridCamera.Children.Add(_lblStatus);

            _lblResultType = new Label
            {
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 20
            };

            slResultContainer.Children.Insert(0, _lblResultType);

            _lblResult = new Label
            {
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 15
            };

            scrollViewResult.Content = _lblResult;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (firstStart)
            {
                firstStart = false;
                gridCamera.Children.Insert(0, new CameraPreview());
            }
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                gridContainer.ColumnDefinitions.Clear();

                Grid.SetRow(gridCamera, 0);
                Grid.SetColumn(gridCamera, 0);
                Grid.SetRow(slResultContainer, 0);
                Grid.SetColumn(slResultContainer, 1);

                if (Device.RuntimePlatform == Device.Android && (gridCamera.Width / gridCamera.Height) < 1.8)
                {
                    try
                    {
                        gridContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = width - 190 });
                        gridContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = 180 });
                    }
                    catch { gridContainer.ColumnDefinitions.Clear(); }
                }
            }
            else
            {
                gridContainer.ColumnDefinitions.Clear();

                Grid.SetRow(gridCamera, 0);
                Grid.SetColumn(gridCamera, 0);
                Grid.SetRow(slResultContainer, 1);
                Grid.SetColumn(slResultContainer, 0);
            }
        }

    }
}