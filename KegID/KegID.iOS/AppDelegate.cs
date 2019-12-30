﻿using Foundation;
using KegID.iOS.Services;
using KegID.Messages;
using Plugin.CrossPlatformTintedImage.iOS;
using Prism;
using Prism.Ioc;
using System;
using UIKit;
using Xamarin.Forms;
using Microsoft.AppCenter.Distribute;
using SegmentedControl.FormsPlugin.iOS;
using Shiny;

namespace KegID.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //iOSLongRunningTaskExample longRunningTaskExample;
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
            // this needs to be loaded before EVERYTHING
            iOSShinyHost.Init(new Startup());

            Forms.Init();
            FormsMaterial.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Xamarin.FormsMaps.Init();
            Rg.Plugins.Popup.Popup.Init();
            TintedImageRenderer.Init();
            //UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);
            Forms9Patch.iOS.Settings.Initialize(this);
            SegmentedControlRenderer.Init();

            Distribute.DontCheckForUpdatesInDebug();

            LoadApplication(new App());
            //WireUpLongRunningTask();

            return base.FinishedLaunching(app, options);
        }

        public override void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
    => Shiny.Jobs.JobManager.OnBackgroundFetch(completionHandler);

        //void WireUpLongRunningTask()
        //{
        //    MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", async message =>
        //    {
        //        longRunningTaskExample = new iOSLongRunningTaskExample();
        //        await longRunningTaskExample.Start(message);
        //    });

        //    MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message =>
        //    {
        //        longRunningTaskExample.Stop();
        //    });
        //}
    }

    //public class IOSInitializer : IPlatformInitializer
    //{
    //    public void RegisterTypes(IContainerRegistry containerRegistry)
    //    {
    //    }
    //}
}
