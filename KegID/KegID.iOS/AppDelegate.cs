using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Touch;
using Foundation;
using KegID.iOS.DependencyServices;
using KegID.iOS.Services;
using KegID.Messages;
using Plugin.CrossPlatformTintedImage.iOS;
using System;
using UIKit;
using Xamarin.Forms;

namespace KegID.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        iOSLongRunningTaskExample longRunningTaskExample;
        void WireUpDownloadTask()
        {
            MessagingCenter.Subscribe<DownloadMessage>(this, "Download", async message => {
                var downloader = new Downloader(message.Url);
                await downloader.DownloadFile();
            });
        }

        public static Action BackgroundSessionCompletionHandler;

        public override void HandleEventsForBackgroundUrl(UIApplication application, string sessionIdentifier, Action completionHandler)
        {
            Console.WriteLine("HandleEventsForBackgroundUrl(): " + sessionIdentifier);
            // We get a completion handler which we are supposed to call if our transfer is done.
            BackgroundSessionCompletionHandler = completionHandler;
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Xamarin.FormsMaps.Init();

            TintedImageRenderer.Init();
            DependencyService.Register<OpenAppService>();
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            LoadApplication(new App());

            WireUpLongRunningTask();
            WireUpDownloadTask();

            return base.FinishedLaunching(app, options);
        }
        void WireUpLongRunningTask()
        {
            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", async message => {
                longRunningTaskExample = new iOSLongRunningTaskExample();
                await longRunningTaskExample.Start();
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
                longRunningTaskExample.Stop();
            });
        }

        // Different way to handle BG Services
        //public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    // Check for new data, and display it
        //    MessagingCenter.Send<object, string>(this, "UpdateLabel", "Hello from iOS");

        //    // Inform system of fetch results
        //    completionHandler(UIBackgroundFetchResult.NewData);
        //}
    }
}
