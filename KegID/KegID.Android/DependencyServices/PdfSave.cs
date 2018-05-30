﻿using KegID.DependencyServices;
using KegID.Droid.DependencyServices;
using PdfSharpCore.Pdf;

[assembly: Xamarin.Forms.Dependency(typeof(PdfSave))]
namespace KegID.Droid.DependencyServices
{
    public class PdfSave : IPdfSave
    {
        public void Save(PdfDocument doc, string fileName)
        {
            string path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + fileName);

            doc.Save(path);
            doc.Close();

            global::Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
                title: "Success",
                message: $"Your PDF generated and saved @ {path}",
                cancel: "OK");
        }
    }

}