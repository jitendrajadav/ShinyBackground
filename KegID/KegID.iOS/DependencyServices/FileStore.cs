using CoreGraphics;
using Foundation;
using KegID.DependencyServices;
using System;
using System.IO;
using UIKit;

namespace KegID.iOS.DependencyServices
{
    public class FileStore : IFileStore
    {
        public string GetFilePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        /// <summary>
        /// Writes the file to local storage.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="bytes">Bytes.</param>
        public string WriteFile(string fileName, byte[] bytes)
        {
            string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string localPath = System.IO.Path.Combine(localFolder, fileName);
            File.WriteAllBytes(localPath, bytes); // write to local storage

            return localPath;
        }

        public string SafeHTMLToPDF(string html, string filename)
        {
            UIWebView webView = new UIWebView(new CGRect(0, 0, 6.5 * 72, 9 * 72));

            //var file = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //file = Path.Combine(file, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            //if (!Directory.Exists(file.ToString()))
            //{
            //    Directory.CreateDirectory(file);
            //}
            //file = Path.Combine(file, filename + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf");

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var file = Path.Combine(documentsPath, filename + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf");
            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var documents = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].ToString();
            //var documents = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0].Path;
            //var file = Path.Combine(documents, "Invoice" + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString() + ".pdf");

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