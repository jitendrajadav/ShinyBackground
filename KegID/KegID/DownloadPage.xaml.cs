using KegID.Messages;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KegID
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DownloadPage : ContentPage
	{
		public DownloadPage ()
		{
			InitializeComponent ();
            downloadButton.Clicked += Download;

            MessagingCenter.Subscribe<DownloadProgressMessage>(this, "DownloadProgressMessage", message => {
                Device.BeginInvokeOnMainThread(() => {
                    downloadStatus.Text = message.Percentage.ToString("P2");
                });
            });

            MessagingCenter.Subscribe<DownloadFinishedMessage>(this, "DownloadFinishedMessage", message => {
                Device.BeginInvokeOnMainThread(() =>
                {
                    catImage.Source = ImageSource.FromFile(message.FilePath);
                });
            });

        }

        void Download(object sender, EventArgs e)
        {
            catImage.Source = null;
            var message = new DownloadMessage
            {
                Url = "http://xamarinuniversity.blob.core.windows.net/ios210/huge_monkey.png"
            };

            MessagingCenter.Send(message, "Download");
        }

    }
}