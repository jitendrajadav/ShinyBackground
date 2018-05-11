using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace KegID.iOS.Services
{
    public class Downloader
    {
        private string targetFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "huge_monkey.png");

        private const string sessionId = "com.xamarin.transfersession";

        private NSUrlSession session;

        readonly string _downloadFileUrl;

        public Downloader(string downloadFileUrl)
        {
            _downloadFileUrl = downloadFileUrl;
        }

        public async Task DownloadFile()
        {
            InitializeSession();

            var pendingTasks = await session.GetTasks2Async();
            if (pendingTasks != null && pendingTasks.DownloadTasks != null)
            {
                foreach (var task in pendingTasks.DownloadTasks)
                {
                    task.Cancel();
                }
            }

            if (File.Exists(targetFilename))
            {
                File.Delete(targetFilename);
            }

            EnqueueDownload();
        }

        void InitializeSession()
        {
            using (var sessionConfig = UIDevice.CurrentDevice.CheckSystemVersion(8, 0)
                ? NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(sessionId)
                : NSUrlSessionConfiguration.BackgroundSessionConfiguration(sessionId))
            {
                sessionConfig.AllowsCellularAccess = true;

                sessionConfig.NetworkServiceType = NSUrlRequestNetworkServiceType.Default;

                sessionConfig.HttpMaximumConnectionsPerHost = 2;

                var sessionDelegate = new CustomSessionDownloadDelegate(targetFilename);
                this.session = NSUrlSession.FromConfiguration(sessionConfig, sessionDelegate, null);
            }
        }

        void EnqueueDownload()
        {
            var downloadTask = this.session.CreateDownloadTask(NSUrl.FromString(_downloadFileUrl));

            if (downloadTask == null)
            {
                new UIAlertView(string.Empty, "Failed to create download task! Please retry.", null, "OK").Show();
                return;
            }

            downloadTask.Resume();
            Console.WriteLine("Starting download. State of task: '{0}'. ID: '{1}'", downloadTask.State, downloadTask.TaskIdentifier);
        }
    }
}