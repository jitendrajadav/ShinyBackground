using Xamarin.Forms;

namespace KegID.PrintTemplates
{
    public class FormatDemoCarousel : CarouselPage
    {
        Format CurrentFormat { get; set; }
        FormatDemoView formatDemoView;
        FormatView formatView;

        public FormatDemoCarousel()
        {
            Title = "Format Demo";
            formatDemoView = new FormatDemoView();
            formatView = new FormatView();

            Children.Add(new ContentPage
            {
                Title = "Select a Format",
                Content = formatDemoView
            }
            );
            Children.Add(new ContentPage
            {
                Title = "Enter Information",
                Content = formatView
            });
            formatDemoView.OnFormatSelected += FormatDemoView_OnFormatSelected;
        }

        private void FormatDemoView_OnFormatSelected(Format fileName)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                formatView.SetFormat(fileName);
                CurrentPage = Children[1];
            });
        }
    }

}
