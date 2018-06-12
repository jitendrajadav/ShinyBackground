using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using CoreGraphics;
using KegID.iOS.DependencyServices;
using KegID.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(ShareFileImplementation))]
namespace KegID.iOS.DependencyServices
{
    /// <summary>
    /// Implementation for ShareFile
    /// </summary>
    public class ShareFileImplementation : IShareFile
    {
        public void ShareLocalFile(string localFilePath, string title = "", object view = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(localFilePath))
                {
                    Console.WriteLine("Plugin.ShareFile: ShareLocalFile Warning: localFilePath null or empty");
                    return;
                }

                var rootController = GetVisibleViewController();// UIApplication.SharedApplication.KeyWindow.RootViewController;
                var sharedItems = new List<NSObject>();
                var fileName = Path.GetFileName(localFilePath);

                var fileUrl = NSUrl.FromFilename(localFilePath);
                sharedItems.Add(fileUrl);

                UIActivity[] applicationActivities = null;
                var activityViewController = new UIActivityViewController(sharedItems.ToArray(), applicationActivities);

                // Subject
                if (!string.IsNullOrWhiteSpace(title))
                    activityViewController.SetValueForKey(NSObject.FromObject(title), new NSString("subject"));

                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
                {
                    rootController.PresentViewController(activityViewController, true, null);
                }
                else
                {
                    var shareView = view as UIView;
                    if (shareView != null)
                    {
                        UIPopoverController popCont = new UIPopoverController(activityViewController);
                        popCont.PresentFromRect(shareView.Frame, shareView, UIPopoverArrowDirection.Any, true);
                    }
                    else
                    {
                        throw new Exception("view is null: for iPad you must pass the view paramater. The view parameter should be the view that triggers the share action, i.e. the share button.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareLocalFile Exception: {0}", ex);
            }
        }

        UIViewController GetVisibleViewController(UIViewController controller = null)
        {
            controller = controller ?? UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (controller.PresentedViewController == null)
                return controller;

            if (controller.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)controller.PresentedViewController).VisibleViewController;
            }

            if (controller.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)controller.PresentedViewController).SelectedViewController;
            }

            return GetVisibleViewController(controller.PresentedViewController);
        }

        /// <summary>
        /// Share a file from a remote resource on compatible services
        /// </summary>
        /// <param name="fileUri">uri to external file</param>
        /// <param name="fileName">name of the file</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable bool</returns>
        public async Task ShareRemoteFile(string fileUri, string fileName, string title = "", object view = null)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var uri = new System.Uri(fileUri);
                    var bytes = await webClient.DownloadDataTaskAsync(uri);
                    var filePath = WriteFile(fileName, bytes);
                    ShareLocalFile(filePath, title, view);
                }
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the file to local storage.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="bytes">Bytes.</param>
        private string WriteFile(string fileName, byte[] bytes)
        {
            string localPath = "";

            try
            {
                string localFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                localPath = System.IO.Path.Combine(localFolder, fileName);
                System.IO.File.WriteAllBytes(localPath, bytes); // write to local storage
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex);
            }

            return localPath;
        }

        public string SafeHTMLToPDF(string html, string filename)
        {
            UIWebView webView = new UIWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72));

            //var file = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //file = Path.Combine(file, DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff"));
            //if (!Directory.Exists(file.ToString()))
            //{
            //    Directory.CreateDirectory(file);
            //}
            //file = Path.Combine(file, filename + DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + ".pdf");

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var file = Path.Combine(documentsPath, filename + DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff") + ".pdf");
            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var documents = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].ToString();
            //var documents = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path;
            //var file = Path.Combine(documents, "Invoice" + "_" + DateTimeOffset.Now.ToShortDateString() + "_" + DateTimeOffset.Now.ToShortTimeString() + ".pdf");

            webView.Delegate = new WebViewCallBack(file);
            webView.ScalesPageToFit = true;
            webView.UserInteractionEnabled = false;
            webView.BackgroundColor = UIColor.White;
            webView.LoadHtmlString(html, null);

            return file;
        }

        class WebViewCallBack : UIWebViewDelegate
        {
            string filename = null;
            public WebViewCallBack(string path)
            {
                filename = path;
            }

            public override void LoadingFinished(UIWebView webView)
            {
                try
                {
                    double height, width;
                    int header, sidespace;

                    width = 595.2;
                    height = 841.8;
                    header = 10;
                    sidespace = 10;


                    UIEdgeInsets pageMargins = new UIEdgeInsets(header, sidespace, header, sidespace);
                    webView.ViewPrintFormatter.ContentInsets = pageMargins;

                    UIPrintPageRenderer renderer = new UIPrintPageRenderer();
                    renderer.AddPrintFormatter(webView.ViewPrintFormatter, 0);

                    CGSize pageSize = new CGSize(width, height);
                    CGRect printableRect = new CGRect(sidespace,
                                      header,
                                      pageSize.Width - (sidespace * 2),
                                      pageSize.Height - (header * 2));
                    CGRect paperRect = new CGRect(0, 0, width, height);
                    renderer.SetValueForKey(FromObject(paperRect), (NSString)"paperRect");
                    renderer.SetValueForKey(FromObject(printableRect), (NSString)"printableRect");
                    NSData file = PrintToPDFWithRenderer(renderer, paperRect);
                    File.WriteAllBytes(filename, file.ToArray());
                }
                catch (Exception ex)
                {

                }
            }

            private NSData PrintToPDFWithRenderer(UIPrintPageRenderer renderer, CGRect paperRect)
            {
                NSMutableData pdfData = new NSMutableData();
                UIGraphics.BeginPDFContext(pdfData, paperRect, null);

                renderer.PrepareForDrawingPages(new NSRange(0, renderer.NumberOfPages));

                CGRect bounds = UIGraphics.PDFContextBounds;

                for (int i = 0; i < renderer.NumberOfPages; i++)
                {
                    UIGraphics.BeginPDFPage();
                    renderer.DrawPage(i, paperRect);
                }
                UIGraphics.EndPDFContent();

                return pdfData;
            }
        }

    }

}