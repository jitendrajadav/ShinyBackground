
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
            _toolbarItemSelectDevice = new ToolbarItem();
            _toolbarItemSelectDevice.Order = ToolbarItemOrder.Primary;
            _toolbarItemSelectDevice.Text = "Device";
            _toolbarItemSelectDevice.Priority = 0;

            this.ToolbarItems.Add(_toolbarItemSelectDevice);

            _btnScan = new Button();
            _btnScan.Text = "START SCANNING";
            _btnScan.TextColor = Color.Black;
            _btnScan.VerticalOptions = LayoutOptions.Start;
            _btnScan.HorizontalOptions = LayoutOptions.Fill;
            _btnScan.BackgroundColor = Color.Yellow;
            _btnScan.Margin = new Thickness(5, 0, 5, 5);

            gridMain.Children.Add(_btnScan, 0, 1);

            _lblStatus = new Label();
            _lblStatus.Text = "Disconnected";
            _lblStatus.TextColor = Color.White;
            _lblStatus.FontSize = 11;
            _lblStatus.VerticalOptions = LayoutOptions.Start;
            _lblStatus.HorizontalOptions = LayoutOptions.End;
            _lblStatus.HorizontalTextAlignment = TextAlignment.Center;
            _lblStatus.BackgroundColor = Color.FromHex("#ffff4444");
            _lblStatus.WidthRequest = 80;
            _lblStatus.Margin = new Thickness(0, 5, 5, 0);

            gridCamera.Children.Add(_lblStatus);

            _lblResultType = new Label();
            _lblResultType.TextColor = Color.White;
            _lblResultType.VerticalOptions = LayoutOptions.Start;
            _lblResultType.HorizontalOptions = LayoutOptions.Fill;
            _lblResultType.HorizontalTextAlignment = TextAlignment.Center;
            _lblResultType.FontSize = 20;

            slResultContainer.Children.Insert(0, _lblResultType);

            _lblResult = new Label();
            _lblResult.TextColor = Color.White;
            _lblResult.VerticalOptions = LayoutOptions.Fill;
            _lblResult.HorizontalOptions = LayoutOptions.Fill;
            _lblResult.HorizontalTextAlignment = TextAlignment.Center;
            _lblResult.VerticalTextAlignment = TextAlignment.Center;
            _lblResult.FontSize = 15;

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