//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using Foundation;
//using KegID.DependencyServices;
//using KegID.iOS.DependencyServices;
//using UIKit;

//[assembly: Xamarin.Forms.Dependency(typeof(Share))]
//namespace KegID.iOS.DependencyServices
//{
//    public class Share : IShare
//    {
//        /// <summary>
//        /// MUST BE CALLED FROM THE UI THREAD
//        /// </summary>
//        /// <param name="filePath"></param>
//        /// <param name="title"></param>
//        /// <param name="message"></param>
//        /// <returns></returns>
//        public async Task Show(string title, string message, string filePath)
//        {
//            var items = new NSObject[] { NSObject.FromObject(title), NSUrl.FromFilename(filePath) };
//            var activityController = new UIActivityViewController(items, null);
//            var vc = GetVisibleViewController();

//            NSString[] excludedActivityTypes = null;

//            if (excludedActivityTypes != null && excludedActivityTypes.Length > 0)
//                activityController.ExcludedActivityTypes = excludedActivityTypes;

//            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
//            {
//                if (activityController.PopoverPresentationController != null)
//                {
//                    activityController.PopoverPresentationController.SourceView = vc.View;
//                }
//            }

//            await vc.PresentViewControllerAsync(activityController, true);

//        }

//        UIViewController GetVisibleViewController()
//        {
//            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

//            if (rootController.PresentedViewController == null)
//                return rootController;

//            if (rootController.PresentedViewController is UINavigationController)
//            {
//                return ((UINavigationController)rootController.PresentedViewController).TopViewController;
//            }

//            if (rootController.PresentedViewController is UITabBarController)
//            {
//                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
//            }

//            return rootController.PresentedViewController;
//        }


//        public void ShareLocalFile(string localFilePath, string title = "", object view = null)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(localFilePath))
//                {
//                    Console.WriteLine("Plugin.ShareFile: ShareLocalFile Warning: localFilePath null or empty");
//                    return;
//                }

//                var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;
//                var sharedItems = new List<NSObject>();
//                var fileName = Path.GetFileName(localFilePath);

//                var fileUrl = NSUrl.FromFilename(localFilePath);
//                sharedItems.Add(fileUrl);

//                UIActivity[] applicationActivities = null;
//                var activityViewController = new UIActivityViewController(sharedItems.ToArray(), applicationActivities);

//                // Subject
//                if (!string.IsNullOrWhiteSpace(title))
//                    activityViewController.SetValueForKey(NSObject.FromObject(title), new NSString("subject"));

//                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
//                {
//                    rootController.PresentViewController(activityViewController, true, null);
//                }
//                else
//                {
//                    var shareView = view as UIView;
//                    if (shareView != null)
//                    {
//                        UIPopoverController popCont = new UIPopoverController(activityViewController);
//                        popCont.PresentFromRect(shareView.Frame, shareView, UIPopoverArrowDirection.Any, true);
//                    }
//                    else
//                    {
//                        throw new Exception("view is null: for iPad you must pass the view paramater. The view parameter should be the view that triggers the share action, i.e. the share button.");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
//                    Console.WriteLine("Exception in Plugin.ShareFile: ShareLocalFile Exception: {0}", ex);
//            }
//        }

//    }
//}