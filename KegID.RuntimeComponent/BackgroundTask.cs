using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace KegID.RuntimeComponent
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background Task Running");

            _deferral = taskInstance.GetDeferral();
            try
            {
                var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

                settings.Values.Add("BackgroundTask", "Hello from UWP");
            }
            catch { }
            _deferral.Complete();
        }
    }
}
