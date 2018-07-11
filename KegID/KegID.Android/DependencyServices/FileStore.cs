//using System.IO;
//using Android.Graphics.Pdf;
//using Android.Webkit;
//using Java.IO;
//using KegID.DependencyServices;
//using Xamarin.Forms;

//namespace KegID.Droid.DependencyServices
//{
//    public class FileStore : IFileStore
//    {
//        public string GetFilePath()
//        {
//            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "image.png");
//        }

//        /// <summary>
//        /// Writes the file to local storage.
//        /// </summary>
//        /// <returns>The file.</returns>
//        /// <param name="fileName">File name.</param>
//        /// <param name="bytes">Bytes.</param>
//        public string WriteFile(string fileName, byte[] bytes)
//        {
//            //var localFolder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

//            //string localPath = Path.Combine(localFolder, fileName);
//            //File.WriteAllBytes(localPath, bytes); // write to local storage

//            //return string.Format("file://{0}/{1}", localFolder, fileName);

//            var directory = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
//            directory = Path.Combine(directory, Android.OS.Environment.DirectoryDownloads);
//            string file = Path.Combine(directory.ToString(), fileName);
//            System.IO.File.WriteAllBytes(file, bytes);
//            return file;//string.Format("file://{0}/{1}", directory, fileName);
//        }


//        public string SafeHTMLToPDF(string html, string filename)
//        {
//            Android.Webkit.WebView webpage = null;
//            var dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/pay&go/");
//            var file = new Java.IO.File(dir + "/" + filename + ".pdf");

//            if (!dir.Exists())
//                dir.Mkdirs();


//            int x = 0;
//            while (file.Exists())
//            {
//                x++;
//                file = new Java.IO.File(dir + "/" + filename + "( " + x + " )" + ".pdf");
//            }

//            if (webpage == null)
//                webpage = new Android.Webkit.WebView(Forms.Context);

//            int width = 2102;
//            int height = 2973;

//            webpage.Layout(0, 0, width, height);
//            webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);
//            webpage.SetWebViewClient(new WebViewCallBack(file.ToString()));

//            return file.ToString();
//        }

//        class WebViewCallBack : WebViewClient
//        {

//            string fileNameWithPath = null;

//            public WebViewCallBack(string path)
//            {
//                fileNameWithPath = path;
//            }

//            public override void OnPageFinished(Android.Webkit.WebView myWebview, string url)
//            {
//                PdfDocument document = new PdfDocument();
//                PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(2120, 3000, 1).Create());

//                myWebview.Draw(page.Canvas);
//                document.FinishPage(page);
//                Stream filestream = new MemoryStream();
//                FileOutputStream fos = new FileOutputStream(fileNameWithPath, false); ;
//                try
//                {
//                    document.WriteTo(filestream);
//                    fos.Write(((MemoryStream)filestream).ToArray(), 0, (int)filestream.Length);
//                    fos.Close();
//                }
//                catch
//                {
//                }
//            }
//        }

//    }
//}