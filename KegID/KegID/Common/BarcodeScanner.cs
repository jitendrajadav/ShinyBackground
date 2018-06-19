using GalaSoft.MvvmLight.Ioc;
using KegID.Messages;
using KegID.Model;
using KegID.Services;
using KegID.ViewModel;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace KegID.Common
{
    public static class BarcodeScanner
    {
        static ZXingScannerPage scanPage;
        private const string Cloud = "collectionscloud.png";

        public static async Task BarcodeScanAsync(IMoveService _moveService, List<Tag> _tags, string _tagsStr, string _page)
        {
            IList<BarcodeModel> models = new List<BarcodeModel>();
            // Create our custom overlay
            var customOverlay = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0
            };

            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            var torch = new Button
            {
                Text = "Toggle Torch",
                BackgroundColor = Color.White,
                HeightRequest = 70,
                VerticalOptions = LayoutOptions.Center
            };

            torch.Clicked += delegate
            {
                scanPage.ToggleTorch();
            };
            var title = new Label
            {
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalTextAlignment = TextAlignment.End
            };
            var done = new Button
            {
                VerticalOptions = LayoutOptions.End,
                Text = "Done",
                TextColor = Color.Blue,
                BackgroundColor = Color.White
            };
            done.Clicked += async delegate
            {
                var message = new StartLongRunningTaskMessage
                {
                    Barcode = models.Select(x => x.Barcode).ToList(),
                    PageName = _page
                };
                MessagingCenter.Send(message, "StartLongRunningTaskMessage");

                await Application.Current.MainPage.Navigation.PopModalAsync();

                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                {
                    case ViewTypeEnum.ScanKegsView:
                        SimpleIoc.Default.GetInstance<ScanKegsViewModel>().AssignBarcodeScannerValue(models);
                        break;
                    case ViewTypeEnum.FillScanView:
                        SimpleIoc.Default.GetInstance<FillScanViewModel>().AssignBarcodeScannerValue(models);
                        break;
                    case ViewTypeEnum.MaintainScanView:
                        SimpleIoc.Default.GetInstance<MaintainScanViewModel>().AssignBarcodeScannerValue(models);
                        break;
                }
            };

            customOverlay.Children.Add(torch, 0, 0);
            customOverlay.Children.Add(title, 0, 1);
            customOverlay.Children.Add(done, 0, 2);

            scanPage = new ZXingScannerPage(customOverlay: customOverlay);
            scanPage.OnScanResult += (result) =>
            Device.BeginInvokeOnMainThread(() =>
            {
                var check = models.Any(x => x.Barcode == result.Text);

                if (!check)
                {
                    title.Text = "Last scan: " + result.Text;
                    BarcodeModel model = new BarcodeModel()
                    {
                        Barcode = result.Text,
                        /*Tags = _tags,*/
                        TagsStr = _tagsStr,
                        Icon = Cloud
                    };
                    models.Add(model);
                    try
                    {
                        // Use default vibration length
                        Vibration.Vibrate();
                    }
                    catch (FeatureNotSupportedException ex)
                    {
                        // Feature not supported on device
                        Crashes.TrackError(ex);
                    }
                    catch (Exception ex)
                    {
                        // Other error has occurred.
                        Crashes.TrackError(ex);
                    }
                }
            });

            await Application.Current.MainPage.Navigation.PushModalAsync(scanPage, animated: false);
        }

        public static async Task BarcodeScanSingleAsync(IMoveService _moveService, List<Tag> _tags, string _tagsStr)
        {
            var barcodes = new Barcode();
            // Create our custom overlay
            var customOverlay = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0
            };

            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            customOverlay.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });

            var torch = new Button
            {
                Text = "Toggle Torch"
            };
            torch.Clicked += delegate
            {
                scanPage.ToggleTorch();
            };
            var title = new Label
            {
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalTextAlignment = TextAlignment.End
            };
            var done = new Button
            {
                VerticalOptions = LayoutOptions.End,
                Text = "Done",
                TextColor = Color.Blue
            };
            done.Clicked += async delegate
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
                switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                {
                    case ViewTypeEnum.ScanKegsView:
                        SimpleIoc.Default.GetInstance<KegSearchViewModel>().AssignBarcodeScannerValueAsync(barcodes);
                        break;
                }
            };

            customOverlay.Children.Add(torch, 0, 0);
            customOverlay.Children.Add(title, 0, 1);
            customOverlay.Children.Add(done, 0, 2);

            scanPage = new ZXingScannerPage(customOverlay: customOverlay);
            scanPage.OnScanResult += (result) =>
            Device.BeginInvokeOnMainThread(async () =>
            {
                title.Text = "Last scan: " + result.Text;
                barcodes.Id = result.Text;
                if (!string.IsNullOrEmpty(barcodes.Id))
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    switch ((ViewTypeEnum)Enum.Parse(typeof(ViewTypeEnum), Application.Current.MainPage.Navigation.ModalStack.LastOrDefault().GetType().Name))
                    {
                        case ViewTypeEnum.KegSearchView:
                            SimpleIoc.Default.GetInstance<KegSearchViewModel>().AssignBarcodeScannerValueAsync(barcodes);
                            break;
                    }
                }
            });

            await Application.Current.MainPage.Navigation.PushModalAsync(scanPage, animated: false);
        }
    }
}
