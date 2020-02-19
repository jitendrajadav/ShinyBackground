using Android.App;
using Android.Content;
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Plugin.CurrentActivity;
using KegID.Droid.DependencyServices;
using KegID.DependencyServices;
using Android.Graphics.Pdf;

[assembly: Xamarin.Forms.Dependency(typeof(ShareFileImplementation))]
namespace KegID.Droid.DependencyServices
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class ShareFileImplementation : IShareFile
    {
        public string SafeHTMLToPDF(string html, string filename,int flag)
        {
            int width = 0;
            int height = 0;

            global::Android.Webkit.WebView webpage = null;
            //var dir = new Java.IO.File(global::Android.OS.Environment.DirectoryDocuments + "/KegIdFiles/");
            var dir = new Java.IO.File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(Environment.CurrentDirectory) + "/KegIdFiles/");

            var file = new Java.IO.File(dir + "/" + filename + ".pdf");

            if (!dir.Exists())
                dir.Mkdirs();

            int x = 0;
            while (file.Exists())
            {
                x++;
                file = new Java.IO.File(dir + "/" + filename + "( " + x + " ).pdf");
            }

            if (webpage == null)
            {
                webpage = new global::Android.Webkit.WebView(Application.Context);
            }

            if (flag == 0)
            {
                width = 2959;
                height = 3873;
            }
            else
            {
                width = 3659;
                height = 4573;
            }
            webpage.Layout(0, 0, width, height);
            webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);
            webpage.SetWebViewClient(new WebViewCallBack(file.ToString(),flag));

            return file.ToString();
        }

        /// <summary>
        /// Simply share a local file on compatible services
        /// </summary>
        /// <param name="localFilePath">path to local file</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable Task</returns>
        public void ShareLocalFile(string localFilePath, string title = "", object view = null)
        {
            //Approch 2 for updated sharing ption above +24 API
           
                if (string.IsNullOrWhiteSpace(localFilePath))
                {
                    return;
                }

                global::Android.Net.Uri fileUri = FileProvider.GetUriForFile(Application.Context, $"{Application.Context.PackageName}.provider", new Java.IO.File(localFilePath));

                var builder =
                    ShareCompat.IntentBuilder.From(CrossCurrentActivity.Current.Activity).SetType(CrossCurrentActivity.Current.Activity.ContentResolver.GetType(fileUri)).SetText(title).AddStream(fileUri);
                var chooserIntent = builder.CreateChooserIntent();
                chooserIntent.SetFlags(ActivityFlags.ClearTop);
                chooserIntent.SetFlags(ActivityFlags.NewTask);
                chooserIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                CrossCurrentActivity.Current.Activity.StartActivity(chooserIntent);
        }

        /// <summary>
        /// Simply share a file from a remote resource on compatible services
        /// </summary>
        /// <param name="fileUri">uri to external file</param>
        /// <param name="fileName">name of the file</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable bool</returns>
        public async Task ShareRemoteFile(string fileUri, string fileName, string title = "", object view = null)
        {
            using var webClient = new WebClient();
            var uri = new System.Uri(fileUri);
            var bytes = await webClient.DownloadDataTaskAsync(uri);
            var filePath = WriteFile(fileName, bytes);
            ShareLocalFile(filePath, title);
        }

        /// <summary>
        /// Writes the file to local storage.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="bytes">Bytes.</param>
        public string WriteFile(string fileName, byte[] bytes)
        {
            string localPath = "";
                var localFolder = global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                localPath = Path.Combine(localFolder, fileName);
                File.WriteAllBytes(localPath, bytes); // write to local storage
                return localPath;
        }
    }

    public class WebViewCallBack : global::Android.Webkit.WebViewClient
    {
        readonly int width = 0;
        readonly int height = 0;

        readonly string fileNameWithPath = null;

        public WebViewCallBack(string path,int flag)
        {
            fileNameWithPath = path;
            if (flag == 0)
            {
                width = 2959;
                height = 3900;
            }
            else
            {
                width = 3659;
                height = 4600;
            }
        }

        public override void OnPageFinished(global::Android.Webkit.WebView myWebview, string url)
        {
            PdfDocument document = new PdfDocument();
            PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(width, height, 1).Create());

            myWebview.Draw(page.Canvas);
            document.FinishPage(page);
            Stream filestream = new MemoryStream();
            
                Java.IO.FileOutputStream fos = new Java.IO.FileOutputStream(fileNameWithPath, false);
                document.WriteTo(filestream);
                fos.Write(((MemoryStream)filestream).ToArray(), 0, (int)filestream.Length);
                fos.Close();
        }
    }
}