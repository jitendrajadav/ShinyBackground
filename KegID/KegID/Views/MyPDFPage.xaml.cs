using KegID.DependencyServices;
using PdfSharp.Xamarin.Forms;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyPDFPage : ContentPage
	{
		public MyPDFPage ()
		{
			InitializeComponent ();
            picker.ItemsSource = new List<string>() { "Item 1", "Item 2", "Item 3" };
            picker.SelectedIndex = 0;
        }

        private void GeneratePDF(object sender, EventArgs e)
        {
            var pdf = PDFManager.GeneratePDFFromView(mainGrid);

            DependencyService.Get<IPdfSave>().Save(pdf, "SinglePage.pdf");
        }
    }
}