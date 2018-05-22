//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xamarin.Forms;

//namespace KegID.PrintTemplates
//{
//    public class FormatDemoCarousel : CarouselPage
//    {
//        Format CurrentFormat { get; set; }
//        FormatDemoView formatDemoView;
//        FormatView formatView;

//        public FormatDemoCarousel()
//        {
//            this.Title = "Format Demo";
//            formatDemoView = new FormatDemoView();
//            formatView = new FormatView();

//            this.Children.Add(new ContentPage
//            {
//                Title = "Select a Format",
//                Content = formatDemoView
//            }
//            );
//            this.Children.Add(new ContentPage
//            {
//                Title = "Enter Information",
//                Content = formatView
//            });
//            formatDemoView.OnFormatSelected += FormatDemoView_OnFormatSelected;
//        }

//        private void FormatDemoView_OnFormatSelected(Format fileName)
//        {
//            Device.BeginInvokeOnMainThread(() =>
//            {
//                formatView.SetFormat(fileName);
//                this.CurrentPage = this.Children[1];
//            });
//        }
//    }

//}
