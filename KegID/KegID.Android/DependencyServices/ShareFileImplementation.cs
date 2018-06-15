﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics.Pdf;
using Android.Webkit;
using Java.IO;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareFileImplementation))]
namespace KegID.Droid.DependencyServices
{
    /// <summary>
    /// Implementation for Feature
    /// </summary> 
    public class ShareFileImplementation : IShareFile
    {
        public string SafeHTMLToPDF(string html, string filename)
        {
            Android.Webkit.WebView webpage = null;
            var dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/KegIdFiles/");
            var file = new Java.IO.File(dir + "/" + filename + ".pdf");

            if (!dir.Exists())
                dir.Mkdirs();

            int x = 0;
            while (file.Exists())
            {
                x++;
                file = new Java.IO.File(dir + "/" + filename + "( " + x + " )" + ".pdf");
            }

            if (webpage == null)
                webpage = new Android.Webkit.WebView(Forms.Context);

            int width = 2959;
            int height = 3873;

            webpage.Layout(0, 0, width, height);
            webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);
            webpage.SetWebViewClient(new WebViewCallBack(file.ToString()));

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
            try
            {

                if (string.IsNullOrWhiteSpace(localFilePath))
                {
                    System.Console.WriteLine("Plugin.ShareFile: ShareLocalFile Warning: localFilePath null or empty");
                    return;
                }

                if (!localFilePath.StartsWith("file://"))
                    localFilePath = string.Format("file://{0}", localFilePath);

                var fileUri = Android.Net.Uri.Parse(localFilePath);

                var intent = new Intent();
                intent.SetFlags(ActivityFlags.ClearTop);
                intent.SetFlags(ActivityFlags.NewTask);
                intent.SetAction(Intent.ActionSend);
                intent.SetType("*/*");
                intent.PutExtra(Intent.ExtraStream, fileUri);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                var chooserIntent = Intent.CreateChooser(intent, title);
                chooserIntent.SetFlags(ActivityFlags.ClearTop);
                chooserIntent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(chooserIntent);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    System.Console.WriteLine("Exception in Plugin.ShareFile: ShareLocalFile Exception: {0}", ex);
            }
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
            try
            {
                using (var webClient = new WebClient())
                {
                    var uri = new System.Uri(fileUri);
                    var bytes = await webClient.DownloadDataTaskAsync(uri);
                    var filePath = WriteFile(fileName, bytes);
                    ShareLocalFile(filePath, title);
                    //return true;
                }
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    System.Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex.Message);
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
                var localFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                localPath = System.IO.Path.Combine(localFolder, fileName);
                System.IO.File.WriteAllBytes(localPath, bytes); // write to local storage

                return string.Format("file://{0}/{1}", localFolder, fileName);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    System.Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex);
            }

            return localPath;
        }
    }

    class WebViewCallBack : WebViewClient
    {
        string fileNameWithPath = null;

        public WebViewCallBack(string path)
        {
            fileNameWithPath = path;
        }

        public override void OnPageFinished(Android.Webkit.WebView myWebview, string url)
        {
            PdfDocument document = new PdfDocument();
            PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(2959, 3900, 1).Create());

            myWebview.Draw(page.Canvas);
            document.FinishPage(page);
            Stream filestream = new MemoryStream();
            FileOutputStream fos = new FileOutputStream(fileNameWithPath, false);
            try
            {
                document.WriteTo(filestream);
                fos.Write(((MemoryStream)filestream).ToArray(), 0, (int)filestream.Length);
                fos.Close();
            }
            catch
            {
            }
        }
    }

}