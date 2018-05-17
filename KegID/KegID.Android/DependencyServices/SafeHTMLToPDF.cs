using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using KegID.DependencyServices;
using KegID.Droid.DependencyServices;

[assembly: Dependency(typeof(HTMLToPDF))]
namespace KegID.Droid.DependencyServices
{
    public class HTMLToPDF : IHTMLToPDF
    {
        public string SafeHTMLToPDF(string html, string filename)
        {
            var dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/pay&go/");
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
                webpage = new Android.Webkit.WebView(GetApplicationContext());

            int width = 2102;
            int height = 2973;

            webpage.Layout(0, 0, width, height);
            webpage.LoadDataWithBaseURL("", html, "text/html", "UTF-8", null);
            webpage.SetWebViewClient(new WebViewCallBack(file.ToString()));

            return file.ToString();
        }
    }

    class WebViewCallBack : WebViewClient
    {

        string fileNameWithPath = null;

        public WebViewCallBack(string path)
        {
            this.fileNameWithPath = path;
        }

        public override void OnPageFinished(Android.Webkit.WebView myWebview, string url)
        {
            PdfDocument document = new PdfDocument();
            PdfDocument.Page page = document.StartPage(new PdfDocument.PageInfo.Builder(2120, 3000, 1).Create());

            myWebview.Draw(page.Canvas);
            document.FinishPage(page);
            Stream filestream = new MemoryStream();
            FileOutputStream fos = new Java.IO.FileOutputStream(fileNameWithPath, false); ;
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